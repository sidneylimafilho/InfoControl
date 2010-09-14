using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using InfoControl.Payment;
using System.Data;

namespace Vivina.Erp.WebUI.Site
{
    public partial class CheckoutPaymentProcess : CheckoutPageBase
    {
        public PaymentResult result;
        //private Customer customer;
        //private Int32 customerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            int operationId = Convert.ToInt32(Request["operationId"]);
            int parcelCount = Convert.ToInt32(Request["parcelCount"]);
            FinancierOperation operation;
            FinancierCondition condition;

            if (!String.IsNullOrEmpty(Request["sale"]))
            {
                var saleManager = new SaleManager(this);
                var sale = saleManager.GetSale(Company.CompanyId, Convert.ToInt32(Request["sale"]));
                result = ProcessPayment(sale);
            }
        }

        private PaymentResult ProcessPayment(Sale sale)
        {
            var provider = sale.FinancierOperation.PaymentProvider;

            if (provider != null)
            {
                PaymentMode mode = PaymentMode.InCash;
                if (provider is MasterCardProvider || provider is VisaProvider)
                    mode = PaymentMode.CreditWithInterestIssuer;

                CreditCard card = null;
                if (!String.IsNullOrEmpty(Request["cardHolder"]))
                    card = new CreditCard(Request["cardHolder"], Request["cardNumber"],
                                          Request["cardCvc2"], Request["cardMonth"], Request["cardYear"], 0);

                return provider.Process(Total.ToString(),
                                        mode,
                                        sale.Invoice.Parcels.Count(),
                                        sale.FinancierOperation.MembershipNumber,
                                        "",
                                        sale.SaleId,
                                        card);
            }

            return new PaymentResult("");
        }



    }
}