using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfoControl.Configuration;

namespace InfoControl.Web.UrlRewriting
{
    public class UrlRewriterSectionGroup : ConfigurationSection
    {
        [ConfigurationProperty("Rules", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(UrlRulesSectionCollection), AddItemName = "Role")]
        public UrlRulesSectionCollection Rules
        {
            get
            {
                return (UrlRulesSectionCollection)this["Rules"];
            }
        }
    }
}
