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

using InfoControl;

public partial class User_Roles : Vivina.Erp.SystemFramework.PageBase
{
    protected int userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;

        if (!String.IsNullOrEmpty(Request["UserId"]))
        {
            userId = Convert.ToInt32(Request["UserId"]);
        }
    }

    protected void odsUserInRoles_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (userId != 0)
        {
            e.InputParameters["UserId"] = userId;
            e.InputParameters["companyId"] = Company.CompanyId;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (cboRoles.SelectedValue == "")
        {
            lblErr.Text = "Nao ha mais perfis disponiveis para este usuario";
            lblErr.Visible = true;
        }
        else
        {
            if (userId != 0)
            {
                // Create object
                UsersInRole userInRoles = new UsersInRole();
                userInRoles.UserId = userId;
                userInRoles.RoleId = Convert.ToInt16(cboRoles.SelectedValue);
                userInRoles.CompanyId = Company.CompanyId;

                // Fires Insert
                UsersInRolesManager uManager = new UsersInRolesManager(this);
                uManager.Insert(userInRoles);

                //Refresh the Combo and Grid
                cboRoles.DataBind();
                grdRolesByUser.DataBind();

                // Refresh the menu
                if (User.Identity.UserId == userId)
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ResetMenu", "top.ResetMenu();", true);
            }
        }
    }
    protected void grdRolesByUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //
        // Cancel a nested event fires
        //
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[0].Text == "Admin" && Company.CompanyId == Page.User.Identity.UserId)
            e.Row.Cells[1].Visible = false;
        else
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
    }

    protected void odsRoles_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["userId"] = userId;
    }

    protected void odsUserInRoles_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        cboRoles.DataBind();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ResetMenu", "top.ResetMenu();", true);
    }

    #region Empty Events

    protected void grdRolesByUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void odsUserInRoles_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {

    }
    protected void grdRolesByUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    #endregion
}

