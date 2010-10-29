using System.Collections.Generic;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class RolesManager
    {
        /// <summary>
        /// Return all Roles of a company, as List
        /// Ready to do a Sort and Page on the Client
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Role> GetRolesByCompanyAsList(int companyId, string sortExpression, int startRowIndex,
                                                  int maximumRows)
        {
            IQueryable<Role> x = GetRolesByCompany(companyId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "RoleId").ToList();
        }

        /// <summary>
        /// Returns all roles that the user don´t have
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataReader GetRemainingRolesByUser(int userId, int companyId)
        {
            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.Parameters.Add("@userId", userId);
            return
                DataManager.ExecuteReader(
                    @"SELECT RoleId, Name
                                        FROM   Roles
                                        WHERE  (CompanyId = @companyId) AND (NOT EXISTS
                                       (SELECT Roles_1.RoleId
                                        FROM   Roles AS Roles_1 INNER JOIN
                                               UsersInRoles AS UI ON Roles_1.RoleId = UI.RoleId
                                        WHERE  (Roles_1.RoleId = Roles.RoleId) AND (UI.UserId = @userId)))");
        }

        /// <summary>
        /// Return all roles that a user have in a determined company
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataReader GetUserInRoles(int userId, int companyId)
        {
            DataManager.Parameters.Add("@companyId", companyId);
            DataManager.Parameters.Add("@userId", userId);
            return
                DataManager.ExecuteReader(
                    @"SELECT role.Name, uir.RoleId, uir.CompanyId, uir.UserId
                                                 FROM UsersInRoles AS uir INNER JOIN
                                                      Roles AS role ON role.RoleId = uir.RoleId
                                                WHERE (uir.CompanyId = @companyId) AND (uir.UserId = @userId)");
        }

        /// <summary>
        /// Basic Delete method
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteUserInRoles(UsersInRole entity)
        {
            DbContext.UsersInRoles.Attach(entity);
            DbContext.UsersInRoles.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Get all roles that have the same name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<Role> GetRolesByName(string name)
        {
            return DbContext.Roles.Where(x => x.Name == name);
        }
    }
}