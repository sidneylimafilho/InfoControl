using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class ContactManager : BusinessManager<InfoControlDataContext>
    {
        public ContactManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Contacts.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Contact> GetAllContacts()
        {
            return DbContext.Contacts;
        }

        /// <summary>
        /// This method gets record counts of all Contacts.
        /// Do not change this method.
        /// </summary>
        public int GetAllContactsCount()
        {
            return GetAllContacts().Count();
        }

        /// <summary>
        /// This method retrieves a single Contact.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=ContactId>ContactId</param>
        public Contact GetContact(Int32 contactId)
        {
            return DbContext.Contacts.Where(x => x.ContactId == contactId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all Contacts.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllContacts(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllContacts().SortAndPage(sortExpression, startRowIndex, maximumRows, "ContactId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Contact entity)
        {
            DbContext.Contacts.Attach(entity);
            DbContext.Contacts.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Contact entity)
        {
            DbContext.Contacts.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }
    }
}