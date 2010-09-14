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

public partial class tour : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FormsAuthentication.SetAuthCookie("teste.infocontrol@vivina.com.br", false);
        if (String.IsNullOrEmpty(Request["refUrl"]))
            Response.Redirect("~/infocontrol");
        else
            Response.Redirect("~/infocontrol/" + Request["refUrl"]);

  
    }
}
