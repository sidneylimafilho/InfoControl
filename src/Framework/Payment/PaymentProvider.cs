using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Configuration;

using InfoControl.Payment.Configuration;

namespace InfoControl.Payment
{
    public abstract class PaymentProvider
    {
        private readonly PaymentSectionGroup _rules = (PaymentSectionGroup)ConfigurationManager.GetSection("InfoControl/Payment");
        ProviderSection section = null;

        public ProviderSection Config
        {
            get
            {
                section = _rules.Providers.Cast<ProviderSection>().FirstOrDefault(p => p.Type == this.ToString());
                if (section == null)
                    throw new ArgumentNullException("The InfoControl.Payment config is invalid! Check whether " + this.ToString() + " key exists!");

                return section;
            }
        }

        public virtual PaymentResult Process(string total, PaymentMode mode, int numParcels, string filiacao, string distribuidor, int numPedido, CreditCard cartao)
        {
            throw new NotImplementedException();
        }

        public virtual void SaveConfiguration(string membership, string operation)
        {
            throw new NotImplementedException();
        }
    }
}
