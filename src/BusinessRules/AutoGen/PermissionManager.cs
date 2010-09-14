using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class PermissionManager : BusinessManager<InfoControlDataContext>
    {
        public PermissionManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Permissions.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Permission> GetAllPermissions()
        {
            return DbContext.Permissions;
        }

        /// <summary>
        /// This method gets record counts of all Permissions.
        /// Do not change this method.
        /// </summary>
        public int GetAllPermissionsCount()
        {
            return GetAllPermissions().Count();
        }

        /// <summary>
        /// This method retrieves a single Permission.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=FunctionId>FunctionId</param>
        /// <param name=RoleId>RoleId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Permission GetPermission(Int32 FunctionId, Int32 RoleId, Int32 CompanyId)
        {
            //
            return
                DbContext.Permissions.Where(
                    x => x.FunctionId == FunctionId && x.RoleId == RoleId && x.CompanyId == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Permission by Function.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=FunctionId>FunctionId</param>
        public IQueryable<Permission> GetPermissionByFunction(Int32 FunctionId)
        {
            //
            return DbContext.Permissions.Where(x => x.FunctionId == FunctionId);
        }

        /// <summary>
        /// This method retrieves Permission by Roles.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=RoleId>RoleId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Permission> GetPermissionByRoles(Int32 RoleId, Int32 CompanyId)
        {
            //
            return DbContext.Permissions.Where(x => x.RoleId == RoleId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Permission by PermissionTypes.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=PermissionTypeId>PermissionTypeId</param>
        public IQueryable<Permission> GetPermissionByPermissionTypes(Int32 PermissionTypeId)
        {
            //
            return DbContext.Permissions.Where(x => x.PermissionTypeId == PermissionTypeId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Permissions filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetPermissions(string tableName, Int32 Function_FunctionId, Int32 Roles_RoleId,
                                    Int32 Roles_CompanyId, Int32 PermissionTypes_PermissionTypeId, string sortExpression,
                                    int startRowIndex, int maximumRows)
        {
            IQueryable<Permission> x = GetFilteredPermissions(tableName, Function_FunctionId, Roles_RoleId,
                                                              Roles_CompanyId, PermissionTypes_PermissionTypeId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "FunctionId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Permission> GetFilteredPermissions(string tableName, Int32 Function_FunctionId,
                                                              Int32 Roles_RoleId, Int32 Roles_CompanyId,
                                                              Int32 PermissionTypes_PermissionTypeId)
        {
            switch (tableName)
            {
                case "Function_Permissions":
                    return GetPermissionByFunction(Function_FunctionId);
                case "Roles_Permissions":
                    return GetPermissionByRoles(Roles_RoleId, Roles_CompanyId);
                case "PermissionTypes_Permissions":
                    return GetPermissionByPermissionTypes(PermissionTypes_PermissionTypeId);
                default:
                    return GetAllPermissions();
            }
        }

        /// <summary>
        /// This method gets records counts of all Permissions filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetPermissionsCount(string tableName, Int32 Function_FunctionId, Int32 Roles_RoleId,
                                       Int32 Roles_CompanyId, Int32 PermissionTypes_PermissionTypeId)
        {
            IQueryable<Permission> x = GetFilteredPermissions(tableName, Function_FunctionId, Roles_RoleId,
                                                              Roles_CompanyId, PermissionTypes_PermissionTypeId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Permission entity)
        {
            //DbContext.Permissions.Attach(entity);
            DbContext.Permissions.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Permission entity)
        {
            //
            DbContext.Permissions.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Permission original_entity, Permission entity)
        {
            //
            DbContext.Permissions.Attach(original_entity);
            original_entity.PermissionTypeId = entity.PermissionTypeId;
            DbContext.SubmitChanges();
        }
    }
}