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
using System.Xml.Linq;
using System.Collections.Generic;


using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web;
namespace Vivina.Erp.WebUI.POS
{
    [Serializable]
    public class SaleItem
    {
        #region private properties
        private Int32 _productId;
        private Int32 _quantity;
        private String _name;
        private String _serialNumber;
        private String _code;
        private Decimal _price;
        private Decimal _unitCost;

        #endregion

        #region properties

        public int? ProductId
        {
            get { return _productId; }
            set
            {

                _productId = Convert.ToInt32(value);
            }
        }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public String Name { get { return _name; } set { _name = value; } }
        public String SerialNumber { get { return _serialNumber; } set { _serialNumber = value; } }
        public String Code { get { return _code; } set { _code = value; } }
        public Decimal? Price
        {
            get { return _price; }
            set
            {
                _price = Convert.ToDecimal(value);
            }
        }
        public Decimal ProfitPrice { get { return _quantity * _price; } }
        public Decimal? UnitCost { get { return _unitCost; } set { _unitCost = Convert.ToDecimal(value); } }
        #endregion


    }
    [Serializable]
    public class Discount
    {
        private Decimal _discountValue;
        private Boolean isCashDiscount;

        public Decimal DiscountValue { get { return _discountValue; } set { _discountValue = value; } }
        public Boolean IsCashDiscount { get { return isCashDiscount; } set { isCashDiscount = value; } }

        public Discount()
        {
            DiscountValue = Decimal.Zero;
            IsCashDiscount = true;
        }

    }

    public partial class Sale2 : Vivina.Erp.SystemFramework.PageBase
    {

        public List<SaleItem> SaleItemList
        {
            get
            {
                if (Session["SaleItemList"] == null)
                    Session["SaleItemList"] = new List<SaleItem>();

                return (Session["SaleItemList"] as List<SaleItem>);
            }
            set { Session["SaleItemList"] = value; }


        }
        public Discount Discount
        {
            get
            {

                if (Page.ViewState["Discount"] == null)
                    Page.ViewState["Discount"] = new Discount();
                return (Page.ViewState["Discount"] as Discount);

            }
            set
            {
                Page.ViewState["Discount"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            throw new InvalidOperationException("");
            if (!IsPostBack)
            {
                SaleItemList = null;
                BindUnitPriceName();
                grdSaleItens.DataBind();
                btnPayment.Visible = false;
            }

        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (CheckProductExists())
                AddExistentSaleItem();
            else
                AddNewSaleItem();

            BindGrdSaleItem();
        }

        protected void btnDeleteBudget_Command(object sender, CommandEventArgs e)
        {

            new SaleManager(this).DeleteBudgetAndBudGetItems(Convert.ToInt16(e.CommandArgument), Company.CompanyId);
            lstBudget.DataBind();
        }

        protected void lstBudget_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaleItemList = null;
            txtDiscount.Text = String.Empty;
            Discount = null;

            SaleManager budgetManager = new SaleManager(this);
            Budget budget = budgetManager.GetBudget(Convert.ToInt32(lstBudget.SelectedValue), Company.CompanyId);
            Page.ViewState["budgetId"] = Convert.ToInt32(lstBudget.SelectedValue);

            Discount.DiscountValue = budgetManager.CalculateBudgetDiscount(budget);
            Discount.IsCashDiscount = true;

            if (budget.AdditionalCost.HasValue)
                Discount.DiscountValue -= budget.AdditionalCost.Value;


            SaleItem saleItem;

            foreach (BudgetItem budgetItem in budgetManager.GetBudgetItemByBudget(budget.BudgetId, Company.CompanyId))
            {
                saleItem = new SaleItem();
                if (budgetItem.ProductId.HasValue)
                    saleItem.Name = budgetItem.Product.Name;
                else
                    saleItem.Name = budgetItem.SpecialProductName + "&nbsp;&nbsp;&nbsp;&nbsp;<img src='" + ResolveClientUrl("~/App_Themes/_global/Company/Product_warning.gif") + "' alt='Este produto não se encontra cadastrado!' />";

                saleItem.ProductId = budgetItem.ProductId;
                saleItem.Quantity = budgetItem.Quantity;
                saleItem.Code = budgetItem.ProductCode;
                saleItem.Price = budgetItem.UnitPrice;
                saleItem.UnitCost = budgetItem.UnitCost;

                SaleItemList.Add(saleItem);
            }
            BindGrdSaleItem();



        }

        protected void grdSaleItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && Convert.ToInt32(grdSaleItens.DataKeys[e.Row.RowIndex]["ProductId"]) != 0)
            {
                DropDownList cboSerialNumber = (DropDownList)e.Row.Cells[3].Controls[1];
                cboSerialNumber.DataSource = new InventoryManager(this).GetInventorySerials(Deposit.DepositId, Convert.ToInt32(grdSaleItens.DataKeys[e.Row.RowIndex]["ProductId"]), Company.CompanyId);
                cboSerialNumber.Items.Add(String.Empty);
                cboSerialNumber.DataBind();
            }


        }

