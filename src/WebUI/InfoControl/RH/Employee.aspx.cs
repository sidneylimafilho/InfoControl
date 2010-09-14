using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Data;
using InfoControl;
using Telerik.Web.UI;
using InfoControl.Web.Security;

[PermissionRequired("Employee")]
public partial class Company_RH_Employee : Vivina.Erp.SystemFramework.PageBase
{
    Employee originalEmployee;
    HumanResourcesManager humanResourcesManager;

    protected void Page_Load(object sender, EventArgs e)
    {
        humanResourcesManager = new HumanResourcesManager(this);

        if (!IsPostBack)
        {
            //
            //retrieve the customerId from Modal Popup
            //
            if (Request["EmployeeId"] != null)
                Page.ViewState["EmployeeId"] = Request["EmployeeId"].DecryptFromHex();
        }
        if (Page.ViewState["EmployeeId"] != null)
        {
            originalEmployee = humanResourcesManager.GetEmployee(Company.CompanyId, Convert.ToInt32(Page.ViewState["EmployeeId"]));
        }    
    }
}
