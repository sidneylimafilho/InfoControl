using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using InfoControl;
using InfoControl.Configuration;
using Vivina.Erp.DataClasses;
using InfoControl.Data;
using Vivina.Erp.BusinessRules.Reports;

public partial class InfoControl_Host_Report : Vivina.Erp.SystemFramework.PageBase
{
    ReportsManager reportsManager;


    #region Functions
    private void showReport(Report report)
    {
        txtName.Text = report.Name;
        txtReportUrl.Text = report.ReportUrl;
        if (report.ReportTablesSchemaId.HasValue)
            cboReportTablesSchemaId.SelectedValue = report.ReportTablesSchemaId.ToString();
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.Items["ReportId"] != null)
        {
            Page.ViewState["ReportId"] = Context.Items["ReportId"];
            reportsManager = new ReportsManager(this);
            showReport(reportsManager.GetReport((Int32)Page.ViewState["ReportId"]));
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        reportsManager = new ReportsManager(this);
        Report original_report, report;
        original_report = new Report();
        report = new Report();

        if (Page.ViewState["ReportId"] != null)
        {
            original_report = reportsManager.GetReport((Int32)Page.ViewState["ReportId"]);
            report.CopyPropertiesFrom(original_report);
        }

        report.Name = txtName.Text;
        report.ReportUrl = txtReportUrl.Text;
        if (!String.IsNullOrEmpty(cboReportTablesSchemaId.SelectedValue))
            report.ReportTablesSchemaId = Convert.ToInt32(cboReportTablesSchemaId.SelectedValue);

        if (original_report.ReportId != 0)
            reportsManager.UpdateReport(original_report, report);
        else
            reportsManager.InsertReport(report);

        Server.Transfer("reports.aspx");


    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("reports.aspx");
    }
}
