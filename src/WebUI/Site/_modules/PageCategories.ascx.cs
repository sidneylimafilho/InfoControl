using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.WebUI.Site;

public partial class Site_PageCategoriesControl : Vivina.Erp.SystemFramework.UserControlBase
{
    private SiteManager manager;
    public SiteManager Manager
    {
        get { return manager ?? (manager = new SiteManager(this)); }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string WebPageId
    {
        get { return Convert.ToString(ViewState["SubjectId"]); }
        set
        {
            ViewState["SubjectId"] = value;

            WebPage = Manager.GetWebPage(Page.Company.CompanyId, Convert.ToInt32(ViewState["SubjectId"]));
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public WebPage WebPage { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (WebPage == null) WebPage = (Page as SitePageBase).WebPage;
    }

    protected void list_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
    }

    protected void odsCategory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Page.Company.CompanyId;
        e.InputParameters["webpageid"] = WebPage.WebPageId;
    }
}