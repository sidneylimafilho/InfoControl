using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Vivina.Framework.Configuration;
using Vivina.Framework.Data;
using Vivina.InfoControl.DataClasses;

namespace Vivina.InfoControl.BusinessRules.Projects
{
    public partial class ProjectManager : Vivina.Framework.Data.BusinessManager<InfoControlDataContext>
    {
        //#region Project
        public ProjectManager(IDataAccessor container) : base(container) { }
        ///// <summary>
        ///// This method returns Projects
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <returns></returns>
        //public IQueryable<Project> GetProjects(int companyId)
        //{

        //    return DbContext.Projects.Where(x => x.CompanyId == companyId);
        //}
        ///// <summary>
        ///// This method returns a single Project
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <param name="projectId"></param>
        ///// <returns></returns>
        //public Project GetProject(int companyId, int projectId)
        //{

        //    return DbContext.Projects.Where(x => x.CompanyId == companyId && x.ProjectId == projectId).FirstOrDefault();
        //}
        ///// <summary>
        ///// This is a basic insert method, change this method to change how the Projects are inserted
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public int InsertProject(Project entity)
        //{

        //    DbContext.Projects.InsertOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //    return entity.ProjectId;
        //}
        ///// <summary>
        ///// This is a basic Update Method, change this method to change how the Projects are updated
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="original_entity"></param>
        //public void UpdateProject(Project entity, Project original_entity)
        //{

        //    DbContext.Projects.Attach(original_entity);
        //    original_entity.CopyPropertiesFrom(entity);
        //    DbContext.SubmitChanges();
        //}

        ///// <summary>
        ///// This is a basic Delete Method, change this method to change how the Projects are Deleted
        ///// </summary>
        ///// <param name="entity"></param>
        //public void DeleteProject(Project entity)
        //{

        //    DbContext.Projects.Attach(entity);
        //    DbContext.Projects.DeleteOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}
        //#endregion
        //#region Task
        ///// <summary>
        ///// This is a basic Insert Method, change this method to change how the Task are inserted
        ///// </summary>
        ///// <param name="entity"></param>
        //public void InsertTask(Task entity)
        //{

        //    DbContext.Tasks.InsertOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}
        ///// <summary>
        ///// This is a basic Delete Method, change this method to change how the Tasks are Deleted
        ///// </summary>
        ///// <param name="entity"></param>
        //public void DeleteTask(Task entity)
        //{

        //    DbContext.Tasks.Attach(entity);
        //    DbContext.Tasks.DeleteOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}
        ///// <summary>
        ///// This method returns the Tasks, using the projectId 
        ///// The tasks are sorted by the ParentTaskId
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <param name="projectId"></param>
        ///// <returns></returns>
        //public IQueryable<Task> GetTasks(int companyId, int projectId)
        //{

        //    return DbContext.Tasks.Where(x => x.CompanyId == companyId && x.ProjectId == projectId).Sort("ParentTaskId");
        //}
        ///// <summary>
        ///// This method returns all task types for a single company
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <returns></returns>
        //public IQueryable<TaskType> GetTaskTypes(int companyId)
        //{

        //    return DbContext.TaskTypes.Where(x => x.CompanyId == companyId);
        //}
        ///// <summary>
        ///// This method returns a single Task
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <param name="projectId"></param>
        ///// <returns></returns>
        //public Task GetTask(int companyId, int projectId, int taskId)
        //{

        //    return DbContext.Tasks.Where(x => x.CompanyId == companyId && x.ProjectId == projectId && x.TaskId == taskId).FirstOrDefault();
        //}
        //#endregion
        //#region TimeEntry
        ///// <summary>
        ///// This method returns the TimeEntry ordered by the BeginTime
        ///// </summary>
        ///// <param name="companyId"></param>
        ///// <param name="taskId"></param>
        ///// <param name="projectId"></param>
        ///// <returns></returns>
        //public IQueryable<TimeEntry> GetTimeSheet(int companyId, int taskId, int projectId)
        //{

        //    return DbContext.TimeEntries.Where(x => x.CompanyId == companyId && x.ProjectId == projectId && x.TaskId == taskId).Sort("BeginTime");
        //}
        ///// <summary>
        ///// Returns a single line of the TimeEntry Table
        ///// </summary>
        ///// <param name="timeSheetId"></param>
        ///// <returns></returns>
        //public TimeEntry GetTimeSheet(int timeSheetId)
        //{

        //    return DbContext.TimeEntries.Where(x => x.TimeEntryId== timeSheetId).FirstOrDefault();
        //}
        ///// <summary>
        ///// This method is a basic Insert method,  change this method to change how the TimeSheet are inserted
        ///// </summary>
        ///// <param name="entity"></param>
        //public void InsertTimeSheet(TimeEntry entity)
        //{

        //    TimeEntry tSheet = DbContext.TimeEntries.Where(x => x.EmployeeId == entity.EmployeeId && (x.BeginTime < entity.BeginTime && x.EndTime > entity.BeginTime) || (x.BeginTime < entity.EndTime && x.EndTime > entity.BeginTime)).FirstOrDefault();
        //    if (tSheet != null)
        //    {
        //        ArgumentException ex = new ArgumentException("O intervalo informado coincide com um intervalo ja existente ! verifique um registro de horário naõ finalizado");
        //        throw ex;
        //    }
        //    DbContext.TimeEntries.InsertOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}
        ///// <summary>
        ///// This method is a basic Delete method. Change this method to change how the TimeEntry are deleted.
        ///// </summary>
        ///// <param name="entity"></param>
        //public void DeleteTimeSheet(TimeEntry entity)
        //{

        //    DbContext.TimeEntries.Attach(entity);
        //    DbContext.TimeEntries.DeleteOnSubmit(entity);
        //    DbContext.SubmitChanges();
        //}
        ///// <summary>
        ///// This method is a basic Update method. Change this method to change how the TimeEntry are updated.
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="original_entity"></param>
        //public void UpdateTimeSheet(TimeEntry entity, TimeEntry original_entity)
        //{

        //    DbContext.TimeEntries.Attach(original_entity);
        //    TimeEntry tSheet = DbContext.TimeEntries.Where(x => x.EmployeeId == entity.EmployeeId && (x.BeginTime < entity.BeginTime && x.EndTime > entity.BeginTime) || (x.BeginTime < entity.EndTime && x.EndTime > entity.BeginTime)).FirstOrDefault();
        //    //
        //    // Verifies if the user are trying to insert a date/time interval, between any other
        //    // inserted on the table by the same user ... 
        //    // However, if the Begin/End time, remais the same, allow the user to update
        //    //
        //    if (tSheet != null)
        //        if (tSheet.BeginTime != entity.BeginTime || tSheet.EndTime != entity.EndTime)
        //        {
        //            ArgumentException ex = new ArgumentException("O intervalo informado coincide com um intervalo ja existente ! verifique um registro de horário naõ finalizado");
        //            throw ex;
        //        }            
        //    original_entity.CopyPropertiesFrom(entity);
        //    DbContext.SubmitChanges();
        //}
        //#endregion
        //#region Priority
        ///// <summary>
        ///// This method returns all the priorities
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Priority> GetPriorities()
        //{

        //    return DbContext.Priorities.Sort("Name");
        //}
        //#endregion
    }
}


