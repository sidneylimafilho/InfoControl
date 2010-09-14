using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using InfoControl.Data;
using InfoControl.Web.Security;
using InfoControl.Web.Security.DataEntities;
using InfoControl.Properties;

namespace InfoControl.Web
{
    public abstract class HttpDataModule : DataAccessor, IHttpModule
    {
        #region IHttpModule Members
        public HttpContext Context { get { return HttpContext.Current; } }

        public virtual void Dispose()
        {
            DataManager.Dispose();
        }
        public virtual void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            Dispose();
        }

        public virtual bool IsDotNetRequest
        {
            get
            {
                if (Context == null || Context.User == null)
                    return false;

                //string loginPage = System.Web.Security.FormsAuthentication.LoginUrl;

                //if (Context.Request.CurrentExecutionFilePath == loginPage)
                //    return false;

                if (Context.Request.PhysicalPath.ToLower().Contains("app_offline.aspx"))
                    return false;

                if (Context.Request.PhysicalPath.ToLower().EndsWith(".aspx"))
                    return true;

                if (Context.Request.PhysicalPath.ToLower().EndsWith("\\"))
                    return true;

                if (Context.Request.PhysicalPath.ToLower().EndsWith(".asmx"))
                    return true;

                if (Context.Request.PhysicalPath.ToLower().EndsWith(".asbx"))
                    return true;

                if (Context.Request.PhysicalPath.ToLower().EndsWith(".svc"))
                    return true;

                if (Context.Request.PhysicalPath.ToLower().EndsWith(".ashx"))
                    return true;

                if (Context.Handler is System.Web.Mvc.MvcHandler)
                    return true;

                return false;
            }
        }

        public virtual bool IsUserLogged
        {
            get
            {
                if (Context == null || Context.User == null)
                    return false;

                if (String.IsNullOrEmpty(Context.User.Identity.Name))
                    return false;

                return Context.User.Identity.IsAuthenticated;
            }
        }
        #endregion
    }
}
