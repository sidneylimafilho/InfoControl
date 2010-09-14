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

public partial class InfoControl_Host_Reports : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("report.aspx");
    }
    protected void grdReports_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Server.Transfer("Report.aspx");
    }
}
