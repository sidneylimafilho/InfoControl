using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class CustomerManager : BusinessManager<InfoControlDataContext>
    {
        public CustomerManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Customers.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Customer> GetAllCustomers()
        {
            //// 
            return DbContext.Customers;
        }

        /// <summary>
        /// This method gets record counts of all Customers.
        /// Do not change this method.
        /// </summary>
        public int GetAllCustomersCount()
        {
            return GetAllCustomers().Count();
        }

        /// <summary>
        /// This method retrieves a single Customer.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CustomerId>CustomerId</param>
        /// <param name=CompanyId>CompanyId</param>        
        public Customer GetCustomer(int CustomerId, int CompanyId)
        {
            return DbContext.Customers.Where(x => x.CustomerId == CustomerId && x.CompanyId == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Customer by Company.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=CompanyId>CompanyId</param>
        public IQueryable<Customer> GetCustomerByCompany(Int32 companyId)
        {
            return DbContext.Customers.Where(x => x.CompanyId == companyId);
        }



        /// <summary>
        /// Return all customers of a company, as queryable
        /// this method are ready for client sort and page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable GetCustomerByCompany(Int32 companyId, Int32? representantId, string sortExpression, string initialLetter,
                                               int startRowIndex, int maximumRows)
        {
            var query = from customer in GetCustomerByCompany(companyId)
                        join legalEntityProfile in DbContext.LegalEntityProfiles on customer.LegalEntityProfileId equals
                            legalEntityProfile.LegalEntityProfileId into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        join profile in DbContext.Profiles on customer.ProfileId equals profile.ProfileId into gProfile
                        from profile in gProfile.DefaultIfEmpty()
                        select new
                                   {
                                       ProfileName = profile.Name,
                                       customer.LegalEntityProfile.CompanyName,
                                       customer.LegalEntityProfile.FantasyName,
                                       Name = legalEntityProfile.CompanyName ?? profile.Name,
                                       Phone = legalEntityProfile.Phone ?? profile.Phone,
                                       customer.CustomerId,
                                       Identification = legalEntityProfile.CNPJ ?? profile.CPF,
                                       Email = legalEntityProfile.Email ?? profile.Email,
                                       customer.CompanyId,
                                       customer.BlockSalesInDebit,
                                       customer.CustomerTypeId,
                                       customer.ModifiedDate,
                                       customer.Profile,
                                       customer.ProfileId,
                                       customer.LegalEntityProfile,
                                       customer.LegalEntityProfileId,
                                       customer.SalesPersonId,
                                       customer.SupplementalSalesPersonId,
                                       customer.SalesPersonCommission,
                                       customer.SupplementalSalesPersonCommission,
                                       customer.UserId,
                                       customer.BankId,
                                       customer.Agency,
                                       customer.AccountNumber,
                                       customer.Ranking,
                                       customer.Observation,
                                       customer.RepresentantId
                                   };
            if (!String.IsNullOrEmpty(initialLetter))
                query = query.Where(customer => customer.Name.StartsWith(initialLetter));

            if (representantId.HasValue)
                query = query.Where(customer => customer.RepresentantId == representantId);

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "Name").AsQueryable();
        }

        public Int32 GetCustomerByCompanyCount(Int32 companyId, Int32? representantId, string sortExpression, string initialLetter,
                                               int startRowIndex, int maximumRows)
        {
            return
                GetCustomerByCompany(companyId, representantId, sortExpression, initialLetter, startRowIndex, maximumRows).Cast
                    <IQueryable>().Count();
        }


        //public IQueryable GetCustomerByCompany(Int32 companyId, Int32? representantId, string sortExpression, string initialLetter,
        //                                       int startRowIndex, int maximumRows)
        //{
        //    var query = GetCustomerByCompany(companyId, sortExpression, initialLetter, startRowIndex, maximumRows);

        //    if(representantId.HasValue)
        //        query = query.where

        //}

        /// <summary>
        /// This method returns quantity of customers by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int GetCustomersByCompanyCount(int companyId)
        {
            return GetCustomerByCompany(companyId).Count();
        }

        ///// <summary>
        ///// This method gets sorted and paged records of all Customers filtered by a specified field.
        ///// Do not change this method.
        ///// </summary>
        //public System.Collections.IList GetCustomers(string tableName, Int32 Company_CompanyId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    IQueryable<Customer> x = GetFilteredCustomers(tableName, Company_CompanyId);
        //    return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CustomerId").ToList();
        //}

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Customer> GetFilteredCustomers(string tableName, Int32 Company_CompanyId)
        {
            switch (tableName)
            {
                case "Company_Customers":
                    return GetCustomerByCompany(Company_CompanyId);
                default:
                    return GetAllCustomers();
            }
        }

        /// <summary>
        /// This method gets records counts of all Customers filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetCustomersCount(int companyId)
        {
            return GetCustomerByCompany(companyId).Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Customer entity)
        {
            // 
            // DbContext.Customers.Attach(entity);
            DbContext.Customers.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        //public void Insert(Customer entity)
        //{
        //   // 
        //    Context.Customers.InsertOnSubmit(entity);
        //    Context.SubmitChanges();
        //}
        /// <summary>
        /// This method return name of customer
        /// </summary>
        /// <param name=entity>entity</param>
        public IList getNames(int matrixId)
        {
            var query = from customer in DbContext.Customers
                        join profile in DbContext.Profiles
                            on customer.ProfileId equals profile.ProfileId
                            into gprofile
                        from profile in gprofile.DefaultIfEmpty()
                        join legalEntityProfile in DbContext.LegalEntityProfiles
                            on customer.LegalEntityProfileId equals legalEntityProfile.LegalEntityProfileId
                            into gLegalEntityProfile
                        from legalEntityProfile in gLegalEntityProfile.DefaultIfEmpty()
                        where customer.CompanyId == matrixId
                        select
                            new { Name = profile.Name ?? "" + legalEntityProfile.CompanyName ?? "", customer.CustomerId };
            return query.ToList();
        }

        /// <summary>
        /// This method return customer by companyId and cpf
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Customer GetCustomer(int companyId, string cpfOrCnpj)
        {
            return
                GetCustomerByCompany(companyId).Where(
                    c => c.Profile.CPF == cpfOrCnpj || c.LegalEntityProfile.CNPJ == cpfOrCnpj).FirstOrDefault();
        }
    }
}