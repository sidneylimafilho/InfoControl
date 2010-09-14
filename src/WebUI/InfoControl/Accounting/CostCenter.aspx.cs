using System;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Security;

[PermissionRequired("CostCenter")]
public partial class InfoControl_Accounting_CostCenter : Vivina.Erp.SystemFramework.PageBase
{
    AccountManager accountManager;
    CostCenter costCenter;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindTree();
    }

    public void BindTree()
    {
        accountManager = new AccountManager(this);
        treCostCenter.DataSource = accountManager.GetCostsCenterAsDataTable(Company.CompanyId);

        treCostCenter.DataBind();
        treCostCenter.ExpandAllNodes();
        txtName.Text = String.Empty;
        cboTreeCostCenters.SelectedValue = String.Empty;

    }



    protected void btnAddCostCenter_Click(object sender, EventArgs e)
    {
        accountManager = new AccountManager(this);
        costCenter = new CostCenter();

        if (treCostCenter.SelectedNode != null)
        {
            CostCenter original_costCenter = accountManager.GetCostCenter(Convert.ToInt32(treCostCenter.SelectedNode.Value));
            costCenter.CopyPropertiesFrom(original_costCenter);

            if (!String.IsNullOrEmpty(cboTreeCostCenters.SelectedValue))
                costCenter.ParentId = Convert.ToInt32(cboTreeCostCenters.SelectedValue);
            else
                costCenter.ParentId = null;

            costCenter.Name = txtName.Text;
            accountManager.UpdateCostCenter(original_costCenter, costCenter);
        }
        else
        {
            costCenter.CompanyId = Company.CompanyId;
            costCenter.Name = txtName.Text;

            if (!String.IsNullOrEmpty(cboTreeCostCenters.SelectedValue))
                costCenter.ParentId = Convert.ToInt32(cboTreeCostCenters.SelectedValue);
            else
                costCenter.ParentId = null;

            accountManager.InsertCostCenter(costCenter);
        }

        BindTree();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BindTree();
    }

    protected void btnDeleteCostCenter_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            var node = treCostCenter.FindNodeByValue(e.CommandArgument.ToString()) ?? new Telerik.Web.UI.RadTreeNode();

            cboTreeCostCenters.SelectedValue = String.Empty;

            if (node.ParentNode != null)
                cboTreeCostCenters.SelectedValue = node.ParentNode.Value;

            txtName.Text = node.Text;
        }
        else if (e.CommandName == "Delete")
        {
            accountManager = new AccountManager(this);
            costCenter = accountManager.GetCostCenter(Convert.ToInt32(e.CommandArgument));
            if (costCenter != null)
            {

                if (costCenter.CostCenters.Any() || costCenter.Bills.Any() || costCenter.Invoices.Any())
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    return;
                }

                try
                {
                    accountManager.DeleteCostCenter(costCenter);
                }
                catch
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    return;
                }
            }
            BindTree();
        }

    }

    protected void odsCostCenters_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void treCostCenter_NodeBound(object o, RadTreeNodeEventArgs e)
    {
        e.Node.Expanded = true;
    }

}
