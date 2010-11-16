using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


public partial class Company_Products_Composite : Vivina.Erp.SystemFramework.PageBase
{
    private int productId;

    protected void Page_Load(object sender, EventArgs e)
    {
        productId = Convert.ToInt32(Request["pid"]);
    }

    protected void odsComposite_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompositeProductId"] = productId;
    }

    protected void grdComposites_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var postOptions = new PostBackOptions(grdComposites, "Select$" + e.Row.RowIndex);
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void grdComposites_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Page.ViewState["CompositeId"] = Convert.ToInt32(grdComposites.DataKeys[e.RowIndex]["CompositeId"]);
    }

    protected void odsComposite_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters.Clear();
        e.InputParameters["CompositeId"] = Page.ViewState["CompositeId"];
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {

        if (!ucSelectProduct.Name.Contains("|"))
        {
            ShowError("O produto integrante da composição deve ter embalagem para aparecer na lista.");
            return;
        }


        if (ucSelectProduct.Package == null)
        {
            ShowError("O produto integrante da composição deve ter embalagem para aparecer na lista.");
            return;
        }

        //
        // Populate the entity CompositeProduct to be inserted 
        //
        var compositeProduct = new CompositeProduct
                               {
                                   Amount = Convert.ToInt32(ucCurrFieldQuantity.Text),
                                   CompositeProductId = productId,
                                   ProductManufacturerId = ucSelectProduct.Manufacturer.ProductManufacturerId,
                                   ProductPackageId = ucSelectProduct.Package.ProductPackageId,
                                   ProductId = ucSelectProduct.Product.ProductId
                               };

        new ProductManager(this).AddComposite(compositeProduct);
        grdComposites.DataBind();
        ucSelectProduct.Name = String.Empty;
        ucCurrFieldQuantity.CurrencyValue = null;
    }
}