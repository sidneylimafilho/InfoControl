using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.WebUI.InfoControl.Host
{
    public partial class Company : SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request["CompanyId"]))
                {
                    Page.ViewState["CompanyId"] = Request["CompanyId"];
                    ShowCompany();
                }

            }
        }

        private void ShowCompany()
        {
            var companyManager = new CompanyManager(this);
            var company = companyManager.GetCompany(Convert.ToInt32(Page.ViewState["CompanyId"]));

            txtCompanyName.Text = company.LegalEntityProfile.CompanyName;
            txtCompanyPlan.Text = company.Plan.Name;

            if (company.User != null)
                txtLastActivityDate.Text = company.User.LastActivityDate.ToString();

            txtUserQuantity.Text = companyManager.GetCompanyUsersCount(company.CompanyId).ToString();
            txtStartDate.Text = company.StartDate.ToShortDateString();

            var responsableForCompany = companyManager.GetResponsableForCompany(company.CompanyId);

            if (responsableForCompany != null)
                txtEmail.Text = responsableForCompany.Email;
        }
    }
}