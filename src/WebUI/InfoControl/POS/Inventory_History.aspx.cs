using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

namespace Vivina.Erp.WebUI.POS
{
    public partial class Inventory_History : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["ProductId"]))
            {
                Session["ProductId"] = Request["ProductId"];
                Session["DepositId"] = Request["DepositId"];
            }
        }
    }
}
