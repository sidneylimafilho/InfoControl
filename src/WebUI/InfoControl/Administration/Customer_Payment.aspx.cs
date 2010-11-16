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
using InfoControl;

public partial class Company_Administration_Customer_Payment : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["CustomerId"]))
            Page.ViewState["CustomerId"] = Request["CustomerId"];
    }
    protected void odsCustomerPayment_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CustomerId"] = Convert.ToInt16(Page.ViewState["CustomerId"]);
        e.InputParameters["CompanyId"] = Company.CompanyId;
        if (rbtClosed.Checked)
            e.InputParameters["Closed"] = true;
        else
            e.InputParameters["Closed"] = false;
    }
    protected void rbtClosed_CheckedChanged(object sender, EventArgs e)
    {
        grdPayment.DataBind();
    }
    protected void rbtOpen_CheckedChanged(object sender, EventArgs e)
    {
        grdPayment.DataBind();
    }
}
