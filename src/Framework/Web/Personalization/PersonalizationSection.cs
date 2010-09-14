using System;
using System.IO;
using System.Configuration;
using System.Web;
using System.Threading;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
using System.Text;


using InfoControl.Runtime;

using InfoControl.Security;

namespace InfoControl.Web.Personalization
{
    public class PersonalizationSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }

        [ConfigurationProperty("cookiesName", IsRequired = true)]
        public string CookieName
        {
            get { return (string)base["cookiesName"]; }
            set { base["cookiesName"] = value; }
        }

        [ConfigurationProperty("cookiesTimeout", IsRequired = true)]
        public int CookieTimeout
        {
            get { return (int)base["cookiesTimeout"]; }
            set { base["cookiesTimeout"] = value; }
        }

        [ConfigurationProperty("cookiePath", IsRequired = false)]
        public string CookiePath
        {
            get { return (string)base["cookiePath"]; }
            set { base["cookiePath"] = value; }
        }

        [ConfigurationProperty("cookieRequireSSL", IsRequired = false)]
        public bool CookieRequireSSL
        {
            get { return (bool)base["cookieRequireSSL"]; }
            set { base["cookieRequireSSL"] = value; }
        }

        [ConfigurationProperty("cookieSlidingExpiration", IsRequired = false)]
        public bool CookieSlidingExpiration
        {
            get { return (bool)base["cookieSlidingExpiration"]; }
            set { base["cookieSlidingExpiration"] = value; }
        }

        [ConfigurationProperty("cookieProtection", IsRequired = false)]
        public bool CookieProtection
        {
            get { return (bool)base["cookieProtection"]; }
            set { base["cookieProtection"] = value; }
        }

        [ConfigurationProperty("createPersistentCookie", IsRequired = false)]
        public bool CreatePersistentCookie
        {
            get { return (bool)base["createPersistentCookie"]; }
            set { base["createPersistentCookie"] = value; }
        }

        [ConfigurationProperty("requireAuthentication", IsRequired = false)]
        public bool RequireAuthentication
        {
            get { return (bool)base["requireAuthentication"]; }
            set { base["requireAuthentication"] = value; }
        }
    }
}
