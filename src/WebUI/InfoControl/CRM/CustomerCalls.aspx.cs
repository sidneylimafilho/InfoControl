using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Services;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


[PermissionRequired("CustomerCalls")]
public partial class InfoControl_CRM_CustomerCalls : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucDateTimeInterval.DateInterval = new DateTimeInterval(DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(1));

            cboTechnicalUser.DataBind();

            //
            // select user in combo if user logged is technical user
            //
            var humanResourcesManager = new HumanResourcesManager(this);
            Employee employee = humanResourcesManager.GetEmployeeByProfile((Int32)User.Identity.ProfileId, Company.CompanyId);

            if (employee != null)
            {
                if (cboTechnicalUser.Items.FindByValue(User.Identity.UserId.ToString()) != null)
                    cboTechnicalUser.SelectedValue = User.Identity.UserId.ToString();
            }

            if (Page.Customization["cboCustomerCallType"] != null)
                cboCustomerCallType.SelectedIndex = Convert.ToInt32(Page.Customization["cboCustomerCallType"]);

            if (Page.Customization["cboStatus"] != null)
                cboStatus.SelectedIndex = Convert.ToInt32(Page.Customization["cboStatus"]);

            if (Page.Customization["cboPageSize"] != null)
                cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);

            if (Page.Customization["cboTechnicalUser"] != null && cboTechnicalUser.Items.FindByValue(Convert.ToString(Page.Customization["cboTechnicalUser"])) != null)
                cboTechnicalUser.SelectedValue = Convert.ToString(Page.Customization["cboTechnicalUser"]);
        }
    }

    protected void odsCustomerCalls_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //switch (Convert.ToInt32(cboCustomerCallType.SelectedValue))
        //{
        //    case 0:
        //        odsCustomerCalls.SelectMethod = "GetCustomerCalls";
        //        odsCustomerCalls.SelectCountMethod = "GetCustomerCallsCount";
        //        break;
        //   case CustomerCallType.ACCOLADE:
        //        odsCustomerCalls.SelectMethod = "GetAccoladeCustomerCalls";
        //        odsCustomerCalls.SelectCountMethod = "GetAccoladeCustomerCallsCount";
        //        break;
        //    case CustomerCallType.COMPLAINT:
        //        odsCustomerCalls.SelectMethod = "GetComplaintCustomerCalls";
        //        odsCustomerCalls.SelectCountMethod = "GetComplaintCustomerCallsCount";
        //        break;
        //    case CustomerCallType.ERROR:
        //        odsCustomerCalls.SelectMethod = "GetErrorCustomerCalls";
        //        odsCustomerCalls.SelectCountMethod = "GetErrorCustomerCallsCount";
        //        break;
        //    case CustomerCallType.SUGESTION:
        //        odsCustomerCalls.SelectMethod = "GetSugestionCustomerCalls";
        //        odsCustomerCalls.SelectCountMethod = "GetSugestionCustomerCallsCount";
        //        break;
        //    case CustomerCallType.SUPPORT:
        //        odsCustomerCalls.SelectMethod = "GetSupportCustomerCalls";
        //        odsCustomerCalls.SelectCountMethod = "GetSupportCustomerCallsCount";
        //        break;
        //}

        e.InputParameters["companyId"] = Company.CompanyId;

        e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;

        if (Page.ViewState["CustomerId"] != null)
            e.InputParameters["customerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);

        if(!String.IsNullOrEmpty(cboCustomerCallType.SelectedValue))
            e.InputParameters["customerCallType"] = Convert.ToInt32(cboCustomerCallType.SelectedValue);

        if (String.IsNullOrEmpty(cboStatus.SelectedValue))
            e.InputParameters["customerCallStatusId"] = null;
        else
            e.InputParameters["customerCallStatusId"] = Convert.ToInt32(cboStatus.SelectedValue);

        //
        //filter by technical user
        //
        if (cboTechnicalUser.SelectedIndex > 0)
            e.InputParameters["technicalEmployeeId"] = Convert.ToInt32(cboTechnicalUser.SelectedValue);
    }

    protected void grdCustomerCalls_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Server.Transfer("CustomerCall.aspx");

        Page.Customization["sortExpression"] = e.SortExpression;
        Page.Customization["sortDirection"] = e.SortDirection;
    }

    protected void grdCustomerCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            //paint line of red if event type equals Error and visualization mode equals All Events
            //
            e.Row.Attributes["onclick"] = "location='CustomerCall.aspx?CustomerCallId=" + e.Row.DataItem.GetPropertyValue("CustomerCallId").EncryptToHex() + "';";
            if ((Convert.ToInt32(grdCustomerCalls.DataKeys[e.Row.RowIndex]["CustomerCallTypeId"]) == CustomerCallType.ERROR))
                e.Row.ForeColor = Color.Red;

            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("CustomerCall.aspx");
    }

    protected void odsCustomerCalls_Deleting(object sender, ObjectDataSourceMethodEventArgs e) { }

    protected void odsTechnicalUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        Int32 pageSize = 0;
        if (cboPageSize.SelectedValue.Contains("Todos"))
            pageSize = Int32.MaxValue;
        else
            pageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        grdCustomerCalls.PageSize = pageSize;

        Page.Customization["cboCustomerCallType"] = cboCustomerCallType.SelectedIndex;
        Page.Customization["cboStatus"] = cboStatus.SelectedIndex;
        Page.Customization["cboTechnicalUser"] = cboTechnicalUser.SelectedValue;
        Page.Customization["cboPageSize"] = cboPageSize.SelectedIndex;

        grdCustomerCalls.DataBind();
    }


    protected void selCustomer_OnSelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        Page.ViewState["CustomerId"] = e.Customer.CustomerId;
    }

    protected void odsCustomerCalls_Deleted(object sender, ObjectDataSourceStatusEventArgs e) { }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdCustomerCalls.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdCustomerCalls.DataBind();
    }

    [WebMethod]
    public static bool CloseCustomerCall(int customerCallId)
    {
        bool result = true;
        using (var customerManager = new CustomerManager(null))
        {
            try
            {
                customerManager.CloseCustomerCall(customerCallId);
            }
            catch
            {
                result = false;
            }
        }
        return result;
    }
}