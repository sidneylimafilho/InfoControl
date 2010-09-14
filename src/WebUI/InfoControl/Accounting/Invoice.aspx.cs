using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;



using InfoControl.Web.Security;

[PermissionRequired("Invoices")]
public partial class Accouting_Invoice : Vivina.Erp.SystemFramework.PageBase
{
    FinancialManager financialManager;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            pnlInvoiceSource.Visible = false;

            if (Request["InvoiceId"] != null)
                Context.Items["InvoiceId"] = Request["InvoiceId"].DecryptFromHex();

            if (Context.Items["InvoiceId"] != null)
                Page.ViewState["InvoiceId"] = Context.Items["InvoiceId"];

            ucParcels.Clear();

            if (Page.ViewState["InvoiceId"] != null)
                showInvoice(Convert.ToInt32(Page.ViewState["InvoiceId"]));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ucParcels.DataSource.Any())
        {
            ShowError(Resources.Exception.NonExistentParcel);
            return;
        }

        SaveInvoice();
        if (((WebControl)sender).ID == "btnNew")
            Response.Redirect("Invoice.aspx");
        else
            Response.Redirect("Invoices.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Invoices.aspx");
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            Page.ViewState["CustomerId"] = e.Customer.CustomerId;
        }
    }

    /// <summary>
    /// this region contains all DataSource selecting
    /// </summary>
    #region DataSource
    protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsInvoiceModels_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["documentTemplateTypeId"] = Convert.ToInt32(DocumentTemplateTypes.Invoice);
    }

    #endregion

    /// <summary>
    /// this region contains all functions
    /// </summary>
    #region functions
    /// <summary>
    /// this function show the Invoice
    /// </summary>
    protected void showInvoice(Int32 invoiceId)
    {
        financialManager = new FinancialManager(this);
        Invoice invoice = financialManager.GetInvoice(Company.CompanyId, invoiceId);
        Contract contract;

        txtSource.Text = invoice.Description;

        if (invoice.AccountingPlanId != null)
            cboAccountPlan.SelectedValue = invoice.AccountingPlanId.ToString();
        if (invoice.CostCenterId != null)
            cboCostCenter.SelectedValue = invoice.CostCenterId.Value.ToString();
        SelCustomer.ShowCustomer(invoice.Customer);
        Sale sale = invoice.Sales.FirstOrDefault();
        if (sale != null)
        {
            lblSale.Visible = true;
            lnkSale.Text = sale.SaleId.ToString();
            lnkSale.NavigateUrl = "../POS/SaleViewer.aspx?SaleId=" + sale.SaleId.EncryptToHex();

            if (sale.ReceiptId.HasValue)
            {
                lblReceipt.Visible = true;
                lnkReceipt.Text = sale.Receipt.ReceiptNumber.ToString();
                lnkReceipt.NavigateUrl = "../Accounting/Receipt.aspx?ReceiptId=" + Convert.ToString(sale.ReceiptId).EncryptToHex();
            }
            pnlInvoiceSource.Visible = true;
        }

        contract = invoice.Contracts.FirstOrDefault();
        if (contract != null)
        {
            lblContract.Visible = true;
            lnkContract.Text = String.IsNullOrEmpty(contract.ContractNumber) ? Convert.ToString(contract.ContractId) : contract.ContractNumber;
            lnkContract.NavigateUrl = "Contract.aspx?ContractId=" + Convert.ToString(contract.ContractId).EncryptToHex();
            pnlInvoiceSource.Visible = true;
        }

        pnlInvoiceSource.Visible = true;
        ucParcels.DataSource = invoice.Parcels.ToList();
    }

    private Invoice SaveInvoice()
    {
        financialManager = new FinancialManager(this);
        Invoice original_invoice = new Invoice();
        Invoice invoice = new Invoice();

        if (Page.ViewState["InvoiceId"] != null)
        {
            original_invoice = financialManager.GetInvoice(Company.CompanyId, Convert.ToInt32(Page.ViewState["InvoiceId"]));
            invoice.CopyPropertiesFrom(original_invoice);
        }

        invoice.Description = txtSource.Text;
        invoice.CompanyId = Company.CompanyId;
        invoice.CostCenterId = Convert.ToInt32(cboCostCenter.SelectedValue);

        if (Page.ViewState["CustomerId"] != null)
            invoice.CustomerId = Convert.ToInt32(Page.ViewState["CustomerId"]);

        //accountPlan
        if (!String.IsNullOrEmpty(cboAccountPlan.SelectedValue))
            invoice.AccountingPlanId = Convert.ToInt32(cboAccountPlan.SelectedValue);

        if (Page.ViewState["InvoiceId"] != null)
        {
            invoice.ModifiedByUser = User.Identity.UserName;
            financialManager.Update(original_invoice, invoice, ucParcels.DataSource);
        }
        else
        {
            invoice.CreatedByUser = User.Identity.UserName;
            financialManager.Insert(invoice, ucParcels.DataSource);
        }


        if (Page.ViewState["InvoiceId"] != null)
            return original_invoice;
        else
            return invoice;
    }
    #endregion

    protected void btnNew_Click(object sender, EventArgs e)
    {
        btnSave_Click(sender, e);
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {

        if (!ucParcels.DataSource.Any())
        {
            ShowError(Resources.Exception.NonExistentParcel);
            return;
        }

        if (String.IsNullOrEmpty(cboInvoiceModel.SelectedValue))
        {
            ShowError("Não há modelo de duplicata para essa empresa!");
            return;
        }

        if (String.IsNullOrEmpty(Request.Form["parcelId"]))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Selecione uma parcela para gerar o documento!')", true);
            return;
        }
        var invoice = SaveInvoice();

        ExportInvoiceDocument(invoice, Convert.ToInt32(Request.Form["parcelId"]), "Duplicata-" + invoice.Description, Convert.ToInt32(cboInvoiceModel.SelectedValue));
    }

    protected void btnBoleto_Click(object sender, EventArgs e)
    {

        if (String.IsNullOrEmpty(Request.Form["parcelId"]))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Selecione uma parcela para gerar boleto!');", true);
            return;
        }

        SaveInvoice();
        ClientScript.RegisterClientScriptBlock(this.GetType(), "", " top.tb_show('Gerar Boleto', '../../InfoControl/Site/Boleto.aspx?parcelId=" + Request.Form["parcelId"] + "');", true);
    }

    private void ExportInvoiceDocument(Invoice invoice, Int32 parcelId, string fileName, Int32 documentTemplateId)
    {
        var financialManager = new FinancialManager(this);

        //
        // Clean buffers of response to not send headers of ASPX pages
        // 
        Response.Clear();
        Response.ContentType = "text/doc";
        //
        // Sets the header that tells to browser to download not show in the screen
        // 
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".rtf");
        //
        // Indicate that file format will be the same model
        // 
        Response.ContentEncoding = System.Text.Encoding.Default;

        //
        // Apply the changes from model, changing []'s for the content
        // 
        Response.Write(financialManager.ApplyInvoiceTemplate(invoice, parcelId, documentTemplateId));

        //
        // Cut the page process to not merge the model content to the page HTML
        // 
        Response.End();
    }

    protected void odsFinancierOperation_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
