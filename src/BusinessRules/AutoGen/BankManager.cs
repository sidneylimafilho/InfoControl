using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class BankManager : BusinessManager<InfoControlDataContext>
    {
        public BankManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Banks.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Bank> GetAllBanks()
        {
            return DbContext.Banks.Where(b => b.ShortName != null);
        }

        public IList GetAllBanksWithNumbers()
        {
            var query = from b in GetAllBanks() select new {b.BankId, Name = b.BankNumber + " - " + b.ShortName};
            return query.ToList();
        }

        /// <summary>
        /// This method gets record counts of all Banks.
        /// Do not change this method.
        /// </summary>
        public int GetAllBanksCount()
        {
            return GetAllBanks().Count();
        }

        /// <summary>
        /// This method retrieves a single Bank.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=BankId>BankId</param>
        public Bank GetBank(Int32 BankId)
        {
            return DbContext.Banks.Where(x => x.BankId == BankId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all Banks.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllBanks(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllBanks().SortAndPage(sortExpression, startRowIndex, maximumRows, "BankId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Bank entity)
        {
            DbContext.Banks.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Bank entity)
        {
            DbContext.Banks.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Bank original_entity, Bank entity)
        {
            DbContext.Banks.Attach(original_entity);
            original_entity.Name = entity.Name;
            original_entity.BankNumber = entity.BankNumber;
            DbContext.SubmitChanges();
        }
    }
}