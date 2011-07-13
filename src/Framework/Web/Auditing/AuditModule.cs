using System;
using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Text;

using InfoControl.Web;
using InfoControl.Properties;
using InfoControl.Data;

using InfoControl.Web.Security;
using InfoControl.Security;

namespace InfoControl.Web.Auditing
{
    public class AuditModule : HttpDataModule
    {
        private static bool _isAuditing = false;

        #region IHttpModule Members

        public override void Dispose()
        {

        }

        public override void Init(HttpApplication context)
        {

            //
            // Users Online
            //
            SessionStateModule sessionModule = context.Modules["Session"] as SessionStateModule;

            //
            sessionModule.End += new EventHandler(OnSessionEnd);
            if (context.Application["Session_End"] == null)
                context.Application["Session_End"] = new EventHandler(OnSessionEnd);
            //
            //Type type = typeof(System.Web.HttpApplication);
            //type = type.Assembly.GetType("System.Web.HttpApplicationFactory");

            //System.Reflection.FieldInfo field = type.GetField("_theApplicationFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            //object factory = field.GetValue(field);

            //field = factory.GetType().GetField("_sessionOnEndMethod", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //object obj = field.GetValue(factory);

            //field.SetValue(factory, new EventHandler(OnSessionEnd).Method);


            context.PreRequestHandlerExecute += new EventHandler(OnRequestEnter);
            context.PostRequestHandlerExecute += new EventHandler(OnRequestEnd);
            context.Error += new EventHandler(OnRequestEnd);
            _isAuditing = true;
        }





        #endregion

        #region UsersOnline
        static void OnSessionEnd(object sender, EventArgs e)
        {
            DataManager manager = new DataManager();
            manager.Parameters.Clear();
            manager.Parameters.Add("@UserId", (HttpContext.Current.User as AccessControlPrincipal).Identity.UserId);
            manager.ExecuteNonQuery("delete from UsersOnline where UserId = @UserId");
            manager.Commit();
        }

        #endregion

        #region UsersInRequest
        void OnRequestEnd(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            if (IsDotNetRequest && IsUserLogged)
            {
                DataManager.Parameters.Clear();
                DataManager.Parameters.Add("@UserId", (application.Context.User as AccessControlPrincipal).Identity.UserId);
                DataManager.ExecuteNonQuery("delete from UsersInRequest where UserId = @UserId and SessionId = @@SPID");
                DataManager.Commit();
            }
            _isAuditing = false;
            DataManager.CloseConnection();
        }

        void OnRequestEnter(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            if (IsDotNetRequest && IsUserLogged)
            {
                application.Context.Trace.Warn("Audit Module Begin");
                try
                {
                    DataManager.Parameters.Clear();
                    DataManager.Parameters.Add("@UserId", (application.Context.User as AccessControlPrincipal).Identity.UserId);
                    DataManager.ExecuteNonQuery("insert into UsersInRequest (UserId, SessionId) values (@UserId, @@SPID)");
                    DataManager.Commit();
                }
                catch { }
                application.Context.Trace.Warn("Audit Module End");
            }
        }
        #endregion

        /// <summary>
        /// Indicates if the application is being logged
        /// </summary>
        public static bool IsAuditing
        {
            get
            {
                return _isAuditing;
            }
        }


    }
}
