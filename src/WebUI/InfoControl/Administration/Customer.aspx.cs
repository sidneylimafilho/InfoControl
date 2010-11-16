using System;
using System.Web.UI;
using InfoControl.Web.Security;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

[PermissionRequired("Customers")]
public partial class Company_Customer : Vivina.Erp.SystemFramework.PageBase
{
    bool isInserting = false;
    CustomerManager customerManager;
    Customer customer;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //retrieve the customerId from Modal Popup
            if (!String.IsNullOrEmpty(Request["CustomerId"]))
                Context.Items["CustomerId"] = Request["CustomerId"];

            if (Context.Items["CustomerId"] != null)
            {
                Page.ViewState["CustomerId"] = Context.Items["CustomerId"];
            }
        }
    }
}
