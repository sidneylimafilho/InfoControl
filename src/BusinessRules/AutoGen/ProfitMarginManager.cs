using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Vivina.Framework.Configuration;
using Vivina.Framework.Data; using Vivina.InfoControl.DataClasses;

namespace Vivina.InfoControl.BusinessRules
{


    public partial class ProfitMarginManager : Vivina.Framework.Data.BusinessManager
    {
        public ProfitMarginManager(IDataAccessor container): base(container){}
        /// <summary>
        /// This method retrieves all ProfitMargins.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<ProfitMargin> GetAllProfitMargins()
        {
            Vivina.InfoControl.DataClasses.InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            return db.ProfitMargins;
        }

        /// <summary>
        /// This method gets record counts of all ProfitMargins.
        /// Do not change this method.
        /// </summary>
        public int GetAllProfitMarginsCount()
        {
            return GetAllProfitMargins().Count();
        }

        /// <summary>
        /// This method retrieves a single ProfitMargin.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=ProfitMarginId>ProfitMarginId</param>
        /// <param name=CompanyId>CompanyId</param>
        public ProfitMargin GetProfitMargin(Int32 ProfitMarginId, Int32 CompanyId)
        {
            Vivina.InfoControl.DataClasses.InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            return db.ProfitMargins.Where(x=>x.ProfitMarginId == ProfitMarginId && x.CompanyId == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves ProfitMargin by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<ProfitMargin> GetProfitMarginByCompany(Int32 CompanyId)
        {
            Vivina.InfoControl.DataClasses.InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            return db.ProfitMargins.Where(x=>x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all ProfitMargins filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public DataTable GetProfitMargins(string tableName, Int32 Company_CompanyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<ProfitMargin> x = GetFilteredProfitMargins(tableName, Company_CompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "ProfitMarginId").ToDataTable();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<ProfitMargin> GetFilteredProfitMargins(string tableName, Int32 Company_CompanyId)
        {
            switch (tableName)
            {
                case "Company_ProfitMargins":
                    return GetProfitMarginByCompany(Company_CompanyId);
                default:
                    return GetAllProfitMargins();
            }
        }

        /// <summary>
        /// This method gets records counts of all ProfitMargins filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetProfitMarginsCount(string tableName, Int32 Company_CompanyId)
        {
            IQueryable<ProfitMargin> x = GetFilteredProfitMargins(tableName, Company_CompanyId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(ProfitMargin entity)
        {
            Vivina.InfoControl.DataClasses.InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            db.ProfitMargins.Attach(entity);
            db.ProfitMargins.Remove(entity);
            db.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(ProfitMargin entity) 
        {
            Vivina.InfoControl.DataClasses.InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            db.ProfitMargins.Add(entity);
            db.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(ProfitMargin original_entity, ProfitMargin entity) 
        {
            Vivina.InfoControl.DataClasses.InfoControlDataContext db = DataManager.CreateContext<InfoControlDataContext>();
            db.ProfitMargins.Attach(original_entity);
            original_entity.Name = entity.Name;
            original_entity.Percentage = entity.Percentage;
            original_entity.ModifiedDate = entity.ModifiedDate;
            db.SubmitChanges();
            
        }

    }
}
