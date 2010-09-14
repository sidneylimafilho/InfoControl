using System;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.WebUI.Site;

public partial class Site_Product : CheckoutPageBase
{
    private Product _product;

    public Product Product
    {
        get
        {
            if (_product == null)
            {
                var manager = new ProductManager(this);
                _product = manager.GetProduct(Convert.ToInt32(Request["pid"]), Company.CompanyId);
            }
            return _product;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Product.Name + " - " + Title;
    }
}