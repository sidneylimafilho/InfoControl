using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Web;

using System.Linq.Expressions;
using System.Web.Configuration;

using InfoControl.Web.UI;
using InfoControl.Data;

namespace InfoControl.Web.Configuration
{
    public partial class Application
    {
        #region Members
        private static object _lock = new object();
        private static Application _currentApplication;
        private static DateTime _lastUpdated;
        private static Dictionary<string, SystemParameter> _currentSystemParameters;
        #endregion

        #region Properties
        public System.Web.HttpApplicationState Items
        {
            get { return HttpContext.Current.Application; }

        }
        public static void Refresh()
        {
            lock (_lock)
            {
                _currentApplication = GetCurrentApplication();
                _lastUpdated = DateTime.Now;
            }
        }
        public static Application Current
        {
            get
            {
                if (_currentApplication == null)
                    Refresh();
                return _currentApplication;
            }
        }
        public Dictionary<string, SystemParameter> SystemParameters
        {
            get
            {
                if (_currentSystemParameters == null)
                {
                    lock (_lock)
                    {
                        if (_currentSystemParameters == null)
                        {
                            _currentSystemParameters = GetCurrentSystemParameters();
                        }
                    }
                }
                return _currentSystemParameters;
            }
        }
        #endregion

        /// <summary> This method retrieves a current Application.
        /// Change this method to alter how that record is received.
        /// </summary>
        internal static Application GetCurrentApplication()
        {
            Application application;

            using (ConfigurationDataContext db = (new DataManager()).CreateDataContext<ConfigurationDataContext>())
                application = db.Applications.Where(x => x.Name == ApplicationName).FirstOrDefault();

            if (application == null)
                throw new ConfigurationErrorsException("The application name dont match in database");

            return application;
        }

        /// <summary> This method retrieves a current System Parameters by Application.
        /// Change this method to alter how that record is received.
        /// </summary>
        internal static Dictionary<string, SystemParameter> GetCurrentSystemParameters()
        {
            DataManager manager = new DataManager();
            ConfigurationDataContext db = manager.CreateDataContext<ConfigurationDataContext>();
            return db.SystemParameters.Where(x => x.ApplicationId == _currentApplication.ApplicationId).ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Retrieve the name of the current application
        /// </summary>
        internal static string ApplicationName
        {
            get
            {
                try
                {
                    string text1 = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"];
                    if (!string.IsNullOrEmpty(text1))
                        return text1;

                    text1 = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
                    if (!string.IsNullOrEmpty(text1))
                        return text1;

                    text1 = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
                    int num1 = text1.IndexOf('.');
                    if (num1 != -1)
                        return text1.Remove(num1);

                    if (string.IsNullOrEmpty(text1))
                        return "/";

                    return text1;
                }
                catch
                {
                    return "/";
                }

            }
        }


    }
}
