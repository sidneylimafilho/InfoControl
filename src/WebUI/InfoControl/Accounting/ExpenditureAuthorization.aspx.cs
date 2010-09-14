using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;

namespace Vivina.Erp.WebUI.InfoControl.Accounting
{
    public partial class ExpenditureAuthorization : Vivina.Erp.SystemFramework.PageBase
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
            if (!IsPostBack)            
                if (!String.IsNullOrEmpty(Request["ExpenditureAuthorizationId"]))
                    ShowExpenditureAuthorization();            
        }

        /// <summary>
        /// This method just shows informations of specified ExpenditureAuthorization at screen
        /// </summary>
        private void ShowExpenditureAuthorization()
        {
            var expenditureAuthorizationId = Convert.ToInt32(Request["ExpenditureAuthorizationId"].DecryptFromHex());
            var expenditureAuthorization = AccountManager.GetExpenditureAuthorization(expenditureAuthorizationId);

            Page.ViewState["customerCallId"] = expenditureAuthorization.CustomerCallId;

            txtCallNumber.Text = expenditureAuthorization.CustomerCall.CallNumber;
            ucCurrFieldAmount.CurrencyValue = expenditureAuthorization.Amount;
            txtDescription.Text = expenditureAuthorization.Description;

            ShowCustomerCallInformations(expenditureAuthorization.CustomerCall);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.ViewState["customerCallId"] == null)
            {
                ShowError("Número de chamado inexistente!");
                return;
            }

            var expenditureAuthorization = new Vivina.Erp.DataClasses.ExpenditureAuthorization();

            if (!String.IsNullOrEmpty(Request["ExpenditureAuthorizationId"]))
                expenditureAuthorization = AccountManager.GetExpenditureAuthorization(Convert.ToInt32(Request["ExpenditureAuthorizationId"].DecryptFromHex()));

            expenditureAuthorization.CustomerCallId = Convert.ToInt32(Page.ViewState["customerCallId"]);

            expenditureAuthorization.Amount = ucCurrFieldAmount.CurrencyValue.Value;
            expenditureAuthorization.CompanyId = Company.CompanyId;
            expenditureAuthorization.Description = txtDescription.Text;

            AccountManager.SaveExpenditureAuthorization(expenditureAuthorization);
            Response.Redirect("ExpenditureAuthorizations.aspx");
        }

        /// <summary>
        /// This method displays customer, representant and employee informations in screen
        /// </summary>
        /// <param name="customerCall"></param>
        private void ShowCustomerCallInformations(CustomerCall customerCall)
        {
            pnlRepresentantName.Visible = pnlTechnicalEmployeeName.Visible = false;
            pnlCustomerName.Visible = true;

            Page.ViewState["customerCallId"] = customerCall.CustomerCallId;

            litCustomer.Text = customerCall.Customer.Name;

            if (customerCall.RepresentantId.HasValue)
            {
                pnlRepresentantName.Visible = true;
                litRepresentant.Text = customerCall.Representant.Profile != null ? customerCall.Representant.Profile.Name : customerCall.Representant.LegalEntityProfile.CompanyName;
            }

            if (customerCall.TechnicalEmployeeId.HasValue)
            {
                pnlTechnicalEmployeeName.Visible = true;
                litTechnicalEmployee.Text = customerCall.Employee.Profile.Name;                
            }
        }

        protected void txtCallNumber_Changed(object sender, EventArgs e)
        {
            pnlCustomerName.Visible = pnlRepresentantName.Visible = pnlTechnicalEmployeeName.Visible = false;

            if (!String.IsNullOrEmpty(txtCallNumber.Text))
            {
                var customerCall = new CustomerManager(this).GetCustomerCall(txtCallNumber.Text);

                if (customerCall != null)
                    ShowCustomerCallInformations(customerCall);
                else
                    Page.ViewState["customerCallId"] = null;
            }
        }

    }
}