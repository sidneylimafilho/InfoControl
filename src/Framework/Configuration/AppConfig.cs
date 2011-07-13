//#define CompactFramework
using System;
#if !CompactFramework
using System.Configuration;
#endif
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using InfoControl.Runtime;
using InfoControl.Data.Configuration;

namespace InfoControl.Configuration
{
    /// <summary>
    /// This class contains all configurations of the application
    /// </summary>
#if CompactFramework
    public static class AppConfig
    {

        private static System.Xml.XmlDocument doc;
        static AppConfig()
        {
            if (doc == null)
            {
                doc = new System.Xml.XmlDocument();
                doc.Load(AppPath() + System.AppDomain.CurrentDomain.FriendlyName + ".config");
            }
        }
#else
    public class AppConfig
    {
        private static System.Configuration.Configuration _config;

        protected static System.Configuration.Configuration Config
        {
            get
            {
                if (_config == null)
                {
                    if (System.Web.Hosting.HostingEnvironment.IsHosted)
                    {
                        _config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
                    }
                    else
                    {
                        _config = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
                    }
                }
                return _config;
            }
        }

        /// <summary> From the configurations in web.config file, gets the default connection string
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return ConnectionStrings[DataAccess.ConnectionStringName].ConnectionString;
        }

        /// <summary> From the configurations in web.config file, gets the default connection string name
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionStringName()
        {
            return AppConfig.DataAccess.ConnectionStringName;
        }

        /// <summary> From the configurations in web.config file, gets the default provider name
        /// </summary>
        /// <returns></returns>
        public static string GetProviderName()
        {
            return AppConfig.ConnectionStrings[AppConfig.DataAccess.ConnectionStringName].ProviderName;
        }

        /// <summary>Returns the specified <see cref="T:System.Configuration.ConfigurationSection"></see> object.</summary>
        /// <returns>The specified <see cref="T:System.Configuration.ConfigurationSection"></see> object.</returns>
        /// <param name="sectionName">The path to the section to be returned.</param>
        public static ConfigurationSection GetSection(string sectionName)
        {
            return Config.GetSection(sectionName);
        }

        /// <summary>Returns the specified <see cref="T:System.Configuration.ConfigurationSection"></see> object.</summary>
        /// <returns>The specified <see cref="T:System.Configuration.ConfigurationSection"></see> object.</returns>
        /// <param name="sectionName">The path to the section to be returned.</param>
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            return Config.GetSection(sectionName) as T;
        }

        /// <summary>Writes the configuration settings contained within this <see cref="T:System.Configuration.Configuration"></see> object to the current XML configuration file.</summary>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The configuration file could not be written to.- or -The configuration file has changed. </exception>
        public static void Save()
        {
            Config.Save();
        }

        /// <summary>Writes the configuration settings contained within this <see cref="T:System.Configuration.Configuration"></see> object to the current XML configuration file.</summary>
        /// <param name="saveMode">A <see cref="T:System.Configuration.ConfigurationSaveMode"></see> value that determines which property values to save.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The configuration file could not be written to.- or -The configuration file has changed. </exception>
        public static void Save(ConfigurationSaveMode saveMode)
        {
            Config.Save(saveMode);
        }

        /// <summary>Writes the configuration settings contained within this <see cref="T:System.Configuration.Configuration"></see> object to the current XML configuration file.</summary>
        /// <param name="saveMode">A <see cref="T:System.Configuration.ConfigurationSaveMode"></see> value that determines which property values to save.</param>
        /// <param name="forceSaveAll">true to save even if the configuration was not modified; otherwise, false.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The configuration file could not be written to.- or -The configuration file has changed. </exception>
        public static void Save(ConfigurationSaveMode saveMode, bool forceSaveAll)
        {
            Config.Save(saveMode, forceSaveAll);
        }

        /// <summary>Writes the configuration settings contained within this <see cref="T:System.Configuration.Configuration"></see> object to the specified XML configuration file.</summary>
        /// <param name="filename">The path and file name to save the configuration file to.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The configuration file could not be written to.- or -The configuration file has changed. </exception>
        public static void SaveAs(string filename)
        {
            Config.SaveAs(filename);
        }

