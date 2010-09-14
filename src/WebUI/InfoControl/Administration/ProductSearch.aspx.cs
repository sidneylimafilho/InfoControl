using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

using Telerik.Web.UI;

public partial class Product_Search : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindTree();

        treRemove.Attributes["onclick"] = "window['" + treCategory.ClientID + "'].UnSelectAllNodes(); window['" + treCategory.ClientID + "'].UpdateSelectedState();";
    }
    protected void odsManufacturer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyID"] = Company.CompanyId;
    }
    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyID"] = Company.CompanyId;
    }
    protected void odsCategory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Hashtable ht = new Hashtable();

        ht["Name"] = txtName.Text;
        ht["ProductCode"] = txtCode.Text;
        ht["ManufacturerId"] = cboManufacturer.SelectedValue;


        ht["IsActive"] = "";
        if (rbtStatusActive.Checked || rbtStatusInactive.Checked)
            ht["IsActive"] = rbtStatusActive.Checked;

        ht["QuantityStart"] = "";
        if (txtQuantityStart.Text != "0")
            ht["QuantityStart"] = txtQuantityStart.Text;

        ht["QuantityEnd"] = "";
        if (txtQuantityEnd.Text != "0")
            ht["QuantityEnd"] = txtQuantityEnd.Text;

        ht["MinimumStart"] = "";
        if (txtMinimumStart.Text != "0")
            ht["MinimumStart"] = txtMinimumStart.Text;

        ht["MinimumEnd"] = "";
        if (txtMinimumEnd.Text != "0")
            ht["MinimumEnd"] = txtMinimumEnd.Text;

        ht["CompanyId"] = Company.CompanyId;
        ht["DepositId"] = cboDeposit.SelectedValue;

        ht["CategoryId"] = "";
        if (treCategory.SelectedNode != null)
            ht["CategoryId"] = treCategory.SelectedNode.Value;

        Context.Items["HashTable"] = ht;
        Server.Transfer("ProductSearch_Results.aspx");
    }

    protected void odsCategories_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
    protected void treCategory_NodeBound(object o, RadTreeNodeEventArgs e)
    {
        e.Node.Expanded = true;
    }

    public void BindTree()
    {
        CategoryManager categoryManager = new CategoryManager(this);
        treCategory.DataSource = categoryManager.GetCategoriesByCompany(Convert.ToInt16(Company.CompanyId));
        treCategory.DataBind();
    }
}
