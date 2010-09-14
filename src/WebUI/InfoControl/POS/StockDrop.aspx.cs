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
using InfoControl.Web.Security;

[PermissionRequired("StockDrop")]
public partial class Company_Stock_StockDrop : Vivina.Erp.SystemFramework.PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }
    protected void odsInventory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["depositId"] = Convert.ToInt32(cboDeposit.SelectedValue);
    }
    protected void cboDeposit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty((sender as DropDownList).SelectedValue))
        {
            cboProducts.Enabled = true;
            cboProducts.DataBind();
            cboProducts.DataSourceID = "odsInventory";
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        InventoryManager iManager = new InventoryManager(this);
        Inventory inventory = new Inventory();

        inventory.ProductId = Convert.ToInt32(cboProducts.SelectedValue);
        inventory.DepositId = Convert.ToInt32(cboDeposit.SelectedValue);
        inventory.CompanyId = Company.CompanyId;

        try
        {
            iManager.InventoryDrop(inventory, ucCurrFieldQuantity.IntValue, (int)DropType.Transfer, null);
        }
        catch (InvalidOperationException ex)
        {

            ShowError(Resources.Exception.InvalidStockQuantity);
            return;

        }

        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Baixa no Estoque efetuada com sucesso!');", true);

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        cboDeposit.ClearSelection();
        cboProducts.ClearSelection();
        cboProducts.Enabled = false;
        ucCurrFieldQuantity.Text = String.Empty;
    }
}

