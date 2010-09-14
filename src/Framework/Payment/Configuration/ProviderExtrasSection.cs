using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfoControl.Configuration;

namespace InfoControl.Payment.Configuration
{

    [Serializable]
    public class ProviderExtrasSection : ConfigurationSection
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["key"]; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
        }
    }

    [Serializable]
    public class ProviderExtrasSectionCollection : ConfigurationElementCollection<ProviderExtrasSection>
    {
        public ProviderExtrasSectionCollection() : base() { }
    }
}
