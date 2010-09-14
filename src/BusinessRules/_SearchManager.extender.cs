using System;
using System.Linq;
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
            if (!String.IsNullOrEmpty(text))
                return new ContactManager(this).GetAllContacts().Where(contact => contact.CompanyId == companyId && contact.Name.Contains(text)).SortAndPage(sortExpression, startRowIndex, maximumRows, "ContactId");

            return null;
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
            if (!String.IsNullOrEmpty(text))
            {
                return
                    new SupplierManager(this).GetSupplierByCompany(companyId).Where(
                        supplier =>
                        supplier.Profile.Name.Contains(text) || supplier.LegalEntityProfile.CompanyName.Contains(text)).
                        SortAndPage(sortExpression, startRowIndex, maximumRows, "SupplierId");
            }
            return null;
        }


        public Int32 GetSuppliersByNameCount(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                             int maximumRows)
        {
            return GetSuppliersByName(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }

       

       

       

        public IQueryable<Bill> GetBills(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                         int maximumRows)
        {
            if (!String.IsNullOrEmpty(text))
                return
                    new FinancialManager(this).GetBillByCompany(companyId).Where(bill => bill.Description.Contains(text)).
                        SortAndPage(sortExpression, startRowIndex, maximumRows, "Description");
            return null;
        }


        public Int32 GetBillsCount(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                   int maximumRows)
        {
            return GetBills(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }

        public IQueryable<Invoice> GetInvoices(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                               int maximumRows)
        {
            if (!String.IsNullOrEmpty(text))
                return
                    new FinancialManager(this).GetInvoicesByCompany(companyId).Where(
                        invoice => invoice.Description.Contains(text)).SortAndPage(sortExpression, startRowIndex,
                                                                                   maximumRows, "Description");
            return null;
        }

        public Int32 GetInvoicesCount(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                      int maximumRows)
        {
            return GetInvoices(companyId, text, sortExpression, startRowIndex, maximumRows).Count();
        }


        public IQueryable<WebPage> GetHelpPages(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                                int maximumRows)
        {
            if (!String.IsNullOrEmpty(text))
            {
                return
                    new SiteManager(this).GetAllWebPages().Where(
                        webPage =>
                        webPage.CompanyId == companyId && webPage.MasterPage.Equals("ajuda") &&
                        webPage.ParentPageId != null && webPage.Name.Contains(text)).SortAndPage(sortExpression,
                                                                                                 startRowIndex,
                                                                                                 maximumRows, "Name");
            }
            return null;
        }

        public Int32 GetHelpPagesCount(Int32 companyId, String text, string sortExpression, int startRowIndex,
                                       int maximumRows)
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