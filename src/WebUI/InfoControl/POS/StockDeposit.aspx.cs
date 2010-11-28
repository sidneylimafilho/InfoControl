using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using Exception=Resources.Exception;

[PermissionRequired("StockDeposit")]
public partial class Company_Stock_Deposit : Vivina.Erp.SystemFramework.PageBase
{
    private CompanyConfiguration companyConfiguration;
    private DataControlField deleteColumn;
    public bool isInserting;
    private bool isLoad;
    private double total;

    public DataTable ProductList
    {
        get
        {
            if (Page.ViewState["_productList"] == null)
                Page.ViewState["_productList"] = CreateProductList();
            return (DataTable) Page.ViewState["_productList"];
        }
        set { Page.ViewState["_productList"] = value; }
    }

    /// <summary>
    /// this method clear the fields
    /// </summary>
    private void CleanFields()
    {
        selProduct.Name = "";
        ucCurrFieldQuantity.CurrencyValue = null;
        txtLocalization.Text = "";

        uctxtRealCost.CurrencyValue = null;
        uctxtProfit.CurrencyValue = null;
        uctxtProfit.CurrencyValue = null;
        uctxtUnitPrice1.CurrencyValue = null;
        uctxtUnitPrice2.CurrencyValue = null;
        uctxtUnitPrice3.CurrencyValue = null;
        uctxtUnitPrice4.CurrencyValue = null;
        uctxtUnitPrice5.CurrencyValue = null;
    }

    #region Functions

    /// <summary>
    /// this method clear the screen
    /// </summary>
    private void CleanScreen()
    {
        cboDeposit.SelectedIndex = 0;
        cboSupplier.SelectedIndex = 0;
        DropDownList1.SelectedIndex = 0;
        txtFiscalNumber.Text = "";

        CleanFields();
        ucEntryDate.DateTime = DateTime.Now.Date;
        ProductList.Clear();
        BindGrid();
    }

    /// <summary>
    /// this method create the dataTable 
    /// </summary>
    /// <returns></returns>
    private DataTable CreateProductList()
    {
        var productList = new DataTable();
        productList.Columns.Add("ProductId", typeof (int));
        productList.Columns.Add("ProductName", typeof (string));
        productList.Columns.Add("Localization", typeof (string));
        productList.Columns.Add("UnitPrice", typeof (string));
        productList.Columns.Add("UnitPrice2", typeof (string));
        productList.Columns.Add("UnitPrice3", typeof (string));
        productList.Columns.Add("UnitPrice4", typeof (string));
        productList.Columns.Add("UnitPrice5", typeof (string));
        productList.Columns.Add("RealCost", typeof (double));
        productList.Columns.Add("Profit", typeof (double));
        productList.Columns.Add("Quantity", typeof (int));

        // move the column delete for the final

        return productList;
    }

    /// <summary>
    /// this method add one product in dataTable
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="product"></param>
    private void AddProduct(Inventory inventory, string product)
    {
        ProductList.Rows.Add(inventory.ProductId, product, inventory.Localization, inventory.UnitPrice, inventory.UnitPrice2 == 0
                                                                                                            ? ""
                                                                                                            : inventory.UnitPrice2.ToString(), inventory.UnitPrice3 == 0
                                                                                                                                                   ? ""
                                                                                                                                                   : inventory.UnitPrice3.ToString(), inventory.UnitPrice4 == 0
                                                                                                                                                                                          ? ""
                                                                                                                                                                                          : inventory.UnitPrice4.ToString(), inventory.UnitPrice5 == 0
                                                                                                                                                                                                                                 ? ""
                                                                                                                                                                                                                                 : inventory.UnitPrice5.ToString(),
                             inventory.RealCost, inventory.Profit, inventory.Quantity);
    }

    /// <summary>
    /// this method binds the grid
    /// </summary>
    private void BindGrid()
    {
        grdInventory.DataSource = ProductList;

        grdInventory.DataBind();
    }

    /// <summary>
    /// this method format a doublevaluein string format and return a decimal
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private decimal FormatDouble(string text)
    {
        text = text.Replace("_", "0");
        text = text.Replace(".", "");
        return Convert.ToDecimal(text);
    }

    /// <summary>
    /// this method format the priceValue
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private Decimal FormatPriceValue(string value)
    {
        if (!string.IsNullOrEmpty(value))
            return FormatDouble(value);
        else
            return 0;
    }

