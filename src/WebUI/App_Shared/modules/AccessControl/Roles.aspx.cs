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
using Vivina.Erp.SystemFramework;
using InfoControl.Web.Security;

[PermissionRequired("Roles")]
public partial class Roles : Vivina.Erp.SystemFramework.PageBase
{
    bool isInserting = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }

    protected void grdRoles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PostBackOptions postOptions = new PostBackOptions(this.grdRoles, "Select$" + e.Row.RowIndex.ToString());
            String insertScript = ClientScript.GetPostBackEventReference(postOptions);

            //
            // This for not uses equal, to not overbound the index
            //
            for (int a = 0; a < e.Row.Cells.Count; a++)
            {
                e.Row.Cells[a].Attributes.Add("onclick", insertScript);
            }

            //
            // Cancel a nested event fires
            //
            if (Convert.ToString(grdRoles.DataKeys[e.Row.RowIndex]["Name"]).ToUpper().Contains("ADMIN"))
                e.Row.Cells[e.Row.Cells.Count - 1].Text = "";
            else
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");

        }
    }
    protected void grdRoles_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        this.grdRoles.Visible = false;
        Context.Items["RoleId"] = grdRoles.DataKeys[e.NewSelectedIndex]["RoleId"].ToString();
        Server.Transfer("Role.aspx");
    }
    protected void odsRoles_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    lblErr.Visible = true;
                    lblErr.Text = "O registro não pode ser apagado pois há outros registros associados!";
                    e.ExceptionHandled = true;
                }
            }
        }
    }
    protected void grdRoles_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("Role.aspx");
        }
    }
    protected void odsRoles_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void odsRoles_Deleted1(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting Products that are associated with others Tables
        //
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    lblErr.Visible = true;
                    lblErr.Text = "O registro não pode ser apagado pois há outros registros associados!";
                    e.ExceptionHandled = true;
                }
            }
        }
    }
}
