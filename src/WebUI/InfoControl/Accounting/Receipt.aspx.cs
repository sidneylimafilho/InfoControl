using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;

using Exception = Resources.Exception;

[PermissionRequired("Receipt")]
public partial class InfoControl_Administration_Receipt : Vivina.Erp.SystemFramework.PageBase
{
    #region properties

    private List<ReceiptItem> ReceiptItems
    {
        get
        {
            if (Page.ViewState["_lstReceiptItem"] == null)
                Page.ViewState["_lstReceiptItem"] = new List<ReceiptItem>();
            return (List<ReceiptItem>)Page.ViewState["_lstReceiptItem"];
        }
        set { Page.ViewState["_lstReceiptItem"] = value; }
    }



    private Receipt Original_Receipt
    {
        get
        {
            if (Session["_original_Receipt"] == null)
            {
                Session["_original_Receipt"] = new Receipt();
                if (IsLoaded)
                    Session["_original_Receipt"] =
                        ReceiptManager.GetReceipt(Convert.ToInt32(Page.ViewState["ReceiptId"]), Company.CompanyId);
            }
            return (Receipt)Session["_original_Receipt"];
        }
        set { Session["_original_Receipt"] = value; }
    }

    private Boolean IsLoaded
    {
        get { return Page.ViewState["ReceiptId"] != null; }
    }

    private ProductManager _productManager;
    private ProductManager ProductManager
    {
        get
        {
            return _productManager ?? (_productManager = new ProductManager(this));
        }

    }

    private ServicesManager _servicesManager;
    private ServicesManager ServicesManager
    {
        get
        {
            return _servicesManager ?? (_servicesManager = new ServicesManager(this));
        }

    }

    private ReceiptManager _receiptManager;
    private ReceiptManager ReceiptManager
    {
        get
        {
            return _receiptManager ?? (_receiptManager = new ReceiptManager(this));
        }
    }

    private InventoryManager _inventoryManager;
    private InventoryManager InventoryManager
    {
        get
        {
            return _inventoryManager ?? (_inventoryManager = new InventoryManager(this));
        }

    }

    private SaleManager _saleManager;
    private SaleManager SaleManager
    {
        get
        {
            return _saleManager ?? (_saleManager = new SaleManager(this));
        }

    }

    private List<Int32> LstServiceOrder
    {
        get
        {
            if (Page.ViewState["_lstServiceOrder"] == null)
                Page.ViewState["_lstServiceOrder"] = new List<Int32>();
            return (List<Int32>)Page.ViewState["_lstServiceOrder"];
        }
        set { Page.ViewState["_lstServiceOrder"] = value; }
    }

    private List<Int32> LstSale
    {
        get
        {
            if (Page.ViewState["_lstSale"] == null)
                Page.ViewState["_lstSale"] = new List<Int32>();
            return (List<Int32>)Page.ViewState["_lstSale"];
        }
        set { Page.ViewState["_lstSale"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearStateValues();

            ShowCustomerFields();

            ucIssueDate.DateTime = DateTime.Now.Date;

            if (Context.Items["ReceiptId"] != null)
                Page.ViewState["ReceiptId"] = Context.Items["ReceiptId"];

            if (!String.IsNullOrEmpty(Request["ReceiptId"]))
                Page.ViewState["ReceiptId"] = Request["ReceiptId"].DecryptFromHex();

            if (IsLoaded)
                ShowReceipt();
            else
            {
                if (!String.IsNullOrEmpty(Request["ServiceOrderId"]))
                {
                    LoadServiceOrder(Convert.ToInt32(Request["ServiceOrderId"].DecryptFromHex()));
                    cboSaleAndOs.Items.Remove(cboSaleAndOs.Items.FindByValue(Request["ServiceOrderId"].DecryptFromHex()));
                }

                if (!String.IsNullOrEmpty(Request["SaleId"]))
                    LoadSale(Convert.ToInt32(Request["SaleId"].DecryptFromHex()));
            }
        }
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            Page.ViewState["customerId"] = e.Customer.CustomerId;
            Page.ViewState["SupplierId"] = null;
            LoadCboSaleAndOs(e.Customer.CustomerId);
        }
    }

