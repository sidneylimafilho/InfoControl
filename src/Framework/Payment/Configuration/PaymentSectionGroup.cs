using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfoControl.Configuration;

namespace InfoControl.Payment.Configuration
{
    public class PaymentSectionGroup :  ConfigurationSection
    {
        [ConfigurationProperty("Providers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProviderSectionCollection), AddItemName = "Provider")]
        public ProviderSectionCollection Providers
        {
            get
            {
                return (ProviderSectionCollection)this["Providers"];
            }
        }        
    }

    
}
