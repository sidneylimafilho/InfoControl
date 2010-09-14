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

public partial class InfoControl_Administration_SupplierSearch_Results : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Context.Items["htSupplier"] != null)
                Page.ViewState["htSupplier"] = Context.Items["htSupplier"];
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Supplier_Search.aspx");
    }
    protected void odsSearchSupplier_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["htSupplier"] = Page.ViewState["htSupplier"];
    }
    protected void grvSearchSuppliers_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["SupplierId"] = grvSearchSuppliers.DataKeys[grvSearchSuppliers.SelectedIndex]["SupplierId"].ToString();
        Server.Transfer("Supplier.aspx");
    }
}
