using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

[PermissionRequired("ServiceOrders")]
public partial class InfoControl_Services_ServiceOrder : Vivina.Erp.SystemFramework.PageBase
{

    #region CustomTypes
    [Serializable]
    public class ServiceItem
    {
        #region Private
        private int _serviceId;
        private int _companyId;
        private String _name;
        private int _timeInMinutes;
        private decimal _servicePrice;
        private Int32 _employeeId;
        private String _employeeName;
        #endregion

        #region Public
        public int ServiceId { get { return _serviceId; } set { _serviceId = value; } }
        public int CompanyId { get { return _companyId; } set { _companyId = value; } }
        public String Name { get { return _name; } set { _name = value; } }
        public int TimeInMinutes { get { return _timeInMinutes; } set { _timeInMinutes = value; } }
        public Decimal ServicePrice { get { return _servicePrice; } set { _servicePrice = value; } }
        public int EmployeeId { get { return _employeeId; } set { _employeeId = value; } }
        public String EmployeeName { get { return _employeeName; } set { _employeeName = value; } }

        #endregion


        public ServiceItem(Int32 serviceId, Int32 companyId, String name, int timeInMinutes, Decimal servicePrice, Int32 employeeId, String employeeName)
        {
            this.ServiceId = serviceId;
            this.CompanyId = companyId;
            this.Name = name;
            this.TimeInMinutes = timeInMinutes;
            this.ServicePrice = servicePrice;
            this.EmployeeId = employeeId;
            this.EmployeeName = employeeName;

        }
    }
    [Serializable]
    public class ProductItem
    {
        #region Private
        private int _productId;
        private String _productName;
        private String _description;
        private Decimal _productPrice;
        private Boolean _isApplied;
        private int _companyId;
        private int _quantity;
        #endregion

        #region Public
        public int CompanyId { get { return _companyId; } set { _companyId = value; } }
        public Int32 ProductId { get { return _productId; } set { _productId = value; } }
        public String ProductName { get { return _productName; } set { _productName = value; } }
        public String Description { get { return _description; } set { _description = value; } }
        public Decimal ProductPrice { get { return _productPrice; } set { _productPrice = value; } }
        public Boolean IsApplied { get { return _isApplied; } set { _isApplied = value; } }
        public Int32 Quantity { get { return _quantity; } set { _quantity = value; } }
        #endregion

        public ProductItem(Int32 companyId, Int32 productId, String productName, Int32 quantity, String description, Decimal productPrice, Boolean isApplied)
        {
            this.CompanyId = companyId;
            this.ProductId = productId;
            this.Description = description;
            this.ProductPrice = productPrice;
            this.IsApplied = IsApplied;
            this.ProductName = productName;
            this.Quantity = quantity;
        }
    }

    #endregion

    #region Managers

    //Manager
    private ServicesManager _servicesManager;
    private CustomerManager _customerManager;
    private ProductManager productManager;
    private InventoryManager inventoryManager;

    #endregion

    #region privateVariables

    private ServiceOrder _original_ServiceOrder;
    private Decimal TotalProductPrice;
    private Decimal TotaServicePrice;

    #endregion

    #region properties

    public ServicesManager ServicesManager
    {
        get
        {
            if (_servicesManager == null)
                _servicesManager = new ServicesManager(this);
            return _servicesManager;
        }
    }

    public CustomerManager CustomerManager
    {
        get
        {
            if (_customerManager == null)
                _customerManager = new CustomerManager(this);
            return _customerManager;
        }
    }

    public Boolean IsLoaded
    {
        get
        {
            return serviceOrderId != null;
        }
    }

    public ServiceOrder Original_ServiceOrder
    {
        get
        {
            if (_original_ServiceOrder == null)
            {
                _original_ServiceOrder = new ServiceOrder();
                if (serviceOrderId != null)
                    _original_ServiceOrder = ServicesManager.GetServiceOrder(Convert.ToInt32(serviceOrderId));
            }
            return _original_ServiceOrder;
        }
    }

