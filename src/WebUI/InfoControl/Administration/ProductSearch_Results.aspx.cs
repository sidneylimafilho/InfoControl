using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl;

public partial class Company_ProductSearch_Results : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Page.ViewState["htProduct"] = Context.Items["HashTable"];

    }
    protected void BusinessManagerDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["ht"] = Page.ViewState["htProduct"];

    }
 
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Server.Transfer("ProductSearch.aspx");
    }

    
    protected void grdProductSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='Product.aspx?ProductId=" + e.Row.DataItem.GetPropertyValue("ProductId").EncryptToHex() + "';";
            if (e.Row.Cells[4].Text.ToLower() == "true")
            {
                e.Row.Cells[4].Text = "Sim";
            }
            else
            {
                e.Row.Cells[4].Text = "Não";
            }
        }
    }
}
