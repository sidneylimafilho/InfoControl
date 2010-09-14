using System;
using System.Data;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using Telerik.Web.UI;
using InfoControl.Web.Security;

[PermissionRequired("AccountingPlan")]
public partial class Company_Accounting_AccountingPlan : Vivina.Erp.SystemFramework.PageBase
{
    AccountManager accountManager;
    AccountingPlan accountingPlan;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {   
            BindTree();
        }
    }

    //This method meets the tree with the plans of accounts
    public void BindTree()
    {
        accountManager = new AccountManager(this);
        DataTable table = accountManager.GetAccountingPlan(Company.CompanyId);
        treAccountingPlan.DataSource = table;
        treAccountingPlan.DataBind();
        treAccountingPlan.ExpandAllNodes();
        cboTreeAccountingPlan.SelectedValue = "";
        txtName.Text = "";
    }

    protected void treAccountingPlan_NodeBound(object o, RadTreeNodeEventArgs e)
    {
        e.Node.Expanded = true;
    }

    protected void btnAddAccountingPlan_Click(object sender, EventArgs e)
    {
        accountManager = new AccountManager(this);
        accountingPlan = new AccountingPlan();

        if (String.IsNullOrEmpty(cboTreeAccountingPlan.SelectedValue))
        {
            ShowError("Selecione um plano de contas pai!");
            return;
        }

        if (treAccountingPlan.SelectedNode != null)
        {
            AccountingPlan original_accountingPlan = accountManager.GetAccountingPlan(Company.CompanyId,Convert.ToInt32(treAccountingPlan.SelectedValue));
            
            accountingPlan.CopyPropertiesFrom(original_accountingPlan);

            accountingPlan.Name = txtName.Text.ToUpper();

            accountingPlan.ParentId = Convert.ToInt32(cboTreeAccountingPlan.SelectedValue);

            accountManager.UpdateAccountingPlan(original_accountingPlan, accountingPlan);
        }
        else
        {
            accountingPlan.CompanyId = Company.CompanyId;
            accountingPlan.Name = txtName.Text.ToUpper();

            accountingPlan.ParentId = Convert.ToInt32(cboTreeAccountingPlan.SelectedValue);

            accountManager.InsertAccountingPlan(accountingPlan);
        }
        BindTree();
    }
    protected void btnCancelSelection_Click(object sender, EventArgs e)
    {
        BindTree();
    }

    protected void btnDeleteAccoutingPlan_Command(object sender, CommandEventArgs e)
    {

        if (treAccountingPlan.SelectedNode == null)
        {
            ShowError("Selecione um plano de contas!");
            return;
        }


        if (e.CommandName == "Select")
        {
            var node = treAccountingPlan.FindNodeByValue(e.CommandArgument.ToString());

            cboTreeAccountingPlan.SelectedValue = String.Empty;

            if (node.ParentNode != null)
                cboTreeAccountingPlan.SelectedValue = node.ParentNode.Value;

            txtName.Text = node.Text;
        }
        else if (e.CommandName == "Delete")
        {
            accountingPlan = new AccountManager(this).GetAccountingPlan(Company.CompanyId, Convert.ToInt32(treAccountingPlan.SelectedNode.Value));
            accountManager = new AccountManager(this);
            switch (accountManager.DeleteAccountingPlan(accountingPlan))
            {
                case AccountManager.AccountPlanDeleteStatus.DeletingRegisterWithForeignKey:
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    break;
                case AccountManager.AccountPlanDeleteStatus.ExistsAssociatedAccountPlan:
                    ShowError(Resources.Exception.ExistsChildCategories);
                    break;
                default:
                    accountManager.DeleteAccountingPlan(accountingPlan);
                    BindTree();
                    break;
            }



        }

    }

    protected void odsCostCenters_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

}
