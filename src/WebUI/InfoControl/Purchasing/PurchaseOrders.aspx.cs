using System;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


public partial class InfoControl_POS_PurchaseOrders : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            cboPageSize.Items.Add(new ListItem { Value = Int16.MaxValue.ToString(), Text = "Todos" });
    }

    protected void odsPurchaseOrders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["status"] = Convert.ToInt32(cboPurchaseOrderStatus.SelectedValue);
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void grdPurchaseOrders_Sorting(object sender, GridViewSortEventArgs e)
    {
    }

    protected void odsPurchaseOrders_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {

    }

    protected void btnNewPurchaseOrder_Click(object sender, EventArgs e)
    {
        Server.Transfer("PurchaseOrder.aspx");
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdPurchaseOrders.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdPurchaseOrders.DataBind();
    }

    protected void grdPurchaseOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='PurchaseOrder.aspx?pid=" +
                                          e.Row.DataItem.GetPropertyValue("PurchaseOrderId").EncryptToHex() + "';";
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        grdPurchaseOrders.DataBind();
    }

    private void InitializeComponent()
    {

    }

    [WebMethod]
    public static bool DeletePurchaseOrder(int purchaseOrderId, int companyId)
    {
        bool result = true;
        using (var purchaseOrderManager = new PurchaseOrderManager(null))
        {
            try
            {
                purchaseOrderManager.DeletePurchaseOrder(purchaseOrderId);
            }
            catch (Exception e)
            {
                result = false;
            }
        }
        return result;
    }
}