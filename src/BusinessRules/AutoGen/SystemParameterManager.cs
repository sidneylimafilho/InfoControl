using System;
using System.Collections;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public partial class SystemParameterManager : BusinessManager<InfoControlDataContext>
    {
        public SystemParameterManager(IDataAccessor container) : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all SystemParameters.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<SystemParameter> GetAllSystemParameters()
        {
            return DbContext.SystemParameters;
        }

        /// <summary>
        /// This method gets record counts of all SystemParameters.
        /// Do not change this method.
        /// </summary>
        public int GetAllSystemParametersCount()
        {
            return GetAllSystemParameters().Count();
        }

        /// <summary>
        /// This method retrieves a single SystemParameter.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=SystemParameterId>SystemParameterId</param>
        public SystemParameter GetSystemParameter(Int32 SystemParameterId)
        {
            return DbContext.SystemParameters.Where(x => x.SystemParameterId == SystemParameterId).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves SystemParameter by Application.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        /// <param name=ApplicationId>ApplicationId</param>
        public IQueryable<SystemParameter> GetSystemParameterByApplication(Int32 ApplicationId)
        {
            return DbContext.SystemParameters.Where(x => x.ApplicationId == ApplicationId);
        }

        /// <summary>
        /// This method gets sorted and paged records of all SystemParameters filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public IList GetSystemParameters(string tableName, Int32 Application_ApplicationId, string sortExpression,
                                         int startRowIndex, int maximumRows)
        {
            IQueryable<SystemParameter> x = GetFilteredSystemParameters(tableName, Application_ApplicationId);
            return x.SortAndPage(sortExpression, startRowIndex, maximumRows, "SystemParameterId").ToList();
        }

        /// <summary>
        /// This method routes a request for filtering by a field value to another method.
        /// Do not change this method.
        /// </summary>
        private IQueryable<SystemParameter> GetFilteredSystemParameters(string tableName,
                                                                        Int32 Application_ApplicationId)
        {
            switch (tableName)
            {
                case "Application_SystemParameters":
                    return GetSystemParameterByApplication(Application_ApplicationId);
                default:
                    return GetAllSystemParameters();
            }
        }

        /// <summary>
        /// This method gets records counts of all SystemParameters filtered by a specified field.
        /// Do not change this method.
        /// </summary>
        public int GetSystemParametersCount(string tableName, Int32 Application_ApplicationId)
        {
            IQueryable<SystemParameter> x = GetFilteredSystemParameters(tableName, Application_ApplicationId);
            return x.Count();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(SystemParameter entity)
        {
            DbContext.SystemParameters.Attach(entity);
            DbContext.SystemParameters.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Insert(SystemParameter entity)
        {
            DbContext.SystemParameters.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        public void Update(SystemParameter original_entity, SystemParameter entity)
        {
            DbContext.SystemParameters.Attach(original_entity);
            original_entity.Name = entity.Name;
            original_entity.Value = entity.Value;
            original_entity.Description = entity.Description;
            original_entity.InitialValue = entity.InitialValue;
            original_entity.ApplicationId = entity.ApplicationId;
            DbContext.SubmitChanges();
        }
    }
}