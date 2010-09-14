using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class AddonManager : BusinessManager<InfoControlDataContext>
    {
        public AddonManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Addons.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        //public IQueryable<Addon> GetAllAddons()
        //{

        //    return DbContext.Addons;
        //}

        ///// <summary>
        ///// This method gets record counts of all Addons.
        ///// Do not change this method.
        ///// </summary>
        //public int GetAllAddonsCount()
        //{
        //    return GetAllAddons().Count();
        //}

        ///// <summary>
        ///// This method retrieves a single Addon.
        ///// Change this method to alter how that record is received.
        ///// </summary>
        ///// <param name=CompanyId>CompanyId</param>
        ///// <param name=PackageId>PackageId</param>
        //public Addon GetAddon(Int32 CompanyId, Int32 PackageId)
        //{

        //    return DbContext.Addons.Where(x=>x.CompanyId == CompanyId && x.PackageId == PackageId).FirstOrDefault();
        //}

        ///// <summary>
        ///// This method retrieves Addon by Packages.
        ///// Change this method to alter how records are retrieved.
        ///// </summary>
        ///// <param name=PackageId>PackageId</param>
        //public IQueryable<Addon> GetAddonByPackages(Int32 PackageId)
        //{

        //    return DbContext.Addons.Where(x=>x.PackageId == PackageId);
        //}

        ///// <summary>
        ///// This method retrieves Addon by Company.
        ///// Change this method to alter how records are retrieved.
        ///// </summary>
        ///// <param name=CompanyId>CompanyId</param>
        //public IQueryable<Addon> GetAddonByCompany(Int32 CompanyId)
        //{

        //    return DbContext.Addons.Where(x=>x.CompanyId == CompanyId);
        //}

        ///// <summary>
        ///// This method gets sorted and paged records of all Addons filtered by a specified field.
        ///// Do not change this method.
        ///// </summary>
        //public System.Collections.IList GetAddons(string tableName, Int32 Packages_PackageId, Int32 Company_CompanyId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    IQueryable<Addon> x = GetFilteredAddons(tableName, Packages_PackageId, Company_CompanyId);
        //    return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CompanyId").ToList();
        //}

        ///// <summary>
        ///// This method routes a request for filtering by a field value to another method.
        ///// Do not change this method.
        ///// </summary>
        //private IQueryable<Addon> GetFilteredAddons(string tableName, Int32 Packages_PackageId, Int32 Company_CompanyId)
        //{
        //    switch (tableName)
        //    {
        //        case "Packages_Addons":
        //            return GetAddonByPackages(Packages_PackageId);
        //        case "Company_Addons":
        //            return GetAddonByCompany(Company_CompanyId);
        //        default:
        //            return GetAllAddons();
        //    }
        //}

        ///// <summary>
        ///// This method gets records counts of all Addons filtered by a specified field.
        ///// Do not change this method.
        ///// </summary>
        //public int GetAddonsCount(string tableName, Int32 Packages_PackageId, Int32 Company_CompanyId)
        //{
        //    IQueryable<Addon> x = GetFilteredAddons(tableName, Packages_PackageId, Company_CompanyId);
        //    return x.Count();
        //}

        ///// <summary>
        ///// This method deletes a record in the table.
        ///// Change this method to alter how records are deleted.
        ///// </summary>
        ///// <param name=entity>entity</param>
        //public void Delete(Addon entity)
        //{

        //    DbContext.Addons.DeleteOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}

        ///// <summary>
        ///// This method inserts a new record in the table.
        ///// Change this method to alter how records are inserted.
        ///// </summary>
        ///// <param name=entity>entity</param>
        //public void Insert(Addon entity) 
        //{

        //    DbContext.Addons.InsertOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}

        ///// <summary>
        ///// This method updates a record in the table.
        ///// Change this method to alter how records are updated.
        ///// </summary>
        ///// <param name=original_entity>original_entity</param>
        ///// <param name=entity>entity</param>
        //public void Update(Addon original_entity, Addon entity) 
        //{

        //    DbContext.Addons.Attach(original_entity);
        //    DbContext.SubmitChanges();

        //}
    }
}