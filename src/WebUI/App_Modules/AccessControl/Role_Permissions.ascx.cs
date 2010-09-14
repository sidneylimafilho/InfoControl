using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;


public partial class AccessControl_Role_Permissions : Vivina.Erp.SystemFramework.UserControlBase
{
    public Permission permission;
    public Permission newPermission;
    public PermissionManager permissionManager;
    public InfoControl.Web.Security.RolesManager roleManager;

    public void updatePermission()
    {
       

        foreach (GridViewRow row in grdFunctions.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                newPermission = new Permission();
                newPermission.FunctionId = Convert.ToInt32(grdFunctions.DataKeys[row.RowIndex]["FunctionId"]);
                newPermission.RoleId = Convert.ToInt32(Page.ViewState["RoleId"]);
                newPermission.CompanyId = Page.Company.CompanyId;

                CheckBox chkRead = (CheckBox)row.Cells[1].FindControl("chkRead");
                CheckBox chkWrite = (CheckBox)row.Cells[2].FindControl("chkWrite");

                if (chkRead.Checked)
                    newPermission.PermissionTypeId = 1;

                if (chkWrite.Checked)
                    newPermission.PermissionTypeId = 2;

                using (permissionManager = new PermissionManager(null))
                {
                    permission = permissionManager.GetPermission(newPermission.FunctionId, newPermission.RoleId, newPermission.CompanyId);
                    if (chkRead.Checked || chkWrite.Checked)
                    {
                        permissionManager.InsertVerifying(permission, newPermission);
                    }
                    else if (permission != null)
                    {
                        permissionManager.Delete(permission);
                    }
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void odsFunctions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Page.ViewState["RoleId"] != null)
        {
            e.InputParameters["RoleId"] = Page.ViewState["RoleId"];
            e.InputParameters["CompanyId"] = Page.Company.ReferenceCompanyId;
        }
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.ViewState["RoleId"] != null)
        {
            updatePermission();
        }
        Page.RefreshCredentials();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ResetMenu", "top.ResetMenu();", true);

        Response.Redirect("Roles.aspx");
    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Roles.aspx");
    }
    protected void grdFunctions_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
