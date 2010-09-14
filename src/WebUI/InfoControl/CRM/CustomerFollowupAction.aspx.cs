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
using Vivina.Erp.DataClasses;

using InfoControl;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("CustomerFollowupActions")]
public partial class InfoControl_CRM_CustomerFollowupAction : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["CustomerFollowupActionId"] != null)
            {
                Page.ViewState["CustomerFollowupActionId"] = Request["CustomerFollowupActionId"].DecryptFromHex();
                frmFollowupAction.ChangeMode(FormViewMode.Edit);
            }
        }
    }
    protected void frmFollowupAction_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        e.Values["CompanyId"] = Company.CompanyId;

        if (String.IsNullOrEmpty(e.Values["Probability"].ToString()))
            e.Values["Probability"] = Convert.ToDecimal("0");
        else
            e.Values["Probability"] = Convert.ToDecimal(e.Values["Probability"]);
    }
    protected void odsCustomerFollowupAction_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Page.ViewState["CustomerFollowupActionId"] != null)
            e.InputParameters["CustomerFollowupActionId"] = Page.ViewState["CustomerFollowupActionId"];
    }
    protected void frmFollowupAction_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        e.NewValues["CompanyId"] = Company.CompanyId;

        if (String.IsNullOrEmpty(e.NewValues["Probability"].ToString()))
            e.NewValues["Probability"] = Convert.ToDecimal("0");
        else
            e.NewValues["Probability"] = Convert.ToDecimal(e.NewValues["Probability"]);
    }
    protected void odsCustomerFollowupAction_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        //Data sent to the method update
        CustomerFollowupAction customerFollowupAction;
        customerFollowupAction = (CustomerFollowupAction)e.InputParameters["entity"];
    }
    protected void frmFollowupAction_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        Server.Transfer("CustomerFollowupActions.aspx");
    }
    protected void frmFollowupAction_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        Server.Transfer("CustomerFollowupActions.aspx");
    }
    protected void frmFollowupAction_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
            Server.Transfer("CustomerFollowupActions.aspx");
    }
}
