using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Data;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

namespace Vivina.Erp.WebUI.Site
{
    public partial class CheckoutPayment : CheckoutPageBase
    {
        private Customer customer;
        private Int32 customerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request["CustomerId"]))
                throw new ArgumentNullException("The customer don't selected or the user not is logged!");

            Budget.Detach().CustomerId = Convert.ToInt32(Request["CustomerId"]);
            
            //customer = new CustomerManager(this).GetCustomer(customerId, Company.CompanyId);
        }

        protected void odsPaymentMethods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

               

        /// <summary>
        /// Creates Sale. This sale can be associated a an budget 
        /// </summary>
        /// <returns></returns>
        private Sale CreateSale()
        {
            var sale = new Sale();
            var budget = new Budget();

            sale.Discount = Decimal.Zero;

            if (!String.IsNullOrEmpty(Request["b"]))
            {
                budget = new SaleManager(this).GetBudget(Convert.ToInt32(Request["b"]),
                                                           Company.CompanyId);
                sale.BudgetId = budget.BudgetId;
                sale.Discount = Convert.ToDecimal(budget.Discount);
            }

            sale.CompanyId = Company.CompanyId;
            sale.OrderDate = DateTime.Now;
            sale.ShipDate = DateTime.Now;
            sale.SaleDate = DateTime.Now;

            if (Deposit != null)
                sale.DepositId = Deposit.DepositId;

            sale.CustomerId = customer.CustomerId;

            return sale;
        }


        /// <summary>
        /// Creates an saleItemList from budgetItems in db or budgetItems in basket
        /// </summary>
        /// <returns></returns>
        private List<SaleItem> CreateSaleItemsFromBudgetItems()
        {
            var saleItemList = new List<SaleItem>();
            var saleManager = new SaleManager(this);
            var budgetItemList = new List<BudgetItem>();

            if (!String.IsNullOrEmpty(Request["b"]))
                budgetItemList =
                    saleManager.GetBudgetItemByBudget(Convert.ToInt32(Request["b"]),
                                                        Company.CompanyId).ToList();
            else
                budgetItemList = Session["basket"] as List<BudgetItem>;

            foreach (BudgetItem item in budgetItemList)
            {
                var saleItem = new SaleItem();
                saleItem.CompanyId = Company.CompanyId;
                saleItem.ProductId = item.ProductId;
                saleItem.UnitCost = item.UnitCost;
                saleItem.UnitPrice = item.UnitPrice.Value;
                saleItem.Quantity = item.Quantity;
                saleItem.SpecialProductName = item.SpecialProductName;
                saleItem.Observation = item.Observation;
                saleItem.ModifiedDate = item.ModifiedDate;
                saleItemList.Add(saleItem);
            }
            return saleItemList;
        }

        /// <summary>
        /// This method creates an parcel list of deal with the financier condition
        /// </summary>
        /// <returns></returns>
        private List<Parcel> CreateParcelList()
        {
            var parcelList = new List<Parcel>();

            FinancierCondition financierCondition = new AccountManager(this).GetFinancierCondition(Company.CompanyId,
                                                                                                   Convert.ToInt32(
                                                                                                       Request.Form[
                                                                                                           "rbtFinancierCondition"
                                                                                                           ]));

            decimal amount = Convert.ToDecimal(Session["totalWithDeliveryPrice"]) / financierCondition.ParcelCount;

            for (int i = 1; i <= financierCondition.ParcelCount; i++)
            {
                var parcel = new Parcel();
                parcel.CompanyId = Company.CompanyId;
                parcel.FinancierOperationId = financierCondition.FinancierOperationId;
                parcel.PaymentMethodId = financierCondition.FinancierOperation.PaymentMethodId;
                parcel.Amount = amount;
                parcel.Description = i + "/" + financierCondition.ParcelCount;
                parcel.DueDate = DateTime.Now;
                parcel.EffectedAmount = null;
                parcel.EffectedDate = null;
                parcelList.Add(parcel);
            }
            return parcelList;
        }


        /// <summary>
        /// This method Generate sale
        /// </summary>
        private void GenerateSale()
        {
            var saleManager = new SaleManager(this);
            saleManager.SaveSale(CreateSale(), CreateSaleItemsFromBudgetItems(), User.Identity.UserId,
                                 CreateParcelList());
        }
    }
}
