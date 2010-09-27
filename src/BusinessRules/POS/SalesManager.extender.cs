using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using System.Text;
using InfoControl.Web.Mail;

namespace Vivina.Erp.BusinessRules
{
#warning melhorar a nomenclatua desse enumerador


    public partial class SaleManager
    {
        /// <summary>
        /// Return all sales executed by a customer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataTable GetSaleByCustomer(int companyId, int customerId)
        {
            var query = from sale in DbContext.Sales
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId
                        join parcel in DbContext.Parcels on sale.InvoiceId equals parcel.InvoiceId
                        where sale.CompanyId == companyId && sale.CustomerId == customerId
                        select new
                                   {
                                       sale.SaleId,
                                       sale.CompanyId,
                                       sale.Discount,
                                       sale.OrderDate,
                                       sale.ShipDate,
                                       sale.SaleDate,
                                       sale.Comment,
                                       sale.CustomerId,
                                       sale.VendorId,
                                       EmployeeName = sale.Employee.Profile.Name,
                                       sale.DepositId,
                                       sale.BudgetId,
                                       //ReceiptNumber = sale.ReceiptId.HasValue ? sale.Receipt.ReceiptNumber : null,
                                       sale.LegalTicketNumber,
                                       sale.InvoiceId,
                                       Value = invoice.Parcels.Sum(p => p.Amount)
                                   };
            return query.ToDataTable();
        }

