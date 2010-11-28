using System;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Auditing;
using InfoControl;



public partial class EventViewer : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {



            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });
            txtBeginDate.Text = DateTime.Now.Date.AddDays(-30).ToString();
            txtEndDate.Text = DateTime.Now.Date.AddDays(1).ToString();

            //
            // select user in combo if user logged is technical user
            //
            var humanResourcesManager = new HumanResourcesManager(this);
            Employee employee = humanResourcesManager.GetEmployeeByProfile((Int32)User.Identity.ProfileId, Company.CompanyId);

            //if (employee != null)
            //{
            //    if (cboTechnicalUser.Items.FindByValue(User.Identity.UserId.ToString()) != null)
            //        cboTechnicalUser.SelectedValue = User.Identity.UserId.ToString();
            //}


        }
    }

    protected void odsEventsViewer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        switch (cboEventType.SelectedValue)
        {
            case "1":
                odsEventsViewer.SelectMethod = "GetWarningEvents";
                odsEventsViewer.SelectCountMethod = "GetWarningEventsCount";
                break;
            case "2":
                odsEventsViewer.SelectMethod = "GetInformationEvents";
                odsEventsViewer.SelectCountMethod = "GetInformationEventsCount";
                break;
            case "3":
                odsEventsViewer.SelectMethod = "GetSugestionEvents";
                odsEventsViewer.SelectCountMethod = "GetSugestionEventsCount";
                break;
            case "4":
                odsEventsViewer.SelectMethod = "GetErrorEvents";
                odsEventsViewer.SelectCountMethod = "GetErrorEventsCount";
                break;
            case "5":
                odsEventsViewer.SelectMethod = "GetOpenEvents";
                odsEventsViewer.SelectCountMethod = "GetOpenEventsCount";
                break;
        }


        e.InputParameters["beginDate"] = Convert.ToDateTime(txtBeginDate.Text);
        e.InputParameters["endDate"] = Convert.ToDateTime(txtEndDate.Text);
        e.InputParameters["eventStatusId"] = cboStatus.SelectedValue;
        if (Page.Customization["sortExpression"] != null)
            e.InputParameters["sortExpression"] = Page.Customization["sortExpression"];

        //if (!e.ExecutingSelectCount && Page.Customization["sortExpression"] != null)
        //{
        //    e.Arguments.SortExpression = Page.Customization["sortExpression"].ToString();
        //}

        //
        //filter by technical user
        //
        //if (cboTechnicalUser.SelectedIndex > 0)
        //    e.InputParameters["technicalUserId"] = Convert.ToInt32(cboTechnicalUser.SelectedValue);
    }

    protected void grdEventViewer_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
            Server.Transfer("EventAdd.aspx");

        Page.Customization["sortExpression"] = e.SortExpression;
        Page.Customization["sortDirection"] = e.SortDirection;
    }

    protected void grdEventViewer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = "location='EventAdd.aspx?EventId=" + grdEventViewer.DataKeys[e.Row.RowIndex]["EventId"] + "';";

            //
            //paint line of red if event type equals Error and visualization mode equals All Events
            //
            if ((Convert.ToInt32(grdEventViewer.DataKeys[e.Row.RowIndex]["EventType"]) == (Int32)EventType.Error))
                e.Row.ForeColor = Color.Red;

            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    //protected void grdEventViewer_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    Context.Items["EventId"] = grdEventViewer.DataKeys[e.NewSelectedIndex]["EventId"].ToString();
    //    //if ((Int32)grdEventViewer.DataKeys[e.NewSelectedIndex]["EventType"] == (Int32)EventType.Error)
    //    //    Server.Transfer("Event.aspx");
    //    //else
    //    Server.Transfer("EventAdd.aspx");
    //}

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("EventAdd.aspx");
    }

    protected void odsEventsViewer_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Vivina.Erp.DataClasses.Event _event = new Vivina.Erp.DataClasses.Event();
        _event.EventId = (Int32)e.InputParameters["EventId"];
        _event.EventType = (Int32)e.InputParameters["EventType"];
        _event.Name = (String)e.InputParameters["Name"];
        _event.Source = (String)e.InputParameters["Source"];
        _event.Message = (String)e.InputParameters["Message"];
        _event.Path = (String)e.InputParameters["Path"];
        _event.StackTrace = (String)e.InputParameters["StackTrace"];
        _event.RefererUrl = (String)e.InputParameters["RefererUrl"];
        _event.HelpLink = (String)e.InputParameters["HelpLink"];
        _event.TargetSite = (String)e.InputParameters["TargetSite"];
        _event.CurrentDate = (DateTime)e.InputParameters["CurrentDate"];
        _event.ExceptionCode = (String)e.InputParameters["ExceptionCode"];
        _event.ApplicationId = (Int32)e.InputParameters["ApplicationId"];
        _event.UserId = (Int32)e.InputParameters["UserId"];
      //  _event.TechnicalUserId = (Int32?)e.InputParameters["TechnicalUserId"];
      //  _event.CompanyId = (Int32?)e.InputParameters["CompanyId"];
        _event.EventStatusId = (Int32?)e.InputParameters["EventStatusId"];
        _event.Version = (String)e.InputParameters["Version"];
        _event.Module = (String)e.InputParameters["Module"];
        e.InputParameters.Clear();
        e.InputParameters.Add("entity", _event);
        //e.InputParameters.Add("technicalUserId", User.Identity.UserId);
    }

    protected void odsTechnicalUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {

        //grdEventViewer.DataBind();
    }

    [WebMethod]
    public static void DeleteEvent(int eventId, int userId)
    {
        using (EventManager manager = new EventManager(null))
        {
            manager.ResolveEvent(manager.GetEvent(eventId));
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdEventViewer.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdEventViewer.DataBind();

    }
    protected void grdEventViewer_PreRender(object sender, EventArgs e)
    {

    }
}