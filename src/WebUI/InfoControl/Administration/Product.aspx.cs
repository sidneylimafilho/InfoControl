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
using InfoControl;
using InfoControl;
using InfoControl.Web.Security;


[PermissionRequired("Products")]
public partial class Company_Product : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //
        // Hack the HtmlEditor bug
        //
        ClientScript.RegisterStartupScript(typeof(Page), "hackHtmlEditor", "top.scroll(0, 0);", true);

        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["ProductId"]))
            {
                Page.ViewState["ProductId"] = Convert.ToInt32(Request["ProductId"].DecryptFromHex());
            }
        }
    }
}
