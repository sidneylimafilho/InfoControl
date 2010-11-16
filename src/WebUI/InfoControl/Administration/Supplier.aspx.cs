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
using InfoControl.Web.Security;

[PermissionRequired("Suppliers")]
public partial class Administration_Company_Supplier : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["w"] == "modal")
            {
                lblTitle.Visible = false;
                //Session["w"] = Request["w"];
            }

            if (Request["SupplierId"] != null)
                Page.ViewState["SupplierId"] = Request["SupplierId"];
        }
    }
}
