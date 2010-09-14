using System;
using System.Web.UI.WebControls;
using InfoControl;
using Telerik.Web.UI;
using InfoControl.Data;
using InfoControl.Web.Security;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using System.Collections.Generic;
using System.Linq;


[PermissionRequired("OrganizationalLevel")]
public partial class Company_Categories_OrganizationLevel : Vivina.Erp.SystemFramework.PageBase
{
    HumanResourcesManager humanResourcesManager;
    OrganizationLevel organizationLevel;


    public void BindTree()
    {
        humanResourcesManager = new HumanResourcesManager(this);
        treeOL.DataSource = humanResourcesManager.GetAllOrganizationLevelToDataTable((int)Company.CompanyId);
        treeOL.DataBind();
        treeOL.ExpandAllNodes();
        txtOL.Text = String.Empty;
        cboTreeOL.SelectedValue = String.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindTree();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        humanResourcesManager = new HumanResourcesManager(this);
        organizationLevel = new OrganizationLevel();

        if (treeOL.SelectedNode != null)
        {

            OrganizationLevel original_organizationLevel = humanResourcesManager.GetOrganizationLevel(Company.CompanyId,
                Convert.ToInt32(treeOL.SelectedNode.Value));
            organizationLevel.CopyPropertiesFrom(original_organizationLevel);

            organizationLevel.Parentid = null;
            organizationLevel.Name = txtOL.Text.ToUpper();

            if (!String.IsNullOrEmpty(cboTreeOL.SelectedValue))
                organizationLevel.Parentid = Convert.ToInt32(cboTreeOL.SelectedValue);

            humanResourcesManager.UpdateOrganizationLevel(original_organizationLevel, organizationLevel);
        }
        else
        {
            organizationLevel.CompanyId = Company.CompanyId;
            organizationLevel.Name = txtOL.Text.ToUpper();
            organizationLevel.Parentid = null;

            if (!String.IsNullOrEmpty(cboTreeOL.SelectedValue))
                organizationLevel.Parentid = Convert.ToInt32(cboTreeOL.SelectedValue);

            humanResourcesManager.InsertOrganizationLevel(organizationLevel);
        }
        BindTree();
    }

    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "select")
        {
            var node = treeOL.FindNodeByValue(e.CommandArgument.ToString()) ?? new RadTreeNode();

            cboTreeOL.SelectedValue = String.Empty;

            if (node.ParentNode != null)
                cboTreeOL.SelectedValue = node.ParentNode.Value;
          
            txtOL.Text = node.Text;
        }

        if (e.CommandName == "delete")
        {
            humanResourcesManager = new HumanResourcesManager(this);
            organizationLevel = humanResourcesManager.GetOrganizationLevel(Company.CompanyId,
                Convert.ToInt32(e.CommandArgument));

            if (organizationLevel != null)
            {
                IQueryable<OrganizationLevel> childOrganizationLevelList;
                childOrganizationLevelList = humanResourcesManager.GetChildOrganizationLevel(Company.CompanyId, organizationLevel.OrganizationlevelId);

                if (childOrganizationLevelList.Count() != 0)
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    return;
                }
                else
                {
                    try
                    {
                        humanResourcesManager.DeleteOrganizationLevel(organizationLevel);
                        BindTree();
                    }
                    catch (Exception)
                    {
                        ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    }
                }
            }
        }
    }

    protected void odsTreeOrganizationLevel_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnCancelSelection_Click(object sender, EventArgs e)
    {
        BindTree();
    }
}
