using System;
using System.Collections;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class ApplicationManager : BusinessManager<InfoControlDataContext>
    {
        public ApplicationManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Applications.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<Application> GetAllApplications()
        {
            return DbContext.Applications;
        }

        /// <summary>
        /// This method gets record counts of all Applications.
        /// Do not change this method.
        /// </summary>
        public int GetAllApplicationsCount()
        {
            return GetAllApplications().Count();
        }

        /// <summary>
        /// This method retrieves a single Application.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=ApplicationId>ApplicationId</param>
        public Application GetApplication(Int32 ApplicationId)
        {
            return DbContext.Applications.Where(x => x.ApplicationId == ApplicationId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all Applications.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public IList GetAllApplications(string sortExpression, int startRowIndex, int maximumRows)
        {
            return
                GetAllApplications().SortAndPage(sortExpression, startRowIndex, maximumRows, "ApplicationId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(Application entity)
        {
            DbContext.Applications.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(Application entity)
        {
            DbContext.Applications.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(Application original_entity, Application entity)
        {
            DbContext.Applications.Attach(original_entity);
            original_entity.Name = entity.Name;
            original_entity.Description = entity.Description;
            original_entity.IsMaintenance = entity.IsMaintenance;
            original_entity.IsActive = entity.IsActive;
            original_entity.MaintenanceMessage = entity.MaintenanceMessage;
            DbContext.SubmitChanges();
        }
    }
}