using System;
using System.Collections.Generic;
using System.Web.Security;
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using BudgetStatus = Vivina.Erp.BusinessRules.SaleManager.BudgetStatus;
using Exception = Resources.Exception;
using User = InfoControl.Web.Security.DataEntities.User;

namespace Vivina.Erp.WebUI.Site
{
    public enum ServiceOrderType
    {
        Single = 1,
        Warranty,
        Contract
    }

    public partial class CheckoutIdentification : CheckoutPageBase
    {
        private CustomerManager _customerManager;
        private MembershipManager _membershipManager;
        private Customer customer;
        private Receipt orignalReceipt;
        private Int32 parcelQuantity;
        private ProfileManager profileManager;
        private Decimal total;
        private User user;
        private string nextUrl = "~/site/Checkout_Payment.aspx";

        public Receipt OriginalReceipt
        {
            get
            {
                if (Page.ViewState["ReceiptID"] != null)
                    orignalReceipt = new ReceiptManager(this).GetReceipt(Convert.ToInt32(Page.ViewState["ReceiptID"]),
                                                                         Company.CompanyId);
                return orignalReceipt;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUserEmail.Enabled = (txtUserEmail.Text == String.Empty);
                txtUserEmail.Text = Budget.CustomerMail;

                parcelQuantity = Convert.ToInt32(Session["parcelQuantity"]);
                lblMessage.Text = "<h1> Já Sou Usuário? </h1>";

                if (User.IsAuthenticated)
                {
                    customer = new CustomerManager(this).GetCustomerByUserName(Company.CompanyId, User.Identity.UserName);
                    Response.Redirect(nextUrl + "?CustomerId=" + customer.CustomerId);
                    if (!String.IsNullOrEmpty(Request["b"]))
                        GenerateServiceOrderAndReceipt();
                    return;
                }

                if (!String.IsNullOrEmpty(Request["m"]))
                {
                    txtUserEmail.Text = Request["m"];
                    User user = new MembershipManager(this).GetUserByEmail(Request["m"]);

                    if (user != null)
                    {
                        lblMessage.Text = "<h1> Controle de Acesso </h1>";
                        pnlRegister.Visible = false;
                        return;
                    }
                    pnlLogin.Visible = false;
                    lblMessage.Text = String.Empty;
                }
            }



            //if (ucProfiles.CompanyProfileEntity != null)
            //{
            //    ucProfiles.CompanyProfileEntity.Address.PostalCode = DeliveryAddress.PostalCode;
            //    ucProfiles.CompanyProfileEntity.AddressComp = DeliveryAddress.AddressComp;
            //    ucProfiles.CompanyProfileEntity.AddressNumber = DeliveryAddress.AddressNumber;
            //}

            //if (ucProfiles.ProfileEntity != null)
            //{
            //    ucProfiles.ProfileEntity.Address.PostalCode = DeliveryAddress.PostalCode;
            //    ucProfiles.ProfileEntity.AddressComp = DeliveryAddress.AddressComp;
            //    ucProfiles.ProfileEntity.AddressNumber = DeliveryAddress.AddressNumber;
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveUser();
            SaveCustomer();
            //
            // verify if the state of register
            //
            switch (_customerManager.Insert(customer, user))
            {
                case MembershipCreateStatus.DuplicateEmail:
                    lblMessage.Text = Exception.ExistentMail;
                    return;

                case MembershipCreateStatus.DuplicateUserName:
                    lblMessage.Text = Exception.ExistUser;
                    return;

                case MembershipCreateStatus.InvalidPassword:
                    lblMessage.Text = Exception.InvalidUserPassword;
                    return;

                case MembershipCreateStatus.Success:
                    nextUrl += "?CustomerId=" + customer.CustomerId;
                    if (!String.IsNullOrEmpty(Request["b"]))
                        GenerateServiceOrderAndReceipt();

                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    Response.Redirect(nextUrl);
                    break;
            }

            // customer = _customerManager.GetCustomerByUserName(Company.CompanyId, txtUserEmail.Text);


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/site/Products.aspx");
        }

        protected void Login_LoggedIn(object sender, EventArgs e)
        {
            using (var manager = new CustomerManager(null))
                customer = customer ?? manager.GetCustomerByUserName(Company.CompanyId, Login.UserName);

            if (customer == null)
                throw new ArgumentNullException("O usuário informado não corresponde a um cliente da empresa!");

            if (!String.IsNullOrEmpty(Request["b"]))
                GenerateServiceOrderAndReceipt();

            var login = (sender as System.Web.UI.WebControls.Login);
            login.DestinationPageUrl = (nextUrl + "?b=" + Request["b"] + "&CustomerId=" + customer.CustomerId);
        }

        private void GenerateServiceOrderAndReceipt()
        {
            int budgetId = Convert.ToInt32(Request["b"]);
            new SaleManager(this).SetBudgetStatus(Company.CompanyId, budgetId, (int)BudgetStatus.Accepted);

            int serviceOrderId = CreateServiceOrderFromBudget(budgetId, customer.CustomerId);
            CreateReceiptFromServiceOrder(serviceOrderId);
        }

        #region Functions

