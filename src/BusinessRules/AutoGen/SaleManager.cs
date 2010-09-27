using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class SaleManager : BusinessManager<InfoControlDataContext>
    {
        public SaleManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Sales.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Sale> GetAllSales()
        {
            return DbContext.Sales;
        }

        /// <summary>
        /// This method gets record counts of all Sales.
        /// Do not change this method.
        /// </summary>
        public int GetAllSalesCount()
        {
            return GetAllSales().Count();
        }

        /// <summary>
        /// This method retrieves a single Sale.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=BudgetId>BudgetId</param>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=UserId>UserId</param>
        public Sale GetSale(Int32 CompanyId, Int32 SaleId, Int32 CustomerId)
        {
            return
                DbContext.Sales.Where(x => x.CompanyId == CompanyId && x.SaleId == SaleId && x.CustomerId == CustomerId)
                    .FirstOrDefault();
        }


        /// <summary>
        /// This method retrieves Sale by CompanyUser.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=UserId>UserId</param>
        public IQueryable<Sale> GetSaleByCompanyUser(Int32 CompanyId)
        {
            return DbContext.Sales.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Sales filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetSales(string tableName, Int32 Budget_BudgetId, Int32 Budget_CompanyId, Int32 Budget_CustomerId,
                              Int32 CompanyUser_CompanyId, Int32 CompanyUser_UserId, string sortExpression,
                              int startRowIndex, int maximumRows)
        {
            IQueryable<Sale> x = GetFilteredSales(tableName, Budget_BudgetId, Budget_CompanyId, Budget_CustomerId,
                                                  CompanyUser_CompanyId, CompanyUser_UserId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CompanyId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Sale> GetFilteredSales(string tableName, Int32 Budget_BudgetId, Int32 Budget_CompanyId,
                                                  Int32 Budget_CustomerId, Int32 CompanyUser_CompanyId,
                                                  Int32 CompanyUser_UserId)
        {
            switch (tableName)
            {
                case "CompanyUser_Sales":
                    return GetSaleByCompanyUser(CompanyUser_CompanyId);
                default:
                    return GetAllSales();
            }
        }

        /// <summary>
        /// This method gets records counts of all Sales filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetSalesCount(string tableName, Int32 Budget_BudgetId, Int32 Budget_CompanyId,
                                 Int32 Budget_CustomerId, Int32 CompanyUser_CompanyId, Int32 CompanyUser_UserId)
        {
            IQueryable<Sale> x = GetFilteredSales(tableName, Budget_BudgetId, Budget_CompanyId, Budget_CustomerId,
                                                  CompanyUser_CompanyId, CompanyUser_UserId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Sale entity)
        {
            DbContext.Sales.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param> 
        public void Insert(Sale entity)
        {
            if (entity.SaleStatusId == 0)
                entity.SaleStatusId = 1; //aguardando expedição 

            entity.CreatedDate = DateTime.Now;

            DbContext.Sales.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Sale original_entity, Sale entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }
    }
}