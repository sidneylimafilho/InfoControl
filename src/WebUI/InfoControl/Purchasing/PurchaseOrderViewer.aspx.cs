using System;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

public partial class InfoControl_POS_PurchaseOrderViewer : Vivina.Erp.SystemFramework.PageBase
{
    private PurchaseOrder purchaseOrder;
    private PurchaseOrderManager purchaseOrderManager;

    private string FormatDouble(TextBox Double)
    {
        //
        // This method fixes the bug that happens on .NET with 2 diferents cultures
        // The data was inconsistante, 'cause the en-US culture uses the "." (dot) to separate decimal
        // values, when the pt-BR culture uses the "," (coma).
        // The replace for the "_" (underline) character is to fix the mask extender problem. If the focus
        // are in the control that have a mask, that control send a "_" to the database.
        //
        Double.Text = Double.Text.Replace("_", "0");
        Double.Text = Double.Text.Replace(".", "");
        Double.Text = Double.Text.Replace(",", ".");
        return Double.Text;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Context.Items["PurchaseOrderId"] != null)
            {
                Page.ViewState["PurchaseOrderId"] = Context.Items["PurchaseOrderId"];
                purchaseOrderManager = new PurchaseOrderManager(this);
                purchaseOrder = purchaseOrderManager.GetPurchaseOrder(
                    Convert.ToInt32(Page.ViewState["PurchaseOrderId"]), Company.CompanyId);
                lblPurchaseOrderCode.Text = "Código: " + purchaseOrder.PurchaseOrderCode;

                if (purchaseOrder.SupplierId != null)
                {
                    var supplierManager = new SupplierManager(this);
                    Supplier supplier = supplierManager.GetSupplier(Convert.ToInt32(purchaseOrder.SupplierId),
                                                                    Company.CompanyId);
                    selSupplier.ShowSupplier(supplier);
                }
            }
        }
    }

    protected void odsPurchaseOrderItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["PurchaseOrderId"] = Convert.ToInt32(Page.ViewState["PurchaseOrderId"]);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("PurchaseOrders.aspx");
    }

    protected void grdPurchaseOrderItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdPurchaseOrderItems.EditIndex = grdPurchaseOrderItems.SelectedIndex;
    }

    protected void selSupplier_SelectedSupplier(object sender, SelectedSupplierEventArgs e)
    {
        if (e.Supplier != null)
            Page.ViewState["SupplierId"] = e.Supplier.SupplierId;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.ViewState["SupplierId"] != null)
        {
            PurchaseOrder original_PurchaseOrder;
            var modified_PurchaseOrder = new PurchaseOrder();
            purchaseOrderManager = new PurchaseOrderManager(this);

            original_PurchaseOrder =
                purchaseOrderManager.GetPurchaseOrder(Convert.ToInt32(Page.ViewState["PurchaseOrderId"]),
                                                      Company.CompanyId);
            modified_PurchaseOrder.CopyPropertiesFrom(original_PurchaseOrder);
            modified_PurchaseOrder.SupplierId = Convert.ToInt32(Page.ViewState["SupplierId"]);

            purchaseOrderManager.Update(original_PurchaseOrder, modified_PurchaseOrder);
        }
        Response.Redirect("PurchaseOrders.aspx");
    }

    protected void grdPurchaseOrderItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        var txtPraidePaid = (grdPurchaseOrderItems.Rows[e.RowIndex].FindControl("txtPricePaid") as TextBox);

        if (!String.IsNullOrEmpty(txtPraidePaid.Text))
            e.NewValues["PricePaid"] = Convert.ToDecimal(txtPraidePaid.Text);
        else
            e.NewValues["PricePaid"] = null;
    }
}