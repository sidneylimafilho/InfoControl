using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


using InfoControl;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Inventory")]
public partial class Company_Inventory : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
        }
    }
    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    protected void odsInventory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        grdProducts.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);


        e.InputParameters["depositId"] = Convert.ToInt32(cboDeposit.SelectedValue);
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["name"] = txtProductName.Text;
        e.InputParameters["description"] = txtDescription.Text;
        e.InputParameters["manufacturerId"] = null;
        e.InputParameters["categoryId"] = null;

        if (!String.IsNullOrEmpty(cboManufacturer.SelectedValue))
            e.InputParameters["manufacturerId"] = Convert.ToInt32(cboManufacturer.SelectedValue);

        if (!String.IsNullOrEmpty(cboTreeCategories.SelectedValue))
            e.InputParameters["categoryId"] = Convert.ToInt32(cboTreeCategories.SelectedValue);

    }
    protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (cboDeposit.SelectedValue != "Matrix" && cboDeposit.SelectedValue != "Company")
                e.Row.Attributes["onclick"] = "location='Inventory.aspx?ProductId=" + grdProducts.DataKeys[e.Row.RowIndex]["ProductId"] + "&DepositId=" + cboDeposit.SelectedValue + "&InventoryId=" + grdProducts.DataKeys[e.Row.RowIndex]["InventoryId"] + "';";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grdProducts.DataBind();
    }
}
