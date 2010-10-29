using System;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class SearchManager : BusinessManager<InfoControlDataContext>
    {
        public SearchManager(IDataAccessor container)
            : base(container)
        {
        }

        public IQueryable<ISearchableItem> Search(string lookFor)
        {
            return null;
        }

        #region Search Methods

        public IQueryable<Contact> GetContacts(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            if (String.IsNullOrEmpty(text)) 
                return null;

            return new ContactManager(this).GetAllContacts()
                                           .Where(contact => contact.CompanyId == companyId && contact.Name.Contains(text))
                                           .SortAndPage(sortExpression, startRowIndex, maximumRows, "ContactId");
        }

        //public IQueryable<Customer> GetCustomersByName(Int32 companyId, String text, string sortExpression,
        //                                               int startRowIndex, int maximumRows)
        //{

        //    if (!String.IsNullOrEmpty(text))
        //    {
        //        return
        //            new CustomerManager(this).GetAllCustomers().Where(
        //                customer =>
        //                customer.Profile.Name.Contains(text) ||
        //                customer.LegalEntityProfile.CompanyName.Contains(text) && customer.CompanyId == companyId).
        //                SortAndPage(sortExpression, startRowIndex, maximumRows, "CustomerId");
        //    }

        //    return null;
        //}

        //public Int32 GetCustomersByNameCount(Int32 companyId, String text, string sortExpression, int startRowIndex,
        //                                     int maximumRows)
        //{
        //    return GetCustomersByName(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        //}


        public IQueryable<Supplier> GetSuppliersByName(Int32 companyId, String text, string sortExpression,
                                                       int startRowIndex, int maximumRows)
        {
            if (String.IsNullOrEmpty(text)) 
                return null;

            return new SupplierManager(this).GetSupplierByCompany(companyId)
                                         .Where(s => s.Profile.Name.Contains(text) || s.LegalEntityProfile.CompanyName.Contains(text))
                                         .SortAndPage(sortExpression, startRowIndex, maximumRows, "SupplierId");


        }


        public Int32 GetSuppliersByNameCount(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetSuppliersByName(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }

        public IQueryable<Bill> GetBills(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            if (String.IsNullOrEmpty(text)) 
                return null;

            return new FinancialManager(this).GetBillByCompany(companyId)
                                             .Where(b => b.Description.Contains(text))
                                             .SortAndPage(sortExpression, startRowIndex, maximumRows, "Description");
        }


        public Int32 GetBillsCount(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetBills(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }

        public IQueryable<Invoice> GetInvoices(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            return new FinancialManager(this).GetInvoicesByCompany(companyId)
                                             .Where(i => i.Description.Contains(text))
                                             .SortAndPage(sortExpression, startRowIndex, maximumRows, "Description");

        }

        public Int32 GetInvoicesCount(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetInvoices(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }


        public IQueryable<WebPage> GetHelpPages(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            return new SiteManager(this).GetAllWebPages()
                                        .Where(p => p.CompanyId == companyId)
                                        .Where(p => p.MasterPage.ToLower().Equals("help"))
                                        .Where(p => p.ParentPageId.HasValue)
                                        .Where(p => p.Name.Contains(text))
                                        .SortAndPage(sortExpression, startRowIndex, maximumRows, "Name");

        }

        public Int32 GetHelpPagesCount(Int32 companyId, String text, string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetHelpPages(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }

        #endregion

        /*
           pegar pelo nome/descrição:
         * fornecedores
         * clientes
         * produtos 
         * empregados
         * contatos
         * contas a pagar e a receber
         * 
         * 
         
         */
    }

    public class ISearchableItem
    {
        public string Name { get; set; }
    }
}