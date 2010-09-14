using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfoControl.Data;
using InfoControl.Web.ScheduledTasks;
using InfoControl.Web.Configuration;

namespace InfoControl.Web.ScheduledTasks
{
    public class SchedulerManager : BusinessManager<ScheduledTasksDataContext>
    {
        public SchedulerManager(IDataAccessor container)
            : base(container)
        {

        }


        #region Methods

        /// <summary>
        /// This method retrieves all ScheduleTask.
        /// Change this method to alter how records are retrieved.
        /// </summary>
        public IQueryable<ScheduledTask> GetAllScheduleTasks()
        {
            return DbContext.ScheduledTasks;
        }

        /// <summary>
        /// This method gets record counts of all ScheduleTask.
        /// Do not change this method.
        /// </summary>
        public int GetAllScheduleTasksCount()
        {
            return GetAllScheduleTasks().Count();
        }

        /// <summary>
        /// This method retrieves a single ScheduleTask.
        /// Change this method to alter how that record is received.
        /// </summary>
        /// <param name=CategoryId>ScheduleTaskId</param>
        public ScheduledTask GetScheduleTask(Int32 ScheduleTaskId)
        {
            // 
            return DbContext.ScheduledTasks.Where(x => x.ScheduledTaskId == ScheduleTaskId).FirstOrDefault();
        }

        /// <summary>
        /// This method pages and sorts over all ScheduleTask.
        /// Do not change this method.
        /// </summary>
        /// <param name=sortExpression>sortExpression</param>
        /// <param name=startRowIndex>startRowIndex</param>
        /// <param name=maximumRows>maximumRows</param>
        public System.Collections.IList GetAllScheduleTasks(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetAllScheduleTasks().SortAndPage(sortExpression, startRowIndex, maximumRows, "ScheduledTaskId").ToList();
        }

        /// <summary>
        /// This method deletes a record in the table.
        /// Change this method to alter how records are deleted.
        /// </summary>
        /// <param name=entity>entity</param>
        public void Delete(ScheduledTask entity)
        {
            // 
            DbContext.ScheduledTasks.Attach(entity);
            DbContext.ScheduledTasks.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method inserts a new record in the table.
        /// Change this method to alter how records are inserted.
        /// </summary>
        /// <param name=entity>entity</param>
        private void Insert(ScheduledTask entity)
        {
            //
            DbContext.ScheduledTasks.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// This method updates a record in the table.
        /// Change this method to alter how records are updated.
        /// </summary>
        /// <param name=original_entity>original_entity</param>
        /// <param name=entity>entity</param>
        private void Update(ScheduledTask original_entity, ScheduledTask entity)
        {
            //
            //DbContext.ScheduledTasks.Attach(original_entity);
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();

        }

        public void SaveScheduleTask(ScheduledTask entity)
        {
            if (entity.ScheduledTaskId > 0)
            {
                var scheduleTask = GetScheduleTask(entity.ScheduledTaskId);
                scheduleTask.CopyPropertiesFrom(entity);
            }
            else
                DbContext.ScheduledTasks.InsertOnSubmit(entity);

            DbContext.SubmitChanges();
        }

        #endregion

    }
}
