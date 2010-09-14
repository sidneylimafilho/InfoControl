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
    public partial class CompanyContracts : Vivina.Erp.SystemFramework.PageBase
    {
        CompanyConfiguration companyConfigurationUpdated;
        CompanyManager companyManager;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ShowCompanyContract();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            companyManager = new CompanyManager(this);
            companyConfigurationUpdated = new CompanyConfiguration();
            companyConfigurationUpdated.CopyPropertiesFrom(Company.CompanyConfiguration);


            companyConfigurationUpdated.ContractAdditionalValue1Name = txtContractAdicionalValue1Name.Text;
            companyConfigurationUpdated.ContractAdditionalValue2Name = txtContractAdicionalValue2Name.Text;
            companyConfigurationUpdated.ContractAdditionalValue3Name = txtContractAdicionalValue3Name.Text;
            companyConfigurationUpdated.ContractAdditionalValue4Name = txtContractAdicionalValue4Name.Text;
            companyConfigurationUpdated.ContractAdditionalValue5Name = txtContractAdicionalValue5Name.Text;

            companyManager.UpdateCompanyConfiguration(Company.CompanyConfiguration, companyConfigurationUpdated);

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void ShowCompanyContract()
        {
            RefreshCredentials();
            CompanyConfiguration companyConfiguration = new CompanyManager(this).GetCompanyConfiguration(Company.CompanyConfiguration.CompanyConfigurationId);

            txtContractAdicionalValue1Name.Text = companyConfiguration.ContractAdditionalValue1Name;
            txtContractAdicionalValue2Name.Text = companyConfiguration.ContractAdditionalValue2Name;
            txtContractAdicionalValue3Name.Text = companyConfiguration.ContractAdditionalValue3Name;
            txtContractAdicionalValue4Name.Text = companyConfiguration.ContractAdditionalValue4Name;
            txtContractAdicionalValue5Name.Text = companyConfiguration.ContractAdditionalValue5Name;
        }

    }
}
