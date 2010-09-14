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
using Vivina.Erp.BusinessRules;

using Vivina.Erp.SystemFramework;

public partial class Aplications : Vivina.Erp.SystemFramework.PageBase
{
    bool isInserting = false;
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void grdApplications_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //
            // Cancel a nested event fires
            //
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
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
    protected void frmApplication_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        showGrid();

    }
    protected void grdApplications_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert") //Testa o clique no botão de inclusão de registro
        {
            isInserting = true;
            e.Cancel = true;

            this.frmApplication.Visible = true;
            this.grdApplications.Visible = false;
            this.grdApplications.DataSourceID = null;
            this.frmApplication.ChangeMode(FormViewMode.Insert);
            this.frmApplication.PageIndex = -1;
            this.frmApplication.DataBind();
        }
    }
    

    private void InitializeComponent()
    {


    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (!IsPostBack)
        {
            this.frmApplication.Visible = (this.grdApplications.Rows.Count == 0);
            this.grdApplications.Visible = !this.frmApplication.Visible;
        }
        
    }

    protected void frmApplication_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        DataManager.Commit();
        InfoControl.Web.Configuration.Application.Refresh();

        showGrid();

    }
    protected void frmApplication_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            showGrid();
        }
    }

    protected void showGrid()
    {
        this.frmApplication.Visible = false;
        this.grdApplications.Visible = true;
        this.grdApplications.DataSourceID = BusinessManagerDataSource1.ID;
        this.grdApplications.DataBind();
    }

    protected void grdApplications_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        this.frmApplication.Visible = true;
        this.grdApplications.Visible = false;
        this.frmApplication.PageIndex = e.NewSelectedIndex;

        this.frmApplication.ChangeMode(FormViewMode.Edit);
    }
}
