using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;

namespace InfoControl.Web.Services
{
    public class JsonFilterAttribute : ActionFilterAttribute
    {
       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
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
    }
}