        /// <summary>Writes the configuration settings contained within this <see cref="T:System.Configuration.Configuration"></see> object to the specified XML configuration file.</summary>
        /// <param name="saveMode">A <see cref="T:System.Configuration.ConfigurationSaveMode"></see> value that determines which property values to save.</param>
        /// <param name="filename">The path and file name to save the configuration file to.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The configuration file could not be written to.- or -The configuration file has changed. </exception>
        public static void SaveAs(string filename, ConfigurationSaveMode saveMode)
        {
            Config.SaveAs(filename, saveMode);
        }

        /// <summary>Writes the configuration settings contained within this <see cref="T:System.Configuration.Configuration"></see> object to the specified XML configuration file.</summary>
        /// <param name="saveMode">A <see cref="T:System.Configuration.ConfigurationSaveMode"></see> value that determines which property values to save.</param>
        /// <param name="forceSaveAll">true to save even if the configuration was not modified; otherwise, false.</param>
        /// <param name="filename">The path and file name to save the configuration file to.</param>
        /// <exception cref="T:System.ArgumentException">filename is null or an empty string ("").</exception>
        public static void SaveAs(string filename, ConfigurationSaveMode saveMode, bool forceSaveAll)
        {
            Config.SaveAs(filename, saveMode, forceSaveAll);
        }
#endif

#if CompactFramework
        public static string AppPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\";
        }
#else
        public static string AppPath()
        {
            return Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\";
        }
#endif

        private static DataAccessSection _dataAccessSection;
        public static DataAccessSection DataAccess
        {
            get
            {
#if CompactFramework
                _dataAccessSection = new DataAccessSection();
                _dataAccessSection.ConnectionStringName = doc.GetElementsByTagName("DataAccess")[0].Attributes["connectionStringName"].Value;
#else
                _dataAccessSection = (DataAccessSection)ConfigurationManager.GetSection("InfoControl/DataAccess");
#endif
                if (_dataAccessSection == null)
                {
                    throw new NotImplementedException(String.Format(Properties.Resources.Section_NoConfigured, "DataAccessSection"));
                }
                return _dataAccessSection;
            }
        }


        /// <summary>
        /// Returns a System.Collections.Specialized.NameValueCollection object containing
        /// the contents of the System.Configuration.AppSettingsSection object for the
        /// current application's default configuration.
        /// </summary>
#if CompactFramework
        private static NameValueCollection appSettingsList;
        public static NameValueCollection AppSettings
        {
            get
            {

                if (appSettingsList == null)
                {
                    appSettingsList = new NameValueCollection();

                    foreach (System.Xml.XmlNode node in doc.GetElementsByTagName("appSettings")[0].ChildNodes)
                    {
                        appSettingsList.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }
                return appSettingsList;
            }
        }
#else
        public static NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }
#endif



        /// <summary>
        /// Returns a System.Configuration.ConnectionStringSettingsCollection object
        /// containing the contents of the System.Configuration.ConnectionStringsSection
        /// object for the current application's default configuration.
        /// </summary>
#if CompactFramework
        private static Dictionary<String, ConnectionStringSettings> connectionStringList;
        public static Dictionary<String, ConnectionStringSettings> ConnectionStrings
        {
            get
            {
                if (connectionStringList == null)
                {
                    connectionStringList = new Dictionary<String, ConnectionStringSettings>();
                    foreach (System.Xml.XmlNode node in doc.GetElementsByTagName("connectionStrings")[0].ChildNodes)
                    {
                        ConnectionStringSettings connString = new ConnectionStringSettings();
                        connString.Name = node.Attributes["name"].Value;
                        connString.ProviderName = node.Attributes["providerName"].Value;
                        connString.ConnectionString = node.Attributes["connectionString"].Value;
                        connectionStringList.Add(connString.Name, connString);
                    }
                }
                return connectionStringList;
            }
        }

        public sealed class ConnectionStringSettings
        {
            public ConnectionStringSettings() { }

            public string ConnectionString;
            public string Name;
            public string ProviderName;
        }
#else
        public static ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }
#endif
    }
}

