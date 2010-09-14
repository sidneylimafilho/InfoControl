//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Linq;
//using System.Linq;
//using Vivina.Framework.Configuration;
//using Vivina.Framework.Data;
//using Vivina.InfoControl.DataClasses;

//namespace Vivina.InfoControl.BusinessRules
//{


//    public partial class FinancierManager : Vivina.Framework.Data.BusinessManager<InfoControlDataContext>
//    {
//        public FinancierManager(IDataAccessor container) : base(container) { }
//        /// <summary>
//        /// This method retrieves all Financiers.
//        /// Change this method to alter how records are retrieved.
//        /// </summary>
//        public IQueryable<Financier> GetAllFinanciers()
//        {
            
//            return DbContext.Financiers;
//        }

//        /// <summary>
//        /// This method gets record counts of all Financiers.
//        /// Do not change this method.
//        /// </summary>
//        public int GetAllFinanciersCount()
//        {
//            return GetAllFinanciers().Count();
//        }

//        /// <summary>
//        /// This method retrieves a single Financier.
//        /// Change this method to alter how that record is received.
//        /// </summary>
//        /// <param name=FinancierId>FinancierId</param>
//        /// <param name=CompanyId>CompanyId</param>
//        public Financier GetFinancier(Int32 FinancierId, Int32 CompanyId)
//        {
            
//            return DbContext.Financiers.Where(x => x.FinancierId == FinancierId && x.CompanyId == CompanyId).FirstOrDefault();
//        }

//        /// <summary>
//        /// This method retrieves Financier by Company.
//        /// Change this method to alter how records are retrieved.
//        /// </summary>
//        /// <param name=CompanyId>CompanyId</param>
//        public IQueryable<Financier> GetFinancierByCompany(Int32 CompanyId)
//        {
            
//            return DbContext.Financiers.Where(x => x.CompanyId == CompanyId);
//        }

//        /// <summary>
//        /// This method gets sorted and paged records of all Financiers filtered by a specified field.
//        /// Do not change this method.
//        /// </summary>
//        public System.Collections.IList GetFinanciers(string tableName, Int32 Company_CompanyId, string sortExpression, int startRowIndex, int maximumRows)
//        {
//            IQueryable<Financier> x = GetFilteredFinanciers(tableName, Company_CompanyId);
//            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "FinancierId").ToList();
//        }

//        /// <summary>
//        /// This method routes a request for filtering by a field value to another method.
//        /// Do not change this method.
//        /// </summary>
//        private IQueryable<Financier> GetFilteredFinanciers(string tableName, Int32 Company_CompanyId)
//        {
//            switch (tableName)
//            {
//                case "Company_Financiers":
//                    return GetFinancierByCompany(Company_CompanyId);
//                default:
//                    return GetAllFinanciers();
//            }
//        }

//        /// <summary>
//        /// This method gets records counts of all Financiers filtered by a specified field.
//        /// Do not change this method.
//        /// </summary>
//        public int GetFinanciersCount(string tableName, Int32 Company_CompanyId)
//        {
//            IQueryable<Financier> x = GetFilteredFinanciers(tableName, Company_CompanyId);
//            return x.Count();
//        }

//        /// <summary>
//        /// This method deletes a record in the table.
//        /// Change this method to alter how records are deleted.
//        /// </summary>
//        /// <param name=entity>entity</param>
//        public void Delete(Financier entity)
//        {
            
//            DbContext.Financiers.Attach(entity);
//            DbContext.Financiers.DeleteOnSubmit(entity);
//            DbContext.SubmitChanges();
//        }

//        /// <summary>
//        /// This method inserts a new record in the table.
//        /// Change this method to alter how records are inserted.
//        /// </summary>
//        /// <param name=entity>entity</param>
//        public void Insert(Financier entity)
//        {
            
//            DbContext.Financiers.InsertOnSubmit(entity);
//            DbContext.SubmitChanges();
//        }

//        /// <summary>
//        /// This method updates a record in the table.
//        /// Change this method to alter how records are updated.
//        /// </summary>
//        /// <param name=original_entity>original_entity</param>
//        /// <param name=entity>entity</param>
//        public void Update(Financier original_entity, Financier entity)
//        {
            
//            DbContext.Financiers.Attach(original_entity);
//            original_entity.Name = entity.Name;
//            original_entity.Period = entity.Period;
//            original_entity.Commission = entity.Commission;
//            DbContext.SubmitChanges();

//        }

//    }
//}
