using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class UsersInRolesManager : BusinessManager<InfoControlDataContext>
    {
        public UsersInRolesManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all UsersInRoles.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<UsersInRole> GetAllUsersInRoles()
        {
            return DbContext.UsersInRoles;
        }

        /// <summary>
        /// This method gets record counts of all UsersInRoles.
        /// Do not change this method.
        /// </summary>
        public int GetAllUsersInRolesCount()
        {
            return GetAllUsersInRoles().Count();
        }

        /// <summary>
        /// This method retrieves a single UsersInRoles.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=RoleId>RoleId</param>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=UserId>UserId</param>
        public UsersInRole GetUsersInRoles(Int32 RoleId, Int32 CompanyId, Int32 UserId)
        {
            return
                DbContext.UsersInRoles.Where(x => x.RoleId == RoleId && x.CompanyId == CompanyId && x.UserId == UserId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves UsersInRoles by User.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=UserId>UserId</param>
        public IQueryable<UsersInRole> GetUsersInRolesByUser(Int32 UserId)
        {
            return DbContext.UsersInRoles.Where(x => x.UserId == UserId);
        }

        /// <summary>
        /// This method retrieves UsersInRoles by CompanyUser.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=UserId>UserId</param>
        public IQueryable<UsersInRole> GetUsersInRolesByCompanyUser(Int32 CompanyId, Int32 UserId)
        {
            return DbContext.UsersInRoles.Where(x => x.CompanyId == CompanyId && x.UserId == UserId);
        }

        /// <summary>
        /// This method retrieves UsersInRoles by Roles.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=RoleId>RoleId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<UsersInRole> GetUsersInRolesByRoles(Int32 RoleId, Int32 CompanyId)
        {
            return DbContext.UsersInRoles.Where(x => x.RoleId == RoleId && x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all UsersInRoles filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetUsersInRoles(string tableName, Int32 User_UserId, Int32 CompanyUser_CompanyId,
                                     Int32 CompanyUser_UserId, Int32 Roles_RoleId, Int32 Roles_CompanyId,
                                     string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<UsersInRole> x = GetFilteredUsersInRoles(tableName, User_UserId, CompanyUser_CompanyId,
                                                                CompanyUser_UserId, Roles_RoleId, Roles_CompanyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "RoleId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<UsersInRole> GetFilteredUsersInRoles(string tableName, Int32 User_UserId,
                                                                Int32 CompanyUser_CompanyId, Int32 CompanyUser_UserId,
                                                                Int32 Roles_RoleId, Int32 Roles_CompanyId)
        {
            switch (tableName)
            {
                case "User_UsersInRoles":
                    return GetUsersInRolesByUser(User_UserId);
                case "CompanyUser_UsersInRoles":
                    return GetUsersInRolesByCompanyUser(CompanyUser_CompanyId, CompanyUser_UserId);
                case "Roles_UsersInRoles":
                    return GetUsersInRolesByRoles(Roles_RoleId, Roles_CompanyId);
                default:
                    return GetAllUsersInRoles();
            }
        }

        /// <summary>
        /// This method gets records counts of all UsersInRoles filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetUsersInRolesCount(string tableName, Int32 User_UserId, Int32 CompanyUser_CompanyId,
                                        Int32 CompanyUser_UserId, Int32 Roles_RoleId, Int32 Roles_CompanyId)
        {
            IQueryable<UsersInRole> x = GetFilteredUsersInRoles(tableName, User_UserId, CompanyUser_CompanyId,
                                                                CompanyUser_UserId, Roles_RoleId, Roles_CompanyId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(UsersInRole entity)
        {
            DbContext.UsersInRoles.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(UsersInRole entity)
        {
            DbContext.UsersInRoles.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(UsersInRole original_entity, UsersInRole entity)
        {
            DbContext.UsersInRoles.Attach(original_entity);
            DbContext.SubmitChanges();
        }
    }
}