        private void SaveCustomer()
        {
            customer = new Customer();
            _customerManager = new CustomerManager(this);
            profileManager = new ProfileManager(this);

            if (ucProfiles.ProfileEntity != null)
            {
                customer.ProfileId = ucProfiles.ProfileEntity.ProfileId;
                if (ucProfiles.ProfileEntity.ProfileId == 0)
                    customer.Profile = ucProfiles.ProfileEntity;

                customer.CreditLimit = ucProfiles.ProfileEntity.CreditLimit;
                customer.Observation = ucProfiles.ProfileEntity.Observations;
            }
            else if (ucProfiles.CompanyProfileEntity != null)
            {
                customer.LegalEntityProfileId = ucProfiles.CompanyProfileEntity.LegalEntityProfileId;
                if (ucProfiles.CompanyProfileEntity.LegalEntityProfileId == 0)
                    customer.LegalEntityProfile = ucProfiles.CompanyProfileEntity;


            }

            customer.CompanyId = Company.CompanyId;
            customer.BlockSalesInDebit = true;

            customer = _customerManager.CheckExistCustomer(customer);
            //_customerManager.Insert(customer);

            lblMessage.Visible = true;
        }

        private void SaveUser()
        {
            user = new User();
            _membershipManager = new MembershipManager(this);

            user.UserName = txtUserEmail.Text;
            user.Password = txtPassword.Text;
            user.PasswordAnswer = txtPassword.Text;
            user.Email = txtUserEmail.Text;

            user.LastLoginDate = DateTime.Now;
            user.LastPasswordChangedDate = DateTime.Now;
            user.LastLockoutDate = DateTime.Now;
            user.LastActivityDate = DateTime.Now;
            user.CreationDate = DateTime.Now;

            user.HasChangePassword = false;
            user.IsOnline = false;
            user.IsActive = false;
            user.IsLockedOut = true;

            user.FailedPasswordAttemptCount = 3;
        }

        private Int32 CreateServiceOrderFromBudget(Int32 budgetID, Int32 customerID)
        {
            //
            // Retrieve budget existent 
            //
            var budgetManager = new SaleManager(this);
            Budget budget = budgetManager.GetBudget(budgetID, Company.CompanyId);
            budget.CustomerId = customerID;

            //
            // Criar uma ordem de serviço
            //
            var servicesManager = new ServicesManager(this);
            var serviceOrder = new ServiceOrder();

            serviceOrder.BudgetId = budgetID;
            serviceOrder.CompanyId = Company.CompanyId;
            serviceOrder.CustomerId = customerID;
            serviceOrder.ServiceOrderTypeId = Convert.ToInt32(ServiceOrderType.Single);
            serviceOrder.ServiceOrderNumber = "OS" + Util.GenerateUniqueID();
            servicesManager.InsertServiceOrder(serviceOrder);

            //
            // Passagem de itens de uma proposta para uma ordem de serviço
            //
            IList<ServiceOrderItem> serviceOrderItemList = new List<ServiceOrderItem>();
            ServiceOrderItem serviceOrderItem;
            foreach (BudgetItem item in budget.BudgetItems)
            {
                serviceOrderItem = new ServiceOrderItem();
                serviceOrderItem.ServiceOrderId = serviceOrder.ServiceOrderId;
                serviceOrderItem.CompanyId = Company.CompanyId;
                serviceOrderItem.Description = item.SpecialProductName;

                if (item.ProductId.HasValue)
                {
                    serviceOrderItem.ProductId = item.ProductId;
                    serviceOrderItem.Description = item.Product.Description;
                }
                else if (item.ServiceId.HasValue)
                {
                    serviceOrderItem.ServiceId = item.ServiceId;
                    serviceOrderItem.Description = item.Service.Name;
                }

                serviceOrderItemList.Add(serviceOrderItem);
            }
            servicesManager.InsertServiceOrderItems(serviceOrderItemList);

            return serviceOrder.ServiceOrderId;
        }

        /// <summary>
        /// Create an receipt from serviceOrder
        /// </summary>
        /// <param name="serviceOrderID"></param>
        private void CreateReceiptFromServiceOrder(Int32 serviceOrderID)
        {
            var receiptManager = new ReceiptManager(this);
            var receipt = new Receipt();
            ServiceOrder serviceOrder = new ServicesManager(this).GetServiceOrder(serviceOrderID);

            if (OriginalReceipt != null)
                receipt.CopyPropertiesFrom(OriginalReceipt);

            receipt.CompanyId = Company.CompanyId;

            IList<ReceiptItem> receiptListItem = new List<ReceiptItem>();
            ReceiptItem receiptItem;
            foreach (BudgetItem item in serviceOrder.Budget.BudgetItems)
            {
                receiptItem = new ReceiptItem();
                receiptItem.Description = item.Service != null ? item.Service.Name : item.Product.Name;
                receiptItem.ServiceId = item.ServiceId;
                receiptItem.ProductId = item.ProductId;
                receiptItem.Quantity = item.Quantity;
                receiptItem.CompanyId = serviceOrder.CompanyId;
                receiptItem.FiscalClass = item.Product != null
                                              ? item.Product.FiscalClass
                                              : String.Empty;
            }
        }

        #endregion
    }
}
