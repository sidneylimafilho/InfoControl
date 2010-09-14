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
using System.Xml.Linq;

namespace Vivina.Erp.WebUI.Accounting
{
    public partial class XRay : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void odsTeste_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["CompanyId"] = Company.CompanyId;
        }
    }
}
