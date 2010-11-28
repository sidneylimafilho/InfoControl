using System;
using System.Data;

using System.Data.Linq;
using System.Linq;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Web.Reporting;
using InfoControl.Web.Reporting.DataClasses;

public partial class ReportGenerator_Columns : ReportStepControl
{

    string orderedColumnsHidden = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.NextStep == ReportSteps.Columns || Page.ActiveStep == ReportSteps.Columns || Page.ActiveStep == ReportSteps.Start)
        {
            int tableId = Page.Settings.Report.ReportTablesSchemaId ?? 0;
            bool isFirstTime = (ViewState["tableId"] == null && tableId != 0);
            bool isNewTableId = (ViewState["tableId"] != null && !ViewState["tableId"].Equals(tableId));

            if (isFirstTime || isNewTableId)
            {
                ViewState["tableId"] = tableId;
                BindColumnsSchema(tableId);
            }
        }

        //
        // As this user control always load, independent of the ActiveStep
        // then verify columns selected and Persist
        //
        PersistColumnsSelected();

        BindSelectedColumns();

        orderedColumnsHidden = Request["OrderedColumnsHidden"];
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        //
        // Attributes properties dont are persisted 
        // case swap step, then re-set
        //
        SetScriptCheckBoxItems();

        Page.ClientScript.RegisterHiddenField("OrderedColumnsHidden", orderedColumnsHidden);
    }

    void PersistColumnsSelected()
    {
        ReportsManager manager = new ReportsManager(null);

        if (!String.IsNullOrEmpty(Request["OrderedColumnsHidden"]))
        {
            Page.Settings.Columns.Clear();
            string[] values = Request["OrderedColumnsHidden"].Trim(',').Split(',');
            foreach (string value in values)
            {
                if (!String.IsNullOrEmpty(value))
                {
                    
                    ReportColumnsSchema columnSchema = manager.GetColumnSchema(Page.Settings.Report.ReportTablesSchemaId.Value, Convert.ToInt32(value));

                    ReportColumn column = new ReportColumn();
                    column.ReportColumnsSchemaId = columnSchema.ReportColumnsSchemaId;
                    column.ReportTablesSchemaId = columnSchema.ReportTablesSchemaId;
                    column.ReportDataTypeId = columnSchema.ReportDataTypeId;
                    column.Name = columnSchema.Name;
                    Page.Settings.Columns.Add(column);
                }
            }
        }
    }



    void BindColumnsSchema(int tableId)
    {
        IQueryable<ReportColumnsSchema> query = Page.ReportManager.RetrieveColumnsSchema(tableId);

        if (Page.Settings.MatrixRows.Count > 0)
            query = query.Where(rc => rc.ReportDataTypeId != ReportDataType.Text);

        chkColumns.DataSource = query;
        chkColumns.DataBind();

        SetScriptCheckBoxItems();
    }

    void BindSelectedColumns()
    {
        OrderedColumns.Items.Clear();
        List<ReportColumn> columns = Page.Settings.Columns;
        foreach (ReportColumn column in columns)
        {
            ListItem item = chkColumns.Items.FindByValue(column.ReportColumnsSchemaId.ToString());
            ListItem selectedItem = new ListItem(item.Text, item.Value);
            selectedItem.Selected = true;
            OrderedColumns.Items.Add(selectedItem);
        }
    }

    void SetScriptCheckBoxItems()
    {
        foreach (ListItem item in chkColumns.Items)
        {
            item.Attributes["onclick"] = "SelectColumn(this, '" + item.Text + "', '" + item.Value + "')";
        }
    }


}
