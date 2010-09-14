using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class RolesManager : BusinessManager<InfoControlDataContext>
    {
        public RolesManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Roles.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Role> GetAllRoles()
        {
            return DbContext.Roles;
        }

        /// <summary>
        /// This method gets record counts of all Roles.
        /// Do not change this method.
        /// </summary>
        public int GetAllRolesCount()
        {
            return GetAllRoles().Count();
        }

        /// <summary>
        /// This method retrieves a single Roles.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=RoleId>RoleId</param>
        /// <param name=CompanyId>CompanyId</param>
        public Role GetRoles(Int32 RoleId, Int32 CompanyId)
        {
            return DbContext.Roles.Where(x => x.RoleId == RoleId && x.CompanyId == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Roles by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Role> GetRolesByCompany(Int32 CompanyId)
        {
            return DbContext.Roles.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves Roles by Application.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ApplicationId>ApplicationId</param>
        public IQueryable<Role> GetRolesByApplication(Int32 ApplicationId)
        {
            return DbContext.Roles.Where(x => x.ApplicationId == ApplicationId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Roles filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetRoles(string tableName, Int32 Company_CompanyId, Int32 Application_ApplicationId,
                              string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Role> x = GetFilteredRoles(tableName, Company_CompanyId, Application_ApplicationId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "RoleId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Role> GetFilteredRoles(string tableName, Int32 Company_CompanyId,
                                                  Int32 Application_ApplicationId)
        {
            switch (tableName)
            {
                case "Company_Roles":
                    return GetRolesByCompany(Company_CompanyId);
                case "Application_Roles":
                    return GetRolesByApplication(Application_ApplicationId);
                default:
                    return GetAllRoles();
            }
        }

        /// <summary>
        /// This method gets records counts of all Roles filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetRolesCount(string tableName, Int32 Company_CompanyId, Int32 Application_ApplicationId)
        {
            IQueryable<Role> x = GetFilteredRoles(tableName, Company_CompanyId, Application_ApplicationId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Role entity)
        {
            DbContext.Roles.Attach(entity);
            DbContext.Roles.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns a specific role entity
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Role GetRole(Int32 roleId)
        {
            return DbContext.Roles.Where(role => role.RoleId == roleId).FirstOrDefault();
        }
        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public int Insert(Role entity)
        {
            DbContext.Roles.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
            return entity.RoleId;
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public int Update(Role original_entity, Role entity)
        {
            var originalRole = GetRole(entity.RoleId);

            originalRole.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
            return originalRole.RoleId;
        }
    }
}