using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl.Data;
using InfoControl;
using InfoControl.Web.Reporting.DataClasses;
using InfoControl.Web.Reporting;
using InfoControl;

namespace Vivina.Erp.WebUI.App_Reports
{
    public partial class DynamicReportField : Vivina.Erp.SystemFramework.PageBase
    {
        Int32 reportColumnsSchemaId;
        Int32 reportTablesSchemaId;

        protected void Page_Load(object sender, EventArgs e)
        {
            reportTablesSchemaId = Convert.ToInt32(Request["ReportTablesSchemaId"].DecryptFromHex());

            if (!String.IsNullOrEmpty(Request["ReportColumnsSchemaId"]))
                reportColumnsSchemaId = Convert.ToInt32(Request["ReportColumnsSchemaId"].DecryptFromHex());

            if (!IsPostBack)
                ShowReportColumnsSchema();
        }

        private void ShowReportColumnsSchema()
        {
            if (reportColumnsSchemaId > 0)
            {
                var reportsManager = new ReportsManager(this);
                var reportColumnsSchema = reportsManager.GetColumnSchema(reportTablesSchemaId, reportColumnsSchemaId);

                txtReportColumnsSchemaName.Text = reportColumnsSchema.Name;
                txtSource.Text = reportColumnsSchema.Source;
                txtForeignKey.Text = reportColumnsSchema.ForeignKey;
                txtPrimaryKey.Text = reportColumnsSchema.PrimaryKey;
                txtPrimaryTable.Text = reportColumnsSchema.PrimaryTable;
                txtPrimaryLabelColumn.Text = reportColumnsSchema.PrimaryLabelColumn;
                cboReportDataType.DataBind();
                cboReportDataType.SelectedValue = reportColumnsSchema.ReportDataTypeId.ToString();
            }
        }

        protected void btnSaveReportColumnsSchema_Click(object sender, EventArgs e)
        {

            var reportsManager = new ReportsManager(this);
            var reportColumnsSchema = new ReportColumnsSchema();
            ReportColumnsSchema original_reportColumnsSchema = new ReportColumnsSchema();

            if (reportTablesSchemaId > 0)
            {
                original_reportColumnsSchema = reportsManager.GetColumnSchema(reportTablesSchemaId, reportColumnsSchemaId);
                reportColumnsSchema.CopyPropertiesFrom(original_reportColumnsSchema);
            }

            reportColumnsSchema.ReportTablesSchemaId = reportTablesSchemaId;

            reportColumnsSchema.Name = txtReportColumnsSchemaName.Text;
            reportColumnsSchema.Source = txtSource.Text;
            reportColumnsSchema.ReportDataTypeId = Convert.ToInt32(cboReportDataType.SelectedValue);
            reportColumnsSchema.ForeignKey = txtForeignKey.Text;
            reportColumnsSchema.PrimaryKey = txtPrimaryKey.Text;
            reportColumnsSchema.PrimaryTable = txtPrimaryTable.Text;
            reportColumnsSchema.PrimaryLabelColumn = txtPrimaryLabelColumn.Text;

            if (reportColumnsSchemaId > 0)
                reportsManager.Update(original_reportColumnsSchema, reportColumnsSchema);
            else
                reportsManager.Insert(reportColumnsSchema);

            Response.Redirect("DynamicReportFields.aspx?ReportTablesSchemaId=" + Request["ReportTablesSchemaId"]);

        }
    }
}
