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
using InfoControl.Data;
using InfoControl.Web.UI;

public partial class Company_RH_Formation_Education : Vivina.Erp.SystemFramework.PageBase
{
    DataSet ds;

    private string tableName;
    public string TableName
    {
        get { return tableName; }
        set { tableName = value; }
    }

    private string dataKeyName;

    public string DataKeyName
    {
        get { return dataKeyName; }
        set { dataKeyName = value; }
    }


    public string DataParameterKeyName
    {
        get { return "@" + DataKeyName; }
    }

    private void ShowMessage(string message)
    {
        ShowError(message);
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        //
        // Mantenho o scroll pois a edição pode ser nas ultimas linhas da grid
        //
        MaintainScrollPositionOnPostBack = true;


        //
        // Configura o cadastro genérico
        //
        TableName = Request["TableName"];
        DataKeyName = Request["DataKeyName"];

        lblTitle.Text = "Cadastro de " + Request["Title"];

        if (!IsPostBack)
            CarregaGrid(-1);


    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            DataManager.Parameters.Add(DataParameterKeyName, grid.DataKeys[e.RowIndex].Value);
            DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
            DataManager.ExecuteNonQuery("DELETE FROM " + TableName + " WHERE " + DataKeyName + " = " + DataParameterKeyName +
                " AND CompanyId = @CompanyId");
            DataManager.Commit();
            ShowMessage("Operação realizada com sucesso!");
        }
        catch (Exception ex)
        {
            ShowMessage("Não foi possivel apagar o registro selecionado pois há outros registros associados!<br />" + ex.Message);
        }

        CarregaGrid(-1);
    }
    protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        CarregaGrid(e.NewEditIndex);
    }
    protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataManager.Parameters.Add("@Name", (grid.Rows[grid.EditIndex].Cells[0].Controls[1] as TextBox).Text);
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        DataManager.Parameters.Add("@EducationLevelId", (grid.Rows[grid.EditIndex].Cells[1].Controls[1] as DropDownList).SelectedValue);

        if (Convert.ToInt32(grid.DataKeys[grid.EditIndex].Value) == 0)
        {
            DataManager.ExecuteNonQuery(@"INSERT INTO " + TableName +
                " (Name,CompanyId,EducationLevelId) VALUES (UPPER(@Name),@CompanyId,@EducationLevelId)");
        }
        else
        {
            DataManager.Parameters.Add(DataParameterKeyName, grid.DataKeys[grid.EditIndex].Value);
            DataManager.ExecuteNonQuery(@"UPDATE " + TableName +
                " SET Name = UPPER(@Name), CompanyId = @CompanyId, EducationLevelId = @EducationLevelId WHERE "
                + DataKeyName + " = " + DataParameterKeyName);
        }
        DataManager.Commit();
        ShowMessage("Operação realizada com sucesso!");
        CarregaGrid(-1);
    }

    void CarregaGrid(int index)
    {
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        ds = DataManager.ExecuteDataSet(@"SELECT *  FROM " + TableName +
            " WHERE CompanyId=@CompanyId ORDER BY Name");
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        DataSet educationData = DataManager.ExecuteDataSet(@"SELECT * FROM EducationLevel
             WHERE CompanyId=@CompanyId ORDER BY Name");

        if (index == -1)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = "";
            dr[2] = System.DBNull.Value;
            ds.Tables[0].Rows.InsertAt(dr, 0);
            grid.EditIndex = 0;
            grid.Attributes["State"] = "Insert";
        }
        else
        {
            grid.EditIndex = (grid.Attributes["State"] == "Insert" ? index - 1 : index);
            grid.Attributes["State"] = "Edit";
        }

        grid.DataKeyNames = new string[] { DataKeyName };
        grid.DataSource = ds;
        grid.DataBind();
    }

    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CarregaGrid(-1);
    }
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowIndex == 0)
        {
            //
            // If the grid is in insert mode, this "if" turn the delete button of the grid
            // to visible = false, only showing the controls that have SAVE and CANCEL functions
            //
            int i = e.Row.Cells.Count;
            e.Row.Cells[i - 2].Visible = false;
            return;
        }

        GridView grid = sender as GridView;

        //
        // This action, gets all lines that are not currently edited and sets
        // the actions to only show the DELETE button, SAVE and CANCEL are now visible = false
        // and the DELETE button now, receives a confirm dialog, asking the user about delection.
        //

        if (e.Row.RowType == DataControlRowType.DataRow &&
            ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Normal))
        {
            String script = Page.ClientScript.GetPostBackEventReference(sender as Control, "Edit$" + e.Row.RowIndex.ToString());
            e.Row.Cells[0].Attributes["onclick"] = script;
            if (e.Row.RowState != DataControlRowState.Edit)
            {
                int i = e.Row.Cells.Count;
                e.Row.Cells[i - 1].Visible = false;
                (e.Row.Cells[1].Controls[1] as DropDownList).Enabled = false;
                e.Row.Cells[i - 2].Attributes["Width"] = "1%";
                e.Row.Cells[i - 2].Attributes["Align"] = "center";
                (e.Row.Cells[i - 2].Controls[0] as LinkButton).Attributes["onclick"] = "return(confirm('Deseja realmente excluir esse registro?'))";
            }
        }

        //
        // Sets the visible = false to DELETE button on the edited row
        //

        else if (e.Row.RowType == DataControlRowType.DataRow &&
            ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit))
        {
            int i = e.Row.Cells.Count;
            e.Row.Cells[i - 2].Visible = false;
        }

        //
        // At last, do not show any text or buttons in the HEADER COLUMN of EDIT controls
        //

        else if (e.Row.RowType == DataControlRowType.Header)
        {
            int i = e.Row.Cells.Count;
            e.Row.Cells[i - 1].Visible = false;
        }
    }
    protected void odsEducationLevel_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
