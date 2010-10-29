using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class SuppliersContactManager : BusinessManager<InfoControlDataContext>
    {
        public SuppliersContactManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all SuppliersContacts.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<SupplierContact> GetAllSuppliersContacts()
        {
            return DbContext.SupplierContacts;
        }

        /// <summary>
        /// This method gets record counts of all SuppliersContacts.
        /// Do not change this method.
        /// </summary>
        public int GetAllSuppliersContactsCount()
        {
            return GetAllSuppliersContacts().Count();
        }

        /// <summary>
        /// This method retrieves a single SupplierContact.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=SupplierId>SupplierId</param>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=ContactId>ContactId</param>
        public SupplierContact GetSuppliersContact(Int32 SupplierId, Int32 CompanyId, Int32 ContactId)
        {
            return
                DbContext.SupplierContacts.Where(x => x.SupplierId == SupplierId && x.ContactId == ContactId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves SupplierContact by Supplier.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=SupplierId>SupplierId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<SupplierContact> GetSuppliersContactBySupplier(Int32 SupplierId, Int32 CompanyId)
        {
            return DbContext.SupplierContacts.Where(x => x.SupplierId == SupplierId);
        }

        /// <summary>
        /// This method retrieves SupplierContact by Contact.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ContactId>ContactId</param>
        public IQueryable<SupplierContact> GetSuppliersContactByContact(Int32 ContactId)
        {
            return DbContext.SupplierContacts.Where(x => x.ContactId == ContactId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all SuppliersContacts filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetSuppliersContacts(string tableName, Int32 Supplier_SupplierId, Int32 Supplier_CompanyId,
                                          Int32 Contact_ContactId, string sortExpression, int startRowIndex,
                                          int maximumRows)
        {
            IQueryable<SupplierContact> x = GetFilteredSuppliersContacts(tableName, Supplier_SupplierId,
                                                                         Supplier_CompanyId, Contact_ContactId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "SupplierId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<SupplierContact> GetFilteredSuppliersContacts(string tableName, Int32 Supplier_SupplierId,
                                                                         Int32 Supplier_CompanyId,
                                                                         Int32 Contact_ContactId)
        {
            switch (tableName)
            {
                case "Supplier_SuppliersContacts":
                    return GetSuppliersContactBySupplier(Supplier_SupplierId, Supplier_CompanyId);
                case "Contact_SuppliersContacts":
                    return GetSuppliersContactByContact(Contact_ContactId);
                default:
                    return GetAllSuppliersContacts();
            }
        }

        /// <summary>
        /// This method gets records counts of all SuppliersContacts filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetSuppliersContactsCount(string tableName, Int32 Supplier_SupplierId, Int32 Supplier_CompanyId,
                                             Int32 Contact_ContactId)
        {
            IQueryable<SupplierContact> x = GetFilteredSuppliersContacts(tableName, Supplier_SupplierId,
                                                                         Supplier_CompanyId, Contact_ContactId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(SupplierContact entity)
        {
            DbContext.SupplierContacts.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(SupplierContact entity)
        {
            DbContext.SupplierContacts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(SupplierContact original_entity, SupplierContact entity)
        {
            DbContext.SupplierContacts.Attach(original_entity);
            DbContext.SubmitChanges();
        }
    }
}