    /// <summary>
    /// this method set the UnitPriceName and show the prices
    /// </summary>
    private void SetUnitPriceNames()
    {
        lblUnitPrice1.Text = Company.CompanyConfiguration.UnitPrice1Name;
        if (String.IsNullOrEmpty(lblUnitPrice1.Text))
            lblUnitPrice1.Text = "Preço de venda";
        lblUnitPrice2.Text = Company.CompanyConfiguration.UnitPrice2Name;
        lblUnitPrice3.Text = Company.CompanyConfiguration.UnitPrice3Name;
        lblUnitPrice4.Text = Company.CompanyConfiguration.UnitPrice4Name;
        lblUnitPrice5.Text = Company.CompanyConfiguration.UnitPrice5Name;

        #region old code

        //grdInventory.Columns[5].HeaderText = Company.CompanyConfiguration.UnitPrice1Name;
        //if (String.IsNullOrEmpty(grdInventory.Columns[5].HeaderText))
        //    grdInventory.Columns[5].HeaderText = lblUnitPrice1.Text;

        //grdInventory.Columns[6].HeaderText = Company.CompanyConfiguration.UnitPrice2Name;
        //grdInventory.Columns[7].HeaderText = Company.CompanyConfiguration.UnitPrice3Name;
        //grdInventory.Columns[8].HeaderText = Company.CompanyConfiguration.UnitPrice4Name;
        //grdInventory.Columns[9].HeaderText = Company.CompanyConfiguration.UnitPrice5Name;
        //grdInventory.DataBind();

        //if (String.IsNullOrEmpty(lblUnitPrice1.Text))
        //{

        //    pnlUnitPrice1.Visible = false;
        //    foreach (GridViewRow item in grdInventory.Rows)
        //    {
        //        item.Cells[4].Visible = false;
        //    }
        //}
        //else
        //    lblUnitPrice1.Text += ":";
        //if (String.IsNullOrEmpty(lblUnitPrice2.Text))
        //{

        //    pnlUnitPrice2.Visible = false;
        //    foreach (GridViewRow item in grdInventory.Rows)
        //    {
        //        item.Cells[5].Visible = false;
        //    }
        //}
        //else
        //    lblUnitPrice2.Text += ":";
        //if (String.IsNullOrEmpty(lblUnitPrice3.Text))
        //{

        //    pnlUnitPrice3.Visible = false;
        //    foreach (GridViewRow item in grdInventory.Rows)
        //    {
        //        item.Cells[6].Visible = false;
        //    }
        //}
        //else
        //    lblUnitPrice3.Text += ":";
        //if (String.IsNullOrEmpty(lblUnitPrice4.Text))
        //{

        //    pnlUnitPrice4.Visible = false;
        //    foreach (GridViewRow item in grdInventory.Rows)
        //    {
        //        item.Cells[7].Visible = false;
        //    }
        //}
        //else
        //    lblUnitPrice4.Text += ":";
        //if (String.IsNullOrEmpty(lblUnitPrice5.Text))
        //{

        //    pnlUnitPrice5.Visible = false;
        //    foreach (GridViewRow item in grdInventory.Rows)
        //    {
        //        item.Cells[8].Visible = false;
        //    }
        //}
        //else
        //    lblUnitPrice5.Text += ":";

        #endregion
    }

    private void DeleteProductItem(Int32 rowIndex)
    {
        if (ProductList.Rows.Count <= rowIndex)
        {
            ShowError(Exception.nonexistentProduct);
            return;
        }
        ProductList.Rows.RemoveAt(rowIndex);
        BindGrid();
    }

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        tipDepositEmpty.Visible = (cboDeposit.Items.Count == 0);

        tipSupplierEmpty.Visible = (cboSupplier.Items.Count == 0);
        SetUnitPriceNames();

