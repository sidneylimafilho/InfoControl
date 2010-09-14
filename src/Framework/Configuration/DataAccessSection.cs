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
            get
            {
                return base["connectionStringName"].ToString();
            }
            set
            {
                base["connectionStringName"] = value;
            }
        }
    }
#endif
}
