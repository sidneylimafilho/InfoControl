using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class PlanManager : BusinessManager<InfoControlDataContext>
    {
        public PlanManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Plans.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Plan> GetAllPlans()
        {
            return DbContext.Plans.OrderBy(Plan => Plan.Name);
        }

        /// <summary>
        /// This method gets record counts of all Plans.
        /// Do not change this method.
        /// </summary>
        public int GetAllPlansCount()
        {
            return GetAllPlans().Count();
        }

        /// <summary>
        /// This method retrieves a single Plan.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=PlanId>PlanId</param>
        public Plan GetPlan(Int32 PlanId)
        {
            return DbContext.Plans.Where(x => x.PlanId == PlanId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves Plan by Packages.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=PackageId>PackageId</param>
        public IQueryable<Plan> GetPlanByPackages(Int32 PackageId)
        {
            return DbContext.Plans.Where(x => x.PackageId == PackageId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all Plans filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetPlans(string tableName, Int32 Packages_PackageId, string sortExpression, int startRowIndex,
                              int maximumRows)
        {
            IQueryable<Plan> x = GetFilteredPlans(tableName, Packages_PackageId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "PlanId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<Plan> GetFilteredPlans(string tableName, Int32 Packages_PackageId)
        {
            switch (tableName)
            {
                case "Packages_Plans":
                    return GetPlanByPackages(Packages_PackageId);
                default:
                    return GetAllPlans();
            }
        }

        /// <summary>
        /// This method gets records counts of all Plans filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetPlansCount(string tableName, Int32 Packages_PackageId)
        {
            IQueryable<Plan> x = GetFilteredPlans(tableName, Packages_PackageId);
            return x.Count();
        }

        /// <summary>
        /// Save the Plan and return Plan connected
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public Plan Save(Plan plan)
        {
            if (plan.PlanId > 0)
            {
                Plan originalPlan = GetPlan(plan.PlanId);
                originalPlan.CopyPropertiesFrom(plan);
                originalPlan.ModifiedDate = DateTime.Now;
                DbContext.SubmitChanges();
                plan = originalPlan;
            }
            else
            {
                plan.ModifiedDate = DateTime.Now;
                DbContext.Plans.InsertOnSubmit(plan);
                DbContext.SubmitChanges();
            }
            return plan;
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Int32 planId)
        {
            DbContext.Plans.DeleteOnSubmit(GetPlan(planId));
            DbContext.SubmitChanges();
        }
    }
}