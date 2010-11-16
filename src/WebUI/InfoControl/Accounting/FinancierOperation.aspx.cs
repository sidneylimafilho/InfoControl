using System;
using System.Linq;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

[PermissionRequired("FinancierOperation")]
public partial class InfoControl_Accounting_FinancierOperation : PageBase
{
    private AccountManager _accountManager;
    private FinancierOperation _originalFinancierOperation;

    public Boolean IsLoaded
    {
        get
        {
            Int32 tmp = 0;
            if (Page.ViewState["FinancierOperationId"] != null &&
                Int32.TryParse(Convert.ToString(Page.ViewState["FinancierOperationId"]), out tmp))
                return true;
            else
                return false;
        }
    }

    public AccountManager AccountManager
    {
        get
        {
            if (_accountManager == null)
                _accountManager = new AccountManager(this);

            return _accountManager;
        }
    }

    public FinancierOperation OriginalFinancierOperation
    {
        get
        {
            _originalFinancierOperation = _originalFinancierOperation ??
                AccountManager.GetFinancierOperation(Company.CompanyId,
                                                     Convert.ToInt32(Page.ViewState["FinancierOperationId"]));
            return _originalFinancierOperation;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearFields();
            if (!String.IsNullOrEmpty(Request["FinancierOperationId"]))
            {
                Page.ViewState["FinancierOperationId"] = Request["FinancierOperationId"];
                ShowFinancierOperation();
            }
        }
    }


    protected void generic_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsFinancierCondition_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (IsLoaded)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
            e.InputParameters["financierOperationId"] = Convert.ToInt32(Page.ViewState["FinancierOperationId"]);
        }
    }

    private void ShowFinancierOperation()
    {
        cboPaymentMethod.SelectedValue = OriginalFinancierOperation.PaymentMethodId.ToString();

        ucCurrFieldAdminTax.CurrencyValue = OriginalFinancierOperation.AdminTax;
        ucCurrFieldAdminTaxUnit.CurrencyValue = OriginalFinancierOperation.AdminTaxUnit;
        ucCurrFieldDiscount.CurrencyValue = OriginalFinancierOperation.Discount;
        txtMembershipNUmber.Text = Convert.ToString(OriginalFinancierOperation.MembershipNumber);
        txtOperationNumber.Text = Convert.ToString(OriginalFinancierOperation.OperationNumber);
        cboAccount.SelectedValue = OriginalFinancierOperation.AccountId.ToString();

        pnlFinancierCondition.Visible = true;
        grdFinancierCondition.DataBind();
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {

        if (ucCurrFieldParcelCount.IntValue == 0)
        {
            ShowError("Quantidade de parcelas não pode ser zero!");
            return;
        }

        var financierCondition = new FinancierCondition
                                     {
                                         CompanyId = Company.CompanyId,
                                         FinancierOperationId = OriginalFinancierOperation.FinancierOperationId,
                                         MonthlyTax = ucCurrFieldMonthlyTax.CurrencyValue.Value,
                                         ParcelCount = ucCurrFieldParcelCount.IntValue
                                     };

        AccountManager.InsertFinancierCondition(financierCondition);
        grdFinancierCondition.DataBind();
        ucCurrFieldMonthlyTax.Text = ucCurrFieldParcelCount.Text = String.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var payment = AccountManager.GetPaymentMethod(Convert.ToInt32(cboPaymentMethod.SelectedValue));

        if (payment != null)
            if (String.IsNullOrEmpty(txtMembershipNUmber.Text) || String.IsNullOrEmpty(txtOperationNumber.Text))
                if (payment.PaymentMethodId == PaymentMethod.MasterCard || payment.PaymentMethodId == PaymentMethod.Visa)
                {
                    ShowError("O numero de afiliação e código de operação são obrigatórios!");
                    return;
                }

        var financierOperation = new FinancierOperation();

        if (IsLoaded)
        {
            financierOperation.CopyPropertiesFrom(OriginalFinancierOperation);

            if (OriginalFinancierOperation.PaymentMethodId != Convert.ToInt32(cboPaymentMethod.SelectedValue) && VerifyUniquePaymentMethod())
            {
                ShowError("Já existe uma operação financeira com o método de pagamento fornecido!");
                return;
            }
        }

        financierOperation.CompanyId = Company.CompanyId;
        financierOperation.AdminTax = Convert.ToDecimal(ucCurrFieldAdminTax.CurrencyValue);
        financierOperation.AdminTaxUnit = Convert.ToDecimal(ucCurrFieldAdminTaxUnit.CurrencyValue);
        financierOperation.Discount = Convert.ToDecimal(ucCurrFieldDiscount.CurrencyValue);
        financierOperation.PaymentMethodId = Convert.ToInt32(cboPaymentMethod.SelectedValue);
        financierOperation.MembershipNumber = txtMembershipNUmber.Text;
        financierOperation.OperationNumber = txtOperationNumber.Text;
        financierOperation.AccountId = Convert.ToInt32(cboAccount.SelectedValue);


        if (IsLoaded)
            AccountManager.UpdateFinancierOperation(OriginalFinancierOperation, financierOperation);
        else
        {
            if (VerifyUniquePaymentMethod())
            {
                ShowError("Já existe uma operação financeira com o método de pagamento fornecido!");
                return;
            }

            AccountManager.InsertFinancierOperation(financierOperation);
            Page.ViewState["FinancierOperationId"] = financierOperation.FinancierOperationId;
            pnlFinancierCondition.Visible = true;
            return;
        }
        //if (financierOperation.PaymentProvider != null)
        //  financierOperation.PaymentProvider.SaveConfiguration(financierOperation.MembershipNumber, financierOperation.OperationNumber);

        Response.Redirect("~/InfoControl/Accounting/FinancierOperations.aspx");
    }

    [WebMethod]
    public static bool DeleteFinancierCondition(Int32 companyId, int financierConditionId)
    {
        bool result = true;
        using (var accountManager = new AccountManager(null))
        {
            try
            {
                accountManager.DeleteFinancierCondition(accountManager.GetFinancierCondition(companyId, financierConditionId));
            }
            catch (SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    #region private functions

    private void ClearFields()
    {
        ucCurrFieldDiscount.CurrencyValue =
          ucCurrFieldAdminTax.CurrencyValue = ucCurrFieldAdminTaxUnit.CurrencyValue = null;
    }

    /// <summary>
    /// This method verify if the paymentMethod supplied already exists in db
    /// </summary>
    private bool VerifyUniquePaymentMethod()
    {
        return new AccountManager(this)
                .GetFinancierOperations(Company.CompanyId,
                                        Convert.ToInt32(cboPaymentMethod.SelectedValue))
                .Any();
    }

    #endregion

    protected void odsAccounts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}