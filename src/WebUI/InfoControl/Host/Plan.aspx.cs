using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;

namespace Vivina.Erp.WebUI.Host
{
    public partial class Plan : Vivina.Erp.SystemFramework.PageBase
    {

        DataClasses.Plan plan;
        DataClasses.Plan originalPlan;
        PlanManager planManager;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request["PlanId"]))
                    ShowPlan();

            }


        }

        /// <summary>
        /// This method just show plan's data in fields of screen
        /// </summary>
        private void ShowPlan()
        {
            planManager = new PlanManager(this);

            originalPlan = planManager.GetPlan(Convert.ToInt32(Request["PlanId"].DecryptFromHex()));


            txtName.Text = originalPlan.Name;
            ucDateTimeInterval.DateInterval = new DateTimeInterval(originalPlan.AvailableStartDate, originalPlan.AvailableEndDate);

            // cboBranch.SelectedValue = Convert.ToString(originalPlan.BranchId);
            cboApplication.SelectedValue = Convert.ToString(originalPlan.ApplicationId);
            cboPackage.SelectedValue = Convert.ToString(originalPlan.PackageId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePlan();

        }
        /// <summary>
        /// This method fill an new plan object and send them to bd for update or insert
        /// </summary>
        private void SavePlan()
        {
            planManager = new PlanManager(this);

            plan = new DataClasses.Plan();
            if(!String.IsNullOrEmpty(Request["PlanId"]))
               plan =  planManager.GetPlan(Convert.ToInt32(Request["PlanId"].DecryptFromHex()));

            plan.Name = txtName.Text;
            plan.AvailableStartDate = ucDateTimeInterval.DateInterval.BeginDate;
            plan.AvailableEndDate = ucDateTimeInterval.DateInterval.EndDate;
            plan.ApplicationId = Convert.ToInt32(cboApplication.SelectedValue);
            plan.PackageId = Convert.ToInt32(cboPackage.SelectedValue);

            planManager.Save(plan);

            Response.Redirect("Plans.aspx");

        }
    }
}
