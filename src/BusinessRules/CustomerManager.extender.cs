using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Web.Security;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules.Comments;
using Vivina.Erp.DataClasses;
using User=InfoControl.Web.Security.DataEntities.User;

namespace Vivina.Erp.BusinessRules
{
    //this region contains all enums used in Customer

    #region enums

    public enum CustomerStatus
    {
        DuplicatedCPF,
        DuplicatedCNPJ,
        Success
    }

    //public enum CustomerCallStatus
    //{
    //    Open,
    //    Close,
    //    Pendent
    //}

    //public enum CustomerCallStatus
    //{
    //    All = 1, Open, Closed
    //}

    //public enum ContractPeriods
    //{

    //}

    #endregion

    public partial class CustomerManager
    {
        #region Contact

        public void AddContact(Contact entity, Int32 companyId, Int32 customerId)
        {
            var contactManager = new ContactManager(this);
            entity.CompanyId = companyId;
            contactManager.Insert(entity);

            var ccManager = new CustomerContactManager(this);
            var cc = new CustomerContact();
            cc.CustomerId = customerId;
            cc.ContactId = entity.ContactId;
            cc.CompanyId = companyId;
            ccManager.Insert(cc);
        }

        public void RemoveContact(Contact entity, Int32 customerId, Int32 companyId)
        {
            var ccManager = new CustomerContactManager(this);
            var cc = new CustomerContact();
            cc.ContactId = entity.ContactId;
            cc.CustomerId = customerId;
            ccManager.Delete(cc);

            var contactManager = new ContactManager(this);
            contactManager.Delete(entity);
        }

        public void UpdateContact(int contactId, Contact entity)
        {
            var contactManager = new ContactManager(this);
            var originalContact = new Contact();
            originalContact = contactManager.GetContact(contactId);
            contactManager.Update(originalContact, entity);
        }

        public Contact GetCustomerContact(int contactId)
        {
            return new ContactManager(this).GetContact(contactId);
        }

        /// <summary>
        /// this method return all contacts of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataTable GetCustomerContactsByCustomer(Int32 customerId)
        {
            var query = from customerContact in DbContext.CustomerContacts
                        where customerContact.CustomerId == customerId
                        select new
                                   {
                                       customerContact.ContactId,
                                       customerContact.Contact.Name
                                   };
            return query.ToDataTable();
        }

        /// <summary>
        /// this method return the Name and Phone of CustomerContacts
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataTable GetContactNameAndPhoneByCustomer(Int32 customerId)
        {
            var query = from customerContact in DbContext.CustomerContacts
                        where customerContact.CustomerId == customerId
                        select new
                                   {
                                       customerContact.ContactId,
                                       NameAndPhone = customerContact.Contact.Name + "-" + customerContact.Contact.Phone,
                                   };
            return query.ToDataTable();
        }

        #endregion

        #region Equipment

        /// <summary>
        /// return all serviceOrders by Equipment
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public DataTable GetServiceOrdersByCustomerEquipment(Int32 customerEquipmentId, string sortExpression,
                                                             int startRowIndex, int maximumRows)
        {
            var query = from serviceOrder in DbContext.ServiceOrders
                        where serviceOrder.CustomerEquipmentId == customerEquipmentId
                        select new
                                   {
                                       serviceOrder.ServiceOrderId,
                                       serviceOrder.ServiceOrderNumber,
                                       ServiceOrderStatus = serviceOrder.ServiceOrderStatus.Name,
                                       ServiceOrderType = serviceOrder.ServiceOrderType.Name
                                   };
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ServiceOrderId").ToDataTable();
        }

        public Int32 GetServiceOrdersByCustomerEquipmentCount(Int32 customerEquipmentId)
        {
            return DbContext.ServiceOrders.Where(s => s.CustomerEquipmentId == customerEquipmentId).Count();
        }

        /// <summary>
        /// This method returns all equipments by customer.
        /// </summary>
        /// <param name=companyId>companyId</param>
        /// <param name=customerId>customerId</param>  
        public IQueryable<CustomerEquipment> GetCustomerEquipments(int companyId, int customerId, string sortExpression,
                                                                   int startRowIndex, int maximumRows)
        {
            return
                DbContext.CustomerEquipments.Where(x => x.CompanyId == companyId && x.CustomerId == customerId).
                    SortAndPage(sortExpression, startRowIndex, maximumRows, "CompanyId");
        }

        /// <summary>
        /// This method returns all equipments by customer.
        /// </summary>
        /// <param name=companyId>companyId</param>
        /// <param name=customerId>customerId</param>  
        public IQueryable<CustomerEquipment> GetCustomerEquipments(int companyId, int customerId)
        {
            return DbContext.CustomerEquipments.Where(x => x.CompanyId == companyId && x.CustomerId == customerId);
        }

