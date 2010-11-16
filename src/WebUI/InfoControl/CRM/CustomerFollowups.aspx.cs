using System;
using System.Web.UI.WebControls;
using System.Web.Services;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("CustomerFollowups")]
public partial class InfoControl_CRM_CustomerFollowups : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucDateTimeInterval.DateInterval = (DateTimeInterval)Page.Customization["dateTimeInterval"];
            txtContactName.Text = Convert.ToString(Page.Customization["contactName"]);

            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
        }
    }

    protected void grdCustomerFollowup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='CustomerFollowup.aspx?CustomerFollowupId=" + grdCustomerFollowup.DataKeys[e.Row.RowIndex]["CustomerFollowupId"] + "';";
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }



    protected void odsCustomerFollowup_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;

        e.InputParameters["contactName"] = txtContactName.Text;

        if (!String.IsNullOrEmpty(cboCustomerFollowupAction.SelectedValue))
            e.InputParameters["customerFollowupActionId"] = cboCustomerFollowupAction.SelectedValue;
        else
            e.InputParameters["customerFollowupActionId"] = null;

        if (ucDateTimeInterval != null)
            e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
    }

    protected void odsCustomerFollowupAction_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["dateTimeInterval"] = ucDateTimeInterval.DateInterval;
        Page.Customization["contactName"] = txtContactName.Text;

        grdCustomerFollowup.DataBind();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdCustomerFollowup.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdCustomerFollowup.DataBind();

    }

    [WebMethod]
    public static void DeleteCustomerFollowup(int customerFollowupId)
    {
        using (CustomerManager customerManager = new CustomerManager(null))
        {
            var taskManager = new TaskManager(null);
            var task = taskManager.GetTask(customerFollowupId, "CustomerFollowUp.aspx");

            if (task != null)
                taskManager.DeleteTask(task);

            customerManager.DeleteCustomerFollowup(customerManager.GetCustomerFollowup(customerFollowupId));
        }
    }
}
