using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoControl.Payment;

namespace Vivina.Erp.DataClasses
{
    public partial class FinancierOperationType
    {
        public const int Cash = 1;
        public const int Check = 2;
        public const int Financing = 3;
    }

    public partial class FinancierOperation
    {
        PaymentProvider provider;
        public PaymentProvider PaymentProvider
        {
            get
            {
                switch (PaymentMethodId)
                {
                    case PaymentMethod.Visa:
                        return provider ?? (provider = new VisaProvider());
                    case PaymentMethod.MasterCard:
                        return provider ?? (provider = new MasterCardProvider());
                    case PaymentMethod.Boleto:
                        return provider ?? (provider = new BoletoProvider());
                    default:
                        return null;
                }
            }
        }
    }
}
