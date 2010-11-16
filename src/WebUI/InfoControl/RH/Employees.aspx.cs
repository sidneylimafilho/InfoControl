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
using System.Web.Services;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;


[PermissionRequired("Employee")]
public partial class Company_RH_Employees : Vivina.Erp.SystemFramework.PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });

            rbtStatus.Items.Add(new ListItem("Todos", ""));
            rbtStatus.Items.Add(new ListItem("Ativos", Convert.ToInt32(EmployeeStatus.Active).ToString()));
            rbtStatus.Items.Add(new ListItem("Afastados", Convert.ToInt32(EmployeeStatus.Inactive).ToString()));


            rbtStatus.SelectedIndex = Convert.ToInt32(Page.Customization["rbtStatus"]);
            cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);


           

            cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);


        }

    }

    protected void odsHumanResources_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["initialLetter"] = ucAlphabeticalPagingEmployees.Letter;

        if (!String.IsNullOrEmpty(rbtStatus.SelectedValue))
        {
            if (Convert.ToInt32(rbtStatus.SelectedValue) == (Int32)EmployeeStatus.Active)
                e.InputParameters["employeeStatus"] = EmployeeStatus.Active;
            else
                e.InputParameters["employeeStatus"] = EmployeeStatus.Inactive;
        }

    }

    protected void grdEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='Employee.aspx?EmployeeId=" + e.Row.DataItem.GetPropertyValue("EmployeeId") + "';";
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void grdEmployees_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Response.Redirect("Employee_PersonalData.aspx");
        }
    }

    protected void ucAlphabeticalPagingEmployees_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdEmployees.DataBind();
    
    }

    //protected void grdEmployees_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Context.Items["EmployeeId"] = grdEmployees.DataKeys[grdEmployees.SelectedIndex]["EmployeeId"].ToString();
    //    Server.Transfer("Employee.aspx");
    //}

    protected void odsHumanResources_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting items that are associated with others Tables
        //
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

    protected void btnInsertNewEmployee_Click(object sender, EventArgs e)
    {
        Response.Redirect("Employee_PersonalData.aspx");
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdEmployees.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        Page.Customization["cboPageSize"] = cboPageSize.SelectedIndex;
        grdEmployees.DataBind();

    }
    [WebMethod]
    public static bool DeleteEmployee(int companyId, int employeeId)
    {
        bool result = true;
        using (var humanResourcesManager = new HumanResourcesManager(null))
        {
            try
            {
                humanResourcesManager.DeleteEmployee(companyId, employeeId);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        Page.Customization["rbtStatus"] = rbtStatus.SelectedIndex;
        Page.Customization["cboStatus"] = cboPageSize.SelectedIndex;

        
        grdEmployees.DataBind();
    }

}
