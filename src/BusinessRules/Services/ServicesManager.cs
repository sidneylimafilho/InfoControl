using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.Mail;
using Vivina.Erp.DataClasses;
using System.Data.Linq;
using InfoControl.Security.Cryptography;

namespace Vivina.Erp.BusinessRules.Services
{
    public enum ServiceOrderStatus
    {
        AwaitingParts = 1,
        InProgress,
        Canceled,
        Success,
        AwaitingTechnical
    }

    public enum ServiceOrderType
    {
        Sundry = 1,
        Warranty,
        Contract
    }

    public class ServicesManager : BusinessManager<InfoControlDataContext>
    {
        public ServicesManager(IDataAccessor container)
            : base(container)
        {
        }

        //this region contains all functions of service

        #region Services

        public IQueryable<Recognizable> SearchServiceAsList(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            //if (DataManager.CacheCommands[methodName] == null)
            //{
            //IQueryable<string> query = from service in DbContext.Services
            //                           where service.CompanyId == companyId && service.Name.Contains(name)
            //                           select service.Name;
            //DataManager.CacheCommands[methodName] = DbContext.GetCommand(query.Take(maximumRows));

            DataManager.CacheCommands[methodName] = CompiledQuery.Compile
                               <InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                               (ctx, _companyId, _name, _maximumRows) => (from service in DbContext.Services
                                                                          where service.CompanyId == companyId && service.Name.Contains(name)
                                                                          select new Recognizable(service.ServiceId.EncryptToHex(), service.Name)).Take(maximumRows));
            //}

            var method = (Func<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>)DataManager.CacheCommands[methodName];
            return method(DbContext, companyId, name, maximumRows);

            //DataReader reader = DataManager.ExecuteCachedQuery(methodName, companyId, "%" + name + "%");
            //var list = new List<string>();
            //while (reader.Read())
            //{
            //    list.Add(reader.GetString(0));
            //}
            //return list;
        }

