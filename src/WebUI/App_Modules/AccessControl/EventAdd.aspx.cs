using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class AccessControl_EventInsertion : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tblName.Visible = tblMessage.Visible = true;

            if (!String.IsNullOrEmpty(Request["EventId"]))
            {
                Page.ViewState["EventId"] = Request["EventId"].DecryptFromHex();
                loadEvent(Convert.ToInt32(Page.ViewState["EventId"]));
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Company.CompanyId == HostCompany.CompanyId)
            Response.Redirect("EventViewer.aspx");
        else
            Response.Redirect("../Site/1/WishList.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //
        //verify if exists selected event type 
        //
        if (!(rbtAlert.Checked || rbtInformation.Checked || rbtSugestion.Checked || rbtError.Checked))
        {
            ShowError(Resources.Exception.UnselectedEventType);
            return;
        }

        EventManager eventManager = new EventManager(this);
        Vivina.Erp.DataClasses.Event ev = new Vivina.Erp.DataClasses.Event();
        Vivina.Erp.DataClasses.Event original_ev = new Vivina.Erp.DataClasses.Event();

        if (Page.ViewState["EventId"] != null)
        {
            original_ev = eventManager.GetEvent(Convert.ToInt32(Page.ViewState["EventId"]));
            ev.CopyPropertiesFrom(original_ev);
        }

       // ev.CompanyId = Company.CompanyId;
        ev.Name = txtName.Text;
        ev.Message = txtMessage.Value;
        ev.ApplicationId = Application.ApplicationId;
        ev.Rating = rtnPriority.CurrentRating;
        //if (!String.IsNullOrEmpty(cboTechnicalUser.SelectedValue))
        //    ev.TechnicalUserId = Convert.ToInt32(cboTechnicalUser.SelectedValue);
        if (!String.IsNullOrEmpty(cboEventStatus.SelectedValue))
            ev.EventStatusId = Convert.ToInt32(cboEventStatus.SelectedValue);

        //set event type
        if (rbtAlert.Checked)
            ev.EventType = (Int32)InfoControl.Web.Auditing.EventType.Warning;
        if (rbtInformation.Checked)
            ev.EventType = (Int32)InfoControl.Web.Auditing.EventType.Information;
        if (rbtSugestion.Checked)
            ev.EventType = (Int32)InfoControl.Web.Auditing.EventType.Sugestion;



        //verify if is update or insert
        if (Page.ViewState["EventId"] != null)
            eventManager.Update(original_ev, ev);
        else
        {
            //where insert mode set creator's ID
            ev.UserId = User.Identity.UserId;
            eventManager.Insert(ev);
        }

        if (Company.CompanyId == HostCompany.CompanyId)
            Server.Transfer("EventViewer.aspx");
        else
            Server.Transfer("../Site/1/WishList.aspx");
    }

    protected void odsTechnicalUser_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    private void loadEvent(int eventId)
    {
        EventManager eventManager = new EventManager(this);
        Event eventEntity = new Event();

        eventEntity = eventManager.GetEvent(eventId);
        
        Comments.SubjectId = eventId;
        Comments.Visible = true; 
        if (Convert.ToInt32(eventEntity.EventStatusId) == (Int32)InfoControl.Web.Auditing.EventType.Error)
        {
            tblMessage.Visible = false;
            tblName.Visible = false;
            dvException.Visible = true;
            rbtAlert.Enabled = rbtError.Enabled = rbtInformation.Enabled = rbtSugestion.Enabled = false;
        }
        else
            rbtAlert.Enabled = rbtError.Enabled = rbtInformation.Enabled = rbtSugestion.Enabled = true;

        tblProcess.Visible = tblPriority.Visible = Company.CompanyId == HostCompany.CompanyId;

        txtName.Text = eventEntity.Name;
        ltrErrorMessage.Text = eventEntity.Message;

        cboEventStatus.SelectedValue = Convert.ToString(eventEntity.EventStatusId);
        rtnPriority.CurrentRating = Convert.ToInt32(eventEntity.Rating);

  
        lnkOpenModal.NavigateUrl = "javascript:void(0);";
        

        ltrCurrentDate.Text = Convert.ToString(eventEntity.CurrentDate);
        ltrPath.Text = eventEntity.Path;
        txtMessage.Value = eventEntity.Message;
        ltrStackTrace.Text = eventEntity.StackTrace;
        ltrUserName.Text = eventEntity.User != null ? eventEntity.User.UserName : String.Empty;

        if (Convert.ToInt32(eventEntity.EventType) == (Int32)InfoControl.Web.Auditing.EventType.Warning)
            rbtAlert.Checked = true;
        if (Convert.ToInt32(eventEntity.EventType) == (Int32)InfoControl.Web.Auditing.EventType.Information)
            rbtInformation.Checked = true;
        if (Convert.ToInt32(eventEntity.EventType) == (Int32)InfoControl.Web.Auditing.EventType.Sugestion)
            rbtSugestion.Checked = true;
        if (Convert.ToInt32(eventEntity.EventType) == (Int32)InfoControl.Web.Auditing.EventType.Error)
            rbtError.Checked = true;
    }
}
