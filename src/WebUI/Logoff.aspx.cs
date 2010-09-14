using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using InfoControl.Security;
using InfoControl.Web.Security;
using InfoControl.Web.Security.DataEntities;


public partial class Logoff : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        FormsAuthentication.SignOut();
        string url = Request["ReturnUrl"] ?? FormsAuthentication.LoginUrl;
        Response.Redirect(url);
    }
}
