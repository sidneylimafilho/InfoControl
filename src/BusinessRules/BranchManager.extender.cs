using System.Collections.Generic;
using System.Linq;
using InfoControl;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class BranchManager
    {
        /// <summary>
        /// Return all branches, allowing sorting and paging in the client
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Branch> GetAllBranchesList(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllBranches().SortAndPage(sortExpression, startRowIndex, maximumRows, "BranchId").ToList();
        }

        /// <summary>
        /// Delete All Branch Functions
        /// </summary>
        public void DeleteAllBranchFunctions()
        {
            DbContext.BranchFunctions.DeleteAllOnSubmit(DbContext.BranchFunctions);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Inserts the Branch Function
        /// </summary>
        /// <param name="entity"></param>
        public void InsertBranchFunction(BranchFunction entity)
        {
            DbContext.BranchFunctions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Delete the Branch Function
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteBranchFunction(BranchFunction entity)
        {
            IQueryable<Permission> query = from permissions in DbContext.Permissions
                                           join company in DbContext.Companies on permissions.CompanyId equals
                                               company.CompanyId
                                           where company.LegalEntityProfile.BranchId == entity.BranchId
                                           select permissions;
            DbContext.Permissions.DeleteAllOnSubmit(query);
            DbContext.SubmitChanges();

            DbContext.BranchFunctions.Attach(entity);
            DbContext.BranchFunctions.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }
    }
}