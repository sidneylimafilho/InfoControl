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

[PermissionRequired("StockTransfer")]
public partial class Company_POS_StockTransfer : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void odsStockTransfer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Deposit != null)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
            e.InputParameters["DepositId"] = Deposit.DepositId;
        }
    }

    protected void grdRefused_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!Convert.ToBoolean(grdRefused.DataKeys[e.Row.RowIndex]["Refused"]))
            {
                e.Row.Cells[4].Text = "Não";
                (e.Row.FindControl("cboDropType") as DropDownList).Enabled = false;
            }
            else
                e.Row.Cells[4].Text = "Sim";
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        Inventory inventory = new Inventory();
        InventoryMoviment movement = new InventoryMoviment();
        InventoryHistory history = new InventoryHistory();
        InventoryManager manager = new InventoryManager(this);
        DropDownList cboDropType;

        foreach (GridViewRow row in grdRefused.Rows)
        {
            cboDropType = row.FindControl("cboDropType") as DropDownList;
            //
            //if selected value not null or empty, then delete iventory moviment and
            //insert a history
            //
            if (!String.IsNullOrEmpty(cboDropType.SelectedValue))
            {
                int movementId = Convert.ToInt16(grdRefused.DataKeys[row.RowIndex]["InventoryMovementId"]);

                movement = manager.RetrievePendingTransfers(movementId);
                inventory = manager.GetProductInventory(Company.CompanyId, movement.ProductId, movement.DepositId);

                history.CompanyId = Company.CompanyId;
                history.CurrencyRateId = inventory.CurrencyRateId;
                history.DepositId = movement.DepositId;
                history.FiscalNumber = inventory.FiscalNumber;
                history.InventoryDropTypeId = Convert.ToInt16(cboDropType.SelectedValue);
                history.Localization = inventory.Localization;
                history.LogDate = DateTime.Now;
                history.MinimumRequired = inventory.MinimumRequired;
                history.ProductId = movement.ProductId;
                history.Profit = inventory.Profit;
                history.Quantity = movement.Quantity;
                history.RealCost = inventory.RealCost;
                history.SupplierId = inventory.SupplierId;
                history.UnitPrice = inventory.UnitPrice;
                history.UserId = User.Identity.UserId;

                manager.InsertHistory(history);
                manager.DeleteMovement(movement);

                grdRefused.DataBind();
            }
        }
    }
}
