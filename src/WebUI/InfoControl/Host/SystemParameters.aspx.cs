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
using InfoControl.Web.Security.DataEntities;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;

using Vivina.Erp.SystemFramework;

public partial class SystemParametersList : Vivina.Erp.SystemFramework.PageBase
{
    bool isInserting = false;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void grdSystemParameters_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");


        }
    }

    protected void BusinessManagerDataSource1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (this.grdSystemParameters.Rows.Count <= 0)
        {
            this.frmSystemParameter.Visible = true;
            this.grdSystemParameters.Visible = false;
        }
    }

    protected void BusinessManagerDataSource1_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
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
    protected void frmSystemParameter_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        showGrid();
    }
    protected void grdSystemParameters_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert") //Testa o clique no botão de inclusão de registro
        {
            isInserting = true;
            e.Cancel = true;

            this.frmSystemParameter.Visible = true;
            this.grdSystemParameters.Visible = false;
            this.grdSystemParameters.DataSourceID = null;
            this.frmSystemParameter.ChangeMode(FormViewMode.Insert);
            this.frmSystemParameter.PageIndex = -1;
            this.frmSystemParameter.DataBind();
        }
    }
    protected void grdSystemParameters_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            this.frmSystemParameter.Visible = true;
            this.grdSystemParameters.Visible = false;
            this.frmSystemParameter.PageIndex = this.grdSystemParameters.SelectedIndex;
            this.frmSystemParameter.ChangeMode(FormViewMode.Edit);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (!IsPostBack)
        {
            this.frmSystemParameter.Visible = (this.grdSystemParameters.Rows.Count == 0);
            this.grdSystemParameters.Visible = !this.frmSystemParameter.Visible;
        }

    }
    protected void frmSystemParameter_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        showGrid();
    }
    protected void frmSystemParameter_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            showGrid();
        }
    }
    protected void showGrid()
    {
        this.frmSystemParameter.Visible = false;
        this.grdSystemParameters.Visible = true;
        this.grdSystemParameters.DataSourceID = odsSystemParameters.ID;
        this.grdSystemParameters.DataBind();
    }
    protected void btnCreateSystemParameter_Click(object sender, EventArgs e)
    {
        Server.Transfer("SystemParameters.aspx");
    }
}
