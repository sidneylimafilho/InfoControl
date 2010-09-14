using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.BusinessRules;
using InfoControl;
using System.Web.Services;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.WebUI.InfoControl.Accounting
{
    public partial class ExpenditureAuthorizations : Vivina.Erp.SystemFramework.PageBase
    {

        private AccountManager _accountManager;

        public AccountManager AccountManager
        {
            get
            {
                if (_accountManager == null)
                    _accountManager = new AccountManager(this);

                return _accountManager;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlAuthorize.Visible = grdExpenditureAuthorizations.Rows.Count != 0;
        }

        protected void odsExpenditureAuthorizations_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;            
        }

        protected void grdExpenditureAuthorizations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = "location='ExpenditureAuthorization.aspx?ExpenditureAuthorizationId=" + e.Row.DataItem.GetPropertyValue("ExpenditureAuthorizationId").EncryptToHex() + "';";

                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('Deseja realmente excluir esse item ?') == false) return false;");

                var litStatus = e.Row.FindControl("litStatus") as Literal;

                if (e.Row.DataItem.GetPropertyValue("IsDenied") == null)
                {
                    litStatus.Text = "Pendente";
                    e.Row.Style.Add("background-color", "yellow");
                    return;
                }

                litStatus.Text = Convert.ToBoolean(e.Row.DataItem.GetPropertyValue("IsDenied")) ? "Negado" : "Autorizado";
            }
        }

        protected void btnAuthorize_Click(object sender, EventArgs e)
        {
            if (!HasSelectedExpenditures())
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(),"","alert('Marque ao menos uma linha!');", true);
                return;
            }

            var expenditureAuthorizationIds = new List<Int32>();

            foreach (var expenditureAuthorizationId in Request["chkSelectRow"].Split(','))
                expenditureAuthorizationIds.Add(Convert.ToInt32(expenditureAuthorizationId));

            AccountManager.AuthorizeExpenditures(Company.CompanyId, expenditureAuthorizationIds);

            grdExpenditureAuthorizations.DataBind();
        }

        /// <summary>
        /// This method verifies if exist some expenditure selected
        /// </summary>
        /// <returns></returns>
        private bool HasSelectedExpenditures()
        {
            return Request.Form["chkSelectRow"] != null;
        }

        protected void btnNonAuthorize_Click(object sender, EventArgs e)
        {
            if (!HasSelectedExpenditures())
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Marque ao menos uma linha!');", true);
                return;
            }

            var expenditureAuthorizationIds = new List<Int32>();

            foreach (var expenditureAuthorizationId in Request["chkSelectRow"].Split(','))
                expenditureAuthorizationIds.Add(Convert.ToInt32(expenditureAuthorizationId));

            AccountManager.NonAuthorizeExpenditures(expenditureAuthorizationIds);

            grdExpenditureAuthorizations.DataBind();
        }

    }
}