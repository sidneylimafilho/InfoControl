using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;

using InfoControl.Web;
using InfoControl.Data;

using Telerik.Web.UI;

[SupportsEventValidation]
[ValidationProperty("AccountingPlanId")]
[ControlValueProperty("AccountingPlanId")]
public partial class App_Shared_AccountingPlan : Vivina.Erp.SystemFramework.UserControlBase
{

    public string AccountingPlanId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        string script = @"
            function nodeClicking(sender, args){
            $get('cboAcountingPlan').innerText = sender.Text;
            $get('cboAcountingPlanMenu').style.display='none';
            $get('" + this.ClientID + @"').value=sender.Value;}";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "nodeClicking", script, true);

        if (IsPostBack)
        {
            if (treeAcountingPlan.SelectedNode != null)
            {
                AccountingPlanId = treeAcountingPlan.SelectedNode.Value;
            }
        }
    }

    protected override void OnDataBinding(EventArgs e)
    {
        base.OnDataBinding(e);

        AccountManager manager = new AccountManager(this);
        DataTable table = manager.GetAccountingPlan(Page.Company.CompanyId);
        treeAcountingPlan.DataSource = table;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        RadTreeNode node = treeAcountingPlan.FindNodeByValue(AccountingPlanId);
        if (node != null)
        {
            node.Selected = true;
            lblAccountingPlanName.Text = node.Text;
        }

        treeAcountingPlan.ExpandAllNodes();
    }
}
