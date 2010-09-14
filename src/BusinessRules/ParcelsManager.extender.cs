using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
 
namespace Vivina.Erp.BusinessRules
{
    public class ParcelsManager : BusinessManager<InfoControlDataContext>
    {
        public ParcelsManager(IDataAccessor container)
            : base(container)
        {
        }


        /// <summary>
        /// This method insert a new parcel 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns> 
        public int Insert(Parcel entity, FinancierCondition financierCondition)
        {
            if (financierCondition == null)
                financierCondition = new FinancierCondition();

            //Set the Closed/Open Parcel
            if (entity.AccountId.HasValue)
            {
                entity.EffectedAmount = entity.Amount;
                entity.EffectedDate = DateTime.Now;
            }

            if (Convert.ToBoolean(entity.IsRecurrent))
            {
                entity.EffectedAmount = null;
                entity.EffectedDate = null;


                if (entity.RecurrentPeriod == 7)
                    entity.DueDate = Convert.ToDateTime(entity.DueDate).AddDays(7);
                else if (entity.RecurrentPeriod == 15)
                    entity.DueDate = Convert.ToDateTime(entity.DueDate).AddDays(15);
                else if (entity.RecurrentPeriod == 30)
                    entity.DueDate = Convert.ToDateTime(entity.DueDate).AddMonths(1);
                else
                    entity.DueDate = Convert.ToDateTime(entity.DueDate).AddYears(1);
            }

            //
            // After that, the code remains the same.
            //

            DbContext.Parcels.InsertOnSubmit(entity);
            DbContext.SubmitChanges();

            if (entity.BillId.HasValue)
                return (int)entity.BillId;
            else
                return (int)entity.InvoiceId;
        }

        private void ProcessParcel(Parcel parcel)
        {
            //
            // Se a conta não foi dada baixa mas foi conciliada então quita
            // 
            if (!parcel.EffectedDate.HasValue)
                parcel.EffectedDate = parcel.OperationDate;
        }

