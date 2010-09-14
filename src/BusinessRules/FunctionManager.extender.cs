using System;
using System.Collections.Generic;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public enum InsertPackageFunctionsStatus
    {
        DuplicateEntry,
        Success
    }

    public class FunctionManager : BusinessManager<InfoControlDataContext>
    {
        public FunctionManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method returns all functions as a Table
        /// </summary>
        /// <returns></returns>
        public IQueryable<Function> GetAllFunctions()
        {
            return DbContext.Functions.OrderBy(func => func.Name);
        }


        /// <summary>
        /// this method return an function
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public Function GetFunction(int functionId)
        {
            return DbContext.Functions.Where(function => function.FunctionId == functionId).FirstOrDefault();
        }

        /// <summary>
        /// this method returns functions
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Function> GetFunctions(string sortExpression, int startRowIndex, int maximumRows)
        {
            return DbContext.Functions.SortAndPage(
                String.IsNullOrEmpty(sortExpression) ? "Name" : sortExpression,
                startRowIndex, maximumRows, "FunctionId");
        }

        /// <summary>
        /// this is the count method of GetFunctions(string sortExpression, int startRowIndex, int maximumRows)
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetFunctionsCount(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetFunctions(sortExpression, startRowIndex, maximumRows).Count();
        }


        /// <summary>
        /// This method is used to order the gridview onte screen Functions.aspx
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Function> GetAllFunctionsList(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetFunctions(sortExpression, startRowIndex, maximumRows).ToList();
        }

        ///// <summary>
        ///// This method gets record counts of all Functions.
        ///// Do not change this method.
        ///// </summary>
        ///// <returns></returns>
        //public int GetAllFunctionsCount()
        //{
        //    return GetAllFunctions().Count();
        //}
        public void InsertFunction(Function entity)
        {
            DbContext.Functions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns all functions in accordance with the branch passed
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public List<Function> GetFunctionByBranchs(int branchId)
        {
            IQueryable<Function> functions = from function in GetAllFunctions()
                                             join branch in DbContext.BranchFunctions on function.FunctionId equals
                                                 branch.FunctionId
                                             where branch.BranchId == branchId
                                             select function;

            return functions.ToList();
        }

        /// <summary>
        /// This method returns all functions remaining by branch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public IQueryable<Function> GetRemainingFunctionsByBranch(int branchId)
        {
            IQueryable<Function> remainingFunctions = from functions in GetAllFunctions()
                                                      where !functions.BranchFunctions.Any(p => p.BranchId == branchId)
                                                      select functions;

            return remainingFunctions;
        }

        /// <summary>
        /// This method returns all functions of a package
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public IQueryable<Function> GetFunctionsByPackage(Int32 packageId)
        {
            IQueryable<Function> query = from func in GetAllFunctions()
                                         join pack in DbContext.PackageFunctions on func.FunctionId equals
                                             pack.FunctionId
                                         where pack.PackageId == packageId
                                         select func;
            return query;
        }

        /// <summary>
        /// this method returns all functions of a plan
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public List<Function> GetFunctionsByPlan(Int32 planId)
        {
            IQueryable<Function> query = from func in GetAllFunctions()
                                         join pack in DbContext.PackageFunctions on func.FunctionId equals
                                             pack.FunctionId
                                         join plan in DbContext.Plans on pack.PackageId equals plan.PackageId
                                         where plan.PlanId == planId
                                         select func;
            return query.ToList();
        }

        /// <summary>
        /// This method delete a function from a package
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="functionId"></param>
        public void DeletePackageFunctions(int packageId, int functionId)
        {
            PackageFunction entity =
                DbContext.PackageFunctions.Where(x => x.FunctionId == functionId && x.PackageId == packageId).
                    FirstOrDefault();
            DbContext.PackageFunctions.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a Function in a package
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public InsertPackageFunctionsStatus InsertPackageFunctions(int packageId, int functionId)
        {
            var entity = new PackageFunction();
            entity.PackageId = packageId;
            entity.FunctionId = functionId;
            try
            {
                DbContext.PackageFunctions.InsertOnSubmit(entity);
                DbContext.SubmitChanges();
            }
            catch
            {
                return InsertPackageFunctionsStatus.DuplicateEntry;
            }
            return InsertPackageFunctionsStatus.Success;
        }

        /// <summary>
        /// This method delete a function on the database
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteFunction(Int32 functionId)
        {
            DbContext.Functions.DeleteOnSubmit(GetFunction(functionId));
            DbContext.SubmitChanges();
        }

        public void Update(Function original_entity, Function entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }
    }
}