using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;
using InfoControl;

using Telerik.Web.UI;

public partial class Company_Administration_ProductParts : Vivina.Erp.SystemFramework.PageBase
{

    public void BindTree()
    {
        
        
        ProductManager manager = new ProductManager(this);
        DataTable table = manager.GetProductPartsByCompany((int)Company.MatrixId).Sort("ParentId").ToDataTable();
        RadTreeView1.DataSource = table;
        RadTreeView1.DataBind();

        lblDelete.Visible = RadTreeView1.Nodes.Count > 0;
        btnDelete.Visible = RadTreeView1.Nodes.Count > 0;
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.Items["Error"] != null)
        {
            ShowError(Context.Items["Error"].ToString());
        }
        if (!IsPostBack)
            BindTree();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ProductManager manager = new ProductManager(this);
        manager.InsertProductPart(
            new ProductPart
            {
                Name = txtProductPart.Text,
                ParentId = RadTreeView1.SelectedNode != null ? Convert.ToInt32(RadTreeView1.SelectedNode.Value) : (int?)null,
                ModifiedDate = DateTime.Now,
                //CompanyId = (int)Company.MatrixId,
                Quantity = Convert.ToInt16(txtQuantity.Text)
            });
        BindTree();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (RadTreeView1.SelectedNode == null)
        {
            ShowError("Selecione uma categoria!");
            return;
        }

        ProductManager manager = new ProductManager(this);
        ProductPart productPart = manager.GetProductPart(Convert.ToInt32(RadTreeView1.SelectedNode.Value));
        productPart.Name = txtProductPart.Text;
        productPart.Quantity = Convert.ToInt16(txtQuantity.Text);
        manager.DbContext.SubmitChanges();
        //DataManager.CurrentContext.SubmitChanges();

        BindTree();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (RadTreeView1.SelectedNode == null)
        {
            ShowError("Selecione uma categoria!");
            return;
        }
        ProductManager manager = new ProductManager(this);
        ProductPart productPart = manager.GetProductPart(Convert.ToInt32(RadTreeView1.SelectedNode.Value));

        try
        {
            manager.DeleteProductPart(productPart);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message.ToString());
            return;
        }

        BindTree();
    }
    protected void RadTreeView1_NodeBound(object o, RadTreeNodeEventArgs e)
    {
        ProductManager manager = new ProductManager(this);
        ProductPart productPart = manager.GetProductPart(Convert.ToInt32(e.Node.Value));
        e.Node.Text += " (" + productPart.Quantity + ")";
        e.Node.Expanded = true;
    }
}
