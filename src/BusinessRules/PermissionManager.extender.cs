using System;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class PermissionManager
    {
        /// <summary>
        /// Method to insert a permission in the database
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <param name="entity"></param>
        public void InsertVerifying(Permission originalEntity, Permission entity)
        {
            if (originalEntity == null)
            {
                DbContext.Permissions.InsertOnSubmit(entity);
            }
            else
            {
                if (originalEntity.PermissionTypeId != entity.PermissionTypeId)
                    originalEntity.CopyPropertiesFrom(entity);
            }

            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method retrieves all the permissions for user
        /// </summary>
        /// <returns></returns>
        public DataReader GetAllPermissionsByPlan(Int32 companyId, Int32 roleId)
        {
            DataManager.Parameters.Add("@RoleId", roleId);
            DataManager.Parameters.Add("@CompanyId", companyId);
            return
                DataManager.ExecuteReader(
                    @"
                SELECT     
                    F.FunctionId, F.Name, ISNULL(Perm.PermissionTypeId, 0) AS PermissionTypeId
                FROM         
                    Functions F LEFT OUTER JOIN
                      (SELECT     FunctionId, PermissionTypeId, RoleId
                        FROM          Permissions
                        WHERE      (RoleId = @RoleId)) AS Perm ON F.FunctionId = Perm.FunctionId
                WHERE EXISTS (
                    /* Add this subquery to get only the functions of plan  */
                    SELECT    Functions.*
                    FROM      Functions INNER JOIN
                              PackageFunction ON Functions.FunctionId = PackageFunction.FunctionId INNER JOIN
                              Plans ON PackageFunction.PackageId = Plans.PackageId INNER JOIN
                              Company ON Plans.PlanId = Company.PlanId
                    WHERE     (Company.CompanyId = @CompanyId and F.FunctionId=Functions.FunctionId)
               UNION 
                    SELECT     Functions.*
                    FROM       LegalEntityProfile INNER JOIN
                               Company ON LegalEntityProfile.LegalEntityProfileId = Company.LegalEntityProfileId INNER JOIN
                               Functions INNER JOIN 
                               BranchFunction ON Functions.FunctionId = BranchFunction.FunctionId ON LegalEntityProfile.BranchId = BranchFunction.BranchId
                    WHERE     (Company.CompanyId = @CompanyId and F.FunctionId=Functions.FunctionId)
                )
                ORDER BY 
                    F.Name
            ");
        }
    }
}