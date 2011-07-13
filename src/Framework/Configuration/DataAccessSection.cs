using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace InfoControl.Data.Configuration
{
#if CompactFramework
    public class DataAccessSection
    {
        public string ConnectionStringName;
    }
#else
    public class DataAccessSection : ConfigurationSection
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return base["connectionStringName"].ToString(); }
            set { base["connectionStringName"] = value; }
        }


        [ConfigurationProperty("tenantConnectionStringName")]
        public string TenantConnectionStringName
        {
            get { return base["tenantConnectionStringName"].ToString(); }
            set { base["tenantConnectionStringName"] = value; }
        }

        [ConfigurationProperty("tenantGetQuery")]
        public string TenantGetQuery
        {
            get { return base["tenantGetQuery"].ToString(); }
            set { base["tenantGetQuery"] = value; }
        }

        [ConfigurationProperty("tenantCookieName")]
        public string TenantCookieName
        {
            get { return base["tenantCookieName"].ToString(); }
            set { base["tenantCookieName"] = value; }
        }

        [ConfigurationProperty("tenantDefaultID")]
        public string TenantDefaultID
        {
            get { return base["tenantDefaultID"].ToString(); }
            set { base["tenantDefaultID"] = value; }
        }
    }
#endif
}
