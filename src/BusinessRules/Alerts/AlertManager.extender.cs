using System;
using System.Collections.Generic;
using System.Linq;
using InfoControl.Data;
using Vivina.Erp.DataClasses;
using InfoControl;

namespace Vivina.Erp.BusinessRules
{
    public partial class AlertManager : BusinessManager<InfoControlDataContext>
    {
        public AlertManager(IDataAccessor container)
            : base(container)
        {
        }

        /// <summary>
        /// This method retrieves all Budgets.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        private IQueryable<Alert> GetAllAlerts()
        {
            return DbContext.Alerts;
        }

        /// <summary>
        /// This method retrieves alerts from specified user and by reminderTime of alert
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reminderTime"></param>
        /// <returns></returns>
        public IList<Alert> GetAlerts(Int32 userId, DateTime reminderTime)
        {
            IQueryable<Alert> query = from alerts in DbContext.Alerts
                                      where
                                          alerts.UserId == userId &&
                                          (alerts.ShowDate == reminderTime || !alerts.ShowDate.HasValue)
                                      select alerts;
            return query.ToList();
        }

        /// <summary>
        /// This method inserts a new alert in db
        /// </summary>
        /// <param name="alert"></param>
        public void InsertAlert(Alert alert)
        {
            DbContext.Alerts.InsertOnSubmit(alert);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates an alert from db
        /// </summary>
        /// <param name="originalAlert"></param>
        /// <param name="alert"></param>
        private void UpdateAlert(Alert originalAlert, Alert alert)
        {
            originalAlert.CopyPropertiesFrom(alert);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method retrieves a specified alert
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public Alert GetAlert(Int32 subjectId, string pageName)
        {
            return DbContext.Alerts.Where(alert => alert.SubjectId == subjectId && alert.PageName == pageName).FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves a specified alert
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public Alert GetAlert(Int32 alertId)
        {
            return DbContext.Alerts.Where(alert => alert.AlertId == alertId).FirstOrDefault();
        }
        
        /// <summary>
        /// This method saves(insert or update) an alert in db
        /// </summary>
        /// <param name="alert"></param>
        public void SaveAlert(Alert alert)
        {
            if (alert.AlertId == 0)
                InsertAlert(alert);
            else
            {
                var originalAlert = this.GetAlert(alert.AlertId);
                UpdateAlert(originalAlert, alert);
            }
        }

        /// <summary>
        /// This method deletes the specified alert from db
        /// </summary>
        /// <param name="alertId"></param>
        public void DeleteAlert(Int32 alertId)
        {
            DbContext.Alerts.DeleteAllOnSubmit(DbContext.Alerts.Where(al => al.AlertId == alertId));
            DbContext.SubmitChanges();
        }


        /// <summary>
        /// This method deletes alerts with specified subjectId and pageName 
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="pageName"></param>
        public void DeleteAlerts(Int32 subjectId, string pageName)
        {
            DbContext.Alerts.DeleteAllOnSubmit(GetAlerts(subjectId, pageName));
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method retrieves alerts with specified subjectId and pageName
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public IQueryable<Alert> GetAlerts(Int32 subjectId, string pageName)
        {
            return DbContext.Alerts.Where(alert => alert.SubjectId == subjectId && alert.PageName == pageName);
        }

        /// <summary>
        /// This method inserts an alert
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="description"></param>
        public void InsertAlert(Int32 userID, String description)
        {
            var alert = new Alert(userID, description);
            InsertAlert(alert);
        }
    }
}