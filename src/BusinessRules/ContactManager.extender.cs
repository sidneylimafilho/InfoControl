using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using System.Reflection;
using System.Data.Linq;

namespace Vivina.Erp.BusinessRules
{
    public partial class ContactManager
    {
        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Contact original_entity, Contact entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method inserts an relationship between customer and contact
        /// </summary>
        /// <param name="customerContact"></param>
        public void InsertCustomerContact(CustomerContact customerContact)
        {
            DbContext.CustomerContacts.InsertOnSubmit(customerContact);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts an relationship between supplier and contact
        /// </summary>
        /// <param name="customerContact"></param>
        public void InsertSupplierContact(SupplierContact supplierContact)
        {
            DbContext.SupplierContacts.InsertOnSubmit(supplierContact);
            DbContext.SubmitChanges();
        }

        public IQueryable<Recognizable> SearchContacts(Int32 companyId, string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            var query = CompiledQuery.Compile<InfoControlDataContext, int, string, int, IQueryable<Recognizable>>(
                (ctx, _companyId, _name, _maximumRows) =>
                (from contact in GetAllContacts()
                 where contact.CompanyId == companyId && contact.Name.Contains(name)
                 select new Recognizable(contact.ContactId, contact.Name)).Take(_maximumRows));

            return query(DbContext, companyId, name, maximumRows);
        }

        public void DeleteContact(Int32 contactId)
        {
            DbContext.Contacts.DeleteOnSubmit(GetContact(contactId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes a customerContact and after deletes the contact
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="supplierId"></param>
        public void DeleteCustomerContact(Int32 contactId)
        {
            DbContext.CustomerContacts.DeleteOnSubmit(GetCustomerContact(contactId));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method deletes a supplierContact and after deletes the contact
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="supplierId"></param>
        public void DeleteSupplierContact(Int32 contactId)
        {
            DbContext.SupplierContacts.DeleteOnSubmit(GetSupplierContact(contactId));
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method returns the contacts of a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetSupplierContacts(Int32 supplierId, string sortExpression, int startRowIndex,
                                                       int maximumRows)
        {
            var query = from contact in DbContext.Contacts
                        join supplierContact in DbContext.SupplierContacts on contact.ContactId equals
                            supplierContact.ContactId
                        where supplierContact.SupplierId == supplierId
                        select new
                        {
                            contact.ContactId,
                            contact.UserId,
                            contact.Name,
                            contact.Email,
                            contact.Phone,
                            contact.Sector,
                            UserName = contact.User.Profile.Name
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ContactId");
        }

        public Int32 GetSupplierContactsCount(Int32 supplierId, string sortExpression, int startRowIndex,
                                              int maximumRows)
        {
            return GetSupplierContacts(supplierId, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }

        /// <summary>
        /// This method returns all contacts by specified user 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IQueryable GetUserContacts(Int32 companyId, Int32 userId)
        {

            var query = from contact in GetContactsByCompany(companyId)
                        join customerContact in DbContext.CustomerContacts on contact.ContactId equals customerContact.ContactId into gCustomerContacts
                        from customerContacts in gCustomerContacts.DefaultIfEmpty()
                        join supplierContact in DbContext.SupplierContacts on contact.ContactId equals supplierContact.ContactId into gSupplierContacts
                        from supplierContacts in gSupplierContacts.DefaultIfEmpty()
                        where contact.UserId == userId
                        select new
                        {
                            NameAndPhone = contact.Name + " | " + contact.Phone,
                            contact.ContactId
                        };

            return query;

        }

        /// <summary>
        /// This method returns the contacts of a specific customer
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetCustomerContacts(Int32 customerId, string sortExpression, int startRowIndex, int maximumRows)
        {
            var query = from contact in DbContext.Contacts
                        join customerContact in DbContext.CustomerContacts on contact.ContactId equals
                            customerContact.ContactId
                        where customerContact.CustomerId == customerId
                        select new
                        {
                            contact.ContactId,
                            contact.UserId,
                            contact.Name,
                            contact.Email,
                            contact.Phone,
                            contact.Sector,
                            UserName = contact.User.Profile.Name
                        };

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "ContactId");
        }

        public Int32 GetCustomerContactsCount(Int32 customerId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetCustomerContacts(customerId, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }


        /// <summary>
        /// This method returns the contact by their name and company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="contactName"></param>
        /// <returns></returns>
        public Contact GetContact(Int32 companyId, String contactName)
        {
            return DbContext.Contacts.Where(contact => contact.CompanyId == companyId && contact.Name.Equals(contactName)).FirstOrDefault();
        }

        /// <summary>
        /// This method returns an specific supplierContact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public SupplierContact GetSupplierContact(Int32 contactId)
        {
            return
                DbContext.SupplierContacts.Where(supplierContact => supplierContact.ContactId == contactId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method returns an specific customerContact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public CustomerContact GetCustomerContact(Int32 contactId)
        {
            return
                DbContext.CustomerContacts.Where(customerContact => customerContact.ContactId == contactId).
                    FirstOrDefault();
        }

        /// <summary>
        /// This method returns a specific customerContact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerContact GetCustomerContact(Int32 contactId, Int32 customerId)
        {
            return DbContext.CustomerContacts.Where(customerContact => customerContact.ContactId == contactId && customerContact.CustomerId == customerId).
                 FirstOrDefault();
        }

        /// <summary>
        /// This method returns a specific supplierContact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public SupplierContact GetSupplierContact(Int32 contactId, Int32 supplierId)
        {
            return DbContext.SupplierContacts.Where(supplierContact => supplierContact.ContactId == contactId && supplierContact.SupplierId == supplierId).
                 FirstOrDefault();
        }


        //public List<Contact> GetContacts(int? supplierId, int? customerId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    return GetContactsAsQueryable(supplierId, customerId, sortExpression, startRowIndex, maximumRows).ToList();
        //}

        /// <summary>
        /// The auxiliar method to GetContacts
        /// It´s necessary to make a server side paging
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        //public int GetContactsCount(int? supplierId, int? customerId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    return GetContactsAsQueryable(supplierId, customerId, sortExpression, startRowIndex, maximumRows).Count();
        //}
        /// <summary>
        /// This method return all contacts by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<Contact> GetContactsByCompany(int companyId)
        {
            //IQueryable<Contact> query = from contact in DbContext.Contacts
            //                            join customerContact in DbContext.CustomerContacts on contact.ContactId equals customerContact.ContactId into gCustomerContact
            //                            from customerContact in gCustomerContact.DefaultIfEmpty()
            //                            where customerContact.CompanyId == companyId
            //                            join customer in DbContext.Customers on customerContact.CustomerId equals customer.CustomerId into gCustomers
            //                            from customer in gCustomers.DefaultIfEmpty()
            //                            select contact;

            //IQueryable<Contact> query2 = from contact in query
            //                             join supplierContact in DbContext.SupplierContacts on contact.ContactId equals supplierContact.ContactId into gSupplierContact
            //                             from supplierContact in gSupplierContact.DefaultIfEmpty()
            //                             where supplierContact.CompanyId == companyId
            //                             join supplier in DbContext.Suppliers on supplierContact.SupplierId equals supplier.SupplierId into gSuppliers
            //                             from supplier in gSuppliers.DefaultIfEmpty()
            //                             select contact;


            IQueryable<Contact> queryCustomer = from customers in DbContext.Customers
                                                join customerContacts in DbContext.CustomerContacts on
                                                    customers.CustomerId equals customerContacts.CustomerId into
                                                    gCustomerContacts
                                                from customerContacts in gCustomerContacts.DefaultIfEmpty()
                                                join contact in DbContext.Contacts on customerContacts.ContactId equals
                                                    contact.ContactId
                                                where (customers.CompanyId == companyId)
                                                select contact;

            IQueryable<Contact> querySupplier = from suppliers in DbContext.Suppliers
                                                join supplierContacts in DbContext.SupplierContacts on
                                                    suppliers.SupplierId equals supplierContacts.SupplierId into
                                                    gSupplierContacts
                                                from supplierContacts in gSupplierContacts.DefaultIfEmpty()
                                                join contact in DbContext.Contacts on supplierContacts.ContactId equals
                                                    contact.ContactId
                                                where (suppliers.CompanyId == companyId)
                                                select contact;

            return queryCustomer.Union(querySupplier);
        }

        /// <summary>
        /// This method return all contacts by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Contact> GetContactsByCompany(int companyId, string sortExpression, int startRowIndex,
                                                        int maximumRows)
        {
            return GetContactsByCompany(companyId).SortAndPage(sortExpression, startRowIndex, maximumRows, "ContactId");
        }

        /// <summary>
        /// This method retrieve contacts by general text. This text can be name, email, phone, etc
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="text"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetContacts(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetDetailedContactsByCompany(companyId, null, null, text, null, sortExpression, startRowIndex, maximumRows);
        }

        public Int32 GetContactsCount(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            return GetContacts(companyId, text, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }

        /// <summary>
        /// This method count number of rows returned by GetContactsByCompany
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetContactsByCompanyCount(int companyId, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetContactsByCompany(companyId, sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method returns detailed contacts by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetDetailedContactsByCompany(int companyId, Int32? userId, String initialLetter, String contactName,
                                                       String contactOwner, string sortExpression, int startRowIndex,
                                                       int maximumRows)
        {
            var customerQuery = from contact in GetContactsByCompany(companyId)
                                join customerContact in DbContext.CustomerContacts on contact.ContactId equals
                                    customerContact.ContactId
                                join customer in DbContext.Customers on customerContact.CustomerId equals
                                    customer.CustomerId
                                join legalEntityProfile in DbContext.LegalEntityProfiles on
                                    customer.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId into
                                    gLegalEntityProfile
                                from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                                join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into
                                    gProfile
                                from profile in gProfile.DefaultIfEmpty()
                                select new
                                           {
                                               ownerId = customer.CustomerId,
                                               isCustomerContact = true,
                                               contact.ContactId,
                                               contact.Name,
                                               contact.Phone,
                                               contact.Email,
                                               contact.Sector,
                                               UserName = contact.User.Profile.Name,
                                               contact.UserId,
                                               OwnerName = legalEntityProfile.CompanyName ?? profile.Name
                                           };



            var supplierQuery = from contact in GetContactsByCompany(companyId)
                                join supplierContact in DbContext.SupplierContacts on contact.ContactId equals
                                    supplierContact.ContactId
                                join supplier in DbContext.Suppliers on supplierContact.SupplierId equals
                                    supplier.SupplierId
                                join legalEntityProfile in DbContext.LegalEntityProfiles on
                                    supplier.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId into
                                    gLegalEntityProfile
                                from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                                join profile in DbContext.Profiles on supplier.ProfileId equals profile.ProfileId into
                                    gProfile
                                from profile in gProfile.DefaultIfEmpty()
                                select new
                                           {
                                               ownerId = supplier.SupplierId,
                                               isCustomerContact = false,
                                               contact.ContactId,
                                               contact.Name,
                                               contact.Phone,
                                               contact.Email,
                                               contact.Sector,
                                               UserName = contact.User.Profile.Name,
                                               contact.UserId,
                                               OwnerName = legalEntityProfile.CompanyName ?? profile.Name
                                           };
            var query = customerQuery.Union(supplierQuery);

            if (!String.IsNullOrEmpty(contactName))
                query = query.Where(q => q.Name.Contains(contactName));

            if (!String.IsNullOrEmpty(contactOwner))
                query = query.Where(q => q.OwnerName.Contains(contactOwner));

            if (!String.IsNullOrEmpty(initialLetter))
                query = query.Where(item => item.Name.StartsWith(initialLetter));

            if (userId.HasValue)
                query = customerQuery.Where(x => x.UserId == userId);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "contactId");
        }


        /// <summary>
        /// this is the count method of GetDetailedContactsByCompany 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetDetailedContactsByCompanyCount(int companyId, Int32? userId, String initialLetter, String contactName,
                                                       String contactOwner, string sortExpression, int startRowIndex,
                                                       int maximumRows)
        {
            return
                GetDetailedContactsByCompany(companyId, userId, initialLetter, contactName, contactOwner, sortExpression,
                                             startRowIndex, maximumRows).Cast<IQueryable>().Count();
        }

        /// <summary>
        /// Query made to bring the customers/suppliers of the registered contacts agrouping by name.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        //public IQueryable GetCustomerAndSupplierByContact(int companyId)
        //{
        //    var customerQuery = from contact in GetContactsByCompany(companyId)
        //                        join customerContact in DbContext.CustomerContacts on contact.ContactId equals customerContact.ContactId
        //                        join customer in DbContext.Customers on customerContact.CustomerId equals customer.CustomerId
        //                        group customer by customer.Profile.Name ?? customer.LegalEntityProfile.CompanyName into gCustomerNames
        //                        select new
        //                        {
        //                            OwnerName = gCustomerNames.Key
        //                        };
        //    var supplierQuery = from contact in GetContactsByCompany(companyId)
        //                        join supplierContact in DbContext.SupplierContacts on contact.ContactId equals supplierContact.ContactId
        //                        join supplier in DbContext.Suppliers on supplierContact.SupplierId equals supplier.SupplierId
        //                        group supplier by supplier.Profile.Name ?? supplier.LegalEntityProfile.CompanyName into gSuppliernames
        //                        select new
        //                        {
        //                            OwnerName = gSuppliernames.Key
        //                        };
        //    var query = customerQuery.Union(supplierQuery);
        //    return query.Sort("OwnerName");
        //}
        /// <summary>
        /// This method returns contacts by supplier
        /// </summary>
        /// <param name="companyID">Can't be null</param>
        /// <param name="supplierID">Can't be null</param>
        /// <returns>an Iqueryable of suppliers</returns>
        public IQueryable<Contact> GetContactsBySupplier(Int32 companyID, Int32 supplierID)
        {
            IQueryable<Contact> query = from contact in DbContext.Contacts
                                        join supplierContact in DbContext.SupplierContacts on contact.ContactId equals
                                            supplierContact.ContactId
                                        where supplierContact.SupplierId == supplierID
                                        select contact;

            return query;
        }
    }
}