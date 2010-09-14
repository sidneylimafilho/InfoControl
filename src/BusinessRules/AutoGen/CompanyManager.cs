using System;
using System.Linq;
using System.Web;
using InfoControl;
using InfoControl.Data;
using InfoControl.Web.Security;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class CompanyManager : BusinessManager<InfoControlDataContext>
    {
        public CompanyManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Companies.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Company> GetAllCompanies()
        {          
            return DbContext.Companies;
        }

        /// <summary>
        /// This method gets record counts of all Companies.
        /// Do not change this method.
        /// </summary>
        public int GetAllCompaniesCount()
        {
            return GetAllCompanies().Count();
        }

        /// <summary>
        /// This method retrieves a single Company.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=companyId>companyId</param>
        public Company GetCompany(Int32 companyId)
        {
            return DbContext.Companies.Where(x => x.CompanyId == companyId).FirstOrDefault();
        }


        /// <summary>
        /// This method retrieves a single Company.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=companyId>companyId</param>
        public Company GetCompany(string cnpj)
        {
            return DbContext.Companies.Where(x => x.LegalEntityProfile.CNPJ == cnpj).FirstOrDefault();
        }

        public Company GetCompanyByContext(HttpContext context)
        {
            Company company = null;
            if (context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                company = GetCurrentAdminCompany(context.User.Identity.Name);
            }
            else if (!String.IsNullOrEmpty(context.Request["cid"]))
            {
                int cid = Convert.ToInt32(context.Request["cid"]);
                company = GetCompany(cid) ?? new Company { CompanyId = cid };
            }

            return company ?? GetCompanyByWebSite(context.Request.Url.Host.ToLower()) ?? GetHostCompany();
        }


        /// <summary>
        /// This method verify if exists an register of companyUser with user supplied
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsCompanyUser(Int32 companyId, String userName)
        {
            if (new MembershipManager(this).GetUserByName(userName) != null)
                return (GetCompanyUser(companyId, userName) != null);

            return false;
        }


        //public Company GetCompany(Int32 companyId, Int32 userId, String webSite)
        //{
        //}

        ///// <summary>
        ///// This method gets records counts of all Companies filtered by a specified field.
        ///// Do not change this method.
        ///// </summary>
        //public int GetCompaniesCount(string tableName, Int32 Branch_BranchId, Int32 Plan_PlanId)
        //{
        //    IQueryable<Company> x = GetFilteredCompanies(tableName, Branch_BranchId, Plan_PlanId);
        //    return x.Count();
        //}

        ///// <summary>
        ///// This method gets sorted and paged records of all Companies filtered by a specified field.
        ///// Do not change this method.
        ///// </summary>
        //public System.Collections.IList GetCompanies(string tableName, Int32 Branch_BranchId, Int32 Plan_PlanId, string sortExpression, int startRowIndex, int maximumRows)
        //{
        //    IQueryable<Company> x = GetFilteredCompanies(tableName, Branch_BranchId, Plan_PlanId);
        //    return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "CompanyId").ToList();
        //}

        /// <summary>
        /// This method retrieves Company by Branch.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=branchId>branchId</param>
        public IQueryable<Company> GetCompanyByBranch(Int32 branchId)
        {
            ////InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            return DbContext.Companies.Where(x => x.LegalEntityProfile.BranchId == branchId);
        }

        /// <summary>
        /// This method retrieves Company by Plan.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=planId>planId</param>
        public IQueryable<Company> GetCompanyByPlan(Int32 planId)
        {
            ////InfoControlDataContext Context = DataManager.CreateContext<InfoControlDataContext>();
            return DbContext.Companies.Where(x => x.PlanId == planId);
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Company> GetFilteredCompanies(string tableName, Int32 Branch_BranchId, Int32 Plan_PlanId)
        {
            switch (tableName)
            {
                case "Branch_Companies":
                    return GetCompanyByBranch(Branch_BranchId);
                case "Plan_Companies":
                    return GetCompanyByPlan(Plan_PlanId);
                default:
                    return GetAllCompanies();
            }
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// This method uses the method companyDelete
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Company entity)
        {
            DeleteCompany(entity.CompanyId);
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        private void Insert(Company entity)
        {
            entity.StartDate = entity.ModifiedDate = DateTime.Now;
            entity.NextStatementDueDate = DateTime.Now.AddMonths(1);
            DbContext.Companies.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Company original_entity, Company entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
            if (entity.MatrixId == null)
                entity.MatrixId = entity.CompanyId;
        }

        /// <summary>
        /// Get a configuration of the company
        /// </summary>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public CompanyConfiguration GetCompanyConfiguration(Int32 configurationId)
        {
            return DbContext.CompanyConfigurations
                .Where(cc => cc.CompanyConfigurationId == configurationId)
                .FirstOrDefault();
        }
    }
}