using System;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


public partial class InfoControl_Administration_ProductImage : Vivina.Erp.SystemFramework.PageBase
{
    private int productId;
    private int productImageId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["pid"])) 
            productId = Convert.ToInt32(Request["pid"].DecryptFromHex());

        DataBind();
    }

    protected void btnSaveImage_Click(object sender, EventArgs e) 
    {
        var productManager = new ProductManager(this);

        if (fupProductImage.FileName != null)
        {
            productManager.InsertProductImage(Company,
                                              new ProductImage
                                              {
                                                  ProductId = productId,
                                                  Description = txtDescription.Text
                                              },
                                              fupProductImage.PostedFile);
            lstProductImages.DataBind();
            txtDescription.Text = String.Empty;
        }
    }

    protected void odsProductImage_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["productId"] = productId;
    }

    protected void odsProductImage_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters.Clear();
        e.InputParameters.Add("productImageId", productImageId);
        e.InputParameters.Add("companyId", Company.CompanyId);
    }

    protected void lstProductImages_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        productImageId = (int) e.Keys["ProductImageId"];
    }
}