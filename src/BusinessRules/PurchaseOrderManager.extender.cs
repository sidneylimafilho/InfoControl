using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfoControl;
using InfoControl.Data;
using InfoControl.Security.Cryptography;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public enum DocumentTemplateTypes
    {
        ProspectOrBudget = 1,
        ServiceOrder,
        PurchaseOrder,
        Contract,
        SupplyAuthorization,
        Invoice
    }

    public enum PurchaseOrderDecision
    {
        LowUnitPrice = 1,
        LowTotalPrice,
        BestDeadline
    }

    public class PurchaseOrderManager : BusinessManager<InfoControlDataContext>
    {
        public PurchaseOrderManager(IDataAccessor container)
            : base(container)
        {
        }

        #region PurchaseRequests

        public IQueryable<PurchaseRequestItem> GetPurchaseRequestItems(int purchaseRequestId)
        {
            return DbContext.PurchaseRequestItems.Where(pr => pr.PurchaseRequestId == purchaseRequestId);
        }

        public PurchaseRequestItem GetPurchaseRequestItem(int purchaseRequestItemId)
        {
            return
                DbContext.PurchaseRequestItems.Where(pr => pr.PurchaseRequestItemId == purchaseRequestItemId).
                    FirstOrDefault();
        }

        public PurchaseRequest GetPurchaseRequest(int purchaseRequestId)
        {
            return DbContext.PurchaseRequests.Where(pr => pr.PurchaseRequestId == purchaseRequestId).FirstOrDefault();
        }

        #endregion

        #region PurchaseOrder

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<PurchaseOrder> GetAllPurchaseOrders()
        {
            return DbContext.PurchaseOrders;
        }

        /// <summary>
        /// This method returns all purchaseOrder by matrixId and purchaseStatus opens
        /// </summary>
        /// <param name="matrixId"></param>
        /// <returns></returns>
        public IQueryable<PurchaseOrder> GetPurchaseOrdersByMatrix(Int32 matrixId)
        {
            return DbContext.PurchaseOrders.Where(
                p => p.Company.MatrixId == matrixId &&
                     (p.PurchaseOrderStatusId == PurchaseOrderStatus.InProcess ||
                      p.PurchaseOrderStatusId == PurchaseOrderStatus.SentToSupplier ||
                      p.PurchaseOrderStatusId == PurchaseOrderStatus.Approved));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public IQueryable<PurchaseOrderQuotedItem> GetPurchaseOrdersItemsByPurchaseOrder(Int32 matrixId,
                                                                                         Int32 purchaseOrderId,
                                                                                         Int32 supplierId)
        {
            var supplierManager = new SupplierManager(this);
            Supplier supplier = supplierManager.GetSupplier(supplierId, matrixId);
            string supplierName = (supplier != null)
                                      ? (supplier.LegalEntityProfile != null)
                                            ? supplier.LegalEntityProfile.CompanyName
                                            : supplier.Profile.Name
                                      : "";

            IQueryable<PurchaseOrderQuotedItem> query =
                from purchaseOrderItem in DbContext.PurchaseOrderItems
                join productPackage in DbContext.ProductPackages on purchaseOrderItem.ProductPackageId equals
                    productPackage.ProductPackageId into gProdPack
                from productPackage in gProdPack.DefaultIfEmpty()
                join productManufacturer in DbContext.ProductManufacturers on purchaseOrderItem.ProductManufacturerId
                    equals productManufacturer.ProductManufacturerId into gProdManu
                from productManufacturer in gProdManu.DefaultIfEmpty()
                join quotationItem in DbContext.QuotationItems.Where(qi => qi.SupplierId == supplierId)
                    on purchaseOrderItem.PurchaseOrderItemId equals quotationItem.PurchaseOrderItemId into
                    gQuotationItem
                from quotationItem in gQuotationItem.DefaultIfEmpty()
                where
                    purchaseOrderItem.CompanyId.Equals(matrixId) &&
                    purchaseOrderItem.PurchaseOrderId.Equals(purchaseOrderId)
                select
                    new PurchaseOrderQuotedItem(
                    purchaseOrderItem.Product.Name + " - " + (productPackage.Name ?? "") + " - " +
                    (productManufacturer.Name ?? ""),
                    purchaseOrderItem.QuantityOrdered,
                    purchaseOrderItem.QuantityReceived ?? purchaseOrderItem.QuantityOrdered,
                    supplierName,
                    quotationItem != null
                        ? quotationItem.Price
                        : Decimal.Zero,
                    purchaseOrderItem.ProductId,
                    purchaseOrderItem.ProductPackageId.Value,
                    purchaseOrderItem.ProductManufacturerId,
                    0,
                    DateTime.MinValue,
                    purchaseOrderItem.PurchaseOrderItemId);

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public IQueryable<PurchaseOrderQuotedItem> GetLastQuotationValues(Int32 matrixId, int purchaseOrderId,
                                                                          int supplierId)
        {
            var supplierManager = new SupplierManager(this);
            Supplier supplier = supplierManager.GetSupplier(supplierId, matrixId);
            string supplierName = (supplier != null)
                                      ? (supplier.LegalEntityProfile != null)
                                            ? supplier.LegalEntityProfile.CompanyName
                                            : supplier.Profile.Name
                                      : "";

            IQueryable<decimal?> queryPrice = from quotationItem in DbContext.QuotationItems
                                              where quotationItem.SupplierId == supplierId
                                              orderby quotationItem.QuotationId descending
                                              select (decimal?)quotationItem.Price;

            IQueryable<PurchaseOrderQuotedItem> query =
                from purchaseOrderItem in
                    DbContext.PurchaseOrderItems.Where(pi => pi.PurchaseOrderId == purchaseOrderId)
                select
                    new PurchaseOrderQuotedItem(
                    purchaseOrderItem.Product.Name + " - " + (purchaseOrderItem.ProductPackage.Name ?? "") + " - " +
                    (purchaseOrderItem.ProductManufacturer.Name ?? ""),
                    purchaseOrderItem.QuantityOrdered,
                    purchaseOrderItem.QuantityReceived ?? purchaseOrderItem.QuantityOrdered,
                    supplierName,
                    (from quotationItem in DbContext.QuotationItems
                     join purchaseItem in DbContext.PurchaseOrderItems on quotationItem.PurchaseOrderItemId equals
                         purchaseItem.PurchaseOrderItemId
                     where quotationItem.SupplierId == supplierId &&
                           purchaseItem.ProductId == purchaseOrderItem.ProductId &&
                           purchaseItem.ProductPackageId == purchaseOrderItem.ProductPackageId &&
                           purchaseItem.ProductManufacturerId == purchaseOrderItem.ProductManufacturerId
                     orderby quotationItem.QuotationItemId descending
                     select (decimal?)quotationItem.Price).FirstOrDefault() ?? 0,
                    purchaseOrderItem.ProductId,
                    purchaseOrderItem.ProductPackageId.Value,
                    purchaseOrderItem.ProductManufacturerId,
                    0,
                    DateTime.MinValue,
                    purchaseOrderItem.PurchaseOrderItemId);

            return query;
        }

        /// <summary>
        /// This method gets record counts of all PurchaseOrders.
        /// Do not change this method.
        /// </summary>
        public int GetAllPurchaseOrdersCount()
        {
            return GetAllPurchaseOrders().Count();
        }

        /// <summary>
        /// this method return all PurchaseOrders by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private IQueryable<PurchaseOrder> GetPurchaseOrdersByCompany(Int32 companyId, string sortExpression,
                                                                     int startRowIndex, int maximumRows)
        {
            IQueryable<PurchaseOrder> query = from purchaseOrder in DbContext.PurchaseOrders
                                              where purchaseOrder.CompanyId == companyId
                                              select purchaseOrder;
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "PurchaseOrderId");
        }

        /// <summary>
        /// this method return the total rows of PurchaseOrderByCompany
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// 
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetPurchaseOrdersByCompanyCount(Int32 companyId, string sortExpression, int startRowIndex,
                                                     int maximumRows)
        {
            return GetPurchaseOrdersByCompany(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method return the Total of PurchaseOrder by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Int32 GetPurchaseOrdersByCompanyCount(Int32 companyId)
        {
            return DbContext.PurchaseOrders.Where(p => p.CompanyId == companyId).Count();
        }

        /// <summary>
        /// This method retrieves a single PurchaseOrder.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=BudgetId>PurchaseOrderId</param>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=CustomerId>CustomerId</param>
        public PurchaseOrder GetPurchaseOrder(Int32 PurchaseOrderId, Int32 CompanyId, Int32 SupplierId)
        {
            return
                DbContext.PurchaseOrders.Where(
                    x => x.PurchaseOrderId == PurchaseOrderId && x.CompanyId == CompanyId && x.SupplierId == SupplierId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves PurchaseOrder by Customer.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<PurchaseOrder> GetPurchaseOrderByCustomer(Int32 SupplierId, Int32 CompanyId)
        {
            return DbContext.PurchaseOrders.Where(x => x.SupplierId == SupplierId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all PurchaseOrders filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetPurchaseOrders(string tableName, Int32 Customer_CustomerId, Int32 Customer_CompanyId,
                                       string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<PurchaseOrder> x = GetFilteredPurchaseOrders(tableName, Customer_CustomerId, Customer_CompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "PurchaseOrderId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<PurchaseOrder> GetFilteredPurchaseOrders(string tableName, Int32 Customer_CustomerId,
                                                                    Int32 Customer_CompanyId)
        {
            switch (tableName)
            {
                case "Customer_PurchaseOrders":
                    return GetPurchaseOrderByCustomer(Customer_CustomerId, Customer_CompanyId);
                default:
                    return GetAllPurchaseOrders();
            }
        }

        /// <summary>
        /// This method gets records counts of all PurchaseOrders filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetPurchaseOrdersCount(string tableName, Int32 Customer_CustomerId, Int32 Customer_CompanyId)
        {
            IQueryable<PurchaseOrder> x = GetFilteredPurchaseOrders(tableName, Customer_CustomerId, Customer_CompanyId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(PurchaseOrder entity)
        {
            entity.PurchaseOrderStatusId = PurchaseOrderStatus.Reproved;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(PurchaseOrder entity, List<PurchaseOrderQuotedItem> purchaseOrderItems, int userId)
        {
            SetPurchaseOrderStatus(entity, PurchaseOrderStatus.InProcess, userId);

            DbContext.PurchaseOrders.InsertOnSubmit(entity);
            DbContext.SubmitChanges();

            SavePurchaseOrderItems(entity, purchaseOrderItems);
        }

        private void SavePurchaseOrderItems(PurchaseOrder entity,
                                            IEnumerable<PurchaseOrderQuotedItem> purchaseOrderItems)
        {
            //insert the new Items
            foreach (PurchaseOrderQuotedItem item in purchaseOrderItems)
            {
                var purchaseOrderItem = new PurchaseOrderItem
                                            {
                                                CompanyId = entity.CompanyId,
                                                PurchaseOrderId = entity.PurchaseOrderId,
                                                ProductId = item.ProductId,
                                                ModifiedDate = DateTime.Now,
                                                QuantityOrdered = item.Quantity,
                                                ProductManufacturerId = item.ProductManufacturerId,
                                                ProductPackageId = item.ProductPackageId
                                            };

                InsertPurchaseOrderItem(purchaseOrderItem);

                //
                // Set the purchaseOrder in request, indicates what purchaseOrder attend this request
                //
                PurchaseRequestItem requestItem = GetPurchaseRequestItem(item.PurchaseRequestItemId);
                if (requestItem != null)
                {
                    requestItem.PurchaseOrderId = entity.PurchaseOrderId;
                    requestItem.PurchaseOrderItemId = purchaseOrderItem.PurchaseOrderItemId;
                    DbContext.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(PurchaseOrder entity,
                           List<PurchaseOrderQuotedItem> purchaseOrderItem, int userId)
        {
            PurchaseOrder original_entity = GetPurchaseOrder(entity.PurchaseOrderId, entity.CompanyId);

            if (original_entity.PurchaseOrderStatusId != entity.PurchaseOrderStatusId)
                SetPurchaseOrderStatus(original_entity, entity.PurchaseOrderStatusId, userId);

            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = entity.ModifiedDate;
            DbContext.SubmitChanges();

            //SavePurchaseOrderItems(original_entity, purchaseOrderItem);
        }

        /// <summary>
        /// This method updates a PurchaseOrder
        /// </summary>
        /// <param name="original_entity">original_entity</param>
        /// <param name="entity">entity</param>
        public void Update(PurchaseOrder original_entity, PurchaseOrder entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = entity.ModifiedDate;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns a single PurchaseOrder from the database
        /// </summary>
        /// <param name="PurchaseOrderCode">The code defined by the user</param>
        /// <returns></returns>
        public PurchaseOrder GetPurchaseOrderByPurchaseOrderCode(string purchaseOrderCode)
        {
            return DbContext.PurchaseOrders.Where(x => x.PurchaseOrderCode == purchaseOrderCode).FirstOrDefault();
        }

        /// <summary>
        /// This method returns a single PurchaseOrder from the database
        /// </summary>
        /// <param name="PurchaseOrderId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public PurchaseOrder GetPurchaseOrder(Int32 purchaseOrderId, Int32 companyId)
        {
            return
                DbContext.PurchaseOrders.Where(x => x.PurchaseOrderId == purchaseOrderId && x.CompanyId == companyId).
                    FirstOrDefault();
        }

        /// <summary>
        /// Delete from both PurchaseOrderItems and PurchaseOrder a line of the database based on the PurchaseOrderId
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="companyId"></param>
        public void DeletePurchaseOrder(int purchaseOrderId)
        {
            foreach (
                PurchaseRequestItem item in
                    DbContext.PurchaseRequestItems.Where(x => x.PurchaseOrderId == purchaseOrderId))
            {
                item.PurchaseOrderId = null;
                item.PurchaseOrderItemId = null;
                DbContext.SubmitChanges();
            }

            DbContext.QuotationItems.DeleteAllOnSubmit(
                DbContext.QuotationItems.Where(qi => qi.PurchaseOrderId == purchaseOrderId));
            DbContext.SubmitChanges();

            DbContext.Quotations.DeleteAllOnSubmit(
                DbContext.Quotations.Where(p => p.PurchaseOrderId == purchaseOrderId));
            DbContext.SubmitChanges();

            DbContext.PurchaseOrderItems.DeleteAllOnSubmit(
                DbContext.PurchaseOrderItems.Where(p => p.PurchaseOrderId == purchaseOrderId));
            DbContext.SubmitChanges();

            DbContext.PurchaseOrders.DeleteAllOnSubmit(
                DbContext.PurchaseOrders.Where(p => p.PurchaseOrderId == purchaseOrderId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns returns all PurchaseOrders in the database that are exclusively PurchaseOrders
        /// in other words, that have not became a sale
        /// </summary>
        /// <param name="SupplierId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private DataTable GetPurchaseOrdersBySupplier(Int32 supplierId, Int32 companyId)
        {
            string query =
                @" SELECT *
                                FROM         PurchaseOrder AS B
                                WHERE     (SupplierId = @supplierId) AND (CompanyId = @companyId) AND (NOT EXISTS
                                    (SELECT     PurchaseOrderId
                                    FROM          Sale
                                    WHERE      (PurchaseOrderId = B.PurchaseOrderId)))";

            DataManager.Parameters.Add("@supplierId", supplierId);
            DataManager.Parameters.Add("@companyId", companyId);
            return DataManager.ExecuteDataTable(query);
        }

        /// <summary>
        /// Convert each PurchaseOrderItem in Inventory Entry
        /// </summary>
        /// <param name="purchaseOrder"></param>
        private void SendPurchaseOrderToInventory(PurchaseOrder purchaseOrder)
        {
            var inventoryManager = new InventoryManager(this);

            foreach (PurchaseOrderItem item in purchaseOrder.PurchaseOrderItems)
                foreach (PurchaseRequestItem reqItem in item.PurchaseRequestItems)
                    if (reqItem.PurchaseRequest.DepositId.HasValue)
                    {
                        var inventory = new Inventory
                                            {
                                                CompanyId = reqItem.CompanyId.Value,
                                                CurrencyRateId = 1,
                                                DepositId = reqItem.PurchaseRequest.DepositId.Value,
                                                EntryDate = DateTime.Now,
                                                ProductId = item.ProductId,
                                                Quantity = Convert.ToInt32(reqItem.Amount.Value),
                                                RealCost =
                                                    item.WinnerQuotationItem.Price * Convert.ToInt32(reqItem.Amount.Value),
                                                SupplierId = purchaseOrder.SupplierId,
                                                UnitPrice =
                                                    item.WinnerQuotationItem.Price * Convert.ToInt32(reqItem.Amount.Value)
                                            };
                        inventoryManager.StockDeposit(inventory, purchaseOrder.SupplierId,
                                                      purchaseOrder.ReceiverUserId.Value);
                    }
        }

        #endregion

        #region PurchaseOrderItem

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PurchaseOrderItem GetPurchaseOrderItem(int id)
        {
            return DbContext.PurchaseOrderItems.Where(poi => poi.PurchaseOrderItemId == id).FirstOrDefault();
        }

        /// <summary>
        /// this method return all purchaseOrderItems by PurchaseOrder(PurchaseOrderId)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<PurchaseOrderItem> GetPurchaseOrderItems(Int32 companyId, Int32 purchaseOrderId,
                                                                   string sortExpression, int startRowIndex,
                                                                   int maximumRows)
        {
            IQueryable<PurchaseOrderItem> query = from purchaseOrderItem in DbContext.PurchaseOrderItems
                                                  where
                                                      purchaseOrderItem.CompanyId == companyId &&
                                                      purchaseOrderItem.PurchaseOrderId == purchaseOrderId
                                                  select purchaseOrderItem;
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "PurchaseOrderItemId");
        }

        /// <summary>
        /// this method return the total rows of purchaseOrderItems
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetPurchaseOrderItemsCount(Int32 companyId, Int32 purchaseOrderId, string sortExpression,
                                                int startRowIndex, int maximumRows)
        {
            return GetPurchaseOrderItems(companyId, purchaseOrderId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void InsertPurchaseOrderItem(PurchaseOrderItem entity)
        {
            entity.ModifiedDate = DateTime.Now.Date;
            DbContext.PurchaseOrderItems.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name="entity"></param>
        public void DeletePurchaseOrderItem(int purchaseOrderItemId)
        {
            List<PurchaseRequestItem> list =
                DbContext.PurchaseRequestItems.Where(r => r.PurchaseOrderItemId == purchaseOrderItemId).ToList();
            foreach (PurchaseRequestItem reqItem in list)
            {
                reqItem.PurchaseOrderItemId = null;
                reqItem.PurchaseOrderId = null;
                DbContext.SubmitChanges();
            }

            DbContext.PurchaseOrderItems.DeleteAllOnSubmit(
                DbContext.PurchaseOrderItems.Where(p => p.PurchaseOrderItemId == purchaseOrderItemId));
            DbContext.SubmitChanges();
        }

        public void UpdatePurchaseOrderItem(PurchaseOrderItem original_entity, PurchaseOrderItem entity)
        {
            DbContext.PurchaseOrderItems.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region Quotation

        public IQueryable<Quotation> GetQuotationsByPurchaseOrder(Int32 companyId, Int32 purchaseOrderId)
        {
            //
            // All Quotations by PurchaseOrder
            //
            IQueryable<Quotation> query = from quotation in DbContext.Quotations
                                          where quotation.CompanyId == companyId &&
                                                quotation.PurchaseOrderId == purchaseOrderId
                                          select quotation;

            return query;
        }

        public IQueryable GetSuppliersByPurchaseOrder(Int32 companyId, Int32 purchaseOrderId)
        {
            //
            // All Quotations by PurchaseOrder
            //
            var query = from quotation in DbContext.Quotations
                        where quotation.CompanyId == companyId &&
                              quotation.PurchaseOrderId == purchaseOrderId
                        select new
                                   {
                                       Name = (quotation.Supplier.LegalEntityProfile != null)
                                                  ? quotation.Supplier.LegalEntityProfile.CompanyName
                                                  : quotation.Supplier.Profile.Name,
                                       quotation.SupplierId
                                   };

            return query;
        }

        public List<PurchaseOrderQuotedItem> GetPurchaseOrderQuotedItems(Int32 companyID, Int32 purchaseOrderID,
                                                                         PurchaseOrderDecision decision)
        {
            switch (decision)
            {
                case PurchaseOrderDecision.LowUnitPrice:
                    return GetPurchaseOrderQuotedItemWithLowUnitPrice(companyID, purchaseOrderID);

                case PurchaseOrderDecision.LowTotalPrice:
                    return GetPurchaseOrderQuotedItemWithLowTotalPrice(companyID, purchaseOrderID);

                case PurchaseOrderDecision.BestDeadline:
                    return GetPurchaseOrderQuotedItemWithBestDeadline(companyID, purchaseOrderID);
                default:
                    return null;
            }
        }

        public IQueryable<QuotationItem> GetQuotationItems(Int32 companyID, Int32 quotationID)
        {
            return DbContext.QuotationItems.Where(item => item.CompanyId == companyID && item.QuotationId == quotationID);
        }

        private List<PurchaseOrderQuotedItem> GetPurchaseOrderQuotedItemWithBestDeadline(Int32 companyId,
                                                                                         Int32 purchaseOrderId)
        {
            var query =
                from purchaseOrderItem in
                    DbContext.PurchaseOrderItems.Where(pi => pi.PurchaseOrderId == purchaseOrderId)
                join productPackage in DbContext.ProductPackages on purchaseOrderItem.ProductPackageId equals
                    productPackage.ProductPackageId into gProdPack
                from productPackage in gProdPack.DefaultIfEmpty()
                join productManufacturer in DbContext.ProductManufacturers on purchaseOrderItem.ProductManufacturerId
                    equals productManufacturer.ProductManufacturerId into gProdManu
                from productManufacturer in gProdManu.DefaultIfEmpty()
                join product in DbContext.Products on purchaseOrderItem.ProductId equals product.ProductId
                select new
                           {
                               purchaseOrderItem.PurchaseOrderItemId,
                               Name =
                    product.Name + " - " + (productPackage.Name ?? "") + " - " + (productManufacturer.Name ?? ""),
                               purchaseOrderItem.ProductId,
                               ProductPackageId = purchaseOrderItem.ProductPackageId.Value,
                               purchaseOrderItem.ProductManufacturerId,
                               purchaseOrderItem.QuantityOrdered,
                               purchaseOrderItem.QuantityReceived,
                               purchaseOrderItem.QuotationItems
                           };

            var list = new List<PurchaseOrderQuotedItem>();
            foreach (var item in query)
            {
                var quotationItem = (from quotaItem in item.QuotationItems
                                     join quotation in DbContext.Quotations on quotaItem.QuotationId equals
                                         quotation.QuotationId into gQuotation
                                     from quotation in gQuotation.DefaultIfEmpty()
                                     join supplier in DbContext.Suppliers on quotation.SupplierId equals
                                         supplier.SupplierId into gSupplier
                                     from supplier in gSupplier.DefaultIfEmpty()
                                     orderby quotation.DeliveryDate
                                     select new
                                                {
                                                    SupplierName = supplier.LegalEntityProfile == null
                                                                       ? supplier.Profile.Name
                                                                       : supplier.LegalEntityProfile.CompanyName,
                                                    quotaItem.Price,
                                                    quotation.DeliveryDate
                                                }).FirstOrDefault();

                list.Add(new PurchaseOrderQuotedItem(item.Name,
                                                     item.QuantityOrdered,
                                                     item.QuantityReceived ?? item.QuantityOrdered,
                                                     quotationItem != null
                                                         ? quotationItem.SupplierName
                                                         : "",
                                                     quotationItem != null
                                                         ? quotationItem.Price
                                                         : Decimal.Zero,
                                                     item.ProductId,
                                                     item.ProductPackageId,
                                                     item.ProductManufacturerId,
                                                     0,
                                                     quotationItem != null
                                                         ? quotationItem.DeliveryDate
                                                         : DateTime.MinValue,
                                                     item.PurchaseOrderItemId));
            }

            return list;
        }

        /// <summary>
        /// This method reurns the PurchaseOrderQuotedItems with lowUnitPrice
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        private List<PurchaseOrderQuotedItem> GetPurchaseOrderQuotedItemWithLowUnitPrice(Int32 companyId,
                                                                                         Int32 purchaseOrderId)
        {
            var query =
                from purchaseOrderItem in
                    DbContext.PurchaseOrderItems.Where(pi => pi.PurchaseOrderId == purchaseOrderId)
                join productPackage in DbContext.ProductPackages on purchaseOrderItem.ProductPackageId equals
                    productPackage.ProductPackageId into gProdPack
                from productPackage in gProdPack.DefaultIfEmpty()
                join productManufacturer in DbContext.ProductManufacturers on purchaseOrderItem.ProductManufacturerId
                    equals productManufacturer.ProductManufacturerId into gProdManu
                from productManufacturer in gProdManu.DefaultIfEmpty()
                join product in DbContext.Products on purchaseOrderItem.ProductId equals product.ProductId
                select new
                           {
                               purchaseOrderItem.PurchaseOrderItemId,
                               Name =
                    product.Name + " - " + (productPackage.Name ?? "") + " - " + (productManufacturer.Name ?? ""),
                               purchaseOrderItem.ProductId,
                               ProductPackageId = purchaseOrderItem.ProductPackageId.Value,
                               purchaseOrderItem.ProductManufacturerId,
                               purchaseOrderItem.QuantityOrdered,
                               purchaseOrderItem.QuantityReceived,
                               purchaseOrderItem.QuotationItems
                           };

            var list = new List<PurchaseOrderQuotedItem>();
            foreach (var item in query)
            {
                var quotationItem = (from quotaItem in item.QuotationItems
                                     join quotation in DbContext.Quotations on quotaItem.QuotationId equals
                                         quotation.QuotationId into gQuotation
                                     from quotation in gQuotation.DefaultIfEmpty()
                                     join supplier in DbContext.Suppliers on quotation.SupplierId equals
                                         supplier.SupplierId into gSupplier
                                     from supplier in gSupplier.DefaultIfEmpty()
                                     orderby quotaItem.Price
                                     select new
                                                {
                                                    SupplierName = supplier.LegalEntityProfile == null
                                                                       ? supplier.Profile.Name
                                                                       : supplier.LegalEntityProfile.CompanyName,
                                                    quotaItem.Price,
                                                    quotation.DeliveryDate
                                                }).FirstOrDefault();

                list.Add(new PurchaseOrderQuotedItem(item.Name,
                                                     item.QuantityOrdered,
                                                     item.QuantityReceived ?? item.QuantityOrdered,
                                                     quotationItem != null
                                                         ? quotationItem.SupplierName
                                                         : "",
                                                     quotationItem != null
                                                         ? quotationItem.Price
                                                         : Decimal.Zero,
                                                     item.ProductId,
                                                     item.ProductPackageId,
                                                     item.ProductManufacturerId,
                                                     0,
                                                     quotationItem != null
                                                         ? quotationItem.DeliveryDate
                                                         : DateTime.MinValue,
                                                     item.PurchaseOrderItemId));
            }

            return list;
        }

        /// <summary>
        /// This method returns the PurchaseOrderQuotedItems from the cheapest supplier
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        private List<PurchaseOrderQuotedItem> GetPurchaseOrderQuotedItemWithLowTotalPrice(Int32 companyId,
                                                                                          Int32 purchaseOrderId)
        {
            IEnumerable<PurchaseOrderQuotedItem> query =
                from purchaseOrderItem in
                    DbContext.PurchaseOrderItems.Where(pi => pi.PurchaseOrderId == purchaseOrderId)
                join productPackage in DbContext.ProductPackages on purchaseOrderItem.ProductPackageId equals
                    productPackage.ProductPackageId into gProdPack
                from productPackage in gProdPack.DefaultIfEmpty()
                join productManufacturer in DbContext.ProductManufacturers on purchaseOrderItem.ProductManufacturerId
                    equals productManufacturer.ProductManufacturerId into gProdManu
                from productManufacturer in gProdManu.DefaultIfEmpty()
                join quotationItem in DbContext.QuotationItems on purchaseOrderItem.PurchaseOrderItemId equals
                    quotationItem.PurchaseOrderItemId into gQuotItem
                from quotationItem in gQuotItem.DefaultIfEmpty()
                join quotation in
                    from quotationMin in DbContext.Quotations
                    where (from quotationMins in DbContext.Quotations.Where(pi => pi.PurchaseOrderId == purchaseOrderId)
                           group quotationMins by quotationMins.QuotationId
                               into g2
                               select new
                                          {
                                              QuotationId = g2.Key,
                                              Price = g2.Min(i => i.TotalPrice)
                                          })
                        .Any(k => k.Price == quotationMin.TotalPrice)
                    select quotationMin
                    on purchaseOrderItem.PurchaseOrderId equals quotation.PurchaseOrderId into gQuotation
                from quotation in gQuotation.DefaultIfEmpty()
                join supplier in DbContext.Suppliers on quotation.SupplierId equals supplier.SupplierId into gSupplier
                from supplier in gSupplier.DefaultIfEmpty()
                join product in DbContext.Products on purchaseOrderItem.ProductId equals product.ProductId
                select
                    new PurchaseOrderQuotedItem(
                    product.Name + " - " + (productPackage.Name ?? "") + " - " + (productManufacturer.Name ?? ""),
                    purchaseOrderItem.QuantityOrdered,
                    purchaseOrderItem.QuantityReceived ?? purchaseOrderItem.QuantityOrdered,
                    supplier.LegalEntityProfile == null
                        ? supplier.Profile.Name
                        : supplier.LegalEntityProfile.CompanyName,
                    quotation.TotalPrice.Value,
                    purchaseOrderItem.ProductId, purchaseOrderItem.ProductPackageId.Value,
                    purchaseOrderItem.ProductManufacturerId,
                    0,
                    quotation.DeliveryDate,
                    purchaseOrderItem.PurchaseOrderItemId);
            return query.ToList();
        }

        /// <summary>
        /// This method retrieves the PurchaseOrderItem using its productID and its PurchaseOrderId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        private PurchaseOrderItem GetPurchaseOrderItem(Int32 companyID, Int32 productId, Int32 purchaseOrderId)
        {
            return
                DbContext.PurchaseOrderItems.Where(
                    purchaseOrderItem =>
                    purchaseOrderItem.CompanyId.Equals(companyID) &&
                    purchaseOrderItem.ProductId == productId &&
                    purchaseOrderItem.PurchaseOrderId == purchaseOrderId).FirstOrDefault();
        }

        private Employee GetEmployeeCompetent(Int32 companyID,
                                              OrganizationLevel organizationLevel,
                                              Decimal amount)
        {
            if (organizationLevel == null)
                return null;
                       
            var humanResourcesManager = new HumanResourcesManager(this);

            foreach (Employee item in humanResourcesManager.GetEmployeesByOrganizationLevel(companyID,
                                                                                      organizationLevel.
                                                                                          OrganizationlevelId))
            {
                if (CalculeAvailableCompetencyValue(item.CompanyId, item.EmployeeId) > amount)
                    return item;
            }
            if (organizationLevel.Parentid.HasValue)
                return GetEmployeeCompetent(
                    companyID,
                    humanResourcesManager.GetOrganizationLevel(
                        companyID,
                        Convert.ToInt32(organizationLevel.Parentid)),
                    amount);

            return null;
        }

        private void SetPurchaseOrderStatus(PurchaseOrder purchaseOrder, int statusId, int userId)
        {
            purchaseOrder.PurchaseOrderStatusId = statusId;

            switch (statusId)
            {
                case PurchaseOrderStatus.InProcess:
                    purchaseOrder.CreatedDate = DateTime.Now;
                    purchaseOrder.CreatorUserId = userId;
                    break;

                case PurchaseOrderStatus.Approved:
                    purchaseOrder.ApprovedDate = DateTime.Now;
                    purchaseOrder.ApproverUserId = userId;
                    break;
                case PurchaseOrderStatus.Bought:
                    purchaseOrder.BoughtDate = DateTime.Now;
                    purchaseOrder.BuyerUserId = userId;
                    break;
                case PurchaseOrderStatus.Concluded:
                    purchaseOrder.ReceivedDate = DateTime.Now;
                    purchaseOrder.ReceiverUserId = userId;

                    SendPurchaseOrderToInventory(purchaseOrder);

                    break;
                case PurchaseOrderStatus.Reproved:
                    break;
                case PurchaseOrderStatus.WaitingforApproval:
                    break;
                case PurchaseOrderStatus.SentToSupplier:
                    break;
            }

            DbContext.SubmitChanges();
        }

        private void ProcessEmployeeCompetencyValue(Int32 companyId, Employee employee, PurchaseOrder purchaseOrder,
                                                    IEnumerable<QuotationItem> quotationItems)
        {
            // Refresh PurchaseOrder
            purchaseOrder = GetPurchaseOrder(purchaseOrder.PurchaseOrderId, purchaseOrder.CompanyId);

            //Get Available employee
            Employee availableEmployee = GetEmployeeCompetent(companyId,
                                                              employee.OrganizationLevel,
                                                              quotationItems.Sum(qi => qi.Price));

            if (availableEmployee == null)
            {
                purchaseOrder.PurchaseOrderStatusId = PurchaseOrderStatus.SentToBuyerCentral;
                DbContext.SubmitChanges();

                var humanResourcesManager = new HumanResourcesManager(this);
                User user = humanResourcesManager.GetCentralBuyer(companyId);
                new AlertManager(this).InsertAlert(
                    user.UserId,
                    "Uma requisição de compra pendente, <a href='Purchasing/PurchaseOrder.aspx?pid=" +
                    purchaseOrder.PurchaseOrderId.ToString() + "' target='content'>clique aqui</a> para analisá-la!");

                return;
            }

            if (availableEmployee.EmployeeId == employee.EmployeeId)
            {
                purchaseOrder.PurchaseOrderStatusId = PurchaseOrderStatus.Approved;

                foreach (QuotationItem item in quotationItems)
                {
                    PurchaseOrderItem pItem = GetPurchaseOrderItem(item.PurchaseOrderItemId);
                    if (purchaseOrder.Quotations.Count() < 3 && pItem.ProductPackage.RequiresQuotationInPurchasing)
                    {
                        purchaseOrder.PurchaseOrderStatusId = PurchaseOrderStatus.SentToSupplier;
                        return;
                    }
                }
            }
            else
            {
                purchaseOrder.PurchaseOrderStatusId = PurchaseOrderStatus.WaitingforApproval;

                User user = new CompanyManager(this).GetUserByEmployee(availableEmployee.EmployeeId);

                //
                // User that can to approve Process Order
                //
                purchaseOrder.ApproverUserId = user.UserId;

                new AlertManager(this).InsertAlert(
                    user.UserId,
                    "Uma requisição de compra pendente, <a href='Purchasing/PurchaseOrder.aspx?pid=" +
                    purchaseOrder.PurchaseOrderId.ToString() + "' target='content'>clique aqui</a> para analisá-la!");
            }

            Update(purchaseOrder, purchaseOrder);
            return;
        }

        public void SaveQuotation(Quotation entity, IList<QuotationItem> quotationItems, Employee employee)
        {
            if (entity.QuotationId == 0)
                InsertQuotation(entity);

            foreach (QuotationItem item in quotationItems)
                if (item.QuotationId == 0)
                {
                    item.QuotationId = entity.QuotationId;
                    InsertQuotationItem(item);
                }
                else
                    UpdateQuotationItem(
                        GetQuotationItem(item.CompanyId, entity.SupplierId, item.PurchaseOrderItemId,
                                         item.PurchaseOrderId),
                        item);

            ProcessEmployeeCompetencyValue(entity.CompanyId, employee,
                                           GetPurchaseOrder(entity.PurchaseOrderId, entity.CompanyId),
                                           quotationItems);
        }

        /// <summary>
        /// This method inserts a quotation
        /// </summary>
        /// <param name="entity"></param>
        private void InsertQuotation(Quotation entity)
        {
            DbContext.Quotations.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        public void InsertQuotationItem(QuotationItem entity)
        {
            DbContext.QuotationItems.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        public void UpdateQuotationItem(QuotationItem original_entity, QuotationItem entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        public QuotationItem GetQuotationItem(Int32 companyId, Int32 supplierId, Int32 purchaseOrderItemId,
                                              Int32 purchaseOrderId)
        {
            return (from qItem in DbContext.QuotationItems
                    where qItem.CompanyId == companyId && qItem.SupplierId == supplierId &&
                          qItem.PurchaseOrderId == purchaseOrderId && qItem.PurchaseOrderItemId == purchaseOrderItemId
                    select qItem).FirstOrDefault();
        }

        /// <summary>
        /// This method returns Available amount of a employee's competency value 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private Decimal CalculeAvailableCompetencyValue(Int32 companyId, Int32 employeeId)
        {
            Employee employee = new HumanResourcesManager(this).GetEmployee(companyId, employeeId);

            //PurchaseOrder
            IQueryable<PurchaseOrder> purchaseOrders = from purchaseOrder in DbContext.PurchaseOrders
                                                       where
                                                           purchaseOrder.CompanyId == companyId &&
                                                           purchaseOrder.EmployeeId == employeeId &&
                                                           purchaseOrder.PurchaseOrderStatusId ==
                                                           PurchaseOrderStatus.Concluded &&
                                                           purchaseOrder.ModifiedDate.Year.Equals(DateTime.Now.Year) &&
                                                           purchaseOrder.ModifiedDate.Month.Equals(DateTime.Now.Month)
                                                       select purchaseOrder;

            //Items
            IQueryable<decimal> quotationItemsValue = from purchaseOrder in purchaseOrders
                                                      join quotation in DbContext.Quotations on
                                                          purchaseOrder.PurchaseOrderId equals quotation.PurchaseOrderId
                                                      join quotationItem in DbContext.QuotationItems on
                                                          quotation.QuotationId equals quotationItem.QuotationId into
                                                          gQuotationItem
                                                      select gQuotationItem.Sum(x => x.Price);

            //Decimal? value = (from purchaseOrder in DbContext.PurchaseOrders
            //                  where purchaseOrder.CompanyId == companyID && purchaseOrder.EmployeeId == employeeId &&
            //                  purchaseOrder.PurchaseOrderStatusId == (Int32)PurchaseOrderStatus.Concluded && purchaseOrder.ModifiedDate.Year.Equals(DateTime.Now.Year) && purchaseOrder.ModifiedDate.Month.Equals(DateTime.Now.Month)
            //                  join quotation in DbContext.Quotations on purchaseOrder.PurchaseOrderId equals quotation.PurchaseOrderId
            //                  join quotationItem in DbContext.QuotationItems on quotation.QuotationId equals quotationItem.QuotationId into gQuotationItem
            //                  select Convert.ToDecimal(gQuotationItem.Sum(x => x.Price))).Sum();

            return Convert.ToDecimal(employee.PurchaseCeilingValue -
                                     Convert.ToDecimal(quotationItemsValue.ToList().Sum()));
        }

        public Quotation GetQuotation(Int32 purchaseOrderId, Int32 supplierId)
        {
            return
                DbContext.Quotations.Where(q => q.SupplierId == supplierId && q.PurchaseOrderId == purchaseOrderId).
                    FirstOrDefault();
        }

        public IQueryable<QuotationItem> GetQuotationsItens(Int32 quotationId)
        {
            return DbContext.QuotationItems.Where(q => q.QuotationId == quotationId);
        }

        #endregion

        #region PurchaseOrderStatus

        /// <summary>
        /// This method returns an Iqueryable of PurchaseOrderStatus
        /// </summary>
        /// <returns>an Iqueryable of PurchaseOrderStatus</returns>
        public IQueryable<PurchaseOrderStatus> GetPurchaseOrderStatus()
        {
            return DbContext.PurchaseOrderStatus;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="status"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetPurchaseOrdersByCompany(Int32 companyId, int status, string sortExpression,
                                                     int startRowIndex, int maximumRows)
        {
            IQueryable<PurchaseOrder> query =
                from purchaseOrder in DbContext.PurchaseOrders
                where purchaseOrder.CompanyId == companyId
                select purchaseOrder;

            if (status > 0)
                query = query.Where(purchaseOrder => purchaseOrder.PurchaseOrderStatusId == status);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "PurchaseOrderId");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="status"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetPurchaseOrdersByCompanyCount(Int32 companyId, int status, string sortExpression,
                                                     int startRowIndex, int maximumRows)
        {
            return
                GetPurchaseOrdersByCompany(companyId, status, sortExpression, startRowIndex, maximumRows).Cast<object>()
                    .Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="purchaseOrderId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataTable GetPurchaseOrderItemsWithQuotationBySupplier(Int32 companyId, Int32 purchaseOrderId,
                                                                      Int32 supplierId)
        {
            var query =
                from purchaseOrderItem in DbContext.PurchaseOrderItems
                join quotationItem in DbContext.QuotationItems on purchaseOrderItem.PurchaseOrderItemId equals
                    quotationItem.PurchaseOrderItemId into gQuotItem
                from quotationItem in gQuotItem.DefaultIfEmpty()
                where purchaseOrderItem.CompanyId == companyId &&
                      purchaseOrderItem.PurchaseOrderId == purchaseOrderId &&
                      quotationItem.SupplierId == supplierId
                select new
                           {
                               Description = purchaseOrderItem.Product.Name + " - " +
                                             purchaseOrderItem.ProductPackage.Name +
                                             (purchaseOrderItem.ProductManufacturer != null
                                                  ? (" - " + purchaseOrderItem.ProductManufacturer.Name)
                                                  : ""),
                               Quantity = purchaseOrderItem.QuantityOrdered,
                               purchaseOrderItem.Product.Unit,
                               UnitValue = (quotationItem != null ? quotationItem.Price : 0m),
                               TotalValue =
                    purchaseOrderItem.QuantityOrdered * (quotationItem != null ? quotationItem.Price : 0m)
                           };

            return query.ToDataTable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        //public String ApplyPurchaseOrderTemplate(PurchaseOrder purchaseOrder, String template)
        //{
        //    var stringBuilder = new StringBuilder(template);
        //    var stringBuilderAdditional = new StringBuilder();
        //    if (purchaseOrder != null)
        //    {
        //        stringBuilder.Replace("[NumeroDeProcessoDeCompra]", purchaseOrder.PurchaseOrderCode + " <br> ");
        //        stringBuilder.Replace("[DataEmissao]", purchaseOrder.ModifiedDate.ToShortDateString() + " <br> ");
        //        //Supplier
        //        stringBuilderAdditional.AppendLine("Dados do Fornecedor:<br>");
        //        if (purchaseOrder.SupplierId.HasValue && purchaseOrder.Supplier.LegalEntityProfileId.HasValue)
        //        {
        //            #region SupplierAddress
        //            stringBuilderAdditional.AppendLine("Nome:" + purchaseOrder.Supplier.LegalEntityProfile.CompanyName +
        //                                               "<br>");
        //            Address supplierAddress = purchaseOrder.Supplier.Address;
        //            if (supplierAddress != null)
        //            {
        //                stringBuilderAdditional.Append(supplierAddress.Name + "," + supplierAddress.SubName + "-" +
        //                                               supplierAddress.City);
        //                stringBuilderAdditional.Append("-" + supplierAddress.State + "-" + "CEP" +
        //                                               supplierAddress.PostalCode + "<br>");
        //            }
        //            #endregion
        //            #region SupplierContact
        //            Contact contact =
        //                new ContactManager(this).GetContactsBySupplier(purchaseOrder.CompanyId,
        //                                                               Convert.ToInt32(purchaseOrder.SupplierId)).
        //                    FirstOrDefault();
        //            if (contact != null)
        //            {
        //                stringBuilderAdditional.AppendLine("Fone:" + contact.Phone + "<br>");
        //                stringBuilderAdditional.AppendLine("Contato:" + contact.Name + "<br>");
        //                stringBuilderAdditional.AppendLine(contact.Email + "<br>");
        //            }
        //            else
        //            {
        //                stringBuilderAdditional.AppendLine("Fone: ----- " + "<br>");
        //                stringBuilderAdditional.AppendLine("Contato: -----" + "<br>");
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            stringBuilderAdditional.AppendLine("Indefinido" + "<br>");
        //        }
        //        stringBuilder.Replace("[Fornecedor]", stringBuilderAdditional.ToString());
        //        #region PurchaseOrderProducts
        //        stringBuilderAdditional = new StringBuilder();
        //        //Header
        //        stringBuilderAdditional.Append("<table width='100%'>");
        //        stringBuilderAdditional.Append("<tr>");
        //        stringBuilderAdditional.Append("<td>ITEM</td>");
        //        stringBuilderAdditional.Append("<td>UNID</td>");
        //        stringBuilderAdditional.Append("<td>QTD</td>");
        //        stringBuilderAdditional.Append("<td>DESCRIÇÂO</td>");
        //        stringBuilderAdditional.Append("<td>VLR UNIT</td>");
        //        stringBuilderAdditional.Append("<td>VLR TOTAL</td>");
        //        stringBuilderAdditional.Append("</tr>");
        //        //Body
        //        Int32 productCount = 0;
        //        Decimal totalValue = Decimal.Zero;
        //        foreach (
        //            DataRow item in
        //                GetPurchaseOrderItemsWithQuotationBySupplier(purchaseOrder.CompanyId,
        //                                                             purchaseOrder.PurchaseOrderId,
        //                                                             Convert.ToInt32(purchaseOrder.SupplierId)).Rows)
        //        {
        //            productCount++;
        //            stringBuilderAdditional.Append("<tr>");
        //            //
        //            stringBuilderAdditional.Append("<td>" + productCount + "</td>");
        //            stringBuilderAdditional.Append("<td>Pç</td>");
        //            stringBuilderAdditional.Append("<td>" + Convert.ToString(item["Quantity"]) + "</td>");
        //            stringBuilderAdditional.Append("<td>" + Convert.ToString(item["Description"]) + "</td>");
        //            stringBuilderAdditional.Append("<td>" + Convert.ToDecimal(item["UnitValue"]).ToString("c") + "</td>");
        //            stringBuilderAdditional.Append("<td>" + Convert.ToDecimal(item["TotalValue"]).ToString("c") +
        //                                           "</td>");
        //            totalValue += Convert.ToDecimal(item["TotalValue"]);
        //            //   
        //            stringBuilderAdditional.Append("</tr>");
        //        }
        //        //Footer
        //        stringBuilderAdditional.Append("<tr>");
        //        stringBuilderAdditional.Append("<td colspan='4'>Total<td>");
        //        stringBuilderAdditional.Append("<td>" + totalValue.ToString("c") + "<td>");
        //        stringBuilderAdditional.Append("</tr>");
        //        stringBuilderAdditional.Append("</table>");
        //        #endregion
        //        stringBuilder.Replace("[ITEMS]", stringBuilderAdditional.ToString());
        //        stringBuilder.Replace("[Usuario]", purchaseOrder.User2.Profile.Name);
        //        stringBuilder.Replace("[UsuarioEmail]", purchaseOrder.User2.UserName);
        //    }
        //    return stringBuilder.ToString();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public String ApplyPurchaseOrderInDocumentTemplate(PurchaseOrder purchaseOrder, String template)
        {
            var stringBuilder = new StringBuilder(template);

            if (purchaseOrder != null)
            {
                stringBuilder.Replace("[NumeroDeProcessoDeCompra]", purchaseOrder.PurchaseOrderCode + " <br> ");
                stringBuilder.Replace("[DataEmissao]", purchaseOrder.ModifiedDate.ToShortDateString() + " <br> ");

                var centerCostHt = new Hashtable();
                var addressHt = new Hashtable();
                foreach (PurchaseOrderItem poItem in purchaseOrder.PurchaseOrderItems)
                    foreach (PurchaseRequestItem reqItem in poItem.PurchaseRequestItems)
                    {
                        addressHt["EnderecoEntrega-CEP"] = reqItem.PurchaseRequest.Address.PostalCode;
                        addressHt["EnderecoEntrega-Rua"] = reqItem.PurchaseRequest.Address.Name;
                        addressHt["EnderecoEntrega-Numero"] = reqItem.PurchaseRequest.AddressNumber;
                        addressHt["EnderecoEntrega-Complemento"] = reqItem.PurchaseRequest.AddressComp;
                        addressHt["EnderecoEntrega-Bairro"] = reqItem.PurchaseRequest.Address.Neighborhood;
                        addressHt["EnderecoEntrega-Cidade"] = reqItem.PurchaseRequest.Address.City;
                        addressHt["EnderecoEntrega-Estado"] = reqItem.PurchaseRequest.Address.State;

                        centerCostHt[reqItem.PurchaseRequest.CostCenter.Name] = true;
                    }

                string centerCost = String.Join(", ", centerCostHt.Keys.Cast<string>().ToArray());

                stringBuilder.Replace("[CentroDeCusto]", centerCost);

                stringBuilder.Replace("[EnderecoEntrega-CEP]", Convert.ToString(addressHt["EnderecoEntrega-CEP"]));
                stringBuilder.Replace("[EnderecoEntrega-Rua]", Convert.ToString(addressHt["EnderecoEntrega-Rua"]));
                stringBuilder.Replace("[EnderecoEntrega-Numero]", Convert.ToString(addressHt["EnderecoEntrega-Numero"]));
                stringBuilder.Replace("[EnderecoEntrega-Complemento]",
                                      Convert.ToString(addressHt["EnderecoEntrega-Complemento"]));
                stringBuilder.Replace("[EnderecoEntrega-Bairro]", Convert.ToString(addressHt["EnderecoEntrega-Bairro"]));
                stringBuilder.Replace("[EnderecoEntrega-Cidade]", Convert.ToString(addressHt["EnderecoEntrega-Cidade"]));
                stringBuilder.Replace("[EnderecoEntrega-Estado]", Convert.ToString(addressHt["EnderecoEntrega-Estado"]));

                //Supplier
                if (purchaseOrder.SupplierId.HasValue && purchaseOrder.Supplier.LegalEntityProfileId.HasValue)
                {
                    #region SupplierAddress

                    stringBuilder.Replace("[Fornecedor-RazaoSocial]",
                                          purchaseOrder.Supplier.LegalEntityProfile.CompanyName);
                    stringBuilder.Replace("[Fornecedor-NomeFantasia]",
                                          purchaseOrder.Supplier.LegalEntityProfile.FantasyName);

                    stringBuilder.Replace("[Fornecedor-Tel]", purchaseOrder.Supplier.LegalEntityProfile.Phone);
                    stringBuilder.Replace("[Fornecedor-Tel2]", purchaseOrder.Supplier.LegalEntityProfile.Phone2);
                    stringBuilder.Replace("[Fornecedor-Tel3]", purchaseOrder.Supplier.LegalEntityProfile.Phone3);
                    stringBuilder.Replace("[Fornecedor-Fax]", purchaseOrder.Supplier.LegalEntityProfile.Fax);

                    Address supplierAddress = purchaseOrder.Supplier.LegalEntityProfile.Address;
                    if (supplierAddress != null)
                    {
                        stringBuilder.Replace("[Fornecedor-CEP]", supplierAddress.PostalCode);
                        stringBuilder.Replace("[Fornecedor-Rua]", supplierAddress.Name);
                        stringBuilder.Replace("[Fornecedor-Numero]",
                                              purchaseOrder.Supplier.LegalEntityProfile.AddressNumber);
                        stringBuilder.Replace("[Fornecedor-Complemento]",
                                              purchaseOrder.Supplier.LegalEntityProfile.AddressComp);
                        stringBuilder.Replace("[Fornecedor-Bairro]", supplierAddress.Neighborhood);
                        stringBuilder.Replace("[Fornecedor-Cidade]", supplierAddress.City);
                        stringBuilder.Replace("[Fornecedor-Estado]", supplierAddress.State);
                    }

                    #endregion

                    #region SupplierContact

                    Contact contact = new ContactManager(this)
                        .GetContactsBySupplier(purchaseOrder.CompanyId,
                                               Convert.ToInt32(purchaseOrder.SupplierId))
                        .FirstOrDefault();
                    if (contact != null)
                    {
                        stringBuilder.Replace("[Fornecedor-NomeContato]", contact.Name);
                        stringBuilder.Replace("[Fornecedor-EmailContato]", contact.Email);
                        stringBuilder.Replace("[Fornecedor-TelContato]", contact.Phone);
                    }

                    #endregion
                }

                #region PurchaseOrderProducts

                var stringBuilderAdditional = new StringBuilder();

                //Header
                stringBuilderAdditional.Append(
                    "<table  cellspacing='4' cellpadding='4' style='border-collapse: collapse;'>");
                stringBuilderAdditional.Append("<tr>");
                stringBuilderAdditional.Append("<td>ITEM</td>");
                stringBuilderAdditional.Append("<td>QTD</td>");
                stringBuilderAdditional.Append("<td>UNID</td>");
                stringBuilderAdditional.Append("<td>DESCRIÇÂO</td>");
                stringBuilderAdditional.Append("<td>VLR UNIT</td>");
                stringBuilderAdditional.Append("<td>VLR TOTAL</td>");
                stringBuilderAdditional.Append("</tr>");

                //Body
                Int32 productCount = 0;
                Decimal totalValue = Decimal.Zero;
                foreach (DataRow item in GetPurchaseOrderItemsWithQuotationBySupplier(purchaseOrder.CompanyId,
                                                                                      purchaseOrder.PurchaseOrderId,
                                                                                      Convert.ToInt32(
                                                                                          purchaseOrder.SupplierId)).
                    Rows)
                {
                    productCount++;

                    stringBuilderAdditional.Append("<tr>");
                    //

                    stringBuilderAdditional.Append("<td>" + productCount + "</td>");
                    stringBuilderAdditional.Append("<td>" + Convert.ToString(item["Quantity"]) + "</td>");
                    stringBuilderAdditional.Append("<td>" + Convert.ToString(item["Unit"]) + "</td>");
                    stringBuilderAdditional.Append("<td>" + Convert.ToString(item["Description"]) + "</td>");
                    stringBuilderAdditional.Append("<td>" + Convert.ToDecimal(item["UnitValue"]).ToString("c") + "</td>");
                    stringBuilderAdditional.Append("<td>" + Convert.ToDecimal(item["TotalValue"]).ToString("c") +
                                                   "</td>");

                    totalValue += Convert.ToDecimal(item["TotalValue"]);
                    //   
                    stringBuilderAdditional.Append("</tr>");
                }

                stringBuilderAdditional.Append("</table>");

                #endregion

                stringBuilder.Replace("[ITEMS]", stringBuilderAdditional.ToString());

                stringBuilder.Replace("[Usuario]", purchaseOrder.User2.Profile.Name);
                stringBuilder.Replace("[UsuarioEmail]", purchaseOrder.User2.UserName);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get the Purchase Order template
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DocumentTemplate GetPurchaseOrderDocumentTemplate(Int32 companyId)
        {
            return
                DbContext.DocumentTemplates.Where(
                    d =>
                    d.CompanyId == companyId && d.DocumentTemplateTypeId == (Int32)DocumentTemplateTypes.PurchaseOrder)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Get the Purchase Order template
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DocumentTemplate GetBudgetRequestDocumentTemplate(Int32 companyId)
        {
            return DbContext
                .DocumentTemplates
                .Where(d => d.CompanyId == companyId)
                .Where(d => d.DocumentTemplateTypeId == (Int32)DocumentTemplateTypes.PurchaseOrder)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get the Purchase Order template
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DocumentTemplate GetSupplyAuthorizationDocumentTemplate(Int32 companyId)
        {
            return DbContext
                .DocumentTemplates
                .Where(d => d.CompanyId == companyId)
                .Where(d => d.DocumentTemplateTypeId == (Int32)DocumentTemplateTypes.SupplyAuthorization)
                .FirstOrDefault();
        }
    }
}