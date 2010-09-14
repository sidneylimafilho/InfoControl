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
    public partial class CompanyPrints : Vivina.Erp.SystemFramework.PageBase
    {
        CompanyConfiguration companyConfigurationUpdated;
        CompanyManager companyManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshCredentials();
                txtPrinterFooter.Text = new CompanyManager(this).GetCompanyConfiguration(Company.CompanyConfiguration.CompanyConfigurationId).PrinterFooter;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            companyManager = new CompanyManager(this);
            companyConfigurationUpdated = new CompanyConfiguration();

            companyConfigurationUpdated.CopyPropertiesFrom(Company.CompanyConfiguration);

            companyConfigurationUpdated.PrinterFooter = txtPrinterFooter.Text;
            companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, companyConfigurationUpdated);


        }
    }
}
