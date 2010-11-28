using System;
using System.Data;
using System.Linq;
using System.Data.Linq;
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

public partial class ReportGenerator_MatrixRows : ReportStepControl
{

    string matrixRowsHidden = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.NextStep == ReportSteps.MatrixRows || Page.ActiveStep == ReportSteps.MatrixRows)
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

        matrixRowsHidden = Request["matrixRowsHidden"];
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        //
        // Attributes properties dont are persisted 
        // case swap step, then re-set
        //
        SetScriptCheckBoxItems();

        Page.ClientScript.RegisterHiddenField("MatrixRowsHidden", matrixRowsHidden);
    }

    void PersistColumnsSelected()
    {
        ReportsManager manager = new ReportsManager(null);

        if (!String.IsNullOrEmpty(Request["MatrixRowsHidden"]))
        {
            Page.Settings.MatrixRows.Clear();
            string[] values = Request["MatrixRowsHidden"].Trim(',').Split(',');
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
                    Page.Settings.MatrixRows.Add(column);
                }
            }
        }
    }



    void BindColumnsSchema(int tableId)
    {
        chkColumns.DataSource =
            Page.ReportManager.RetrieveColumnsSchema(tableId)
            .Where(col => col.ReportDataTypeId == ReportDataType.ForeignKey);

        chkColumns.DataBind();

        SetScriptCheckBoxItems();
    }

    void BindSelectedColumns()
    {
        OrderedColumns.Items.Clear();
        List<ReportColumn> columns = Page.Settings.MatrixRows;
        foreach (ReportColumn column in columns)
        {
            ListItem selectedItem;
            ListItem item = chkColumns.Items.FindByValue(column.ReportColumnsSchemaId.ToString());

            if (item != null)
            {
                selectedItem = new ListItem(item.Text, item.Value);
                selectedItem.Selected = true;
                OrderedColumns.Items.Add(selectedItem);
            }
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
