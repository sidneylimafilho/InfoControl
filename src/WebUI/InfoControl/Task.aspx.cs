using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;



public partial class InfoControl_Task : Vivina.Erp.SystemFramework.PageBase
{
    private TaskManager _taskManager;
    private Task _originalTask;

    #region Properties

    public bool IsLoaded
    {
        get { return Page.ViewState["TaskId"] != null; }
    }

    public Task OriginalTask
    {
        get
        {
            return _originalTask
                   ?? (_originalTask = _taskManager.GetTask(Convert.ToInt32(Page.ViewState["TaskId"]))
                                       ?? new Task
                                          {
                                              CreatorUserId = User.Identity.UserId
                                          });
        }
    }

    public bool CanChange
    {
        get { return (!OriginalTask.CreatorUserId.HasValue || OriginalTask.CreatorUserId == User.Identity.UserId); }
    }

    public List<User> Users
    {
        get
        {
            if (Session["lstUser"] == null)
                Session["lstUser"] = new List<User>();
            return (List<User>)Session["lstUser"];
        }

        set { Session["lstUser"] = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        txtName.Focus();
        cboTaskStatus.DataBind();

        _taskManager = new TaskManager(this);
        var companyManager = new CompanyManager(this);

        if (!IsPostBack)
        {
            ucEndDate.DateTime = ucBeginDate.DateTime = DateTime.Today;

            if (!String.IsNullOrEmpty(Request["ServiceOrderId"]))
                Page.ViewState["ServiceOrderId"] = Request["ServiceOrderId"].DecryptFromHex();

            // In case of task has been generated from appointment
            if (!String.IsNullOrEmpty(Request["StartDate"]) && !String.IsNullOrEmpty(Request["EndDate"]))
            {
                ucBeginDate.DateTime = Convert.ToDateTime(Request["StartDate"]);
                ucEndDate.DateTime = Convert.ToDateTime(Request["EndDate"]);
            }

            Users = null;
            if (!String.IsNullOrEmpty(Request["TaskId"]))
            {
                Page.ViewState["TaskId"] = Convert.ToInt32(Request["TaskId"]);
                ShowTask();
            }
            else
            {
                Users.Add(companyManager.GetUser(Company.CompanyId, User.Identity.UserId));
                BindListUser();
            }
        }

        ucComments.Visible = (Request["TaskId"] != null);
    }

    protected void odsTask_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["userId"] = User.Identity.UserId;
        e.InputParameters["sortExpression"] = Page.ViewState["sortExpression"];
        e.InputParameters["status"] = TaskStatus.Proposed;
    }

    protected void odsSelect_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) { }

