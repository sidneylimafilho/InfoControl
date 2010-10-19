using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Data;
using InfoControl;

namespace Vivina.Erp.WebUI
{
    public partial class CompanyReports : Vivina.Erp.SystemFramework.PageBase
    {

        CompanyConfiguration companyConfigurationUpdated;
        CompanyManager companyManager;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ShowReport();


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            companyConfigurationUpdated = new CompanyConfiguration();
            companyManager = new CompanyManager(this);
            companyConfigurationUpdated.CopyPropertiesFrom(Company.CompanyConfiguration);

            companyConfigurationUpdated.ReportFooter = txtFooter.Value;
            companyConfigurationUpdated.ReportHeader = txtHeader.Value;
            companyConfigurationUpdated.ReportMarginTop = txtReportUp.Text;
            companyConfigurationUpdated.ReportMarginRight = txtReportRight.Text;
            companyConfigurationUpdated.ReportMarginLeft = txtReportLeft.Text;
            companyConfigurationUpdated.ReportMargimBottom = txtReportBottom.Text;

            companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, companyConfigurationUpdated);



        }

        private void ShowReport()
        {
            RefreshCredentials();
            CompanyConfiguration companyConfiguration = new CompanyManager(this).GetCompanyConfiguration(Company.CompanyConfiguration.CompanyConfigurationId);

            txtFooter.Value = companyConfiguration.ReportFooter;
            txtHeader.Value = companyConfiguration.ReportHeader;
            txtReportBottom.Text = companyConfiguration.ReportMargimBottom;
            txtReportLeft.Text = companyConfiguration.ReportMarginLeft;
            txtReportRight.Text = companyConfiguration.ReportMarginRight;
            txtReportUp.Text = companyConfiguration.ReportMarginTop;

        }
    }
}