        /// <summary>
        /// this method insert a service
        /// </summary>
        /// <param name="service"></param>
        public void InsertService(Service entity)
        {
            DbContext.Services.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method update a service
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateService(Service original_entity, Service entity)
        {
            DbContext.Services.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete a service
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteService(Service entity)
        {
            DbContext.Services.DeleteOnSubmit(GetService(entity.ServiceId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return a single Service
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public Service GetService(Int32 ServiceId)
        {
            return DbContext.Services.Where(x => x.ServiceId == ServiceId).FirstOrDefault();
        }

        /// <summary>
        /// this method return all Services with Paging.it is used to feed the grade
        /// </summary>
        /// <returns></returns>
        public IQueryable<Service> GetServices(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                DbContext.Services.Where(x => x.CompanyId == companyId).SortAndPage(sortExpression, startRowIndex,
                                                                                    maximumRows, "ServiceId").
                    AsQueryable();
        }

        /// <summary>
        /// This method return the total of services
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetServicesCount(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetServices(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method return all services by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Service> GetServices(int companyId)
        {
            return DbContext.Services.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// this method return the total of service
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetServicesCount(Int32 companyId)
        {
            return GetServices(companyId).Count();
        }

        public Service GetServiceByName(Int32 companyID, String name)
        {
            return GetServices(companyID).Where(service => service.Name.Equals(name)).FirstOrDefault();
        }

        #endregion

        //this region contains all functions of serviceOrderType

        #region ServiceOrderType

        /// <summary>
        /// this method return all ServiceOrderTypes
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllServiceOrderTypes()
        {
            return DbContext.ServiceOrderTypes.ToDataTable();
        }

        /// <summary>
        /// This method retrieve the serviceOrderTypes by specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<DataClasses.ServiceOrderType> GetServiceOrderTypes(Int32 companyId)
        {
            return DbContext.ServiceOrderTypes.Where(serviceOrderType => serviceOrderType.CompanyId == companyId);
        }

        #endregion

        //this region contains all functions of ServiceOrderStatus

        #region ServiceOrderStatus

        /// <summary>
        /// return all ServiceOrderStatus
        /// </summary>
        /// <returns></returns>
        public DataTable getAllServiceOrderStatus()
        {
            return DbContext.ServiceOrderStatus.ToDataTable();
        }

        #endregion

        //this region contains all functions of ServiceOrder

        #region ServiceOrder

        /// <summary>
        /// This method returns the servicesOrders by customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetServiceOrdersByCustomer(Int32 customerID, string sortExpression, int startRowIndex,
                                                     int maximumRows)
        {
            var query = from serviceOrder in DbContext.ServiceOrders
                        join serviceOrderStatus in DbContext.ServiceOrderStatus
                            on serviceOrder.ServiceOrderStatusId equals serviceOrderStatus.ServiceOrderStatusId
                        where serviceOrder.CustomerId == customerID
                        select new
                        {
                            serviceOrder.ServiceOrderNumber,
                            serviceOrderStatusName = serviceOrderStatus.Name,
                            serviceOrder.OpenedDate,
                            serviceOrder.ServiceOrderId,
                            serviceOrderStatus.ServiceOrderStatusId
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ServiceOrderId");
        }

        /// <summary>
        /// This method retrieve the count of registers returns by GetServiceOrdersByCustomer method
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetServiceOrdersByCustomerCount(Int32 customerID, string sortExpression, int startRowIndex,
                                                     int maximumRows)
        {
            return
                GetServiceOrdersByCustomer(customerID, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        /// <summary>
        /// this method save the service order
        /// </summary>
        /// <param name="original_entity"> this parameter is used only in Update</param>
        /// <param name="entity"> this parameter always is used</param>
        /// <param name="lstServiceOrderItem"> list of items</param>
        public void SaveServiceOrder(ServiceOrder original_entity, ServiceOrder entity, List<ServiceOrderItem> lstServiceOrderItem, Int32? depositId)
        {
            var stockManager = new InventoryManager(this);

            //
            // Insert
            //

            if (original_entity.ServiceOrderId == 0)
            {
                //insert
                entity.OpenedDate = entity.CreatedDate = entity.ModifiedDate = DateTime.Now;

                DbContext.ServiceOrders.InsertOnSubmit(entity);
                DbContext.SubmitChanges();

                InsertServiceOrderItems(entity.ServiceOrderId, lstServiceOrderItem);

                if (depositId.HasValue && entity.ServiceOrderStatusId == (int)ServiceOrderStatus.Success)
                    RemoveQuantityInStockFromServiceOrderItemList(depositId.Value, lstServiceOrderItem);

                entity.ServiceOrderNumber = "OS-" + entity.ServiceOrderId;
                DbContext.SubmitChanges();

                return;
            }

            //
            //Update
            //                       

            if (!depositId.HasValue)
            {
                UpdateServiceOrder(original_entity, entity);

                DeleteAllServiceOrderItems(original_entity);

                InsertServiceOrderItems(lstServiceOrderItem);

                if (ServiceOrderWasChangedForSuccessStatus(original_entity, entity))
                {
                    if (original_entity.Customer.UserId.HasValue)
                        SendEmailToCustomer(original_entity);
                }

                return;
            }

            if (ServiceOrderWasChangedForSuccessStatus(original_entity, entity) && depositId.HasValue)
            {
                RemoveQuantityInStockFromServiceOrderItemList(depositId.Value, lstServiceOrderItem);

                UpdateServiceOrder(original_entity, entity);

                DeleteAllServiceOrderItems(entity);

                InsertServiceOrderItems(original_entity.ServiceOrderId, lstServiceOrderItem);

                //
                // verifies if the customer related in serviceOrder has an user
                // before to send the email
                //                 
                if (original_entity.Customer.UserId.HasValue)
                    SendEmailToCustomer(original_entity);

                return;
            }

            if (ServiceOrderIsSuccessStatusAndNotChanged(original_entity, entity) && depositId.HasValue)
            {
                UpdateServiceOrder(original_entity, entity);

                AddQuantityInStockFromServiceOrderItemList(depositId.Value, original_entity.ServiceOrderId);

                RemoveQuantityInStockFromServiceOrderItemList(depositId.Value, lstServiceOrderItem);

                DeleteAllServiceOrderItems(entity);

                InsertServiceOrderItems(original_entity.ServiceOrderId, lstServiceOrderItem);

                return;
            }

            if (ServiceOrderIsSuccessStatusAndWasChanged(original_entity, entity) && depositId.HasValue)
            {
                UpdateServiceOrder(original_entity, entity);

                AddQuantityInStockFromServiceOrderItemList(depositId.Value, original_entity.ServiceOrderId);

                DeleteAllServiceOrderItems(entity);

                InsertServiceOrderItems(original_entity.ServiceOrderId, lstServiceOrderItem);

                return;
            }
        }

        /// <summary>
        /// This method sends an email to customer, case the status of Service Order 
        /// was changed to success
        /// </summary>
        /// <param name="originalServiceOrder"></param>
        private void SendEmailToCustomer(ServiceOrder originalServiceOrder)
        {
            if (originalServiceOrder.Customer.UserId.HasValue)
            {
                Postman.Send("mailer@InfoControl.com.br", originalServiceOrder.Customer.User.UserName, "Ordem de serviço atendida",
                    String.Format(@"Ordem de serviço de número: {0} criada no dia {1}, foi atendida e concluída !!
                                        <br/><br/> <a href='{2}'>Clique aqui para gerar sua nota fiscal!</a>", originalServiceOrder.ServiceOrderNumber,
                        originalServiceOrder.OpenedDate, originalServiceOrder.Company.LegalEntityProfile.Website), null);
            }
        }

        /// <summary>
        /// This method inserts the serviceOrderItems in specified serviceOrder
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <param name="serviceOrderItemList"></param>
        private void InsertServiceOrderItems(Int32 serviceOrderId, List<ServiceOrderItem> serviceOrderItemList)
        {
            foreach (var item in serviceOrderItemList)
            {
                item.ServiceOrderId = serviceOrderId;

                if (item.EmployeeId == 0)
                    item.EmployeeId = null;

                InsertServiceOrderItem(item);
            }
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method verifies if the serviceOrder was changed for successStatus
        /// </summary>
        /// <param name="originalServiceOrder"></param>
        /// <param name="newServiceOrder"></param>
        /// <returns></returns>
        private bool ServiceOrderWasChangedForSuccessStatus(ServiceOrder originalServiceOrder, ServiceOrder newServiceOrder)
        {
            return originalServiceOrder.ServiceOrderStatusId.Value != (int)ServiceOrderStatus.Success &&
                       newServiceOrder.ServiceOrderStatusId.Value == (int)ServiceOrderStatus.Success;
        }

        /// <summary>
        /// This method verifies if the Service Order status is success and was not changed
        /// </summary>
        /// <param name="originalServiceOrder"></param>
        /// <param name="newServiceOrder"></param>
        /// <returns></returns>
        private bool ServiceOrderIsSuccessStatusAndNotChanged(ServiceOrder originalServiceOrder, ServiceOrder newServiceOrder)
        {
            return originalServiceOrder.ServiceOrderStatusId == (int)ServiceOrderStatus.Success && newServiceOrder.ServiceOrderStatusId == (int)ServiceOrderStatus.Success;
        }

        /// <summary>
        /// This method verifies if the Service Order status is success
        /// and was changed for some other status
        /// </summary>
        /// <param name="originalServiceOrder"></param>
        /// <param name="newServiceOrder"></param>
        /// <returns></returns>
        private bool ServiceOrderIsSuccessStatusAndWasChanged(ServiceOrder originalServiceOrder, ServiceOrder newServiceOrder)
        {
            return originalServiceOrder.ServiceOrderStatusId.Value == (int)ServiceOrderStatus.Success && newServiceOrder.ServiceOrderStatusId.Value != (int)ServiceOrderStatus.Success;
        }

        /// <summary>
        /// This method makes stock drop from products in serviceOrderItemList of session
        /// </summary>
        /// <param name="depositId"></param>
        /// <param name="serviceOrderItemList"></param>
        private void RemoveQuantityInStockFromServiceOrderItemList(Int32 depositId, List<ServiceOrderItem> serviceOrderItemList)
        {
            var inventoryManager = new InventoryManager(this);

            foreach (var item in serviceOrderItemList)
                if (item.ProductId.HasValue)
                {
                    var inventoryProduct = inventoryManager.GetInventory(item.CompanyId, Convert.ToInt32(item.ProductId), Convert.ToInt32(depositId));
                    inventoryManager.InventoryDrop(inventoryProduct, Convert.ToInt32(item.Quantity), Convert.ToInt32(DropType.Use), null);
                }
        }

        /// <summary>
        /// This method adds quantity in stock from serviceOrderItems of specified serviceOrder from db
        /// </summary>
        /// <param name="depositId"></param>
        /// <param name="serviceOrderId"></param>
        private void AddQuantityInStockFromServiceOrderItemList(Int32 depositId, Int32 serviceOrderId)
        {
            var serviceOrderItems = GetServiceOrderItems(serviceOrderId).ToList();
            var inventoryManager = new InventoryManager(this);

            foreach (var item in serviceOrderItems)
            {
                if (item.ProductId.HasValue)
                {
                    var inventory = new Inventory();

                    inventory.CopyPropertiesFrom(inventoryManager.GetInventory(item.CompanyId, Convert.ToInt32(item.ProductId), Convert.ToInt32(depositId)));

                    inventoryManager.AddQuantityInDeposit(item.CompanyId, null, Convert.ToInt32(item.ProductId), Convert.ToInt32(depositId), Convert.ToInt32(item.Quantity));
                }
            }

        }

        /// <summary>
        /// this method insert a ServiceOrder
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Int32 InsertServiceOrder(ServiceOrder entity)
        {
            entity.OpenedDate = DateTime.Now;
            entity.ServiceOrderStatusId = (int)ServiceOrderStatus.InProgress;
            entity.ServiceOrderTypeId = (int)ServiceOrderType.Sundry;
            DbContext.ServiceOrders.InsertOnSubmit(checkClosedServiceOrder(entity));
            DbContext.SubmitChanges();
            return entity.ServiceOrderId;
        }

        /// <summary>
        /// this method update a ServiceOrder
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateServiceOrder(ServiceOrder original_entity, ServiceOrder entity)
        {
            original_entity = GetServiceOrder(original_entity.ServiceOrderId);
            original_entity.ModifiedDate = DateTime.Now;
            original_entity.CopyPropertiesFrom(checkClosedServiceOrder(entity));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete a serviceOrder and all ServiceOrderItem from the ServiceOrder
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteServiceOrder(ServiceOrder entity)
        {
            DeleteAllServiceOrderItems(entity);

            //DbContext.ServiceOrders.Attach(entity);
            DbContext.ServiceOrders.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method can return all or some service orders based on search parameters 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="serviceOrderStatusId"></param>
        /// <param name="serviceOrderTypeId"></param>
        /// <param name="organizationLevelId"></param>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param> 
        /// <returns></returns>
        public IQueryable GetServiceOrders(Int32 companyId, Int32? serviceOrderStatusId, Int32? serviceOrderTypeId,
                                           Int32? organizationLevelId,
                                           Int32? employeeId, DateTimeInterval dateTimeInterval, String competency,
                                           string sortExpression, int startRowIndex, int maximumRows)
        {

            return GetServiceOrders(companyId, String.Empty, serviceOrderStatusId, serviceOrderTypeId, organizationLevelId, employeeId, dateTimeInterval, competency, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// This method returns the total of service orders after the search
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="serviceOrderStatusId"></param>
        /// <param name="serviceOrderTypeId"></param>
        /// <param name="organizationLevelId"></param>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetServiceOrdersCount(int companyId, Int32? serviceOrderStatusId, Int32? serviceOrderTypeId,
                                           Int32? organizationLevelId, Int32? employeeId, DateTimeInterval dateTimeInterval, String competency,
                                           string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetServiceOrders(companyId, serviceOrderStatusId, serviceOrderTypeId, organizationLevelId, employeeId,
                                 dateTimeInterval, competency, sortExpression, startRowIndex, maximumRows).Cast
                    <Object>().Count();
        }

        public Int32 GetServiceOrdersCount(int companyId, string customerName, Int32? serviceOrderStatusId, Int32? serviceOrderTypeId,
                                          Int32? organizationLevelId, Int32? employeeId, DateTimeInterval dateTimeInterval, String competency,
                                          string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetServiceOrders(companyId, customerName, serviceOrderStatusId, serviceOrderTypeId, organizationLevelId, employeeId,
                                    dateTimeInterval, competency, sortExpression, startRowIndex, maximumRows).Cast
                       <Object>().Count();
        }


        /// <summary>
        /// This method returns serviceOrders with specified search parameters 
        /// this method is adapted for gridViews
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerName"></param>
        /// <param name="serviceOrderStatusId"></param>
        /// <param name="serviceOrderTypeId"></param>
        /// <param name="organizationLevelId"></param>
        /// <param name="employeeId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="competency"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetServiceOrders(Int32 companyId, String customerName, Int32? serviceOrderStatusId, Int32? serviceOrderTypeId,
                                          Int32? organizationLevelId, Int32? employeeId, DateTimeInterval dateTimeInterval, String competency,
                                          string sortExpression, int startRowIndex, int maximumRows)
        {

            var query = from serviceOrder in DbContext.ServiceOrders.Where(x => x.CompanyId == companyId)
                        join customer in DbContext.Customers on serviceOrder.CustomerId equals customer.CustomerId
                        join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into gProfile
                        from profile in gProfile.DefaultIfEmpty()
                        select new
                        {
                            serviceOrder,
                            serviceOrder.ServiceOrderItems,
                            serviceOrder.ServiceOrderId,
                            serviceOrder.CustomerId,
                            customerName = (legalEntityProfile.CompanyName ?? "") + (profile.Name ?? ""),
                            serviceOrder.CompanyId,
                            serviceOrder.ServiceOrderNumber,
                            serviceOrder.CustomerCallId,
                            serviceOrder.ServiceOrderTypeId,
                            serviceOrder.ServiceOrderStatusId,
                            serviceOrder.CustomerEquipmentId,
                            serviceOrder.OpenedDate,
                            serviceOrder.ClosedDate,
                            status = serviceOrder.ServiceOrderStatus.Name,
                            type = serviceOrder.ServiceOrderType.Name,
                            serviceOrder.ReceiptId,
                            serviceOrder.TechnicalDecision,
                            serviceOrder.PhysicalServiceOrder,
                            serviceOrder.PhysicalServiceOrderName,
                            serviceOrder.ServiceOrderProductDamageId,
                            serviceOrder.ServiceOrderEquipmentDamageId,
                            serviceOrder.ServiceOrderTestId,
                            serviceOrder.ServiceType,
                            serviceOrder.ServiceOrderProductType,
                            serviceOrder.ServiceOrderHaltType,
                            serviceOrder.ServiceOrderInstallType,
                        };

            if (!String.IsNullOrEmpty(customerName))
                query = query.Where(s => s.customerName.Contains(customerName));

            if (serviceOrderStatusId.HasValue)
                query = query.Where(s => s.ServiceOrderStatusId.Equals(serviceOrderStatusId));

            if (serviceOrderTypeId.HasValue && serviceOrderTypeId != 0)
                query = query.Where(t => t.ServiceOrderTypeId.Equals(serviceOrderTypeId));

            if (organizationLevelId.HasValue)
                query =
                    query.Where(
                        ol =>
                        ol.serviceOrder.ServiceOrderItems.Any(
                            serviceOrderItem => serviceOrderItem.Employee.OrganizationlevelId == organizationLevelId));

            if (employeeId.HasValue)
                query =
                    query.Where(
                        e =>
                        e.serviceOrder.ServiceOrderItems.Any(
                            serviceOrderItem => serviceOrderItem.EmployeeId == employeeId));

            if (dateTimeInterval != null)
                query =
                    query.Where(
                        dt => dt.OpenedDate >= dateTimeInterval.BeginDate && dt.OpenedDate <= dateTimeInterval.EndDate);

            if (!String.IsNullOrEmpty(competency))
                query = query.Where(t => (from tu in DbContext.TaskUsers
                                          join employee in DbContext.Employees on tu.User.ProfileId equals
                                              employee.ProfileId
                                          join employeeCompetency in DbContext.EmployeeCompetencies on
                                              employee.EmployeeId equals employeeCompetency.EmployeeId
                                          where
                                              employeeCompetency.Name.Equals(competency) &&
                                              tu.Task.SubjectId == t.serviceOrder.ServiceOrderId
                                          select employee).Any());

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ServiceOrderId");

        }


        /// <summary>
        /// this method return all ServiceOrder with Paging. It is used to feed the grade
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetServiceOrders(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetServiceOrders(companyId, null, null, null, null, null, null, sortExpression, startRowIndex,
                                    maximumRows);
        }

        /// <summary>
        /// This method returns ServiceOrders by company and their status
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="serviceOrderStatusId"></param>
        /// <returns></returns>
        public IQueryable GetOpenServiceOrders(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {

            var query = from serviceOrder in DbContext.ServiceOrders.Where(x => x.CompanyId == companyId)
                        where serviceOrder.ServiceOrderStatusId != Convert.ToInt32(ServiceOrderStatus.Success)
                        select new
                        {
                            CustomerName = serviceOrder.Customer.Profile != null ? serviceOrder.Customer.Profile.Name : serviceOrder.Customer.LegalEntityProfile.CompanyName,
                            ServiceOrderNumber = serviceOrder.ServiceOrderNumber,
                            ServiceOrderStatusName = serviceOrder.ServiceOrderStatus.Name,
                            OpenedDate = serviceOrder.OpenedDate,
                            ServiceOrderId = serviceOrder.ServiceOrderId
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "OpenedDate Desc");
        }

        public Int32 GetOpenServiceOrdersCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetOpenServiceOrders(companyId, sortExpression, startRowIndex, maximumRows).Cast<object>().Count();
        }


        /// <summary>
        ///this method return the total of serviceOrders
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetServiceOrdersCount(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetServiceOrders(companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }



        /// <summary>
        /// this method return a service order
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
#warning este método deve procurar usando companyId
        public ServiceOrder GetServiceOrder(Int32 serviceOrderId)
        {
            return DbContext.ServiceOrders.Where(x => x.ServiceOrderId == serviceOrderId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a serviceOrder by budget
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <param name="budgetId">can't be null</param>
        /// <returns></returns>
        public ServiceOrder GetServiceOrder(Int32 companyId, Int32 budgetId)
        {
            return DbContext.ServiceOrders.Where(serviceOrder => serviceOrder.CompanyId == companyId && serviceOrder.BudgetId == budgetId).FirstOrDefault();
        }


        /// <summary>
        /// this method checks the serviceOrder(closed,opened)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ServiceOrder checkClosedServiceOrder(ServiceOrder entity)
        {
            if (entity.ServiceOrderStatusId == (int)ServiceOrderStatus.Success)
                entity.ClosedDate = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// this method retrieves the serviceOrder by ServiceOrderNumber
        /// </summary>
        /// <param name="serviceOrderNumber"></param>
        /// <returns></returns>
        public ServiceOrder GetServiceOrderByServiceOrderNumber(Int32 companyId, string serviceOrderNumber)
        {
            return
                DbContext.ServiceOrders.Where(
                    s => s.CompanyId == companyId && s.ServiceOrderNumber == serviceOrderNumber).FirstOrDefault();
        }

        #endregion

        //this region contains all functions of ServiceOrderBook

        #region ServiceOrderBook

        /// <summary>
        /// This method deletes a specific serviceOrderBook
        /// </summary>
        /// <param name="serviceOrderBook"></param>
        public void DeleteServiceOrderBook(ServiceOrderBook serviceOrderBook)
        {
            DbContext.ServiceOrderBooks.DeleteOnSubmit(serviceOrderBook);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method retrieve a specific serviceOrderBook
        /// </summary>
        /// <param name="serviceOrderBookId"></param>
        /// <returns></returns>
        public ServiceOrderBook GetServiceOrderBook(Int32 serviceOrderBookId)
        {
            return DbContext.ServiceOrderBooks.Where(serviceOrderBook => serviceOrderBook.ServiceOrderBookId == serviceOrderBookId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns several ServiceOrderBooks from db
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <param name="representantId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderBook> GetServiceOrderBooks(Int32 companyId, Int32 employeeId, Int32? representantId)
        {
            var query = DbContext.ServiceOrderBooks.Where(serviceOrderBook => serviceOrderBook.CompanyId == companyId && serviceOrderBook.EmployeeId == employeeId);

            if (representantId.HasValue)
                query = query.Where(serviceOrderBook => serviceOrderBook.RepresentantId == representantId);

            return query;
        }

        #endregion

        //this region contains all function of ServiceOrderItem

        #region ServiceOrderItem

        /// <summary>
        /// this method insert a ServiceOrderItem
        /// </summary>
        /// <param name="entity"></param>
        public void InsertServiceOrderItem(ServiceOrderItem entity)
        {
            DbContext.ServiceOrderItems.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        public void InsertServiceOrderItems(IList<ServiceOrderItem> serviceOrderItems)
        {
            DbContext.ServiceOrderItems.InsertAllOnSubmit(serviceOrderItems);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete all ServiceOrderItem by ServiceOrder
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteAllServiceOrderItems(ServiceOrder entity)
        {
            DbContext.ServiceOrderItems.DeleteAllOnSubmit(GetServiceOrderItems(entity.ServiceOrderId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return all ServiceOrderItem by ServiceOrder
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderItem> GetServiceOrderItems(Int32 serviceOrderId)
        {
            return DbContext.ServiceOrderItems.Where(x => x.ServiceOrderId == serviceOrderId);
        }

        /// <summary>
        /// this method return all ServiceOrderItem by ServiceOrder as List
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        public List<ServiceOrderItem> GetServiceOrderItemsAsList(Int32 serviceOrderId)
        {
            return GetServiceOrderItems(serviceOrderId).ToList();
        }

        /// <summary>
        /// return the products of serviceOrder
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        /// 
        public DataTable GetServiceOrderProductsAsDataTable(Int32 serviceOrderId, Int32 depositId)
        {
            IQueryable<ServiceOrderItem> queryServiceOrderItems = GetServiceOrderItems(serviceOrderId);
            var query = from serviceOrderItems in queryServiceOrderItems
                        join inventory in DbContext.Inventories on serviceOrderItems.ProductId equals
                            inventory.ProductId
                        where inventory.DepositId == depositId
                        select new
                        {
                            companyId = serviceOrderItems.CompanyId,
                            productId = inventory.ProductId,
                            productName = inventory.Product.Name,
                            productPrice = inventory.UnitPrice,
                            serviceOrderDescription = serviceOrderItems.Description,
                            serviceOrderItems.IsApplied,
                        };
            return query.ToDataTable();
        }

        /// <summary>
        /// this method return all services of serviceOrder
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        public DataTable GetServiceOrderServicesAsDataTable(Int32 serviceOrderId)
        {
            IQueryable<ServiceOrderItem> queryServiceOrderItems = GetServiceOrderItems(serviceOrderId);
            var query = from serviceOrderItems in queryServiceOrderItems
                        join service in DbContext.Services on serviceOrderItems.ServiceId equals service.ServiceId
                        select new
                        {
                            serviceId = service.ServiceId,
                            companyId = serviceOrderItems.CompanyId,
                            name = service.Name,
                            servicePrice = service.Price,
                            timeInMinutes = service.TimeInMinutes,
                            serviceOrderDescription = serviceOrderItems.Description,
                            serviceOrderItems.EmployeeId,
                            employeeName = serviceOrderItems.Employee != null
                                               ? serviceOrderItems.Employee.Profile.Name
                                               : null
                        };
            return query.ToDataTable();
        }

        #endregion

        #region Auxiliar

        /// <summary>
        /// this method retrieves the serviceOrder tests
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderTest> GetServiceOrderTest(Int32 companyId)
        {
            return DbContext.ServiceOrderTests.Where(t => t.CompanyId == companyId);
        }

        /// <summary>
        /// this is the GetServiceOrderTest count method
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetServiceOrderTestCount(Int32 companyId)
        {
            return GetServiceOrderTest(companyId).Count();
        }

        /// <summary>
        /// this method retrieves the ServiceOrderEquipmentDamages
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderEquipmentDamage> GetServiceOrderEquipmentDamages(Int32 companyId)
        {
            return DbContext.ServiceOrderEquipmentDamages.Where(s => s.CompanyId == companyId);
        }

        /// <summary>
        /// this is the method count of GetServiceOrderEquipmentDamages
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetServiceOrderEquipmentDamagesCount(Int32 companyId)
        {
            return GetServiceOrderEquipmentDamages(companyId).Count();
        }

        /// <summary>
        /// this method return the ServiceOrderProductDamages
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderProductDamage> GetServiceOrderProductDamages(Int32 companyId)
        {
            return DbContext.ServiceOrderProductDamages.Where(p => p.CompanyId == companyId);
        }

        /// <summary>
        /// this is the count method of GetServiceOrderProductDamagesCount
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetServiceOrderProductDamagesCount(Int32 companyId)
        {
            return GetServiceOrderProductDamages(companyId).Count();
        }

        /// <summary>
        /// this method return the ServiceOrderHaltType
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderHaltType> GetServiceOrderHaltTypes(Int32 companyId)
        {
            return DbContext.ServiceOrderHaltTypes.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// this method return the ServiceOrderInstallType
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderInstallType> GetServiceOrderInstallTypes(Int32 companyId)
        {
            return DbContext.ServiceOrderInstallTypes.Where(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// this method return the ServiceOrderProductType
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceOrderProductType> GetServiceOrderProductsType(Int32 companyId)
        {
            return DbContext.ServiceOrderProductTypes.Where(x => x.CompanyId == companyId);
        }

        public IQueryable<ServiceType> GetServiceTypes()
        {
            return DbContext.ServiceTypes;
        }


        /// <summary>
        /// This method returns the serviceTypes of specific company 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ServiceType> GetServiceTypes(Int32 companyId)
        {
            return GetServiceTypes().Where(serviceType => serviceType.CompanyId == companyId);
        }

        #endregion
    }
}