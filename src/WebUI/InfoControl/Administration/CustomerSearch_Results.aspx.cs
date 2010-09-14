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

public partial class InfoControl_Administration_CustomerSearch_Result : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Context.Items["htCustomer"] != null)
                Page.ViewState["htCustomer"] = Context.Items["htCustomer"];
        }
    }
   
    protected void odsSearchCustomers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["htCustomer"] = Page.ViewState["htCustomer"];
    }

    protected void grdSearchCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["CustomerId"] = grdSearchCustomers.DataKeys[grdSearchCustomers.SelectedIndex]["CustomerId"].ToString();
        Server.Transfer("Customer.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Customer_Search.aspx");
    }
}
