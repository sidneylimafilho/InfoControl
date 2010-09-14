using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;

using Vivina.Erp.SystemFramework;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Service")]
public partial class InfoControl_Services_Service : Vivina.Erp.SystemFramework.PageBase
{
    private void SetControlPermission(ControlCollection controlCollection)
    {

        //new TextBox().Attributes[]
        foreach (Control chilControl in controlCollection)
        {
            if ((chilControl as WebControl) != null)
                (chilControl as WebControl).Attributes["permissionRequired"] = "Service";
            if (chilControl.HasControls())
                SetControlPermission(chilControl.Controls);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["ServiceId"]))
            {
                Page.ViewState["ServiceId"] = Convert.ToInt32(Request["ServiceId"].DecryptFromHex());
                frmServices.ChangeMode(FormViewMode.Edit); 
            }
        }
    }
    protected void frmServices_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        e.Values["CompanyId"] = Company.CompanyId;
        e.Values["Price"] = Convert.ToDecimal(e.Values["Price"].ToString().RemoveMask());
        e.Values["TimeInMinutes"] = Convert.ToInt32(e.Values["TimeInMinutes"].ToString().RemoveMask());

        if (!String.IsNullOrEmpty(e.Values["ISS"].ToString()))
            e.Values["ISS"] = Convert.ToDecimal(e.Values["ISS"].ToString().RemoveMask());
        else
            e.Values["ISS"] = null;
    }
    protected void odsServices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        if (Page.ViewState["ServiceId"] != null)
        {

            e.InputParameters["ServiceId"] = Page.ViewState["ServiceId"];
        }
    }
    protected void odsServices_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        //Data sent to the method update
        Service service;
        service = (Service)e.InputParameters["entity"];
    }
    protected void odsServices_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Server.Transfer("services.aspx");
    }
    protected void odsServices_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Service service;
        service = (Service)e.InputParameters["entity"];
    }
    protected void frmServices_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        e.NewValues["CompanyId"] = Company.CompanyId;
        //Formatting the fields 
        e.NewValues["Price"] = Convert.ToDecimal(e.NewValues["Price"].ToString().RemoveMask());
        e.NewValues["TimeInMinutes"] = Convert.ToInt32(e.NewValues["TimeInMinutes"].ToString().RemoveMask());

        if (!String.IsNullOrEmpty(e.NewValues["ISS"].ToString()))
            e.NewValues["ISS"] = Convert.ToDecimal(e.NewValues["ISS"].ToString().RemoveMask());
        else
            e.NewValues["ISS"] = null;
    }
    protected void odsServices_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        Server.Transfer("services.aspx");
    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {

    }
    protected void frmServices_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
            Server.Transfer("services.aspx");
    }

    //protected override void OnPreRender(EventArgs e)
    //{

    //    base.OnPreRender(e);



    //protected override void OnInit(EventArgs e)
    //{
    //    SetControlPermission(Page.Controls[0].Controls);
    //    base.OnInit(e);
    //    SetControlPermission(Page.Controls[0].Controls);
    //}
    protected override void OnPreRenderComplete(EventArgs e)
    {

        base.OnPreRenderComplete(e);
        Trace.Warn(DateTime.Now.ToString());
        SetControlPermission(Page.Controls[0].Controls);
        Trace.Warn(DateTime.Now.ToString());

    }
    //protected override void OnPreLoad(EventArgs e)
    //{
    //    SetControlPermission(Page.Controls[0].Controls);
    //    base.OnPreLoad(e);
    //    SetControlPermission(Page.Controls[0].Controls);

    //}

    //protected override void OnLoad(EventArgs e)
    //{
    //    SetControlPermission(Page.Controls[0].Controls);
    //    base.OnLoad(e);
    //    SetControlPermission(Page.Controls[0].Controls);
    //}


}
