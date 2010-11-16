using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Telerik.Web.UI;

namespace Vivina.Erp.WebUI.RH
{
    public partial class Employee_ProfessionalData : Vivina.Erp.SystemFramework.PageBase
    {
        private HumanResourcesManager _humanResourcesManager;
        private int employeeId;

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
            pnlAdditionalFields.Visible = dtlAdditionalInformation.Items.Count > 0;

            if (!String.IsNullOrEmpty(Request["eid"]))
                employeeId = Convert.ToInt32(Request["eid"]);

            if (!IsPostBack && employeeId != 0)
                ShowEmployee();
        }

        protected void odsOrganizationLevel_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;

        }

        protected void odsEmployeeFunction_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        #region Employee Profissional Data


        public void ShowEmployee()
        {
            var employee = HumanResourcesManager.GetEmployee(Company.CompanyId, employeeId);

            txtEnrollment.Text = employee.Enrollment;
            ucCurrComission.CurrencyValue = employee.Comission;
            ucCurrrFieldHoursWeek.CurrencyValue = employee.WorkingHours;

            if (employee.OrganizationlevelId.HasValue)
                cboOrgLevel.SelectedValue = employee.OrganizationlevelId.ToString();

            if (employee.BondId.HasValue)
                cboBond.SelectedValue = employee.BondId.ToString();

            if (employee.EmployeeFunctionId.HasValue)
                cboPost.SelectedValue = employee.EmployeeFunctionId.ToString();

            if (employee.ShiftId.HasValue)
                cboShift.SelectedValue = employee.ShiftId.ToString();

            if (employee.WorkJourneyId.HasValue)
                cboWorkJourney.SelectedValue = employee.WorkJourneyId.ToString();

            if (employee.IsSalesperson)
                chkIsSalesPerson.Checked = employee.IsSalesperson;

            if (employee.IsTechnical.HasValue)
                chkIsTechnical.Checked = employee.IsTechnical.Value;

            if (employee.BankId.HasValue)
                cboBank.SelectedValue = employee.BankId.ToString();

            if (employee.JourneyBegin.HasValue)
                txtJourneyBegin.Text = employee.JourneyBegin.Value.ToShortTimeString();

            if (employee.JourneyEnd.HasValue)
                txtJourneyEnd.Text = employee.JourneyEnd.Value.ToShortTimeString();

            if (employee.IntervalBegin.HasValue)
                txtIntervalBegin.Text = employee.IntervalBegin.Value.ToShortTimeString();

            if (employee.IntervalEnd.HasValue)
                txtIntervalEnd.Text = employee.IntervalEnd.Value.ToShortTimeString();


            ucDtAdmissionDate.DateTime = employee.AdmissionDate;
            ucCurrFieldSalary.CurrencyValue = employee.Salary;
            txtAccountingNumber.Text = employee.AccountNumber;
            txtAgency.Text = employee.Agency;
            ucCurrFieldtxtHH.CurrencyValue = employee.HH;
            txtCtps.Text = employee.Ctps;
            txtCtpsSerial.Text = employee.CtpsSerial;
            txtPis.Text = employee.Pis;

            if (employee.IsActive ?? true)
                rbtActive.Checked = true;
            else
            {
                rbtAway.Checked = true;
                cboEmployeeAlienationCause.SelectedValue = employee.AlienationId.ToString();
                ucDtEmployeeAwayDate.DateTime = employee.AlienationDate;
            }
        }

        protected void treeOrgLevel_NodeBound(object o, RadTreeNodeEventArgs e)
        {
            e.Node.Expanded = true;
        }

        protected void lisAdditionalInformation_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //
            // When a item come to the reader, it automatically searchs the AdditionalInformationData
            // to fill the ComboBox with the related value.
            //
            DropDownList cboOrg = e.Item.Controls[5] as DropDownList;
            HumanResourcesManager hManager = new HumanResourcesManager(this);
            cboOrg.DataSource = hManager.GetAdditionalInformationData(Company.CompanyId, Convert.ToInt32((e.Item.Controls[1] as Label).Text));
            cboOrg.DataTextField = "Name";
            cboOrg.DataValueField = "AddonInfoDataId";
            cboOrg.DataBind();
            //
            // If the form are in the update mode, the combobox will bind to the correct value, automatically
            //
            if (employeeId != null)
            {
                EmployeeAdditionalInformation addInfo = hManager.GetEmployeeAdditionalInformation
                    (
                    Company.CompanyId,
                    Convert.ToInt32((e.Item.Controls[1] as Label).Text),
                    employeeId
                    );
                if (addInfo != null)
                    cboOrg.SelectedValue = addInfo.AddonInfoDataId.ToString();
            }
        }

        /// <summary>
        /// Populate the employee object to be inserted/updated. Need to be revise
        /// since the button is outside of Iframe
        /// </summary>
        /// <param name="employee"></param>
        public void HidrateEmployee(Employee employee)
        {
            //
            // Initialize nullables fields 
            //

            employee.OrganizationlevelId = null;
            employee.BankId = null;
            employee.AlienationId = null;
            employee.AlienationDate = null;

            employee.ConfederacyContributionId = null;
            employee.AssociatedContribution1Id = null;
            employee.AssociatedContribution2Id = null;
            employee.SindicalContributionId = null;

            employee.SupportContributionId = null;

            employee.Agency = txtAgency.Text;
            employee.AccountNumber = txtAccountingNumber.Text;
            employee.Salary = ucCurrFieldSalary.CurrencyValue;
            employee.HH = ucCurrFieldtxtHH.CurrencyValue;

            employee.Ctps = txtCtps.Text;
            employee.CtpsSerial = txtCtpsSerial.Text;
            employee.Pis = txtPis.Text;
            employee.ModifiedDate = DateTime.Now;

            employee.WorkingHours = ucCurrrFieldHoursWeek.IntValue;
            employee.IsSalesperson = chkIsSalesPerson.Checked;
            employee.IsTechnical = chkIsTechnical.Checked;
            employee.Comission = ucCurrComission.CurrencyValue;

            employee.AdmissionDate = ucDtAdmissionDate.DateTime;
            employee.Enrollment = txtEnrollment.Text;
            employee.CompanyId = Company.CompanyId;

            if (!String.IsNullOrEmpty(cboOrgLevel.SelectedValue))
                employee.OrganizationlevelId = Convert.ToInt32(cboOrgLevel.SelectedValue);

            if (!String.IsNullOrEmpty(cboBank.SelectedValue))
                employee.BankId = Convert.ToInt32(cboBank.SelectedValue);

            if (ucCurrFieldWorkingHours.CurrencyValue.HasValue)
                employee.WorkingHours = Convert.ToInt32(ucCurrFieldWorkingHours.CurrencyValue.Value);

            if (ucCurrFieldFGTS.CurrencyValue.HasValue)
                employee.Fgts = ucCurrFieldFGTS.CurrencyValue.Value;

            if (ucCurrFieldHealthFuless.CurrencyValue.HasValue)
                employee.Healthfulless = ucCurrFieldHealthFuless.CurrencyValue.Value;

            if (ucCurrFieldHazardPay.CurrencyValue.HasValue)
                employee.HazardPay = ucCurrFieldHazardPay.CurrencyValue.Value;

            if (ucCurrFieldOtherIncomes.CurrencyValue.HasValue)
                employee.OtherIncomes = ucCurrFieldOtherIncomes.CurrencyValue.Value;

            if (ucCurrFieldAnuency.CurrencyValue.HasValue)
                employee.Anuency = ucCurrFieldAnuency.CurrencyValue.Value;

            if (!String.IsNullOrEmpty(cboBond.SelectedValue))
                employee.BondId = Convert.ToInt32(cboBond.SelectedValue);

            if (!String.IsNullOrEmpty(cboPost.SelectedValue))
                employee.PostId = Convert.ToInt32(cboPost.SelectedValue);

            if (!String.IsNullOrEmpty(cboShift.SelectedValue))
                employee.ShiftId = Convert.ToInt32(cboShift.SelectedValue);

            if (!String.IsNullOrEmpty(cboWorkJourney.SelectedValue))
                employee.WorkJourneyId = Convert.ToInt32(cboWorkJourney.SelectedValue);

            if (!String.IsNullOrEmpty(txtJourneyBegin.Text))
                employee.JourneyBegin = Convert.ToDateTime(txtJourneyBegin.Text);

            if (!String.IsNullOrEmpty(txtJourneyEnd.Text))
                employee.JourneyEnd = Convert.ToDateTime(txtJourneyEnd.Text);

            if (!String.IsNullOrEmpty(txtIntervalBegin.Text))
                employee.IntervalBegin = Convert.ToDateTime(txtIntervalBegin.Text);

            if (!String.IsNullOrEmpty(txtIntervalEnd.Text))
                employee.IntervalEnd = Convert.ToDateTime(txtIntervalEnd.Text);

            if (rbtAway.Checked)
            {
                employee.IsActive = false;
                employee.AlienationId = Convert.ToInt32(cboEmployeeAlienationCause.SelectedValue);
                employee.AlienationDate = ucDtEmployeeAwayDate.DateTime;
            }
            else
                employee.IsActive = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //pnlEmployeeAway.Visible = rbtAway.Checked;
        }

        #endregion



        protected void dtlAdditionalInformation_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var cboOrg = e.Item.Controls[5] as DropDownList;
            int addonInfoId = Convert.ToInt16(e.Item.DataItem.GetPropertyValue("AddonInfoId"));



            //
            // When a item come to the reader, it automatically searchs the AdditionalInformationData
            // to fill the ComboBox with the related value.
            //

            //
            // Populate combos
            //
            cboOrg.Attributes["addonInfoId"] = addonInfoId.ToString();

            cboOrg.DataSource = HumanResourcesManager.GetAdditionalInformationData(Company.CompanyId, addonInfoId);
            cboOrg.DataTextField = "Name";
            cboOrg.DataValueField = "AddonInfoDataId";


            cboOrg.DataBind();

            if (employeeId != 0)
            {
                EmployeeAdditionalInformation addInfo = HumanResourcesManager.GetEmployeeAdditionalInformation(Company.CompanyId, addonInfoId, employeeId);

                if (addInfo != null)
                    cboOrg.SelectedValue = addInfo.AddonInfoDataId.ToString();
            }
        }


        protected void odsAdditionalInformation_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var employee = new Employee();

            if (employeeId != 0)
            {
                var originalEmployee = HumanResourcesManager.GetEmployee(Company.CompanyId, employeeId);
                employee.CopyPropertiesFrom(originalEmployee);
            }

            employee.ModifiedByUser = User.Identity.UserName;

            HidrateEmployee(employee);
            HumanResourcesManager.SaveEmployee(employee);

            if (employeeId == 0)
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Employees.aspx';", true);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["eid"]))
                employeeId = Convert.ToInt32(Request["eid"]);

            if (employeeId != 0)
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "parent.location='Employees.aspx';", true);
            else
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Employees.aspx';", true);
        }
    }
}
