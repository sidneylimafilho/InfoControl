using System;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

public partial class Company_POS_SaleViewer : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["ReadOnly"]))
        {
            grdSaleItems.Enabled = grdPaymentMethod.Enabled = false;
            btnCancelSale.Visible = btnGenerateFiscalSale.Visible = false;
        }

        if (!IsPostBack)
            if (!String.IsNullOrEmpty(Request["SaleId"]))
            {
                Page.ViewState["SaleId"] = Request["SaleId"].DecryptFromHex();
                showSale(Convert.ToInt32(Page.ViewState["SaleId"]));
                HidrateCustomer(Convert.ToInt32(Page.ViewState["SaleId"]));
            }
    }

    private void HidrateCustomer(int saleId)
    {
        //
        // Show the customer
        // 
        var saleManager = new SaleManager(this);
        Sale sale = saleManager.GetSale(Company.CompanyId, saleId);
        Customer customer = sale.Customer;

        if (customer != null)
        {
            sel_customer.ShowCustomer(customer);
        }
    }

    /// <summary>
    /// this method show all data of the Sale
    /// </summary>
    /// <param name="saleId"></param>
    private void showSale(int saleId)
    {
        var saleManager = new SaleManager(this);
        var sale = saleManager.GetSale(Company.CompanyId, saleId) ?? new Sale();
        lblSaleNumber.Text = saleId.ToString();

        btnGenerateFiscalSale.OnClientClick = "location='../Accounting/Receipt.aspx?SaleId=" + Convert.ToString(sale.SaleId).EncryptToHex() + "'; return false;";
        btnGenerateFiscalSale.UseSubmitBehavior = false;

        if (sale.IsCanceled)
        {
            btnCancelSale.Visible = false;
            btnGenerateFiscalSale.Visible = false;
        }

        if (sale.SaleDate != null)
            lblSaleDate.Text = sale.SaleDate.Value.ToShortDateString();


        lblSaleStatus.Text = sale.SaleStatus.Name;

        if (sale.ReceiptId != null)
        {
            lnkReceipt.Text = sale.Receipt.ReceiptNumber.ToString();
            lnkReceipt.NavigateUrl = "../Accounting/Receipt.aspx?ReceiptId=" + Convert.ToString(sale.ReceiptId).EncryptToHex();

            if (sale.Receipt.IssueDate.HasValue)
                lblReceiptDate.Text = sale.Receipt.IssueDate.Value.ToShortDateString();
            else
                lblReceiptDate.Text = "";
        }
    }

    protected void odsSaleItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = (int)Company.MatrixId;
        e.InputParameters["saleId"] = Convert.ToInt32(Page.ViewState["SaleId"]);
    }

    protected void btnCancelSale_Click(object sender, EventArgs e)
    {
        var saleManager = new SaleManager(this);
        saleManager.CancelSale(Convert.ToInt32(Page.ViewState["SaleId"]), (int)Company.MatrixId, User.Identity.UserId);

        Response.Redirect("SaleList.aspx");


    }

    protected void odsCustomer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) { }

    protected void odsSaleData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["saleId"] = Convert.ToInt32(Page.ViewState["SaleId"]);
    }

    protected void grdSaleItems_RowDataBound(object sender, GridViewRowEventArgs e) { }

    protected void odsReceipt_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Convert.ToInt32(Company.CompanyId);
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        var saleManager = new SaleManager(this);
        if (e.Customer != null)
        {
            Sale sale = saleManager.GetSale(Company.CompanyId, Convert.ToInt32(Page.ViewState["SaleId"]));
            sale.CustomerId = e.Customer.CustomerId;
            saleManager.Update(saleManager.GetSale(Company.CompanyId, Convert.ToInt32(Page.ViewState["SaleId"])), sale);
        }
    }

    protected void odsPaymentMethod_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        var saleManager = new SaleManager(this);

        Sale sale = saleManager.GetSale(Company.MatrixId.Value, Convert.ToInt32(Page.ViewState["SaleId"]));

        if (sale.InvoiceId.HasValue)
        {
            e.InputParameters["companyId"] = sale.CompanyId;
            e.InputParameters["saleId"] = sale.SaleId;
        }
        else
            e.Cancel = true;

    }
}