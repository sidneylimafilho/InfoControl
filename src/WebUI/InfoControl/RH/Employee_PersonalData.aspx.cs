using System;
using System.Web.UI;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.WebUI.RH
{
    public partial class Employee_PersonalData : Vivina.Erp.SystemFramework.PageBase
    {
        private HumanResourcesManager _humanResourcesManager;
        private int employeeId = 0;

        public HumanResourcesManager HumanResourcesManager
        {
            get
            {
                if (_humanResourcesManager == null)
                    _humanResourcesManager = new HumanResourcesManager(this);

                return _humanResourcesManager;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["eid"]))
                employeeId = Convert.ToInt32(Request["eid"].DecryptFromHex());
            else
                litTitle.Visible = true;

            if (!IsPostBack && employeeId != 0)
                ucProfile.ProfileEntity = HumanResourcesManager.GetEmployee(Company.CompanyId, employeeId).Profile;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var employee = HumanResourcesManager.GetEmployeeByProfile(ucProfile.ProfileEntity.ProfileId, Company.CompanyId) ?? new Employee();

            employee.CompanyId = Company.CompanyId;
            employee.Profile = ucProfile.ProfileEntity;

            if (employee.EmployeeId == 0)
                employee.CreatedByUser = User.Identity.UserName;
            else
                employee.ModifiedByUser = User.Identity.UserName;

            HumanResourcesManager.SaveEmployee(employee);

            if (Request["w"] == "modal")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "top.tb_remove();", true);
                return;
            }

            if (employeeId != 0)
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "parent.location='Employee.aspx?EmployeeId=" + employee.EmployeeId.EncryptToHex() + "';", true);
            else
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Employee.aspx?EmployeeId=" + employee.EmployeeId.EncryptToHex() + "';", true);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (Request["w"] == "modal")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "top.$.modal.Hide();", true);
                return;
            }

            if (employeeId != 0)
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "parent.location='Employees.aspx';", true);
            else
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Employees.aspx';", true);
        }
    }
}
