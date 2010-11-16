using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.DataClasses;

public partial class Company_Administration_CustomerCalls : Vivina.Erp.SystemFramework.PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["CustomerId"]))
                Page.ViewState["CustomerId"] = Request["CustomerId"];
        }
    }

    protected void odsCustomerCalls_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["dateTimeInterval"] = new DateTimeInterval(DateTime.Now.Sql2005MinValue(), DateTime.MaxValue);
        e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);
    }

    protected void grdCustomerCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes["onclick"] = "top.$.lightbox('CRM/CustomerCall.aspx?CustomerCallId=" + grdCustomerCalls.DataKeys[e.Row.RowIndex]["CustomerCallId"] + "&lightbox[iframe]=true' );return;";

    }
}
