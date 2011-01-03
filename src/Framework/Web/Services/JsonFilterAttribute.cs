using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;

namespace InfoControl.Web.Services
{
    public class JavaScriptSerializerAttribute : ActionFilterAttribute, IOperationBehavior//, IDispatchMessageInspector
    {
        #region Controller > ActionFilterAttribute

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //
            // Get a shortcut to context objects
            //
            var request = filterContext.HttpContext.Request;
            var server = filterContext.HttpContext.Server;
            string contentType = request.ContentType;

            if (string.IsNullOrEmpty(contentType)) return;

            if (!contentType.Contains("application/json")) return;

            string paramValue = request.QueryString.ToString();

            if (request.RequestType == "GET")
                paramValue = server.UrlDecode(paramValue);
            else
                using (var reader = new StreamReader(request.InputStream))
                    paramValue = reader.ReadToEnd();

            var serializer = new JavaScriptSerializer();
            var rawResult = (IDictionary<string, object>)serializer.DeserializeObject(paramValue);

            foreach (var item in filterContext.ActionDescriptor.GetParameters())
            {
                var deserializeMethod = serializer.GetType()
                .GetMethods().First(m => m.Name == "ConvertToType")
                .MakeGenericMethod(item.ParameterType);

                filterContext.ActionParameters[item.ParameterName] = deserializeMethod.Invoke(serializer, new[] { rawResult[item.ParameterName] });
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        #endregion

        //
        // Baseado nos atributos disponiveis em 
        // http://www.israelaece.com/post/WCF-Internals-e-Extensibilidade.aspx
        //
        #region WCF Service > IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            //throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription,
                                            System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            var SerializerBehavior = operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();

            dispatchOperation.Formatter = new CustomFormatter(dispatchOperation.Formatter, operationDescription);
        }

        public void Validate(OperationDescription operationDescription)
        {
            //throw new NotImplementedException();
        }


        private class CustomFormatter : IDispatchMessageFormatter
        {

            IDispatchMessageFormatter original;
            OperationDescription operationDescription;

            public CustomFormatter(IDispatchMessageFormatter original, OperationDescription operationDescription)
            {
                this.original = original;
                this.operationDescription = operationDescription;
            }



            #region IDispatchMessageFormatter Members

            public void DeserializeRequest(Message message, object[] parameters)
            {
                //
                // Get a shortcut to context objects
                //
                //var request = HttpContext.Current.Request;
                //var server = HttpContext.Current.Server;

                var doc = new XmlDocument();
                doc.Load(message.GetReaderAtBodyContents());

                // Get parameters details
                var inputMsg = operationDescription.Messages.First(md => md.Direction == MessageDirection.Input);

                for (int i = 0; i < inputMsg.Body.Parts.Count; i++)
                {
                    var part = inputMsg.Body.Parts[i];
                    var paramNode = doc.FirstChild.ChildNodes.Cast<XmlNode>()
                                                  .Where(n => n.Name.ToLower() == part.Name.ToLower())
                                                  .FirstOrDefault();
                    if (paramNode != null)
                        parameters[i] = Deserialize(part.Type, paramNode);
                }

            }

            private object Deserialize(Type type, XmlNode node)
            {
                string rawObject = node.Value ?? node.InnerXml;
                var attr = node.Attributes["type"];


                //
                // Convert CLI Native Types
                //
                if (type == null)
                {
                    Double resultDouble = default(Double);
                    DateTime resultDateTime = default(DateTime);

                    if (attr != null)
                    {
                        if (attr.Value == "number")
                            if (Double.TryParse(rawObject, out resultDouble)) return resultDouble;

                        if (attr.Value == "string")
                            if (DateTime.TryParse(rawObject, out resultDateTime)) return resultDateTime;

                        return rawObject;
                    }
                }

                if (attr == null)
                    return Convert.ChangeType(rawObject, type);

                //
                // Tipos nullable
                //
                if (type.IsGenericType)
                {
                    if (!String.IsNullOrEmpty(rawObject))
                        return Convert.ChangeType(rawObject, type.GetGenericArguments()[0]);
                    else
                        return null;
                }


                if (attr.Value != "object")
                    return Convert.ChangeType(rawObject, type);
                

                //
                // Convert complex types
                //
                var obj = Activator.CreateInstance(type);

                foreach (var n in node.ChildNodes.Cast<XmlNode>())
                    if (n.Attributes["type"].Value != "object")
                        obj.SetPropertyValue(n.Name, Deserialize(null, n));

                return obj;
            }





            /// <summary>
            /// 
            /// </summary>
            /// <param name="messageVersion"></param>
            /// <param name="parameters"></param>
            /// <param name="result"></param>
            /// <returns></returns>
            public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
            {
                //
                // Pega os tipos de retorno aceitos pelo navegador
                //
                Message request = OperationContext.Current.RequestContext.RequestMessage;
                var prop = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                string accepts = prop.Headers[HttpRequestHeader.Accept];

                if (accepts.Contains("text/xml") || accepts.Contains("application/xml"))
                {
                    return null;
                }


                //
                // Referencias 
                // http://www.israelaece.com/post/Por-dentro-da-classe-Message.aspx
                // http://www.codeproject.com/Articles/34632/How-to-pass-arbitrary-data-in-a-Message-object-usi.aspx
                // 

                var message = Message.CreateMessage(MessageVersion.None,
                                                    null,
                                                    new TextBodyWriter(result.SerializeToJson()));

                //
                // Raw significa que haverá serialização
                //
                message.Properties[WebBodyFormatMessageProperty.Name] = new WebBodyFormatMessageProperty(WebContentFormat.Raw);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

                return message;
            }



            #endregion


        }

        #endregion


    }

    /// <summary>
    /// Necessary to write out the contents as text (used with the Raw return type)
    /// </summary>
    public class TextBodyWriter : BodyWriter
    {
        byte[] messageBytes;

        public TextBodyWriter(string message)
            : base(true)
        {
            this.messageBytes = Encoding.UTF8.GetBytes(message);
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("Binary");
            writer.WriteBase64(this.messageBytes, 0, this.messageBytes.Length);
            writer.WriteEndElement();
        }
    }


}
