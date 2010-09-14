using System;
using System.Collections.Generic;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class PlanManager
    {
        /// <summary>
        /// Returns all plans
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public List<Plan> GetAllPlansList(string sortExpression, int startRowIndex, int maximumRows)
        {
            IQueryable<Plan> x = GetAllPlans();
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "PlanId").ToList();
        }


        public IQueryable GetPlans(DateTimeInterval dateTimeInterval, String name, string sortExpression,
                                   int startRowIndex, int maximumRows)
        {
            var query = from plan in DbContext.Plans
                        join package in DbContext.Packages on plan.PackageId equals package.PackageId
                        where
                            plan.AvailableStartDate >= dateTimeInterval.BeginDate &&
                            plan.AvailableEndDate <= dateTimeInterval.EndDate
                        select new
                                   {
                                       plan.PlanId,
                                       PlanName = plan.Name,
                                       AvailableEndDate = plan.AvailableEndDate.Date,
                                       AvailableStartDate = plan.AvailableStartDate.Date,
                                       PackageName = package.Name
                                   };

            if (!String.IsNullOrEmpty(name))
                query = query.Where(plan => plan.PlanName.Contains(name));

            return query.SortAndPage(sortExpression, startRowIndex, maximumRows, "PlanId");
        }

        public Int32 GetPlansCount(DateTimeInterval dateTimeInterval, String name, string sortExpression,
                                   int startRowIndex, int maximumRows)
        {
            return GetPlans(dateTimeInterval, name, sortExpression, startRowIndex, maximumRows).Cast<Object>().Count();
        }


        /// <summary>
        /// Returns a single plan that are in use
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Plan GetCurrentPlan(Int32 companyId)
        {
            IQueryable<Plan> query = from cmp in DbContext.Companies
                                     join cmp2 in DbContext.Companies on cmp.ReferenceCompanyId equals cmp2.CompanyId
                                     join pla in DbContext.Plans on cmp2.PlanId equals pla.PlanId
                                     where cmp.CompanyId == companyId
                                     select pla;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Return all plan that are not outdated
        /// </summary>
        /// <returns></returns>
        public IQueryable<Plan> GetActivePlans()
        {
            return GetAllPlans().Where(pl => pl.AvailableEndDate >= DateTime.Now).OrderBy(p => p.Package.Price);
        }

        /// <summary>
        /// Return all plan that are not outdated
        /// </summary>
        /// <returns></returns>
        public IQueryable<Plan> GetActivePlansByBranch(Int32 branchId)
        {
            IQueryable<Plan> query = GetActivePlans();
            if (branchId == 0)
            {
                query = query.Where(pl => pl.BranchId == null);
            }
            else
            {
                query = query.Where(pl => pl.BranchId == branchId);
            }
            return query;
        }
    }
}