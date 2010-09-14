using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl.Web.UI;
using InfoControl.Web.Security;
using System.Web.Services;
using Vivina.Erp.BusinessRules.Services;

[PermissionRequired("ServiceOrderBook")]
public partial class InfoControl_Services_ServiceOrderBook : Vivina.Erp.SystemFramework.PageBase
{
    DataSet ds;
    private void loadGrid(int index)
    {
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        ds = DataManager.ExecuteDataSet(@"SELECT EmployeeProfile.Name AS [EmployeeName], Coalesce(RepresentantProfile.Name,LegalEntityProfile.CompanyName) AS [RepresentantName], ServiceOrderBookId, ServiceOrderBook.CompanyId, ServiceOrderBook.StartNumber," +
                                        " ServiceOrderBook.FinishNumber, ServiceOrderBook.Quantity, ServiceOrderBook.MinimumQuantity, ServiceOrderBook.RepresentantId," +
                                        " ServiceOrderBook.EmployeeId FROM ServiceOrderBook" +
                                        " LEFT JOIN Representant ON ServiceOrderBook.RepresentantId = Representant.RepresentantId" +
                                        " LEFT JOIN Employee ON ServiceOrderBook.EmployeeId = Employee.EmployeeId" +
                                        " AND ServiceOrderBook.CompanyId = Employee.CompanyId" +
                                        " LEFT JOIN Profile AS [RepresentantProfile] ON Representant.ProfileId = RepresentantProfile.ProfileId" +
                                        " LEFT JOIN LegalEntityProfile ON Representant.LegalEntityProfileId = LegalEntityProfile.LegalEntityProfileId" +
                                        " LEFT JOIN Profile AS [EmployeeProfile] ON Employee.ProfileId = EmployeeProfile.ProfileId" +
                                        " WHERE ServiceOrderBook.CompanyId = @CompanyId");

        if (index == -1)
        {
            DataRow dr = ds.Tables[0].NewRow();

            dr[1] = 0;
            dr[2] = 0;
            dr[3] = DBNull.Value;
            dr[4] = DBNull.Value;
            dr[5] = DBNull.Value;
            dr[6] = DBNull.Value;

            ds.Tables[0].Rows.InsertAt(dr, 0);
            grdServiceOrderBook.EditIndex = 0;
            grdServiceOrderBook.Attributes["State"] = "Insert";
        }
        else
        {
            grdServiceOrderBook.EditIndex = (grdServiceOrderBook.Attributes["State"] == "Insert" ? index - 1 : index);
            grdServiceOrderBook.Attributes["State"] = "Edit";
        }

        grdServiceOrderBook.DataSource = ds;
        grdServiceOrderBook.DataBind();

        //
        // Set as default in cboSupplier the name of company for line in edition
        //
        //(grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].FindControl("cboSupplier") as DropDownList).Items[0].Text = Company.LegalEntityProfile.CompanyName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            ds = new DataSet();

            loadGrid(-1);
        }
    }

    protected void odsEmployee_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsRepresentant_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void grdServiceOrderBook_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataManager.Parameters.Add("@CompanyId", Company.CompanyId);
        //
        // Retrive SupplierId
        //
        String representantId = (grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].Cells[0].Controls[1] as DropDownList).SelectedValue;

        DataManager.Parameters.Add("@EmployeeId", (grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].Cells[1].Controls[1] as DropDownList).SelectedValue);
        DataManager.Parameters.Add("@StartNumber", (grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].Cells[2].Controls[1] as CurrencyField).IntValue);
        DataManager.Parameters.Add("@FinishNumber", (grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].Cells[3].Controls[1] as CurrencyField).IntValue);
        DataManager.Parameters.Add("@Quantity", (grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].Cells[4].Controls[1] as CurrencyField).IntValue);
        DataManager.Parameters.Add("@MinimumQuantity", (grdServiceOrderBook.Rows[grdServiceOrderBook.EditIndex].Cells[5].Controls[1] as CurrencyField).IntValue);

        if (Convert.ToInt32(grdServiceOrderBook.DataKeys[grdServiceOrderBook.EditIndex].Value) == 0)
        {
            //If insertMode SupplierId equals NULL
            DataManager.Parameters.Add("@RepresentantId", (String.IsNullOrEmpty(representantId) ? Convert.DBNull : representantId));

            DataManager.ExecuteNonQuery(@"INSERT INTO ServiceOrderBook" +
                                        " (CompanyId,RepresentantId,EmployeeId,StartNumber,FinishNumber,Quantity,MinimumQuantity)" +
                                        " VALUES (@CompanyId,@RepresentantId,@EmployeeId,@StartNumber,@FinishNumber,@Quantity,@MinimumQuantity)");
        }
        else
        {
            //If updateMode SupplierId equals 0 for compare register
            DataManager.Parameters.Add("@RepresentantId", (String.IsNullOrEmpty(representantId) ? "0" : representantId));

            DataManager.ExecuteNonQuery(@"UPDATE ServiceOrderBook SET StartNumber=@StartNumber," +
                                        " FinishNumber=@FinishNumber, Quantity=@Quantity," +
                                        " MinimumQuantity=@MinimumQuantity WHERE CompanyId=@CompanyId" +
                                        " AND ISNULL(RepresentantId, 0)=@RepresentantId AND EmployeeId=@EmployeeId");
        }
        DataManager.Commit();

        loadGrid(-1);
    }

    protected void grdServiceOrderBook_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex == 0)
        {
            //
            // If the grid is in insert mode, this "if" turn the delete button of the grid
            // to visible = false, only showing the controls that have SAVE and CANCEL functions
            //

            //e.Row.Cells[i - 2].Visible = false;

            e.Row.Cells[e.Row.Cells.Count - 2].Attributes["style"] = "display:none";
            return;
        }

        //
        // This action, gets all lines that are not currently edited and sets
        // the actions to only show the DELETE button, SAVE and CANCEL are now visible = false
        // and the DELETE button now, receives a confirm dialog, asking the user about delection.
        //

        if (e.Row.RowType == DataControlRowType.DataRow &&
            ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Normal))
        {
            //String script = Page.ClientScript.GetPostBackEventReference(sender as Control, "Edit$" + e.Row.RowIndex.ToString());
            //// e.Row.Cells[0].Attributes["onclick"] = script;
            if (e.Row.RowState != DataControlRowState.Edit)
            {
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
                e.Row.Cells[e.Row.Cells.Count - 2].Attributes["Align"] = "center";
                e.Row.Cells[e.Row.Cells.Count - 2].Attributes["onclick"] = "return(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?'))";
            }
        }

        //
        // Sets the visible = false to DELETE button on the edited row
        //

        else if (e.Row.RowType == DataControlRowType.DataRow &&
            ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit))
        {
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
        }

        //
        // At last, do not show any text or buttons in the HEADER COLUMN of EDIT controls
        //

        else if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void grdServiceOrderBook_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        loadGrid(e.NewSelectedIndex);
    }

    protected void grdServiceOrderBook_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        loadGrid(-1);
    }


    /// <summary>
    /// This method deletes a serviceOrderBook together with js file "ServiceOrderBook.aspx.js"
    /// </summary>
    /// <param name="serviceOrderBookId"></param>
    /// <returns></returns>
    [WebMethod]
    public static bool DeleteServiceOrderBook(Int32 serviceOrderBookId)
    {
        var result = true;

        using (var serviceManager = new ServicesManager(null))
        {
            try
            {
                serviceManager.DeleteServiceOrderBook(serviceManager.GetServiceOrderBook(serviceOrderBookId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }

        return result;
    }
}
