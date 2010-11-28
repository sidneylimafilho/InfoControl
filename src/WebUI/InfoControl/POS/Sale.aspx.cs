using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;


using InfoControl.Web.Security;
using DiscountType = Vivina.Erp.BusinessRules.SaleManager.DiscountType;



[PermissionRequired("Sale")]
public partial class Company_VPOS_Budget : Vivina.Erp.SystemFramework.PageBase
{
    DataTable basket = new DataTable();


    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["basket"] != null)
            basket = Session["basket"] as DataTable;


        if (!IsPostBack)
        {
            //
            // Reset Sessions
            //
            ucCurrFieldQuantityData.Focus();
            Session["Total"] = null;
            Session["Discount"] = null;
            Session["CustomerId"] = null;
            Session["BudgetId"] = null;
            Session["basket"] = null;
            //
            // Reset fields
            //
            txtDiscount.Text = "0,00";
            txtDiscount.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            txtDiscount.Attributes["onkeyup"] = "CalculateDiscount(this);";
            ucCurrFieldQuantityData.CurrencyValue = null;

            ucTxtUnitPrice.CurrencyValue = null;

            //txtUnitPrice.Text = "";
            //
            // This variable is setted true if the customer is InDebit to 
            // make the Payment button to not be show
            //

            ResetForm();
            BindUnitPriceName();
        }
        else
        {
            //
            //recalculate discount if IsPostBack equals True
            //

            Decimal subTotal = Convert.ToDecimal("0" + lblSubtotal.Text);
            String discount;

            if (!String.IsNullOrEmpty(txtDiscount.Text))
            {
                discount = txtDiscount.Text;

                //verify discount type (percent or cash)
                if (discount.Contains("%"))
                {
                    discount = discount.Replace("%", "");
                    lblTotal.Text = (subTotal - ((subTotal * Convert.ToDecimal("0" + discount)) / 100)).ToString("N");
                }
                else
                {
                    try
                    {
                        lblTotal.Text = (subTotal - Convert.ToDecimal("0" + discount)).ToString("N");

                    }
                    catch (FormatException)
                    {
                        ShowError(Resources.Exception.InvalidDiscountValue);
                    }
                }
            }
        }
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (ucCurrFieldQuantityData.IntValue == 0)
        {
            ShowError("Quantidade não pode ser zero!");
            return;
        }


        btnAdd.Focus();
        if (CheckProductExists())
            AddExistentProduct();
        else
            AddNewProduct();

        BindGrid();


    }

    protected void grdSaleItens_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        basket.Rows.RemoveAt(e.RowIndex);
        BindGrid();
    }

    protected void grdSaleItens_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Verify the row type 

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            InventoryManager inventoryManager = new InventoryManager(this);
            DropDownList cboSerialNumber = (DropDownList)e.Row.Cells[4].Controls[1];
            if (!String.IsNullOrEmpty(grdSaleItens.DataKeys[e.Row.RowIndex]["ProductId"].ToString()))
            {
                if (Deposit != null)
                {
                    cboSerialNumber.DataSource = inventoryManager.GetInventorySerials(Deposit.DepositId, Convert.ToInt32(grdSaleItens.DataKeys[e.Row.RowIndex]["ProductId"]), Company.CompanyId);
                    cboSerialNumber.Items.Add("");
                    cboSerialNumber.DataBind();
                }
            }
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('Deseja realmente excluir o ítem da venda ?') == false) return false;");

        }

    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            CustomerManager customerManager = new CustomerManager(this);
            Session["CustomerId"] = e.Customer.CustomerId;
            lstBudget.DataSourceID = "odsBudgets";
            lstBudget.DataBind();

            lblBudget.Visible = helpToolTipBudget.Visible = lstBudget.Items.Count > 0;
        }

    }

    protected void lstBudget_SelectedIndexChanged(object sender, EventArgs e)
    {
        //
        //Toda vez que um usuário selecionar um orçamento, eu envio o valor para uma variável JavaScript,
        //para poder pegar na tela de pagamento dinamicamente, o processo é feito, para no caso de a venda
        //ser concretizada, o orçamento deverá ser apagado (Executando em sessão, pois não achei o método de 
        //fazer com hidden
        //
        //Page.ViewState["bugdetId"] = bugdetId;


        CreateBasket();
        Session["BudgetId"] = Convert.ToInt32(lstBudget.SelectedValue);
        ProductManager productManager = new ProductManager(this);
        SaleManager saleManager = new SaleManager(this);
        Budget budget = saleManager.GetBudget((Int32)Session["BudgetId"], Company.CompanyId);

        if (budget.Discount.HasValue)
        {
            if (budget.DiscountType == (int)DiscountType.Cash)
                txtDiscount.Text = budget.Discount.Value.ToString("##,##0.00");
            else
            {
#warning eliminar variavel temporaria/ criar método, ou executar função novamente
                Decimal? discount = budget.BudgetItems.Sum(bi => bi.Quantity * bi.UnitPrice) * (budget.Discount.Value / 100);
                if (discount.HasValue)
                    txtDiscount.Text = discount.Value.ToString("##,##0.00");
            }


        }
        if (budget.AdditionalCost.HasValue)
            txtDiscount.Text = (Convert.ToDecimal(txtDiscount.Text) - Convert.ToDecimal(budget.AdditionalCost.Value)).ToString("##,##0.00");
        foreach (DataRow row in productManager.GetProductsByBudget((Int32)Session["BudgetId"]).Rows)
        {
            string productName;
            if (!String.IsNullOrEmpty(row["Name"].ToString()))
                productName = row["Name"].ToString();
            else
                productName = row["SpecialProductName"].ToString();
            //string productName = row["Name"].ToString() + row["SpecialProductName"].ToString();

            if (String.IsNullOrEmpty(row["ProductId"].ToString()))
                productName += "&nbsp;&nbsp;&nbsp;&nbsp;<img src='" + ResolveClientUrl("~/App_Shared/themes/glasscyan/Company/Product_warning.gif") + "' alt='Este produto não se encontra cadastrado!' />";

            basket.Rows.Add(
                row["ProductId"],
                row["Quantity"],
                    productName,
                /*SerialNumber*/ "",
                row["ProductCode"],
                row["unitPrice"],
                Convert.ToDouble(row["Quantity"]) * Convert.ToDouble(row["unitPrice"]),
                row["UnitCost"] != DBNull.Value ? Convert.ToDouble(row["UnitCost"]) : 0
                );
        }

        BindGrid();
    }

    protected void odsBudgets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = Convert.ToInt32(Session["CustomerId"]);
        e.InputParameters[1] = Company.CompanyId;
    }

    protected void odsSalesPerson_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void cboProfit_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void odsProfitMargins_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnDeleteBudget_Command(object sender, CommandEventArgs e)
    {
        var bManager = new SaleManager(this);
        bManager.DeleteBudgetAndBudGetItems(Convert.ToInt16(e.CommandArgument), Company.CompanyId);

        //
        // Load the budgets 
        //
        lstBudget.DataSourceID = "odsBudgets";
        lstBudget.DataBind();
    }

    protected void lnkCustomer_Click(object sender, EventArgs e)
    {
    }

    protected void odsInventorySerial_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["depositId"] = Deposit.DepositId;
    }

    protected void cboSerialNumber_TextChanged(object sender, EventArgs e)
    {
        SetQuantityAndSerialNumbers();
    }

    #endregion

    #region Utility Functions
    private DataTable CreateBasket()
    {
        basket = new DataTable();
        basket.Columns.Add("ProductId", typeof(int));
        basket.Columns.Add("Quantity", typeof(int));
        basket.Columns.Add("ProductName", typeof(string));
        basket.Columns.Add("SerialNumber", typeof(string));
        basket.Columns.Add("ProductCode", typeof(string));
        basket.Columns.Add("UnitPrice", typeof(double));
        basket.Columns.Add("ProfitPrice", typeof(double));
        basket.Columns.Add("UnitCost", typeof(double));
        Session["basket"] = basket;
        return basket;
    }
    private bool CheckIfCanSale(int productId, int quantity)
    {
        Deposit dep = new Deposit();
        User user = new User();
        CompanyManager cManager = new CompanyManager(this);
        InventoryManager iManager = new InventoryManager(this);

        //
        // Verifies wich deposit are the active deposit to this Company / User
        //
        user.UserId = (Page as InfoControl.Web.UI.DataPage).User.Identity.UserId;
        dep = cManager.GetCurrentDeposit(user.UserId, Company.CompanyId);

        //
        // Get the data of the respectively inventory
        //
        var prod = iManager.GetProductInventory(Company.CompanyId, productId, Deposit.DepositId);

        //
        // Compare the Inventory Quantity wich the Sale quantity, if the Sale Quantity are 
        // less than the Inventory, the Can Sale flag will be true, else the sale will be canceled
        //
        if (quantity <= prod.Quantity) return true;
        else return false;


    }

    private void VerifyIfCanSale()
    {
        btnPayment.Visible = lblTotal.Text != "0,00";
    }
    private void CalculateTotals()
    {
        double total = 0;
        foreach (DataRow product in basket.Rows)
        {
            total += Convert.ToDouble(product["ProfitPrice"]);
        }
        lblSubtotal.Text = total.ToString("##,##0.00");
        if (!txtDiscount.Text.Contains("%"))
        {
            if (!string.IsNullOrEmpty(txtDiscount.Text))
                total -= Convert.ToDouble(txtDiscount.Text);
        }
        else
        {
            txtDiscount.Text = txtDiscount.Text.Replace("%", "");
            total = total - (total * Convert.ToDouble(txtDiscount.Text) / 100);
            txtDiscount.Text += "%";
        }

        //lblSubtotal.Text = total.ToString("##,##0.00");
        lblTotal.Text = total.ToString("##,##0.00");
        Session["Total"] = lblTotal.Text;

        Session["Discount"] = Convert.ToDecimal(Convert.ToDecimal(lblTotal.Text) - Convert.ToDecimal(lblSubtotal.Text));

        btnPayment.OnClientClick = "ApplyPaymentMethod(); return false;";
        VerifyIfCanSale();
    }

    /// <summary>
    /// this function format the textbox(change , to .)
    /// </summary>
    /// <param name="Double"></param>
    /// <returns></returns>
    private string FormatDouble(TextBox Double)
    {

        return Double.Text.RemoveMask();
    }

    /// <summary>
    /// this method bind the grid
    /// </summary>
    private void BindGrid()
    {
        grdSaleItens.DataSource = basket;
        grdSaleItens.DataBind();

        CalculateTotals();

        selProduct.Name = "";
        ucCurrFieldQuantityData.CurrencyValue = null;
        //txtUnitPrice.Text = "";
        ucTxtUnitPrice.CurrencyValue = null;
    }
    private void SetQuantityAndSerialNumbers()
    {
        DropDownList cboSerialNumber;
        foreach (GridViewRow row in grdSaleItens.Rows)
        {
            cboSerialNumber = (DropDownList)row.Cells[4].Controls[1];
            basket.Rows[row.RowIndex]["SerialNumber"] = "" + cboSerialNumber.SelectedValue;
        }
    }
    private string RequestControl(Control control)
    {
        return Request[control.ClientID.Replace("_", "$")];
    }
    private void ResetForm()
    {
        CreateBasket();
        BindGrid();
        ViewState["CustomerId"] = null;
        lstBudget.DataSourceID = "";
        btnPayment.Visible = false;
    }

    /// <summary>
    /// this method add a nonexistent product in dtProducts
    /// </summary>
    private void AddNewProduct()
    {

        if (Deposit == null)
        {
            ShowError("O usuário não está habilitado para gerar vendas!");
            return;
        }

        if (!ucTxtUnitPrice.CurrencyValue.HasValue)
            ucTxtUnitPrice.CurrencyValue = 0;

        addProductRow(0, ucCurrFieldQuantityData.IntValue, selProduct.Name, "0", ucTxtUnitPrice.CurrencyValue.Value, 0);


    }

    /// <summary>
    /// this method add a existent product in DataTable Products
    /// </summary>
    private void AddExistentProduct()
    {

        if (Deposit == null)
        {
            ShowError("O usuário não está habilitado para gerar vendas!");
            return;
        }

        var iManager = new InventoryManager(this);
        var pManager = new ProductManager(this);
        Inventory productInInventory;

        var product = pManager.GetProductByName(Company.CompanyId, selProduct.Product.Name);

        decimal unitCost = 0;
        decimal unitPrice = 0;

        if (ucTxtUnitPrice.CurrencyValue.HasValue)
            unitPrice = ucTxtUnitPrice.CurrencyValue.Value;

        productInInventory = iManager.GetProductInventory(Company.CompanyId, product.ProductId, Deposit.DepositId);

        if (productInInventory != null)
        {
            if (unitPrice == 0)
                unitPrice = productInInventory.UnitPrice;
            unitCost = productInInventory.RealCost;
        }

        Page.ViewState["InventoryProductId"] = product.ProductId;

        if (!ucTxtUnitPrice.CurrencyValue.HasValue)
            ucTxtUnitPrice.CurrencyValue = 0;

        addProductRow(product.ProductId, ucCurrFieldQuantityData.IntValue, product.Name, product.ProductCode, unitPrice, unitCost);
    }

    /// <summary>
    /// this method add one row of product in datatable of products
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="quantity"></param>
    /// <param name="productName"></param>
    /// <param name="productCode"></param>
    /// <param name="productPrice"></param>
    private void addProductRow(Int32 productId, Int32 quantity, string productName, String productCode, decimal productPrice, decimal unitCost)
    {
        DataRow productRow = basket.NewRow();

        productRow["ProductId"] = productId;
        productRow["Quantity"] = quantity;
        productRow["ProductName"] = productName;
        productRow["SerialNumber"] = "";
        productRow["ProductCode"] = productCode;
        productRow["UnitPrice"] = productPrice;
        productRow["ProfitPrice"] = productPrice * quantity;
        productRow["UnitCost"] = unitCost;
        basket.Rows.Add(productRow);

    }

    /// <summary>
    /// this method verify if product exist in inventory
    /// </summary>
    private bool CheckProductInventoryExists(Int32 productId)
    {
        InventoryManager inventoryManager = new InventoryManager(this);
        if (inventoryManager.GetProductInventory(Company.CompanyId, productId, Deposit.DepositId) != null)
            return true;
        else
            return false;

    }

    /// <summary>
    /// this method return true if product exists or return false if its not exists
    /// </summary>
    /// <returns></returns>
    private Boolean CheckProductExists()
    {
        ProductManager pManager = new ProductManager(this);

        if (selProduct.Product != null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// this method bind all unitPriceNames from company
    /// </summary>
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
        cboUnitPriceName.Items.Add("");
        foreach (String item in lstUnitPriceNames)
        {
            uniPriceNameCount += 1;
            lstItem = new ListItem();
            lstItem.Text = item;
            lstItem.Value = uniPriceNameCount.ToString();
            cboUnitPriceName.Items.Add(lstItem);
        }

        if (cboUnitPriceName.Items.Count < 3)
            saleTypes.Visible = false;
    }

    /// <summary>
    /// this method update the productPrice from products in datatable products
    /// </summary>
    private void ChangeProductsUnitPrice()
    {
        Decimal? unitPrice;
        foreach (GridViewRow row in grdSaleItens.Rows)
        {
            if (basket.Rows[row.RowIndex]["ProductId"] != DBNull.Value && Convert.ToInt32(basket.Rows[row.RowIndex]["ProductId"]) != 0)
            {
                if (CheckProductInventoryExists(Convert.ToInt32(basket.Rows[row.RowIndex]["ProductId"])))
                {
                    unitPrice = GetProductUnitPrice(Convert.ToInt32(basket.Rows[row.RowIndex]["ProductId"]), Convert.ToInt32(cboUnitPriceName.SelectedValue));
                    if (unitPrice.HasValue)
                        basket.Rows[row.RowIndex]["UnitPrice"] = unitPrice;
                    else
                        basket.Rows[row.RowIndex]["UnitPrice"] = 0;

                    basket.Rows[row.RowIndex]["ProfitPrice"] = Convert.ToInt32(basket.Rows[row.RowIndex]["Quantity"]) * Convert.ToDecimal(basket.Rows[row.RowIndex]["UnitPrice"]);
                }

            }
        }
    }

    private decimal? GetProductUnitPrice(Int32 productId, Int32 unitPriceCode)
    {
        InventoryManager inventoryManager = new InventoryManager(this);
        Inventory inventory = inventoryManager.GetProductInventory(Company.CompanyId, productId, Deposit.DepositId);
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

    /// <summary>
    /// this method bind the serialNumbers
    /// </summary>
    private void BindSerialNumber(Int32 rowIndex)
    {
        //InventoryManager inventoryManager = new InventoryManager(this);
        //cboSerialNumber.DataSource = inventoryManager.GetInventory(Company.CompanyId, , Deposit.DepositId).InventorySerials;
        //cboSerialNumber.DataBind();
    }

    #endregion


    protected void cboUnitPriceName_TextChanged(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(cboUnitPriceName.SelectedValue))
        {
            ChangeProductsUnitPrice();
            BindGrid();
        }
    }
    protected void grdSaleItens_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {

    }

}