        /// <summary>
        /// This method returns sales by customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        //public IQueryable GetSalesByCustomer(Int32 customerID, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    return DbContext.Sales.Where(sale => sale.CustomerId == customerID).SortAndPage(sortExpression, startRowIndex, maximumRows, "SaleId");
        //}
        /// <summary>
        /// This method returns sales by customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetSalesByCustomer(Int32 customerID, string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from sale in DbContext.Sales
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId into
                            gSalesInvoices
                        from invoice in gSalesInvoices.DefaultIfEmpty()
                        where sale.CustomerId == customerID
                        select new
                                   {
                                       sale.BudgetId,
                                       sale.Comment,
                                       sale.CompanyId,
                                       sale.CustomerId,
                                       sale.DepositId,
                                       sale.Discount,
                                       sale.SaleDate,
                                       sale.InvoiceId,
                                       sale.IsCanceled,
                                       sale.LegalTicketNumber,
                                       sale.OrderDate,
                                       sale.ReceiptId,
                                       ReceiptNumber = sale.ReceiptId.HasValue ? sale.Receipt.ReceiptNumber : null,
                                       sale.ShipDate,
                                       sale.VendorId,
                                       sale.SaleId,
                                       InvoiceValue = invoice.Parcels.Sum(parcel => parcel.Amount)
                                   };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "SaleDate");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetSalesByCustomerCount(Int32 customerID, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetSalesByCustomer(customerID, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }

        /// <summary>
        /// Pega a Venda pelo seu SaleId, Verifica todas as vendas de todas as filiais.
        /// Método utilizado para procurar um produto para devolução, por exemplo.
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="SaleId"></param>
        /// <returns></returns>
        public Sale GetSale(Int32 MatrixId, Int32 SaleId)
        {
            IQueryable<Sale> query = from sales in DbContext.Sales
                                     join comp in DbContext.Companies on sales.CompanyId equals comp.CompanyId
                                     where (comp.MatrixId == MatrixId && sales.SaleId == SaleId)
                                     select sales;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// this method returns an sale by id
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        /// 
        public Sale GetSaleById(Int32 saleId)
        {
            return DbContext.Sales.Where(x => x.SaleId == saleId).FirstOrDefault();
        }

        /// <summary>
        /// Pega a Venda pelo seu FiscalNumber
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="FiscalNumber"></param>
        /// <returns></returns>
        public Sale GetSaleByFiscalNumber(Int32 MatrixId, Int32 ReceiptNumber)
        {
            IQueryable<Sale> query = from sales in DbContext.Sales
                                     join comp in DbContext.Companies on sales.CompanyId equals comp.CompanyId
                                     join receipt in DbContext.Receipts on sales.ReceiptId equals receipt.ReceiptId
                                     where (comp.MatrixId == MatrixId && receipt.ReceiptNumber == ReceiptNumber)
                                     select sales;

            return query.FirstOrDefault();
        }


        /// <summary>
        /// This method set null on the field receiptID of one sale
        /// </summary>
        /// <param name="saleID"></param>
        public void SetNullReceiptIDInSale(Int32 saleID)
        {
            var query = DbContext.Sales.Where(sale => sale.SaleId == saleID).FirstOrDefault();
            query.ReceiptId = null;
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method sets null in budgetId of specified serviceOrder
        /// </summary>
        /// <param name="serviceOrderId">can't be null </param>
        public void DetachBudgetFromServiceOrder(Int32 serviceOrderId)
        {
            var serviceOrder = DbContext.ServiceOrders.Where(x => x.ServiceOrderId == serviceOrderId).FirstOrDefault();
            serviceOrder.BudgetId = null;

            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method retrieve an sale by receipt and company
        /// </summary>
        /// <param name="MatrixId"></param>
        /// <param name="receiptID"></param>
        /// <returns></returns>
        public Sale GetSaleByReceipt(Int32 MatrixId, Int32 receiptID)
        {
            IQueryable<Sale> query = from sales in DbContext.Sales
                                     join comp in DbContext.Companies on sales.CompanyId equals comp.CompanyId
                                     join receipt in DbContext.Receipts on sales.ReceiptId equals receipt.ReceiptId
                                     where (comp.MatrixId == MatrixId && receipt.ReceiptId == receiptID)
                                     select sales;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// this method execute the sale(save items and insert an invoice)
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="list"></param>
        /// <param name="userId"></param>
        /// <param name="invoice"></param>
        /// <param name="lstParcels"></param>
        /// <returns></returns>
        public Sale SaveSale(Sale sale, List<SaleItem> list, Int32 userId, List<Parcel> lstParcels)
        {
            sale.CreatedDate = sale.ModifiedDate = DateTime.Now;
            Insert(sale);

            SaveSaleItems(sale, list);

            if (sale.CustomerId.HasValue)
            {
                var invoice = new Invoice();
                invoice.CompanyId = sale.CompanyId;
                invoice.EntryDate = DateTime.Now;
                invoice.CustomerId = sale.CustomerId;
                //invoice.DocumentNumber = sale.SaleId.ToString();
                invoice.Description = "VENDA";
                SaveSaleInvoice(sale, invoice, lstParcels);
            }

            return sale;
        }

        public Sale SaveSale(Sale sale, Budget budget, Int32 userId, DateTime firstDatePayment, FinancierCondition condition)
        {
            var saleItemlist = (sale.SaleItems != null) ? sale.SaleItems.ToList() : new List<SaleItem>();

            //
            // Save Budget
            //
            if (budget != null)
            {
                Budget originalBudget = GetBudget(budget.BudgetId, budget.CompanyId);
                if (originalBudget != null)
                {
                    originalBudget.CopyPropertiesFrom(budget);
                    DbContext.SubmitChanges();
                }
                else
                    InsertBudget(budget, null);

                sale.BudgetId = budget.BudgetId;

                saleItemlist = CreateSaleItemList(budget.BudgetItems.ToList());
            }


            //
            // Save Sale
            //            
            var parcelList = CreateParcelList(saleItemlist.Sum(i => i.Quantity * i.UnitPrice), firstDatePayment, condition);

            return SaveSale(sale, saleItemlist, userId, parcelList);
        }

        private List<SaleItem> CreateSaleItemList(List<BudgetItem> budgetItemList)
        {
            List<SaleItem> list = new List<SaleItem>();

            foreach (var row in budgetItemList)
                list.Add(new SaleItem()
                {
                    CompanyId = row.Budget.CompanyId,
                    ModifiedDate = DateTime.Now,
                    Quantity = row.Quantity,
                    UnitPrice = row.UnitPrice.Value,
                    UnitCost = row.UnitCost,
                    ProductId = row.ProductId,
                    SpecialProductName = row.SpecialProductName
                });

            return list;
        }

        private List<Parcel> CreateParcelList(decimal total, DateTime firstDatePayment, FinancierCondition condition)
        {
            List<Parcel> parcelList = new List<Parcel>();
            for (int i = 1; i <= condition.ParcelCount; i++)
            {
                decimal amount = Math.Round(total / condition.ParcelCount, 2);
                if (i == 1)
                    amount += (total - (amount * condition.ParcelCount));

                Parcel parcel = new Parcel();
                parcel.FinancierOperationId = condition.FinancierOperationId;
                parcel.Amount = amount;
                parcel.Description = i + "/" + condition.ParcelCount;
                parcel.DueDate = firstDatePayment.AddMonths(i - 1);
                parcel.CompanyId = condition.CompanyId;

                //if (condition.FinancierOperation.PaymentMethod.PaymentMethodTypeId == PaymentMethodType.CashOrOnLineTransfer)
                //{
                //    parcel.EffectedDate = DateTime.Now;
                //    parcel.EffectedAmount = parcel.Amount;
                //}

                parcelList.Add(parcel);
            }
            return parcelList;
        }

        /// <summary>
        /// this method save the invoice created by Sale
        /// </summary>
        /// <param name="sale"></param>
        private void SaveSaleInvoice(Sale sale, Invoice invoice, List<Parcel> lstParcels)
        {
            //
            // Save Invoice
            //
            var financialManager = new FinancialManager(this);
            sale.InvoiceId = financialManager.InsertRetrievingId(invoice);
            DbContext.SubmitChanges();


            //
            // Save Parcels
            //
            var parcelsManager = new ParcelsManager(this);
            var accountManager = new AccountManager(this);

            var firstParcel = lstParcels.FirstOrDefault();
            var condition = accountManager.GetFinancierConditionByParcelCount(firstParcel.CompanyId, firstParcel.FinancierOperationId.Value, lstParcels.Count());

            foreach (Parcel parcel in lstParcels)
            {
                parcel.InvoiceId = invoice.InvoiceId;
                parcelsManager.Insert(parcel, condition);
            }


        }

        public DataTable MapOfSale_Totals(int companyId, DateTime startDate)
        {
            var query = from sale in DbContext.Sales
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId
                        join parcel in DbContext.Parcels on invoice.InvoiceId equals parcel.InvoiceId
                        where (sale.CompanyId == companyId) && (sale.SaleDate >= startDate)
                        group parcel by invoice.EntryDate
                            into gParcel
                            select new
                                       {
                                           Value = gParcel.Sum(p => p.Amount),
                                           SaleDate = gParcel.Key
                                       };

            return query.ToDataTable();
        }

        /// <summary>
        /// This method return all sales grouped by a salesperson
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public DataTable MapOfSale_SalesPerson(int companyId, DateTime startDate)
        {
            var query = from sale in DbContext.Sales
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId
                        join parcel in DbContext.Parcels on invoice.InvoiceId equals parcel.InvoiceId
                        join salePerson in DbContext.Employees on sale.VendorId equals salePerson.EmployeeId
                        join profile in DbContext.Profiles on salePerson.ProfileId equals profile.ProfileId
                        where (sale.CompanyId == companyId) && (sale.SaleDate >= startDate)
                        group parcel by profile.Name
                            into gParcel
                            select new
                                       {
                                           Value = gParcel.Sum(p => p.Amount),
                                           Name = gParcel.Key
                                       };
            return query.ToDataTable();
        }

        /// <summary>
        /// This method return all sales grouped by a category
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public DataTable MapOfSale_Category(int companyId, DateTime startDate)
        {
            var query = from sale in DbContext.Sales
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId
                        join parcel in DbContext.Parcels on invoice.InvoiceId equals parcel.InvoiceId
                        join saleItem in DbContext.SaleItems on sale.SaleId equals saleItem.SaleId
                        join product in DbContext.Products on saleItem.ProductId equals product.ProductId
                        join category in DbContext.Categories on product.CategoryId equals category.CategoryId
                        where (sale.CompanyId == companyId) && (sale.SaleDate >= startDate)
                        group parcel by category.Name
                            into gParcel
                            select new
                                       {
                                           Value = gParcel.Sum(p => p.Amount),
                                           Name = gParcel.Key
                                       };
            return query.ToDataTable();
        }

        /// <summary>
        /// This method return all sales grouped by a PaymentMethod
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable MapOfSale_Payment(int companyId, DateTimeInterval dateInterval)
        {
            var query = from sale in DbContext.Sales
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId
                        join parcel in DbContext.Parcels on invoice.InvoiceId equals parcel.InvoiceId
                        join paymentMethod in DbContext.PaymentMethods on parcel.PaymentMethodId equals
                            paymentMethod.PaymentMethodId
                        where
                            (sale.CompanyId == companyId) &&
                            ((sale.SaleDate >= dateInterval.BeginDate) && (sale.SaleDate <= dateInterval.EndDate))
                        group parcel by paymentMethod.Name
                            into gParcel
                            select new
                                       {
                                           Value = gParcel.Sum(p => p.Amount),
                                           Name = gParcel.Key
                                       };
            return query.ToDataTable();
        }

        /// <summary>
        /// This method returns the history of sales of one company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="showCanceled"></param> 
        /// <returns></returns> 
        public IQueryable GetSaleHistory(Int32 companyId, Int32? customerId, Int32? representantId, Int32? receiptNumber,
                                         DateTimeInterval dateTimeInterval, bool? showCanceled, Int32? saleStatusId,
                                         string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from sale in DbContext.Sales
                        join customer in DbContext.Customers on sale.CustomerId equals customer.CustomerId into
                            gSale_customer
                        from customer in gSale_customer.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gCustomerLegalEntityProfile
                        from legalEntityProfile in gCustomerLegalEntityProfile.DefaultIfEmpty()
                        join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into
                            gCustomerProfile
                        from profile in gCustomerProfile.DefaultIfEmpty()
                        join invoice in DbContext.Invoices on sale.InvoiceId equals invoice.InvoiceId into gSaleInvoice
                        from invoice in gSaleInvoice.DefaultIfEmpty()
                        join receipt in DbContext.Receipts on sale.ReceiptId equals receipt.ReceiptId into gReceipts
                        from receipt in gReceipts.DefaultIfEmpty()
                        join employee in DbContext.Employees on sale.VendorId equals employee.EmployeeId into
                            gEmployeeVendor
                        from employee in gEmployeeVendor.DefaultIfEmpty()
                        join employeeProfile in DbContext.Profiles on employee.ProfileId equals
                            employeeProfile.ProfileId
                        where
                            sale.SaleDate >= dateTimeInterval.BeginDate && sale.SaleDate <= dateTimeInterval.EndDate &&
                            sale.IsCanceled == showCanceled && sale.CompanyId == companyId
                        select new
                        {
                            sale.SaleStatusId,
                            sale.SaleId,
                            sale.SaleDate,
                            sale.IsCanceled,
                            sale.ReceiptId,
                            receipt.ReceiptNumber,
                            employeeName = employeeProfile.Name,
                            InvoiceValue = sale.InvoiceId != null ? sale.Invoice.Parcels.Sum(x => x.Amount) : 0,
                            CustomerId = (int?)customer.CustomerId,
                            RepresentantId = (int?)customer.RepresentantId,
                            customerName = (legalEntityProfile.CompanyName ?? (profile.Name ?? ""))
                        };

            if (customerId.HasValue)
                query = query.Where(customer => customer.CustomerId == customerId);

            if (receiptNumber.HasValue)
                query = query.Where(receipt => receipt.ReceiptNumber == receiptNumber);

            if (saleStatusId.HasValue)
                query = query.Where(sale => sale.SaleStatusId == saleStatusId);

            if (representantId.HasValue)
                query = query.Where(customer => customer.RepresentantId == representantId);


            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "SaleDate");
        }


        /// <summary>
        /// This method returns an extense list of the sales
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
#warning este método deve ser subistituido pelo método acima(linq)
        public DataTable GetSaleHistory(Int32 companyId, Int32? customerId, DateTimeInterval dateTimeInterval,
                                        bool? showCanceled)
        {
            string query = String.Empty;
            query =
                @" SELECT       
                                    s.SaleId, s.SaleDate,s.ReceiptId,Receipt.ReceiptNumber,Profile.Name AS EmployeeName,
                                    Customer.CustomerId,COALESCE(LegalEntityProfile.CompanyName,'') + COALESCE(cProfile.Name,'') AS Customer, 
                                    SUM(SaleItem.Quantity*SaleItem.UnitPrice) AS Total
                               FROM         
                                    Sale AS s INNER JOIN
                                    SaleItem ON SaleItem.SaleId = s.SaleId LEFT OUTER JOIN
                                    Product ON Product.ProductId = SaleItem.ProductId LEFT OUTER JOIN
                                    Customer ON s.CustomerId = Customer.CustomerId LEFT OUTER JOIN
                                    LegalEntityProfile ON Customer.LegalEntityProfileId = LegalEntityProfile.LegalEntityProfileId LEFT OUTER JOIN
                                    Profile as cProfile ON Customer.ProfileId = cProfile.ProfileId LEFT OUTER JOIN
                                    Employee ON s.VendorId = Employee.EmployeeId LEFT OUTER JOIN
                                    Profile ON Employee.ProfileId = Profile.ProfileId LEFT OUTER JOIN
                                    Receipt ON s.ReceiptId = Receipt.ReceiptId LEFT OUTER JOIN
                                    Invoice ON s.InvoiceId = Invoice.InvoiceId
                               WHERE     
                                    (s.CompanyId = @company) AND 
                                    (s.SaleDate >= @startDate) AND 
                                    (s.SaleDate <= @endDate)";

            if (customerId.HasValue)
            {
                query += "AND Customer.CustomerId = @customerId";
                DataManager.Parameters.Add("@customerId", customerId.Value);
            }


            query += " AND (s.IsCanceled = @showCanceled)";

            query +=
                "  GROUP BY s.SaleDate, LegalEntityProfile.CompanyName, Profile.Name,cProfile.Name, s.SaleId,s.ReceiptId,Receipt.ReceiptNumber,Customer.CustomerId";
            DataManager.Parameters.Add("@company", companyId);

            if (dateTimeInterval == null)
                dateTimeInterval = new DateTimeInterval(DateTime.Now.Sql2005MinValue(), DateTime.MaxValue);
            else
                dateTimeInterval.EndDate.AddDays(1);

            DataManager.Parameters.Add("@startDate", dateTimeInterval.BeginDate.Date);
            DataManager.Parameters.Add("@endDate", dateTimeInterval.EndDate.Date);

            DataManager.Parameters.Add("@showCanceled", showCanceled);
            return DataManager.ExecuteDataTable(query);
        }

        public Int32 GetSaleHistoryCount(Int32 companyId, Int32? customerId, DateTimeInterval dateTimeInterval,
                                         bool? showCanceled)
        {
            return GetSaleHistory(companyId, customerId, dateTimeInterval, showCanceled).Rows.Count;
        }

        private IQueryable<Sale> GetSalesByCompany(Int32 companyId)
        {
            return DbContext.Sales.Where(sale => sale.CompanyId.Equals(companyId));
        }


        public Int32 GetSaleHistoryCount(Int32 companyId, Int32? customerId, Int32? representantId, Int32? receiptNumber,
                                         DateTimeInterval dateTimeInterval, bool? showCanceled, Int32? saleStatusId,
                                         String sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            return
                GetSaleHistory(companyId, customerId, representantId, receiptNumber, dateTimeInterval, showCanceled, saleStatusId,
                               sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }

        /// <summary>
        /// Return all parcels of a sale
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public IList GetSaleParcels(int saleId)
        {
            var parc = from invoices in DbContext.Invoices
                       join sale in DbContext.Sales on invoices.InvoiceId equals sale.InvoiceId
                       join parcels in DbContext.Parcels on invoices.InvoiceId equals parcels.InvoiceId
                       join paymentMethod in DbContext.PaymentMethods on parcels.PaymentMethodId equals
                           paymentMethod.PaymentMethodId
                       where sale.SaleId == saleId
                       select new
                                  {
                                      parcels.ParcelId,
                                      parcels.DueDate,
                                      parcels.EffectedDate,
                                      parcels.Description,
                                      parcels.Amount,
                                      payment = paymentMethod.Name
                                  };
            return parc.ToList();
        }

        /// <summary>
        /// This method cancels a sale and return the products to inventory
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="matrixId"></param>
        public void CancelSale(int saleId, int matrixId, Int32 userId)
        {
            var inventoryManager = new InventoryManager(this);
            var receiptManager = new ReceiptManager(this);
            var financialManager = new FinancialManager(this);
            var parcelManager = new ParcelsManager(this);

            Sale sale = GetSale(matrixId, saleId);
            sale.IsCanceled = true;

            if (sale.ReceiptId != null)
            {
                Receipt receipt = receiptManager.GetReceipt((int)sale.ReceiptId, sale.CompanyId);
                sale.ReceiptId = null;
                receiptManager.DeleteReceipt(receipt);
                receipt.IsCanceled = true;
            }

            sale.InvoiceId = null;

            //
            // return the products to inventory
            //
            foreach (SaleItem saleItem in sale.SaleItems)
            {
                if (saleItem.ProductId != null)
                {
                    Inventory inventory = inventoryManager.GetProductInventory(saleItem.CompanyId,
                                                                               (int)saleItem.ProductId,
                                                                               (int)sale.DepositId);
                    if (inventory != null)
                        inventoryManager.StockDeposit(inventory, null, userId);
                }
            }

            //
            // Delete the invoice of sale
            //
            if (sale.InvoiceId.HasValue)
                financialManager.DeleteInvoice((int)sale.InvoiceId, sale.CompanyId);

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return the consumption by sale
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Double GetConsumptionBySale(int companyId)
        {
            var query = from sale in DbContext.Sales
                        where sale.CompanyId == companyId
                        join saleItems in DbContext.SaleItems on sale.SaleId equals saleItems.SaleId
                        group saleItems by saleItems.CompanyId
                            into gSaleItems
                            select new
                                       {
                                           media =
                            ((gSaleItems.Sum(x => x.UnitPrice * x.Quantity) - gSaleItems.Sum(x => x.Sale.Discount)) /
                             gSaleItems.Sum(x => x.Quantity))
                                       };
            return Convert.ToDouble(query.FirstOrDefault().media);
        }

        /// <summary>
        /// this method return the consumption by employee
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetConsumptionByEmployee(int companyId)
        {
            var query = from sale in DbContext.Sales
                        where sale.CompanyId == companyId
                        join saleItems in DbContext.SaleItems on sale.SaleId equals saleItems.SaleId
                        join employee in DbContext.Employees on sale.VendorId equals employee.EmployeeId
                        join profile in DbContext.Profiles on employee.ProfileId equals profile.ProfileId
                        group saleItems by profile.Name
                            into gSaleItems
                            select new
                                       {
                                           Vendedor = gSaleItems.Key,
                                           Total =
                            (gSaleItems.Sum(x => x.UnitPrice * x.Quantity) - gSaleItems.Sum(x => x.Sale.Discount))
                                       };
            return query.ToDataTable();
        }

        /// <summary>
        /// This method return sale by invoice
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public Sale GetSaleByInvoice(int companyId, int invoiceId)
        {
            return DbContext.Sales.Where(s => s.CompanyId == companyId && s.InvoiceId == invoiceId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieve all saleStatus in db
        /// </summary>
        /// <returns></returns>
        public IQueryable<DataClasses.SaleStatus> GetAllSaleStatus()
        {
            return DbContext.SaleStatus;
        }

        #region SaleItems

        /// <summary>
        /// this method save the Saleitems of Sale
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="list"></param>
        private void SaveSaleItems(Sale sale, List<SaleItem> list)
        {
            foreach (SaleItem item in list)
            {
                item.SaleId = sale.SaleId;
                //item.Sale = sale;
                InsertSaleItem(item, sale);
            }
        }

        /// <summary>
        /// this method return all SaleItems by Sale
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public List<SaleItem> GetSaleItemsBySale(Int32 saleId)
        {
            return DbContext.SaleItems.Where(i => i.SaleId == saleId).ToList();
        }

        /// <summary>
        /// Method to insert a Item in the saleItem
        /// This method verifies if you are trying to insert a composite product, and treat it
        /// insert a customerEquipment If the customer exists
        /// This method also, drop off the quantity in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sale"></param>
        public void InsertSaleItem(SaleItem saleItem, Sale sale)
        {
            DbContext.SaleItems.InsertOnSubmit(saleItem);
            DbContext.SubmitChanges();

            var product = new ProductManager(this).GetProduct(Convert.ToInt32(saleItem.ProductId),
                                                                             saleItem.CompanyId);
            if (product != null)
                saleItem.ProductId = product.ProductId;
            DbContext.SubmitChanges();

            SaveSaleItemAsCustomerEquipment(saleItem);
            DropProductInSale(saleItem);
        }

        /// <summary>
        /// this method save a sale item as customer Equipment
        /// </summary>
        /// <param name="saleItem"></param>
        private void SaveSaleItemAsCustomerEquipment(SaleItem saleItem)
        {
            var customerManager = new CustomerManager(this);
            var customerEquipment = new CustomerEquipment();
            if (saleItem.Sale.CustomerId.HasValue && saleItem.ProductId.HasValue &&
                saleItem.Product.AddCustomerEquipmentInSale == true)
            {
                customerEquipment.CustomerId = saleItem.Sale.CustomerId.Value;
                customerEquipment.CompanyId = saleItem.CompanyId;
                customerEquipment.SerialNumber = saleItem.SerialNumber;
                customerEquipment.Name = saleItem.Product.Name;
                if (saleItem.Product.Manufacturer != null)
                    customerEquipment.Manufacturer = saleItem.Product.Manufacturer.Name;
                customerManager.InsertCustomerEquipment(customerEquipment);
            }
        }

        /// <summary>
        /// this method drop the product in Sale
        /// </summary>
        /// <param name="saleItem"></param>
        private void DropProductInSale(SaleItem saleItem)
        {
            var manager = new InventoryManager(this);
            Inventory inventory;
            if (saleItem.ProductId.HasValue && saleItem.Product.DropCompositeInStock && saleItem.Sale.DepositId.HasValue)
            {
                inventory = manager.GetProductInventory(saleItem.CompanyId, (int)saleItem.ProductId,
                                                        saleItem.Sale.DepositId.Value);
                if (inventory != null)
                {
                    inventory.ProductId = saleItem.ProductId.Value;
                    inventory.CompanyId = saleItem.CompanyId;
                    inventory.DepositId = saleItem.Sale.DepositId.Value;
                    manager.InventoryDrop(inventory, saleItem.Quantity, (int)DropType.Sell, saleItem.SaleId);
                }
            }
        }

        /// <summary>
        /// Return all products of a Sale
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="SaleId"></param>
        /// <returns></returns>
        public IList GetSaleProducts(Int32 CompanyId, Int32 SaleId)
        {
            var query = from sItems in DbContext.SaleItems
                        join prod in DbContext.Products on sItems.ProductId equals prod.ProductId into g
                        from prod in g.DefaultIfEmpty()
                        where (sItems.CompanyId == CompanyId && sItems.SaleId == SaleId)
                        select new
                                   {
                                       productId = sItems.ProductId ?? 0,
                                       productCode = "0" + prod.ProductCode,
                                       Name = prod.Name ?? "" + sItems.SpecialProductName ?? "",
                                       sItems.Quantity,
                                       sItems.UnitPrice,
                                       total = sItems.Quantity * sItems.UnitPrice
                                   };
            return query.ToList();
        }

        /// <summary>
        /// this method return the items of a Sale
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="saleId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public DataTable GetSaleProductsAsDataTable(Int32 companyId, Int32 saleId, string sortExpression,
                                                    Int32 startRowIndex, Int32 maximumRows)
        {
            var query = from saleItem in DbContext.SaleItems
                        where saleItem.CompanyId == companyId && saleItem.SaleId == saleId
                        join products in DbContext.Products on saleItem.ProductId equals products.ProductId into
                            gProducts
                        from product in gProducts.DefaultIfEmpty()
                        select new
                                   {
                                       productCode = product.ProductCode ?? "0",
                                       productName = product.Name ?? saleItem.SpecialProductName,
                                       quantity = saleItem.Quantity,
                                       saleItem.UnitPrice,
                                       total = saleItem.Quantity * saleItem.UnitPrice
                                   };
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "productCode").ToDataTable();
        }

        /// <summary>
        /// this method is the count Method of GetSaleProductsAsDataTable
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="saleId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetSaleProductsAsDataTableCount(Int32 companyId, Int32 saleId, string sortExpression,
                                                     Int32 startRowIndex, Int32 maximumRows)
        {
            return GetSaleProductsAsDataTable(companyId, saleId, sortExpression, startRowIndex, maximumRows).Rows.Count;
        }

        #endregion



        #region Budget

        public enum DiscountType
        {
            Percentage,
            Cash
        }

        public enum BudgetStatus
        {
            Open = 1,
            SentToCustomer = 2,
            Accepted = 3,
            Rejected = 4
        }


        public String ApplyBudgetTemplate(Budget budget, Int32 budgetDocumentTemplateId)
        {
            var addressManager = new AddressManager(this);

            DocumentTemplate documentTemplate = new CompanyManager(this).GetDocumentTemplate(budgetDocumentTemplateId);

            string template = documentTemplate == null ? GetTempBudgetTemplate() : documentTemplate.Content;
            bool isRtf = documentTemplate.FileName.EndsWith("rtf", StringComparison.OrdinalIgnoreCase);

            var stringBuilder = new StringBuilder(template);
            var tempStringBuilder = new StringBuilder();

            if (budget.BudgetItems == null)
                budget = GetBudget(budget.BudgetId, budget.CompanyId);

            stringBuilder.Replace("[NumeroDoOrcamento]", budget.BudgetCode);
            stringBuilder.Replace("[DataEmissao]", budget.ModifiedDate.ToShortDateString() +
                (isRtf ? "<br />".ToRtf() : "<br />"));

            //header with informations of current date 
            string header = budget.Company.LegalEntityProfile.Address.State + " , " + DateTime.Now.ToLongDateString().Split(',').ElementAt(1);
            stringBuilder.Replace("[Cabecalho]", header);



            #region CompanyInformations

            stringBuilder.Replace("[NomeDaEmpresa]", budget.Company.LegalEntityProfile.CompanyName);
            stringBuilder.Replace("[TelefoneDaEmpresa]", budget.Company.LegalEntityProfile.Phone);

            stringBuilder.Replace("[Endereco-Empresa]", budget.Company.LegalEntityProfile.Address.Name.ToCapitalize());

            stringBuilder.Replace("[Endereco-Complemento-Empresa]", String.Empty);

            if (!String.IsNullOrEmpty(budget.Company.LegalEntityProfile.AddressComp))
                stringBuilder.Replace("[Endereco-Complemento-Empresa]", budget.Company.LegalEntityProfile.AddressComp.ToCapitalize());

            stringBuilder.Replace("[Endereco-Numero-Empresa]", budget.Company.LegalEntityProfile.AddressNumber);
            stringBuilder.Replace("[Endereco-Cep-Empresa]", budget.Company.LegalEntityProfile.PostalCode);
            stringBuilder.Replace("[Endereco-Cidade-Empresa]", budget.Company.LegalEntityProfile.Address.City.ToCapitalize());
            stringBuilder.Replace("[Endereco-Estado-Empresa]", addressManager.GetAcronymState(budget.Company.LegalEntityProfile.Address.State));
            stringBuilder.Replace("[Endereco-Bairro-Empresa]", budget.Company.LegalEntityProfile.Address.Neighborhood.ToCapitalize());

            #endregion

            //Customer
            if (budget.CustomerId.HasValue || !String.IsNullOrEmpty(budget.CustomerName))
            {
                #region Customer Data

                stringBuilder.Replace("[NomeDoCliente]", String.IsNullOrEmpty(budget.CustomerName)
                                                             ? budget.Customer.Name
                                                             : budget.CustomerName);

                stringBuilder.Replace("[EmailDoClente]", budget.Customer != null
                                                             ? budget.Customer.Email
                                                             : budget.CustomerMail);

                stringBuilder.Replace("[TelefoneDoCliente]", String.IsNullOrEmpty(budget.CustomerPhone)
                                                                 ? budget.Customer.Phone
                                                                 : budget.CustomerPhone);

                #endregion



                if (budget.Customer != null)
                {
                    #region Customer Address

                    if (budget.Customer.Address != null)
                    {
                        stringBuilder.Replace("[EnderecoDoCliente]", String.Empty);

                        if (!String.IsNullOrEmpty(budget.Customer.Address.Name))
                            stringBuilder.Replace("[EnderecoDoCliente]", budget.Customer.Address.Name.ToCapitalize());

                        stringBuilder.Replace("[Endereco-Complemento]", String.Empty);

                        if (!String.IsNullOrEmpty(budget.Customer.AddressComp))
                            stringBuilder.Replace("[Endereco-Complemento]", budget.Customer.AddressComp.ToCapitalize());

                        stringBuilder.Replace("[Endereco-Bairro]", String.Empty);

                        if (!String.IsNullOrEmpty(budget.Customer.Address.Neighborhood))
                            stringBuilder.Replace("[Endereco-Bairro]", budget.Customer.Address.Neighborhood.ToCapitalize());

                        stringBuilder.Replace("[Endereco-Cidade]", String.Empty);

                        if (!String.IsNullOrEmpty(budget.Customer.Address.City))
                            stringBuilder.Replace("[Endereco-Cidade]", budget.Customer.Address.City.ToCapitalize());

                        stringBuilder.Replace("[Endereco-Numero]", String.Empty);

                        if (!String.IsNullOrEmpty(budget.Customer.AddressNumber))
                            stringBuilder.Replace("[Endereco-Numero]", budget.Customer.AddressNumber);

                        stringBuilder.Replace("[Endereco-Cep]", String.Empty);

                        if (!String.IsNullOrEmpty(budget.Customer.Address.PostalCode))
                            stringBuilder.Replace("[Endereco-Cep]", budget.Customer.Address.PostalCode);

                        stringBuilder.Replace("[Endereco-Estado]", addressManager.GetAcronymState(budget.Company.LegalEntityProfile.Address.State));
                    }

                    #endregion


                    string phone2 = "", phone3 = "";
                    if (budget.Customer.LegalEntityProfile != null)
                    {
                        phone2 = budget.Customer.LegalEntityProfile.Phone2;
                        phone3 = budget.Customer.LegalEntityProfile.Phone3;
                    }

                    stringBuilder.Replace("[Telefone2]", phone2);
                    stringBuilder.Replace("[Telefone3]", phone3);



                    stringBuilder.Replace("[CNPJ]", budget.Customer.LegalEntityProfile != null
                                                        ? "CNPJ: " + budget.Customer.LegalEntityProfile.CNPJ
                                                        : String.Empty);

                    stringBuilder.Replace("[CPF]", budget.Customer.Profile != null
                                                       ? "CPF: " + budget.Customer.Profile.CPF
                                                       : String.Empty);

                    stringBuilder.Replace("[IE]", budget.Customer.LegalEntityProfile != null
                                                      ? "IE: " + budget.Customer.LegalEntityProfile.IE
                                                      : String.Empty);

                    stringBuilder.Replace("[RG]", budget.Customer.Profile != null
                                                      ? "RG: " + budget.Customer.Profile.RG
                                                      : String.Empty);
                }
                else
                {
                    stringBuilder.Replace("[EnderecoDoCliente]", String.Empty);
                    stringBuilder.Replace("[Endereco-Complemento]", String.Empty);
                    stringBuilder.Replace("[Endereco-Numero]", String.Empty);
                    stringBuilder.Replace("[Endereco-Cep]", String.Empty);
                    stringBuilder.Replace("[Endereco-Cidade]", String.Empty);
                    stringBuilder.Replace("[Endereco-Estado]", String.Empty);
                    stringBuilder.Replace("[Telefone2]", String.Empty);
                    stringBuilder.Replace("[Telefone3]", String.Empty);
                }
            }

            #region BudgetItems

            tempStringBuilder = new StringBuilder();

            //Header


            //Body
            Int32 itemCount = 0;
            Decimal totalValue = Decimal.Zero;
            foreach (BudgetItem item in GetBudgetItemByBudget(budget.BudgetId, budget.CompanyId))
            {
                itemCount++;

                string itemName = item.SpecialProductName;

                tempStringBuilder.AppendFormat(@"
				<fieldset>
					<table>
						<tr>
							<td style='white-space:nowrap; width:1%;' width='1%'>Código:</td>
							<td class='code' colspan='2'>{0}</td>
						</tr>
						<tr>
							<td style='white-space:nowrap'>Qtd:</td>
							<td class='qtd' colspan='2'>{1}</td>
						</tr>
						<tr>
							<td style='white-space:nowrap'>Desc:</td>
							<td class='description' colspan='2'>{2}</td>
						</tr>
						<tr>
							<td style='white-space:nowrap'>Vlr Unit:</td>
							<td>{3}</td>
							<td style='text-align:right'>Valor: {4}</td>
						</tr>
					</table>
				</fieldset>", item.ProductCode + " | " + itemName,
                              item.Quantity,
                              item.ProductDescription,
                              item.UnitPrice.Value.ToString("c"),
                              (item.Quantity * item.UnitPrice.Value).ToString("c"));

                totalValue += (item.Quantity * item.UnitPrice.Value);
            }

            // Subtotal
            tempStringBuilder.AppendFormat(@"<table width='100%'><tr>
											 <td style='white-space:nowrap'><b>Subtotal (R$):&nbsp;</b></td>
											 <td style='white-space:nowrap; text-align:right'>{0}<td>
											 </tr>", totalValue);

            // AdditionalCost
            if (budget.AdditionalCost.HasValue)
            {
                tempStringBuilder.AppendFormat(@"<tr>
												 <td style='white-space:nowrap'><b>Adicional (R$):&nbsp;</b></td>
												 <td style='white-space:nowrap; text-align:right'>{0}<td>
												 </tr>", budget.AdditionalCost.Value.ToString("c"));
                totalValue += budget.AdditionalCost.Value;
            }

            // Discount
            if (budget.Discount.HasValue)
            {
                tempStringBuilder.AppendFormat(@"<tr>
												 <td style='white-space:nowrap'><b>Desconto (R$):&nbsp;</b></td>
												 <td style='white-space:nowrap; text-align:right'>{0}<td>
												 </tr>", budget.Discount.Value.ToString("c"));
                totalValue -= budget.Discount.Value;
            }

            //Footer
            tempStringBuilder.AppendFormat(@"<tr>
													<td style='white-space:nowrap'><b>Valor Total da Proposta (R$):&nbsp;&nbsp;&nbsp;</b></td>		
													<td style='white-space:nowrap; text-align:right'>&nbsp;&nbsp;<b>{0}<b></td>
												</tr>
												<tr>
													<td style='white-space:nowrap;  text-align:center' colspan='2'><b>({1})<b></td>
												</tr>
											</table>", totalValue.ToString("c"), totalValue.AtFull().ToUpper());

            stringBuilder.Replace("[Items]", isRtf ? tempStringBuilder.ToString().ToRtf() : tempStringBuilder.ToString());

            stringBuilder.Replace("[TotalDaProposta]", totalValue.ToString());
            stringBuilder.Replace("[ValorTotalPorExtenso]", totalValue.AtFull().ToUpper());

            #endregion

            #region Others

            stringBuilder.Replace("[Contato]", budget.ContactName);
            stringBuilder.Replace("[DataEntrega]", budget.DeliveryDate);
            stringBuilder.Replace("[Garantia]", budget.Warranty);

            stringBuilder.Replace("[Validade]", budget.ExpirationDate.ToString());
            stringBuilder.Replace("[FormaPagamento]", budget.PaymentMethod);
            stringBuilder.Replace("[Observacao]", budget.Observation);

            stringBuilder.Replace("[Vendedor]", budget.Employee.Profile.AbreviatedName);
            stringBuilder.Replace("[EmailVendedor]", budget.Employee.Profile.Email);
            stringBuilder.Replace("[FormaEntrega]", budget.DeliveryDescription);
            stringBuilder.Replace("[Pintura/Tratamento]", budget.Treatment);

            #endregion


            return stringBuilder.ToString();
        }

        private string GetTempBudgetTemplate()
        {
            return @"
<html>
<body>
[NumeroDoOrcamento]
[DataEmissao]
[NomeDoCliente]
[Items]
[TotalDaProposta]
[ValorTotalPorExtenso]
</body>
</html>
";
        }

        /// <summary>
        /// Get the budget request template
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DocumentTemplate GetBudgetRequestDocumentTemplate(Int32 companyId)
        {
            return DbContext.DocumentTemplates.Where(d => d.CompanyId == companyId && d.DocumentTemplateTypeId == (Int32)DocumentTemplateTypes.ProspectOrBudget).FirstOrDefault();
        }

        /// <summary>
        /// This method returns the budget documents template of specified company
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <returns></returns>
        public IQueryable<DocumentTemplate> GetBudgetDocumentsTemplate(Int32 companyId)
        {
            return DbContext.DocumentTemplates.Where(d => d.CompanyId == companyId && d.DocumentTemplateTypeId == (Int32)DocumentTemplateTypes.ProspectOrBudget);
        }


        /// <summary>
        /// This method returns a single budget from the database
        /// </summary>
        /// <param name="budgetCode">The code defined by the user</param>
        /// <returns></returns>
        public Budget GetBudget(int companyId, string budgetCode)
        {
            return
                DbContext.Budgets.Where(x => x.CompanyId == companyId && x.BudgetCode.Equals(budgetCode)).FirstOrDefault
                    ();
        }

        /// <summary>
        /// This method returns a single budget from the database
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Budget GetBudget(Int32 budgetId, Int32 companyId)
        {
            return DbContext.Budgets.Where(x => x.BudgetId == budgetId && x.CompanyId == companyId).FirstOrDefault();
        }

        /// <summary>
        /// This method returns returns all budgets in the database that are exclusively budgets
        /// in other words, that have not became a sale
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
#warning Esse método não esta no padrão correto de nomenclatura
        public DataTable GetBudgetByCustomerExcludingSales(Int32 customerId, Int32 companyId)
        {
            string query =
                @" SELECT *
                                FROM         Budget AS B
                                WHERE     (CustomerId = @customerId) AND (CompanyId = @companyId) AND (NOT EXISTS
                                    (SELECT     BudgetId
                                    FROM          Sale
                                    WHERE      (BudgetId = B.BudgetId)))";

            DataManager.Parameters.Add("@customerId", customerId);
            DataManager.Parameters.Add("@companyId", companyId);
            return DataManager.ExecuteDataTable(query);
        }

        /// <summary>
        /// this method return all budgets are not sales 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetRemainingBudgets(Int32 customerId, Int32 companyId)
        {
            IQueryable<Budget> query = from budget in DbContext.Budgets
                                       where
                                           budget.CustomerId == customerId && budget.CompanyId == companyId &&
                                           !budget.Sales.Any()
                                       select budget;
            return query.ToDataTable();
        }

        /// <summary>
        /// Delete from both budgetItems and Budget a line of the database based on the budgetId
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="companyId"></param>
#warning Esse método não est� nos padr�es de nomenclatura
        public void DeleteBudgetAndBudGetItems(int budgetId, int companyId)
        {
            string budgetItemsQuery =
                @"DELETE 
                             FROM BudgetItem 
                             WHERE (BudgetId = @BudgetId)";

            DataManager.Parameters.Add("@budgetId", budgetId);
            //DataManager.Parameters.Add("@companyId", companyId);
            DataManager.ExecuteNonQuery(budgetItemsQuery);

            string budgetQuery =
                @"DELETE 
                             FROM Budget
                             WHERE (BudgetId = @BudgetId) AND (CompanyId = @companyId)";

            DataManager.Parameters.Add("@budgetId", budgetId);
            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.ExecuteNonQuery(budgetQuery);
        }

        /// <summary>
        /// This method returns Budgets
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="customerId"></param>
        /// <param name="budgetStatus"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns> 
        public IQueryable GetBudgets(Int32 companyId, Int32? customerId, BudgetStatus budgetStatus, Int32 vendorId,
                                     DateTime? beginDate, DateTime? endDate, String telephone, string sortExpression,
                                     int startRowIndex, int maximumRows)
        {
            return GetBudgets(companyId, customerId, String.Empty, budgetStatus, vendorId, beginDate, endDate, telephone, sortExpression, startRowIndex, maximumRows);
        }


        public Int32 GetBudgetsCount(Int32 companyId, Int32? customerId, BudgetStatus budgetStatus, Int32 vendorId,
                                     DateTime? beginDate, DateTime? endDate, String telephone, string sortExpression,
                                     int startRowIndex, int maximumRows)
        {
            return
                GetBudgets(companyId, customerId, budgetStatus, vendorId, beginDate, endDate, telephone, sortExpression,
                           startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }




        public IQueryable GetBudgets(Int32 companyId, Int32? customerId, String productName, BudgetStatus budgetStatus, Int32 vendorId,
                                    DateTime? beginDate, DateTime? endDate, String telephone, string sortExpression,
                                    int startRowIndex, int maximumRows)
        {
            var query = from budget in GetBudgets(companyId, customerId, null)
                        join bStatus in DbContext.BudgetStatus on budget.BudgetStatusId equals bStatus.BudgetStatusId
                            into gBudgetStatus
                        from bStatus in gBudgetStatus.DefaultIfEmpty()
                        join customer in DbContext.Customers on budget.CustomerId equals customer.CustomerId into
                            gBudget
                        from customer in gBudget.DefaultIfEmpty()
                        join profiles in DbContext.Profiles on customer.ProfileId equals profiles.ProfileId into
                            gProfiles
                        from profiles in gProfiles.DefaultIfEmpty()
                        join legalEntityProfiles in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId
                            equals legalEntityProfiles.LegalEntityProfileId into gLegalEntityProfiles
                        from legalEntityProfiles in gLegalEntityProfiles.DefaultIfEmpty()
                        select new
                        {
                            budget.CompanyId,
                            budget.BudgetId,
                            budget.VendorId,
                            budget.ModifiedDate,
                            budget.CustomerId,
                            budget.BudgetCode,
                            budget.Discount,
                            budget.ContactName,
                            budget.CreatedDate,
                            budget.Warranty,
                            budget.Observation,
                            budget.Cover,
                            budget.Summary,
                            budget.ExpirationDate,
                            budget.PaymentMethod,
                            budget.AdditionalCost,
                            budget.IPI,
                            budget.DeliveryDate,
                            budget.DeliveryDescription,
                            budget.DiscountType,
                            budget.BudgetStatusId,
                            BudgetStatus = bStatus.Name,
                            budget.BudgetItems,
                            CustomerName = profiles.Name ?? legalEntityProfiles.CompanyName ?? budget.CustomerName,
                            customer.Profile,
                            customer.LegalEntityProfile,
                            BudgetValue = budget.BudgetItems.Sum(bi => bi.Quantity * bi.UnitPrice)
                                         - (budget.Discount ?? 0)
                                         + (budget.AdditionalCost ?? 0)
                        };

            switch (budgetStatus)
            {
                case BudgetStatus.Open:
                case BudgetStatus.Accepted:
                case BudgetStatus.Rejected:
                case BudgetStatus.SentToCustomer:
                    query = query.Where(b => b.BudgetStatusId == (int)budgetStatus);
                    break;
            }

            if (beginDate != null)
                query = query.Where(q => q.CreatedDate >= beginDate);

            if (endDate != null)
                query = query.Where(q => q.CreatedDate <= endDate);

            if (vendorId != 0)
                query = query.Where(v => v.VendorId.Equals(vendorId));

            if (!String.IsNullOrEmpty(productName))
                query = query.Where(x => x.BudgetItems.Any(bi => bi.ProductId.HasValue && bi.SpecialProductName.Contains(productName)));

            if (!String.IsNullOrEmpty(telephone))
                query =
                    query.Where(t => t.Profile.Phone.Equals(telephone) || t.LegalEntityProfile.Phone.Equals(telephone));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "CreatedDate desc");
        }


        public Int32 GetBudgetsCount(Int32 companyId, Int32? customerId, String productName, BudgetStatus budgetStatus, Int32 vendorId,
                                  DateTime? beginDate, DateTime? endDate, String telephone, string sortExpression,
                                  int startRowIndex, int maximumRows)
        {
            return GetBudgets(companyId, customerId, productName, budgetStatus, vendorId, beginDate, endDate, telephone, sortExpression, startRowIndex, maximumRows).Cast<object>().Count();

        }






        public IQueryable GetBudgets(Int32 companyId, Int32? customerId, BudgetStatus budgetStatus,
                                     string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetBudgets(companyId, customerId, budgetStatus, 0, null, null, String.Empty, sortExpression,
                              startRowIndex, maximumRows);
        }


        /// <summary>
        /// This method retrieves all Budgets.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Budget> GetAllBudgets()
        {
            return DbContext.Budgets;
        }

        /// <summary>
        /// This method gets record counts of all Budgets.
        /// Do not change this method.
        /// </summary>
        public int GetAllBudgetsCount()
        {
            return GetAllBudgets().Count();
        }

        /// <summary>
        /// This method retrieves a single Budget.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=BudgetId>BudgetId</param>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=CustomerId>CustomerId</param>
        public Budget GetBudget(Int32 BudgetId, Int32 CompanyId, Int32 CustomerId)
        {
            return
                DbContext.Budgets.Where(
                    x => x.BudgetId == BudgetId && x.CompanyId == CompanyId && x.CustomerId == CustomerId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Budget by Customer.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Budget> GetBudgetByCustomer(Int32 CustomerId, Int32 CompanyId)
        {
            return DbContext.Budgets.Where(x => x.CustomerId == CustomerId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Budgets filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetBudgetsAsList(string tableName, Int32 Customer_CustomerId, Int32 Customer_CompanyId,
                                      string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Budget> x = GetFilteredBudgets(tableName, Customer_CustomerId, Customer_CompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "BudgetId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Budget> GetFilteredBudgets(string tableName, Int32 Customer_CustomerId,
                                                      Int32 Customer_CompanyId)
        {
            switch (tableName)
            {
                case "Customer_Budgets":
                    return GetBudgetByCustomer(Customer_CustomerId, Customer_CompanyId);
                default:
                    return GetAllBudgets();
            }
        }

        /// <summary>
        /// This method gets records counts of all Budgets filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetBudgetsCount(string tableName, Int32 Customer_CustomerId, Int32 Customer_CompanyId)
        {
            IQueryable<Budget> x = GetFilteredBudgets(tableName, Customer_CustomerId, Customer_CompanyId);
            return x.Count();
        }

        /// <summary>
        /// this method return accepted budgets
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetAcceptedBudgets(int companyId, Int32? customerId, string sortExpression, int startRowIndex,
                                             int maximumRows)
        {
            var query = from budget in GetAcceptedBudgets(companyId, customerId)
                        join budgetStatus in DbContext.BudgetStatus on budget.BudgetStatusId equals
                            budgetStatus.BudgetStatusId into gBudgetStatus
                        from budgetStatus in gBudgetStatus.DefaultIfEmpty()
                        join customer in DbContext.Customers on budget.CustomerId equals customer.CustomerId into
                            gBudget
                        from customer in gBudget.DefaultIfEmpty()
                        join profiles in DbContext.Profiles on customer.ProfileId equals profiles.ProfileId into
                            gProfiles
                        from profiles in gProfiles.DefaultIfEmpty()
                        join legalEntityProfiles in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId
                            equals legalEntityProfiles.LegalEntityProfileId into gLegalEntityProfiles
                        from legalEntityProfiles in gLegalEntityProfiles.DefaultIfEmpty()
                        select new
                        {
                            budget.CompanyId,
                            budget.BudgetId,
                            budget.VendorId,
                            budget.ModifiedDate,
                            budget.CustomerId,
                            budget.BudgetCode,
                            budget.Discount,
                            budget.ContactName,
                            budget.CreatedDate,
                            budget.Warranty,
                            budget.Observation,
                            budget.Cover,
                            budget.Summary,
                            budget.ExpirationDate,
                            budget.PaymentMethod,
                            budget.AdditionalCost,
                            budget.IPI,
                            budget.DeliveryDate,
                            budget.DeliveryDescription,
                            budget.DiscountType,
                            BudgetStatus = budgetStatus.Name,
                            CustomerName = profiles.Name ?? legalEntityProfiles.CompanyName,
                            BudgetValue = budget.BudgetItems.Sum(bi => bi.Quantity * bi.UnitPrice)
                        };
            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "BudgetId");
        }

        public Int32 GetAcceptedBudgetsCount(int companyId, Int32? customerId, string sortExpression, int startRowIndex,
                                             int maximumRows)
        {
            return
                GetAcceptedBudgets(companyId, customerId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        /// <summary>
        /// this method return all accepted budgets
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private IQueryable<Budget> GetAcceptedBudgets(Int32 companyId, Int32? customerId)
        {
            var saleManager = new SaleManager(this);
            IQueryable<Budget> budgetInSale = from sale in DbContext.Sales
                                              where sale.CompanyId == companyId
                                              select sale.Budget;
            if (customerId.HasValue)
                budgetInSale = budgetInSale.Where(budget => budget.CustomerId == customerId);

            return budgetInSale;
        }

        /// <summary>
        /// this method return budgets not accepted
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetNotAcceptedBudgets(int companyId, Int32? customerId, string sortExpression,
                                                int startRowIndex, int maximumRows)
        {
            var query =
                from budget in GetBudgets(companyId, customerId, null).Except(GetAcceptedBudgets(companyId, customerId))
                join customer in DbContext.Customers on budget.CustomerId equals customer.CustomerId into gBudget
                from customer in gBudget.DefaultIfEmpty()
                join profiles in DbContext.Profiles on customer.ProfileId equals profiles.ProfileId into gProfiles
                from profile in gProfiles.DefaultIfEmpty()
                join legalEntityProfiles in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                    legalEntityProfiles.LegalEntityProfileId into gLegalEntityProfiles
                from legalEntityProfile in gLegalEntityProfiles.DefaultIfEmpty()
                select new
                {
                    budget.CompanyId,
                    budget.BudgetId,
                    budget.VendorId,
                    budget.ModifiedDate,
                    budget.CustomerId,
                    budget.BudgetCode,
                    budget.Discount,
                    budget.ContactName,
                    budget.CreatedDate,
                    budget.Warranty,
                    budget.Observation,
                    budget.Cover,
                    budget.Summary,
                    budget.ExpirationDate,
                    budget.PaymentMethod,
                    budget.AdditionalCost,
                    budget.IPI,
                    budget.DeliveryDate,
                    budget.DeliveryDescription,
                    budget.DiscountType,
                    CustomerName = profile.Name ?? legalEntityProfile.CompanyName,
                    BudgetValue = budget.BudgetItems.Sum(bi => bi.Quantity * bi.UnitPrice)
                };

            if (customerId.HasValue)
                query = query.Where(budget => budget.CustomerId == customerId);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "BudgetId");
        }

        public Int32 GetNotAcceptedBudgetsCount(int companyId, Int32? customerId, string sortExpression,
                                                int startRowIndex, int maximumRows)
        {
            return
                GetNotAcceptedBudgets(companyId, customerId, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        public Decimal CalculateBudgetDiscount(Budget entity)
        {
            if (!entity.Discount.HasValue)
                return Decimal.Zero;

            if (entity.DiscountType == (Int32)DiscountType.Cash)
                return entity.Discount.Value;
            else
                return
                    Convert.ToDecimal(entity.BudgetItems.Sum(bItem => bItem.Quantity * bItem.UnitPrice) *
                                      entity.Discount.Value / 100);
        }

        /// <summary>
        /// This method modify budget status of a specific budget
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="budgetId"></param>
        /// <param name="budgetStatusId"></param>
        public void SetBudgetStatus(Int32 companyId, Int32 budgetId, Int32 budgetStatusId)
        {
            Budget budget = GetBudget(budgetId, companyId);
            budget.BudgetStatusId = budgetStatusId;
            DbContext.SubmitChanges();
        }

        #region BudgetItemManager

        /// <summary>
        /// This method retrieves all BudgetItems.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<BudgetItem> GetAllBudgetItems()
        {
            return DbContext.BudgetItems;
        }

        /// <summary>
        /// This method gets record counts of all BudgetItems.
        /// Do not change this method.
        /// </summary>
        public int GetAllBudgetItemsCount()
        {
            return GetAllBudgetItems().Count();
        }

        /// <summary>
        /// this method return a single BudgetItem
        /// </summary>
        /// <param name="BudgetId">BudgetId</param>
        /// <param name="BudgetItemId">BudgetItemId</param>
        /// <returns></returns>
        public BudgetItem GetBudgetItem(Int32 BudgetId, Int32 BudgetItemId)
        {
            return
                DbContext.BudgetItems.Where(i => i.BudgetId == BudgetId && i.BudgetItemId == BudgetItemId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves BudgetItem by Budget.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=BudgetId>budgetId</param>
        /// <param name=CompanyId>companyId</param>      
        public IQueryable<BudgetItem> GetBudgetItemByBudget(Int32 budgetId, Int32 companyId)
        {
            return DbContext.BudgetItems.Where(x => x.BudgetId == budgetId && x.Budget.CompanyId == companyId);
        }

        /// <summary>
        /// This method retrieves BudgetItem by Product.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ItemId>ItemId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<BudgetItem> GetBudgetItemByProduct(Int32 ItemId, Int32 CompanyId)
        {
            return DbContext.BudgetItems.Where(x => x.ProductId == ItemId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all BudgetItems filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetBudgetItems(string tableName, Int32 Budget_BudgetId, Int32 Budget_CompanyId,
                                    Int32 Budget_CustomerId, Int32 Product_ItemId, Int32 Product_CompanyId,
                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<BudgetItem> x = GetFilteredBudgetItems(tableName, Budget_BudgetId, Budget_CompanyId,
                                                              Budget_CustomerId, Product_ItemId, Product_CompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "BudgetId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<BudgetItem> GetFilteredBudgetItems(string tableName, Int32 Budget_BudgetId,
                                                              Int32 Budget_CompanyId, Int32 Budget_CustomerId,
                                                              Int32 Product_ItemId, Int32 Product_CompanyId)
        {
            switch (tableName)
            {
                case "Budget_BudgetItems":
                    return GetBudgetItemByBudget(Budget_BudgetId, Budget_CompanyId);
                case "Product_BudgetItems":
                    return GetBudgetItemByProduct(Product_ItemId, Product_CompanyId);
                default:
                    return GetAllBudgetItems();
            }
        }

        /// <summary>
        /// This method gets records counts of all BudgetItems filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetBudgetItemsCount(string tableName, Int32 Budget_BudgetId, Int32 Budget_CompanyId,
                                       Int32 Budget_CustomerId, Int32 Product_ItemId, Int32 Product_CompanyId)
        {
            IQueryable<BudgetItem> x = GetFilteredBudgetItems(tableName, Budget_BudgetId, Budget_CompanyId,
                                                              Budget_CustomerId, Product_ItemId, Product_CompanyId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void DeleteBudgetItem(BudgetItem entity)
        {
            DbContext.BudgetItems.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes all budgetItems by budgetId
        /// </summary>
        /// <param name="budgetId"></param>
        public void DeleteBudgetItems(Int32 companyId, Int32 budgetId)
        {
            DbContext.BudgetItems.DeleteAllOnSubmit(GetBudgetItemByBudget(companyId, budgetId));
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void InsertBudgetItem(BudgetItem entity)
        {
            if (entity.ProductId == 0)
                entity.ProductId = null;
            entity.ModifiedDate = DateTime.Now;
            DbContext.BudgetItems.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary> 
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void UpdateBudgetItem(BudgetItem original_entity, BudgetItem entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        public IQueryable<Budget> GetBudgets(Int32 companyId, Int32? customerId, DateTimeInterval interval)
        {
            IQueryable<Budget> query = from budget in DbContext.Budgets
                                       where budget.CompanyId == companyId
                                       select budget;

            if (customerId.HasValue)
                query = query.Where(c => c.CustomerId == customerId);

            if (interval != null)
                query = query.Where(b => b.CreatedDate >= interval.BeginDate && b.CreatedDate <= interval.EndDate);

            return query;
        }

        private IQueryable GetBudgets(Int32 companyId, Int32? customerId, string sortExpression, int startRowIndex,
                                     int maximumRows)
        {
            var query = from budget in GetBudgets(companyId, customerId, null)
                        join customer in DbContext.Customers on budget.CustomerId equals customer.CustomerId into
                            gBudgets
                        from customer in gBudgets.DefaultIfEmpty()
                        join profiles in DbContext.Profiles on customer.ProfileId equals profiles.ProfileId into
                            gProfiles
                        from profile in gProfiles.DefaultIfEmpty()
                        join legalEntityProfiles in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId
                            equals legalEntityProfiles.LegalEntityProfileId into gLegalEntityProfiles
                        from legalEntityProfile in gLegalEntityProfiles.DefaultIfEmpty()
                        select new
                        {
                            budget.CompanyId,
                            budget.BudgetId,
                            budget.VendorId,
                            budget.ModifiedDate,
                            budget.CustomerId,
                            budget.BudgetCode,
                            budget.Discount,
                            budget.ContactName,
                            budget.CreatedDate,
                            budget.Warranty,
                            budget.Observation,
                            budget.Cover,
                            budget.Summary,
                            budget.ExpirationDate,
                            budget.PaymentMethod,
                            budget.AdditionalCost,
                            budget.IPI,
                            budget.DeliveryDate,
                            budget.DeliveryDescription,
                            budget.DiscountType,
                            budget.BudgetStatusId,
                            CustomerName = profile.Name ?? legalEntityProfile.CompanyName ?? budget.CustomerName,
                            BudgetValue = budget.BudgetItems.Sum(bi => bi.Quantity * bi.UnitPrice)
                                        - (budget.Discount ?? 0)
                                        + (budget.AdditionalCost ?? 0)
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "BudgetId");
        }

        /// <summary>
        /// this method return the count of GetBudgets
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetBudgetsCount(Int32 companyId, Int32? customerId, string sortExpression, int startRowIndex,
                                     int maximumRows)
        {
            return
                GetBudgets(companyId, customerId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// This method sends the budget to customer by email 
        /// </summary>
        /// <param name="budgetId"> can't be null</param>
        /// <param name="company">can't be null</param>
        /// <param name="addressFileToAttach"> address of file to attach in email. 
        /// If this parameter is null, the budget(at .html version) will send in body of email.
        /// </param>
        public void SendBudgetToCustomer(int budgetId, Company company, Int32 budgetDocumentTemplateId, string addressFileToAttach)
        {
            Budget budget = GetBudget(budgetId, company.CompanyId);

            budget.BudgetStatusId = (int)BudgetStatus.SentToCustomer;
            DbContext.SubmitChanges();

            string customerMail = budget.CustomerMail ?? budget.Customer.Email;
            string mailBody = String.Empty;

            // case not exist file to attach, the budget is sended in body's email 
            if (String.IsNullOrEmpty(addressFileToAttach))
            {
                mailBody = ApplyBudgetTemplate(budget, budgetDocumentTemplateId).Replace("</body>",
                   "<br /><br />Caso tenha gostado do orçamento e deseja concluir a compra " +
                            "basta clicar no link abaixo:<br />" +
                            "<a href='http://" + company.LegalEntityProfile.Website + "/site/Checkout_Basket.aspx?b=" + budgetId.EncryptToHex() +
                            " '>Efetuar Compra</a> ou copie o seguinte endereço no seu navegador: " +
                            company.LegalEntityProfile.Website + "/site/Checkout_Basket.aspx?b=" + budgetId.EncryptToHex() + "</body>");
            }
            else
            {
                mailBody = "<br />Caso tenha gostado do orçamento(veja o orçamento no arquivo em anexo) e deseja concluir a compra " +
                                "basta clicar no link abaixo:<br />" +
                                "<a href='http://" + company.LegalEntityProfile.Website + "/site/Checkout_Basket.aspx?b=" + budgetId.EncryptToHex() +
                                " '>Efetuar Compra</a> ou copie o seguinte endereço no seu navegador: " +
                                company.LegalEntityProfile.Website + "/site/Checkout_Basket.aspx?b=" + budgetId.EncryptToHex();
            }

            Postman.Send(
                "mailer@vivina.com.br",
                customerMail,
                "Orçamento solicitado no site " + company.LegalEntityProfile.Website,
                mailBody, new[] { addressFileToAttach });
        }

        /// <summary>
        /// This method verify if exist a specific documentTemplate in a company.
        /// Return true if the company has the specified documentTemplate.
        /// </summary>
        /// <param name="companyId">can't be null</param>
        /// <param name="documentTemplateType"> the documentTemplate for to check if his exist in company specified </param>
        /// <returns></returns>
        public bool HasDocumentTemplateInCompany(Int32 companyId, DocumentTemplateTypes documentTemplateType)
        {
            return DbContext.DocumentTemplates.Where(documentTemplate => documentTemplate.CompanyId == companyId && documentTemplate.DocumentTemplateTypeId == Convert.ToInt32(documentTemplateType)) != null;
        }

        #endregion

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Budget entity)
        {
            //DbContext.Budgets.Attach(entity);
            DbContext.Budgets.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Save the Budget
        /// </summary>
        /// <param name="budget"></param>
        /// <param name="budgetItems"></param>
        public Budget SaveBudget(Budget budget, List<BudgetItem> budgetItems)
        {
            var originalBudget = GetBudget(budget.BudgetId, budget.CompanyId);
            if (originalBudget == null)
                InsertBudget(originalBudget = budget, budgetItems);
            else
                UpdateBudget(originalBudget, budget, budgetItems);

            return originalBudget;
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        private void InsertBudget(Budget budget, List<BudgetItem> budgetItems)
        {
            if (!budget.BudgetStatusId.HasValue)
                budget.BudgetStatusId = (int)BudgetStatus.Open;

            budget.CreatedDate = DateTime.Now;
            budget.ModifiedDate = DateTime.Now;

            DbContext.Budgets.InsertOnSubmit(budget);
            DbContext.SubmitChanges();

            if (String.IsNullOrEmpty(budget.BudgetCode))
            {
                budget.BudgetCode = GetBudgets(budget.CompanyId, null, DateTimeInterval.ThisYear).Count() + "-" + DateTime.Now.Year;

                DbContext.SubmitChanges();
            }

            if (budgetItems != null)
                foreach (var item in budgetItems)
                {
                    item.BudgetId = budget.BudgetId;
                    InsertBudgetItem(item);
                }
        }

        /// <summary>
        /// this method did not remove all budgetItems anymore and update the budget
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        private void UpdateBudget(Budget original_entity, Budget entity, List<BudgetItem> budgetItems)
        {
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();

            //
            //  Update budgetItems
            // 
            foreach (var budgetItem in original_entity.BudgetItems.ToArray())
                DeleteBudgetItem(budgetItem);

            foreach (var item in budgetItems)
            {
                item.BudgetId = original_entity.BudgetId;
                InsertBudgetItem(item.Detach());
            }

            DbContext.SubmitChanges();
        }

        #endregion
    }
}