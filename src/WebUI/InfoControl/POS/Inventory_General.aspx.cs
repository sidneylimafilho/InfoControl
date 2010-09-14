using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

namespace Vivina.Erp.WebUI.POS
{
    public partial class Inventory_General : Vivina.Erp.SystemFramework.PageBase
    {
        InventoryManager inventoryManager;
        Inventory original_Inventory;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request["ProductId"] != null)
            {
                Page.ViewState["ProductId"] = Request["ProductId"].DecryptFromHex();
                Page.ViewState["DepositId"] = Request["DepositId"].DecryptFromHex();
                showInventory();
                SetUnitPriceNames();

                btnPrintBarCode.OnClientClick = "top.tb_show(" + "'Impressão de Código de Barras'," + "'../InfoControl/BarCode_Print.aspx?DepositId=" +
                                                Page.ViewState["DepositId"].EncryptToHex() +
                                                "&ProductId=" + Page.ViewState["ProductId"].EncryptToHex() +
                                                "'); return false;";
            }
        }

        protected void odsSupplier_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["matrixId"] = (int)Company.MatrixId;
        }

        protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void odsCurrencyRate_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            inventoryManager = new InventoryManager(this);
            Inventory inventory = new Inventory();
            original_Inventory = inventoryManager.GetInventory(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProductId"]), Convert.ToInt32(Page.ViewState["DepositId"]));

            if (original_Inventory == null)
                return;
            inventory.CopyPropertiesFrom(original_Inventory);
            inventory.FiscalNumber = txtFiscalNumber.Text;
            inventory.SupplierId = Convert.ToInt32(cboSupplier.SelectedValue);
            inventory.MinimumRequired = ucCurrFieldMinimunRequired.IntValue;
            inventory.Localization = txtLocalization.Text;
            inventory.RealCost = uctxtRealCost.CurrencyValue.Value;
            inventory.Profit = uctxtProfit.CurrencyValue.Value;
            inventory.UnitPrice = uctxtUnitPrice1.CurrencyValue.Value;
            if (!String.IsNullOrEmpty(cboCurrencyRate.SelectedValue))
                inventory.CurrencyRateId = Convert.ToInt32(cboCurrencyRate.SelectedValue);
            if (uctxtUnitPrice2.CurrencyValue.HasValue)
                inventory.UnitPrice2 = uctxtUnitPrice2.CurrencyValue.Value;
            if (uctxtUnitPrice3.CurrencyValue.HasValue)
                inventory.UnitPrice3 = uctxtUnitPrice3.CurrencyValue.Value;
            if (uctxtUnitPrice4.CurrencyValue.HasValue)
                inventory.UnitPrice4 = uctxtUnitPrice4.CurrencyValue.Value;
            if (uctxtUnitPrice5.CurrencyValue.HasValue)
                inventory.UnitPrice5 = uctxtUnitPrice5.CurrencyValue.Value;

            inventoryManager.Update(original_Inventory, inventory);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "parent.location='Inventories.aspx';", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "parent.location='Inventories.aspx';", true);
        }

        #region functions

        /// <summary>
        /// this method show the inventory
        /// </summary>
        private void showInventory()
        {
            inventoryManager = new InventoryManager(this);
            original_Inventory = inventoryManager.GetInventory(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProductId"]), Convert.ToInt32(Page.ViewState["DepositId"]));

            lblProductName.Text = original_Inventory.Product.Name;
            lblAverageCost.Text = original_Inventory.AverageCosts.ToString();
            lblQuantity.Text = original_Inventory.Quantity.ToString();
            lblQuantityInReserve.Text = original_Inventory.QuantityInReserve.ToString();

            cboDeposit.SelectedValue = original_Inventory.DepositId.ToString();

            if (original_Inventory.SupplierId.HasValue)
                cboSupplier.SelectedValue = original_Inventory.SupplierId.ToString();

            if (original_Inventory.CurrencyRateId.HasValue)
                cboCurrencyRate.SelectedValue = original_Inventory.CurrencyRateId.ToString();

            txtFiscalNumber.Text = original_Inventory.FiscalNumber;
            txtLocalization.Text = original_Inventory.Localization;

            ucCurrFieldMinimunRequired.CurrencyValue = original_Inventory.MinimumRequired;

            uctxtRealCost.CurrencyValue = original_Inventory.RealCost;
            uctxtProfit.CurrencyValue = original_Inventory.Profit;
            uctxtUnitPrice1.CurrencyValue = original_Inventory.UnitPrice;
            uctxtUnitPrice2.CurrencyValue = original_Inventory.UnitPrice2;
            uctxtUnitPrice3.CurrencyValue = original_Inventory.UnitPrice3;
            uctxtUnitPrice4.CurrencyValue = original_Inventory.UnitPrice4;
            uctxtUnitPrice5.CurrencyValue = original_Inventory.UnitPrice5;
        }

        /// <summary>
        /// this method set the UnitPriceName and show the prices
        /// </summary>
        private void SetUnitPriceNames()
        {
            lblUnitPrice1.Text = Company.CompanyConfiguration.UnitPrice1Name;
            lblUnitPrice2.Text = Company.CompanyConfiguration.UnitPrice2Name;
            lblUnitPrice3.Text = Company.CompanyConfiguration.UnitPrice3Name;
            lblUnitPrice4.Text = Company.CompanyConfiguration.UnitPrice4Name;
            lblUnitPrice5.Text = Company.CompanyConfiguration.UnitPrice5Name;

            if (String.IsNullOrEmpty(lblUnitPrice2.Text))
            {
                pnlUnitPrice2.Visible = false;
            }
            else
                lblUnitPrice2.Text += ":";
            if (String.IsNullOrEmpty(lblUnitPrice3.Text))
            {
                pnlUnitPrice3.Visible = false;
            }
            else
                lblUnitPrice3.Text += ":";
            if (String.IsNullOrEmpty(lblUnitPrice4.Text))
            {
                pnlUnitPrice4.Visible = false;
            }
            else
                lblUnitPrice4.Text += ":";
            if (String.IsNullOrEmpty(lblUnitPrice5.Text))
            {
                pnlUnitPrice5.Visible = false;
            }
            else
                lblUnitPrice5.Text += ":";
        }

        #endregion
    }
}
