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

public partial class Role_General : InfoControl.Web.UI.DataUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            if (Context.Items["RoleId"] != null)
            {
                frmRole.ChangeMode(FormViewMode.Edit);
                if ((frmRole.FindControl("NameTextBox") as TextBox).Text.ToUpper().Contains("ADMIN"))
                    (frmRole.FindControl("NameTextBox") as TextBox).Enabled = false;
            }
    }
    protected void odsRoles_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Context.Items["RoleId"] != null)
        {
            e.InputParameters["RoleId"] = Context.Items["RoleId"].ToString();
            e.InputParameters["CompanyId"] = (Page as Vivina.Erp.SystemFramework.PageBase).Company.CompanyId.ToString();
        }
    }
    protected void frmRole_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            Response.Redirect("Roles.aspx");
        }
    }
    protected void odsRoles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        var newRole = e.InputParameters[0] as Vivina.Erp.DataClasses.Role;
        newRole.ApplicationId = (Page as Vivina.Erp.SystemFramework.PageBase).Application.ApplicationId;
        newRole.CompanyId = (Page as Vivina.Erp.SystemFramework.PageBase).Company.CompanyId;
        newRole.LastUpdatedDate = DateTime.Now;
    }
    protected void odsRoles_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

        Context.Items["RoleId"] = e.ReturnValue;
        Server.Transfer("Role.aspx");
    }
    protected void odsRoles_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        var  role = e.InputParameters["entity"] as Vivina.Erp.DataClasses.Role;
        role.ApplicationId = (Page as Vivina.Erp.SystemFramework.PageBase).Application.ApplicationId;
        role.CompanyId = (Page as Vivina.Erp.SystemFramework.PageBase).Company.CompanyId;
        role.LastUpdatedDate = DateTime.Now;
    }
    protected void odsRoles_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Context.Items["RoleId"] = e.ReturnValue;
        Response.Redirect("Roles.aspx");
    }
}
