using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class BranchManager : BusinessManager<InfoControlDataContext>
    {
        public BranchManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Branches.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Branch> GetAllBranches()
        {
            IOrderedQueryable<Branch> query = from branch in DbContext.Branches
                                              orderby branch.Name
                                              select branch;
            return query.AsQueryable();
            // return DbContext.Branches.;
        }

        /// <summary>
        /// This method gets record counts of all Branches.
        /// Do not change this method.
        /// </summary>
        public int GetAllBranchesCount()
        {
            return GetAllBranches().Count();
        }

        /// <summary>
        /// This method retrieves a single Branch.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=BranchId>BranchId</param>
        public Branch GetBranch(Int32 BranchId)
        {
            return DbContext.Branches.Where(x => x.BranchId == BranchId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all Branches.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllBranches(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllBranches().SortAndPage(sortExpression, startRowIndex, maximumRows, "BranchId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Branch entity)
        {
            DbContext.Branches.Attach(entity);
            DbContext.Branches.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Branch entity)
        {
            DbContext.Branches.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Branch original_entity, Branch entity)
        {
            DbContext.Branches.Attach(original_entity);
            original_entity.Name = entity.Name;
            DbContext.SubmitChanges();
        }
    }
}