#warning Implementar o Save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //
        // Replaced user control DateTimeInterval for DateTime using the new feature hour
        //
        if (ucEndDate.DateTime != null)
            if (ucBeginDate.DateTime.Value > ucEndDate.DateTime.Value)
            {
                ShowError(Resources.Exception.StartTimeIsBiggerThanEndTime);
                return;
            }

        _taskManager = new TaskManager(this);
        Task task = OriginalTask.Duplicate();
        task.Name = txtName.Text;

        if (Page.ViewState["ServiceOrderId"] != null)
        {
            task.SubjectId = Convert.ToInt32(Page.ViewState["ServiceOrderId"]);
            task.PageName = "serviceorder.aspx";
            ServiceOrder os = new ServicesManager(this).GetServiceOrder(task.SubjectId.Value);
            task.Name = "OS" + os.ServiceOrderNumber + " - " + task.Name;
        }

        task.TaskStatusId = Convert.ToInt32(cboTaskStatus.SelectedValue);
        task.Priority = Convert.ToInt32(rtnRanking.CurrentRating);
        task.Cost = ucCurrFieldCost.CurrencyValue;
        task.Deadline = ucDeadLineDate.DateTime;
        task.CreatorUserId = User.Identity.UserId;

        if (!String.IsNullOrEmpty(cboAlertMinutesBefore.SelectedValue))
            task.AlertMinutesBefore = Convert.ToInt32(cboAlertMinutesBefore.SelectedValue);

        if (CanChange)
            task.Description = txtDescription.Value.Replace("$0", "<br/>");

        if (!String.IsNullOrEmpty(cboParentTasks.SelectedValue))
            task.ParentTaskId = Convert.ToInt32(cboParentTasks.SelectedValue);

        task.FinishDate = ucEndDate.DateTime;

        if (ucBeginDate.DateTime > DateTime.MinValue.Sql2005MinValue())
            task.StartDate = ucBeginDate.DateTime;


        var alertManager = new AlertManager(this);


        if (Page.ViewState["TaskId"] != null)
        {
            _taskManager.SaveTask(OriginalTask, task, Users);

            if (alertManager.GetAlerts(task.TaskId, "task.aspx") != null)
                alertManager.DeleteAlerts(task.TaskId, "task.aspx");

        }
        else
            _taskManager.SaveTask(task, task, Users);


        if (!String.IsNullOrEmpty(Request["app"]))
            CreateAlerts(task);


        if (((WebControl)sender).ID == "btnSave")
        {
            if (task.PageName == "serviceorder.aspx")
                Response.Redirect("Appointments.aspx?ServiceOrderId=" + Request["ServiceOrderId"]);
            else if (!String.IsNullOrEmpty(Request["app"]))
                Response.Redirect("Appointments.aspx");
            else
                Response.Redirect("Tasks.aspx");
        }
        else
        {
            var appointment = Request["app"];
            Response.Redirect("Task.aspx?app=" + appointment);
        }

    }

    #region Functions

    private void ShowTask()
    {
        ucComments.SubjectId = OriginalTask.TaskId;
        txtName.Text = OriginalTask.Name;

        ucDeadLineDate.DateTime = OriginalTask.Deadline;
        ucCurrFieldCost.CurrencyValue = OriginalTask.Cost;
        cboAlertMinutesBefore.SelectedValue = Convert.ToString(OriginalTask.AlertMinutesBefore);

        if (OriginalTask.TaskStatusId.HasValue)
            cboTaskStatus.SelectedValue = OriginalTask.TaskStatusId.ToString();

        txtDescription.Value = OriginalTask.Description;

        if (!CanChange)
        {
            txtName.Enabled = false;

            //txtDescription.Visible = false;
            txtDescription.Value = "<br/><p>" + txtDescription.Value + "</p><br/>";
            txtDescription.Value += "<p class='comment-footer'><span class='cTxt11b'>" + OriginalTask.User.Profile.Name + "</span></p><br/>";
            //txtDescription.AutoResizeHeight = true;
            //txtDescription.Height = Unit.Percentage(1);

            cboParentTasks.Enabled = false;
            rtnRanking.Enabled = false;
            ucBeginDate.Enabled = ucEndDate.Enabled = false;
            cboAlertMinutesBefore.Enabled = false;
            ucCurrFieldCost.Enabled = false;
            ucDeadLineDate.Enabled = false;
        }

        //
        // Replaced user control DateTimeInterval for DateTime using the new feature hour
        //
        ucBeginDate.DateTime = OriginalTask.StartDate;
        ucEndDate.DateTime = OriginalTask.FinishDate;

        cboParentTasks.DataBind();
        if (OriginalTask.ParentTaskId.HasValue)
            cboParentTasks.SelectedValue = OriginalTask.ParentTaskId.ToString();

        if (OriginalTask.Priority.HasValue)
            rtnRanking.CurrentRating = Convert.ToInt32(OriginalTask.Priority);

        foreach (TaskUser taskUser in OriginalTask.TaskUsers)
            AddUser(taskUser.User);

        BindListUser();
    }

    private void AddUser(User user)
    {
        if (!Users.Any(usr => usr.UserId == user.UserId))
            Users.Add(user);
    }

    private void ClearListUser()
    {
        Users = new List<User>();
    }

    private void BindListUser()
    {
        var companyManager = new TaskManager(this);

        if (IsLoaded)
            Users = companyManager.GetUsersByTask(OriginalTask.TaskId).ToList();

        lstUsers.DataSource = Users;
        lstUsers.DataBind();
    }

    private void RemoveUser(Int32 userId)
    {
        if (IsLoaded)
            _taskManager.RemoveUserOfTask(OriginalTask.TaskId, userId);

        Users.Remove(Users.Where(usr => usr.UserId == userId).FirstOrDefault());
        BindListUser();
    }

    private void ClearFields()
    {
        txtName.Text = String.Empty;
        txtDescription.Value = String.Empty;
        cboParentTasks.SelectedValue = string.Empty;
        ClearListUser();
        Page.ViewState["TaskId"] = null;
    }

    #endregion


    /// <summary>
    /// This method creates two alerts to specified appointment
    /// </summary>
    private void CreateAlerts(Task task)
    {
        var alertManager = new AlertManager(this);

        var alertBeforeOfAppointment = new Alert
        {
            PageName = "task.aspx",
            SubjectId = task.TaskId,
            ShowDate = task.StartDate.Value.AddDays(-1),
            UserId = User.Identity.UserId,
            Description = "Compromisso '" + task.Name + "' será amanhã dia " + task.StartDate + " !"
        };

        var alertInDayOfAppointment = new Alert
        {
            PageName = "task.aspx",
            SubjectId = task.TaskId,
            ShowDate = task.StartDate.Value,
            UserId = User.Identity.UserId,
            Description = "Compromisso '" + task.Name + "' é hoje dia " + task.StartDate + " !"
        };

        alertManager.InsertAlert(alertBeforeOfAppointment);
        alertManager.InsertAlert(alertInDayOfAppointment);
    }

    protected void selUser_SelectedUser(object sender, SelectedUserEventArgs e)
    {
        if (e.User == null)
            return;

        if (!IsLoaded)
            AddUser(e.User);
        else
            if (!OriginalTask.TaskUsers.Any(tu => tu.UserId == e.User.UserId))
                _taskManager.AddUserInTask(OriginalTask.TaskId, e.User.UserId);

        BindListUser();
    }

    protected void lstUsers_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        RemoveUser(Convert.ToInt32(e.CommandArgument));
    }
}