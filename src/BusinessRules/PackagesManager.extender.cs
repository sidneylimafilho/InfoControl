using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class PackagesManager
    {
        /// <summary>
        /// Return all Packages as List
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Package> GetAllPackagesList(string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Package> x = GetAllPackages();
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "PackageId").ToList();
        }

        /// <summary>
        /// Inserting retrieving the inserted item Id
        /// </summary>
        /// <param name="entity">A instance of Package Class</param>
        public int InsertRetrievingID(Package entity)
        {
            DbContext.Packages.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
            return entity.PackageId;
        }

        /// <summary>
        /// Get Upgraded packages that a Company currently have
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList GetAdditionalPackages(int companyId)
        {
            var packages = from pak in DbContext.Packages
                           join pakAdd in DbContext.PackageAdditionals on pak.PackageId equals pakAdd.PackageId
                           where pakAdd.CompanyId == companyId && pakAdd.EndDate == null
                           select
                               new {pak, pakAdd.AddonId, isCancelable = (pakAdd.StartDate.AddMonths(1) <= DateTime.Now)};
            return packages.ToList();
        }

        /// <summary>
        /// Get all packages that aren´t in a plan
        /// </summary>
        /// <returns></returns>
        public DataTable GetUpdatePackages()
        {
            string sql =
                @"SELECT    Name + ' (R$ ' + CAST(Price AS Varchar) + ')' AS Name, PackageId
                FROM         Package
                WHERE     (NOT EXISTS
                          (SELECT     Pack2.PackageId
                           FROM          Package AS Pack2 INNER JOIN
                           Plans ON Package.PackageId = Plans.PackageId)) AND (IsActive = 'True')";

            return DataManager.ExecuteDataTable(sql);
        }

        /// <summary>
        /// Insert a PackageAdditional in the database
        /// </summary>
        /// <param name="entity"></param>
        public void UpGrade(PackageAdditional entity)
        {
            DbContext.PackageAdditionals.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// Remove a PackageAdditional in the database
        /// </summary>
        /// <param name="packageAdditionalId"></param>
        public void DownGrade(int packageAdditionalId)
        {
            PackageAdditional original = GetPackageAdditional(packageAdditionalId);
            PackageAdditional package = GetPackageAdditional(packageAdditionalId);
            package.EndDate = DateTime.Now;
            UpdatePackageAdditional(original, package);
        }

        /// <summary>
        /// Return a single PackageAdditional
        /// </summary>
        /// <param name="packageAdditionalId"></param>
        /// <returns></returns>
        public PackageAdditional GetPackageAdditional(int packageAdditionalId)
        {
            return DbContext.PackageAdditionals.Where(x => x.AddonId == packageAdditionalId).FirstOrDefault();
        }

        /// <summary>
        /// Basic Update Method
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdatePackageAdditional(PackageAdditional original_entity, PackageAdditional entity)
        {
            DbContext.PackageAdditionals.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method returns all packages actives
        /// </summary>
        /// <returns></returns>
        public IQueryable<Package> GetPackagesActives()
        {
            return DbContext.Packages.Where(x => x.IsActive == true);
        }
    }
}