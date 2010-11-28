using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.Linq;

using InfoControl.Web.Reporting;
using InfoControl.Web.Reporting.DataClasses;

public partial class ReportGenerator_GroupBy : ReportStepControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.NextStep == ReportSteps.Sort || Page.ActiveStep == ReportSteps.Sort)
        {
            int tableId = Page.Settings.Report.ReportTablesSchemaId ?? 0;
            bool isFirstTime = (ViewState["tableId"] == null && tableId != 0);
            bool isNewTableId = (ViewState["tableId"] != null && !ViewState["tableId"].Equals(tableId));

            if (isFirstTime || isNewTableId)
            {
                ViewState["tableId"] = tableId;
                BindGrid(-1);
            }
        }
    }

    void BindGrid(int index)
    {
        List<ReportSort> list = Page.Settings.SortedColumns;

        if (index == -1)
        {
            list.Add(new ReportSort());
            grid.EditIndex = 0;
            if (list.Count > 0)
            {
                grid.EditIndex = list.Count - 1;
            }
        }
        else
        {
            grid.EditIndex = index;
        }
        grid.DataSource = list;
        grid.DataBind();
    }

    void BindColumns(DropDownList combo)
    {
        combo.DataSource = Page.ReportManager.RetrieveColumnsSchema(Page.Settings.Report.ReportTablesSchemaId.Value);
        combo.DataBind();
        combo.Items.Insert(0, new ListItem("- - - - - - - - - - - - - -", ""));
    }
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        {
            BindColumns(e.Row.Cells[0].Controls[1] as DropDownList);
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Page.Settings.SortedColumns.RemoveAt(e.RowIndex);
        BindGrid(-1);
    }
    protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (e.RowIndex < Page.Settings.SortedColumns.Count)
            Page.Settings.SortedColumns.RemoveAt(e.RowIndex);

        int reportColumnsSchemaId = Convert.ToInt32((grid.Rows[e.RowIndex].Cells[0].Controls[1] as DropDownList).SelectedValue);
        bool asc = Convert.ToBoolean((grid.Rows[e.RowIndex].Cells[1].Controls[1] as DropDownList).SelectedValue);


        // Verifies if yet inserted
        ReportSort sortItem = Page.Settings.SortedColumns.Find(p => p.ReportColumnsSchemaId == reportColumnsSchemaId);

        if (sortItem == null)
        {
            sortItem = new ReportSort();
            sortItem.ReportTablesSchemaId = Page.Settings.Report.ReportTablesSchemaId.Value;
            sortItem.ReportColumnsSchemaId = reportColumnsSchemaId;
            sortItem.Ascending = asc;
            sortItem.Name = (grid.Rows[e.RowIndex].Cells[0].Controls[1] as DropDownList).SelectedItem.Text;
            Page.Settings.SortedColumns.Insert(e.RowIndex, sortItem);
        }
        else
        {
            sortItem.Ascending = asc;
        }
        BindGrid(-1);
    }
    protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        BindGrid(e.NewEditIndex);
    }
    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        BindGrid(-1);
    }
}
