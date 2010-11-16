using System;
using System.Linq;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

[PermissionRequired("Bills")]
public partial class Accounting_Bill : Vivina.Erp.SystemFramework.PageBase
{
    private Bill _bill;

    public Bill OriginalBill
    {
        get
        {
            if (_bill == null)
                if (Request["BillId"] != null)
                {
                    var financialManager = new FinancialManager(this);
                    _bill = financialManager.GetBill(Convert.ToInt32(Request["BillId"]), Company.CompanyId);
                }
            return _bill ?? (_bill = new Bill());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucParcels.Clear();
            rbtReceipt.Checked = true;
            //cboAccountPlan.DataBind();

            if (Request["BillId"] != null)
            {
                Page.ViewState["BillId"] = Request["BillId"];

                showBill();
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ucParcels.DataSource.Any())
        {
            ShowError(Resources.Exception.NonExistentParcel);
            return;
        }

        SaveBill(sender);
    }

    protected void SelSupplier_SelectedSupplier(object sender, SelectedSupplierEventArgs e)
    {
        if (e.Supplier != null)
            Page.ViewState["SupplierId"] = e.Supplier.SupplierId;
    }

    /// <summary>
    /// this region contains all functions
    /// </summary>

    /// <summary>
    /// this region contains all DataSource selecting
    /// </summary>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Bills.aspx");
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (!ucParcels.DataSource.Any())
        {
            ShowError(Resources.Exception.NonExistentParcel);
            return;
        }

        SaveBill(sender);
    }

    private void SaveBill(object sender)
    {
        var bill = new Bill();

        bill.CopyPropertiesFrom(OriginalBill);

        bill.CompanyId = Company.CompanyId;
        // Numero da Nota fiscal do fornecedor
        bill.DocumentNumber = txtDocumentNumber.Text;
       
        bill.Description = txtDescription.Text;
        bill.CostCenterId = Convert.ToInt32(cboCostCenter.SelectedValue);
        bill.EntryDate = DateTime.Now;



        //supplier
        if (Page.ViewState["SupplierId"] != null)
            bill.SupplierId = Convert.ToInt32(Page.ViewState["SupplierId"]);
        else
            bill.SupplierId = null;

        //accountPlan
        if (!String.IsNullOrEmpty(cboAccountPlan.SelectedValue))
            bill.AccountingPlanId = Convert.ToInt32(cboAccountPlan.SelectedValue);

        //documentType
        if (rbtGuia.Checked)
            bill.DocumentType = (Int32)DocumentType.Guia;
        else if (rbtReceipt.Checked)
            bill.DocumentType = (Int32)DocumentType.receipt;
        else if (rbtOthers.Checked)
            bill.DocumentType = (Int32)DocumentType.others;

        var billManager = new FinancialManager(this);

        //bill.Parcels = ucParcels.DataSource;

        if (Page.ViewState["BillId"] != null)
        {
            bill.ModifiedByUser = User.Identity.UserName;
            billManager.Update(OriginalBill, bill, ucParcels.DataSource);
        }
        else
        {
            bill.CreatedByUser = User.Identity.UserName;
            billManager.Insert(bill, ucParcels.DataSource);
            Context.Items["PostBack"] = Context.Items["BillId"] = bill.BillId;
        }
        ucParcels.Clear();

        if ((sender as Button).ID == "btnNew")
            Response.Redirect("Bill.aspx");
        else
            Response.Redirect("Bills.aspx");
    }

    #region functions

    /// <summary>
    /// this function show the bill
    /// </summary>
    protected void showBill()
    {
        txtDescription.Text = OriginalBill.Description;
        txtDocumentNumber.Text = OriginalBill.DocumentNumber;
        //ucCurrFieldBillValue.CurrencyValue = OriginalBill.BillValue;

        if (OriginalBill.AccountingPlanId.HasValue)
            cboAccountPlan.SelectedValue = OriginalBill.AccountingPlanId.Value.ToString();
        if (OriginalBill.CostCenterId.HasValue)
            cboCostCenter.SelectedValue = OriginalBill.CostCenterId.Value.ToString();

        //document Type
        if (OriginalBill.DocumentType == (int)DocumentType.Guia)
            rbtGuia.Checked = true;
        else if (OriginalBill.DocumentType == (int)DocumentType.receipt)
            rbtReceipt.Checked = true;
        else if (OriginalBill.DocumentType == (int)DocumentType.others)
            rbtOthers.Checked = true;

        selSupplier.ShowSupplier(OriginalBill.Supplier);
        ucParcels.DataSource = OriginalBill.Parcels.ToList();
    }

    #endregion

    #region DataSource

    protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    #endregion
}