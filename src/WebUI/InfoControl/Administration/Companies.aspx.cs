using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Companies")]
public partial class Company_Companies : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            cboPageSize.Items.Add(new ListItem("Todos", Int16.MaxValue.ToString()));
    }

    protected void odsCompanies_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["UserId"] = User.Identity.UserId;
    }

    protected void grdCompanies_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='Company.aspx?CompanyId=" + e.Row.DataItem.GetPropertyValue("CompanyId") + "';";

            //
            // Cancel a nested event fires
            //

            //Compares the company which is online
            //with the company that will be added in the gridview

            if (Company.CompanyId != Convert.ToInt32(grdCompanies.DataKeys[e.Row.RowIndex]["CompanyId"]))
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            else
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    //protected void grdCompanies_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if (e.SortExpression == "Insert")
    //    {
    //        Server.Transfer("Company.aspx");
    //    }
    //}

    //protected void grdCompanies_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    Context.Items["CompanyId"] = grdCompanies.DataKeys[e.NewSelectedIndex]["CompanyId"].ToString();
    //    Server.Transfer("Company.aspx");
    //}

    protected void odsCompanies_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    e.ExceptionHandled = true;
                }
            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "top.ResetHeader();", true);
        }
    }

    //protected void btnTransfer_Click(object sender, EventArgs e)
    //{
    //    Server.Transfer("Company.aspx");
    //}

    protected void odsCompanies_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdCompanies.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdCompanies.DataBind();
    }
}
