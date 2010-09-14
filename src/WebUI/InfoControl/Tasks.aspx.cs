using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


public partial class InfoControl_Project_Tasks : Vivina.Erp.SystemFramework.PageBase
{
    private int? _parentId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rbtHierarchy.Checked = false;
            rbtDate.Checked = false;

            if (Page.Customization["rbtlTaskView"] != null)
            {
                if (Convert.ToInt32(Page.Customization["rbtlTaskView"]) == 1)
                    rbtDate.Checked = true;
                else
                    rbtHierarchy.Checked = true;
            }

            rbtlTaskStatus.SelectedValue = "1";
            if (Page.Customization["rbtlTaskStatus"] != null)
                rbtlTaskStatus.SelectedValue = Convert.ToString(Page.Customization["rbtlTaskStatus"]);

            if ((Page.Customization["ucDateTimeInterval"] as DateTimeInterval) != null)
                ucDateTimeInterval.DateInterval = (Page.Customization["ucDateTimeInterval"] as DateTimeInterval);

            if (Page.Customization["TaskName"] != null)
                txtTask.Text = Convert.ToString(Page.Customization["TaskName"]);
        }
    }

    protected void rtvTasks_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        var task = e.Node.DataItem.GetPropertyValue("task") as Task;
        DateTime deadline = task.FinishDate ?? task.Deadline ?? DateTime.MaxValue;

        Int32 priority = 0;
        if (task.Priority.HasValue)
            priority = task.Priority.Value;

        var chkCompleteTask = e.Node.FindControl("chkCompleteTask") as HtmlInputCheckBox;
        if (chkCompleteTask != null)
        {
            chkCompleteTask.Attributes["companyid"] = Company.CompanyId.ToString();
            chkCompleteTask.Attributes["taskid"] = task.TaskId.ToString();
            chkCompleteTask.Attributes["userid"] = User.Identity.UserId.ToString();
            if (task.TaskStatusId.HasValue)
                chkCompleteTask.Checked = task.TaskStatusId.Value == (Int32)TaskStatus.Concluded;

            e.Node.CssClass = "";
            if (chkCompleteTask.Checked)
                e.Node.CssClass = "deleted";
        }

        //
        // Rating
        //
        string starCss = chkCompleteTask.Checked
                             ? "emptyRatingStar"
                             : (deadline.Subtract(DateTime.Now).Days < 3 || priority == 5)
                                   ? "savedRatingStar"
                                   : (deadline.Subtract(DateTime.Now).Days < 5 || priority == 4)
                                         ? "filledRatingStar"
                                         : "emptyRatingStar";

        string starHtml = string.Format(@"<span class='ratingStar {0}' style='float: left;'>&nbsp;</span>", starCss);

        var rating = e.Node.FindControl("rating") as HtmlGenericControl;
        if (rating != null)
        {
            for (int i = 0; i < priority; i++)
                rating.InnerHtml += starHtml;

            rating.Visible = !chkCompleteTask.Checked;
        }

        //
        // Shared
        //
        var lnkShared = e.Node.FindControl("shared") as HtmlGenericControl;
        lnkShared.Visible = false;
        if (lnkShared != null && task.TaskUsers.Count() > 1)
        {
            lnkShared.Visible = true;
            lnkShared.Attributes["title"] = "Compartilhada com " +
                task.TaskUsers
                .Where(tu => tu.UserId != User.Identity.UserId)
                .Select(x => x.User.Profile.AbreviatedName).ToArray().Join(",");
        }

        //
        // Link
        //
        var lnkTask = e.Node.FindControl("lnkTask") as HtmlAnchor;
        if (lnkTask != null)
            lnkTask.HRef = "task.aspx?taskid=" + task.TaskId.EncryptToHex();

        //
        // Date
        //
        var date = e.Node.FindControl("date") as HtmlGenericControl;
        if (date != null)
        {
            date.Attributes["style"] += "font-size:7pt; color:#999;";
            date.InnerHtml = ((deadline < DateTime.MaxValue.Date)
                                  ? deadline.ToLocalDateString() + "&nbsp;|&nbsp;"
                                  : "");
        }

        //
        // Task
        //
        var name = e.Node.FindControl("name") as HtmlGenericControl;
        if (name != null)
        {
            name.Attributes["class"] = (deadline.Subtract(DateTime.Now).Days < 3 || priority == 5)
                                           ? "essential"
                                           : (deadline.Subtract(DateTime.Now).Days < 7 || priority == 4)
                                                 ? "warning"
                                                 : (priority > 0)
                                                     ? "normal"
                                                     : "waiting";

            if (task.Tasks.Count() > 0)
            {
                //chkCompleteTask = e.Node.FindControl("chkCompleteTask") as HtmlInputCheckBox;
                //if (chkCompleteTask != null)
                //    chkCompleteTask.Visible = false;

                name.Attributes["style"] += "font-weight:bold;";
                e.Node.Expanded = true;

                rtvTasks.ExpandAllNodes();

                e.Node.ExpandMode = TreeNodeExpandMode.ServerSide;
            }
        }
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        //
        // Save Last search
        //
        Page.Customization["rbtlTaskView"] = rbtHierarchy.Checked ? 2 : 1;
        Page.Customization["rbtlTaskStatus"] = rbtlTaskStatus.SelectedValue;
        Page.Customization["ucDateTimeInterval"] = ucDateTimeInterval.DateInterval;
        Page.Customization["TaskName"] = txtTask.Text;
        rtvTasks.DataBind();

    }

    [WebMethod]
    public static void CompleteTask(Int32 companyId, Int32 taskId, Int32 userId)
    {
        using (var manager = new TaskManager(null))
        {
            Task originalTask = manager.GetTask(taskId);
            Task task = originalTask.Duplicate();
            task.TaskStatusId = TaskStatus.Concluded;
            task.CreatorUserId = userId;
            manager.SaveTask(originalTask, task, null);
        }
    }

    protected void odsTaskByUser_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        rtvTasks.DataFieldParentID = "ParentTaskId";

        e.InputParameters["userId"] = User.Identity.UserId;


        e.InputParameters["parentId"] = _parentId;

        if (rbtDate.Checked)
        {
            e.InputParameters["filterType"] = (Int32)FilterType.Date;
            rtvTasks.DataFieldParentID = "";
        }
        else
        {
            e.InputParameters["filterType"] = (Int32)FilterType.Hierarchy;
        }

        if (!String.IsNullOrEmpty(txtTask.Text))
        {
            rtvTasks.DataFieldParentID = "";
            e.InputParameters["filterType"] = (Int32)FilterType.Date;
            e.InputParameters["Name"] = txtTask.Text;
        }

        if (ucDateTimeInterval.DateInterval != null)
        {
            e.InputParameters["dtInterval"] = ucDateTimeInterval.DateInterval;
        }

        if (!String.IsNullOrEmpty(rbtlTaskStatus.SelectedValue))
        {
            e.InputParameters["status"] = Convert.ToInt32(rbtlTaskStatus.SelectedValue);
        }
    }

    protected void rtvTasks_DataBinding(object sender, EventArgs e) { }

    protected void odsTaskStatus_Selected(object sender, ObjectDataSourceStatusEventArgs e) { }

    protected void rtvTasks_NodeExpand(object sender, RadTreeNodeEventArgs e)
    {
        _parentId = Convert.ToInt32(e.Node.Value);
        rtvTasks.DataBind();

        List<RadTreeNode> list = rtvTasks.Nodes.Cast<RadTreeNode>().ToList();
        foreach (RadTreeNode node in list)
            e.Node.Nodes.Add(node);
    }
}