        protected void odsBudgets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (Page.ViewState["CustomerId"] != null)
            {
                e.InputParameters["CustomerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);
                e.InputParameters["CompanyId"] = Company.CompanyId;
            }
        }

        protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
        {
            if (e.Customer != null)
            {
                Page.ViewState["CustomerId"] = e.Customer.CustomerId;
                lstBudget.DataBind();
            }

        }

        protected void cboUnitPriceName_TextChanged(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(cboUnitPriceName.SelectedValue))
            {
                ChangeProductsUnitPrice();
                BindGrdSaleItem();
            }
        }

        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        protected void cboSerialNumber_TextChanged(object sender, EventArgs e)
        {
        }

        protected void grdSaleItens_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SaleItemList.RemoveAt(e.RowIndex);
            BindGrdSaleItem();
        }

        protected void btnPayment_Click(object sender, ImageClickEventArgs e)
        {
            Session["Total"] = Convert.ToDecimal(lblTotal.Text);
            Session["SubTotal"] = Convert.ToDecimal(lblSubtotal.Text);
            Session["Discount"] = Convert.ToDecimal(lblSubtotal.Text);
            Session["CustomerId"] = Page.ViewState["CustomerId"];
            Session["BudgetId"] = Page.ViewState["budgetId"];
            Page.ClientScript.RegisterStartupScript(this.GetType(), "",
                   "top.$.lightbox('POS/Sale_Parcels2.aspx?lightbox[iframe]=true') ", true);


        }

        #region Functions

        protected Boolean CheckProductExists()
        {
            return new ProductManager(this).GetProductByName(Convert.ToInt32(Company.MatrixId), selProduct.Name) != null;

        }

        protected bool CheckProductInventoryExists(Int32 productId)
        {
            return new InventoryManager(this).GetProductInventory(Company.CompanyId, productId, Deposit.DepositId) != null;
        }

        protected void AddExistentSaleItem()
        {
            SaleItem saleItem = new SaleItem();
            Product product = new ProductManager(this).GetProductByName((int)Company.MatrixId, selProduct.Name);
            Inventory productInInventory = new InventoryManager(this).GetProductInventory(
                 Company.CompanyId, product.ProductId, Deposit.DepositId);

            saleItem.ProductId = product.ProductId;
            saleItem.Name = selProduct.Name;
            //se o preco na tela for != de null, então ele é convertido e arqmazenado
            //senão verificamos se o preco do produto já existe em inventário, se este for o caso, armazenamos o preco
            //senão, setamos 0 para o price 
            saleItem.Price = ucCurrFieldUnitPrice.CurrencyValue.HasValue ? ucCurrFieldUnitPrice.CurrencyValue.Value : (productInInventory != null ? Convert.ToDecimal(productInInventory.UnitPrice) : Decimal.Zero);
            saleItem.Quantity = ucCurrFieldQuantityData.IntValue;
            if (productInInventory != null)
                saleItem.UnitCost = productInInventory.RealCost;

            SaleItemList.Add(saleItem);
        }

        protected void AddNewSaleItem()
        {
            SaleItem saleItem = new SaleItem();
            saleItem.ProductId = null;
            saleItem.Quantity = ucCurrFieldQuantityData.IntValue;
            saleItem.Name = selProduct.Name;
            saleItem.Code = String.Empty;
            saleItem.Price = ucCurrFieldUnitPrice.CurrencyValue;
            saleItem.UnitCost = Decimal.Zero;
            SaleItemList.Add(saleItem);
            // AddSaleItem(saleItem);

        }

        /// <summary>
        /// This method add a new Item in the SaleItemList
        /// </summary>
        /// <param name="saleItem"></param>
        private void AddSaleItem(SaleItem saleItem)
        {
            SaleItem sItem = SaleItemList.Where(i => i.ProductId == saleItem.ProductId || i.Name == saleItem.Name).FirstOrDefault();
            if (sItem != null)
            {
                sItem.Quantity += saleItem.Quantity;
                sItem.UnitCost += saleItem.UnitCost;
            }
            else
                SaleItemList.Add(saleItem);

        }

        protected void BindGrdSaleItem()
        {
            grdSaleItens.DataSource = SaleItemList;
            lblSubtotal.Text = SaleItemList.Sum(item => item.ProfitPrice).ToString("##,##0.00");
            CalculateTotal();
            grdSaleItens.DataBind();
            btnPayment.Visible = SaleItemList.Any();
        }

        private void BindUnitPriceName()
        {
            cboUnitPriceName.Items.Clear();
            Int32 uniPriceNameCount = 0;
            ListItem lstItem;

            List<String> lstUnitPriceNames = new List<string>();
            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice1Name))
                lstUnitPriceNames.Add(Company.CompanyConfiguration.UnitPrice1Name);
            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice2Name))
                lstUnitPriceNames.Add(Company.CompanyConfiguration.UnitPrice2Name);
            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice3Name))
                lstUnitPriceNames.Add(Company.CompanyConfiguration.UnitPrice3Name);
            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice4Name))
                lstUnitPriceNames.Add(Company.CompanyConfiguration.UnitPrice4Name);
            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice5Name))
                lstUnitPriceNames.Add(Company.CompanyConfiguration.UnitPrice5Name);

            // add a white item
            cboUnitPriceName.Items.Add(String.Empty);

            foreach (String item in lstUnitPriceNames)
            {
                uniPriceNameCount += 1;
                lstItem = new ListItem();
                lstItem.Text = item;
                lstItem.Value = uniPriceNameCount.ToString();
                cboUnitPriceName.Items.Add(lstItem);
            }
            if (cboUnitPriceName.Items.Count > 1)
                cboUnitPriceName.Visible = true;

        }

        private void ChangeProductsUnitPrice()
        {
            foreach (GridViewRow row in grdSaleItens.Rows)
            {
                if (SaleItemList[row.RowIndex].ProductId != 0 && CheckProductInventoryExists(Convert.ToInt32(SaleItemList[row.RowIndex].ProductId)))
                    SaleItemList[row.RowIndex].Price = GetProductUnitPrice(Convert.ToInt32(SaleItemList[row.RowIndex].ProductId), Convert.ToInt32(cboUnitPriceName.SelectedValue)) ?? 0;
            }
        }

        private decimal? GetProductUnitPrice(Int32 productId, Int32 unitPriceCode)
        {
            Inventory inventory = new InventoryManager(this).GetProductInventory(Company.CompanyId, productId, Deposit.DepositId);
            switch (unitPriceCode)
            {
                case 1:
                    return inventory.UnitPrice;
                    break;
                case 2:
                    return inventory.UnitPrice2;
                    break;
                case 3:
                    return inventory.UnitPrice3;
                    break;
                case 4:
                    return inventory.UnitPrice4;
                    break;
                case 5:
                    return inventory.UnitPrice5;
                    break;

                default: throw new InvalidOperationException();
            }
        }

        protected void CalculateTotal()
        {
            Decimal discount = Decimal.Zero;
            Discount.IsCashDiscount = true;

            if (!Decimal.TryParse(txtDiscount.Text, out discount) && txtDiscount.Text.Contains("%"))
            {
                Discount.IsCashDiscount = false;
                txtDiscount.Text = txtDiscount.Text.Replace("%", "");
                Decimal.TryParse(txtDiscount.Text, out discount);
            }

            Discount.DiscountValue = discount;

            if (!Discount.IsCashDiscount)
            {
                lblTotal.Text = (SaleItemList.Sum(i => i.ProfitPrice) - (SaleItemList.Sum(i => i.ProfitPrice) * Discount.DiscountValue / 100)).ToString("##,##0.00"); ;
                txtDiscount.Text += "%";

            }
            else
                lblTotal.Text = (SaleItemList.Sum(i => i.ProfitPrice) - Discount.DiscountValue).ToString("##,##0.00"); ;

        }
        #endregion


    }


}
