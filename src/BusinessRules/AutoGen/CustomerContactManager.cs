using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class CustomerContactManager : BusinessManager<InfoControlDataContext>
    {
        public CustomerContactManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all CustomerContacts.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<CustomerContact> GetAllCustomerContacts()
        {
            return DbContext.CustomerContacts;
        }

        /// <summary>
        /// This method gets record counts of all CustomerContacts.
        /// Do not change this method.
        /// </summary>
        public int GetAllCustomerContactsCount()
        {
            return GetAllCustomerContacts().Count();
        }

        /// <summary>
        /// This method retrieves a single CustomerContact.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=CompanyId>CompanyId</param>
        /// <param name=ContactId>ContactId</param>
        public CustomerContact GetCustomerContact(Int32 CustomerId, Int32 CompanyId, Int32 ContactId)
        {
            return
                DbContext.CustomerContacts.Where(x => x.CustomerId == CustomerId && x.ContactId == ContactId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves CustomerContact by Customer.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<CustomerContact> GetCustomerContactByCustomer(Int32 CustomerId, Int32 CompanyId)
        {
            return DbContext.CustomerContacts.Where(x => x.CustomerId == CustomerId);
        }

        /// <summary>
        /// This method retrieves CustomerContact by Contact.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ContactId>ContactId</param>
        public IQueryable<CustomerContact> GetCustomerContactByContact(Int32 ContactId)
        {
            return DbContext.CustomerContacts.Where(x => x.ContactId == ContactId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all CustomerContacts filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetCustomerContacts(string tableName, Int32 Customer_CustomerId, Int32 Customer_CompanyId,
                                         Int32 Contact_ContactId, string sortExpression, int startRowIndex,
                                         int maximumRows)
        {
            IQueryable<CustomerContact> x = GetFilteredCustomerContacts(tableName, Customer_CustomerId,
                                                                        Customer_CompanyId, Contact_ContactId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CustomerId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<CustomerContact> GetFilteredCustomerContacts(string tableName, Int32 Customer_CustomerId,
                                                                        Int32 Customer_CompanyId,
                                                                        Int32 Contact_ContactId)
        {
            switch (tableName)
            {
                case "Customer_CustomerContacts":
                    return GetCustomerContactByCustomer(Customer_CustomerId, Customer_CompanyId);
                case "Contact_CustomerContacts":
                    return GetCustomerContactByContact(Contact_ContactId);
                default:
                    return GetAllCustomerContacts();
            }
        }

        /// <summary>
        /// This method gets records counts of all CustomerContacts filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetCustomerContactsCount(string tableName, Int32 Customer_CustomerId, Int32 Customer_CompanyId,
                                            Int32 Contact_ContactId)
        {
            IQueryable<CustomerContact> x = GetFilteredCustomerContacts(tableName, Customer_CustomerId,
                                                                        Customer_CompanyId, Contact_ContactId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(CustomerContact entity)
        {
            DbContext.CustomerContacts.Attach(entity);
            DbContext.CustomerContacts.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(CustomerContact entity)
        {
            DbContext.CustomerContacts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(CustomerContact original_entity, CustomerContact entity)
        {
            DbContext.CustomerContacts.Attach(original_entity);
            DbContext.SubmitChanges();
        }
    }
}