        if (!IsPostBack)
        {
            if (lblUnitPrice1.Text != null)
                lblPriceDefault.Visible = false;

            ucEntryDate.DateTime = DateTime.Now;

            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice1Name))
            {
                grdInventory.Columns.Add(new BoundField
                                         {
                                             HeaderText = Company.CompanyConfiguration.UnitPrice1Name,
                                             DataField = "UnitPrice"
                                         });
                pnlUnitPrice1.Visible = true;
            }

            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice2Name))
            {
                grdInventory.Columns.Add(new BoundField
                                         {
                                             HeaderText = Company.CompanyConfiguration.UnitPrice2Name,
                                             DataField = "UnitPrice2"
                                         });
                pnlUnitPrice2.Visible = true;
            }
            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice3Name))
            {
                grdInventory.Columns.Add(new BoundField
                                         {
                                             HeaderText = Company.CompanyConfiguration.UnitPrice3Name,
                                             DataField = "UnitPrice3"
                                         });
                pnlUnitPrice3.Visible = true;
            }

            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice4Name))
            {
                grdInventory.Columns.Add(new BoundField
                                         {
                                             HeaderText = Company.CompanyConfiguration.UnitPrice4Name,
                                             DataField = "UnitPrice4"
                                         });
                pnlUnitPrice4.Visible = true;
            }

            if (!String.IsNullOrEmpty(Company.CompanyConfiguration.UnitPrice5Name))
            {
                grdInventory.Columns.Add(new BoundField
                                         {
                                             HeaderText = Company.CompanyConfiguration.UnitPrice5Name,
                                             DataField = "UnitPrice5"
                                         });
                pnlUnitPrice5.Visible = true;
            }

            grdInventory.Columns.Add(new CommandField
                                     {
                                         ShowDeleteButton = true,
                                         DeleteText = "<div class='delete' title='excluir'> </div> ",
                                         DeleteImageUrl = "../../App_Shared/themes/glasscyan/Controls/GridView/img/Delete.png"
                                     });
        }
    }

    protected void odsSupplier_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["matrixId"] = (int) Company.MatrixId;
    }

    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void grdInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DeleteProductItem(e.RowIndex);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Inventory inv;
        var inventoryManager = new InventoryManager(this);
        // Product prod = new Product();
        //  ProductManager pManager = new ProductManager(this);

        if (ProductList.Rows.Count == 0)
        {
            ShowError(Exception.AddProductInList);
            BindGrid();
            return;
        }

        foreach (DataRow row in ProductList.Rows)
        {
            inv = new Inventory();
            inv.EntryDate = ucEntryDate.DateTime;
            inv.FiscalNumber = txtFiscalNumber.Text;
            if (DropDownList1.SelectedIndex > 0)
                inv.CurrencyRateId = Convert.ToInt32("0" + DropDownList1.SelectedValue);
            inv.DepositId = Convert.ToInt32(cboDeposit.SelectedValue);
            inv.CompanyId = Company.CompanyId;
            inv.ProductId = Convert.ToInt32(row["ProductId"]);
            inv.Quantity = Convert.ToInt32(row["Quantity"]);
            inv.ModifiedDate = DateTime.Now;
            inv.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
            inv.Localization = Convert.ToString(row["Localization"]);

            if (!String.IsNullOrEmpty(row["UnitPrice2"].ToString()))
                inv.UnitPrice2 = Convert.ToDecimal(row["UnitPrice2"]);
            if (!String.IsNullOrEmpty(row["UnitPrice3"].ToString()))
                inv.UnitPrice3 = Convert.ToDecimal(row["UnitPrice3"]);
            if (!String.IsNullOrEmpty(row["UnitPrice4"].ToString()))
                inv.UnitPrice4 = Convert.ToDecimal(row["UnitPrice4"]);
            if (!String.IsNullOrEmpty(row["UnitPrice5"].ToString()))
                inv.UnitPrice5 = Convert.ToDecimal(row["UnitPrice5"]);
            inv.RealCost = Convert.ToDecimal(row["RealCost"]);
            inv.Profit = Convert.ToDecimal(row["Profit"]);
            inv.MinimumRequired = default(int);
            inv.QuantityInReserve = default(int);
            inv.SupplierId = Convert.ToInt32(cboSupplier.SelectedValue);

            inventoryManager.StockDeposit(inv, Convert.ToInt32(cboSupplier.SelectedValue), User.Identity.UserId);
        }
        CleanScreen();
        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Entrada no Estoque feita com sucesso!');", true);
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        btnAdd.Focus();

        var inv = new Inventory();
        var pManager = new ProductManager(this);

        if (selProduct.Product != null)
        {
            inv.ProductId = selProduct.Product.ProductId;
            if (ucCurrFieldQuantity.IntValue == 0)
            {
                ShowError(Exception.InvalidProductQuantity);
                return;
            }

            inv.Quantity = ucCurrFieldQuantity.IntValue;
            inv.Localization = txtLocalization.Text;
            if (uctxtUnitPrice1.CurrencyValue.HasValue)
                inv.UnitPrice = uctxtUnitPrice1.CurrencyValue.Value;

            if (uctxtUnitPrice2.CurrencyValue.HasValue)
                inv.UnitPrice2 = uctxtUnitPrice2.CurrencyValue.Value;

            if (uctxtUnitPrice3.CurrencyValue.HasValue)
                inv.UnitPrice3 = uctxtUnitPrice3.CurrencyValue.Value;

            if (uctxtUnitPrice4.CurrencyValue.HasValue)
                inv.UnitPrice4 = uctxtUnitPrice4.CurrencyValue.Value;

            if (uctxtUnitPrice5.CurrencyValue.HasValue)
                inv.UnitPrice5 = uctxtUnitPrice5.CurrencyValue.Value;

            if (uctxtRealCost.CurrencyValue.HasValue)
                inv.RealCost = uctxtRealCost.CurrencyValue.Value;

            if (uctxtProfit.CurrencyValue.HasValue)
                inv.Profit = uctxtProfit.CurrencyValue.Value;
            AddProduct(inv, selProduct.Name);
        }
        else
            ShowError(Exception.nonexistentProduct);

        BindGrid();

        CleanFields();
    }

    #endregion
}