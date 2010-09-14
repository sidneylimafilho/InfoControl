using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Security;


[PermissionRequired("ChangePassword")] 
public partial class ChangePassword : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e) 
    {
    }

    protected void ContinuePushButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InfoControl/StartPage.aspx");
    }
    protected void CancelPushButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InfoControl/StartPage.aspx");
    }


}