        /// <summary>
        /// this method return the CustomerEquipment
        /// </summary>
        /// <param name="customerEquipmentId"></param>
        /// <returns></returns>
        public CustomerEquipment GetCustomerEquipment(Int32 customerEquipmentId)
        {
            return
                DbContext.CustomerEquipments.Where(e => e.CustomerEquipmentId == customerEquipmentId).FirstOrDefault();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void InsertCustomerEquipment(CustomerEquipment entity)
        {
            DbContext.CustomerEquipments.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete one CustomerEquipment.
        /// </summary>
        /// <param name=entity>entity</param>
        public void DeleteCustomerEquipment(CustomerEquipment entity)
        {
            DbContext.CustomerEquipments.Attach(entity);
            DbContext.CustomerEquipments.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method update one CustomerEquipment.
        /// </summary>
        /// <param name=entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void UpdateCustomerEquipment(CustomerEquipment original_entity, CustomerEquipment entity)
        {
            DbContext.CustomerEquipments.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns one CustomerEquipment.
        /// </summary>
        /// <param name=customerEquipmentID>customerEquipmentID</param>
        public CustomerEquipment GetWarrantyTypes(int customerEquipmentID)
        {
            return
                DbContext.CustomerEquipments.Where(x => x.CustomerEquipmentId == customerEquipmentID).FirstOrDefault();
        }

        #endregion

        #region Customer

        public Customer GetHostCustomerByUserName(String name)
        {
            throw new Exception("Essa função não deve existir mais!");
            //var companyManager = new CompanyManager(this); 
            //Company company = companyManager.GetCurrentAdminCompany(name);
            //if (company != null)
            //    return GetCustomerByLegalEntityProfile(companyManager.GetHostCompany().CompanyId,
            //                                           company.LegalEntityProfileId);
            //return null;
        }

        /// <summary>
        /// This method returns a customer by cpf
        /// </summary>
        /// <param name="companyId">Can't be null</param>
        /// <param name="cpf">Can't be null</param>
        /// <returns>a FirstOrDefault customer that contains the parameter cpf and companyId</returns>
        private Customer GetCustomerByCPF(Int32 companyId, String cpf)
        {
            IQueryable<Customer> query = from customer in DbContext.Customers
                                         join profile in DbContext.Profiles on customer.ProfileId equals
                                             profile.ProfileId
                                         where customer.CompanyId.Equals(companyId) && profile.CPF.Equals(cpf)
                                         select customer;
            return query.FirstOrDefault();
        }

        /// <summary>
        ///  This method returns a customer by CNPJ
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="CNPJ"></param>
        /// <returns>a FirstOrDefault customer that contains the parameter CNPJ and companyID</returns>
        private Customer GetCustomerByCNPJ(Int32 companyID, String CNPJ)
        {
            IQueryable<Customer> query = from customer in DbContext.Customers
                                         join legalProfile in DbContext.LegalEntityProfiles on
                                             customer.LegalEntityProfileId equals legalProfile.LegalEntityProfileId
                                         where customer.CompanyId.Equals(companyID) && legalProfile.CNPJ.Equals(CNPJ)
                                         select customer;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// This method returns true if the customer already exists
        /// </summary>
        /// <param name="customer">Can't be null</param>
        /// <returns>a boolean that indicates if the customer exists</returns>
        public Customer CheckExistCustomer(Customer customer)
        {
            if (customer.LegalEntityProfileId.HasValue)
                customer = GetCustomerByLegalEntityProfile(customer.CompanyId, customer.LegalEntityProfileId.Value) ??
                           customer;
            else if (customer.ProfileId.HasValue)
                customer = GetCustomerByProfile(customer.CompanyId, customer.ProfileId.Value) ?? customer;

            return (customer);
        }

        public bool ExistCustomer(Customer customer)
        {
            return CheckExistCustomer(customer).CustomerId != 0;
        }

        public void Insert(Customer entity)
        {
            /*
             * if this customer is already company's customer don't register him
             * To validate the best method is search customers by cpf or cnpj
            */
            if (entity != null && ExistCustomer(entity))
                return;

            if (entity != null)
            {
                entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
                DbContext.Customers.InsertOnSubmit(entity);
                DbContext.SubmitChanges();
            }
        }

        /// <summary>
        /// This method insert an customer and an user atached
        /// this method return true when customer and user inserted
        /// </summary>
        public MembershipCreateStatus Insert(Customer customer, User user)
        {
            MembershipCreateStatus status;
            var companyManager = new CompanyManager(this);
            var membershipManager = new MembershipManager(this);

            membershipManager.Insert(user, out status, true);
            //
            // creates an new user
            //
            if (status == MembershipCreateStatus.Success)
            {
                customer.UserId = user.UserId;
                customer.CreatedDate = customer.ModifiedDate = DateTime.Now;
                Insert(customer);
                return MembershipCreateStatus.Success;
            }

            return status;
        }

        public void Update(Customer original_entity, Customer entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method search customer using customer object
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IQueryable SearchCustomers(Hashtable htCustomer, string sortExpression, int startRowIndex,
                                          int maximumRows)
        {
            ///retrieving customer by Company
            IQueryable<Customer> queryCustomer = from customer in DbContext.Customers
                                                 where customer.CompanyId == Convert.ToInt32(htCustomer["CompanyId"])
                                                 select customer;

            ///retrieving all contacts related with customer
            IQueryable<CustomerContact> queryCustomerContact = from customer in queryCustomer
                                                               join customerContacts in DbContext.CustomerContacts on
                                                                   customer.CustomerId equals
                                                                   customerContacts.CustomerId into gCustomerContacts
                                                               from customerContacts in
                                                                   gCustomerContacts.DefaultIfEmpty()
                                                               select customerContacts;

            if (!String.IsNullOrEmpty(htCustomer["Contact"] as String))
            {
                queryCustomerContact =
                    queryCustomerContact.Where(cc => cc.Contact.Name.Contains(htCustomer["Contact"].ToString())).
                        AsQueryable();

                ///join customerContact whith customer
                queryCustomer = from customer in queryCustomer
                                join customerContact in queryCustomerContact on customer.CustomerId equals
                                    customerContact.CustomerId
                                select customer;

                /// after join, retrieving customer using distinc Method
                queryCustomer = queryCustomer.Distinct();

                ///Distinct method is used to discriminate the customers, because the relationship of customer and
                ///contact return customers with only one distinc property(contact)
            }

            var query = from customers in queryCustomer
                        join profiles in DbContext.Profiles on customers.ProfileId equals profiles.ProfileId into
                            gProfiles
                        from profile in gProfiles.DefaultIfEmpty()
                        join legalEntityProfiles in DbContext.LegalEntityProfiles on customers.LegalEntityProfileId
                            equals legalEntityProfiles.LegalEntityProfileId into gLegalEntityProfiles
                        from legalEntityProfile in gLegalEntityProfiles.DefaultIfEmpty()
                        select new
                                   {
                                       customers.CompanyId,
                                       customers.CustomerId,
                                       customers.BlockSalesInDebit,
                                       customers.CustomerTypeId,
                                       customers.ProfileId,
                                       customers.LegalEntityProfileId,
                                       customers.ModifiedDate,
                                       customers.SalesPersonId,
                                       customers.SalesPersonCommission,
                                       customers.SupplementalSalesPersonId,
                                       customers.SupplementalSalesPersonCommission,
                                       customers.UserId,
                                       customers.BankId,
                                       customers.Agency,
                                       customers.AccountNumber,
                                       customers.Ranking,
                                       customers.Observation,
                                       customers.RepresentantId,
                                       ProfileName = profile.Name,
                                       legalEntityProfile.CompanyName,
                                       legalEntityProfile.FantasyName,
                                       Name = legalEntityProfile.CompanyName ?? profile.Name,
                                       RG = profile != null
                                                ? profile.RG
                                                : String.Empty,
                                       VotingTitle = profile != null
                                                         ? profile.VotingTitle
                                                         : String.Empty,
                                       Identification = profile.CPF ?? legalEntityProfile.CNPJ,
                                       Email = profile.Email ?? legalEntityProfile.Email,
                                       Phone = profile.Phone ?? legalEntityProfile.Phone,
                                   };

            //if (!String.IsNullOrEmpty(htCustomer["CustomerId"] as String))
            //    query = query.Where(c => c.CustomerId == Convert.ToInt32(htCustomer["CustomerId"]));

            if (!String.IsNullOrEmpty(htCustomer["SelectedCustomer"] as String))
                query = query.Where(c => c.Name.Contains(htCustomer["SelectedCustomer"].ToString()));

            if (htCustomer["CPF"].ToString() != "___.___.___-__")
                query = query.Where(c => c.Identification.Contains(htCustomer["CPF"].ToString()));
            else if (htCustomer["CNPJ"].ToString() != "__.___.___/____-__")
                query = query.Where(c => c.Identification.Contains(htCustomer["CNPJ"].ToString()));

            if (!String.IsNullOrEmpty(htCustomer["Email"] as String))
                query = query.Where(c => c.Email.Contains(htCustomer["Email"].ToString()));

            if (!String.IsNullOrEmpty(htCustomer["votingTitle"] as String))
                query = query.Where(c => c.VotingTitle.Contains(htCustomer["votingTitle"].ToString()));

            if (!String.IsNullOrEmpty(htCustomer["RG"] as String))
                query = query.Where(c => c.RG.Contains(htCustomer["RG"].ToString()));

            if (htCustomer["Phone"].ToString() != "(__)____-____")
                query = query.Where(c => c.Phone.Contains(htCustomer["Phone"].ToString()));

            if (!String.IsNullOrEmpty(htCustomer["CustomerTypeId"] as String))
                query = query.Where(c => c.CustomerTypeId == Convert.ToInt32(htCustomer["CustomerTypeId"]));

            if (!String.IsNullOrEmpty(htCustomer["representantId"].ToString()))
                query = query.Where(c => c.RepresentantId == Convert.ToInt32(htCustomer["representantId"]));

            //
            //Filter results by Ranking
            //
            if (Convert.ToInt32(htCustomer["Ranking"]) > 0)
                query = query.Where(c => c.Ranking.Value >= Convert.ToInt32(htCustomer["Ranking"]));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "CustomerId");
        }

        /// <summary>
        /// This method count number of registers returned by SearchCustomers
        /// </summary>
        /// <param name="htCustomer"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 SearchCustomersCount(Hashtable htCustomer, string sortExpression, int startRowIndex,
                                          int maximumRows)
        {
            return SearchCustomers(htCustomer, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// Method to fill the AutoCompleteTextBox of Customers
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Recognizable> SearchCustomers(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            if (DataManager.CacheCommands[methodName] == null)
            {
                Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>> query = CompiledQuery.Compile
                    <InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                    (ctx, _companyId, _name, _maximumRows) =>
                    (
                        from customer in DbContext.Customers
                        join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into gprofile
                        from profile in gprofile.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        where customer.CompanyId == _companyId &&
                              (
                                  profile.Name.Contains(_name) ||
                                  profile.CPF.Contains(_name) ||
                                  legalEntityProfile.CompanyName.Contains(_name) ||
                                  legalEntityProfile.FantasyName.Contains(_name) ||
                                  legalEntityProfile.CNPJ.Contains(_name)
                              )
                        select new Recognizable(customer.CustomerId.EncryptToHex(),
                                                (profile.CPF ?? legalEntityProfile.CNPJ) + " | " +
                                                (profile.Name ??
                                                 legalEntityProfile.FantasyName ?? legalEntityProfile.CompanyName))
                    ).Take(_maximumRows));

                DataManager.CacheCommands[methodName] = query;
            }

            var method =
                (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)
                DataManager.CacheCommands[methodName];
            return method(DbContext, companyId, name, maximumRows);
        }

        public Customer GetCustomerByUserName(int companyId, string userName)
        {
            return GetCustomerByCompany(companyId).Where(c => c.User.UserName == userName).FirstOrDefault();
        }

        public Customer GetCustomerByName(string name, int companyId)
        {
            return
                GetCustomerByCompany(companyId).Where(
                    cust => cust.Profile.Name.Contains(name) || cust.LegalEntityProfile.CompanyName.Contains(name)).
                    FirstOrDefault();
        }

        public IQueryable GetCustomersBirthDay(Int32 companyId, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            var query = from customer in DbContext.Customers
                        join profileCustomer in DbContext.Profiles
                            on customer.ProfileId equals profileCustomer.ProfileId
                        where
                            (customer.CompanyId == companyId &&
                             customer.Profile.BirthDate.HasValue &&
                             (customer.Profile.BirthDate.Value.DayOfYear >= DateTime.Now.DayOfYear - 7 ||
                              customer.Profile.BirthDate.Value.DayOfYear <= DateTime.Now.DayOfYear + 7))
                        select new
                                   {
                                       customer.CustomerId,
                                       customer.Profile.Name,
                                       customer.Profile.BirthDate,
                                       customer.Profile.Email,
                                       customer.CompanyId,
                                       customer.Profile.Phone
                                   };

            return query;
        }

        public Int32 GetCustomersBirthDayCount(Int32 companyId, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            return
                GetCustomersBirthDay(companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns a customer by customer LegalEntityProfileId
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public Customer GetHostCustomerByLegalEntityProfileId(Int32 legalEntityProfileId)
        {
            var companyManager = new CompanyManager(this);
            Company hostCompany = companyManager.GetHostCompany();
            return GetCustomerByLegalEntityProfile(hostCompany.CompanyId, legalEntityProfileId);
        }

        #region Custumer-User

        /// <summary>
        /// This method just change the username of user attached with customer
        /// </summary>
        /// <param name="originalCutomer"></param>
        /// <param name="newUserName"></param>
        /// <returns></returns>
        public void UpdateUserNameOfCustomer(Customer originalCutomer, String newUserName)
        {
            originalCutomer.User.UserName = newUserName;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method creates an user for an existing customer 
        /// </summary>
        public MembershipCreateStatus InsertUserForCustomer(Customer originalCustomer, User user)
        {
            var membershipManager = new MembershipManager(this);


            MembershipCreateStatus status;

            membershipManager.Insert(user, out status, true);

            if (status == MembershipCreateStatus.Success)
            {
                originalCustomer.UserId = user.UserId;
                DbContext.SubmitChanges();

                return MembershipCreateStatus.Success;
            }

            return status;
        }

        #endregion

        ///// <summary>
        ///// this method delete all associations of customer and also delete the customer  
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <param name="customerId"></param>
        //public void DeleteAllAssociations(Int32 companyId, Int32 customerId)
        //{ 

        //    DbContext.Customers.

        //}

        #endregion

        #region CustomerCall

        #region CustomerCallByType

        public IQueryable GetAllSugestionCustomerCalls(Int32 CustomerCallStatusId)
        {
            return GetCustomerCalls(null, null, CustomerCallStatus.Closed, CustomerCallType.SUGESTION, null,
                                    new DateTimeInterval(DateTime.MinValue.Sql2005MinValue().Date, DateTime.MaxValue),
                                    "", 0, int.MaxValue);
        }

        /// <summary>
        /// this method return customerCalls
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable GetCustomerCalls(Int32? companyId, Int32? customerId, Int32? customerCallStatusId,
                                           Int32? customerCallType, Int32? technicalEmployeeId,
                                           DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            var queryCustomerCall = from customerCall in DbContext.CustomerCalls
                                    join employee in DbContext.Employees on customerCall.TechnicalEmployeeId equals
                                        employee.EmployeeId into gTechnicalEmployees
                                    from technicalEmployee in gTechnicalEmployees.DefaultIfEmpty()
                                    join postUsers in DbContext.Users on customerCall.UserId equals postUsers.UserId
                                        into gPostUsers
                                    from postUser in gPostUsers.DefaultIfEmpty()
                                    join postProfiles in DbContext.Profiles on postUser.ProfileId equals
                                        postProfiles.ProfileId into gPostProfiles
                                    from postProfile in gPostProfiles.DefaultIfEmpty()
                                    join technicalProfiles in DbContext.Profiles on technicalEmployee.ProfileId equals
                                        technicalProfiles.ProfileId into gTechnicalProfiles
                                    from technicalProfile in gTechnicalProfiles.DefaultIfEmpty()
                                    join customerCallStatus in DbContext.CustomerCallStatus on
                                        customerCall.CustomerCallStatusId equals customerCallStatus.CustomerCallStatusId
                                        into gCustomerCallStatus
                                    from customerCallStatus in gCustomerCallStatus.DefaultIfEmpty()
                                    where
                                        ((customerCall.OpenedDate >= dateTimeInterval.BeginDate) &&
                                         (customerCall.OpenedDate <= dateTimeInterval.EndDate))
                                    select new
                                               {
                                                   customerCall.CompanyId,
                                                   customerCall.CustomerCallId,
                                                   customerCall.ModifiedDate,
                                                   customerCall.CustomerId,
                                                   customerCall.CallNumber,
                                                   customerCall.CallNumberAssociated,
                                                   customerCall.Sector,
                                                   customerCall.OpenedDate,
                                                   customerCall.ClosedDate,
                                                   customerCall.CustomerEquipmentId,
                                                   customerCall.Description,
                                                   customerCall.CustomerCallTypeId,
                                                   customerCall.Subject,
                                                   customerCall.UserId,
                                                   customerCall.TechnicalEmployeeId,
                                                   customerCall.Source,
                                                   customerCall.Rating,
                                                   CustomerName =
                                        customerCall.Customer.Profile != null
                                            ? customerCall.Customer.Profile.Name
                                            : customerCall.Customer.LegalEntityProfile.CompanyName,
                                                   CustomerCallStatusId = customerCall.CustomerCallStatusId ?? 0,
                                                   technicalProfile,
                                                   customerCallStatusName = customerCallStatus.Name,
                                                   UserName = postProfile.Name,
                                                   UserNameEmail = postProfile.Email,
                                                   userNamePhone = postProfile.Phone
                                               };

            if (companyId.HasValue)
                queryCustomerCall = queryCustomerCall.Where(cc => cc.CompanyId == companyId);

            if (customerCallStatusId.HasValue)
            {
                if (customerCallStatusId == CustomerCallStatus.Open)
                    queryCustomerCall =
                        queryCustomerCall.Where(
                            cc =>
                            cc.CustomerCallStatusId == CustomerCallStatus.New ||
                            cc.CustomerCallStatusId == CustomerCallStatus.Waiting);
                else
                    queryCustomerCall = queryCustomerCall.Where(cc => cc.CustomerCallStatusId == customerCallStatusId);
            }

            if (customerCallType.HasValue)
                queryCustomerCall = queryCustomerCall.Where(cc => cc.CustomerCallTypeId == customerCallType);

            if (technicalEmployeeId.HasValue)
                queryCustomerCall = queryCustomerCall.Where(cc => cc.TechnicalEmployeeId == (Int32) technicalEmployeeId);

            if (customerId.HasValue)
                queryCustomerCall = queryCustomerCall.Where(cc => cc.CustomerId == (Int32) customerId);

            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "OpenedDate DESC";

            return
                queryCustomerCall.SortAndPage(sortExpression, startRowIndex, maximumRows, "OpenedDate").
                    OrderByDescending(x => x.Rating);
        }

        /// <summary>
        /// this method returns customer's calls by customer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="customerCallType"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetCustomerCallsByCustomer(Int32 companyId, Int32 customerId, Int32? customerCallStatusId,
                                                     Int32? customerCallType, Int32? technicalEmployeeId,
                                                     DateTimeInterval dateTimeInterval, string sortExpression,
                                                     int startRowIndex, int maximumRows)
        {
            return GetCustomerCalls(companyId, customerId, customerCallStatusId, customerCallType, technicalEmployeeId,
                                    dateTimeInterval, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// This method returns customerCalls by customer
        /// </summary>
        /// <returns></returns>
        public IQueryable<CustomerCall> GetCustomerCallsByCustomer(Int32 customerId)
        {
            return DbContext.CustomerCalls.Where(customerCall => customerCall.CustomerId == customerId);
        }


        /// <summary>
        /// this is hr GetCustomerCallsByCustomer's count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="customerCallType"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetCustomerCallsByCustomerCount(Int32 companyId, Int32 customerId, Int32? customerCallStatusId,
                                                     Int32? customerCallType, Int32? technicalEmployeeId,
                                                     DateTimeInterval dateTimeInterval, string sortExpression,
                                                     int startRowIndex, int maximumRows)
        {
            return
                GetCustomerCallsByCustomer(companyId, customerId, customerCallStatusId, customerCallType,
                                           technicalEmployeeId, dateTimeInterval, sortExpression, startRowIndex,
                                           maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns customercalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param> 
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetCustomerCalls(Int32 companyId, Int32? customerCallStatusId, Int32? technicalEmployeeId,
                                           DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            return GetCustomerCalls(companyId, null, customerCallStatusId, null, technicalEmployeeId, dateTimeInterval,
                                    sortExpression, startRowIndex, maximumRows);
        }


        /// <summary>
        /// this is the GetCustomerCalls' count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetCustomerCallsCount(Int32 companyId, Int32? customerCallStatusId, Int32? technicalEmployeeId,
                                           DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            return
                GetCustomerCalls(companyId, customerCallStatusId, technicalEmployeeId, dateTimeInterval, sortExpression,
                                 startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        public Int32 GetCustomerCallsCount(Int32? companyId, Int32? customerId, Int32? customerCallStatusId,
                                           Int32? customerCallType, Int32? technicalEmployeeId,
                                           DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            return
                GetCustomerCalls(companyId, customerCallStatusId, customerCallStatusId, customerCallType,
                                 technicalEmployeeId, dateTimeInterval, sortExpression, startRowIndex, maximumRows).Cast
                    <object>().Count();
        }


        /// <summary>
        /// This method returns the count of customerCalls by specified company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetCustomerCallsCount(Int32 companyId)
        {
            return DbContext.CustomerCalls.Where(customerCall => customerCall.CompanyId == companyId).Count();
        }


        /// <summary>
        /// This method retrieves a customerCall by their companyId and callNumber 
        /// </summary>
        /// <param name="callNumber"></param>
        /// <returns></returns>
        public CustomerCall GetCustomerCall(Int32 companyId, String callNumber)
        {
            return
                DbContext.CustomerCalls.Where(
                    customerCall => customerCall.CallNumber.Equals(callNumber) && customerCall.CompanyId == companyId).
                    FirstOrDefault();
        }


        public IQueryable<CustomerCall> GetAllCustomerCalls()
        {
            return DbContext.CustomerCalls;
        }


        ///// <summary>
        ///// This method returns the open customerCalls from a specified company
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <returns></returns>
        //public IQueryable<object> GetOpenCustomerCalls(Int32 companyId)
        //{

        //    var query = from customerCall in GetAllCustomerCalls()
        //                where customerCall.CompanyId == companyId && customerCall.CustomerCallStatusId != CustomerCallStatus.Closed
        //                join employee in DbContext.Employees on customerCall.TechnicalEmployeeId equals employee.EmployeeId into gEmployees
        //                from employee in gEmployees.DefaultIfEmpty()
        //                select new
        //                {
        //                    CallNumber = customerCall.CallNumber,
        //                    CustomerCallId = customerCall.CustomerCallId,
        //                    CustomerCallStatus = customerCall.CustomerCallStatus.Name,
        //                    CustomerName = customerCall.Customer.Profile != null ? customerCall.Customer.Profile.Name : customerCall.Customer.LegalEntityProfile.CompanyName,
        //                    OpenedDate = customerCall.OpenedDate,
        //                    TechnicalEmployee = employee.EmployeeId != 0 ? employee.Profile.Name : String.Empty,
        //                    ServiceOrderNumber = customerCall.ServiceOrders.Any() ? customerCall.ServiceOrders.First().ServiceOrderNumber : String.Empty,
        //                    ServiceOrderId = customerCall.ServiceOrders.Any() ? customerCall.ServiceOrders.First().ServiceOrderId : 0
        //                };
        //    return query;
        //}


        /// <summary>
        /// This method returns the open customerCalls from a specified company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetOpenCustomerCalls(Int32 companyId, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            var query = from customerCall in GetAllCustomerCalls()
                        where
                            customerCall.CompanyId == companyId &&
                            customerCall.CustomerCallStatusId != CustomerCallStatus.Closed
                        join employee in DbContext.Employees on customerCall.TechnicalEmployeeId equals
                            employee.EmployeeId into gEmployees
                        from employee in gEmployees.DefaultIfEmpty()
                        select new
                                   {
                                       customerCall.CallNumber,
                                       customerCall.CustomerCallId,
                                       CustomerCallStatusName = customerCall.CustomerCallStatus.Name,
                                       CustomerName =
                            customerCall.Customer.Profile != null
                                ? customerCall.Customer.Profile.Name
                                : customerCall.Customer.LegalEntityProfile.CompanyName,
                                       customerCall.OpenedDate,
                                       TechnicalEmployee =
                            employee.EmployeeId != 0 ? employee.Profile.Name : String.Empty,
                                       ServiceOrderNumber =
                            customerCall.ServiceOrders.Any()
                                ? customerCall.ServiceOrders.First().ServiceOrderNumber
                                : String.Empty,
                                       ServiceOrderId =
                            customerCall.ServiceOrders.Any() ? customerCall.ServiceOrders.First().ServiceOrderId : 0
                                   };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "OpenedDate Desc");
        }

        /// <summary>
        /// This method just returns the count of registers from GetOpenCustomerCalls method
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetOpenCustomerCallsCount(Int32 companyId, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            return GetOpenCustomerCalls(companyId, sortExpression, startRowIndex, maximumRows).Cast<object>().Count();
        }


        /// <summary>
        /// this method returns the count of customerCalls i db
        /// </summary>
        /// <returns></returns>
        public Int32 GetCustomerCallsCount()
        {
            return DbContext.CustomerCalls.Count();
        }


        /// <summary>
        /// this method returns accolade CustomerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetAccoladeCustomerCalls(Int32 companyId, Int32? customerCallStatusId,
                                                   Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                   string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetCustomerCalls(companyId, null, customerCallStatusId, CustomerCallType.ACCOLADE,
                                    technicalEmployeeId, dateTimeInterval, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        ///  this is the count method of GetAccoladeCustomerCalls 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetAccoladeCustomerCallsCount(Int32 companyId, Int32? customerCallStatusId,
                                                   Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                   string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetAccoladeCustomerCalls(companyId, customerCallStatusId, technicalEmployeeId, dateTimeInterval,
                                         sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all information CustomerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetSupportCustomerCalls(Int32 companyId, Int32? customerCallStatusId,
                                                  Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                  string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetCustomerCalls(companyId, null, customerCallStatusId, CustomerCallType.SUPPORT, technicalEmployeeId,
                                    dateTimeInterval, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        ///  this is the count method of GetSupportCustomerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetSupportCustomerCallsCount(Int32 companyId, Int32? customerCallStatusId,
                                                  Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                  string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetSupportCustomerCalls(companyId, customerCallStatusId, technicalEmployeeId, dateTimeInterval,
                                        sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns complaints CustomerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetComplaintCustomerCalls(Int32 companyId, Int32? customerCallStatusId,
                                                    Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetCustomerCalls(companyId, null, customerCallStatusId, CustomerCallType.COMPLAINT,
                                    technicalEmployeeId, dateTimeInterval, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetComplaintCustomerCalls
        /// </summary>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetComplaintCustomerCallsCount(Int32 companyId, Int32? customerCallStatusId,
                                                    Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetComplaintCustomerCalls(companyId, customerCallStatusId, technicalEmployeeId, dateTimeInterval,
                                          sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns sugestion customerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetSugestionCustomerCalls(Int32 companyId, Int32? customerCallStatusId,
                                                    Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetCustomerCalls(companyId, null, customerCallStatusId, CustomerCallType.SUGESTION,
                                    technicalEmployeeId, dateTimeInterval, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count Method of GetSugestionCustomerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetSugestionCustomerCallsCount(Int32 companyId, Int32? customerCallStatusId,
                                                    Int32? technicalEmployeeId, DateTimeInterval dateTimeInterval,
                                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetSugestionCustomerCalls(companyId, customerCallStatusId, technicalEmployeeId, dateTimeInterval,
                                          sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns error customercalls 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetErrorCustomerCalls(Int32 companyId, Int32? customerCallStatusId, Int32? technicalEmployeeId,
                                                DateTimeInterval dateTimeInterval, string sortExpression,
                                                int startRowIndex, int maximumRows)
        {
            return GetCustomerCalls(companyId, null, customerCallStatusId, CustomerCallType.ERROR, technicalEmployeeId,
                                    dateTimeInterval, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetErrorCustomerCalls
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerCallStatusId"></param>
        /// <param name="technicalEmployeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetErrorCustomerCallsCount(Int32 companyId, Int32? customerCallStatusId, Int32? technicalEmployeeId,
                                                DateTimeInterval dateTimeInterval, string sortExpression,
                                                int startRowIndex, int maximumRows)
        {
            return
                GetErrorCustomerCalls(companyId, customerCallStatusId, technicalEmployeeId, dateTimeInterval,
                                      sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method returns customerCalls excluding error customerCall
        /// </summary>
        /// <returns></returns>
        public IQueryable GetCustomerCallExcludingErrorCustomerCall()
        {
            var queryCustomerCall = from customerCall in DbContext.CustomerCalls
                                    join employee in DbContext.Employees on customerCall.TechnicalEmployeeId equals
                                        employee.EmployeeId into gTechnicalEmployees
                                    from technicalEmployee in gTechnicalEmployees.DefaultIfEmpty()
                                    join postUsers in DbContext.Users on customerCall.UserId equals postUsers.UserId
                                        into gPostUsers
                                    from postUser in gPostUsers.DefaultIfEmpty()
                                    join postProfiles in DbContext.Profiles on postUser.ProfileId equals
                                        postProfiles.ProfileId into gPostProfiles
                                    from postProfile in gPostProfiles.DefaultIfEmpty()
                                    join technicalProfiles in DbContext.Profiles on technicalEmployee.ProfileId equals
                                        technicalProfiles.ProfileId into gTechnicalProfiles
                                    from technicalProfile in gTechnicalProfiles.DefaultIfEmpty()
                                    join customerCallStatus in DbContext.CustomerCallStatus on
                                        customerCall.CustomerCallStatusId equals customerCallStatus.CustomerCallStatusId
                                        into gCustomerCallStatus
                                    from customerCallStatus in gCustomerCallStatus.DefaultIfEmpty()
                                    where
                                        customerCall.CustomerCallTypeId != CustomerCallType.ERROR &&
                                        customerCall.CompanyId == new CompanyManager(this).GetHostCompany().CompanyId
                                    select new
                                               {
                                                   customerCall.CompanyId,
                                                   customerCall.CustomerCallId,
                                                   customerCall.ModifiedDate,
                                                   customerCall.CustomerId,
                                                   customerCall.CallNumber,
                                                   customerCall.CallNumberAssociated,
                                                   customerCall.Sector,
                                                   customerCall.OpenedDate,
                                                   customerCall.ClosedDate,
                                                   customerCall.CustomerEquipmentId,
                                                   customerCall.Description,
                                                   customerCall.CustomerCallTypeId,
                                                   customerCall.Subject,
                                                   customerCall.UserId,
                                                   customerCall.TechnicalEmployeeId,
                                                   customerCall.Source,
                                                   customerCall.Rating,
                                                   CustomerCallStatusId = customerCall.CustomerCallStatusId ?? 0,
                                                   TechnicalUserName = technicalProfile.Name,
                                                   customerCallStatusName = customerCallStatus.Name,
                                                   UserName = postProfile.Name,
                                                   UserNameEmail = postProfile.Email,
                                                   userNamePhone = postProfile.Phone
                                               };
            return queryCustomerCall.OrderByDescending(customercall => customercall.OpenedDate);
        }

        #endregion

        #region CustomerCallStatus

        public IQueryable<CustomerCallStatus> GetCustomerCallStatus()
        {
            return DbContext.CustomerCallStatus;
        }

        #endregion

        #region CRUD

        /// <summary>
        /// This method verifies if a customer is generating the same customerCall
        /// return false if exist the same customerCall  
        /// </summary>
        public bool CheckCustomerCallFromCustomer(Int32 customerId, string exceptionSubject, string callNumberAssociated)
        {
            CustomerCall customerCall =
                DbContext.CustomerCalls.Where(x => x.CustomerId == customerId && x.Subject == exceptionSubject &&
                                                   x.CallNumberAssociated == callNumberAssociated).FirstOrDefault();

            if (customerCall == null)
                return true;

            if (customerCall.CustomerCallStatusId == CustomerCallStatus.New ||
                customerCall.CustomerCallStatusId == CustomerCallStatus.Waiting)
                return false;

            if (customerCall.CustomerCallStatusId == CustomerCallStatus.Closed)
            {
                customerCall.CustomerCallStatusId = Convert.ToInt32(CustomerCallStatus.New);
                customerCall.OpenedDate = DateTime.Now;
                DbContext.SubmitChanges();

                var commManager = new CommentsManager(this);
                commManager.AddCommentInCustomerCall(customerCall.CustomerCallId,
                                                     "Chamado foi reaberto automaticamente pelo sistema!");
            }

            return false;
        }


        /// <summary>
        /// this method closes a customerCall
        /// </summary>
        /// <param name="entity"></param>
        public void CloseCustomerCall(Int32 customerCallId)
        {
            CustomerCall entity = GetCustomerCall(customerCallId);
            entity.CustomerCallStatusId = CustomerCallStatus.Closed;
            entity.ClosedDate = DateTime.Now.Date;
            DbContext.SubmitChanges();

            //
            // Send a message to customer alerting that your call was closed
            //
            if (entity.UserId.HasValue)
            {
                string message = "";
                switch (entity.CustomerCallTypeId)
                {
                    case CustomerCallType.ACCOLADE:
                        message = "Toda a equipe da Vivina, agradece seu elogio, fica muito satisfeita ao saber que existem pessoas sendo beneficiadas pelo sistema!";
                        break;
                    case CustomerCallType.COMPLAINT:
                        message = "Obrigado por ter reclamado dos nossos serviços, somente assim podemos aprender, corrigir e saber o que fazer para não errar mais.<br /><br />Muito Obrigado!";
                        break;
                    case CustomerCallType.ERROR:
                        message = String.Format("<center>Sabe um erro que deu em {0} no dia {1} foi corrigido! Essas joaninhas aprenderam a lição!" + 
                                                "<br /><br />"+
                                                "<a href='javascript:;' onclick=\"top.tb_show('Chamado', 'CRM/CustomerCall.aspx?ModalPopUp=1&ReadOnly=true&CustomerCallId={2}');\">" + 
                                                "   Clique aqui para ver o chamado!"+
                                                "</a>"+
                                                "</center>",
                                                entity.Sector,
                                                entity.OpenedDate.ToShortDateString(),
                                                entity.CustomerCallId.EncryptToHex());
                        break;
                    case CustomerCallType.SUGESTION:
                        message = "Obrigado pela sugestão, nós iremos cuidar para que sua idéia seja levada em consideração na próxima versão!";
                        break;
                    case CustomerCallType.SUPPORT:
                        message = "Toda a equipe da Vivina fica muito satisfeita ao saber que existem pessoas sendo beneficiadas pelo sistema!";
                        break;
                }

                var manager = new AlertManager(this);
                manager.InsertAlert(entity.UserId.Value, message);
            }
        }

        /// <summary>
        /// this method return a customerCall
        /// </summary>
        /// <param name="customerCallId"></param>
        /// <returns></returns>
        public CustomerCall GetCustomerCall(Int32 customerCallId)
        {
            return DbContext.CustomerCalls.Where(c => c.CustomerCallId == customerCallId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves a customerCall by their callNumber
        /// </summary>
        /// <param name="callNumber"></param>
        /// <returns></returns>
        public CustomerCall GetCustomerCall(String callNumber)
        {
            return
                DbContext.CustomerCalls.Where(customerCall => customerCall.CallNumber.Equals(callNumber)).FirstOrDefault
                    ();
        }

        /// <summary>
        /// This method delete one CustomerCall.
        /// </summary>
        /// <param name=entity>entity</param>
        public void DeleteCustomerCall(CustomerCall entity)
        {
            DbContext.CustomerCalls.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method update one CustomerCall.
        /// </summary>
        /// <param name=entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void UpdateCustomerCall(CustomerCall original_entity, CustomerCall entity)
        {
            if (original_entity.CustomerCallStatusId == CustomerCallStatus.Closed)
                entity.ClosedDate = DateTime.Now;

            if (!original_entity.TechnicalEmployeeId.HasValue && entity.TechnicalEmployeeId.HasValue &&
                original_entity.UserId.HasValue)
            {
                var manager = new AlertManager(this);
                Employee employee = new HumanResourcesManager(this).GetEmployee(entity.CompanyId,
                                                                                (int) entity.TechnicalEmployeeId);

                manager.InsertAlert(entity.UserId.Value,
                                    "Olá " + original_entity.User.Profile.FirstName + ", prazer!<br /> Meu nome é " +
                                    employee.Profile.AbreviatedName +
                                    " e sou o responsável pelo seu chamado, que está em análise!");
            }

            SetCustomerCallPriority(entity);
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method insert a customerCall and insert a new appointment for a TechnicalEmployee
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="employeeId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        public void InsertCustomerCall(CustomerCall entity, Int32? employeeId, DateTime? beginTime, DateTime? endTime)
        {
            if (!CheckCustomerCallFromCustomer(entity.CustomerId, entity.Subject, entity.CallNumberAssociated))
                return;

            if (entity.CustomerEquipmentId == 0)
                entity.CustomerEquipmentId = null;

            entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
            entity.OpenedDate = DateTime.Now;

            SetCustomerCallPriority(entity);
            DbContext.CustomerCalls.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        private void SetCustomerCallPriority(CustomerCall entity)
        {
            if (entity.CustomerCallTypeId == CustomerCallType.ERROR)
                entity.Rating = 5;
        }

        #endregion

        #region CustomerCallStatus

        /// <summary>
        /// this method return all customerCallType
        /// </summary>
        /// <returns></returns>
        public IQueryable<CustomerCallType> GetCustomerCallTypes()
        {
            return DbContext.CustomerCallTypes;
        }

        #endregion

        #endregion

        #region Profile

        /// <summary>
        /// this method returns a customer in accordande with your LegalEntityProfile
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="legalEntityProfileId"></param>
        /// <returns></returns>
        /// 
        public Customer GetCustomerByLegalEntityProfile(int companyId, int legalEntityProfileId)
        {
            return
                DbContext.Customers.Where(
                    x => x.CompanyId == companyId && x.LegalEntityProfileId == legalEntityProfileId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a customer in accordance with your profile.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public Customer GetCustomerByProfile(int companyId, int profileId)
        {
            return DbContext.Customers.Where(x => x.CompanyId == companyId && x.ProfileId == profileId).FirstOrDefault();
        }

        #endregion

        #region Sale

        public bool IsInDebit(int customerId)
        {
            Parcel parcel = (from inv in DbContext.Invoices
                             join pc in DbContext.Parcels on inv.InvoiceId equals pc.InvoiceId
                             where inv.CustomerId == customerId && pc.DueDate < DateTime.Now
                                   && pc.EffectedDate == null
                             select pc).FirstOrDefault();
            return (parcel != null);
        }

        /// <summary>
        /// this method return summarize of sale value by customerType
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable summarizeSaleValueByCustomerType(int companyId)
        {
            var query = from sale in DbContext.Sales
                        join customer in DbContext.Customers on sale.CustomerId equals customer.CustomerId
                        join customerType in DbContext.CustomerTypes on customer.CustomerTypeId equals
                            customerType.CustomerTypeId
                        join saleItem in DbContext.SaleItems on sale.SaleId equals saleItem.SaleId
                        group saleItem by customerType.Name
                        into gSaleItems
                            select new
                                       {
                                           tipo = gSaleItems.Key,
                                           total = (gSaleItems.Sum(x => x.UnitPrice*x.Quantity))
                                       };
            return query.ToDataTable();
        }

        /// <summary>
        /// this method the customer Related with a sale
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public DataTable GetCustomerBySale(int saleId)
        {
            var query = from customer in DbContext.Customers
                        join sale in DbContext.Sales on customer.CustomerId equals sale.CustomerId
                        where sale.SaleId == saleId
                        select new
                                   {
                                       name =
                            customer.Profile.Name ?? "" + customer.LegalEntityProfile.CompanyName ?? "",
                                       //address = customer.Profile.Address.Name ?? "" + customer.LegalEntityProfile.Address.Name ?? "",
                                       postalCode =
                            customer.Profile.PostalCode ?? "" + customer.LegalEntityProfile.PostalCode ?? "",
                                       phone = customer.Profile.Phone ?? "" + customer.LegalEntityProfile.Phone ?? ""
                                   };
            return query.ToDataTable();
        }

        #endregion

        #region CustomerFollowUp

        private void GenerateTaskFromCustomerFollowUp(CustomerFollowup customerFollowup, Int32 userId,
                                                      DateTime appointmentDate, string appoimentSubject)
        {
            // ver se já existe tarefa associada ao followUp 
            var taskManager = new TaskManager(this);
            var task = new Task();
            Task originalTask;

            originalTask = taskManager.GetTask(customerFollowup.CustomerFollowupId, "CustomerFollowUp.aspx");

            if (originalTask != null)
                task.CopyPropertiesFrom(originalTask);

            task.SubjectId = customerFollowup.CustomerFollowupId;
            task.PageName = "CustomerFollowUp.aspx";
            task.Priority = 3;
            task.Name = appoimentSubject;
            task.StartDate = appointmentDate;

            DataClasses.User user = new CompanyManager(null).GetUser(customerFollowup.CompanyId, userId);
            var listUser = new List<DataClasses.User>();

            listUser.Add(user);

            if (originalTask != null)
                taskManager.SaveTask(originalTask, task, listUser);
            else
                taskManager.SaveTask(task, null, listUser);
        }

        /// <summary>
        /// this method insert a CustomerFollowup
        /// </summary>
        /// <param name="entity"></param>
        public void InsertCustomerFollowup(CustomerFollowup entity, Int32? userId, DateTime? appointmentDate,
                                           string appointmentSubject)
        {
            entity.EntryDate = DateTime.Now;
            DbContext.CustomerFollowups.InsertOnSubmit(entity);
            DbContext.SubmitChanges();

            if (appointmentDate.HasValue && userId.HasValue && !String.IsNullOrEmpty(appointmentSubject))
                GenerateTaskFromCustomerFollowUp(entity, Convert.ToInt32(userId), Convert.ToDateTime(appointmentDate),
                                                 appointmentSubject);
        }

        /// <summary>
        /// this method update a customerfollowup
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateCustomerFollowup(CustomerFollowup original_entity, CustomerFollowup entity, Int32? userId,
                                           DateTime? appointmentDate, string appointmentSubject)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();

            if (appointmentDate.HasValue && userId.HasValue && !String.IsNullOrEmpty(appointmentSubject))
                GenerateTaskFromCustomerFollowUp(entity, Convert.ToInt32(userId), Convert.ToDateTime(appointmentDate),
                                                 appointmentSubject);
        }

        /// <summary>
        /// this method delete a customerfollowup
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteCustomerFollowup(CustomerFollowup entity)
        {
            //DbContext.CustomerFollowups.Attach(entity);
            DbContext.CustomerFollowups.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return a single Customerfollowup
        /// </summary>
        /// <param name="customerFollowupId"></param>
        /// <returns></returns>
        [Obsolete("This method was changed by new method bellow")]
        public CustomerFollowup GetCustomerFollowup(Int32 customerFollowupId)
        {
            return DbContext.CustomerFollowups.Where(c => c.CustomerFollowupId == customerFollowupId).FirstOrDefault();
        }

        /// <summary>
        /// this method return a CustomerFollowup
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerFollowupId"></param>
        /// <returns></returns>
        public CustomerFollowup GetCustomerFollowup(Int32 companyId, Int32 customerFollowupId)
        {
            return
                DbContext.CustomerFollowups.Where(
                    customerFollowup =>
                    customerFollowup.CompanyId == companyId && customerFollowup.CustomerFollowupId == customerFollowupId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// this method return all CustomerFollowup
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetCustomerFollowups(Int32 companyId, Int32? customerFollowupActionId, string contactName,
                                               DateTimeInterval dateTimeInterval,
                                               string sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            var query = from customerFollowup in DbContext.CustomerFollowups
                        where customerFollowup.CompanyId == companyId
                        join customerFollowupAction in DbContext.CustomerFollowupActions on
                            customerFollowup.CustomerFollowupActionId equals
                            customerFollowupAction.CustomerFollowupActionId into gCustomerFollowUpActions
                        from customerFollowUpActions in gCustomerFollowUpActions.DefaultIfEmpty()
                        join contact in DbContext.Contacts on customerFollowup.ContactId equals contact.ContactId
                        select new
                                   {
                                       customerFollowup.CustomerFollowupId,
                                       customerFollowup.CompanyId,
                                       customerFollowup.ContactId,
                                       customerFollowup.CustomerFollowupActionId,
                                       customerFollowup.UserId,
                                       customerFollowup.EntryDate,
                                       customerFollowup.Description,
                                       ContactName = contact.Name,
                                       CustomerFollowupActionName = customerFollowUpActions.Name
                                   };

            if (customerFollowupActionId.HasValue)
                query = query.Where(x => x.CustomerFollowupActionId == customerFollowupActionId);

            if (!String.IsNullOrEmpty(contactName))
                query = query.Where(x => x.ContactName.Contains(contactName));

            if (dateTimeInterval != null)
                query =
                    query.Where(
                        x => x.EntryDate >= dateTimeInterval.BeginDate && x.EntryDate <= dateTimeInterval.EndDate);

            return query.SortAndPage(
                String.IsNullOrEmpty(sortExpression)
                    ? "EntryDate Desc"
                    : sortExpression,
                startRowIndex, maximumRows, "CustomerFollowupId");
        }

        /// <summary>
        /// this method return the total of CustomerFollowups
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetCustomerFollowupsCount(Int32 companyId, Int32? customerFollowupActionId, string contactName,
                                               DateTimeInterval dateTimeInterval,
                                               string sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            return
                GetCustomerFollowups(companyId, customerFollowupActionId, contactName, dateTimeInterval, sortExpression,
                                     startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        #endregion

        #region CustomerFollowupAction

        /// <summary>
        /// this method insert a new CustomerFollowupAction
        /// </summary>
        /// <param name="customerFollowupAction"></param>
        public void InsertCustomerFollowupAction(CustomerFollowupAction entity)
        {
            DbContext.CustomerFollowupActions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete a CustomerFollowupAction
        /// </summary>
        /// <param name="customerFollowupAction"></param>
        public void DeleteCustomerFollowUpAction(CustomerFollowupAction entity)
        {
            //DbContext.CustomerFollowupActions.Attach(entity);
            DbContext.CustomerFollowupActions.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method update a CustomerFollowupAction
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateCustomerFollowupAction(CustomerFollowupAction original_entity, CustomerFollowupAction entity)
        {
            DbContext.CustomerFollowupActions.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return a single CustomerFollowupAction
        /// </summary>
        /// <param name="customerFollowupActionId"></param>
        /// <returns></returns>
        public CustomerFollowupAction GetCustomerFollowupAction(Int32 customerFollowupActionId)
        {
            return
                DbContext.CustomerFollowupActions.Where(c => c.CustomerFollowupActionId == customerFollowupActionId).
                    FirstOrDefault();
        }

        /// <summary>
        /// this method return all CustomerFollowupActions
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IList GetCustomerFollowupActions(Int32 companyId, string sortExpression, Int32 startRowIndex,
                                                Int32 maximumRows)
        {
            return DbContext.CustomerFollowupActions
                .Where(c => c.CompanyId == companyId)
                .SortAndPage(sortExpression, startRowIndex, maximumRows, "CustomerFollowupActionId")
                .ToList();
        }

        /// <summary>
        /// this method return the total of CustomerFollowupActions
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetCustomerFollowupActionsCount(Int32 companyId, string sortExpression, Int32 startRowIndex,
                                                     Int32 maximumRows)
        {
            return GetCustomerFollowupActions(companyId, sortExpression, startRowIndex, maximumRows).Count;
        }

        /// <summary>
        /// this method returl all CustomerFollowupActions as Datatable
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetCustomerFollowupActions(Int32 companyId)
        {
            return DbContext.CustomerFollowupActions.Where(c => c.CompanyId == companyId).ToDataTable();
        }

        /// <summary>
        /// this method return the total of CustomerFollowupActions
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        //public Int32 GetCustomerFollowupActionsCount(Int32 companyId)
        //{
        //    return GetCustomerFollowupActions(companyId).Rows.Count;
        //}

        #endregion

        #region Contract

        /// <summary>
        /// return the contracts frm customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataTable GetContractsByCustomer(Int32 customerId)
        {
            return DbContext.Contracts.Where(x => x.CustomerId == customerId).ToDataTable();
        }

        ///// <summary>
        ///// this method return all period of contracts
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetContractPeriod()
        //{
        //    return DbContext.ContractPeriods.ToDataTable();
        //}

        #endregion

        #region CustomerType

        /// <summary>
        /// This method return all customer types by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<CustomerType> GetAllCustomerType(int companyId)
        {
            return DbContext.CustomerTypes.Where(c => c.CompanyId == companyId);
        }

        #endregion

        //this region contains all functions of CustomerCall

        //this regions contains all functions of CustomerFollowup

        //this region contains all functions of CustomerFollowupAction 
    }
}