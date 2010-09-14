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
using Vivina.Erp.BusinessRules;
using InfoControl;



namespace Vivina.Erp.WebUI.App_Reports
{
    public partial class DynamicReportVision : Vivina.Erp.SystemFramework.PageBase
    {

        ReportsManager reportsManager;
        ReportTablesSchema reportTablesSchema;
        ReportColumnsSchema reportColumnsSchema;
        Int32 reportTablesSchemaId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["ReportTablesSchemaId"]))
                reportTablesSchemaId = Convert.ToInt32(Request["ReportTablesSchemaId"].DecryptFromHex());

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request["ReportTablesSchemaId"]))
                    ShowReportTablesSchema();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ReportTablesSchema original_reportTablesSchema = new ReportTablesSchema();
            reportsManager = new ReportsManager(this);
            reportTablesSchema = new ReportTablesSchema();

            if (reportTablesSchemaId != 0)
            {
                original_reportTablesSchema = reportsManager.GetTableSchema(reportTablesSchemaId);
                reportTablesSchema.CopyPropertiesFrom(original_reportTablesSchema);

            }

            reportTablesSchema.Name = txtName.Text;
            reportTablesSchema.SqlText = txtSqltext.Text;

            if (reportTablesSchemaId != 0)
                reportsManager.Update(original_reportTablesSchema, reportTablesSchema);
            else
            {
                reportsManager.Insert(reportTablesSchema);
                Response.Redirect("DynamicReport.aspx?ReportTablesSchemaId=" + reportTablesSchema.ReportTablesSchemaId.EncryptToHex());
            }


            // Server.Transfer("dynamicReports.aspx");
            // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Customer.aspx?CustomerId=" + customer.CustomerId.EncryptToHex() + "';", true);

        }

        private void ShowReportTablesSchema()
        {
            reportsManager = new ReportsManager(this);
            reportTablesSchema = reportsManager.GetTableSchema(reportTablesSchemaId);
            txtName.Text = reportTablesSchema.Name;
            txtSqltext.Text = reportTablesSchema.SqlText;
        }
    }
}
