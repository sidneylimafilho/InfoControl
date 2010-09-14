using System;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

public partial class Company_RH_Employee_History : Vivina.Erp.SystemFramework.PageBase
{
    int employeeId;
    protected void Page_Load(object sender, EventArgs e)
    {
        employeeId = Convert.ToInt32(Request["eid"].DecryptFromHex());
        grdEmployeeStatusHistory.DataBind();
    }

    #region STATUS
    protected void odsHistory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["employeeId"] = employeeId;
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void employeeHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
    }
    protected void grdEmployeeStatusHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var humanResourcesManager = new HumanResourcesManager(this);
        humanResourcesManager.DeleteStatusHistory(
            humanResourcesManager.GetEmployeeStatusHistory(
            Convert.ToInt16(grdEmployeeStatusHistory.DataKeys[e.RowIndex]["StatusHistoryId"])));
        e.Cancel = true;
        grdEmployeeStatusHistory.DataBind();
    }
    #endregion
}