        /// <summary>
        /// Método para edição de uma parcela
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <param name="entity"></param>
        public void Update(Parcel original_entity, Parcel entity)
        {
            original_entity = GetParcel(original_entity.ParcelId, original_entity.CompanyId);
            original_entity.CopyPropertiesFrom(entity);
            ProcessParcel(original_entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Método para exclusão de uma parcela
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Parcel entity)
        {
            DbContext.Parcels.DeleteOnSubmit(
                DbContext.Parcels.Where(p => p.ParcelId == entity.ParcelId).FirstOrDefault());
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete bill parcel
        /// </summary>
        /// <param name="billId"></param>
        public void DeleteParcels(int billId)
        {
            DbContext.Parcels.DeleteAllOnSubmit(DbContext.Parcels.Where(p => p.BillId == billId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method delete invoice parcels 
        /// </summary>
        /// <param name="invoiceId"></param>
#warning este método deve ser subistituido pelo método DeleteInvoiceParcels(companyId,InvoiceId)
        public void DeleteInvoiceParcels(int invoiceId)
        {
            DbContext.Parcels.DeleteAllOnSubmit(DbContext.Parcels.Where(p => p.InvoiceId == invoiceId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method deletes invoice's parcels 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="invoiceId"></param>
        public void DeleteInvoiceParcels(Int32 companyId, Int32 invoiceId)
        {
            DbContext.Parcels.DeleteAllOnSubmit(GetInvoiceParcels(companyId, invoiceId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method deletes bill's parcels
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="billId"></param>
        public void DeleteBillParcels(Int32 companyId, Int32 billId)
        {
            DbContext.Parcels.DeleteAllOnSubmit(GetBillParcels(companyId, billId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Get parcels is Open or closed
        /// </summary>
        /// <param name="isClosed"></param>
        /// <returns></returns>
        public IQueryable<Parcel> GetParcels(bool isClosed)
        {
            IQueryable<Parcel> parcels = DbContext.Parcels;
            if (isClosed)
                parcels = parcels.Where(p => p.EffectedAmount > 0 && p.EffectedDate.HasValue);

            return parcels;
        }

        /// <summary>
        /// Retorna as parcelas de Contas à Pagar, com ordenação e Paginação
        /// </summary>
        /// <param name="matrixId"></param>
        /// <param name="billId"></param>
        /// <param name="invoiceId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Parcel> GetBillParcels(int companyId, int billId, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            IQueryable<Parcel> parcel = DbContext.Parcels.Where(x => x.CompanyId == companyId && x.BillId == billId);
            return parcel.SortAndPage(sortExpression, startRowIndex, maximumRows, "EffectedDate").ToList();
        }

        /// <summary>
        /// Retorna uma DataTable ordenada por data de vencimento das parcelas
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public IList GetBillParcelsTable(int billId, int companyId)
        {
            return GetBillParcels(companyId, billId).ToList();
        }

        /// <summary>
        /// Return all lines of the table parcels that have the same billId and CompanyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="billId"></param>
        /// <returns></returns>
        public List<Parcel> GetBillParcels(int companyId, int billId)
        {
            IQueryable<Parcel> query = from parcel in DbContext.Parcels
                                       where parcel.BillId == billId && parcel.CompanyId == companyId
                                       select parcel;
            return query.OrderBy(x => x.DueDate).ToList();
        }

        /// <summary>
        /// this  method return the parcels by bill and paymentMethod
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="billId"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public List<Parcel> GetBillParcelsByPaymentMethodAsList(Int32 companyId, Int32 billId, Int32 paymentMethodId)
        {
            return
                GetBillParcels(companyId, billId).Where(p => p.PaymentMethodId == paymentMethodId).OrderBy(
                    p => p.DueDate).ToList();
        }

        public IQueryable<Parcel> GetInvoiceParcels(Int32 companyId, Int32 invoiceId)
        {
            return DbContext.Parcels.Where(x => x.CompanyId == companyId && x.InvoiceId == invoiceId);
        }

        /// <summary>
        /// Retorna as parcelas de Contas à Receber, com ordenação e Paginação
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="invoiceId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Parcel> GetInvoiceParcels(int companyId, int invoiceId, string sortExpression, int startRowIndex,
                                              int maximumRows)
        {
            IQueryable<Parcel> parcel =
                DbContext.Parcels.Where(x => x.CompanyId == companyId && x.InvoiceId == invoiceId);
            return parcel.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate").ToList();
        }

        /// <summary>
        /// Retorna uma DataTable, ordenada pela ordem de vencimento das parcelas
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public List<Parcel>
            Count(int invoiceId, int companyId)
        {
            return
                DbContext.Parcels.Where(x => x.InvoiceId == invoiceId && x.CompanyId == companyId).Sort("DueDate").
                    ToList();
        }

        public List<Parcel> GetInvoiceParcelsByPaymentMethodAsList(int invoiceId, int companyId, Int32 paymentMethodId)
        {
            return
                GetInvoiceParcels(invoiceId, companyId).Where(p => p.PaymentMethodId == paymentMethodId).OrderBy(
                    p => p.DueDate).ToList();
        }

        /// <summary>
        /// this method return the SaleParcels
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public IQueryable<Parcel> GetParcelsBySale(Int32 companyId, Int32 saleId, string sortExpression,
                                                   int startRowIndex, int maximumRows)
        {
            var saleManager = new SaleManager(this);
            return
                GetInvoiceParcels(companyId, saleManager.GetSale(companyId, saleId).InvoiceId.Value, sortExpression,
                                  startRowIndex, maximumRows).AsQueryable();
        }

        /// <summary>
        /// this is the CountMethod of GetParcelsBySale
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="saleId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetParcelsBySaleCount(Int32 companyId, Int32 saleId, string sortExpression, int startRowIndex,
                                           int maximumRows)
        {
            return GetParcelsBySale(companyId, saleId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// Returns one line of the DB Parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId, int companyId)
        {
            return DbContext.Parcels.Where(x => x.CompanyId == companyId && x.ParcelId == parcelId).FirstOrDefault();
        }

        /// <summary>
        /// return all closed parcels and open parcels by account in period
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        [Obsolete("This method was changed by Other method that has dateTimeInterval as parameter")]
        public IQueryable<Parcel> GetParcelsByAccountInPeriod(int companyId, int accountId, DateTime beginDate,
                                                              DateTime endDate, string sortExpression, int startRowIndex,
                                                              int maximumRows)
        {
            IQueryable<Parcel> parcelQuery = from parcel in DbContext.Parcels
                                             where (parcel.CompanyId == companyId) &&
                                                   (
                                                       (parcel.EffectedDate >= beginDate &&
                                                        parcel.EffectedDate <= endDate && parcel.AccountId == accountId) ||
                                                       (parcel.DueDate >= beginDate && parcel.DueDate <= endDate &&
                                                        parcel.AccountId == null)
                                                   )
                                             select parcel;

            return parcelQuery.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate");
        }

        public IQueryable<Parcel> GetParcelsByAccountInPeriod(int companyId, int accountId,
                                                              DateTimeInterval dateTimeInterval, string sortExpression,
                                                              int startRowIndex, int maximumRows)
        {
            IQueryable<Parcel> parcelQuery = from parcel in DbContext.Parcels
                                             where (parcel.CompanyId == companyId) &&
                                                   (
                                                       (parcel.EffectedDate >= dateTimeInterval.BeginDate &&
                                                        parcel.EffectedDate <= dateTimeInterval.EndDate &&
                                                        parcel.AccountId == accountId) ||
                                                       (parcel.DueDate >= dateTimeInterval.BeginDate &&
                                                        parcel.DueDate <= dateTimeInterval.EndDate &&
                                                        parcel.AccountId == null)
                                                   )
                                             select parcel;

            return parcelQuery.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate");
        }


        /// <summary>
        /// this method return the number of parcels
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetParcelsByAccountInPeriodCount(int companyId, int accountId, DateTime beginDate, DateTime endDate,
                                                      string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetParcelsByAccountInPeriod(companyId, accountId, beginDate, endDate, sortExpression, startRowIndex,
                                            maximumRows).Count();
        }


        /// <summary>
        /// this method retuns parcels by account 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountId"></param>
        /// <param name="dueDateBeginInterval"></param>
        /// <param name="dueDateEndInterval"></param>
        /// <param name="operationDateBeginInterval"></param>
        /// <param name="operationDateEndInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        [Obsolete("this method isn't  still used, is was changed to GetParcelsByAccountInPeriod method ")]
        public IQueryable<Parcel> GetParcelsByAccount(int companyId, int accountId, DateTime effectedDateBeginInterval,
                                                      DateTime effectedDateEndInterval,
                                                      DateTime operationDateBeginInterval,
                                                      DateTime operationDateEndInterval, string sortExpression,
                                                      int startRowIndex, int maximumRows)
        {
            IQueryable<Parcel> parcelQuery = from parcel in DbContext.Parcels
                                             where
                                                 (parcel.CompanyId == companyId &&
                                                  (parcel.AccountId == accountId || !parcel.AccountId.HasValue)) &&
                                                 (parcel.EffectedDate >= effectedDateBeginInterval &&
                                                  parcel.EffectedDate <= effectedDateEndInterval) &&
                                                 (parcel.OperationDate >= operationDateBeginInterval &&
                                                  parcel.OperationDate <= operationDateEndInterval)
                                             select parcel;
            return parcelQuery.SortAndPage(sortExpression, startRowIndex, maximumRows, "DueDate");
        }


        /// <summary>
        /// this is the count method of GetParcelsByAccount
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountId"></param>
        /// <param name="effectedDateBeginInterval"></param>
        /// <param name="effectedDateEndInterval"></param>
        /// <param name="operationDateBeginInterval"></param>
        /// <param name="operationDateEndInterval"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        [Obsolete("this method isn't  still used, is was changed to GetParcelsByAccountInPeriod method ")]
        public Int32 GetParcelsByAccountCount(int companyId, int accountId, DateTime effectedDateBeginInterval,
                                              DateTime effectedDateEndInterval, DateTime operationDateBeginInterval,
                                              DateTime operationDateEndInterval, string sortExpression,
                                              int startRowIndex, int maximumRows)
        {
            return
                GetParcelsByAccount(companyId, accountId, effectedDateBeginInterval, effectedDateEndInterval,
                                    operationDateEndInterval, operationDateEndInterval, sortExpression, startRowIndex,
                                    maximumRows).Count();
        }

        /// <summary>
        /// this method returns parcels by account in Period. This method compares the beginDate/endDate
        /// to DueDate and OperationDate
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<Parcel> GetParcelsByAccountInPeriod(int companyId, int accountId,
                                                              DateTimeInterval dateTimeInterval)
        {
            IQueryable<Parcel> query = from parcel in GetParcelsByAccount(companyId, accountId)
                                       where
                                           (parcel.DueDate > dateTimeInterval.BeginDate &&
                                            parcel.DueDate < dateTimeInterval.EndDate)
                                           ||
                                           (parcel.OperationDate > dateTimeInterval.BeginDate &&
                                            parcel.OperationDate < dateTimeInterval.EndDate)
                                       select parcel;
            return query;
        }

        /// <summary>
        /// this method returns parcels by account
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private IQueryable<Parcel> GetParcelsByAccount(Int32 companyId, int accountId)
        {
            return DbContext.Parcels.Where(parcel => parcel.CompanyId == companyId && parcel.AccountId == accountId);
        }


        /// <summary>
        /// this method returns invoice parcels by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Parcel> GetInvoiceParcelsByCompany(Int32 companyId)
        {
            IQueryable<Parcel> parcels = from parcel in DbContext.Parcels
                                         join invoice in DbContext.Invoices on parcel.InvoiceId equals invoice.InvoiceId
                                         where invoice.CompanyId == companyId
                                         select parcel;
            return parcels;
        }



        /// <summary>
        /// This method retrieves the bill's parcels sum that not is conciliated in a specified company 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public decimal GetBillParcelsNonEffected(Int32 companyId)
        {
            var query = DbContext.Parcels.Where(parcel => parcel.BillId.HasValue && parcel.CompanyId == companyId);

            decimal parcelsSum = Decimal.Zero;

            if (query.Any(parcel => !parcel.OperationDate.HasValue))
                parcelsSum = query.Sum(parcel => parcel.Amount);

            return parcelsSum;
        }

        /// <summary>
        /// This method retrieves the invoice's parcels sum that not is conciliated in a specified company  
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public decimal GetInvoiceParcelsNonEffected(Int32 companyId)
        {
            var query = DbContext.Parcels.Where(parcel => parcel.InvoiceId.HasValue && parcel.CompanyId == companyId);

            decimal parcelsSum = Decimal.Zero;

            if (query.Any(parcel => !parcel.OperationDate.HasValue))
                parcelsSum = query.Sum(parcel => parcel.Amount);

            return parcelsSum;
        }






        /// <summary>
        /// this method returns parcels by company,period and account
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IQueryable<Parcel> GetInvoiceParcelsByPeriodInAcount(Int32 companyId, DateTime? beginDate,
                                                                    DateTime? endDate, Int32? accountId)
        {
            return GetParcelsByPeriodInAccount(GetInvoiceParcelsByCompany(companyId), beginDate, endDate, accountId);
        }

        public IQueryable<Parcel> GetBillParcelsByPeriodInAccount(Int32 companyId, DateTime? beginDate,
                                                                  DateTime? endDate, Int32? accountId)
        {
            return GetParcelsByPeriodInAccount(GetBillParcelsByCompany(companyId), beginDate, endDate, accountId);
        }

        /// <summary>
        /// this method searches parcels by period and account 
        /// </summary>
        /// <param name="parcels"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private IQueryable<Parcel> GetParcelsByPeriodInAccount(IQueryable<Parcel> parcels, DateTime? beginDate,
                                                               DateTime? endDate, Int32? accountId)
        {
            if (beginDate.HasValue)
                parcels = parcels.Where(parcel => parcel.EffectedDate >= beginDate.Value.Date);
            if (endDate.HasValue)
                parcels = parcels.Where(parcel => parcel.EffectedDate < endDate.Value.Date.AddDays(1).AddMinutes(-1));
            if (accountId.HasValue)
                parcels = parcels.Where(parcel => parcel.AccountId == accountId);

            return parcels.OrderBy(parcel => parcel.EffectedDate);
        }

        /// <summary>
        /// this method returns bill parcels by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Parcel> GetBillParcelsByCompany(Int32 companyId)
        {
            IQueryable<Parcel> parcels = from parcel in DbContext.Parcels
                                         join bill in DbContext.Bills on parcel.BillId equals bill.BillId
                                         where bill.CompanyId == companyId
                                         select parcel;
            return parcels;
        }

        /// <summary>
        /// this method updates all parcels, this is normally used with parcel's control.
        /// The properties'value of parcels must be correct
        /// </summary>
        /// <param name="original_parcels"></param>
        /// <param name="parcels"></param>
        public void UpdateParcels(IEnumerable<Parcel> original_parcels, IEnumerable<Parcel> parcels)
        {
            ///delete all parcels besides remaining parcels
            foreach (Parcel original_parcel in original_parcels)
                if (!parcels.Any(remainingParcel => remainingParcel.ParcelId == original_parcel.ParcelId))
                    Delete(original_parcel);

            DbContext.SubmitChanges();

            Parcel newParcel;
            foreach (Parcel parcel in parcels)
            {
                newParcel = GetParcel(parcel.ParcelId, parcel.CompanyId);
                if (newParcel == null)
                    newParcel = new Parcel();

                newParcel.CopyPropertiesFrom(parcel);

                if (parcel.ParcelId == Decimal.Zero) ///insert a new Parcel
                    Insert(newParcel, null);

                DbContext.SubmitChanges();
            }
        }

        /// <summary>
        /// this method deletes invoice's parcels after a parameter date
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="date"></param>
        public void DeleteAllNextInvoiceParcels(Invoice invoice, DateTime date)
        {
            DbContext.Parcels.DeleteAllOnSubmit(
                GetInvoiceParcels(invoice.CompanyId, invoice.InvoiceId).Where(p => p.DueDate > date));
            DbContext.SubmitChanges();
        }
    }
}