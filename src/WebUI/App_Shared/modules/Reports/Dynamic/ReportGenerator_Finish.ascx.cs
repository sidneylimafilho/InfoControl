using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Web;
using InfoControl.Web.Reporting;
using InfoControl.Web.Reporting.DataClasses;

public partial class ReportGenerator_Finish : ReportStepControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        tblSave.Visible = Page.User.IsAuthenticated;


    }
    protected void btnGeraRelatorio_Click(object sender, EventArgs e)
    {

    }
    protected void btnGravaRelatorio_Click(object sender, EventArgs e)
    {
        Page.Settings.Report.Name = txtReportTitle.Text;
        Page.SaveReportSettings();
    }
}