    protected void SelTransporter_SelectedTransporter(object sender, SelectedTransporterEventArgs e)
    {
        if (e.Transporter != null)
            Page.ViewState["TransporterId"] = e.Transporter.TransporterId;
    }

    protected void SelSupplier_SelectedSupplier(object sender, SelectedSupplierEventArgs e)
    {
        if (e.Supplier != null)
        {
            Page.ViewState["SupplierId"] = e.Supplier.SupplierId;
            Page.ViewState["customerId"] = null;
            ResetcboSaleAndOs();
        }
    }

    protected void btnReceipItemProduct_Click(object sender, ImageClickEventArgs e)
    {
        // 
        // Popula o receiptItem 
        //
        Inventory inventory;
        Decimal icms = 0, ipi = 0;
        Product product = SelProductAndService.Product;
        Service service = SelProductAndService.Service;
        var receiptItem = new ReceiptItem();

        if (ucCurrFieldIPI.CurrencyValue.HasValue)
            ipi = ucCurrFieldIPI.CurrencyValue.Value;

        if (ucCurrFieldICMS.CurrencyValue.HasValue)
            icms = ucCurrFieldICMS.CurrencyValue.Value;

        receiptItem.ICMS = icms;
        receiptItem.IPI = ipi;
        receiptItem.Description = SelProductAndService.Name;
        receiptItem.UnitPrice = ucCurrFieldUnitPrice.CurrencyValue;

        if (ucCurrFieldProductQuantity.IntValue != 0)
            receiptItem.Quantity = ucCurrFieldProductQuantity.IntValue;
        else
        {
            ShowError("Quantidade não pode ser zero!");
            return;
        }

        if (product != null)
        {
            receiptItem.ProductId = product.ProductId;
            receiptItem.Description = product.Name;
            if (!ucCurrFieldUnitPrice.CurrencyValue.HasValue)
            {
                inventory = InventoryManager.GetInventory(Company.CompanyId, product.ProductId, Deposit.DepositId);
                if (inventory != null)
                    receiptItem.UnitPrice = inventory.UnitPrice;
            }
        }
        else if (service != null)
        {
            receiptItem.ServiceId = service.ServiceId;
            receiptItem.Description = service.Name;
            if (!ucCurrFieldUnitPrice.CurrencyValue.HasValue)
                receiptItem.UnitPrice = service.Price;
        }

        //
        // Adiciona na Lista que está em memória
        // OBS: Ainda não foi para o banco de dados
        //
        ReceiptItems.Add(receiptItem);

        CalculateTotal(GetReceiptItemTotalValue(receiptItem, 1));

        BindReceiptItems();

        //
        // Limpar os campos de produto
        //
        ucCurrFieldProductQuantity.Text = String.Empty;
        ucCurrFieldUnitPrice.Text = String.Empty;
        ucCurrFieldIPI.Text = String.Empty;
        ucCurrFieldICMS.Text = String.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Focus();
        var receipt = new Receipt();
        var lstSavingReceiptItem = new List<ReceiptItem>();

        if (!ReceiptItems.Any())
        {
            ShowError(Exception.UnselectedItem);
            return;
        }

        UpdateReceiptValue();

        if (IsLoaded)
            receipt.CopyPropertiesFrom(Original_Receipt);

        receipt.SubstitutionICMSBase = ucCurrFieldSubstituionICMSBase.CurrencyValue;
        receipt.SubstitutionICMSValue = ucCurrFieldSubstituionICMSValue.CurrencyValue;
        receipt.FreightValue = ucCurrFieldFreightValue.CurrencyValue;
        receipt.InsuranceValue = ucCurrFieldInsuranceValue.CurrencyValue;
        receipt.OthersChargesValue = ucCurrFieldOthersChargesValue.CurrencyValue;


        if (!String.IsNullOrEmpty(lblReceiptValue.Text))
            receipt.ReceiptValue = Convert.ToDecimal(lblReceiptValue.Text.Replace("_", ""));

        receipt.CompanyId = Company.CompanyId;

        if (Page.ViewState["customerId"] != null)
        {
            receipt.SupplierId = null;
            receipt.CustomerId = Convert.ToInt32(Page.ViewState["customerId"]);
        }
        else
        {
            receipt.CustomerId = null;
            receipt.SupplierId = Convert.ToInt32(Page.ViewState["SupplierId"]);
        }
        if (Page.ViewState["TransporterId"] != null)
            receipt.TransporterId = Convert.ToInt32(Page.ViewState["TransporterId"]);

        receipt.DeliveryDate = null;
        receipt.EntryDate = null;

        if (ucEntrydate.DateTime.HasValue)
            receipt.EntryDate = ucEntrydate.DateTime;
        else
            receipt.DeliveryDate = ucDeliveryDate.DateTime;

        if (ucIssueDate.DateTime.HasValue)
            receipt.IssueDate = ucIssueDate.DateTime.Value;

        receipt.CfopId = Convert.ToInt32(cboCFOP.SelectedValue);

        receipt.ReceiptNumber = ucCurrFieldReceiptNumber.IntValue;

        foreach (ReceiptItem item in ReceiptItems)
        {
            item.ReceiptId = receipt.ReceiptId;
            lstSavingReceiptItem.Add(item);
        }

        try
        {
            ///update ReceiptValue
            ReceiptManager.SaveReceipt(Original_Receipt, receipt, lstSavingReceiptItem, LstServiceOrder, LstSale);
        }
        catch (InvalidOperationException)
        {
            ShowError(Exception.InvalidReceiptNumber);
            return;
        }
        Server.Transfer("Receipts.aspx");
    }

