using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using InfoControl;
using Vivina.Erp;
using InfoControl.Data;
using InfoControl.Web.ScheduledTasks;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;


public partial class InfoControl_Host_ScheduleTask : Vivina.Erp.SystemFramework.PageBase
{
    InfoControl.Web.ScheduledTasks.ScheduledTask scheduledTask;
    SchedulerManager schedulerManager;
    int scheduledTaskId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["ScheduledTaskId"]))
            scheduledTaskId = Convert.ToInt32(Request["ScheduledTaskId"].DecryptFromHex());

        //txtLastRunStatus.Enabled = false;

        if (!IsPostBack)
        {
            if (scheduledTaskId > 0)
            {
                schedulerManager = new SchedulerManager(this);
                scheduledTask = schedulerManager.GetScheduleTask(scheduledTaskId);

                txtName.Text = scheduledTask.Name;
                ucPeriodDate.DateTime = scheduledTask.StartTime;
                ucCurrFieldTxtPeriod.CurrencyValue = scheduledTask.Period;
                chkEnabled.Checked = scheduledTask.Enabled;
                txtTypeFullName.Text = scheduledTask.TypeFullName;
                txtLastRunStatus.Value = scheduledTask.LastRunStatus;
            }
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var original_scheduledTask = new InfoControl.Web.ScheduledTasks.ScheduledTask();

        schedulerManager = new SchedulerManager(this);
        scheduledTask = new InfoControl.Web.ScheduledTasks.ScheduledTask();

        if (scheduledTaskId > 0)
        {
            original_scheduledTask = schedulerManager.GetScheduleTask(scheduledTaskId);
            scheduledTask = original_scheduledTask.Duplicate();
        }

        scheduledTask.Name = txtName.Text;
        scheduledTask.Period = ucCurrFieldTxtPeriod.IntValue;
        scheduledTask.Enabled = chkEnabled.Checked;
        scheduledTask.TypeFullName = txtTypeFullName.Text;

        if (ucPeriodDate.DateTime != null)
            scheduledTask.StartTime = Convert.ToDateTime(ucPeriodDate.DateTime);
        
        schedulerManager.SaveScheduleTask(scheduledTask);

        Server.Transfer("ScheduleTasks.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("ScheduleTasks.aspx");
    }

    protected void odsScheduledTask_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["ScheduleTaskId"] = scheduledTaskId;
    }
}
