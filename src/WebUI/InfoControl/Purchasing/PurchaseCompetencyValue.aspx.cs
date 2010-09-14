using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using Exception = Resources.Exception;

namespace Vivina.Erp.WebUI.Purchasing
{
    [PermissionRequired("PurchaseCompetencyValue")]
    public partial class PurchaseCompetencyValue : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlPurchaseCeilingValue.Visible = false;
        }

        protected void odsCompetency_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(rtvCompetencyValue.SelectedValue) < 0)
            {
                ShowError(Exception.UnselectedEmployee);
                return;
            }

            var humanResourcesManager = new HumanResourcesManager(this);
            Employee employee = humanResourcesManager.GetEmployee(Company.CompanyId,
                                                            Convert.ToInt32(rtvCompetencyValue.SelectedValue));
            if (employee == null)
            {
                ShowError(Exception.unavailableEmployee);
                return;
            }

            humanResourcesManager.SetEmployeeCompetency(employee,
                                                  ucCurrFieldPurchaseCeilingValue.CurrencyValue,
                                                  rbtCentralBuyer.Checked);

            rtvCompetencyValue.DataBind();

            pnlPurchaseCeilingValue.Visible = false;
            ucCurrFieldPurchaseCeilingValue.CurrencyValue = null;
        }

        protected void rtvCompetencyValue_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            var humanResourcesManager = new HumanResourcesManager(this);
            Employee employee = humanResourcesManager.GetEmployee(Company.CompanyId,
                                                            Convert.ToInt32(rtvCompetencyValue.SelectedValue));

            if (employee == null)
                return;

            pnlPurchaseCeilingValue.Visible = Convert.ToInt32(rtvCompetencyValue.SelectedValue) > 0;
            rbtCentralBuyer.Checked = employee.CentralBuyer ?? false;
        }

        protected void rtvCompetencyValue_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {

        }
    }
}