    public List<ServiceItem> ServiceItemsList
    {
        get
        {
            if (Session["_serviceItemsList"] == null)
            {
                Session["_serviceItemsList"] = new List<ServiceItem>();
                if (IsLoaded)
                {
                    ServiceItem serviceItem;
                    List<ServiceOrderItem> lstServiceOrderItem = new ServicesManager(this).GetServiceOrderItemsAsList(Original_ServiceOrder.ServiceOrderId);

                    foreach (ServiceOrderItem item in lstServiceOrderItem)
                    {
                        if (item.ServiceId.HasValue)
                        {
                            serviceItem = new ServiceItem(item.ServiceId.Value, item.CompanyId, item.Service.Name, item.Service.TimeInMinutes, item.Price ?? item.Service.Price, Convert.ToInt32(item.EmployeeId), item.Employee == null ? "" : item.Employee.Profile.Name);
                            (Session["_serviceItemsList"] as List<ServiceItem>).Add(serviceItem);
                        }
                    }
                }
            }
            return (List<ServiceItem>)Session["_serviceItemsList"];
        }
        set
        {
            Session["_serviceItemsList"] = value;
        }
    }

    public List<ProductItem> ProductItemsList
    {
        get
        {
            if (Session["_productItemsList"] == null)
            {
                Session["_productItemsList"] = new List<ProductItem>();
                if (IsLoaded)
                {
                    ProductItem productItem;
                    List<ServiceOrderItem> lstServiceOrderItem = new ServicesManager(this).GetServiceOrderItemsAsList(Original_ServiceOrder.ServiceOrderId);

                    foreach (ServiceOrderItem item in lstServiceOrderItem)
                    {

                        if (item.ProductId.HasValue)
                        {
                            Decimal productPrice = decimal.Zero;
                            Inventory inventory = new Inventory();

                            if (Deposit != null)
                                inventory = new InventoryManager(this).GetInventory(Company.CompanyId, item.ProductId.Value, Deposit.DepositId);

                            if (inventory != null)
                                productPrice = inventory.UnitPrice;

                            productItem = new ProductItem(item.CompanyId, item.ProductId.Value, item.Product.Name, Convert.ToInt32(item.Quantity), item.Description, productPrice, item.IsApplied == null ? false : item.IsApplied.Value);
                            (Session["_productItemsList"] as List<ProductItem>).Add(productItem);
                        }
                    }
                }
            }
            return (List<ProductItem>)Session["_productItemsList"];
        }
        set
        {
            Session["_productItemsList"] = value;
        }
    }

    #endregion

    Int32? serviceOrderId;

