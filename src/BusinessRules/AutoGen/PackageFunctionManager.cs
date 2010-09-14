using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class PackageFunctionManager : BusinessManager<InfoControlDataContext>
    {
        public PackageFunctionManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all PackageFunctions.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<PackageFunction> GetAllPackageFunctions()
        {
            return DbContext.PackageFunctions;
        }

        /// <summary>
        /// This method gets record counts of all PackageFunctions.
        /// Do not change this method.
        /// </summary>
        public int GetAllPackageFunctionsCount()
        {
            return GetAllPackageFunctions().Count();
        }

        /// <summary>
        /// This method retrieves a single PackageFunction.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=PackageId>PackageId</param>
        /// <param name=FunctionId>FunctionId</param>
        public PackageFunction GetPackageFunction(Int32 PackageId, Int32 FunctionId)
        {
            return
                DbContext.PackageFunctions.Where(x => x.PackageId == PackageId && x.FunctionId == FunctionId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves PackageFunction by Packages.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=PackageId>PackageId</param>
        public IQueryable<PackageFunction> GetPackageFunctionByPackages(Int32 PackageId)
        {
            return DbContext.PackageFunctions.Where(x => x.PackageId == PackageId);
        }

        /// <summary>
        /// This method retrieves PackageFunction by Function.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=FunctionId>FunctionId</param>
        public IQueryable<PackageFunction> GetPackageFunctionByFunction(Int32 FunctionId)
        {
            return DbContext.PackageFunctions.Where(x => x.FunctionId == FunctionId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all PackageFunctions filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetPackageFunctions(string tableName, Int32 Packages_PackageId, Int32 Function_FunctionId,
                                         string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<PackageFunction> x = GetFilteredPackageFunctions(tableName, Packages_PackageId,
                                                                        Function_FunctionId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "PackageId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<PackageFunction> GetFilteredPackageFunctions(string tableName, Int32 Packages_PackageId,
                                                                        Int32 Function_FunctionId)
        {
            switch (tableName)
            {
                case "Packages_PackageFunctions":
                    return GetPackageFunctionByPackages(Packages_PackageId);
                case "Function_PackageFunctions":
                    return GetPackageFunctionByFunction(Function_FunctionId);
                default:
                    return GetAllPackageFunctions();
            }
        }

        public IQueryable<Function> GetFunctions()
        {
            return DbContext.Functions.OrderBy(f => f.Name);
        }

        /// <summary>
        /// This method returns all functions in accordance with the package passed
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public List<Function> GetFunctionByPackages(int packageId)
        {
            IQueryable<Function> functions = from function in GetFunctions()
                                             join package in DbContext.PackageFunctions on function.FunctionId equals
                                                 package.FunctionId
                                             where package.PackageId == packageId
                                             select function;

            return functions.ToList();
        }

        /// <summary>
        /// This method returns all functions remaining by package
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public IQueryable<Function> GetRemainingFunctionsByPackage(int packageId)
        {
            IQueryable<Function> remainingFunctions = from functions in GetFunctions()
                                                      where
                                                          !functions.PackageFunctions.Any(p => p.PackageId == packageId)
                                                      select functions;

            return remainingFunctions;
        }

        /// <summary>
        /// This method gets records counts of all PackageFunctions filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetPackageFunctionsCount(string tableName, Int32 Packages_PackageId, Int32 Function_FunctionId)
        {
            IQueryable<PackageFunction> x = GetFilteredPackageFunctions(tableName, Packages_PackageId,
                                                                        Function_FunctionId);
            return x.Count();
        }

        /// <summary>
        /// Insert the function in Permissions of Administrator
        /// </summary>
        /// <param name="functionId"></param>
        public void InsertFunctionInPermissionsOfAdmin(int functionId)
        {
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(PackageFunction entity)
        {
            IQueryable<Permission> query = from permissions in DbContext.Permissions
                                           join company in DbContext.Companies on permissions.CompanyId equals
                                               company.CompanyId
                                           join plan in DbContext.Plans on company.PlanId equals plan.PlanId
                                           where
                                               plan.PackageId == entity.PackageId &&
                                               permissions.FunctionId == entity.FunctionId
                                           select permissions;
            DbContext.Permissions.DeleteAllOnSubmit(query);
            DbContext.SubmitChanges();


            DbContext.PackageFunctions.Attach(entity);
            DbContext.PackageFunctions.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(PackageFunction entity)
        {
            DbContext.PackageFunctions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();


            var rManager = new RolesManager(this);
            IQueryable<Role> queryAdminRoles = (from companies in DbContext.Companies
                                                join roles in DbContext.Roles on companies.CompanyId equals
                                                    roles.CompanyId
                                                join plan in DbContext.Plans on companies.PlanId equals plan.PlanId
                                                where plan.PackageId == entity.PackageId && roles.Name.Contains("Admin")
                                                select roles);


            List<Role> listAdminRoles = queryAdminRoles.ToList();

            var permissionManager = new PermissionManager(this);

            foreach (Role role in listAdminRoles)
            {
                if (permissionManager.GetPermission(entity.FunctionId, role.RoleId, role.CompanyId) == null)
                {
                    var permission = new Permission();
                    permission.CompanyId = role.CompanyId;
                    permission.FunctionId = entity.FunctionId;
                    permission.PermissionTypeId = (int) AccessControlActions.Change;
                    permission.RoleId = role.RoleId;
                    permissionManager.Insert(permission);
                }
            }
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(PackageFunction original_entity, PackageFunction entity)
        {
            DbContext.PackageFunctions.Attach(original_entity);
            DbContext.SubmitChanges();
        }
    }
}