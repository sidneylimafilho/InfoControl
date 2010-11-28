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


using InfoControl.Web.UI;
using InfoControl.Web.Reporting;
using InfoControl.Web.Reporting.DataClasses;

public partial class ReportGenerator_Sort : ReportStepControl
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
                BindColumns(cboColumn);
                BindGrid(-1);
            }
        }
    }
    void BindGrid(int index)
    {
        grid.EditIndex = index;
        grid.DataSource = Page.Settings.SortedColumns;
        grid.DataBind();
    }
    void BindColumns(DropDownList combo)
    {
        //combo.DataSource = Page.ReportManager.RetrieveColumnsSchema(Page.Settings.Report.ReportTablesSchemaId);
        combo.DataSource = Page.Settings.Columns;
        combo.DataBind();
        combo.Items.Insert(0, new ListItem("- - - - - - - - - - - - - -", ""));
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindGrid(-1);
    }
    
    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        BindGrid(-1);
    }
    protected void btnAddSortColumn_Click(object sender, ImageClickEventArgs e)
    {
        ReportSort sortItem = new ReportSort();
        sortItem.ReportTablesSchemaId = Page.Settings.Report.ReportTablesSchemaId.Value;
        sortItem.ReportColumnsSchemaId = Convert.ToInt32(cboColumn.SelectedValue);
        sortItem.Ascending = Convert.ToBoolean(cboOrder.SelectedValue);
        sortItem.Name = cboColumn.SelectedItem.Text;
        
        Page.Settings.SortedColumns.Add(sortItem);

        BindGrid(-1);
    }
}
