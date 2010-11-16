using System;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


namespace Vivina.Erp.WebUI.Administration
{
    public partial class Product_Manufacturer : Vivina.Erp.SystemFramework.PageBase
    {
        private int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["pid"]))
                productId = Convert.ToInt32(Request["pid"]);
        }

        protected void odsProductManufactures_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["productId"] = productId;
        }

        protected void btnSaveFabricante_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtManufacturer.Text))
                return;

            var productManager = new ProductManager(this);
            productManager.InsertProductManufacturer(productId,
                                                     new ProductManufacturer
                                                     {
                                                         Name = txtManufacturer.Text
                                                     });

            grdManufacturer.DataBind();
            txtManufacturer.Text = String.Empty;
        }
    }
}