using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl.Web.UI;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

namespace InfoControl.Web.UI
{
    public partial class Payment : Vivina.Erp.SystemFramework.UserControlBase
    {
        public Decimal totalWithDeliveryPrice;
        public Decimal total;

        protected void Page_Load(object sender, EventArgs e)
        {
            totalWithDeliveryPrice = Convert.ToDecimal(Page.ViewState["totalWithDeliveryPrice"]);

            total = Convert.ToDecimal(Page.ViewState["total"]);

        }

        protected void odsPaymentMethods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            total = Convert.ToDecimal(Page.ViewState["total"]);

            e.InputParameters["companyId"] = Page.Company.CompanyId;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request.Form["rbtFinancierCondition"]))
            {
                lblError.Text = "Selecione uma condição de pagamento!";
                return;
            }

            Session["FinacierConditionId"] = Request.Form["rbtFinancierCondition"];

            var financierCondition = new AccountManager(this).GetFinancierCondition(Page.Company.CompanyId, Convert.ToInt32(Request.Form["rbtFinancierCondition"]));

            Session["amount"] = totalWithDeliveryPrice / financierCondition.ParcelCount;
            Response.Redirect("CreateCustomer.aspx");
        }
    }
}