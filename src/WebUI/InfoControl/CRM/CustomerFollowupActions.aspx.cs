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
using System.Web.Services;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("CustomerFollowupActions")]
public partial class InfoControl_CRM_CustomerFollowupActions : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
        }

    }
    protected void grdCustomerFollowupAction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='CustomerFollowupAction.aspx?CustomerFollowupActionId=" + e.Row.DataItem.GetPropertyValue("CustomerFollowupActionId").EncryptToHex() + "';";

            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }
    protected void grdCustomerFollowupAction_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("CustomerFollowupAction.aspx");
        }
    }

    protected void grdCustomerFollowupAction_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void odsCustomerFollowupAction_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }




    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("CustomerFollowupAction.aspx");

    }
    protected void grdCustomerFollowupAction_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    e.ExceptionHandled = true;
                }
            }
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdCustomerFollowupAction.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdCustomerFollowupAction.DataBind();

    }

    [WebMethod]
    public static bool DeleteCustomerFollowupAction(int customerFollowupActionId)
    {
        bool result = true;
        using (CustomerManager customerManager = new CustomerManager(null))
        {
            try
            {
                customerManager.DeleteCustomerFollowUpAction(customerManager.GetCustomerFollowupAction(customerFollowupActionId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }
}
