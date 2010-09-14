
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.WebUI;
using Vivina.Erp.SystemFramework;

public partial class Company_RH_AdditionalInfo : PageBase
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

    void CarregaGrid(int index)
    {
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        DataManager.Parameters.Add("@AddonInfoId", cboAdditionalInfo.SelectedValue);

        ds = DataManager.ExecuteDataSet(@"SELECT *  FROM " + TableName +
           " WHERE CompanyId=@CompanyId AND AddonInfoId = @AddonInfoId ORDER BY Name");

        if (index == -1)
        {
            DataRow dr = ds.Tables[0].NewRow();
            dr[0] = 0;

            if (dr[1].GetType() == typeof(int))
                dr[1] = default(int);
            if (dr[1].GetType() == typeof(string))
                dr[1] = "";

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



        if (!IsPostBack)
        {
            cboAdditionalInfo.DataBind();
            CarregaGrid(-1);
        }

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
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException() is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
            }
        }
        CarregaGrid(-1);
    }

    protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataManager.Parameters.Add("@Name", (grid.Rows[grid.EditIndex].Cells[0].Controls[1] as TextBox).Text);
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        DataManager.Parameters.Add("@AddonInfoId", cboAdditionalInfo.SelectedValue);

        if (Convert.ToInt32(grid.DataKeys[grid.EditIndex].Value) == 0)
        {
            DataManager.ExecuteNonQuery(@"INSERT INTO " + TableName +
              " (Name,CompanyId,AddonInfoId) VALUES (UPPER(@Name),@CompanyId,@AddonInfoId)");
        }
        else
        {
            DataManager.Parameters.Add(DataParameterKeyName, grid.DataKeys[grid.EditIndex].Value);
        
            DataManager.ExecuteNonQuery(@"UPDATE " + TableName +
               " SET Name = UPPER(@Name), CompanyId = @CompanyId, AddonInfoId = @AddonInfoId WHERE "
               + DataKeyName + " = " + DataParameterKeyName);
        }

        DataManager.Commit();

        CarregaGrid(-1);
    }

    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        CarregaGrid(-1);
    }

    protected void grid_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        CarregaGrid(e.NewSelectedIndex);
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;");
        }
    }

    protected void cboAdditionalInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaGrid(-1);
    }
    protected void odsHumanResources_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
