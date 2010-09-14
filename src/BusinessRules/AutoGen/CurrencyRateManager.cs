using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class CurrencyRateManager : BusinessManager<InfoControlDataContext>
    {
        public CurrencyRateManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all CurrencyRates.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<CurrencyRate> GetAllCurrencyRates()
        {
            return DbContext.CurrencyRates;
        }

        /// <summary>
        /// This method gets record counts of all CurrencyRates.
        /// Do not change this method.
        /// </summary>
        public int GetAllCurrencyRatesCount()
        {
            return GetAllCurrencyRates().Count();
        }

        /// <summary>
        /// This method retrieves a single CurrencyRate.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CurrencyRateId>CurrencyRateId</param>
        public CurrencyRate GetCurrencyRate(Int32 CurrencyRateId)
        {
            return DbContext.CurrencyRates.Where(x => x.CurrencyRateId == CurrencyRateId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all CurrencyRates.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllCurrencyRates(string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetAllCurrencyRates().SortAndPage(sortExpression, startRowIndex, maximumRows, "CurrencyRateId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(CurrencyRate entity)
        {
            DbContext.CurrencyRates.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(CurrencyRate entity)
        {
            DbContext.CurrencyRates.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(CurrencyRate original_entity, CurrencyRate entity)
        {
            DbContext.CurrencyRates.Attach(original_entity);
            original_entity.Name = entity.Name;
            original_entity.Symbol = entity.Symbol;
            original_entity.EndOfDayRate = entity.EndOfDayRate;
            original_entity.ModifiedDate = entity.ModifiedDate;
            DbContext.SubmitChanges();
        }
    }
}