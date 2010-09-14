using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class PackagesManager : BusinessManager<InfoControlDataContext>
    {
        public PackagesManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Packages.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Package> GetAllPackages()
        {
            return DbContext.Packages;
        }

        /// <summary>
        /// This method gets record counts of all Packages.
        /// Do not change this method.
        /// </summary>
        public int GetAllPackagesCount()
        {
            return GetAllPackages().Count();
        }

        /// <summary>
        /// This method retrieves a single Packages.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=PackageId>PackageId</param>
        public Package GetPackages(Int32 PackageId)
        {
            return GetAllPackages().Where(x => x.PackageId == PackageId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all Packages.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllPackages(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllPackages().SortAndPage(sortExpression, startRowIndex, maximumRows, "PackageId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Package entity)
        {
            DbContext.Packages.DeleteAllOnSubmit(DbContext.Packages.Where(x => x.PackageId == entity.PackageId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Package entity)
        {
            entity.ModifiedDate = DateTime.Now;
            DbContext.Packages.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Package original_entity, Package entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }
    }
}