using System;
using System.Web.UI.WebControls;
using InfoControl;
using System.Web.Services;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;

[PermissionRequired("Account")]
public partial class Accounting_Accounts : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void odsAccounts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void grdAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Attributes["onclick"] = "location='Account.aspx?AccountId=" + e.Row.DataItem.GetPropertyValue("AccountId").EncryptToHex() + "';";
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Clear();

        }
    }

    
    [WebMethod]
    public static bool DeleteAccount(int accountId, int companyId)
    {
        bool result = true;
        using (AccountManager accountManager = new AccountManager(null))
        {
            try
            {
                accountManager.Delete(accountManager.GetAccount(accountId, companyId));
            }
            catch (System.Data.SqlClient.SqlException)
            {
                result = false;
            }
        }
        return result;
    }

}
