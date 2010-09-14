using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class CompanyUserManager : BusinessManager<InfoControlDataContext>
    {
        public CompanyUserManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all CompanyUsers.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<CompanyUser> GetAllCompanyUsers()
        {
            return DbContext.CompanyUsers;
        }

        /// <summary>
        /// This method gets record counts of all CompanyUsers.
        /// Do not change this method.
        /// </summary>
        public int GetAllCompanyUsersCount()
        {
            return GetAllCompanyUsers().Count();
        }

        /// <summary>
        /// This method retrieves a single CompanyUser.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=UserId>UserId</param>
        public CompanyUser GetCompanyUser(Int32 CompanyId, Int32 UserId)
        {
            return DbContext.CompanyUsers.Where(x => x.CompanyId == CompanyId && x.UserId == UserId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves CompanyUser by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<CompanyUser> GetCompanyUserByCompany(Int32 CompanyId)
        {
            return DbContext.CompanyUsers.Where(x => x.CompanyId == CompanyId);
        }

        /// <summary>
        /// This method retrieves CompanyUser by User.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=UserId>UserId</param>
        public IQueryable<CompanyUser> GetCompanyUserByUser(Int32 UserId)
        {
            return DbContext.CompanyUsers.Where(x => x.UserId == UserId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all CompanyUsers filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetCompanyUsers(string tableName, Int32 Company_CompanyId, Int32 User_UserId, string sortExpression,
                                     int startRowIndex, int maximumRows)
        {
            IQueryable<CompanyUser> x = GetFilteredCompanyUsers(tableName, Company_CompanyId, User_UserId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CompanyId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<CompanyUser> GetFilteredCompanyUsers(string tableName, Int32 Company_CompanyId,
                                                                Int32 User_UserId)
        {
            switch (tableName)
            {
                case "Company_CompanyUsers":
                    return GetCompanyUserByCompany(Company_CompanyId);
                case "User_CompanyUsers":
                    return GetCompanyUserByUser(User_UserId);
                default:
                    return GetAllCompanyUsers();
            }
        }

        /// <summary>
        /// This method gets records counts of all CompanyUsers filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetCompanyUsersCount(string tableName, Int32 Company_CompanyId, Int32 User_UserId)
        {
            IQueryable<CompanyUser> x = GetFilteredCompanyUsers(tableName, Company_CompanyId, User_UserId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(CompanyUser entity)
        {
            DbContext.CompanyUsers.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(CompanyUser entity)
        {
            DbContext.CompanyUsers.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(CompanyUser original_entity, CompanyUser entity)
        {
            DbContext.CompanyUsers.Attach(original_entity);
            DbContext.SubmitChanges();
        }
    }
}