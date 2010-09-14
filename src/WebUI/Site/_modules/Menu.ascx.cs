using System;
using System.Web.UI.WebControls;
using Vivina.Erp.SystemFramework;

namespace Vivina.Erp.WebUI.Site
{
    public partial class SiteMenu : Vivina.Erp.SystemFramework.UserControlBase<PageBase>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void odsMenu_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Page.Company.CompanyId;
        }
    }
}