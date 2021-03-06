using System;
using System.Linq;
using System.Data.Linq;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.IO;
using System.Net.Mail;
using InfoControl.Web;

namespace InfoControl.Web.Auditing
{
    public class ExceptionNotifierModule : HttpDataModule
    {
        #region Members
        private Hashtable providerConfig = ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/Provider") as Hashtable;
        private Hashtable eventLogConfig = ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/EventLog") as Hashtable;
        private Hashtable databaseConfig = ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/Database") as Hashtable;
        private Hashtable emailConfig = ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/Email") as Hashtable;
        private Hashtable fileConfig = ConfigurationManager.GetSection("InfoControl/ExceptionNotifier/File") as Hashtable;
        private InfoControl.Web.Auditing.ExceptionManager notifier;
        private Type notifierType;
        #endregion

        #region IHttpModule Members
        private static bool _enabled = false;
        #endregion

        #region Properties
        public static bool Enabled
        {
            get { return _enabled; }
        }
        #endregion

        public override void Dispose()
        {

        }

        public override void Init(HttpApplication context)
        {
            base.Init(context);


            if (providerConfig == null)
                throw new ConfigurationErrorsException("It is necessary to configure at least a provider!");

            if (providerConfig["type"] == null)
                throw new ConfigurationErrorsException("It is necessary to configure a provider type!");

            notifierType = Type.GetType(providerConfig["type"].ToString());
            if (!typeof(InfoControl.Web.Auditing.ExceptionManager).IsAssignableFrom(notifierType))
                throw new ConfigurationErrorsException("The provider type must be inherits the 'InfoControl.Web.Auditing.ExceptionManager'!");

            if (eventLogConfig == null && databaseConfig == null && emailConfig == null && fileConfig == null)
                throw new ConfigurationErrorsException("It is necessary to configure at least a notifier!");


            context.Error += new EventHandler(OnErrorRequest);
            _enabled = true;
        }

        public void OnErrorRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                notifier = Activator.CreateInstance(notifierType) as InfoControl.Web.Auditing.ExceptionManager;

                try
                {
                    var ex = HttpContext.Current.Error;
                    if ((ex.Message ?? "").Contains("HttpUnhandledException"))
                        ex = ex.InnerException;

                    HttpContext.Current.Trace.Warn("Exception Module Begin");
                    notifier.Notify(ex);
                }
                catch (Exception ex)
                {
                    //
                    // Retry to notify
                    //
                    notifier.Notify(ex);
                }
                finally
                {
                    //
                    // Guarda no cache o erro para ser mostrado amigavelmente no Custom Error pages
                    //
                    //if (HttpContext.Current.Session != null)
                    //    HttpContext.Current.Session["Error"] = exception;
                    HttpContext.Current.Trace.Warn("Exception Module End");
                }
            }
        }


    }
}