    protected void grdReceiptItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //
        // Recalcula o total e Remove a linha da lista em memória
        //
        CalculateTotal(-GetReceiptItemTotalValue(ReceiptItems[e.RowIndex], -1));
        ReceiptItems.RemoveAt(e.RowIndex);

        //
        // Atualiza a grid já sem o item removido
        //
        BindReceiptItems();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Receipts.aspx");
    }

    protected void cboSaleAndOs_TextChanged(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(cboSaleAndOs.SelectedValue))
        {
            if (cboSaleAndOs.SelectedItem.Text.Contains("VE"))
                LoadSale(Convert.ToInt32(cboSaleAndOs.SelectedValue));
            else
                LoadServiceOrder(Convert.ToInt32(cboSaleAndOs.SelectedValue));

            cboSaleAndOs.Items.Remove(cboSaleAndOs.SelectedItem);

        }
    }

    protected void updateReceiptValue_TextChanged(object sender, EventArgs e) { }

    protected void rbtSelCustomer_CheckedChanged(object sender, EventArgs e)
    {
        ShowCustomerFields();
    }

    protected void rbtSelSupplier_CheckedChanged(object sender, EventArgs e)
    {
        ShowSupplierFields();
    }

    #region Functions

    /// <summary>
    /// Função que limpa e inicializa as variáveis da ViewState
    /// </summary>
    private void ClearStateValues()
    {
        ReceiptItems = null;
        Original_Receipt = null;
        ViewState["total"] = 0;

        ucCurrFieldICMSBase.CurrencyValue = null;
        ucCurrFieldICMSValue.CurrencyValue = null;
        ucCurrFieldSubstituionICMSBase.CurrencyValue = null;
        ucCurrFieldTotalProductValue.CurrencyValue = null;
        ucCurrFieldFreightValue.CurrencyValue = null;
        ucCurrFieldInsuranceValue.CurrencyValue = null;
        ucCurrFieldOthersChargesValue.CurrencyValue = null;
        ucCurrFieldIPITotalValue.CurrencyValue = null;
        lblReceiptValue.Text = String.Empty;
    }

    /// <summary>
    /// this method update the receiptValue
    /// </summary>
    private void UpdateReceiptValue()
    {
        Decimal icmsValue = 0,
                ipiTotalValue = 0,
                substituionIcmsValue = 0,
                othersValues = 0,
                freightValue = 0,
                insuranceValue = 0,
                receiptValue = 0,
                productValue = 0;

        if (ucCurrFieldICMSValue.CurrencyValue.HasValue)
            icmsValue = ucCurrFieldICMSValue.CurrencyValue.Value;

        if (ucCurrFieldIPITotalValue.CurrencyValue.HasValue)
            ipiTotalValue = ucCurrFieldIPITotalValue.CurrencyValue.Value;

        if (ucCurrFieldSubstituionICMSValue.CurrencyValue.HasValue)
            substituionIcmsValue = ucCurrFieldSubstituionICMSValue.CurrencyValue.Value;

        if (ucCurrFieldOthersChargesValue.CurrencyValue.HasValue)
            othersValues = ucCurrFieldOthersChargesValue.CurrencyValue.Value;

        if (ucCurrFieldFreightValue.CurrencyValue.HasValue)
            freightValue = ucCurrFieldFreightValue.CurrencyValue.Value;

        if (ucCurrFieldInsuranceValue.CurrencyValue.HasValue)
            insuranceValue = ucCurrFieldInsuranceValue.CurrencyValue.Value;

        productValue = Convert.ToDecimal(ViewState["total"]);

        receiptValue = productValue + icmsValue + ipiTotalValue + substituionIcmsValue + othersValues + freightValue +
                       insuranceValue;
        lblReceiptValue.Text = receiptValue.ToString();
    }

    /// <summary>
    /// this function fill the ReceiptItem and return this
    /// </summary>
    /// <param name="receiptId"></param>
    /// <param name="receiptItemRow"></param>
    /// <returns></returns>
    private ReceiptItem SetReceiptItemValues(Int32 receiptId, ReceiptItem item)
    {
        var receiptItem = new ReceiptItem();
        receiptItem.CompanyId = Company.CompanyId;
        receiptItem.Description = item.Description;
        receiptItem.ReceiptId = receiptId;
        receiptItem.ServiceId = item.ServiceId;
        receiptItem.ProductId = item.ProductId;
        receiptItem.FiscalClass = item.FiscalClass;
        receiptItem.Quantity = item.Quantity;
        receiptItem.UnitPrice = item.UnitPrice;
        receiptItem.IPI = item.IPI;
        receiptItem.ICMS = item.ICMS;
        return receiptItem;
    }

    /// <summary>
    /// this method show the ReceiptItems
    /// 
    /// </summary>
    private void BindReceiptItems()
    {
        grdReceiptItem.DataSource = ReceiptItems;
        grdReceiptItem.DataBind();
    }

    /// <summary>
    /// this function show the receipt
    /// </summary>
    /// <param name="receipt"></param>
    private void ShowReceipt()
    {
        ucCurrFieldReceiptNumber.Text = Convert.ToString(Original_Receipt.ReceiptNumber);

        if (Original_Receipt.EntryDate.HasValue)
        {
            ShowSupplierFields();
            ucEntrydate.DateTime = Original_Receipt.EntryDate;
            SelSupplier.ShowSupplier(Original_Receipt.Supplier);
            rbtSelSupplier.Checked = true;
        }
        else
        {
            ShowCustomerFields();
            ucDeliveryDate.DateTime = Original_Receipt.DeliveryDate;
            SelCustomer.ShowCustomer(Original_Receipt.Customer);
            rbtSelCustomer.Checked = true;
        }

        if (Convert.ToBoolean(Request["ReadOnly"]))
            DisableControlsForPopUpView();

        ucDeliveryDate.DateTime = Original_Receipt.DeliveryDate;
        ucIssueDate.DateTime = Original_Receipt.IssueDate;

        if (Original_Receipt.CfopId.HasValue)
            cboCFOP.SelectedValue = Original_Receipt.CfopId.ToString();

        SelTransporter.ShowTransporter(Original_Receipt.Transporter);

        LoadReceiptItems();

        ucCurrFieldSubstituionICMSValue.CurrencyValue = Original_Receipt.SubstitutionICMSValue;
        ucCurrFieldSubstituionICMSBase.CurrencyValue = Original_Receipt.SubstitutionICMSBase;
        ucCurrFieldOthersChargesValue.CurrencyValue = Original_Receipt.OthersChargesValue;
        ucCurrFieldFreightValue.CurrencyValue = Original_Receipt.FreightValue;
        ucCurrFieldInsuranceValue.CurrencyValue = Original_Receipt.InsuranceValue;

        DisableReceiptType();
    }

    /// <summary>
    /// this method load one serviceOrder and its items
    /// </summary>
    /// <param name="serviceOrderNumber"></param>
    private void LoadServiceOrder(Int32 serviceOrderId)
    {
        ServiceOrder serviceOrder = ServicesManager.GetServiceOrder(serviceOrderId);
        LstServiceOrder.Add(serviceOrder.ServiceOrderId);

        var serviceOrderItems = ServicesManager.GetServiceOrderItems(serviceOrderId);

        foreach (ServiceOrderItem serviceOrderItem in serviceOrderItems)
            ConvertServiceOrderItemToReceiptItem(serviceOrderItem);

        if (!IsPostBack)
            SelCustomer.ShowCustomer(serviceOrder.Customer);

        UpdateReceiptValue();
        BindReceiptItems();
    }




    /// <summary>
    /// this function shmw all customerFields
    /// </summary>
    private void ShowCustomerFields()
    {
        pnlCustomer.Visible = true;
        pnlSupplier.Visible = false;
        pnlEntryDate.Visible = false;
        pnlDeliveryDate.Visible = true;
        Page.ViewState["SupplierId"] = null;
        rbtSelCustomer.Checked = true;
        cboSaleAndOs.Items.Clear();
        var listItem = new ListItem();
        listItem.Text = "";
        listItem.Value = "";
        cboSaleAndOs.Items.Add(listItem);
    }

    /// <summary>
    /// this function shmw all customerFields
    /// </summary>
    private void ShowSupplierFields()
    {
        pnlCustomer.Visible = false;
        pnlSupplier.Visible = true;
        pnlEntryDate.Visible = true;
        pnlDeliveryDate.Visible = false;
        Page.ViewState["customerId"] = null;
        cboSaleAndOs.Items.Clear();
        var listItem = new ListItem();
        listItem.Text = "";
        listItem.Value = "";
        cboSaleAndOs.Items.Add(listItem);
    }

    /// <summary>
    /// this function disable the radiobuttons
    /// </summary>
    private void DisableReceiptType()
    {
        rbtSelCustomer.Enabled = rbtSelSupplier.Enabled = false;
    }

    /// <summary>
    /// this method reset the cboSaleAndOs
    /// </summary>
    private void ResetcboSaleAndOs()
    {
        cboSaleAndOs.Items.Clear();
        var listItem = new ListItem();
        listItem.Text = String.Empty;
        listItem.Value = String.Empty;
        cboSaleAndOs.Items.Add(listItem);
        cboSaleAndOs.SelectedValue = String.Empty;
    }

    /// <summary>
    /// this method fill the cboSaleAndOs
    /// </summary>
    private void LoadCboSaleAndOs(Int32 customerId)
    {
        ListItem lstItem;
        DataTable dtSaleAndOs = ReceiptManager.GetSaleAndOSNonInvoiced(Company.CompanyId, customerId);
        cboSaleAndOs.Items.Clear();
        cboSaleAndOs.Items.Add(String.Empty);
        foreach (DataRow row in dtSaleAndOs.Rows)
        {
            lstItem = new ListItem();
            lstItem.Text = row["number"].ToString();
            if (row["number"].ToString().Contains("VE-"))
                lstItem.Text += Convert.ToDateTime(row["date"]).ToShortDateString();
            lstItem.Value = row["id"].ToString();
            cboSaleAndOs.Items.Add(lstItem);
        }
    }

    /// <summary>
    /// Carrega os items que vem do banco na lista em memória, lista essa que popula a grid
    /// </summary>
    private void LoadReceiptItems()
    {
        List<ReceiptItem> loadedReceiptItems =
            ReceiptManager.GetReceiptItemsAsList(Company.CompanyId, Original_Receipt.ReceiptId);

        foreach (ReceiptItem item in loadedReceiptItems)
        {
            if (item.ProductId.HasValue)
                AddReceiptItem(item.Product, item);
            else if (item.ServiceId.HasValue)
                AddReceiptItem(item.Service, item);
            else
                AddReceiptItem((Product)null, item);
        }
        BindReceiptItems();
    }

    /// <summary>
    /// Pega o valor total que está na viewState e adiciona a variavel othersValues
    /// </summary>
    /// <param name="receiptItem"></param>
    /// <returns></returns>
    private void CalculateTotal(decimal othersValues)
    {
        //
        // Pega o valor total que está na viewState e adiciona a variavel othersValues
        //
        ViewState["total"] = Convert.ToDecimal(ViewState["total"]) + othersValues;

        UpdateReceiptValue();
    }

    /// <summary>
    /// Calcula o valor total de uma linha da Grid
    /// </summary>
    /// <param name="receiptItem"></param>
    /// <returns></returns>
    private decimal GetReceiptItemTotalValue(ReceiptItem receiptItem, int factor)
    {
        Decimal icmsValue = Decimal.Zero;
        Decimal totalProductValue = Convert.ToDecimal(receiptItem.UnitPrice * receiptItem.Quantity);
        Decimal ipiValue = 0;

        if (receiptItem.ICMS > decimal.Zero)
            icmsValue = Convert.ToDecimal(totalProductValue * (receiptItem.ICMS / 100));

        if (receiptItem.IPI > decimal.Zero)
            ipiValue = Convert.ToDecimal(totalProductValue * (receiptItem.IPI / 100));

        if (!ucCurrFieldIPITotalValue.CurrencyValue.HasValue)
            ucCurrFieldIPITotalValue.CurrencyValue = 0;

        ucCurrFieldIPITotalValue.CurrencyValue += ipiValue * factor;

        if (!ucCurrFieldICMSValue.CurrencyValue.HasValue)
            ucCurrFieldICMSValue.CurrencyValue = 0;

        ucCurrFieldICMSValue.CurrencyValue += icmsValue * factor;

        if (!ucCurrFieldTotalProductValue.CurrencyValue.HasValue)
            ucCurrFieldTotalProductValue.CurrencyValue = decimal.Zero;

        ucCurrFieldTotalProductValue.CurrencyValue += totalProductValue * factor;

        return totalProductValue;
    }

    private void AddReceiptItem(Service service, ReceiptItem receiptItem)
    {
        var serviceItem = new ReceiptItem();
        serviceItem.ReceiptItemId = receiptItem.ReceiptItemId;
        serviceItem.CompanyId = receiptItem.CompanyId;
        serviceItem.ServiceId = receiptItem.ServiceId;
        serviceItem.UnitPrice = receiptItem.UnitPrice;
        serviceItem.Quantity = 1;
        serviceItem.ProductId = receiptItem.ProductId;
        serviceItem.IPI = receiptItem.IPI;
        serviceItem.ICMS = receiptItem.ICMS;
        serviceItem.Description = !String.IsNullOrEmpty(service.Name)
                                      ? service.Name
                                      : receiptItem.Description;
        ReceiptItems.Add(serviceItem);

        CalculateTotal(GetReceiptItemTotalValue(serviceItem, 1));
    }

    private void AddReceiptItem(Product product, ReceiptItem receiptItem)
    {
        String productName = String.Empty;

        if (receiptItem.ProductId.HasValue)
            product = ProductManager.GetProduct(receiptItem.ProductId.Value, Company.CompanyId);

        if (product != null)
        {
            receiptItem.ICMS = product.ICMS;
            receiptItem.IPI = product.IPI;
            productName = product.Name;
        }

        var productItem = new ReceiptItem();
        productItem.ReceiptItemId = receiptItem.ReceiptItemId;
        productItem.ReceiptId = receiptItem.ReceiptId;
        productItem.UnitPrice = receiptItem.UnitPrice;
        productItem.Quantity = Convert.ToInt32(receiptItem.Quantity);
        productItem.IPI = receiptItem.IPI;
        productItem.ICMS = receiptItem.ICMS;
        productItem.ProductId = receiptItem.ProductId;
        productItem.Description = !String.IsNullOrEmpty(productName)
                                      ? productName
                                      : receiptItem.Description;
        ReceiptItems.Add(productItem);

        CalculateTotal(GetReceiptItemTotalValue(productItem, 1));
    }

    private void LoadSale(Int32 saleId)
    {
        Sale sale = new SaleManager(this).GetSale(
            Company.MatrixId.HasValue
                ? Company.MatrixId.Value
                : Company.CompanyId, saleId);
        LstSale.Add(sale.SaleId);

        ReceiptItem receiptItem;
        foreach (SaleItem item in sale.SaleItems)
        {
            receiptItem = new ReceiptItem
                          {
                              Quantity = Convert.ToInt32(item.Quantity),
                              UnitPrice = item.UnitPrice,
                              Description = item.SpecialProductName
                          };

            if (item.ProductId.HasValue)
            {
                receiptItem.ProductId = item.ProductId;
                receiptItem.IPI = item.Product.IPI;
                receiptItem.ICMS = item.Product.ICMS;
                receiptItem.Description = item.Product.Name;

            }
            AddReceiptItem(item.Product, receiptItem);

        }

        SelCustomer.ShowCustomer(sale.Customer);

        ///set discount of sale as otherValues
        ///change the sign because the discount is default
        ucCurrFieldOthersChargesValue.CurrencyValue += sale.Discount;

        BindReceiptItems();
    }

    private void ConvertServiceOrderItemToReceiptItem(ServiceOrderItem serviceOrderItem)
    {
        var receiptItem = new ReceiptItem();

        receiptItem.CompanyId = Company.CompanyId;
        receiptItem.Quantity = 1;

        if (serviceOrderItem.ProductId.HasValue)
        {
            receiptItem.ProductId = serviceOrderItem.ProductId.Value;
            receiptItem.Description = serviceOrderItem.Product.Name;
            receiptItem.FiscalClass = serviceOrderItem.Product.FiscalClass;
            Inventory inventory = new InventoryManager(this).GetProductInventory(Company.CompanyId,
                                                                                 receiptItem.ProductId.Value,
                                                                                 Deposit.DepositId);
            receiptItem.UnitPrice = inventory != null
                                        ? inventory.UnitPrice
                                        : Decimal.Zero;
            AddReceiptItem(serviceOrderItem.Product, receiptItem);
        }
        else
        {
            receiptItem.UnitPrice = serviceOrderItem.Price; 
            receiptItem.ServiceId = serviceOrderItem.ServiceId.Value;
            receiptItem.Description = serviceOrderItem.Service.Name;
            AddReceiptItem(serviceOrderItem.Service, receiptItem);
        }
    }

    /// <summary>
    /// This Function disable controls to not allow the user to edit their value
    /// when in a popup from CustomerCentral.
    /// </summary>
    private void DisableControlsForPopUpView()
    {
        Title = String.Empty;
        lblIssueDate.Text = ucIssueDate.Text;
        lblDeliveryDate.Text = ucDeliveryDate.Text;
        lblIssueDate.Visible = true;
        lblDeliveryDate.Visible = true;
        ucIssueDate.Visible = false;
        ucEntrydate.Visible = false;
        ucDeliveryDate.Visible = false;
        ucCurrFieldReceiptNumber.Enabled = false;
        cboCFOP.Enabled = false;
        SelCustomer.Enabled = false;
        rowReceiptItem.Visible = false;
        grdReceiptItem.Enabled = false;
        ucCurrFieldICMSBase.Enabled = false;
        ucCurrFieldICMSValue.Enabled = false;
        ucCurrFieldSubstituionICMSBase.Enabled = false;
        ucCurrFieldSubstituionICMSValue.Enabled = false;
        ucCurrFieldTotalProductValue.Enabled = false;
        ucCurrFieldFreightValue.Enabled = false;
        ucCurrFieldInsuranceValue.Enabled = false;
        ucCurrFieldOthersChargesValue.Enabled = false;
        ucCurrFieldIPITotalValue.Enabled = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
    }

    #endregion
}