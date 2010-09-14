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
    public partial class CheckoutSummary : CheckoutPageBase
    {
        public PaymentResult result;
        //private Customer customer;
        //private Int32 customerId;

        public Sale Sale {
            get { return Session["Sale"] as Sale; } 
            set { Session["Sale"] = value; } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var saleManager = new SaleManager(this);

            if (String.IsNullOrEmpty(Request["sale"]))
            {
                int operationId = Convert.ToInt32(Request["operationId"]);
                int parcelCount = Convert.ToInt32(Request["parcelCount"]);
                FinancierOperation operation;
                FinancierCondition condition;

                #region ValidatePaymentProcess

                var accountManager = new AccountManager(this);
                operation = accountManager.GetFinancierOperation(Company.CompanyId, operationId);

                if (operation == null)
                    throw new ArgumentNullException();

                condition = operation.FinancierConditions.FirstOrDefault(x => x.ParcelCount == parcelCount) ?? new FinancierCondition()
                {
                    CompanyId = Company.CompanyId,
                    FinancierOperation = operation,
                    ParcelCount = 1,
                    MonthlyTax = 0
                };

                #endregion


                Sale = new Sale();
                Sale.SaleDate = Sale.ShipDate = Sale.OrderDate = DateTime.Now.Date;
                Sale.CompanyId = Company.CompanyId;
                Sale.CustomerId = Budget.CustomerId;

                Sale = saleManager.SaveSale(Sale, Budget, User.Identity.UserId, DateTime.Now, condition);

                Response.Redirect("Checkout_Summary.aspx?sale=" + Sale.SaleId);
            }
            else
            {
                Sale = saleManager.GetSale(Company.CompanyId, Convert.ToInt32(Request["sale"]));
            }
        }


    }
}