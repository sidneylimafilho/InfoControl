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

namespace InfoControl.Web.Services
{
    public class JsonFilterAttribute : ActionFilterAttribute, IOperationBehavior//, IDispatchMessageInspector
    {
        #region ActionFilterAttribute

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

        #region IOperationBehavior Members

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
            dispatchOperation.Formatter = new JsonFormatter(dispatchOperation.Formatter, operationDescription);
        }

        public void Validate(OperationDescription operationDescription)
        {
            //throw new NotImplementedException();
        }

        #endregion


    }

    //public class JsonParameterInspector : IParameterInspector
    //{
    //    #region IParameterInspector Members

    //    public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object BeforeCall(string operationName, object[] inputs)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}

    //public class JsonFormatterBehaviorAttribute : Attribute, IEndpointBehavior
    //{
    //    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

    //    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    //    {
    //        foreach (ClientOperation operation in clientRuntime.Operations)
    //            operation.Formatter = new JsonFormatter();
    //    }

    //    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    //    {
    //        foreach (DispatchOperation operation in endpointDispatcher.DispatchRuntime.Operations)
    //            operation.Formatter = new JsonFormatter();
    //    }

    //    public void Validate(ServiceEndpoint endpoint) { }
    //}

    public class JsonFormatter : IDispatchMessageFormatter, IClientMessageFormatter
    {

        IDispatchMessageFormatter original;
        OperationDescription operationDescription;

        public JsonFormatter(IDispatchMessageFormatter original, OperationDescription operationDescription)
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
            var request = HttpContext.Current.Request;
            var server = HttpContext.Current.Server;

            var doc = new XmlDocument();
            doc.Load(message.GetReaderAtBodyContents());

            // Get parameters details
            var inputMsg = operationDescription.Messages.First(md => md.Direction == MessageDirection.Input);

            for (int i = 0; i < inputMsg.Body.Parts.Count; i++)
            {
                var type = inputMsg.Body.Parts[i].Type;
                var paramNode = doc.ChildNodes[0].ChildNodes[i];

                parameters[i] = Deserialize(type, paramNode);
            }

        }

        private object Deserialize(Type type, XmlNode node)
        {
            var obj = Activator.CreateInstance(type);

            foreach (var n in node.ChildNodes.Cast<XmlNode>())
                if (!n.ChildNodes.Cast<XmlNode>().Any(it => it.NodeType == XmlNodeType.Element))
                    obj.SetPropertyValue(n.Name, TryGetValue(n));
                else
                    obj.SetPropertyValue(n.Name, Deserialize(type, n));

            return obj;
        }

        private object TryGetValue(XmlNode node)
        {
            Double resultDouble = default(Double);
            DateTime resultDateTime = default(DateTime);
            string possibleValue = node.Value ?? node.InnerText;

            var attr = node.Attributes["type"];
            if (attr != null)
            {
                if (attr.Value == "number")
                    if (Double.TryParse(possibleValue, out resultDouble)) return resultDouble;

                if (attr.Value == "string")
                    if (DateTime.TryParse(possibleValue, out resultDateTime)) return resultDateTime;

                return possibleValue;
            }
            return null;
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            return original.SerializeReply(messageVersion, parameters, result);
        }



        #endregion

        #region IClientMessageFormatter Members

        public object DeserializeReply(Message message, object[] parameters)
        {
            //Tracing.TraceSource.TraceInformation("ClientMessageFormatter.DeserializeReply");
            //Tracing.TraceSource.Flush();
            return null;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            //Tracing.TraceSource.TraceInformation("ClientMessageFormatter.SerializeRequest");
            //Tracing.TraceSource.Flush();
            return null;
        }

        #endregion
    }



}
