using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


namespace Vivina.Erp.WebUI.InfoControl.Accounting
{
    public partial class Statement : Vivina.Erp.SystemFramework.PageBase
    {

        AccountManager accountManager;
        Decimal StatementTotal = decimal.Zero;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request["StatementId"]))
                {
                    Page.ViewState["StatementId"] = Request["StatementId"].DecryptFromHex();

                    var companyManager = new CompanyManager(this);
                    var statement = companyManager.GetStatement(Convert.ToInt32(Page.ViewState["StatementId"]));
                    var l = statement.Company.LegalEntityProfile;

                    litCustomerName.Text = l.CompanyName;
                    litCustomerAddress.Text = l.Address.Name + " - " + l.AddressNumber + " " + l.AddressComp;
                    litCustomerEmail.Text = l.Email;
                    litCustomerPhone.Text = l.Phone;

                    litPeriodBegin.Text = statement.PeriodBegin.ToShortDateString();
                    litPeriodEnd.Text = statement.PeriodEnd.ToShortDateString();
                    litBoletusNumber.Text = statement.BoletusNumber;

                    grdStatementItems.DataSource = statement.StatementItems;
                    grdStatementItems.DataBind();
                }
            }
        }



        protected void grdStatementItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                StatementTotal += Convert.ToDecimal(grdStatementItems.DataKeys[e.Row.RowIndex]["Value"]);
                lblStatementTotal.Text = Convert.ToString(StatementTotal);
            }
        }
    }
}