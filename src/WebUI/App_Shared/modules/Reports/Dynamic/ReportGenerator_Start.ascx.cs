using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Web.UI;
using InfoControl.Web.Reporting;

public partial class ReportGenerator_Start : ReportStepControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        int tableId = Convert.ToInt32("0" + cboTables.SelectedValue);
        bool isFirstTime = (ViewState["tableId"] == null && tableId != 0);
        bool isNewTableId = (ViewState["tableId"] != null && !ViewState["tableId"].Equals(tableId));

        if (isFirstTime || isNewTableId)
        {
            ViewState["tableId"] = Convert.ToInt32("0" + cboTables.SelectedValue);
            Page.Settings.Clear();
            Page.Settings.Report.Name = Resources.Resource.GenericReport;
            Page.Settings.Report.ReportTablesSchemaId = Convert.ToInt32("0" + cboTables.SelectedValue);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (cboTables.Items.Count == 2)
            cboTables.SelectedIndex = 1;
    }
    protected void btnNominalReport_Click(object sender, ImageClickEventArgs e)
    {
        Page.Wizard.ActiveStepIndex = (int)ReportSteps.Columns;
    }
    protected void btnStatisticsReport_Click(object sender, ImageClickEventArgs e)
    {
        Page.Wizard.ActiveStepIndex = (int)ReportSteps.MatrixRows;
    }

    protected void cboReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        Page.LoadSettings(Convert.ToInt32(cboReports.SelectedValue));
        Page.Wizard.ActiveStepIndex = (int)ReportSteps.Finish;
    }
}
