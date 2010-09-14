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

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class Accounting_Checks : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void odsChecks_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void grdChecks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }
    protected void grdChecks_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("Check.aspx");
        }
    }
    protected void grdChecks_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["CheckId"] = grdChecks.DataKeys[grdChecks.SelectedIndex]["CheckId"].ToString();
        Server.Transfer("Check.aspx");
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("check.aspx");
    }
}
