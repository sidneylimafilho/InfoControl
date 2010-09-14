using System;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using InfoControl.Web.Configuration;
using InfoControl.Properties;

namespace InfoControl.Web.Configuration
{
    public class MaintenanceModule : HttpDataModule
    {
        Hashtable config;


        private static bool _enabled = false;
        public static bool Enabled
        {
            get { return _enabled; }
        }

        #region IHttpModule Members
        public override void Init(HttpApplication context)
        {
            base.Init(context);


            config = ConfigurationManager.GetSection("InfoControl/Maintenance") as Hashtable;
            if (config == null)
            {
                throw new ConfigurationErrorsException(String.Format(Resources.Section_dont_was_configured, "InfoControl/Maintenance"));
            }

            if (config["maintenancePage"] == null)
            {
                throw new NullReferenceException("The attribute 'maintenancePage' in Maintenance Section is required!");
            }

            string maintenancePage = config["maintenancePage"].ToString();
            if (String.IsNullOrEmpty(maintenancePage))
            {
                throw new NullReferenceException("The attribute 'maintenancePage' in Maintenance Section is empty!");
            }

            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(maintenancePage);
            if (!System.IO.File.Exists(filePath))
            {
                throw new NullReferenceException("The attribute 'maintenancePage' in Maintenance Section is invalid!");
            }
            _enabled = true;
            context.PostAcquireRequestState += new EventHandler(OnBeginRequest);
        }
        public void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = (sender as HttpApplication).Context;

            context.Trace.Warn("Maintenace Module Begin");
            if (IsValid(context))
            {
                //
                // Check if application is Maintenance Mode
                //
                if ((!Application.Current.IsActive || Application.Current.IsMaintenance) && 
                    !context.Request.CurrentExecutionFilePath.ToLower().Contains("applications.aspx"))
                {
                    context.Response.Redirect("~/App_Offline.aspx", true);
                    return;
                }
            }
            context.Trace.Warn("Maintenance Module End");
        }

        private bool IsValid(HttpContext context)
        {
            if (!IsDotNetRequest)
                return false;
            
            if (!Convert.ToBoolean(config["enabled"]))
                return false;

            if (context.Request.PhysicalPath.ToLower().Contains("app_accesscontrol"))
                return false;

            return true;
        }
        public override void Dispose()
        {
            // TODO: The method or operation is not implemented.
        }
        #endregion
    }
}
