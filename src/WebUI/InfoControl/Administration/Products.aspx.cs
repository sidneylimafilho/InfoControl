using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;

[PermissionRequired("Products")]
public partial class Company_Products : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboManufacturer.Items.Add(new ListItem("", "0"));
            cboManufacturer.DataBind();

            if (Page.Customization["cboManufacturer"] != null && cboManufacturer.Items.FindByValue(Convert.ToString(Page.Customization["cboManufacturer"])) != null)
                cboManufacturer.SelectedValue = Convert.ToString(Page.Customization["cboManufacturer"]);

            if (Page.Customization["cboTreeCategory"] != null && cboTreeCategories.ExistItem(Convert.ToString(Page.Customization["cboTreeCategory"])))
                cboTreeCategories.SelectedValue = Convert.ToString(Page.Customization["cboTreeCategory"]);

            txtProductName.Text = Convert.ToString(Page.Customization["ProductName"]);
            txtDescription.Text = Convert.ToString(Page.Customization["Description"]);
            cboPageSize.SelectedValue = Convert.ToString(Page.Customization["cboPageSize"]);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void odsProducts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //
        // The ObjectDataSource have to be the CompanyId setted, to perform the query, in this part of
        // the code, the companyId argument is filled up. 
        //
        e.InputParameters["companyId"] = Company.MatrixId;

        if (!String.IsNullOrEmpty(cboTreeCategories.SelectedValue))
            e.InputParameters["categoryId"] = Convert.ToInt32(cboTreeCategories.SelectedValue);
        else
            e.InputParameters["categoryId"] = null;

        e.InputParameters["manufacturerId"] = Convert.ToInt32(cboManufacturer.SelectedValue);
        e.InputParameters["name"] = txtProductName.Text;
        e.InputParameters["description"] = txtDescription.Text;
        e.InputParameters["isTemp"] = chkIsTemp.Checked;
        e.InputParameters["initialLetter"] = ucAlphabeticalPaging.Letter;
    }

    protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            e.Row.Attributes["onclick"] = "location='Product.aspx?ProductId=" + e.Row.DataItem.GetPropertyValue("ProductId") + "';";
        }
    }

    protected void grdProducts_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Response.Redirect("Product.aspx");
        }
    }

    protected void cboSelectPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdProducts.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdProducts.DataBind();
    }

    [WebMethod]
    public static bool DeleteProduct(int productid)
    {
        bool result = true;
        using (ProductManager productManager = new ProductManager(null))
        {
            try
            {
                productManager.Delete(productManager.GetProduct(productid));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    protected void odsCategories_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsManufacturer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Page.Customization["ProductName"] = txtProductName.Text;
        Page.Customization["Description"] = txtDescription.Text;
        Page.Customization["cboTreeCategory"] = cboTreeCategories.SelectedValue;
        Page.Customization["cboManufacturer"] = cboManufacturer.SelectedValue;
        Page.Customization["cboPageSize"] = cboPageSize.SelectedValue;
        grdProducts.DataBind();
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product_General.aspx");
    }

    protected void ucAlphabeticalPaging_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdProducts.DataBind();
    }
}
