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

using InfoControl.Data;
using InfoControl.Web;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Security;

[PermissionRequired("StockTransfer")]
public partial class Company_POS_StockMovement_IN : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void odsStockCompany_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["userId"] = User.Identity.UserId;
    }

    protected void odsStockTransfer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Deposit != null)
        {
            e.InputParameters["companyId"] = Convert.ToInt32(cboCompany.SelectedValue);
            e.InputParameters["destinationCompanyId"] = Company.CompanyId;
            e.InputParameters["destinationDepositId"] = Deposit.DepositId;
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdReceive.DataBind();
    }

    protected void btnReceive_Click(object sender, EventArgs e)
    {
        InventoryManager invManager = new InventoryManager(this);
        InventoryMoviment movement;

        foreach (GridViewRow row in grdReceive.Rows)
        {
            movement = new InventoryMoviment();
            int movementId = Convert.ToInt16(grdReceive.DataKeys[row.RowIndex]["InventoryMovementId"]);
            movement = invManager.RetrievePendingTransfers(movementId);
            invManager.TransferStockDeposit(movement, User.Identity.UserId);
        }

        cboCompany.DataBind();
        grdReceive.DataBind();
    }

    protected void grdReceive_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        InventoryManager invManager = new InventoryManager(this);
        InventoryMoviment movement = new InventoryMoviment();

        int movementId = Convert.ToInt16(grdReceive.DataKeys[e.RowIndex]["InventoryMovementId"]);
        int destinationCompanyId = Convert.ToInt16(grdReceive.DataKeys[e.RowIndex]["CompanyDestinationId"]);

        invManager.RefuseMovement(movementId);

        grdReceive.DataBind();
        e.Cancel = true;
    }
}
