using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using InfoControl.Configuration;

namespace InfoControl.Web.UrlRewriting
{
    [Serializable]
    public class UrlRulesSection : ConfigurationSection
    {
        [ConfigurationProperty("From", IsRequired = true, IsKey = true)]
        public string From
        {
            get { return (string)this["From"]; }
        }
        [ConfigurationProperty("To", IsRequired = true)]
        public string To
        {
            get { return (string)this["To"]; }
        }
    }

    [Serializable]
    public class UrlRulesSectionCollection : ConfigurationElementCollection<UrlRulesSection>
    {
        public UrlRulesSectionCollection() : base() { }
    }
}
