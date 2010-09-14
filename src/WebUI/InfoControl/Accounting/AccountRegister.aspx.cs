using System;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Security.Cryptography;
using InfoControl.Web.Security;
using InfoControl.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using System.Data.SqlTypes;

using DateTimeInterval = InfoControl.DateTimeInterval;
using System.Collections.Generic;

[PermissionRequired("AccountRegister")]
public partial class InfoControl_Accounting_AccountRegister : Vivina.Erp.SystemFramework.PageBase
{
    private Int32 tmp;
    private decimal totalDone = Decimal.Zero;
    private decimal totalPossible = Decimal.Zero;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CalculateBalance();
            ucDateTimeInterval.DateInterval = new DateTimeInterval(DateTime.Now.AddDays(-7), DateTime.Now.Date.AddDays(7));
            if (Page.Customization["dateTimeInterval"] != null)
                ucDateTimeInterval.DateInterval = (DateTimeInterval)Page.Customization["dateTimeInterval"];

            cboAccount.DataBind();

            if (Page.Customization["AccountIndex"] != null && cboAccount.Items.FindByValue(Convert.ToString(Page.Customization["AccountIndex"])) != null)
                cboAccount.SelectedValue = Convert.ToString(Page.Customization["AccountIndex"]);
        }
    }


    /// <summary>
    ///  This method calculates the possible cash of the company, cash that the company has in bank
    ///  and make the subtraction them
    /// </summary>
    private void CalculateBalance()
    {

        var accountManager = new AccountManager(this);

        //
        // Calculates the possible cash of the company
        //
        
        totalPossible = accountManager.GetParcelsValueFromInvoices(Company.CompanyId) - accountManager.GetParcelsValueFromBills(Company.CompanyId);

        //
        // Calculates the cash that the company has in bank
        //
        
        totalDone = (decimal)accountManager.GetSumConciliatedParcelsValueFromInvoices(Company.CompanyId) - (decimal)accountManager.GetSumConciliatedParcelsValueFromBills(Company.CompanyId);
        
        txtDone.Text = String.Format("{0:c}", totalDone); 
        txtPossible.Text = String.Format("{0:c}", totalPossible);
    }

    protected void odsAccountRegister_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (ucDateTimeInterval.DateInterval == null || !Int32.TryParse(cboAccount.SelectedValue, out tmp))
        {
            e.Cancel = true;
            return;
        }
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["accountId"] = Convert.ToInt32(cboAccount.SelectedValue);
        e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
    }

    protected void grdAccountRegister_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var billId = (Int32?)e.Row.DataItem.GetPropertyValue("BillId");
            var invoiceId = (Int32?)e.Row.DataItem.GetPropertyValue("InvoiceId");

            var link = e.Row.FindControl("lblParcelDescription") as HyperLink;
            if (link != null)
                link.NavigateUrl = (billId != null) ?
                    "Bill.aspx?BillId=" + billId.Value.EncryptToHex() :
                    "Invoice.aspx?InvoiceId=" + invoiceId.Value.EncryptToHex();
        }
    }

    protected void odsAccount_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["AccountIndex"] = cboAccount.SelectedValue;
        Page.Customization["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
        grdAccountRegister.DataBind();
        CalculateBalance();
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        var parcelsManager = new ParcelsManager(this);
        Parcel parcel;
        CheckBox registered;

        foreach (GridViewRow row in grdAccountRegister.Rows)
        {
            var parcelId = (Int32)grdAccountRegister.DataKeys[row.RowIndex]["ParcelId"];
            registered = (row.Cells[0].Controls[1] as CheckBox);

            if (registered != null)
                if (registered.Checked)
                {
                    parcel = parcelsManager.GetParcel(parcelId, Company.CompanyId);
                    if (parcel == null)
                        return;

                    var ucDtDateMov = (row.Cells[4].Controls[3] as Date);

                    parcel.EffectedAmount = parcel.Amount;

                    parcel.OperationDate = parcel.DueDate;
                    parcel.OperationDate = ucDtDateMov.DateTime;

                    parcel.AccountId = Convert.ToInt32(cboAccount.SelectedValue);
                    parcelsManager.Update(parcel, parcel);
                }
        }
        grdAccountRegister.DataBind();
        CalculateBalance();
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (ucCurrFieldBalance.Text == "0,00")
        {
            litErrorMessage.Visible = true;
            litErrorMessage.Text = "<font color='red'> Saldo a ser ajustado não pode ser zero! </font>";
            return;
        }

        var bill = new Bill();
        var parcel = new Parcel();
        var financialManager = new FinancialManager(this);

        bill.CompanyId = Company.CompanyId;
        bill.DocumentType = 3; // document type "Others"
        bill.EntryDate = DateTime.Now;

        parcel.EffectedDate = SqlDateTime.MinValue.Value;
        parcel.DueDate = SqlDateTime.MinValue.Value;
        parcel.OperationDate = SqlDateTime.MinValue.Value;

        bill.Description = "(Conta a pagar através de acerto de saldo)";
        parcel.Amount = ucCurrFieldBalance.CurrencyValue.Value;
        parcel.EffectedAmount = parcel.Amount;

        var parcels = new List<Parcel>();
        parcels.Add(parcel);

        financialManager.Insert(bill, parcels);

        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Operação realizada com sucesso!')", true);
        ucCurrFieldBalance.CurrencyValue = null;
        litErrorMessage.Visible = false;
        CalculateBalance();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ucCurrFieldBalance.Text == "0,00")
        {
            litErrorMessage.Visible = true;
            litErrorMessage.Text = "<font color='red'> Saldo a ser ajustado não pode ser zero! </font>";
            return;
        }


        var invoice = new Invoice();
        var parcel = new Parcel();
        var financialManager = new FinancialManager(this);

        invoice.CompanyId = Company.CompanyId;
        invoice.EntryDate = DateTime.Now;

        parcel.EffectedDate = SqlDateTime.MinValue.Value;
        parcel.DueDate = SqlDateTime.MinValue.Value;
        parcel.OperationDate = SqlDateTime.MinValue.Value;

        invoice.Description = "(Conta a receber através de acerto de saldo)";

        parcel.Amount = ucCurrFieldBalance.CurrencyValue.Value;
        parcel.EffectedAmount = parcel.Amount;

        var parcels = new List<Parcel>();
        parcels.Add(parcel);

        financialManager.Insert(invoice, parcels);

        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Operação realizada com sucesso!')", true);
        ucCurrFieldBalance.CurrencyValue = null;
        litErrorMessage.Visible = false;
        CalculateBalance();
    }
}