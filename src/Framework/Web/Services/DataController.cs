using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using InfoControl.Data;
using InfoControl.Web.Auditing;
using InfoControl.Web.Configuration;
using InfoControl.Web.Security;

namespace InfoControl.Web.Services
{
    /// <summary>
    /// Represents a InfoControl.Web.UI.Page with capabilities of the data access and maintenance of transactions in context, through of a DataManager
    /// </summary>
    public class DataController : Controller, IDataAccessor
    {
        internal Application _application;

        private DataManager _DataManager;

        public DataController()
        {
            _application = new Application();
            if (MaintenanceModule.Enabled)
            {
                _application = Application.Current;
            }
        }

        public new HttpContext HttpContext
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        ///  Get the information about the current application
        /// </summary>
        public Application Application
        {
            get { return Application.Current; }
        }

        /// <summary>
        ///  Get the information about the current application
        /// </summary>
        public new HttpSessionState Session
        {
            get { return HttpContext.Session; }
        }


        /// <summary>
        /// Get the application state for the current request
        /// </summary>
        public HttpApplicationState ApplicationState
        {
            get { return HttpContext.Application; }
        }

        /// <summary>
        /// Get information about the user making the page request
        /// </summary>
        public new AccessControlPrincipal User
        {
            get { return HttpContext.User as AccessControlPrincipal; }
        }

        #region IDataAccessor Members

        /// <summary>
        /// Provides helper functions that implements data access
        /// </summary>
        public DataManager DataManager
        {
            get { return _DataManager ?? (_DataManager = new DataManager()); }
        }

        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            try
            {
                Trace.TraceWarning("Controller.OnActionExecuted");
                DataManager.Commit();
            }
            catch (Exception ex)
            {
                HttpContext.AddError(ex);
            }
            
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            try
            {
                Trace.TraceWarning("Controller.OnException");
                DataManager.Rollback();
            }
            catch (Exception ex)
            {
                HttpContext.AddError(ex);
            }
        }

        #region Actions
        
       

        public JsonResult ClientResponse(Func<object> func)
        {
            return ClientResponse(func, null, null, JsonRequestBehavior.DenyGet);
        }

        public JsonResult ClientResponse(Func<object> func, JsonRequestBehavior behavior)
        {
            return ClientResponse(func, null, null, behavior);
        }

        public JsonResult ClientResponse(Func<object> func, string contentType)
        {
            return ClientResponse(func, contentType, null, JsonRequestBehavior.DenyGet);
        }

        public JsonResult ClientResponse(Func<object> func, string contentType, JsonRequestBehavior behavior)
        {
            return ClientResponse(func, contentType, null, behavior);
        }

        public JsonResult ClientResponse(Func<object> func, string contentType, Encoding encoding)
        {
            return ClientResponse(func, contentType, encoding, JsonRequestBehavior.DenyGet);
        }

        public JsonResult ClientResponse(Func<object> func, string contentType, Encoding encoding,
                                         JsonRequestBehavior behavior)
        {
            try
            {
                var c = new ClientResponse {Data = func()};
                return Json(c, contentType, encoding, behavior);
            }
            catch (Exception ex)
            {
                return Json(new ClientResponse {Errors = ex.Message});
            }
        }

        #endregion
        [JsonFilter]
        public ActionResult GetSession(string key)
        {
            return ClientResponse(() => Session[key], JsonRequestBehavior.AllowGet);
        }
    }
}