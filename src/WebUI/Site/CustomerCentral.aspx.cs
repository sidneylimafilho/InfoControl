using System;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.WebUI.Site;

public partial class Site_CustomerCentral : SitePageBase
{
    private Customer customer;
    private CustomerManager customerManager;

  

    protected void Page_Load(object sender, EventArgs e)
    {
        customerManager = new CustomerManager(this);

        //
        // Verify if the user logged is a customer on the Hostcompany
        //
        if (!String.IsNullOrEmpty(Request["host"]))
            customer = customerManager.GetHostCustomerByLegalEntityProfileId(Company.LegalEntityProfileId);
        else
            customer = customerManager.GetCustomerByUserName(Company.CompanyId, User.Identity.UserName);
    }

    protected void odsServiceOrders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["customerID"] = customer.CustomerId;
    }

    protected void odsCustomerCalls_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["customerId"] = customer.CustomerId;
        e.InputParameters["companyId"] = customer.CompanyId;
        e.InputParameters["dateTimeInterval"] = new DateTimeInterval(DateTime.Now.AddYears(-8), DateTime.Now.AddDays(2));
    }

    protected void odsReceipts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["customerID"] = customer.CustomerId;
    }

    protected void odsSales_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["customerID"] = customer.CustomerId;
    }

    protected void grdCustomerCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] =
                "top.$.lightbox('../InfoControl/CRM/CustomerCall.aspx?ModalPopUp=1&ReadOnly=true&CustomerCallId=" +
                e.Row.DataItem.GetPropertyValue("CustomerCallId").EncryptToHex() + "');";
        }
    }

    protected void grdServiceOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] =
                "top.$.lightbox('../InfoControl/Services/ServiceOrder.aspx?ModalPopUp=1&ReadOnly=true&ServiceOrderId=" +
                e.Row.DataItem.GetPropertyValue("ServiceOrderId").EncryptToHex() + "');";
        }
    }

    protected void grdReceipts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] =
                "top.$.lightbox('../InfoControl/Accounting/Receipt.aspx?ModalPopUp=1&ReadOnly=true&ReceiptId=" +
                e.Row.DataItem.GetPropertyValue("ReceiptId").EncryptToHex() + "');";
        }
    }

    protected void grdSales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] =
                "top.$.lightbox('../InfoControl/POS/SaleViewer.aspx?ModalPopUp=1&ReadOnly=true&SaleId=" +
                e.Row.DataItem.GetPropertyValue("SaleId").EncryptToHex() + "');";
        }
    }
}