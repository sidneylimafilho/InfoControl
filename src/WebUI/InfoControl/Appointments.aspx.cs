using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


using Exception = Resources.Exception;

public partial class Company_Administration_Agenda : Vivina.Erp.SystemFramework.PageBase
{
    private Employee employee;
    private HumanResourcesManager manager;
    private int? serviceOrderId;

    protected void Page_Load(object sender, EventArgs e)
    {
        manager = new HumanResourcesManager(this);
        employee = new Employee();

        if (!String.IsNullOrEmpty(Request["ServiceOrderId"]))
            serviceOrderId = Convert.ToInt32(Request["ServiceOrderId"]);

        if (!IsPostBack)
        {
            ResetAgenda();
            selDate.DateTime = radAppointment.SelectedDate = DateTime.Today;
        }

        radAppointment.GroupBy = "";
        radAppointment.ReadOnly = false;
        radAppointment.Width = Unit.Percentage(100);
        radAppointment.SelectedDate = DateTime.Today;
    }

    protected void radAppointment_AppointmentDelete(object sender, SchedulerCancelEventArgs e)
    {
        var taskManager = new TaskManager(this);
        taskManager.DeleteTask(Convert.ToInt32(e.Appointment.ID));
    }

    protected void odsEmployees_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void radAppointment_FormCreated(object sender, SchedulerFormCreatedEventArgs e)
    {
        if (e.Container.Mode == SchedulerFormMode.AdvancedInsert)
        {
            //Redirecionar para a tela de tarefa já com data e horário setados e mandando o 
            //id de ordem de serviço


            string url = "Task.aspx?StartDate=" + e.Appointment.Start + "&EndDate=" + e.Appointment.End + "&app=true";

            if (serviceOrderId.HasValue)
                url = url + "&ServiceOrderId=" + Request["ServiceOrderId"];

            Response.Redirect(url);
        }

        if (e.Container.Mode == SchedulerFormMode.AdvancedEdit)
        {
            var taskManager = new TaskManager(this);
            string redirect = "Task.aspx?TaskId=" + taskManager.GetTask((int)e.Appointment.ID).TaskId;

            Response.Redirect(redirect);
        }
    }

    protected void btnSelectDate_Click(object sender, EventArgs e)
    {
        radAppointment.SelectedDate = selDate.DateTime.Value;

        if (txtEmployee.Text.ToUpper() == "TODOS")
        {
            txtEmployee.Text = "Todos";
            radAppointment.GroupBy = "UserName";
        }
        else
        {
            string[] cpf = txtEmployee.Text.Split(new[] { " | " }, StringSplitOptions.None);

            if (cpf.Count() == 2)
            {
                employee = manager.RetrieveEmployeeByCpf(Company.CompanyId, cpf[0]);

                if (employee != null && employee.Profile.CPF != "")
                {
                    Page.ViewState["userId"] = manager.GetUserByEmployee(employee.EmployeeId).UserId;
                    Page.ViewState["EmployeeId"] = employee.EmployeeId;
                }
                else
                    EmployeeNotFound();
            }
            else
                EmployeeNotFound();
        }
    }

    protected void odsAppointment_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["userId"] = Page.ViewState["userId"];
        e.InputParameters["dtInterval"] = new DateTimeInterval(radAppointment.VisibleRangeStart, radAppointment.VisibleRangeEnd);
        e.InputParameters["companyId"] = Company.CompanyId;

        if (!String.IsNullOrEmpty(cboCompetency.Text))
            e.InputParameters["competency"] = cboCompetency.Text;

        if (serviceOrderId.HasValue)
        {
            e.InputParameters["subjectId"] = serviceOrderId;
            e.InputParameters["pageName"] = "serviceorder.aspx";
        }
    }

    protected void odsCompetencies_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void radAppointment_AppointmentUpdate(object sender, AppointmentUpdateEventArgs e)
    {
        var taskManager = new TaskManager(this);
        Task task = taskManager.GetTask((int)e.Appointment.ID);

        task.StartDate = e.ModifiedAppointment.Start;
        task.FinishDate = e.ModifiedAppointment.End;

        taskManager.SaveTask(task, task, null);
    }

    protected void odsEmployeesList_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (radAppointment.ReadOnly)
            radAppointment.Width = Unit.Pixel((e.ReturnValue as DataTable).Rows.Count * 150);
    }

    protected void SelCustomer_SelectedCustomer(object sender, SelectedCustomerEventArgs e)
    {
        Page.ViewState["CustomerId"] = e.Customer.CustomerId;
    }

    #region Functions

    private void EmployeeNotFound()
    {
        ShowError(Exception.UnselectedEmployee);
        ResetAgenda();
    }

    private void DateNotFound()
    {
        //ShowError(Resources.Exception.InvalidDate);
    }

    private void ResetAgenda()
    {
        employee = manager.GetEmployeeByUser(User.Identity, Company.CompanyId);
        if (employee != null)
        {
            Page.ViewState["EmployeeId"] = employee.EmployeeId;
            Page.ViewState["CPF"] = employee.Profile.CPF;
            txtEmployee.Text = employee.Profile.CPF + " | " + employee.Profile.Name;
        }
    }

    #endregion

    #region appointmentList

    protected void odsAppointmentList_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        if (Page.ViewState["EmployeeIdList"] != null)
            e.InputParameters["employeeID"] = Convert.ToInt32(Page.ViewState["EmployeeIdList"]);
        else
            e.InputParameters["employeeID"] = null;

    }

    #endregion

    protected void odsTasksServiceOrder_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["serviceOrderId"] = serviceOrderId;
    }
}