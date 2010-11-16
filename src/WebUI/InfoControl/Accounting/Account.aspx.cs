using System;
using System.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Account")]
public partial class Accounting_Account : Vivina.Erp.SystemFramework.PageBase
{

    AccountManager accountManager;
    Account account;
    Account originalAccount;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["AccountId"]))
            {
                Page.ViewState["AccountId"] = Convert.ToInt32(Request["AccountId"]);
                ShowAccount();
            }
        }
    }

    private void ShowAccount()
    {
        accountManager = new AccountManager(this);
        originalAccount = accountManager.GetAccount(Convert.ToInt32(Page.ViewState["AccountId"]), Company.CompanyId);
       
        cboBank.SelectedValue = Convert.ToString(originalAccount.BankId);        
        txtAccountNumber.Text = originalAccount.AccountNumber;
        txtAgency.Text = originalAccount.Agency;
        txtAgencyMail.Text = originalAccount.AgencyMail;
        txtAgencyManager.Text = originalAccount.AgencyManager;
        txtAgencyPhone.Text = originalAccount.AgencyPhone;

        ucAddress.PostalCode = originalAccount.PostalCode;
        ucAddress.AddressComp = originalAccount.AddressComp;
        ucAddress.AddressNumber = originalAccount.AddressNumber;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        accountManager = new AccountManager(this);
        account = new Account();

        if (!String.IsNullOrEmpty(Request["AccountId"]))
        {
            originalAccount = accountManager.GetAccount(Convert.ToInt32(Page.ViewState["AccountId"]), Company.CompanyId);
            account.CopyPropertiesFrom(originalAccount);
        }

        account.CompanyId = Company.CompanyId;
        account.BankId = Convert.ToInt32(cboBank.SelectedValue);
        account.AccountNumber = txtAccountNumber.Text;
        account.Agency = txtAgency.Text;
        account.AgencyMail = txtAgencyMail.Text;
        account.AgencyManager = txtAgencyManager.Text;
        account.AgencyPhone = txtAgencyPhone.Text;

        account.PostalCode = ucAddress.PostalCode;
        account.AddressComp = ucAddress.AddressComp;
        account.AddressNumber = ucAddress.AddressNumber;

        if (!String.IsNullOrEmpty(Request["AccountId"]))
            accountManager.Update(originalAccount, account);
        else
            accountManager.Insert(account);

        Response.Redirect("Accounts.aspx");

    }
}