using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;

namespace Vivina.Erp.WebUI.RH
{
    public partial class Employee_Competency : Vivina.Erp.SystemFramework.PageBase
    {
        private Int32 employeeId;
        protected void Page_Load(object sender, EventArgs e)
        {
            employeeId = Convert.ToInt32(Request["eid"]);
            if (!IsPostBack)
                grdCompetency.DataBind();
        }

        protected void odsCompetency_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["employeeId"] = employeeId;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var humanResourcesManager = new HumanResourcesManager(this);
            var competency = new EmployeeCompetency();

            string skillName = txtSkillName.Text.Trim().ToCapitalize();
            competency.Name = skillName;
            competency.EmployeeId = employeeId;
            competency.CompanyId = Company.CompanyId;
            competency.Rating = rtnRanking.CurrentRating;

            humanResourcesManager.SaveEmployeeCompetency(competency);
            txtSkillName.Text = String.Empty;
            rtnRanking.CurrentRating = 0;
            grdCompetency.DataBind();
        }
    }
}
