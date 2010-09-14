using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfoControl;
using InfoControl.Data;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules.Reports
{
    public class ReportsManager : BusinessManager<InfoControlDataContext>
    {
        public ReportsManager(IDataAccessor container)
            : base(container)
        {
        }

        #region CRUD

        /// <summary>
        /// this method insert the Report
        /// </summary>
        /// <param name="entity"></param>
        public void InsertReport(Report entity)
        {
            DbContext.Reports.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method update the report
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        public void UpdateReport(Report original_entity, Report entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method delete the report
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteReport(Report entity)
        {
            DbContext.Reports.Attach(entity);
            DbContext.Reports.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        #endregion

        /// <summary>
        /// this method return a specific report 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public Report GetReport(Int32 reportId)
        {
            return DbContext.Reports.Where(r => r.ReportId == reportId).FirstOrDefault();
        }

        /// <summary>
        /// this method return the reports
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Report> GetReports(string sortExpression, int startRowIndex, int maximumRows)
        {
            return DbContext.Reports.SortAndPage(sortExpression, startRowIndex, maximumRows, "ReportId");
        }

        /// <summary>
        /// this is the cout method of GetReports
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetReportsCount(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetReports(sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// Get reports for part name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public DataReader SearchReport(string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            if (DataManager.CacheCommands[methodName] == null)
            {
                IQueryable<Report> query = from report in DbContext.Reports
                                           where report.Name.ToUpper().Contains(name.ToUpper())
                                           select report;
                DataManager.CacheCommands[methodName] = DbContext.GetCommand(query);
            }

            return DataManager.ExecuteCachedQuery(methodName, "%" + name + "%");
        }

        public string[] SearchReportAsArray(string name, Int32 maximumRows)
        {
            string methodName = MethodBase.GetCurrentMethod().ToString();

            if (DataManager.CacheCommands[methodName] == null)
            {
                IQueryable<string> query = from reporters in DbContext.Reports
                                           where reporters.Name.Contains(name)
                                           select reporters.Name;
                DataManager.CacheCommands[methodName] = DbContext.GetCommand(query);
            }

            DataReader reader = DataManager.ExecuteCachedQuery(methodName, "%" + name + "%", "%" + name + "%");
            var list = new List<string>();
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            return list.ToArray();
        }

        #region ReportTablesSchema

        /// <summary>
        /// this method return ReportTablesSchema
        /// </summary>
        /// <returns></returns>
        public IQueryable<ReportTablesSchema> GetReportTablesSchema()
        {
            return DbContext.ReportTablesSchemas;
        }

        #endregion
    }
}