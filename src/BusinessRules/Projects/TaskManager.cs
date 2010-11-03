using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfoControl;
using InfoControl.Data;
using InfoControl.Security.Cryptography;
using InfoControl.Web.Security;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{

    #region TaskStatus enum

    public enum FilterType
    {
        Date = 1,
        Hierarchy
    }

    #endregion

    public class TaskManager : BusinessManager<InfoControlDataContext>
    {
        public TaskManager(IDataAccessor container)
            : base(container)
        {
        }

        #region Tasks

        /// <summary>
        /// this method returns a DataTable of tasks
        /// </summary>8
        /// <returns></returns>
        public IQueryable<Task> GetAllTasks()
        {
            return DbContext.Tasks;
        }

        /// <summary>
        /// This method returns a task by specified subjectId and pageName
        /// </summary>
        /// <param name="subjectId">can't be null</param>
        /// <param name="pageName">can't be null</param>
        /// <returns></returns>
        public Task GetTask(Int32 subjectId, string pageName)
        {
            return DbContext.Tasks.Where(task => task.SubjectId == subjectId && task.PageName == pageName).FirstOrDefault();
        }

        /// <summary>
        /// this method returns task as DataTable
        /// </summary>
        /// <returns></returns>
        //public DataTable GetTasksAsDataTable()
        //{
        //    return GetTasks().ToDataTable();
        //}
        /// <summary>
        /// this method returns tasks by user
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private IQueryable<Task> GetTasksByUser(Int32 userId)
        {
            IQueryable<Task> query = DbContext.Tasks.AsQueryable();

            if (userId > 0) query = query.Where(t => t.TaskUsers.Any(x => x.UserId == userId));

            return query;
        }

        /// <summary>
        /// This method returns tasks by user in determined dateTime interval
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateTimeInterval"></param>
        /// <returns></returns>
        private IQueryable GetTasksByUser(Int32 userId, DateTimeInterval dateTimeInterval, String sortExpression,
                                          int maximumRows, int startRowIndex)
        {
            var query =
                from task in
                    GetTasksByUser(userId).Where(
                    task => task.StartDate >= dateTimeInterval.BeginDate && task.FinishDate <= dateTimeInterval.EndDate)
                from taskUser in task.TaskUsers
                select new
                           {
                               task.Name,
                               task.TaskId,
                               task.Description,
                               taskUser.UserId,
                               UserName = taskUser.User.Name,
                               BeginTime = task.StartDate.Value,
                               EndTime = task.FinishDate ?? task.Deadline ?? DateTime.MaxValue
                           };

            return query;
        }

        /// <summary>
        /// This method return all task of an user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public IQueryable<Task> GetTasksByUser(Int32 userId, String sortExpression)
        {
            return GetTasksByUser(userId).Sort(sortExpression ?? "TaskStatusId Desc, Priority Desc");
        }

        /// <summary>
        /// This method returns the quantity of registers 
        /// returned by GetTasksByUser(in respective overload) method
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Int32 GetTasksByUserCount(Int32 userId, DateTimeInterval dateTimeInterval, String sortExpression,
                                         int maximumRows, int startRowIndex)
        {
            return
                GetTasksByUser(userId, dateTimeInterval, sortExpression, maximumRows, startRowIndex).Cast<IQueryable>().
                    Count();
        }

        /// <summary>
        /// This method returns tasks by status, user and filter type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="status"></param>
        /// <param name="filterType"></param>
        /// <returns></returns>


        public IQueryable<Task> GetTasks(Int32? userId, Int32 status, FilterType filterType, String name,
                                   DateTimeInterval dtInterval, int? parentId, int? subjectId, string pageName, Int32? companyId,
                                   String competency, String sortExpression)
        {
            //
            // returns all tasks and users releated  
            //

            var query = GetAllTasks();

            //
            // Returns all tasks from users in one specific company
            //
            if (!userId.HasValue && String.IsNullOrEmpty(competency) && companyId.HasValue)
                query = query.Where(q => q.User.CompanyUsers.Any(u => u.CompanyId.Equals(companyId)));

            //
            // Here is taken apart a case where returns all employees that possess the selected skill
            // passed like parameter
            // 
            if (!String.IsNullOrEmpty(competency))
            {
                query = from qry in query
                        join employee in DbContext.Employees on qry.User.ProfileId equals employee.ProfileId
                        where
                            employee.EmployeeCompetencies.Any(ec => ec.Name.Equals(competency)) &&
                            employee.CompanyId.Equals(companyId)
                        select qry;
            }

            if (userId.HasValue && String.IsNullOrEmpty(competency))
                query = query.Where(task => task.TaskUsers.Any(tu => tu.UserId == userId));

            if (dtInterval != null)
                query = query.Where(t => t.StartDate >= dtInterval.BeginDate && t.FinishDate <= dtInterval.EndDate);

            if (!String.IsNullOrEmpty(pageName))
                query = query.Where(t => t.PageName == pageName);

            if (!String.IsNullOrEmpty(name))
                query = query.Where(t => t.Name.Contains(name));    

            if (filterType == FilterType.Date)
            {
                query = query.Where(t => !t.Tasks.Any());
            }
            else if (filterType == FilterType.Hierarchy)
            {
                query = parentId.HasValue
                            ? query.Where(t => t.ParentTaskId == parentId)
                            : query.Where(t => !t.ParentTaskId.HasValue);
            }

            if (status != 0)
                query = query.Where(s => s.TaskStatusId.Equals(status));

            return query.Sort(sortExpression);
        }

        /// <summary>
        /// this method returns open tasks by user
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        private IQueryable<Task> GetTasksByUser(Int32 userId, String sortExpression, int status)
        {
            IQueryable<Task> query = GetTasksByUser(userId, sortExpression);
            if (status == TaskStatus.Concluded || status == TaskStatus.InProcess || status == TaskStatus.Proposed)
                query = query.Where(task => task.TaskStatusId == status);

            return query;
        }

        /// <summary>
        /// this method returns tasks By user as DataTable
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public DataTable GetTasksByUserAsDataTable(Int32 userId, String sortExpression, int status)
        {
            return GetTasksByUser(userId, sortExpression, status).ToDataTable();
        }

        public DataTable GetTasksAsDataTable(Int32 userId, String sortExpression)
        {
            return GetTasksByUser(userId, sortExpression).ToDataTable();
        }

        public DataTable GetChildTasksAsDataTable(Int32 userId, String sortExpression)
        {
            return GetTasksByUser(userId, sortExpression).Where(task => !task.Tasks.Any()).ToDataTable();
        }

        /// <summary>
        /// this method returns OpenTasks as DataTable
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public DataTable GetOpenedChildTasksAsDataTable(Int32 userId, String sortExpression)
        {
            return
                GetTasksByUser(userId, sortExpression, TaskStatus.Proposed).Where(task => !task.Tasks.Any()).ToDataTable
                    ();
        }

        /// <summary>
        /// this method returns OpenTasks as DataTable
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public IQueryable GetNotClosedTasksAsDataTable(Int32 userId, String sortExpression)
        {
            return GetTasks(userId, TaskStatus.Concluded, FilterType.Hierarchy, String.Empty, null,
                                  null, null, null, null, null, String.Empty);
        }

        /// <summary>
        /// this method returns OpenTasks as DataTable
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public DataTable GetClosedTasksAsDataTable(Int32 userId, String sortExpression)
        {
            return GetTasksByUser(userId, sortExpression, TaskStatus.Concluded).ToDataTable();
        }

        /// <summary>
        /// this method returns OpenTasks as DataTable
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public DataTable GetClosedChildTasksAsDataTable(Int32 userId, String sortExpression)
        {
            return
                GetTasksByUser(userId, sortExpression, TaskStatus.Concluded).Where(task => !task.Tasks.Any()).
                    ToDataTable();
        }

        /// <summary>
        /// this method returns tasks
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IQueryable<Task> GetTasks(string sortExpression, int startRowIndex, int maximumRows)
        {
            return DbContext.Tasks.SortAndPage(sortExpression ?? "Priority Desc", startRowIndex, maximumRows, "TaskId");
        }

        /// <summary>
        /// this is the count method of tasks
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public Int32 GetTasksCount(string sortExpression, int startRowIndex, int maximumRows)
        {
            return GetTasks(sortExpression, startRowIndex, maximumRows).Count();
        }

        /// <summary>
        /// this method return a Task
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task GetTask(Int32 taskId)
        {
            return DbContext.Tasks.Where(task => task.TaskId == taskId).FirstOrDefault();
        }

        /// <summary>
        /// this method inserts a task
        /// </summary>
        /// <param name="entity"></param>
        private void InsertTask(Task entity)
        {
            DbContext.Tasks.InsertOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method updates a task
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        private void UpdateTask(Task original_entity, Task entity)
        {
            original_entity.CopyPropertiesFrom(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method saves task and Taskusers
        /// </summary>
        /// <param name="original_entity"></param>
        /// <param name="entity"></param>
        /// <param name="TaskUsers"></param>
        public void SaveTask(Task original_entity, Task entity, List<User> TaskUsers)
        {
            if (original_entity.TaskId == 0)
            {
                DbContext.Tasks.InsertOnSubmit(original_entity);
            }
            else
            {
                if (entity.TaskStatusId == TaskStatus.Concluded) // Closing Task
                {
                    CompleteTask(original_entity, entity, TaskUsers);
                }
                else if (original_entity.TaskStatusId == TaskStatus.Concluded &&
                         entity.TaskStatusId != TaskStatus.Concluded) // Reopen task
                {
                    if (original_entity.ParentTaskId.HasValue)
                    {
                        Task copy = original_entity.Task1.Duplicate();
                        SaveTask(original_entity.Task1, copy, TaskUsers);
                    }
                }

                //Set the original CreatorUser
                entity.CreatorUserId = original_entity.CreatorUserId;
                original_entity.CopyPropertiesFrom(entity);

                DbContext.SubmitChanges();
            }

            original_entity.ModifiedDate = DateTime.Now;
            DbContext.SubmitChanges();

            //
            // Warranty all users in all hierarchy
            //
            //RemoveAllUsersOfTask(original_entity.TaskId);
            if (TaskUsers != null)
                foreach (User user in TaskUsers)
                    AddUserInTask(original_entity.TaskId, user.UserId);
        }

        private void CompleteTask(Task original_entity, Task entity, List<User> TaskUsers)
        {
            if (original_entity.CreatorUserId == entity.CreatorUserId)
            {
                original_entity.TaskStatusId = TaskStatus.Concluded;
                foreach (Task child in original_entity.Tasks)
                {
                    Task copy = child.Duplicate();

                    copy.ModifiedDate = DateTime.Now;

                    CompleteTask(child, copy, TaskUsers);
                }

                //
                // Close parent Task when all children are closed
                //
                //Task parentTask = original_entity.Task1;
                //if (parentTask != null && parentTask.Tasks.Where(t => !t.IsCompleted).Count() == 0)
                //{
                //    parentTask.IsCompleted = true;
                //    parentTask.ModifiedDate = DateTime.Now;
                //}
            }
            else
            {
                entity.TaskStatusId = original_entity.TaskStatusId;
                RemoveUserOfTask(original_entity.TaskId, entity.CreatorUserId.Value);

                if (TaskUsers != null)
                    TaskUsers.Remove(TaskUsers.Where(u => u.UserId.Equals(entity.CreatorUserId)).FirstOrDefault());
            }

            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method deletes a task
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteTask(Task entity)
        {
            foreach (Task childTask in entity.Tasks.ToList())
                DeleteTask(childTask);

            RemoveAllUsersOfTask(entity.TaskId);
            DbContext.Tasks.DeleteOnSubmit(entity);
            DbContext.SubmitChanges();
        }

        /// <summary>
        /// this method deletes a task
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteTask(int id)
        {
            DeleteTask(GetTask(id));
        }

        #endregion

        #region TaskUsers

        /// <summary>
        /// this method deletes taskUser
        /// </summary>
        /// <param name="taskId"></param>
        private void RemoveAllUsersOfTask(int taskId)
        {
            DbContext.TaskUsers.DeleteAllOnSubmit(DbContext.TaskUsers.Where(taskUser => taskUser.TaskId == taskId));
            DbContext.SubmitChanges();
        }

        public void AddUserInTask(Int32 taskId, Int32 userId)
        {
            AddUserInTask(GetTask(taskId), userId, null);
        }

        public void AddUserInTask(Task task, Int32 userId, Alert alert)
        {
            if (!TaskContainsUser(task, userId))
            {
                if (task.CreatorUserId != userId)
                    InsertAlertOnAddUserInTask(userId, task, alert);

                DbContext.TaskUsers.InsertOnSubmit(new TaskUser
                                                       {
                                                           TaskId = task.TaskId,
                                                           UserId = userId
                                                       });
                DbContext.SubmitChanges();
            }

            //
            // Add user in Parent Task
            //
            //foreach (Task child in task.Tasks)
            //    AddUserInTask(child, userId, alert);

            //
            // Add user in Parent Task
            //
            if (task.ParentTaskId.HasValue && !TaskContainsUser(task.Task1, userId))
                AddUserInTask(task.Task1, userId, alert);
        }

        private void InsertAlertOnAddUserInTask(Int32 userId, Task task, Alert alert)
        {
            alert = alert ?? new Alert();
            if (alert.AlertId != 0) return;

            //
            // Send Alert to New Participant Task
            //
            alert.UserId = userId;
            alert.Description = "Você foi adicionado na tarefa " + CreateTaskHtmlLink(task) + "!";

            new AlertManager(this).InsertAlert(alert);
        }

        /// <summary>
        /// this method returns true if the user exists in task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean ExistUserInTask(Task task, Int32 userId)
        {
            var companyManager = new CompanyManager(this);
            IQueryable<User> users = GetUsersByTask(task.TaskId);
            return users.Any(user => user.UserId == userId);
        }

        /// <summary>
        /// this method returns users by Task
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IQueryable<User> GetUsersByTask(Int32 taskId)
        {
            IQueryable<User> users = from user in DbContext.Users
                                     join taskUsers in DbContext.TaskUsers on user.UserId equals taskUsers.UserId
                                     where taskUsers.TaskId == taskId
                                     select user;
            return users;
        }

        /// <summary>
        /// this method checks if exists user in Task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="userId"></param>
        private Boolean TaskContainsUser(Task task, Int32 userId)
        {
            return GetTaskUser(task.TaskId, userId) != null;
        }

        /// <summary>
        /// this method returns taskUser
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private TaskUser GetTaskUser(Int32 taskId, Int32 userId)
        {
            return
                DbContext.TaskUsers.Where(taskUser => taskUser.TaskId == taskId && taskUser.UserId == userId).
                    FirstOrDefault();
        }

        public void RemoveUserOfTask(Int32 taskId, Int32 userId)
        {
            RemoveUserOfTask(GetTask(taskId), userId, null); // Assert object connected
        }

        /// <summary>
        /// this method removes user of task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="userId"></param>
        /// <param name="alert"></param>
        private void RemoveUserOfTask(Task task, Int32 userId, Alert alert)
        {
            alert = alert ?? new Alert();
            if (alert.AlertId == 0)
            {
                //
                // Send Alert to Creator Task
                //
                var manager = new MembershipManager(this);
                alert.UserId = task.CreatorUserId;
                alert.Description = alert.Description ?? String.Format(
                                                             "<b>{0}</b> concluiu sua parte na tarefa " +
                                                             CreateTaskHtmlLink(task) + "!",
                                                             manager.GetUser(userId).Profile.AbreviatedName);

                new AlertManager(this).InsertAlert(alert);
            }

            foreach (Task childTask in task.Tasks)
                RemoveUserOfTask(childTask, userId, alert);

            DbContext.TaskUsers.DeleteAllOnSubmit(
                DbContext.TaskUsers.Where(taskUser => taskUser.TaskId == task.TaskId && taskUser.UserId == userId));
            DbContext.SubmitChanges();
        }

        #endregion

        #region TaskStatus

        /// <summary>
        /// This method returns the TaskStatus
        /// </summary>
        /// <returns></returns>
        public IQueryable<TaskStatus> GetTaskStatus()
        {
            return DbContext.TaskStatus;
        }

        #endregion

        #region Task ServiceOrders

        /// <summary>
        /// This method retrieve tasks by specific ServiceOrder
        /// </summary>
        /// <returns></returns>
        public IQueryable<Task> GetTaksByServiceOrder(Int32 companyId, Int32 serviceOrderId)
        {
            IQueryable<Task> query = from task in DbContext.Tasks
                                     join serviceOrder in DbContext.ServiceOrders on task.SubjectId equals
                                         serviceOrder.ServiceOrderId
                                     where
                                         serviceOrder.CompanyId == companyId &&
                                         serviceOrder.ServiceOrderId == serviceOrderId
                                     select task;

            return query;
        }

        #endregion

        internal static string CreateTaskHtmlLink(Task task)
        {
            return
                String.Format(
                    "<a target='content' href='javascript:;' onclick='top.tb_show(\"Tarefas\",\"Task.aspx?taskId={0}\");'>\"{1}\"</a>",
                    task.TaskId.EncryptToHex(),
                    task.Name);
        }
    }
}