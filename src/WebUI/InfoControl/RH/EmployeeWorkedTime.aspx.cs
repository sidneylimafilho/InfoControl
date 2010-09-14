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

using Vivina.Erp.SystemFramework;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using System.Collections.Generic;
using Vivina.Erp.BusinessRules.HumanResources;
using Vivina.Erp.SystemFramework;
using InfoControl.Data;
using InfoControl.Web;
using InfoControl.Web.Auditing;
using InfoControl.Web.Security;

[PermissionRequired("Employee")]
public partial class InfoControl_RH_EmployeeWorkedTime : Vivina.Erp.SystemFramework.PageBase
{
    HumanResourcesManager humanResourcesManager;
    List<Employee> lstEmployee;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            humanResourcesManager = new HumanResourcesManager(this);
            Session["lstEmployee"] = humanResourcesManager.GetEmployeeByCompanyAsList(Company.CompanyId);
            grdEmployeeWorkedTime.DataSource = (List<Employee>)Session["lstEmployee"];

            grdEmployeeWorkedTime.DataBind();

        }
    }
    protected void grdEmployeeWorkedTime_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        grdEmployeeWorkedTime.EditIndex = e.NewSelectedIndex;
    }
    protected void grdEmployeeWorkedTime_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Appointment appointment;
        //AppointmentManager appointmentManager = new AppointmentManager(this);
        //lstEmployee = (List<Employee>)Session["lstEmployee"];
        //String journeyBegin = String.Empty, journeyEnd = String.Empty, intervalBegin = String.Empty, intervalEnd = String.Empty;
        //int rowsCount = 0;


        //for (int i = 0; i < grdEmployeeWorkedTime.Rows.Count; i++)
        //{

        //    journeyBegin = (grdEmployeeWorkedTime.Rows[i].Cells[1].Controls[1] as TextBox).Text;
        //    journeyEnd = (grdEmployeeWorkedTime.Rows[i].Cells[4].Controls[1] as TextBox).Text;
        //    intervalBegin = (grdEmployeeWorkedTime.Rows[i].Cells[2].Controls[1] as TextBox).Text;
        //    intervalEnd = (grdEmployeeWorkedTime.Rows[i].Cells[3].Controls[1] as TextBox).Text;

        //    //intervalBegin = (grdEmployeeWorkedTime.Rows[i].Cells[2].FindControl("txtIntervalBegin") as TextBox).Text;
        //    //intervalEnd = (grdEmployeeWorkedTime.Rows[i].Cells[3].FindControl("txtIntervalEnd") as TextBox).Text;

        //    ///this appointment begins at BeginTime and during until InteralBegin
        //    if (!String.IsNullOrEmpty(journeyBegin) && !String.IsNullOrEmpty(intervalBegin))
        //    {
        //        appointment = new Appointment();
        //        appointment.EmployeeId = lstEmployee[i].EmployeeId;
        //        appointment.CompanyId = Company.CompanyId;
        //        appointment.BeginTime = Convert.ToDateTime(journeyBegin);
        //        appointment.EndTime = Convert.ToDateTime(intervalBegin);
        //        appointment.TaskName = "Período de trabalho";
        //        appointmentManager.InsertAppointment(appointment);
        //    }
        //    ///this appointment begins after interval and duringuntil EndTime
        //    if (!String.IsNullOrEmpty(intervalEnd) && !String.IsNullOrEmpty(journeyEnd))
        //    {
        //        appointment = new Appointment();
        //        appointment.EmployeeId = lstEmployee[i].EmployeeId;
        //        appointment.CompanyId = Company.CompanyId;
        //        appointment.BeginTime = Convert.ToDateTime(intervalEnd);
        //        appointment.EndTime = Convert.ToDateTime(journeyEnd);
        //        appointment.TaskName = "Período de trabalho";
        //        appointmentManager.InsertAppointment(appointment);
        //    }
        //}



    }
}
