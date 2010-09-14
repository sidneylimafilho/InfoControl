//using System;
//using System.Collections;
//using System.Linq;
//using InfoControl.Data;
//using Vivina.Erp.DataClasses;

//namespace Vivina.Erp.BusinessRules
//{
//    public class BillingManager : BusinessManager<InfoControlDataContext>
//    {
//        public BillingManager(IDataAccessor container) : base(container)
//        {
//        }

//        /// <summary>
//        /// This method retrieves all Billings.
//        /// Change this method to alter how records are retrieved.
//        /// </summary>
//        public IQueryable<Billing> GetAllBillings()
//        {
//            return DbContext.Billings;
//        }

//        /// <summary>
//        /// This method gets record counts of all Billings.
//        /// Do not change this method.
//        /// </summary>
//        public int GetAllBillingsCount()
//        {
//            return GetAllBillings().Count();
//        }

//        /// <summary>
//        /// This method retrieves a single Billing.
//        /// Change this method to alter how that record is received.
//        /// </summary>
//        /// <param name=BillingId>BillingId</param>
//        public Billing GetBilling(Int32 BillingId)
//        {
//            return DbContext.Billings.Where(x => x.BillingId == BillingId).FirstOrDefault();
//        }

//        /// <summary>
//        /// This method retrieves Billing by Company.
//        /// Change this method to alter how records are retrieved.
//        /// </summary>
//        /// <param name=CompanyId>CompanyId</param>
//        public IQueryable<Billing> GetBillingByCompany(Int32 CompanyId)
//        {
//            return DbContext.Billings.Where(x => x.CompanyId == CompanyId);
//        }

//        /// <summary>
//        /// This method gets sorted and paged records of all Billings filtered by a specified field.
//        /// Do not change this method.
//        /// </summary>
//        public IList GetBillings(string tableName, Int32 Company_CompanyId, string sortExpression, int startRowIndex,
//                                 int maximumRows)
//        {
//            IQueryable<Billing> x = GetFilteredBillings(tableName, Company_CompanyId);
//            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "BillingId").ToList();
//        }

//        /// <summary>
//        /// This method routes a request for filtering by a field value to another method.
//        /// Do not change this method.
//        /// </summary>
//        private IQueryable<Billing> GetFilteredBillings(string tableName, Int32 Company_CompanyId)
//        {
//            switch (tableName)
//            {
//                case "Company_Billings":
//                    return GetBillingByCompany(Company_CompanyId);
//                default:
//                    return GetAllBillings();
//            }
//        }

//        /// <summary>
//        /// This method gets records counts of all Billings filtered by a specified field.
//        /// Do not change this method.
//        /// </summary>
//        public int GetBillingsCount(string tableName, Int32 Company_CompanyId)
//        {
//            IQueryable<Billing> x = GetFilteredBillings(tableName, Company_CompanyId);
//            return x.Count();
//        }

//        /// <summary>
//        /// This method deletes a record in the table.
//        /// Change this method to alter how records are deleted.
//        /// </summary>
//        /// <param name=entity>entity</param>
//        public void Delete(Billing entity)
//        {
//            DbContext.Billings.DeleteOnSubmit(entity);
//            DbContext.SubmitChanges();
//        }

//        /// <summary>
//        /// This method inserts a new record in the table.
//        /// Change this method to alter how records are inserted.
//        /// </summary>
//        /// <param name=entity>entity</param>
//        public void Insert(Billing entity)
//        {
//            DbContext.Billings.InsertOnSubmit(entity);
//            DbContext.SubmitChanges();
//        }

//        /// <summary>
//        /// This method updates a record in the table.
//        /// Change this method to alter how records are updated.
//        /// </summary>
//        /// <param name=original_entity>original_entity</param>
//        /// <param name=entity>entity</param>
//        public void Update(Billing original_entity, Billing entity)
//        {
//            DbContext.Billings.Attach(original_entity);
//            original_entity.CompanyId = entity.CompanyId;
//            original_entity.Name = entity.Name;
//            original_entity.BoletusNumber = entity.BoletusNumber;
//            original_entity.PaymentDate = entity.PaymentDate;
//            original_entity.EmissionDate = entity.EmissionDate;
//            original_entity.PaymentValue = entity.PaymentValue;
//            DbContext.SubmitChanges();
//        }
//    }
//}