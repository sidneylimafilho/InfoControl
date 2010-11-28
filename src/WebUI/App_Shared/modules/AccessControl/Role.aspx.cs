using System;
using System.Web.UI;
using InfoControl.Web.Security;

[PermissionRequired("Roles")]
public partial class AccessControl_Role : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Context.Items["RoleId"] != null)
            {
                Page.ViewState["RoleId"] = Context.Items["RoleId"];
                tabPermissions.Visible = true;
            }
        }
    }
}