using System;
using System.Linq;
using System.ComponentModel;
using System.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


public partial class App_Shared_SelectProduct : Vivina.Erp.SystemFramework.UserControlBase
{
    private Product _product;
    private bool _required = false;

    public string Name
    {
        get { return txtProduct.Text; }
        set { txtProduct.Text = value; }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ValidationGroup { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public bool Required
    {
        get { return _required; }
        set { _required = value; }
    }

    public Product Product
    {
        get
        {
            return _product ??
                   (_product = new ProductManager(this).GetProductByName(
                            Page.Company.MatrixId.Value,
                            txtProduct.Text.Split('|')[0].Trim()));
        }
        set
        {
            _product = value;
        }
    }

    public ProductManufacturer Manufacturer
    {
        get
        {
            return Product.ProductManufacturers.Where(p => p.Name == txtProduct.Text.Split('|')[2].Trim()).FirstOrDefault();
        }
    }

    public ProductPackage Package
    {
        get
        {
            if (!String.IsNullOrEmpty(txtProduct.Text.Split('|')[1]))
                return Product.ProductPackages.Where(p => p.Name == txtProduct.Text.Split('|')[1].Trim()).FirstOrDefault();
            else
                return null;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        txtProduct.Attributes["servicepath"] = ResolveUrl("~/Controller/SearchService/SearchProduct");

        valProduct.ValidationGroup = Required
                                         ? ValidationGroup
                                         : "_NonValidation";
    }

    /// <summary>
    /// Show name of product in TextBox
    /// </summary>
    /// <param name="product"></param>
    public void ShowProduct(Product product)
    {
        // essa função deverá mostar o nome do produto

        if (product != null)
        {
            txtProduct.Text = product.Name;
        }
    }

    public class SelectingProductEventArgs : EventArgs
    {
        public string productName { get; set; }
    }
    public class SelectedProductEventArgs : EventArgs
    {
        public Product Product { get; set; }
    }
}