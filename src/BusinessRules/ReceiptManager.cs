using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class ReceiptManager : BusinessManager<InfoControlDataContext>
    {
        #region ReceiptType enum

        public enum ReceiptType
        {
            All = 0,
            Entry,
            Delivery
        }

        #endregion

        public ReceiptManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// this method return true if the receiptNumber exist
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="receiptNumber"></param>
        /// <returns></returns>
        private Boolean ExistReceiptNumber(Int32 companyId, Int32 receiptNumber)
        {
            return
                DbContext.Receipts.Where(r => r.CompanyId == companyId && r.ReceiptNumber == receiptNumber).
                    FirstOrDefault() != null;
        }

        /// <summary>
        /// this method save the receipt and its items
        /// </summary>
        /// <param name="original_entity">this parameter is used in update operation</param>
        /// <param name="entity">this parameter always is used</param>
        /// <param name="lstReceiptItem">this parameter contai the list of ReceiptItems</param>
        public void SaveReceipt(Receipt original_entity, Receipt entity, List<ReceiptItem> lstReceiptItem,
                                List<Int32> lstServiceOrder, List<Int32> lstSale)
        {
            //Managers
            var accountManager = new AccountManager(this);
            var parcelsManager = new ParcelsManager(this);
            var financialManager = new FinancialManager(this);
            var invoice = new Invoice();
            var bill = new Bill();

            entity.ModifiedDate = DateTime.Now;
            if (original_entity.ReceiptId == 0)
            {
                //insert the Receipt
                if (ExistReceiptNumber(entity.CompanyId, Convert.ToInt32(entity.ReceiptNumber)))
                    throw new InvalidOperationException();

                DbContext.Receipts.InsertOnSubmit(entity);
            }
            else
            {
                //update
                original_entity.CopyPropertiesFrom(entity);

                //delete all receiptItens
                DeleteReceiptItemsByReceipt(entity.ReceiptId, entity.CompanyId);
            }

            DbContext.SubmitChanges();

            foreach (ReceiptItem item in lstReceiptItem)
            {
                item.ReceiptId = entity.ReceiptId;
                item.CompanyId = entity.CompanyId;
                InsertReceiptItem(item);
            }
            ////insert the ReceiptItems
            //for (int i = 0; i < lstReceiptItem.Count; i++)
            //{
            //    lstReceiptItem[i].ReceiptId = entity.ReceiptId;
            //    InsertReceiptItem(lstReceiptItem[i]);
            //}

            if (entity.SupplierId.HasValue)
            {
                bill.CompanyId = entity.CompanyId;
                bill.EntryDate = DateTime.Now;
                bill.SupplierId = entity.SupplierId;
                bill.DocumentNumber = Convert.ToString(entity.ReceiptNumber);
                //if (entity.ReceiptValue.HasValue)
                //    bill.BillValue = entity.ReceiptValue.Value;
                financialManager.Insert(bill, null);
            }
            else
            {
                SetReceiptIdInSale(entity.CompanyId, lstSale, entity.ReceiptId);
                SetReceiptIdInServiceOrder(entity.CompanyId, lstServiceOrder, entity.ReceiptId);
            }

            DbContext.SubmitChanges();

            if (bill.BillId != 0)
            {
                //insert the parcel
                var parcel = new Parcel();
                parcel.PaymentMethodId = PaymentMethod.Cash;
                parcel.CompanyId = entity.CompanyId;
                parcel.DueDate = DateTime.Now;
                //parcel.Amount = bill.BillValue.Value;
                parcel.BillId = bill.BillId;

                parcelsManager.Insert(parcel,
                                      accountManager.GetFinancierConditionByParcelCount(entity.CompanyId,
                                                                                        PaymentMethod.Cash, 1));
            }

            DbContext.SubmitChanges();
        }

        public void SaveReceipt(Receipt original_entity, Receipt entity, List<ReceiptItem> lstReceiptItem)
        {
            SaveReceipt(original_entity, entity, lstReceiptItem, new List<Int32>(), new List<Int32>());
        }

        /// <summary>
        /// this method set the receiptId in the serviceOrder
        /// </summary>
        /// <param name="lstServiceOrder"></param>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        private void SetReceiptIdInServiceOrder(Int32 companyId, List<Int32> lstServiceOrder, Int32 receiptId)
        {
            var servicesManager = new ServicesManager(this);

            ServiceOrder serviceOrder;

            for (int i = 0; i < lstServiceOrder.Count; i++)
            {
                serviceOrder = servicesManager.GetServiceOrder(lstServiceOrder[i]);
                serviceOrder.ReceiptId = receiptId;
                DbContext.SubmitChanges();
            }
        }

        /// <summary>
        /// this method set the receiptId in the sale
        /// </summary>
        /// <param name="list"></param>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        private void SetReceiptIdInSale(Int32 companyId, List<Int32> lstSale, Int32 receiptId)
        {
            var saleManager = new SaleManager(this);
            Sale sale;
            for (int i = 0; i < lstSale.Count; i++)
            {
                sale = saleManager.GetSale(companyId, lstSale[i]);
                sale.ReceiptId = receiptId;
                DbContext.SubmitChanges();
            }
        }

        /// <summary>
        /// this method delete the receipt
        /// </summary>
        /// <param name="receipt"></param>
        public void DeleteReceipt(Receipt entity)
        {
            //SaleManager manager = new SaleManager(this);
            //Sale sale = manager.GetSaleByFiscalNumber(entity.CompanyId, entity.ReceiptNumber.Value);
            //if (sale != null)
            //    sale.ReceiptId = null;

            DeleteReceiptItemsByReceipt(entity.ReceiptId, entity.CompanyId);

            //DbContext.Receipts.Attach(entity);
            DbContext.Receipts.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return the receipt by Id
        /// </summary>
        /// <param name="receiptId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Receipt GetReceipt(Int32 receiptId, Int32 companyId)
        {
            return DbContext.Receipts.Where(r => r.CompanyId == companyId && r.ReceiptId == receiptId).FirstOrDefault();
        }

        /// <summary>
        /// this method return the Iqueryable of Receipts
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Receipt> GetReceiptsByCompany(Int32 companyId)
        {
            return DbContext.Receipts.Where(r => r.CompanyId == companyId);
        }

        /// <summary>
        /// This method returns receipts by customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Receipt> GetReceiptsByCustomer(Int32 customerID, string sortExpression, int startRowIndex,
                                                         int maximumRows)
        {
            return DbContext.Receipts.Where(receipt => receipt.CustomerId == customerID).SortAndPage(sortExpression,
                                                                                                     startRowIndex,
                                                                                                     maximumRows,
                                                                                                     "ReceiptId");
        }

        /// <summary>
        /// This method retrieve the count of registers of GetReceiptsByCustomer method returns
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetReceiptsByCustomerCount(Int32 customerID, string sortExpression, int startRowIndex,
                                                int maximumRows)
        {
            return GetReceiptsByCustomer(customerID, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method return all needed columns
        /// </summary>
        /// <param name="receipts"></param>
        /// <returns></returns>
        private IQueryable GetFormatedReceipts(IQueryable<Receipt> receipts, Int32 companyId, Int32 receiptType,
                                               string sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            var query = from receipt in receipts
                        where receipt.CompanyId == companyId
                        join customers in DbContext.Customers on receipt.CustomerId equals customers.CustomerId into
                            gCustomer
                        from customer in gCustomer.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into gProfile
                        from profile in gProfile.DefaultIfEmpty()
                        join transporters in DbContext.Transporters on receipt.TransporterId equals
                            transporters.TransporterId into gTransporter
                        from transporter in gTransporter.DefaultIfEmpty()
                        join transporterlegalEntityProfiles in DbContext.LegalEntityProfiles on
                            transporter.LegalEntityProfileId equals transporterlegalEntityProfiles.LegalEntityProfileId
                            into gTransporterLegalEntityProfile
                        from transporterlegalEntityProfile in gTransporterLegalEntityProfile.DefaultIfEmpty()
                        join supplierLegalEntityProfiles in DbContext.LegalEntityProfiles on
                            receipt.Supplier.LegalEntityProfileId equals
                            supplierLegalEntityProfiles.LegalEntityProfileId into gSupplierLegalEntityProfile
                        from supplierLegalEntityProfile in gSupplierLegalEntityProfile.DefaultIfEmpty()
                        join supplierProfiles in DbContext.Profiles on receipt.Supplier.ProfileId equals
                            supplierProfiles.ProfileId into gSupplierProfile
                        from supplierProfile in gSupplierProfile.DefaultIfEmpty()
                        select new
                                   {
                                       receipt.ReceiptId,
                                       receipt.CompanyId,
                                       receipt.CustomerId,
                                       receipt.TransporterId,
                                       receipt.IssueDate,
                                       receipt.EntryDate,
                                       receipt.ModifiedDate,
                                       receipt.CfopId,
                                       receipt.ReceiptNumber,
                                       receipt.SubstitutionICMSBase,
                                       receipt.SubstitutionICMSValue,
                                       receipt.FreightValue,
                                       receipt.InsuranceValue,
                                       receipt.OthersChargesValue,
                                       receipt.ReceiptValue,
                                       receipt.SupplierId,
                                       receipt.DeliveryDate,
                                       customerName = profile.Name ?? legalEntityProfile.CompanyName,
                                       supplierName = supplierProfile.Name ?? supplierLegalEntityProfile.CompanyName,
                                       transporterName = transporterlegalEntityProfile.CompanyName,
                                       receipt.IsCanceled
                                   };
            if (receiptType == (Int32)ReceiptType.Delivery)
                query = query.Where(rpt => rpt.DeliveryDate.HasValue);

            if (receiptType == (Int32)ReceiptType.Entry)
                query = query.Where(receipt => receipt.EntryDate.HasValue);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ReceiptId");
        }

        /// <summary>
        /// this method return all receipts by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private IQueryable GetReceipts(Int32 companyId, string sortExpression, Int32 startRowIndex, Int32 maximumRows,
                                       Int32 receiptType, Int32 initialNumber, Int32 finalNumber)
        {
            return GetFormatedReceipts(DbContext.Receipts, companyId, receiptType, sortExpression, startRowIndex,
                                       maximumRows);
        }

        /// <summary>
        /// this method return the number of rows in GetReceiptByCompany
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private Int32 GetReceiptsCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return DbContext.Receipts.Where(r => r.CompanyId == companyId).Count();
        }

        /// <summary>
        /// this method returns a collection of formated receipts
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <param name="initialDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetReceipts(Int32 companyId, Int32? customerId, Int32? supplierId, string selectCustomer,
                                      DateTimeInterval dateTimeInterval, Int32 receiptType, Decimal? initialNumber,
                                      Decimal? finalNumber, String sortExpression, Int32 startRowIndex,
                                      Int32 maximumRows)
        {
            IQueryable<Receipt> receipts = DbContext.Receipts;

            #region query de union

            // Executar duas queries cada query busca por uma parte(Cliente/ fornecedor). Após a execução das queries executar um union/concat no resultado.

            //Customer's search
            //IQueryable<Receipt> customerQuery = from receipt in DbContext.Receipts
            //                                    join customer in DbContext.Customers on receipt.CustomerId equals customer.CustomerId

            //                                    join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into gProfile
            //                                    from profile in gProfile.DefaultIfEmpty()
            //                                    where profile.Name.Contains(selectCustomerOrSupplier)

            //                                    join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId into gLegalProfile
            //                                    from legalEntityProfile in gLegalProfile.DefaultIfEmpty()
            //                                    where legalEntityProfile.CompanyName.Contains(selectCustomerOrSupplier)

            //                                    select receipt;

            //IQueryable<Receipt> SupplierQuery = from receipt in DbContext.Receipts
            //                                    join supplier in DbContext.Suppliers on receipt.SupplierId equals supplier.SupplierId

            //                                    join profile in DbContext.Profiles on supplier.ProfileId equals profile.ProfileId into gProfile
            //                                    from profile in gProfile.DefaultIfEmpty()
            //                                    where profile.Name.Contains(selectCustomerOrSupplier)

            //                                    join legalEntityProfile in DbContext.LegalEntityProfiles on supplier.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId into gLegalProfile
            //                                    from legalEntityProfile in gLegalProfile.DefaultIfEmpty()
            //                                    where legalEntityProfile.CompanyName.Contains(selectCustomerOrSupplier)

            //                                    select receipt;
            ////
            //IQueryable<Receipt> receiptsQuery = customerQuery.Union(SupplierQuery);

            //if (initialDate.HasValue)
            //    receiptsQuery = receiptsQuery.Where(x => x.DeliveryDate >= initialDate.Value.Date || x.EntryDate >= initialDate.Value.Date);

            //if (endDate.HasValue)
            //    receiptsQuery = receiptsQuery.Where(x => x.DeliveryDate <= endDate.Value.Date || x.EntryDate >= endDate.Value.Date);

            //if (initialNumber.HasValue)
            //    receiptsQuery = receiptsQuery.Where(x => x.ReceiptNumber >= initialNumber.Value);

            //if (finalNumber.HasValue)
            //    receiptsQuery = receiptsQuery.Where(x => x.ReceiptNumber <= finalNumber.Value);

            #endregion

            if (selectCustomer != "")
                receipts =
                    receipts.Where(
                        receipt =>
                        receipt.Customer.Profile.Name.Contains(selectCustomer) ||
                        receipt.Customer.LegalEntityProfile.CompanyName.Contains(selectCustomer));

            if (dateTimeInterval != null)
            {
                receipts =
                    receipts.Where(
                        receipt =>
                        receipt.DeliveryDate >= dateTimeInterval.BeginDate.Date ||
                        receipt.EntryDate >= dateTimeInterval.BeginDate.Date);

                receipts =
                    receipts.Where(
                        receipt =>
                        receipt.DeliveryDate <= dateTimeInterval.EndDate.Date ||
                        receipt.EntryDate >= dateTimeInterval.EndDate.Date);
            }
            if (initialNumber.HasValue)
                receipts = receipts.Where(receipt => receipt.ReceiptNumber >= initialNumber.Value);

            if (finalNumber.HasValue)
                receipts = receipts.Where(receipt => receipt.ReceiptNumber <= finalNumber.Value);

            return GetFormatedReceipts(receipts, companyId, receiptType, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetReceipts
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="supplierId"></param>
        /// <param name="initialDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetReceiptsCount(Int32 companyId, Int32? customerId, Int32? supplierId, string selectCustomer,
                                      DateTimeInterval dateTimeInterval, Int32 receiptType, Decimal? initialNumber,
                                      Decimal? finalNumber, String sortExpression, Int32 startRowIndex,
                                      Int32 maximumRows)
        {
            return
                GetReceipts(companyId, customerId, supplierId, selectCustomer, dateTimeInterval, receiptType,
                            initialNumber, finalNumber, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        /// <summary>
        /// this method return all Sale and OS non invoided
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataTable GetSaleAndOSNonInvoiced(int companyId, Int32 customerId)
        {
            var saleQuery = from sale in DbContext.Sales
                            where !sale.ReceiptId.HasValue
                            select new
                                       {
                                           number = "VE-" + (sale.SaleDate ?? DateTime.Now),
                                           date = sale.SaleDate ?? DateTime.Now,
                                           id = sale.SaleId,
                                           companyId = sale.CompanyId,
                                           customerId = sale.CustomerId ?? 0,
                                           receiptId = sale.ReceiptId
                                       };

            var ServiceOrderQuery = from serviceOrder in DbContext.ServiceOrders
                                    where !serviceOrder.ReceiptId.HasValue
                                    select new
                                               {
                                                   number = "OS-" + serviceOrder.ServiceOrderNumber,
                                                   date = DateTime.Now,
                                                   id = serviceOrder.ServiceOrderId,
                                                   companyId = serviceOrder.CompanyId,
                                                   customerId = serviceOrder.CustomerId,
                                                   receiptId = serviceOrder.ReceiptId
                                               };

            var querySaleAndOS =
                saleQuery.Concat(ServiceOrderQuery).Where(x => x.companyId == companyId && x.customerId == customerId);
            return querySaleAndOS.ToDataTable();
        }

        public IQueryable<Recognizable> SearchProductAndService(Int32 companyId, string name, Int32 maximumRows)
        {
            var items = new List<Recognizable>(new ProductManager(this).SearchProductAsList(companyId, name, maximumRows));

            items.AddRange(new ServicesManager(this).SearchServiceAsList(companyId, name, maximumRows));

            return items.AsQueryable<Recognizable>();
        }

        #region ReceiptItems

        /// <summary>
        /// this method insert the ReceiptItem
        /// </summary>
        /// <param name="entity"></param>
        private void InsertReceiptItem(ReceiptItem entity)
        {
            DbContext.ReceiptItems.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method update the ReceiptItem
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        private void UpdateReceiptItem(ReceiptItem original_entity, ReceiptItem entity)
        {
            DbContext.ReceiptItems.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete the receipt
        /// </summary>
        /// <param name="entity"></param>
        private void DeleteReceiptItem(ReceiptItem entity)
        {
            DbContext.ReceiptItems.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete all receiptItems bu receipt
        /// </summary>
        /// <param name="receiptId"></param>
        private void DeleteReceiptItemsByReceipt(Int32 receiptId, Int32 companyId)
        {
            DbContext.ReceiptItems.DeleteAllOnSubmit(GetReceiptItems(companyId, receiptId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return all ReceiptItems By receipt
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public DataTable GetReceiptItemsAsDataTable(Int32 receiptId, Int32 companyId)
        {
            var query = from receiptItem in DbContext.ReceiptItems
                        join products in DbContext.Products on receiptItem.ProductId equals products.ProductId into
                            gProducts
                        from product in gProducts.DefaultIfEmpty()
                        where receiptItem.ReceiptId == receiptId
                        select new
                                   {
                                       receiptItem.CompanyId,
                                       receiptItem.ReceiptId,
                                       receiptItem.ReceiptItemId,
                                       receiptItem.ProductId,
                                       receiptItem.ServiceId,
                                       receiptItem.Description,
                                       productName = product.Name,
                                       receiptItem.FiscalClass,
                                       receiptItem.Quantity,
                                       receiptItem.UnitPrice,
                                       receiptItem.IPI,
                                       receiptItem.ICMS,
                                   };

            return query.ToDataTable();
        }

        /// <summary>
        /// this method returns an Iqueryable of Receipt Items
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public IQueryable<ReceiptItem> GetReceiptItems(Int32 companyId, Int32 receiptId)
        {
            return DbContext.ReceiptItems.Where(item => item.CompanyId == companyId && item.ReceiptId == receiptId);
        }

        /// <summary>
        /// this method returns a  list of Receipt Items
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public List<ReceiptItem> GetReceiptItemsAsList(Int32 companyId, Int32 receiptId)
        {
            return GetReceiptItems(companyId, receiptId).ToList();
        }

        #endregion
    }
}