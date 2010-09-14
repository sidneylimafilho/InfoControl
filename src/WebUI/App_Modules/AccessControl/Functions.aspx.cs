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
using Vivina.Erp.SystemFramework;


public partial class FunctionList : Vivina.Erp.SystemFramework.PageBase
{
    bool isInserting = false;

    protected void showGrid()
    {
        this.frmFunctions.Visible = false;
        this.grdFunctions.Visible = true;
        this.grdFunctions.DataSourceID = odsFunctions.ID;
        this.grdFunctions.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblErr.Visible = false;
    }

    protected void grdFunctions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            PostBackOptions postOptions = new PostBackOptions(this.grdFunctions, "Select$" + e.Row.RowIndex.ToString());
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
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");

        }
    }
    protected void odsFunctions_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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
    protected void frmFunctions_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            showGrid();
        }
    }
    protected void frmFunctions_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        RefreshCredentials();
        ClientScript.RegisterStartupScript(this.GetType(), "ResetMenu", "top.ResetMenu();", true);
        showGrid();               
    }
    protected void frmFunctions_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        RefreshCredentials();
        ClientScript.RegisterStartupScript(this.GetType(), "ResetMenu", "top.ResetMenu();", true); 
        showGrid();
    }
    protected void grdFunctions_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert") //Testa o clique no botão de inclusão de registro
        {
            isInserting = true;
            e.Cancel = true;

            this.frmFunctions.Visible = true;
            this.grdFunctions.Visible = false;
            this.grdFunctions.DataSourceID = null;
            this.frmFunctions.ChangeMode(FormViewMode.Insert);
            this.frmFunctions.PageIndex = -1;
            this.frmFunctions.DataBind();
        }
    }

    protected void grdFunctions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {


        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (!IsPostBack)
        {
            this.frmFunctions.Visible = (this.grdFunctions.Rows.Count == 0);
            this.grdFunctions.Visible = !this.frmFunctions.Visible;
        }

    }
    protected void grdFunctions_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        this.frmFunctions.Visible = true;
        this.grdFunctions.Visible = false;
        this.frmFunctions.PageIndex = ((grdFunctions.PageIndex * grdFunctions.PageSize) + e.NewSelectedIndex);
        this.frmFunctions.ChangeMode(FormViewMode.Edit);
    }

}