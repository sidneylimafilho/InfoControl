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
using InfoControl.Web.Security.DataEntities;
using InfoControl.Web.Security;
using InfoControl;

[PermissionRequired("Users")]
public partial class UserGeneral : Vivina.Erp.SystemFramework.PageBase
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["UserId"] != null)
                Context.Items["UserId"] = Request["UserId"].DecryptFromHex();

            if (Context.Items["UserId"] != null)
            {
                //TabPanel2.Enabled = true;
                Page.ViewState["UserId"] = Context.Items["UserId"].ToString();
                Page.ViewState["CompanyId"] = Company.CompanyId;
            }
        }
    }
}
