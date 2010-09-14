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
    public class ProviderSection : ConfigurationSection
    {

        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string Type
        {
            get { return (string)this["type"]; }
        }

        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)this["address"]; }
        }

        [ConfigurationProperty("Extras", IsRequired = false, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProviderExtrasSectionCollection), AddItemName = "add")]
        public ProviderExtrasSectionCollection Extras
        {
            get { return (ProviderExtrasSectionCollection)this["Extras"]; }
        }
    }

    [Serializable]
    public class ProviderSectionCollection : ConfigurationElementCollection<ProviderSection>
    {
        public ProviderSectionCollection() : base() { }
    }
}
