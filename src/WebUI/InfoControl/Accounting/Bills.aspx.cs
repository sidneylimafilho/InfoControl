using System;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using System.Data.Linq;
using System.Linq;
using Telerik.Web.UI;
using InfoControl.Web.Security;


[PermissionRequired("Bills")]
public partial class Accounting_Bills : Vivina.Erp.SystemFramework.PageBase
{
    #region events

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Response.Redirect("Bill.aspx");
    }

    protected void grdBill_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Response.Redirect("Bill.aspx");
    }

    protected void grdBill_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick" && e.Item is GridDataItem)
        {
            Context.Items["BillId"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BillId"];
            Response.Redirect("Bill.aspx");
        }
        else if (e.CommandName == "Delete" && e.Item.ItemType != GridItemType.GroupFooter)
        {
            var financialManager = new FinancialManager(this);
            if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BillId"] != null)
            {
                var bill = financialManager.GetBill(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BillId"]), Company.CompanyId);

                if (bill.ExpenditureAuthorizations.Any())
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('O registro não pode ser apagado pois há outros registros de autorização de despesas associados!')", true);
                    return;
                }

                financialManager.DeleteBill(Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BillId"]), Company.CompanyId);

            }
            grdBill.DataBind();
            if (grdBill.Items.Count == 0)
                grdBill.ShowFooter = false;
        }
    }

    protected void grdBill_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            e.Item.Attributes["onclick"] = "location='Bill.aspx?BillId=" + e.Item.DataItem.GetPropertyValue("BillId") + "';";

        if (e.Item.ItemType == GridItemType.GroupFooter)
        {
            e.Item.Cells[e.Item.Cells.Count - 1].Visible = false;
        }
    }

    protected void odsSearchBills_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["accountSearch"] = ucAccountSearch;
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void SearchAccount_SelectedParameters(object sender, EventArgs e)
    {

        grdBill.ShowFooter = grdBill.MasterTableView.ShowGroupFooter = true;

        grdBill.DataBind();
        if (grdBill.Items.Count == 0)
            grdBill.ShowFooter = grdBill.MasterTableView.ShowGroupFooter = false;
    }

    #endregion

    protected void odsAccountingPlan_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
