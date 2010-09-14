using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;

[PermissionRequired("RMA")]
public partial class Company_POS_StockRMASupplier : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void odsRMA_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["depositId"] = Deposit.DepositId;
    }

    protected void grdRefused_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        InventoryManager inventoryManager = new InventoryManager(this);
        int RmaId = Convert.ToInt16(grdRefused.DataKeys[e.RowIndex]["InventoryRMAId"]);
        inventoryManager.DeleteRMA(RmaId);
        grdRefused.DataBind();
        e.Cancel = true;
    }
}
