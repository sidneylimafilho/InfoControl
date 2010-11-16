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
using InfoControl;
using InfoControl;

public partial class InfoControl_Administration_TransporterSearch_Results : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Context.Items["htTransporter"] != null)
                Page.ViewState["htTransporter"] = Context.Items["htTransporter"];
        }
    }
    protected void odsSearchTransporter_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["htTransporter"] = Page.ViewState["htTransporter"];
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Transporter_Search.aspx");
    }

    protected void grdTransporters_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow)

        e.Row.Attributes["onclick"] = "location='Transporter.aspx?TransporterId=" + e.Row.DataItem.GetPropertyValue("TransporterId") + "';";
    }
   
}
