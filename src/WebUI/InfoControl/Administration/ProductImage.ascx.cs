using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using Vivina.InfoControl.BusinessRules;
using Vivina.InfoControl.DataClasses;
using System.Security.AccessControl;

public partial class InfoControl_Administration_ProductImage : Vivina.InfoControl.SystemFramework.Web.UserControlBase
{
    ProductImage productImage;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSaveImage_Click(object sender, EventArgs e)
    {
        ProductManager productManager = new ProductManager(this);
        Product product;
        ProductImage productImages = new ProductImage();
        if (fupProductImage.FileName != null)
        {
            productImages.ProductId = Convert.ToInt32(Page.ViewState["ProductId"]);
            productImages.Description = txtDescription.Text;
            productManager.InsertProductImage(Page.Company.CompanyId, productImages, fupProductImage.PostedFile);
            grdProductImage.DataBind();
        }
    }
    protected void odsProductImage_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["productId"] = Convert.ToInt32(Page.ViewState["ProductId"]);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("products.aspx");
    }
    protected void odsProductImage_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters.Clear();
        e.InputParameters.Add("entity", productImage);
        e.InputParameters.Add("companyId", Page.Company.CompanyId);
    }
    protected void grdProductImage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        productImage = new ProductImage();
        productImage.ProductId = Convert.ToInt32(e.Keys["ProductId"]);
        productImage.ProductImageId = Convert.ToInt32(e.Keys["ProductImageId"]);
        if (e.Keys["ImageUrl"] != null)
            productImage.ImageUrl = e.Keys["ImageUrl"].ToString();
        if (e.Keys["Description"] != null)
            productImage.Description = e.Keys["Description"].ToString();

    }
    protected void grdProductImage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }
}
