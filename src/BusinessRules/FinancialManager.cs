using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BoletoNet;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules.Accounting;
using Vivina.Erp.DataClasses;
using System.Text;
using System.Collections;

namespace Vivina.Erp.BusinessRules
{
#warning procurar a tradução de guia de recolhimento e trocar o enum e as telas
    public enum DocumentType
    {
        Guia = 1,
        receipt = 2,
        others = 3
    }


    public class FinancialManager : ParcelsManager
    {
        public FinancialManager(IDataAccessor container)
            : base(container)
        {

        }


        #region Invoice

        #region CRUD

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Invoice entity)
        {
            DbContext.Invoices.Attach(entity);
            DbContext.Invoices.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Invoice entity, IList<Parcel> parcels)
        {
            entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
            InsertInvoice(entity);

            var parcelsManager = new ParcelsManager(this);
            foreach (Parcel item in parcels)
            {
                item.InvoiceId = entity.InvoiceId;
                item.CompanyId = entity.CompanyId;
                DbContext.Parcels.InsertOnSubmit(item);
            }
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        [Obsolete("This method isn't correct 'cause it doesn't update the parcels")]
        public void Update(Invoice original_entity, Invoice entity)
        {
            var parcelsManager = new ParcelsManager(this);
            parcelsManager.UpdateParcels(
                parcelsManager.GetInvoiceParcels(original_entity.CompanyId, original_entity.InvoiceId),
                entity.Parcels.AsQueryable());
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates invoice's parcel
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        /// <param name="parcels"></param>
        public void Update(Invoice original_entity, Invoice entity, IList<Parcel> parcels)
        {
            var parcelsManager = new ParcelsManager(this);
            original_entity.CopyPropertiesFrom(entity);
            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();

            parcelsManager.UpdateParcels(
                parcelsManager.GetInvoiceParcels(original_entity.CompanyId, original_entity.InvoiceId), parcels);
        }

        #endregion

        /// <summary>
        /// This method retrieves Invoices by Sale.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=BudgetId>BudgetId</param>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=UserId>UserId</param>
        public IQueryable GetInvoicesBySale(Int32 CompanyId, Int32 BudgetId, Int32 CustomerId)
        {
            IQueryable<Receipt> query = from invoice in DbContext.Receipts
                                        join receipts in DbContext.Receipts on invoice.ReceiptId equals
                                            receipts.ReceiptId
                                        join sale in DbContext.Sales on receipts.ReceiptId equals sale.ReceiptId
                                        where
                                            invoice.CompanyId == CompanyId && invoice.CustomerId == CustomerId &&
                                            sale.SaleId == BudgetId
                                        select invoice;
            return query.AsQueryable();
        }


        #region Advanced CRUD

        /// <summary>
        /// Delete the Invoice and the related Parcels from the Database.
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="companyId"></param>
        public void DeleteInvoice(int invoiceId, int companyId)
        {
            //
            //If exists a sale by this invoice, then set invoiceId with null
            //
            var saleManager = new SaleManager(this);
            Sale original_sale = saleManager.GetSaleByInvoice(companyId, invoiceId);
            if (original_sale != null)
            {
                var sale = new Sale();
                sale.CopyPropertiesFrom(original_sale);
                sale.InvoiceId = null;

                saleManager.Update(original_sale, sale);
            }

            var parcelsManager = new ParcelsManager(this);
            parcelsManager.DeleteInvoiceParcels(invoiceId);

            DbContext.Invoices.DeleteOnSubmit(
                DbContext.Invoices.Where(i => i.CompanyId == companyId && i.InvoiceId == invoiceId).FirstOrDefault());
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method return a invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public Invoice GetInvoice(Int32 companyId, Int32 invoiceId)
        {
            return DbContext.Invoices.Where(i => i.CompanyId == companyId && i.InvoiceId == invoiceId).FirstOrDefault();
        }

        /// <summary>
        /// This method inserts a Invoice, returning a Invoice Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertRetrievingId(Invoice entity)
        {
            InsertInvoice(entity);
            return entity.InvoiceId;
        }


        /// <summary>
        /// This method inserts a invoice
        /// </summary>
        /// <param name="entity"></param>
        private void InsertInvoice(Invoice entity)
        {
            if (entity.CostCenterId.HasValue &&
                new AccountManager(this).GetCostCenter(entity.CostCenterId.Value) == null)
                entity.CostCenterId = null;

            DbContext.Invoices.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        #region GetInvoices-overload

        /// <summary>
        /// this is the Generic method used to execute all others queries
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="closed"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
#warning Este método está duplicado
        private IQueryable GetAllInvoices(Int32 companyId, IAccountSearch accountSearch, Boolean? closed,
                                          string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetInvoices(companyId, accountSearch, closed, sortExpression, startRowIndex, maximumRows);
        }


        private IQueryable<Invoice> GetInvoicesByAccountingPlan(Int32 companyId, Int32 accountingPlanId)
        {
            var query = from invoice in DbContext.Invoices
                        join accountingPlan in DbContext.AccountingPlanTree(accountingPlanId, companyId)
                        on invoice.AccountingPlanId equals accountingPlan.AccountingPlanId
                        select invoice;

            return query;
        }

#warning este método está duplicado
        /// <summary>
        /// this is the Generic method used to execute all others queries
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="closed"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetInvoices(Int32 companyId, IAccountSearch accountSearch, Boolean? closed,
                                       string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Invoice> invoices;

            if (accountSearch.AccountPlanId.HasValue)
                invoices = GetInvoicesByAccountingPlan(companyId, Convert.ToInt32(accountSearch.AccountPlanId));
            else
                invoices = GetInvoicesByCompany(companyId);

            var query = from invoice in invoices
                        join parcel in DbContext.Parcels on invoice.InvoiceId equals parcel.InvoiceId
                        orderby parcel.DueDate
                        select new
                        {
                            invoice.AccountingPlanId,
                            invoice.InvoiceId,
                            invoice.CompanyId,
                            invoice.EntryDate,
                            invoice.Description,
                            invoice.CustomerId,
                            invoice.CostCenterId,
                            DueDate = parcel.DueDate,
                            Desc = parcel.Description,
                            Amount = parcel.EffectedAmount ?? parcel.Amount,
                            parcel.EffectedAmount,
                            parcel.EffectedDate,
                            parcel.AccountId,
                            parcel.IdentificationNumber,
                            customerName =
                 (invoice.Customer.LegalEntityProfile.CompanyName ?? "") +
                 (invoice.Customer.Profile.Name ?? "")
                        };

            if (accountSearch != null)
            {
                // Interval of date
                if (accountSearch.dateTimeInterval != null && accountSearch.dateTimeInterval.BeginDate != null)
                    query = query.Where(inv => inv.DueDate >= accountSearch.dateTimeInterval.BeginDate);

                if (accountSearch.dateTimeInterval != null && accountSearch.dateTimeInterval.EndDate != null)
                    query = query.Where(inv => inv.DueDate <= accountSearch.dateTimeInterval.EndDate);

                //Cost Center
                if (accountSearch.CostCenterId.HasValue && accountSearch.CostCenterId.Value != Decimal.Zero)
                    query = query.Where(inv => inv.CostCenterId == accountSearch.CostCenterId);

                //Customer/Supplier name
                if (!String.IsNullOrEmpty(accountSearch.Name))
                    query = query.Where(inv => inv.customerName.Contains(accountSearch.Name));

                //Identification
                if (!String.IsNullOrEmpty(accountSearch.Identification))
                    query = query.Where(inv => inv.IdentificationNumber.Contains(accountSearch.Identification));

                //Account
                if (accountSearch.AccountId.HasValue)
                    query = query.Where(inv => inv.AccountId.Equals(accountSearch.AccountId));

                //ParcelValue
                if (accountSearch.ParcelValue.HasValue)
                    query =
                        query.Where(
                            inv =>
                            inv.Amount.Equals(accountSearch.ParcelValue) ||
                            inv.EffectedAmount.Equals(accountSearch.ParcelValue));

                if (accountSearch.ParcelStatus == (int?)ParcelStatus.EXPIRED)
                    query =
                        query.Where(
                            inv =>
                            inv.DueDate < DateTime.Now && inv.EntryDate > accountSearch.dateTimeInterval.BeginDate);

                if (accountSearch.ParcelStatus == (int?)ParcelStatus.EXPIRING)
                    query = query.Where(inv => inv.DueDate.Month == DateTime.Now.Month && inv.DueDate >= DateTime.Now);

                //if closed is not null then compares IsOpen with Closed
                if (!closed.HasValue)
                    query = query.AsQueryable();
                else if (closed.Value)
                    query = query.Where(p => p.EffectedAmount > 0 && p.EffectedDate.HasValue);
                else
                    query = query.Where(p => p.EffectedDate == null || p.EffectedAmount == null);
            }

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate").AsQueryable();
        }


        /// <summary>
        /// Returns the next parcel of each Invoice, with a smaller due date, by companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetInvoices(int companyId)
        {
            string selectInvoice =
                @"SELECT     Invoice.*, Parcel.ParcelId, Parcel.DueDate,
                         Parcel.Description, Parcel.Amount
              FROM         
                         Invoice 
              INNER JOIN
                         Parcel ON Invoice.InvoiceId = Parcel.InvoiceId              
              INNER JOIN
                    (SELECT     
                            InvoiceId, MIN(DueDate) AS duedate
                     FROM          Parcel AS p1
                     WHERE      (InvoiceId IS NOT NULL) 
                     GROUP BY InvoiceId) AS p2 ON Parcel.DueDate = p2.duedate AND Invoice.InvoiceId = p2.InvoiceId
              WHERE     (Invoice.CompanyId = @companyId)
              ORDER BY Parcel.DueDate";

            DataManager.Parameters.Add("@companyId", companyId);
            return DataManager.ExecuteDataTable(selectInvoice);
        }

        /// <summary>
        /// this is the Generic Method
        /// </summary>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="companyId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="parcelStatus"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetInvoices(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                      int startRowIndex, int maximumRows)
        {
            IQueryable query = null;

            switch (accountSearch.ParcelStatus)
            {
                case ParcelStatus.OPEN:
                    query = GetOpenInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                case ParcelStatus.CLOSED:
                    query = GetClosedInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                case ParcelStatus.EXPIRED:
                    query = GetExpiredInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                case ParcelStatus.EXPIRING:
                    query = GetExpiringInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                default:
                    query = GetAllInvoices(companyId, accountSearch, null, sortExpression, startRowIndex, maximumRows);
                    break;
            }
            return query;
        }

        /// <summary>
        /// This method return the count of GetInvoices that contains this assigned
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountSearch"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetInvoicesCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                      int startRowIndex, int maximumRows)
        {
            return
                GetInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        /// <summary>
        /// this is the count method of GetInvoices
        /// </summary>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="companyId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="parcelStatus"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetInvoicesCount(Int32 companyId, IAccountSearch accountSearch, bool closed, string sortExpression,
                                      int startRowIndex, int maximumRows)
        {
            return
                GetAllInvoices(companyId, accountSearch, null, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        #endregion

        #region  filterMethods

        //All

        /// <summary>
        /// this method return all invoices
        /// </summary>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetAllInvoices(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetInvoices(companyId, null, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetInvoicesByCompany
        /// </summary>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetAllInvoicesCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                (Int32)GetAllInvoices(companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        //Expired

        /// <summary>
        /// this method return all expired invoices
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetExpiredInvoices(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                              int startRowIndex, int maximumRows)
        {
            return
                GetAllInvoices(companyId, accountSearch, false, sortExpression, startRowIndex, maximumRows).AsQueryable();
        }

        /// <summary>
        /// this Method return the count of expiredInvoices
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetExpiredInvoicesCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                              int startRowIndex, int maximumRows)
        {
            return
                GetExpiredInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        //Expiring

        /// <summary>
        /// return all expiring invoices(Current month)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetExpiringInvoices(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                               int startRowIndex, int maximumRows)
        {
            accountSearch.dateTimeInterval =
                new DateTimeInterval(DateTime.Parse("01" + DateTime.Now.ToString("/MM/yyyy")),
                                     DateTime.Now.AddMonths(1).AddDays(DateTime.Now.Day * -1));
            // return GetInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
            return GetAllInvoices(companyId, accountSearch, false, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// This method return quantity of records of GetExpiringInvoices
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetExpiringInvoicesCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                               int startRowIndex, int maximumRows)
        {
            return
                GetExpiringInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        //Closed

        /// <summary>
        /// this method return all clodes invoices
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetClosedInvoices(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                             int startRowIndex, int maximumRows)
        {
            return GetAllInvoices(companyId, accountSearch, true, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return the total of rows
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetClosedInvoicesCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                             int startRowIndex, int maximumRows)
        {
            return
                GetClosedInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>
                    ().Count();
        }

        //Open

        /// <summary>
        /// this method return all open invoices
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetOpenInvoices(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                           int startRowIndex, int maximumRows)
        {
            return GetAllInvoices(companyId, accountSearch, false, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return the total rows
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetOpenInvoicesCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                           int startRowIndex, int maximumRows)
        {
            return
                GetOpenInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        #endregion

        #region ExportBoletoNet

        public void GerarArquivoRemessa(Int32 companyId, Int32 accountId, DateTime beginDate, DateTime endDate,
                                        Stream stream)
        {
            var accountManager = new AccountManager(this);
            var companyManager = new CompanyManager(this);

            Company company = companyManager.GetCompany(companyId);
            FinancierOperation operation = accountManager.GetFinancierOperationBoleto(companyId);
            Boletos boletos = ConvertInvoiceParcelsInBoleto(companyId, accountId, beginDate, endDate);
            var exportFile = new ArquivoRemessa(TipoArquivo.CNAB400);
            var cedente = new Cedente(company.LegalEntityProfile.CNPJ, company.LegalEntityProfile.CompanyName,
                                      operation.Account.Agency, Convert.ToString(operation.Account.AgencyDigit), operation.Account.AccountNumber,
                                      Convert.ToString(operation.Account.AccountNumberDigit));


            exportFile.GerarArquivoRemessa(String.Empty,
                                           new Banco(Convert.ToInt32(operation.Account.Bank.BankNumber)), cedente, boletos, stream,
                                           1);
        }

        private Boletos ConvertInvoiceParcelsInBoleto(Int32 companyId, Int32 accountId, DateTime beginDate,
                                                      DateTime endDate)
        {
            var customerManager = new CustomerManager(this);
            var profileManager = new ProfileManager(this);
            var companyManager = new CompanyManager(this);
            var accountManager = new AccountManager(this);
            Boleto boleto;
            Sacado sacado;
            Endereco endereco;
            var address = new Address();
            Company company = companyManager.GetCompany(companyId);
            Account account = accountManager.GetAccount(accountId, companyId);
            var boletos = new Boletos();

            var cedente = new Cedente(company.LegalEntityProfile.CNPJ, company.LegalEntityProfile.CompanyName,
                                      account.Agency, Convert.ToString(account.AgencyDigit), account.AccountNumber,
                                      Convert.ToString(account.AccountNumberDigit));


            foreach (Parcel parcel in GetOpenInvoiceParcelInPeriodByAccount(companyId, accountId, beginDate, endDate))
            {
                endereco = new Endereco();

                if (parcel.Invoice.Customer.LegalEntityProfileId.HasValue)
                {
                    //Address
                    address = parcel.Invoice.Customer.LegalEntityProfile.Address;

                    endereco.Numero = parcel.Invoice.Customer.LegalEntityProfile.AddressNumber;
                    endereco.Complemento = parcel.Invoice.Customer.LegalEntityProfile.AddressComp;

                    //sacado
                    sacado = new Sacado(parcel.Invoice.Customer.LegalEntityProfile.CNPJ,
                                        parcel.Invoice.Customer.LegalEntityProfile.CompanyName);
                }
                else
                {
                    //Address
                    address = parcel.Invoice.Customer.Profile.Address;

                    endereco.Numero = parcel.Invoice.Customer.Profile.AddressNumber;
                    endereco.Complemento = parcel.Invoice.Customer.Profile.AddressComp;

                    //sacado
                    sacado = new Sacado(parcel.Invoice.Customer.Profile.CPF, parcel.Invoice.Customer.Profile.Name);
                }

                //Address
                endereco.Bairro = address.Neighborhood;
                endereco.CEP = address.PostalCode;
                endereco.Cidade = address.City ?? String.Empty;
                endereco.Logradouro = address.Name;
                endereco.UF = address.State;

                //boleto
                boleto = new Boleto(parcel.DueDate, Convert.ToDouble(parcel.Amount), String.Empty, String.Empty, cedente);

                sacado.Endereco = endereco;

                boleto.Sacado = sacado;

                var instrucao = new Instrucao(Convert.ToInt32(account.Bank.BankNumber));

                var banco = new Banco(Convert.ToInt32(account.Bank.BankNumber));

                instrucao.Banco = banco;
                instrucao.QuantidadeDias = 0;
                instrucao.Descricao = String.Empty;
                instrucao.Codigo = 0;

                boleto.CodigoBarra.LinhaDigitavel = String.Empty;
                boleto.DataDocumento = DateTime.Now;
                boleto.DataVencimento = parcel.DueDate;
                boleto.ValorDesconto = 0;

                boleto.Instrucoes = new List<IInstrucao>();
                boleto.Instrucoes.Add(instrucao);
                boletos.Add(boleto);
            }
            return boletos;
        }

        #endregion

        /// <summary>
        /// this method returns expired invoices in period
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetExpiredInvoicesInPeriod(Int32 companyId, IAccountSearch accountSearch,
                                                     String sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            return GetInvoices(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetExpiredInvoicesInPeriod
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetExpiredInvoicesInPeriodCount(Int32 companyId, IAccountSearch accountSearch,
                                                     String sortExpression, Int32 startRowIndex, Int32 maximumRows)
        {
            return
                GetExpiredInvoicesInPeriod(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        /// <summary>
        /// This method retrieves Invoices by Company.
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IQueryable<Invoice> GetInvoicesByCompany(Int32 CompanyId)
        {
            return DbContext.Invoices.Where(invoice => invoice.CompanyId == CompanyId);
        }

        public IQueryable<Invoice> GetInvoicesByCompany(Int32 companyId, String sortExpression, Int32 startRowIndex,
                                                        Int32 maximumRows)
        {
            return GetInvoicesByCompany(companyId).SortAndPage(sortExpression, startRowIndex, maximumRows, "InvoiceId");
        }

        public Int32 GetInvoicesByCompanyCount(Int32 companyId, String sortExpression, Int32 startRowIndex,
                                               Int32 maximumRows)
        {
            return GetInvoicesByCompany(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method returns the delayed invoice by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetDelayedInvoicesByCompanyAsDataTable(Int32 companyId)
        {
            //accountSearch.dateTimeInterval = new DateTimeInterval(DateTime.MinValue.Sql2005MinValue(), DateTime.Now.Date);

            return null;
            // GetInvoices(companyId, null, new DateTimeInterval(DateTime.MinValue.Sql2005MinValue(), DateTime.Now.Date), false, "InvoiceId", 0, int.MaxValue).Cast<IQueryable>().ToDataTable();

            //ParcelsManager parcelsManager = new ParcelsManager(this);

            //var query = from invoice in GetInvoicesByCompany(companyId)
            //            join parcel in parcelsManager.GetInvoiceParcelsByCompany(companyId) on invoice.InvoiceId equals parcel.InvoiceId
            //            where parcel.DueDate < DateTime.Now.Date && !parcel.EffectedAmount.HasValue && !parcel.EffectedDate.HasValue
            //            select new
            //            {
            //                codigo = invoice.InvoiceId,
            //                docNumber = invoice.DocumentNumber,
            //                Valor = invoice.InvoiceValue,
            //                Data = parcel.DueDate

            //            };
            //return query.ToDataTable();
        }

        /// <summary>
        /// This method returns all invoices of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable GetInvoicesByCustomer(int customerId, int companyId, Boolean? closed)
        {
            var query = from inv in DbContext.Invoices
                        join parcel in (new ParcelsManager(this)).GetParcels(closed.Value) on inv.InvoiceId equals
                            parcel.InvoiceId
                        join sale in DbContext.Sales on inv.InvoiceId equals sale.InvoiceId into gSale
                        from sale in gSale.DefaultIfEmpty()
                        join receipt in DbContext.Receipts on sale.ReceiptId equals receipt.ReceiptId into gReceipts
                        from receipt in gReceipts.DefaultIfEmpty()
                        where (inv.CompanyId == companyId && inv.CustomerId == customerId)
                        orderby parcel.DueDate
                        select new { parcel.DueDate, parcel.Description, parcel.Amount, receipt.ReceiptNumber };
            return query.AsQueryable();
        }

        /// <summary>
        /// this method return all invoices Filtered by specific fields
        /// </summary>
        /// <param name="beginDate">Starting date for consulting</param>
        /// <param name="endDate">Final date for consulting</param>
        /// <param name="closed">if true the account is opened</param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        ///
#warning mover para o parcelsManager, o método usa os recursos do parcelsManager
        private IQueryable<Parcel> GetOpenInvoiceParcelInPeriodByAccount(Int32 companyId, Int32 accountId,
                                                                         DateTime beginDate, DateTime endDate)
        {
            var parcelsManager = new ParcelsManager(this);
            IQueryable<Parcel> parcels = from parcel in parcelsManager.GetParcels(false)
                                         where
                                             parcel.CompanyId == companyId && parcel.AccountId == accountId &&
                                             parcel.InvoiceId.HasValue
                                             && parcel.DueDate >= beginDate.Date && parcel.DueDate <= endDate.Date
                                         select parcel;
            return parcels;
        }


        /// <summary>
        /// this method returns the total amount of invoices
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Decimal GetInvoiceAmountByPeriodInAccount(Int32 companyId, DateTime? beginDate, DateTime? endDate,
                                                         Int32? accountId)
        {
            var parcelsManager = new ParcelsManager(this);
            IQueryable<Parcel> invoiceParcels = parcelsManager.GetInvoiceParcelsByPeriodInAcount(companyId, beginDate,
                                                                                                 endDate, accountId);

            Decimal invoiceAmountValue = Decimal.Zero;
            if (invoiceParcels.Any())
                invoiceAmountValue = invoiceParcels.Sum(parcel => parcel.Amount);

            return invoiceAmountValue;
        }

        public Decimal GetRegisteredInvoiceAmountByPeriod(Int32 companyId, DateTime? beginDate, DateTime? endDate,
                                                          Int32? accountId)
        {
            var parcelsManager = new ParcelsManager(this);
            IQueryable<Parcel> invoiceParcels = parcelsManager.GetInvoiceParcelsByPeriodInAcount(companyId, beginDate,
                                                                                                 endDate, accountId);
            invoiceParcels = invoiceParcels.Where(inv => inv.OperationDate.HasValue);

            Decimal invoiceAmountValue = Decimal.Zero;
            if (invoiceParcels.Any())
                invoiceAmountValue = invoiceParcels.Sum(parcel => parcel.Amount);

            return invoiceAmountValue;
        }
        /// <summary>
        /// This method replaces the keywords to informations of specified invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="parcelId"></param>
        /// <param name="documentTemplateId"></param>
        /// <returns>The string with keywords replaced to invoice informations</returns>
        public String ApplyInvoiceTemplate(Invoice invoice, Int32 parcelId, Int32 documentTemplateId)
        {

            var documentTemplate = new CompanyManager(this).GetDocumentTemplate(documentTemplateId);
            var content = new StringBuilder(documentTemplate.Content);

            //
            // Invoice
            //

            content.Replace("[Descrição]", invoice.Description);
            content.Replace("[DataDeCadastro]", invoice.EntryDate.ToString());
            content.Replace("[NomeDoCliente]", invoice.Customer.Name);
            content.Replace("[CentroDeCusto]", invoice.CostCenter.Name);
            content.Replace("[PlanoDeContas]", invoice.AccountingPlan.Name);

            //
            // Parcel 
            //

            var parcel = new ParcelsManager(this).GetParcel(parcelId, invoice.CompanyId);

            content.Replace("[Vencimento]", parcel.DueDate.ToString());
            content.Replace("[ValorDaParcela]", parcel.Amount.ToString());
            content.Replace("[FormaDePagamento]", parcel.PaymentMethod.Name);
            content.Replace("[DescriçaoDaParcela]", parcel.Description);


            if (parcel.EffectedDate.HasValue)
                content.Replace("[DataDoPagamento]", parcel.EffectedDate.ToString());
            else
                content.Replace("[DataDoPagamento]", String.Empty);

            if (parcel.EffectedAmount.HasValue)
                content.Replace("[ValorPago]", parcel.EffectedAmount.ToString());
            else
                content.Replace("[ValorPago]", String.Empty);

            if (!String.IsNullOrEmpty(parcel.IdentificationNumber))
                content.Replace("[Identificação]", parcel.IdentificationNumber);
            else
                content.Replace("[Identificação]", String.Empty);

            if (parcel.AccountId.HasValue)
                content.Replace("[Conta]", parcel.Account.Bank.ShortName + " - " + parcel.Account.AccountNumber);
            else
                content.Replace("[Conta]", String.Empty);

            return content.ToString();
        }


        #endregion

        #region Bill

        /// <summary>
        /// Delete the Bill and the related Parcels from the Database.
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="companyId"></param>
        public void DeleteBill(int billId, int companyId)
        {
            var parcelsManager = new ParcelsManager(this);
            parcelsManager.DeleteBillParcels(companyId, billId);
            DeleteBill(GetBill(billId, companyId));
        }

        /// <summary>
        /// this method deletes a bill
        /// </summary>
        /// <param name="bill"></param>
        private void DeleteBill(Bill bill)
        {
            DbContext.Bills.DeleteOnSubmit(bill);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Insert a Bill into the DB and sets the returnValue as the Id recently added
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertRetrievingId(Bill entity)
        {
            Insert(entity, null);
            return entity.BillId;
        }

        #region filterMethods

        /// <summary>
        /// this method return all bills
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetAllBills(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                      int startRowIndex, int maximumRows)
        {
            return GetBills(beginDate, endDate, null, companyId, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return the total rows of GetAllBills Method
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetAllBillsCount(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                      int startRowIndex, int maximumRows)
        {
            return
                GetAllBills(beginDate, endDate, companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        /// <summary>
        /// this method return the ExpiredBills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetExpiredBills(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                          int startRowIndex, int maximumRows)
        {
            return GetBills(beginDate, endDate, false, companyId, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return the total rows of GetExpiredBills Method
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetExpiredBillsCount(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                          int startRowIndex, int maximumRows)
        {
            return
                GetExpiredBills(beginDate, endDate, companyId, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        /// <summary>
        /// this method return all expering bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetExpiringBills(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetBills(DateTime.Parse("01" + DateTime.Now.ToString("/MM/yyyy")),
                            DateTime.Now.AddMonths(1).AddDays(-(DateTime.Now.Day)), false, companyId, sortExpression,
                            startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return thr total rows of GetExperingBills Method
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetExpiringBillsCount(Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetExpiringBills(companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// this method return all closed bills
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetClosedBills(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                         int startRowIndex, int maximumRows)
        {
            return GetBills(beginDate, endDate, true, companyId, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return the total rows of ClosedBills
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetClosedBillsCount(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                         int startRowIndex, int maximumRows)
        {
            return
                GetClosedBills(beginDate, endDate, companyId, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }

        /// <summary>
        /// this method return all Open bills
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetOpenBills(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                       int startRowIndex, int maximumRows)
        {
            return GetBills(beginDate, endDate, false, companyId, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this method return the total rows of GetOpenBills Method
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetOpenBillsCount(DateTime beginDate, DateTime endDate, Int32 companyId, string sortExpression,
                                       int startRowIndex, int maximumRows)
        {
            return
                GetOpenBills(beginDate, endDate, companyId, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>
                    ().Count();
        }

        #endregion

        #region NewFilterMethods

        //All

        /// <summary>
        /// this method returns all Bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetAllBills(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                       int startRowIndex, int maximumRows)
        {
            return GetBills(companyId, accountSearch, null, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetAllBills(Int32 companyId, Int32 accountPlanId, Int32 costCenterId, DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetAllBillsCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                       int startRowIndex, int maximumRows)
        {
            return
                GetAllBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        //Expired

        /// <summary>
        /// This method returns expired Bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetExpiredBills(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                           int startRowIndex, int maximumRows)
        {
            return GetBills(companyId, accountSearch, false, sortExpression, startRowIndex, maximumRows).AsQueryable();
        }

        /// <summary>
        /// this is the count method of GetExpiredBills(Int32 companyId, Int32 accountPlanId, Int32 costCenterId, DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetExpiredBillsCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                           int startRowIndex, int maximumRows)
        {
            return
                GetExpiredBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        //Expiring

        /// <summary>
        /// this method returns expiring bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetExpiringBills(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                            int startRowIndex, int maximumRows)
        {
            accountSearch.dateTimeInterval =
                new DateTimeInterval(DateTime.Parse("01" + DateTime.Now.ToString("/MM/yyyy")),
                                     DateTime.Now.AddMonths(1).AddDays(-(DateTime.Now.Day)));
            return GetBills(companyId, accountSearch, false, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of  GetExpiringBills(Int32 companyId, Int32 accountPlanId, Int32 costCenterId, DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetExpiringBillsCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                            int startRowIndex, int maximumRows)
        {
            return
                GetExpiringBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>()
                    .Count();
        }

        //Closed

        /// <summary>
        /// this method returns closed bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetClosedBills(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                          int startRowIndex, int maximumRows)
        {
            return GetBills(companyId, accountSearch, true, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        /// this is the count method of GetClosedBills(Int32 companyId, Int32 accountPlanId, Int32 costCenterId, DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetClosedBillsCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                          int startRowIndex, int maximumRows)
        {
            return
                GetClosedBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        // Open
        /// <summary>
        /// this method returns Open Bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private IQueryable GetOpenBills(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                        int startRowIndex, int maximumRows)
        {
            return GetBills(companyId, accountSearch, false, sortExpression, startRowIndex, maximumRows);
        }

        /// <summary>
        ///  this is the count method of GetOpenBills(Int32 companyId, Int32 accountPlanId, Int32 costCenterId, DateTimeInterval dateTimeInterval, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountPlanId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        private Int32 GetOpenBillsCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                        int startRowIndex, int maximumRows)
        {
            return
                GetOpenBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().
                    Count();
        }

        #endregion

        /// <summary>
        /// thsi methods returns the amount of bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
#warning verificar se este método está sendo usado
        public Decimal GetBillAmountByPeriod(Int32 companyId, DateTime? beginDate, DateTime? endDate)
        {
            var parcelsManager = new ParcelsManager(this);
            IQueryable<Parcel> billParcels = parcelsManager.GetBillParcelsByCompany(companyId);
            Decimal billAmount = Decimal.Zero;

            if (beginDate.HasValue)
                billParcels = billParcels.Where(parcel => parcel.DueDate > beginDate);
            if (endDate.HasValue)
                billParcels = billParcels.Where(parcel => parcel.DueDate < endDate);
            if (billParcels.Any())
                billAmount = billParcels.Sum(parcel => parcel.Amount);

            return billAmount;
        }


        /// <summary>
        /// this method returns the total amount of bills
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Decimal GetBillAmountByPeriodInAccount(Int32 companyId, DateTime? beginDate, DateTime? endDate,
                                                      Int32? accountId)
        {
            var parcelsManager = new ParcelsManager(this);
            Decimal billAmountValue = Decimal.Zero;
            IQueryable<Parcel> billParcels = parcelsManager.GetBillParcelsByPeriodInAccount(companyId, beginDate,
                                                                                            endDate, accountId);
            if (billParcels.Any())
                billAmountValue = billParcels.Sum(parcel => parcel.Amount);

            return billAmountValue;
        }


        public Decimal GetRegisteredBillAmountByPeriod(Int32 companyId, DateTime? beginDate, DateTime? endDate,
                                                       Int32? accountId)
        {
            var parcelsManager = new ParcelsManager(this);
            IQueryable<Parcel> billParcels = parcelsManager.GetBillParcelsByPeriodInAccount(companyId, beginDate,
                                                                                            endDate, accountId);
            billParcels = billParcels.Where(bill => bill.OperationDate.HasValue);
            if (billParcels.Any())
                return billParcels.Sum(parcel => parcel.Amount);
            return Decimal.Zero;
        }


        private IQueryable<Bill> GetBillsByAccountingPlan(Int32 companyId, Int32 accountingPlanId)
        {

            var query = from bill in GetAllBills()
                        join accountingPlan in DbContext.AccountingPlanTree(accountingPlanId, companyId)
                        on bill.AccountingPlanId equals accountingPlan.AccountingPlanId
                        select bill;

            return query;
        }

        #region GetBills-Overloaded
        private IQueryable GetBills(Int32 companyId, IAccountSearch accountSearch, Boolean? closed,
                                    string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Bill> bills;

            if (accountSearch.AccountPlanId.HasValue)
                bills = GetBillsByAccountingPlan(companyId, Convert.ToInt32(accountSearch.AccountPlanId));
            else
                bills = GetBillByCompany(companyId);
                       
            var query = from bill in bills
                        join parcel in DbContext.Parcels on bill.BillId equals parcel.BillId
                        orderby parcel.DueDate
                        select new
                        {
                            bill.AccountingPlanId,
                            bill.DocumentType,
                            bill.BillId,
                            bill.CompanyId,
                            bill.DocumentNumber,
                            bill.EntryDate,
                            bill.Description,
                            bill.Supplier,
                            bill.CostCenterId,
                            parcel.DueDate,
                            Desc = parcel.Description,
                            parcel.AccountId,
                            Amount = parcel.EffectedAmount ?? parcel.Amount,
                            parcel.EffectedAmount,
                            parcel.EffectedDate,
                            parcel.IdentificationNumber,
                            supplierName =
                 (bill.Supplier.LegalEntityProfile.CompanyName ?? String.Empty) +
                 (bill.Supplier.Profile.Name ?? String.Empty)
                        };

            if (accountSearch != null)
            {
                // Interval of date
                if (accountSearch.dateTimeInterval != null && accountSearch.dateTimeInterval.BeginDate != null)
                    query = query.Where(bill => bill.DueDate >= accountSearch.dateTimeInterval.BeginDate);

                if (accountSearch.dateTimeInterval != null && accountSearch.dateTimeInterval.EndDate != null)
                    query = query.Where(bill => bill.DueDate <= accountSearch.dateTimeInterval.EndDate);

                //Cost Center
                if (accountSearch.CostCenterId.HasValue && accountSearch.CostCenterId.Value != Decimal.Zero)
                    query = query.Where(bill => bill.CostCenterId == accountSearch.CostCenterId);

                //Customer/Supplier name
                if (!String.IsNullOrEmpty(accountSearch.Name))
                    query = query.Where(bill => bill.supplierName.Contains(accountSearch.Name));

                //Identification
                if (!String.IsNullOrEmpty(accountSearch.Identification))
                    query = query.Where(bill => bill.IdentificationNumber.Contains(accountSearch.Identification));

                //Account
                if (accountSearch.AccountId.HasValue)
                    query = query.Where(bill => bill.AccountId.Equals(accountSearch.AccountId));

                //ParcelValue
                if (accountSearch.ParcelValue.HasValue)
                    query =
                        query.Where(
                            bill =>
                            bill.Amount.Equals(accountSearch.ParcelValue) ||
                            bill.EffectedAmount.Equals(accountSearch.ParcelValue));


                if (accountSearch.ParcelStatus == ParcelStatus.EXPIRED)
                    query =
                        query.Where(
                            bill =>
                            bill.DueDate < DateTime.Now && bill.EntryDate > accountSearch.dateTimeInterval.BeginDate);

                if (accountSearch.ParcelStatus == ParcelStatus.EXPIRING)
                    query = query.Where(bill => bill.DueDate.Month == DateTime.Now.Month && bill.DueDate >= DateTime.Now);
            }


            /*  This structure was used 'cause the importance level of each conditional.
             * 
             * 
             * The first one is about All values, if closed hasn't value, Return the raw result.This statement 
             * doesn't require any execution, just return raw results therefore return them
             * 
             * 
             * The Second one has an else statement 'cause each state is important. For instance, is necessary 
             * execute others instuctions if closed is false or is true. Therefore each statement(if/else) has the same importance
             * When both states of if statements have the same importance if/else is used.
             * 
             * RefactorName: Substituir condição aninhada por cláusula guarda - kent Beck
             */

            //if closed is not null then compares IsOpen with Closed
            if (!closed.HasValue)
                query = query.AsQueryable();
            else if (closed.Value)
                query = query.Where(p => p.EffectedAmount > Decimal.Zero && p.EffectedDate.HasValue);
            else
                query = query.Where(p => !p.EffectedDate.HasValue || !p.EffectedAmount.HasValue);


            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate").AsQueryable();
        }

        public Int32 GetBillsCount(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                   int startRowIndex, int maximumRows)
        {
            return
                GetBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        public IQueryable GetBills(Int32 companyId, IAccountSearch accountSearch, string sortExpression,
                                   int startRowIndex, int maximumRows)
        {
            IQueryable query;

            switch (accountSearch.ParcelStatus)
            {
                case ParcelStatus.OPEN:
                    query = GetOpenBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                case ParcelStatus.CLOSED:
                    query = GetClosedBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                case ParcelStatus.EXPIRED:
                    query = GetExpiredBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                case ParcelStatus.EXPIRING:
                    query = GetExpiringBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
                default:
                    query = GetAllBills(companyId, accountSearch, sortExpression, startRowIndex, maximumRows);
                    break;
            }
            return query;
        }

        /// <summary>
        /// Returns a List of Bills allowing paging and sorting in the Client
        /// </summary>
        /// <param name="companyId">The Actualy Company Managed</param>
        /// <param name="sortExpression">The Column that will be Sorted</param>
        /// <param name="startRowIndex">The first row to be shown</param>
        /// <param name="maximumRows">How many rows will be showed</param>
        /// <returns></returns>
        public IQueryable GetBills(DateTime beginDate, DateTime endDate, Boolean? closed, Int32 companyId,
                                   string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Bill> bills = GetBillByCompany(companyId);

            //select all bills According to the dates
            var query = from bill in bills
                        join parcel in DbContext.Parcels on bill.BillId equals parcel.BillId
                        where parcel.DueDate >= beginDate && parcel.DueDate <= endDate
                        orderby parcel.DueDate
                        select new
                        {
                            bill.AccountingPlanId,
                            bill.BillId,
                            bill.CompanyId,
                            bill.DocumentType,
                            bill.DocumentNumber,
                            Observation = bill.Description,
                            bill.EntryDate,
                            bill.CostCenterId,
                            parcel.DueDate,
                            parcel.Description,
                            parcel.Amount,
                            parcel.EffectedDate,
                            parcel.EffectedAmount,
                            SupplierName =
                 (bill.Supplier.LegalEntityProfile.CompanyName ?? "") + (bill.Supplier.Profile.Name ?? ""),
                            parcel.AccountId
                        };
            //if closed is not null then compares IsOpen with Closed
            if (closed.HasValue)
            {
                if (closed.Value)
                    query = query.Where(p => p.EffectedAmount > 0 && p.EffectedDate.HasValue);
                else
                {
                    query = query.Where(p => !p.EffectedDate.HasValue || !p.EffectedAmount.HasValue);
                }
            }

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate").AsQueryable();
        }

        /// <summary>
        /// this is the count method of  GetBills(DateTime beginDate, DateTime endDate, Boolean? closed, Int32 companyId, string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="closed"></param>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetBillsCount(DateTime beginDate, DateTime endDate, Boolean? closed, Int32 companyId,
                                   string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetBills(beginDate, endDate, closed, companyId, sortExpression, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }


        /// <summary>
        /// Returns the next parcel of a Bill, with a smaller due date, by companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataTable GetBills(int companyId)
        {
            var query = from parcel in DbContext.Parcels
                        join bill in DbContext.Bills on parcel.BillId equals bill.BillId
                        join supplier in DbContext.Suppliers on bill.SupplierId equals supplier.SupplierId into
                            gSuppliers
                        from supplier in gSuppliers.DefaultIfEmpty()
                        join profile in DbContext.Profiles on supplier.ProfileId equals profile.ProfileId into gProfiles
                        from profile in gProfiles.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles on supplier.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfiles
                        from legalEntityProfile in gLegalEntityProfiles.DefaultIfEmpty()
                        where bill.CompanyId == 304
                        orderby parcel.DueDate
                        select new
                        {
                            bill.BillId,
                            parcel.ParcelId,
                            parcel.DueDate,
                            parcel.Description,
                            parcel.Amount,
                            Name = profile.Name ?? legalEntityProfile.CompanyName,
                            bill.DocumentNumber
                        };

            return query.ToDataTable();
        }

        #endregion



        /// <summary>
        /// This method retrieves all Bills.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Bill> GetAllBills()
        {
            return DbContext.Bills;
        }

        /// <summary>
        /// This method gets record counts of all Bills.
        /// Do not change this method.
        /// </summary>
        public int GetAllBillsCount()
        {
            return GetAllBills().Count();
        }

        /// <summary>
        /// This method retrieves a single Bill.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=BillId>BillId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Bill GetBill(Int32 BillId, Int32 CompanyId)
        {
            return DbContext.Bills.Where(x => x.BillId == BillId && x.CompanyId == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Bill by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Bill> GetBillByCompany(Int32 CompanyId)
        {
            return DbContext.Bills.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Bill by Supplier.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=SupplierId>SupplierId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Bill> GetBillBySupplier(Int32 SupplierId, Int32 CompanyId)
        {
            return DbContext.Bills.Where(x => x.SupplierId == SupplierId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Bills filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetBills(string tableName, Int32 Company_CompanyId, Int32 Supplier_SupplierId,
                              Int32 Supplier_CompanyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Bill> x = GetFilteredBills(tableName, Company_CompanyId, Supplier_SupplierId, Supplier_CompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "BillId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Bill> GetFilteredBills(string tableName, Int32 Company_CompanyId, Int32 Supplier_SupplierId,
                                                  Int32 Supplier_CompanyId)
        {
            switch (tableName)
            {
                case "Company_Bills":
                    return GetBillByCompany(Company_CompanyId);
                case "Supplier_Bills":
                    return GetBillBySupplier(Supplier_SupplierId, Supplier_CompanyId);
                default:
                    return GetAllBills();
            }
        }

        /// <summary>
        /// This method gets records counts of all Bills filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetBillsCount(string tableName, Int32 Company_CompanyId, Int32 Supplier_SupplierId,
                                 Int32 Supplier_CompanyId)
        {
            IQueryable<Bill> x = GetFilteredBills(tableName, Company_CompanyId, Supplier_SupplierId, Supplier_CompanyId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Bill entity)
        {
            DbContext.Bills.Attach(entity);
            DbContext.Bills.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        //public void Insert(Bill entity)
        //{
        //    //ParcelsManager parcelsManager = new ParcelsManager(this);
        //    //List<Parcel> billParcels = new List<Parcel>();
        //    //Parcel parcel;
        //    if (entity.CostCenterId.HasValue && new AccountManager(null).GetCostCenter(entity.CostCenterId.Value) == null)
        //        entity.CostCenterId = null;
        //    DbContext.Bills.InsertOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}
        public void Insert(Bill entity, IList<Parcel> parcels)
        {
            if (entity.CostCenterId.HasValue &&
                new AccountManager(this).GetCostCenter(entity.CostCenterId.Value) == null)
                entity.CostCenterId = null;

            entity.CreatedDate = entity.ModifiedDate = DateTime.Now;
            DbContext.Bills.InsertOnSubmit(entity);
            DbContext.SubmitChanges();

            var parcelsManager = new ParcelsManager(this);

            if (parcels != null)
            {
                foreach (Parcel item in parcels)
                {
                    item.BillId = entity.BillId;
                    item.CompanyId = entity.CompanyId;
                    DbContext.Parcels.InsertOnSubmit(item);
                }
            }
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        [Obsolete("This method isn't correct 'cause it doesn't update the parcels")]
        public void Update(Bill original_entity, Bill entity)
        {
            var parcelsManager = new ParcelsManager(this);

            foreach (Parcel parcel in entity.Parcels)
                if (!parcel.BillId.HasValue)
                    parcel.BillId = entity.BillId;

            DbContext.SubmitChanges();
            parcelsManager.UpdateParcels(
                parcelsManager.GetBillParcels(original_entity.CompanyId, original_entity.BillId).AsQueryable(),
                entity.Parcels.AsQueryable());
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        public void Update(Bill original_entity, Bill entity, IList<Parcel> parcels)
        {
            var parcelsManager = new ParcelsManager(this);

            entity.ModifiedDate = DateTime.Now;
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();

            foreach (Parcel item in parcels)
                item.BillId = original_entity.BillId;


            parcelsManager.UpdateParcels(
                parcelsManager.GetBillParcels(original_entity.CompanyId, original_entity.BillId), parcels);
        }


        #endregion


        /*
WITH AccountingPlanTree (AccountingPlanId, ParentId, DueDate, Amount) AS 
(
    SELECT ap.AccountingPlanId, ap.ParentId,  MONTH(p.DueDate) as DueDate, CAST(SUM(p.Amount) as decimal(18,2))
    FROM Parcel p INNER JOIN 
    	 Bill b ON p.BillId = b.BillId INNER JOIN		
    	 AccountingPlan ap ON ap.AccountingPlanId = b.AccountingPlanId 
    WHERE NOT EXISTS (
    	SELECT NULL FROM AccountingPlan WHERE ParentId = ap.AccountingPlanId) AND p.CompanyId=1
    GROUP BY ap.AccountingPlanId, ap.ParentId,  MONTH(p.DueDate)

UNION ALL

    SELECT 
    	 ap.AccountingPlanId, 
    	 ap.ParentId,  
    	 tree.DueDate, 
    		
    	 CAST( tree.Amount + ISNULL( 
    	 (SELECT  p.Amount 
    	  FROM Parcel p INNER JOIN 
    		   Bill b ON p.BillId = b.BillId 
    	  WHERE b.AccountingPlanId = ap.AccountingPlanId AND MONTH(p.DueDate)=tree.DueDate)
    	  ,0) as decimal(18,2))
    	  
    FROM AccountingPlan ap INNER JOIN	
    	 AccountingPlanTree tree ON ap.AccountingPlanId = tree.ParentId 

)


SELECT 
	AccountingPlanId,	
	ISNULL([1], 0) [1], 
	ISNULL([2], 0) [2], 
	ISNULL([3], 0) [3], 
	ISNULL([4], 0) [4], 
	ISNULL([5], 0) [5], 
	ISNULL([6], 0) [6], 
	ISNULL([7], 0) [7], 
	ISNULL([8], 0) [8], 
	ISNULL([9], 0) [9], 
	ISNULL([10], 0) [10], 
	ISNULL([11], 0) [11], 
	ISNULL([12], 0) [12]  
FROM (
    SELECT AccountingPlanId, ParentId, DueDate, SUM(AMOUNT) amount
    FROM AccountingPlanTree
    GROUP by AccountingPlanId, ParentId, DueDate
) AS table PIVOT (
	SUM(AMOUNT) 
	FOR [DueDate] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
) AS pivotTable


         */

    }
}
