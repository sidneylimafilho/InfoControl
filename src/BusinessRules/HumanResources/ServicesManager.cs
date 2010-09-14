using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules.HumanResources
{
    public class ServicesManager : BusinessManager<InfoControlDataContext>
    {
        public ServicesManager(IDataAccessor container) : base(container)
        {
        }

        #region History

        /// <summary>
        /// Returns a DataSet with all information about the history, including the service name 
        /// and Organization Level name.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        //public System.Collections.IList GetServiceHistories(int companyId, int employeeId)
        //{
        //    
        //    var query = from his in Context.ServiceHistories
        //                join org in Context.OganizationLevels on his.OrganizationLevelId equals org.OrganizationlevelId
        //                join serv in Context.Services on his.ServiceId equals serv.ServiceId
        //                where his.EmployeeId == employeeId && his.CompanyId == companyId
        //                select new { his, orgName = org.Name, servName = serv.Name };
        //    return query.ToList();
        //}
        ///// <summary>
        ///// Return a single row from the table ServiceHistory
        ///// </summary>
        ///// <param name="serviceHistoryId"></param>
        ///// <returns></returns>
        //public ServiceHistory GetServiceHistory(int serviceHistoryId)
        //{
        //    
        //    return Context.ServiceHistories.Where(x => x.ServiceHistoryId == serviceHistoryId).FirstOrDefault();
        //}

        ///// <summary>
        ///// Basic Insert Method
        ///// </summary>
        ///// <param name="entity"></param>
        //public void InsertServiceHistory(ServiceHistory entity)
        //{
        //    
        //    Context.ServiceHistories.InsertOnSubmit(entity);
        //    Context.SubmitChanges();
        //}
        ///// <summary>
        ///// Basic Update Method
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="original_entity"></param>
        //public void UpdateServiceHistory(ServiceHistory entity, ServiceHistory original_entity)
        //{
        //    
        //    Context.ServiceHistories.Attach(original_entity);
        //    original_entity.CopyPropertiesFrom(entity);
        //    Context.SubmitChanges();
        //}
        ///// <summary>
        ///// Basic Delete Method
        ///// </summary>
        ///// <param name="entity"></param>
        //public void DeleteServiceHistory(ServiceHistory entity)
        //{
        //    
        //    Context.ServiceHistories.Attach(entity);
        //    Context.ServiceHistories.DeleteOnSubmit(entity);
        //    Context.SubmitChanges();
        //}

        #endregion
    }
}