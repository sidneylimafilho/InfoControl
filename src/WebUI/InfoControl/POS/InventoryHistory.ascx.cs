using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Vivina.Erp.BusinessRules;

public partial class InfoControl_POS_InventoryHistory : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void odsInventoryHistory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["productId"] = Session["ProductId"];
        e.InputParameters["depositId"] = Session["DepositId"];
    }
    protected void grdInventotyHistory_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void grdInventotyHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if InventoryDropTypeId is sell then add the onClick event 
            if (grdInventotyHistory.DataKeys[e.Row.RowIndex]["InventoryDropTypeId"] != DBNull.Value)
            {
                if (Convert.ToInt32(grdInventotyHistory.DataKeys[e.Row.RowIndex]["InventoryDropTypeId"]) == (int)DropType.Sell)
                {
                    PostBackOptions postOptions = new PostBackOptions((Control)sender, "Select$" + e.Row.RowIndex.ToString());
                    String selectScript = Page.ClientScript.GetPostBackEventReference(postOptions);

                    //
                    // This for not uses equal, to not overbound the index
                    //
                    for (int a = 0; a < e.Row.Cells.Count; a++)
                    {
                        e.Row.Cells[a].Attributes.Add("onclick", selectScript);
                    }
                }
            }
            //if (e.Row.RowIndex != 0)
            //    e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            //e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }


    }
    protected void odsInventoryHistory_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void grdInventotyHistory_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
    }
    protected void odsInventoryHistory_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Server.Transfer("inventories.aspx");
    }
    protected void odsInventoryHistory_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }
    protected void grdInventotyHistory_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //Context.Items["SaleId"] = grdInventotyHistory.DataKeys[e.NewSelectedIndex]["SaleId"];
        //if (Context.Items["SaleId"] != null)
        //    Server.Transfer("SaleViewer.aspx");
    }
}
