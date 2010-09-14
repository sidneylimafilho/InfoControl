using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using System.Web.Services;
using System.Collections.Generic;
using InfoControl.Web.Security;
using Vivina.Erp.WebUI.InfoControl.CRM;

[PermissionRequired("CustomerFollowups")]
public partial class InfoControl_CRM_CustomerFollowup : Vivina.Erp.SystemFramework.PageBase
{
    //Manager
    CustomerManager customerManager;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            companyId.Value = Convert.ToString(Company.CompanyId);
            userId.Value = Convert.ToString(User.Identity.UserId);

            if (!String.IsNullOrEmpty(Request["CustomerFollowupId"]))
            {
                Page.ViewState["CustomerFollowupId"] = Convert.ToInt32(Request["CustomerFollowupId"].DecryptFromHex());
                ShowCustomerFollowup();

                customerFollowUpId.Value = Convert.ToString(Page.ViewState["CustomerFollowupId"]);
            }
        }
    }


    #region Functions

    /// <summary>
    /// this method show a customerFollowup
    /// </summary>
    private void ShowCustomerFollowup()
    {

        customerManager = new CustomerManager(this);
        var customerFollowup = customerManager.GetCustomerFollowup(Company.CompanyId, Convert.ToInt32(Page.ViewState["CustomerFollowupId"]));

        txtContactId.Value = customerFollowup.ContactId.ToString();

        var taskManager = new TaskManager(this);
        var task = taskManager.GetTask(customerFollowup.CustomerFollowupId, "CustomerFollowUp.aspx");

        if (task != null)
        {
            ucNextMeetingDate.DateTime = task.StartDate;
            txtAppoitmentSubject.Text = task.Name;
        }

        selContact.ShowContact(customerFollowup.Contact);

        if (customerFollowup.CustomerFollowupActionId.HasValue)
            cboCustomerFollowupAction.SelectedValue = customerFollowup.CustomerFollowupActionId.ToString();

        txtEntryDate.Text = "Data de Entrada:<br/><b>" + customerFollowup.EntryDate.ToString("dd/MM/yyyy") + "</b>";
        txtDescription.Text = customerFollowup.Description;

        Page.ViewState["CustomerId"] = customerFollowup.CustomerId;
        Page.ViewState["ContactId"] = customerFollowup.ContactId;

    }

    /// <summary>
    /// This method verify if can convert string to datetime
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private bool ValidateDate(string date)
    {
        DateTime outDate;
        return DateTime.TryParse(date, out outDate);
    }
    #endregion

    protected void selContact_SelectedContact(object sender, SelectedContactEventArgs e)
    {
        if (e.Contact != null)
            txtContactId.Value = Convert.ToString(e.Contact.ContactId);
    }

    protected void odsCustomerFollowupAction_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    [WebMethod]
    public static bool SaveCustomerFollowUp(string[] customerFollowUp)
    {
        using (var customerManager = new CustomerManager(null))
        {
            var cf = new CustomerFollowup();
            var originalCustomerFollowUp = new CustomerFollowup();

            if (!String.IsNullOrEmpty(customerFollowUp[8]))
            {
                originalCustomerFollowUp = customerManager.GetCustomerFollowup(Convert.ToInt32(customerFollowUp[1]), Convert.ToInt32(customerFollowUp[8]));
                cf.CopyPropertiesFrom(originalCustomerFollowUp);
            }

            cf.ContactId = Convert.ToInt32(customerFollowUp[0]);
            cf.CompanyId = Convert.ToInt32(customerFollowUp[1]);
            cf.UserId = Convert.ToInt32(customerFollowUp[2]);

            cf.Description = customerFollowUp[3].ToString();

            if (!String.IsNullOrEmpty(customerFollowUp[4]))
                cf.CustomerFollowupActionId = Convert.ToInt32(customerFollowUp[4]);

            if (!String.IsNullOrEmpty(customerFollowUp[8]))
            {
                if (!IsAppointment(customerFollowUp))
                    customerManager.UpdateCustomerFollowup(originalCustomerFollowUp, cf, null, null, null);
                else
                    customerManager.UpdateCustomerFollowup(originalCustomerFollowUp, cf, Convert.ToInt32(customerFollowUp[9]), Convert.ToDateTime(customerFollowUp[6]), customerFollowUp[10]);
            }
            else
                if (!IsAppointment(customerFollowUp))
                    customerManager.InsertCustomerFollowup(cf, null, null, null);
                else
                    customerManager.InsertCustomerFollowup(cf, Convert.ToInt32(customerFollowUp[9]), Convert.ToDateTime(customerFollowUp[6]), customerFollowUp[10]);

            if (cf.CustomerFollowupId != 0)
                return true;
            else
                return false;
        }
    }

    private static bool IsAppointment(string[] customerFollowUp)
    {
        if (String.IsNullOrEmpty(customerFollowUp[9]) || String.IsNullOrEmpty(customerFollowUp[6]) || String.IsNullOrEmpty(customerFollowUp[10]))
            return false;

        return true;
    }
}
