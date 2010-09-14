using System;
using System.Web.Services;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("FinancierOperation")]
public partial class InfoControl_Accounting_FinancierOperations : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void grdFinancierOperations_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Response.Redirect("FinancierOperation.aspx");
    }
    protected void grdFinancierOperations_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

        Response.Redirect("FinancierOperation.aspx?FinancierOperationId=" + Convert.ToString(grdFinancierOperations.DataKeys[e.NewSelectedIndex]["FinancierOperationId"]).EncryptToHex());
    }

    [WebMethod]
    public static bool DeleteFinancierOperation(Int32 companyId, int financierOperationId)
    {
        bool result = true;
        using (AccountManager accountManager = new AccountManager(null))
        {
            try
            {
                accountManager.DeleteFinancierConditions(companyId, financierOperationId);
                accountManager.DeleteFinancierOperation(accountManager.GetFinancierOperation(companyId, financierOperationId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {

                result = false;
            }


        }
        return result;
    }
    protected void grdFinancierOperations_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void odsFinancierOperations_Selecting1(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void btnTransfer_Click1(object sender, EventArgs e)
    {
        Server.Transfer("FinancierOperation.aspx");
    }
}
