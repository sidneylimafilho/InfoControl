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
using System.Xml.Linq;

public partial class InfoControl_Host_SiteTemplates : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("SiteTemplate.aspx");
    }
    protected void grdSiteTemplates_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        Context.Items["SiteTemplateId"] = grdSiteTemplates.DataKeys[e.NewSelectedIndex]["SiteTemplateId"];
        Server.Transfer("SiteTemplate.aspx");
    }
    protected void grdSiteTemplates_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Server.Transfer("SiteTemplate.aspx");
    }
    protected void grdSiteTemplates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //
        //Verify if the row is a data row, to not get header and footer
        //
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }
}