    #region events
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!String.IsNullOrEmpty(Request["ServiceOrderId"]))
            serviceOrderId = Convert.ToInt32(Request["ServiceOrderId"].DecryptFromHex());

        if (!IsPostBack)
        {
            ServiceItemsList = null;
            ProductItemsList = null;

            if (Convert.ToBoolean(Request["ReadOnly"]))
                DisableControlsForPopUpView();

            chkServiceType.DataBind();
            pnlTestType.Visible = chkServiceType.Items.Count > 0;

            chklstEquipmentDamage.DataBind();
            pnlEquipmentDamage.Visible = chklstEquipmentDamage.Items.Count > 0;

            chklstProductDamage.DataBind();
            pnlProductDamage.Visible = chklstProductDamage.Items.Count > 0;

            chkServiceType.DataBind();
            pnlServiceType.Visible = chkServiceType.Items.Count > 0;

            chkProductType.DataBind();
            pnlProductType.Visible = chkProductType.Items.Count > 0;

            chkInstallType.DataBind();
            pnlInstallType.Visible = chkInstallType.Items.Count > 0;

            chkHaltType.DataBind();
            pnlHaltType.Visible = chkHaltType.Items.Count > 0;

            chklstTests.DataBind();
            pnlTestType.Visible = chklstTests.Items.Count > 0;

            txtServiceOrderNumber.Text = Util.GenerateUniqueID();

            pnlCustomerContracts.Attributes["style"] = "display:none;";
            pnlCustomerCalls.Attributes["style"] = "display:none;";
            updPanelEquipment.Attributes["style"] = "display:none;";

            //verify the existence of ServiceOrder
            if (!String.IsNullOrEmpty(Request["ServiceOrderId"]))
            {
                btnShowAppointments.OnClientClick = "top.tb_show('Agenda' , '../infocontrol/Appointments.aspx?ServiceOrderId=" + Request["ServiceOrderId"] + "'); return false;";

                btnGenerateReceipt.Visible = true;

                //bind Combobox
                cboServiceOrderType.DataBind();
                cboServiceOrderStatus.DataBind();

                //set ServiceOrderNumber
                txtServiceOrderNumber.Text = Original_ServiceOrder.ServiceOrderNumber;

                //set combobox
                if (cboServiceOrderType.Items.FindByValue(Convert.ToString(Original_ServiceOrder.ServiceOrderTypeId)) != null)
                    cboServiceOrderType.SelectedValue = Convert.ToString(Original_ServiceOrder.ServiceOrderTypeId);

                if (Original_ServiceOrder.ServiceOrderStatusId.HasValue)
                    cboServiceOrderStatus.SelectedValue = Convert.ToString(Original_ServiceOrder.ServiceOrderStatusId);

                if (Original_ServiceOrder.DepositId.HasValue)
                    cboDeposit.SelectedValue = Convert.ToString(Original_ServiceOrder.DepositId);

                if (Original_ServiceOrder.CustomerCallId.HasValue)
                {
                    LoadCustomerCallsFromCustomer(Original_ServiceOrder.Customer);

                    cboCustomerCalls.SelectedValue = Original_ServiceOrder.CustomerCallId.ToString();

                    pnlCustomerCalls.Attributes["style"] = "display:block";


                    if (Original_ServiceOrder.CustomerCall.CustomerEquipmentId.HasValue)
                        ShowCustomerEquipment(Original_ServiceOrder.CustomerCall.CustomerEquipment);

                }

                SelCustomer.ShowCustomer(Original_ServiceOrder.Customer);
                
                if (Original_ServiceOrder.CustomerEquipmentId.HasValue)                    
                    ShowCustomerEquipment(Original_ServiceOrder.CustomerEquipment);

                if (Original_ServiceOrder.ContractId.HasValue)
                    cboCustomerContracts.SelectedValue = Original_ServiceOrder.ContractId.ToString();

                lblCreatedDate.Text = "Data de Abertura:<br />" + Original_ServiceOrder.OpenedDate.ToString();

                //set the Gridview

                BindService();
                BindProducts();

                SetServiceOrderEquipmentDamage(Original_ServiceOrder.ServiceOrderEquipmentDamageId);
                SetServiceOrderProductDamage(Original_ServiceOrder.ServiceOrderProductDamageId);
                SetServiceTest(Original_ServiceOrder.ServiceOrderTestId);
                SetServiceOrderHaltType(Original_ServiceOrder.ServiceOrderHaltType);
                SetServiceOrderInstallType(Original_ServiceOrder.ServiceOrderInstallType);
                SetServiceOrderProductType(Original_ServiceOrder.ServiceOrderProductType);
                SetServiceOrderType(Original_ServiceOrder.ServiceType);

                //
                //Brings the comments associated to service order being loaded
                //
                ucComments.SubjectId = (int)serviceOrderId;

            } // load CustomerCall
            else if (Context.Items["CustomerCallId"] != null)
            {
                Page.ViewState["CustomerCallId"] = Context.Items["CustomerCallId"];
                CustomerCall customerCall = CustomerManager.GetCustomerCall(Convert.ToInt32(Context.Items["CustomerCallId"]));
                SelCustomer.ShowCustomer(customerCall.Customer);
                if (customerCall.CustomerEquipmentId.HasValue)
                    cboCustomerEquipments.SelectedValue = customerCall.CustomerEquipmentId.ToString();
            }
        }
        if (String.IsNullOrEmpty(Request["ServiceOrderId"]))
            btnShowAppointments.Visible = false;
    }


    /// <summary>
    /// This method loads the customerCalls related to selected customer 
    /// </summary>
    /// <param name="customer"></param>
    private void LoadCustomerCallsFromCustomer(Customer customer)
    {
        cboCustomerCalls.DataSource = customer.CustomerCalls;
        cboCustomerCalls.Items.Add(String.Empty);
        cboCustomerCalls.DataBind();
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        if (e.Customer != null)
        {
            Page.ViewState["CustomerId"] = e.Customer.CustomerId;
            cboCustomerEquipments.Items.Clear();
            cboCustomerCalls.Items.Clear();
            cboCustomerContracts.Items.Clear();
            cboCustomerContracts.Items.Add(String.Empty);

            cboCustomerEquipments.DataSource = e.Customer.CustomerEquipments;



            cboCustomerEquipments.Items.Add(String.Empty);

            cboCustomerEquipments.DataBind();
            cboCustomerContracts.DataBind();

            if (e.Customer.CustomerEquipments.Any())
                updPanelEquipment.Attributes["style"] = "display:block";
            else
                updPanelEquipment.Attributes["style"] = "display:none";

            if (e.Customer.CustomerCalls.Any())
            {
                LoadCustomerCallsFromCustomer(e.Customer);
                pnlCustomerCalls.Attributes["style"] = "display:block";
            }
            else
                pnlCustomerCalls.Attributes["style"] = "display:none";

            pnlCustomerContracts.Attributes["style"] = "display:block";
        }
    }





    protected void btnServiceOrderItemProduct_Click(object sender, ImageClickEventArgs e)
    {
        //search the Product in Inventory
        productManager = new ProductManager(this);
        inventoryManager = new InventoryManager(this);

        Product product = productManager.GetProductByName(Company.CompanyId, txtProduct.Text);

        litErrorMessage.Visible = false;

        if (product != null)
        {
            if (!String.IsNullOrEmpty(cboDeposit.SelectedValue))
                if (!IsValidProductWithDeposit(product))
                    return;

            if (ucCurrFieldQuantity.IntValue == 0)
            {
                litErrorMessage.Visible = true;
                litErrorMessage.Text = "Quantidade não pode ser zero!";
                return;
            }

            Decimal productPrice = Decimal.Zero;
            Inventory inventory = inventoryManager.GetProductInventory(Company.CompanyId, product.ProductId, Deposit.DepositId);
            if (inventory != null)
                productPrice = inventory.UnitPrice;

            ProductItem productItem = new ProductItem(product.CompanyId, product.ProductId, product.Name, ucCurrFieldQuantity.IntValue, txtDescription.Text, productPrice, Convert.ToBoolean(choProductIsApplied.Checked));
            ProductItemsList.Add(productItem);
            BindProducts();

            txtProduct.Text = String.Empty;
            txtDescription.Text = String.Empty;
            ucCurrFieldQuantity.Text = String.Empty;
        }
    }

    protected void btnAddServiceItem_Click(object sender, ImageClickEventArgs e)
    {
        Service service = ServicesManager.GetService(Convert.ToInt32(cboService.SelectedValue));
        if (service == null)
            return;

        ServiceItem serviceItem = new ServiceItem(
            service.ServiceId, service.CompanyId, service.Name, service.TimeInMinutes,
            ucCurrFieldServicePrice.CurrencyValue.Value == 0 ? service.Price : ucCurrFieldServicePrice.CurrencyValue.Value,
            Convert.ToInt32(cboServiceEmployee.SelectedValue), cboServiceEmployee.SelectedItem.Text);

        ServiceItemsList.Add(serviceItem);
        BindService();


        //clear the fields
        cboService.SelectedValue = String.Empty;
        cboServiceEmployee.SelectedValue = String.Empty;
        ucCurrFieldServicePrice.Text = String.Empty;

    }


    protected void grdServiceOrderItemProduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DeleteServiceOrderEquipment(e.RowIndex);
        BindProducts();
    }

    protected void grdService_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DeleteService(e.RowIndex);
        BindService();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ServiceItemsList.Any() && !ProductItemsList.Any())
        {
            ShowError(Resources.Exception.ServiceOrderWithoutProductAndService);
            return;
        }

        Int32? serviceOrderId = SaveServiceOrder();
        if (serviceOrderId.HasValue)
            Response.Redirect("ServiceOrders.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ServiceOrders.aspx");
    }

    protected void grdServiceOrderItemProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TotalProductPrice += Convert.ToDecimal("0" + e.Row.Cells[e.Row.Cells.Count - 2].Text) * Convert.ToInt32(e.Row.Cells[e.Row.Cells.Count - 3].Text);
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('Deseja realmente excluir este item?') == false) return false;");
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[e.Row.Cells.Count - 2].Text = "Total: <b>" + TotalProductPrice.ToString("C") + "</b>";
            e.Row.Cells[e.Row.Cells.Count - 2].HorizontalAlign = HorizontalAlign.Right;
        }
    }


    protected void grdService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TotaServicePrice += Convert.ToDecimal("0" + e.Row.Cells[e.Row.Cells.Count - 2].Text);

            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('Deseja realmente excluir este item?') == false) return false;");
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[e.Row.Cells.Count - 2].Text = "Total: <b>" + TotaServicePrice.ToString("C") + "</b>";
            e.Row.Cells[e.Row.Cells.Count - 2].HorizontalAlign = HorizontalAlign.Right;
        }
    }



    protected void odsEquipments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
        e.InputParameters["CustomerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);
    }


    protected void cboCustomerCalls_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCall customerCall = null;
        cboCustomerEquipments.SelectedValue = String.Empty;
        pnlShowEquipment.Visible = false;

        if (!String.IsNullOrEmpty(cboCustomerCalls.SelectedValue))
            customerCall = new CustomerManager(this).GetCustomerCall(Convert.ToInt32(cboCustomerCalls.SelectedValue));

        if (customerCall.CustomerEquipmentId.HasValue)
            ShowCustomerEquipment(customerCall.CustomerEquipment);
    }


    protected void cboCustomerEquipments_TextChanged(object sender, EventArgs e)
    {
        ///Show Equipment
        if (!String.IsNullOrEmpty(cboCustomerEquipments.Text))
        {
            CustomerEquipment customerEquipment = CustomerManager.GetCustomerEquipment(Convert.ToInt32(cboCustomerEquipments.SelectedValue));
            ShowCustomerEquipment(customerEquipment);
        }
        else
            pnlShowEquipment.Visible = false;
    }


    protected void btnGenerateReceipt_Click(object sender, EventArgs e)
    {
        var serviceOrderId = SaveServiceOrder();
        if (serviceOrderId.HasValue)
            Response.Redirect("../Accounting/Receipt.aspx?ServiceOrderId=" + serviceOrderId.EncryptToHex());
    }

    protected void odsCustomerContracts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Page.ViewState["CustomerId"] != null)
        {
            e.InputParameters["CompanyId"] = Company.CompanyId;
            e.InputParameters["CustomerId"] = Convert.ToInt32(Page.ViewState["CustomerId"]);
        }
    }

    #endregion

    #region Technical
    /*              Alterações em atuação técnica              */
    //protected void imgAddTechnicalTime_Click(object sender, ImageClickEventArgs e)
    //{

    //    AppointmentManager appointmentManager = new AppointmentManager(this);
    //    EmployeeManager employeeManager = new EmployeeManager(this);
    //    bool validDate = false;
    //    DateTime initialDate = DateTime.Now, endDate = DateTime.Now;


    //    //verify if the BeginDate and EndDate are valid 
    //    
    //    validDate = DateTime.TryParse(txtInitialDate.Text, out initialDate);
    //    validDate &= DateTime.TryParse(txtEndDate.Text, out endDate);

    //    if (validDate == false || endDate < initialDate)
    //    {
    //        ShowError(Resources.Exception.InvalidDate);
    //        return;
    //    }

    //    // verify if the employee is available
    //    if (!employeeManager.CheckAvailableEmployee(Company.CompanyId, Convert.ToInt32(cboServiceOrderEmployee.SelectedValue), initialDate, endDate))
    //    {
    //        ShowError(Resources.Exception.unavailableEmployee);
    //        return;
    //    }

    //    //create the appointment
    //    Appointment appointment = new Appointment();
    //    appointment.CompanyId = Company.CompanyId;
    //    appointment.Employee = employeeManager.GetEmployee(Company.CompanyId, Convert.ToInt32(cboServiceOrderEmployee.SelectedValue));
    //    appointment.TaskName = txtTechnicalDescription.Text;
    //    appointment.BeginTime = initialDate;
    //    appointment.EndTime = endDate;
    //    if (fupDescription.HasFile)
    //    {
    //        appointment.FileUrl = Company.GetFilesDirectory() + fupDescription.FileName;
    //        fupDescription.SaveAs(Server.MapPath(appointment.FileUrl));
    //        appointment.TaskName = appointment.TaskName + "<br /> " +
    //            "<a href='" + ResolveUrl(appointment.FileUrl) + fupDescription.FileName + "'>" + fupDescription.FileName + "</a>";
    //    }

    //    AppointmentsList.Add(appointment);
    //    BindLstTechnicalAppointments();
    //}

    //protected void odsTechnicalAppointments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    //{
    //    if (serviceOrderId != null)
    //    {
    //        e.InputParameters["CompanyId"] = Company.CompanyId;
    //        e.InputParameters["ServiceOrderId"] = Convert.ToInt32(serviceOrderId);
    //    }
    //}
    #endregion

    //this region contains all function used on the screen
    #region functions

    /// <summary>
    /// This method validates the addition of product with selected deposit 
    /// </summary>
    /// <param name="product"></param> 
    private bool IsValidProductWithDeposit(Product product)
    {
        if (!String.IsNullOrEmpty(cboDeposit.SelectedValue))
        {
            var productInventory = inventoryManager.GetProductInventory(Company.CompanyId, product.ProductId, Convert.ToInt32(cboDeposit.SelectedValue));

            if (productInventory == null)
            {
                litErrorMessage.Visible = true;
                litErrorMessage.Text = "Produto não existe no estoque selecionado!";
                return false;
            }

            if (productInventory.Quantity < ucCurrFieldQuantity.IntValue)
            {
                litErrorMessage.Visible = true;
                litErrorMessage.Text = "Quantidade do produto selecionado insuficiente! Restam apenas " + productInventory.Quantity + " unidades!";
                return false;
            }
        }
        return true;
    }

    private Int32? SaveServiceOrder()
    {
        ServiceOrder serviceOrder = new ServiceOrder();
        List<ServiceOrderItem> lstServiceOrderItem = new List<ServiceOrderItem>();

        if (IsLoaded)
        {
            serviceOrder.CopyPropertiesFrom(Original_ServiceOrder);
            serviceOrder.ModifiedByUser = User.Identity.UserName;
        }
        else
            serviceOrder.CreatedByUser = User.Identity.UserName;

        serviceOrder.CompanyId = Company.CompanyId;
        serviceOrder.CustomerId = Convert.ToInt32(Page.ViewState["CustomerId"]);

        if (!String.IsNullOrEmpty(cboCustomerEquipments.SelectedValue))
            serviceOrder.CustomerEquipmentId = Convert.ToInt32(cboCustomerEquipments.SelectedValue);

        if (!String.IsNullOrEmpty(cboCustomerCalls.SelectedValue))
            serviceOrder.CustomerCallId = Convert.ToInt32(cboCustomerCalls.SelectedValue);

        if (!String.IsNullOrEmpty(cboCustomerContracts.SelectedValue))
            serviceOrder.ContractId = Convert.ToInt32(cboCustomerContracts.SelectedValue);
        else
            serviceOrder.ContractId = null;

        serviceOrder.DepositId = null;

        if (!String.IsNullOrEmpty(cboDeposit.SelectedValue))
            serviceOrder.DepositId = Convert.ToInt32(cboDeposit.SelectedValue);

        serviceOrder.ServiceOrderNumber = txtServiceOrderNumber.Text;

        if (Page.ViewState["CustomerCallId"] != null)
            serviceOrder.CustomerCallId = Convert.ToInt32(Page.ViewState["CustomerCallId"]);

        serviceOrder.ServiceOrderTypeId = Convert.ToInt32(cboServiceOrderType.SelectedValue);

        if (!String.IsNullOrEmpty(cboServiceOrderStatus.SelectedValue))
            serviceOrder.ServiceOrderStatusId = Convert.ToInt32(cboServiceOrderStatus.SelectedValue);

        //add the ServiceOrderItems in lstServiceOrderItem
        ServiceOrderItem serviceOrderItem;

        foreach (ServiceItem item in ServiceItemsList)
        {
            serviceOrderItem = new ServiceOrderItem();
            serviceOrderItem.CompanyId = item.CompanyId;
            serviceOrderItem.EmployeeId = item.EmployeeId;
            serviceOrderItem.ServiceId = item.ServiceId;
            serviceOrderItem.Description = item.Name;
            serviceOrderItem.Price = item.ServicePrice;
            serviceOrderItem.IsApplied = true;
            serviceOrderItem.ServiceOrderId = serviceOrder.ServiceOrderId;
            lstServiceOrderItem.Add(serviceOrderItem);
        }

        foreach (ProductItem item in ProductItemsList)
        {
            serviceOrderItem = new ServiceOrderItem();
            serviceOrderItem.Description = item.Description;
            serviceOrderItem.IsApplied = item.IsApplied;
            serviceOrderItem.ProductId = item.ProductId;
            serviceOrderItem.CompanyId = item.CompanyId;
            serviceOrderItem.Quantity = item.Quantity;
            serviceOrderItem.ServiceOrderId = serviceOrder.ServiceOrderId;
            lstServiceOrderItem.Add(serviceOrderItem);
        }

        //clear ids
        serviceOrder.ServiceOrderProductDamageId = String.Empty;
        serviceOrder.ServiceOrderEquipmentDamageId = String.Empty;
        serviceOrder.ServiceOrderTestId = String.Empty;

        serviceOrder.ServiceOrderInstallType = String.Empty;
        serviceOrder.ServiceOrderProductType = String.Empty;
        serviceOrder.ServiceOrderHaltType = String.Empty;
        serviceOrder.ServiceType = String.Empty;

        ///save serviceType 
        foreach (ListItem item in chkServiceType.Items)
            if (item.Selected)
                serviceOrder.ServiceType += String.Empty + item.Value + ",";

        ///save InstallType  
        foreach (ListItem item in chkInstallType.Items)
            if (item.Selected)
                serviceOrder.ServiceOrderInstallType += String.Empty + item.Value + ",";

        ///save ProductType  
        foreach (ListItem item in chkProductType.Items)
            if (item.Selected)
                serviceOrder.ServiceOrderProductType += String.Empty + item.Value + ",";

        ///save HaltType  
        foreach (ListItem item in chkHaltType.Items)
            if (item.Selected)
                serviceOrder.ServiceOrderHaltType += String.Empty + item.Value + ",";

        ///save tests  
        foreach (ListItem item in chklstTests.Items)
            if (item.Selected)
                serviceOrder.ServiceOrderTestId += String.Empty + item.Value + ",";

        ///save ProductsDamage
        foreach (ListItem item in chklstEquipmentDamage.Items)
            if (item.Selected)
                serviceOrder.ServiceOrderEquipmentDamageId += String.Empty + item.Value + ",";

        ///save EquipmentsDamage
        foreach (ListItem item in chklstProductDamage.Items)
            if (item.Selected)
                serviceOrder.ServiceOrderProductDamageId += String.Empty + item.Value + ",";

        ///remove a last
        if (!String.IsNullOrEmpty(serviceOrder.ServiceType))
            serviceOrder.ServiceType = serviceOrder.ServiceType.Remove(serviceOrder.ServiceType.Length - 1);

        if (!String.IsNullOrEmpty(serviceOrder.ServiceOrderTestId))
            serviceOrder.ServiceOrderTestId = serviceOrder.ServiceOrderTestId.Remove(serviceOrder.ServiceOrderTestId.Length - 1);

        if (!String.IsNullOrEmpty(serviceOrder.ServiceOrderInstallType))
            serviceOrder.ServiceOrderInstallType = serviceOrder.ServiceOrderInstallType.Remove(serviceOrder.ServiceOrderInstallType.Length - 1);

        if (!String.IsNullOrEmpty(serviceOrder.ServiceOrderProductType))
            serviceOrder.ServiceOrderProductType = serviceOrder.ServiceOrderProductType.Remove(serviceOrder.ServiceOrderProductType.Length - 1);

        if (!String.IsNullOrEmpty(serviceOrder.ServiceOrderHaltType))
            serviceOrder.ServiceOrderHaltType = serviceOrder.ServiceOrderHaltType.Remove(serviceOrder.ServiceOrderHaltType.Length - 1);

        if (!String.IsNullOrEmpty(serviceOrder.ServiceOrderEquipmentDamageId))
            serviceOrder.ServiceOrderEquipmentDamageId = serviceOrder.ServiceOrderEquipmentDamageId.Remove(serviceOrder.ServiceOrderEquipmentDamageId.Length - 1);

        if (!String.IsNullOrEmpty(serviceOrder.ServiceOrderProductDamageId))
            serviceOrder.ServiceOrderProductDamageId = serviceOrder.ServiceOrderProductDamageId.Remove(serviceOrder.ServiceOrderProductDamageId.Length - 1);


        int? depositId = null;

        if (serviceOrder.DepositId.HasValue)
            depositId = serviceOrder.DepositId;

        ///Save the ServiceOrder
        ServicesManager.SaveServiceOrder(Original_ServiceOrder, serviceOrder, lstServiceOrderItem, depositId);

        return serviceOrder.ServiceOrderId;
    }

    #region Services

    /// <summary>
    /// this method meets the grdService
    /// </summary>
    private void BindService()
    {
        grdService.DataSource = ServiceItemsList;
        grdService.DataBind();
    }

    /// <summary>
    /// this method delete a service from dtService
    /// </summary>
    /// <param name="rowIndex"></param>
    private void DeleteService(Int32 rowIndex)
    {
        ServiceItemsList.RemoveAt(rowIndex);
    }

    #endregion

    #region ServiceOrderEquipments

    /// <summary>
    /// this method meets the grdServiceOrderItemProduct
    /// </summary>
    private void BindProducts()
    {
        grdServiceOrderItemProduct.DataSource = ProductItemsList;
        grdServiceOrderItemProduct.DataBind();
    }

    /// <summary>
    /// this method delete a ServiceOrderEquipment
    /// </summary>
    /// <param name="rowIndex"></param>
    private void DeleteServiceOrderEquipment(Int32 rowIndex)
    {
        ProductItemsList.RemoveAt(rowIndex);
    }

    #endregion

    #region Equipments

    /// <summary>
    /// This method shows of customerEquipment informations in screen
    /// </summary>
    /// <param name="customerEquipment"></param>
    private void ShowCustomerEquipment(CustomerEquipment customerEquipment)
    {
        pnlShowEquipment.Visible = true;
        cboCustomerEquipments.SelectedValue = customerEquipment.CustomerEquipmentId.ToString();
        lblEquipmentModel.Text = customerEquipment.Model;
        lblEquipmentManufacturer.Text = customerEquipment.Manufacturer;
        lblSerialNumber.Text = customerEquipment.SerialNumber;
        lblEquipmentDescription.Text = customerEquipment.Description;
        lblEquipmentModelText.Visible = lblEquipmentManufacturerText.Visible = lblSerialNumberText.Visible = lblEquipmentDescriptionText.Visible = true;
    }

    #endregion


    private void SetServiceOrderType(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
            return;
        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chkServiceType.Items.FindByValue(item) != null)
                chkServiceType.Items.FindByValue(item).Selected = true;
    }

    private void SetServiceOrderEquipmentDamage(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
            return;
        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chklstEquipmentDamage.Items.FindByValue(item) != null)
                chklstEquipmentDamage.Items.FindByValue(item).Selected = true;
    }

    private void SetServiceOrderProductDamage(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
            return;
        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chklstProductDamage.Items.FindByValue(item) != null)
                chklstProductDamage.Items.FindByValue(item).Selected = true;
    }

    private void SetServiceTest(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
            return;

        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chklstTests.Items.FindByValue(item) != null)
                chklstTests.Items.FindByValue(item).Selected = true;
    }

    private void SetServiceOrderHaltType(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
        {
            return;
        }
        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chkHaltType.Items.FindByValue(item) != null)
                chkHaltType.Items.FindByValue(item).Selected = true;
    }

    private void SetServiceOrderInstallType(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
        {
            return;
        }
        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chkInstallType.Items.FindByValue(item) != null)
                chkInstallType.Items.FindByValue(item).Selected = true;
    }

    private void SetServiceOrderProductType(String selectedValue)
    {
        if (String.IsNullOrEmpty(selectedValue))
        {
            return;
        }
        String[] selectedValues = selectedValue.Split(',');
        foreach (String item in selectedValues)
            if (chkProductType.Items.FindByValue(item) != null)
                chkProductType.Items.FindByValue(item).Selected = true;
    }

    private void DisableControlsForPopUpView()
    {
        if (grdService != null)
            grdService.Enabled = false;

        Title = String.Empty;
        txtServiceOrderNumber.Enabled = false;
        SelCustomer.Enabled = false;
        cboServiceOrderType.Enabled = false;
        cboServiceOrderStatus.Enabled = false;
        cboCustomerCalls.Enabled = false;
        cboCustomerContracts.Enabled = false;
        cboCustomerEquipments.Enabled = false;
        rowService.Visible = false;
        rowProduct.Visible = false;
        grdServiceOrderItemProduct.Enabled = false;
        chkServiceType.Enabled = false;
        chkProductType.Enabled = false;
        chkInstallType.Enabled = false;
        chkHaltType.Enabled = false;
        chklstEquipmentDamage.Enabled = false;
        chklstProductDamage.Enabled = false;
        chklstTests.Enabled = false;
        rowButtons.Visible = false;
        btnShowAppointments.Visible = false;
    }

    #endregion



    protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

}
