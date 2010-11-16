using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;

using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Inventory")]
public partial class POS_Company_Inventory : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request["ProductId"] != null)
        {
            Page.ViewState["ProductId"] = Request["ProductId"];
            Page.ViewState["DepositId"] = Request["DepositId"];
        }
    }
}
