using System;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


[PermissionRequired("Categories")]
public partial class Company_Categories : Vivina.Erp.SystemFramework.PageBase
{
    private CategoryManager _categoryManager;
    Category category;

    public CategoryManager CategoryManager
    {
        get
        {
            if (_categoryManager == null)
                _categoryManager = new CategoryManager(this);

            return _categoryManager;
        }
    }

    public void BindTree()
    {
        treCategory.DataBind();
        treCategory.ExpandAllNodes();
        txtCategory.Text = String.Empty;
        cboTreeCategory.SelectedValue = String.Empty;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindTree();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        category = new Category();

        if (treCategory.SelectedNode != null)
        {
            Category original_category = CategoryManager.GetCategory(Convert.ToInt32(treCategory.SelectedNode.Value));
            category.CopyPropertiesFrom(original_category);

            category.ParentId = null;
            if (!String.IsNullOrEmpty(cboTreeCategory.SelectedValue))
                category.ParentId = Convert.ToInt32(cboTreeCategory.SelectedValue);

            category.Name = txtCategory.Text.ToUpper();
            CategoryManager.Update(original_category, category);
        }
        else
        {
            category.CompanyId = (int)Company.MatrixId;
            category.Name = txtCategory.Text.ToUpper();

            category.ParentId = null;
            if (!String.IsNullOrEmpty(cboTreeCategory.SelectedValue))
                category.ParentId = Convert.ToInt32(cboTreeCategory.SelectedValue);

            category.ModifiedDate = DateTime.Now;
            CategoryManager.Insert(category);
        }
        BindTree();
    }

    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        if (treCategory.SelectedNode == null)
        {
            ShowError(Resources.Exception.SelectCategory);
            return;
        }

        if (e.CommandName == "select")
        {
            RadTreeNode node = treCategory.FindNodeByValue(e.CommandArgument.ToString()) ?? new RadTreeNode();
            txtCategory.Text = node.Text;

            cboTreeCategory.SelectedValue = String.Empty; 

            if (node.ParentNode != null)
                cboTreeCategory.SelectedValue = node.ParentNode.Value;
                
        }

        if (e.CommandName == "delete")
        {
            Category category = CategoryManager.GetCategory(Convert.ToInt32(e.CommandArgument));

            if (category != null)
            {
                if (treCategory.SelectedNode.Nodes.Count != 0)
                {
                    ShowError(Resources.Exception.ExistsChildCategories);
                    return;
                }
                if (category.Products.Any())
                {
                    ShowError(Resources.Exception.ExistsProductInCategory);
                    return;
                }

                CategoryManager.Delete(category);
            }
            BindTree();
        }
    }

    protected void odsCategory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.MatrixId;
    }

    protected void btnCancelSelection_Click(object sender, EventArgs e)
    {
        BindTree();
    }

}
