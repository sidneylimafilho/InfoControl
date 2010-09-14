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
using InfoControl.Web.Configuration;

using InfoControl;
using InfoControl.Data;
using InfoControl.Web;

public partial class _Frame : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "InfoControl v" + WebConfig.AppSettings["ApplicationVersion"] + " (beta)";

        //if (Company == null || Deposit == null)
        //    Response.Redirect(WebConfig.Authentication.Forms.LoginUrl);      
    }
}
