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

public partial class Host_PaymentMethods : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void grdPaymentMethod_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }
    protected void grdPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["PaymentMethodId"] = grdPaymentMethod.DataKeys[grdPaymentMethod.SelectedIndex]["PaymentMethodId"].ToString();
        Server.Transfer("PaymentMethod.aspx");
    }
    protected void grdPaymentMethod_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("PaymentMethod.aspx");
        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("PaymentMethod.aspx");
    }
}
