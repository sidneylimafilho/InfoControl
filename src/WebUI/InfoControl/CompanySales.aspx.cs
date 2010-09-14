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
    public partial class CompanySales : Vivina.Erp.SystemFramework.PageBase
    {
        CompanyConfiguration companyConfigurationUpdated;
        CompanyManager companyManager;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ShowUnitPricesNames();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            companyManager = new CompanyManager(this);
            companyConfigurationUpdated = new CompanyConfiguration();

            companyConfigurationUpdated.CopyPropertiesFrom(Company.CompanyConfiguration);

            companyConfigurationUpdated.UnitPrice1Name = txtUnitPrice1Name.Text;

            companyConfigurationUpdated.UnitPrice2Name = txtUnitPrice2Name.Text;
            companyConfigurationUpdated.UnitPrice3Name = txtUnitPrice3Name.Text;
            companyConfigurationUpdated.UnitPrice4Name = txtUnitPrice4Name.Text;
            companyConfigurationUpdated.UnitPrice5Name = txtUnitPrice5Name.Text;
            

            companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, companyConfigurationUpdated);

        }

        private void ShowUnitPricesNames()
        {
            RefreshCredentials();
            CompanyConfiguration companyConfiguration = new CompanyManager(this).GetCompanyConfiguration(Company.CompanyConfiguration.CompanyConfigurationId);

            txtUnitPrice1Name.Text = companyConfiguration.UnitPrice1Name;
            txtUnitPrice2Name.Text = companyConfiguration.UnitPrice2Name;
            txtUnitPrice3Name.Text = companyConfiguration.UnitPrice3Name;
            txtUnitPrice4Name.Text = companyConfiguration.UnitPrice4Name;
            txtUnitPrice5Name.Text = companyConfiguration.UnitPrice5Name;

        }
    }
}
