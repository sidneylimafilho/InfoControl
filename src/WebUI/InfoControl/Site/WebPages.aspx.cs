using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Services;
using Telerik.Web.UI;
using InfoControl;
using InfoControl;
using InfoControl.Web;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;


public partial class InfoControl_Site_SiteMaps : Vivina.Erp.SystemFramework.PageBase
{
    private int? parentId;
    private SiteManager siteManager;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            configWebsite.Visible = String.IsNullOrEmpty(Company.LegalEntityProfile.Website);

            txtWebSite.Text = Company.LegalEntityProfile.Website;
            fieldWebSite.Visible = !configWebsite.Visible;
        }
    }

    protected void odsSiteMaps_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["parentId"] = parentId;
    }

    protected void rtvSitePages_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        var page = e.Node.DataItem as WebPage;

        //e.Node.FindControl<HtmlAnchor>("lnkSiteMaps").HRef = "WebPage.aspx?PageId=" + page.WebPageId.EncryptToHex();

        var lnkSiteMaps = e.Node.FindControl<HtmlAnchor>("lnkSiteMaps");
        lnkSiteMaps.Attributes["onclick"] = "top.$.lightbox('Site/WebPage.aspx?PageId=" + page.WebPageId.EncryptToHex() + "&lightbox[iframe]=true');";
        lnkSiteMaps.Style.Add(HtmlTextWriterStyle.Color, page.IsPublished ? "default" : "grey");

        var lnkExternal = e.Node.FindControl<HtmlAnchor>("lnkExternal");
        lnkExternal.Attributes["class"] += page.IsPublished ? " webLink" : " draft";
        lnkExternal.Title = page.IsPublished ? "Publicado" : "Rascunho";

        if (page.WebPages.Count() > 0)
            e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        var manager = new ProfileManager(this);
        Company.LegalEntityProfile.Website = txtWebSite.Text;
        manager.SaveLegalEntityProfile(Company.LegalEntityProfile);
        configWebsite.Visible = false;
    }

    protected void rtvSiteMaps_NodeExpand(object sender, RadTreeNodeEventArgs e)
    {
        parentId = Convert.ToInt32(e.Node.Value);
        rtvSiteMaps.DataBind();

        List<RadTreeNode> list = rtvSiteMaps.Nodes.Cast<RadTreeNode>().ToList();
        foreach (RadTreeNode node in list)
            e.Node.Nodes.Add(node);
    }

    [WebMethod]
    public static void DeletePage(int companyId, int pageId)
    {
        using (var siteManager = new SiteManager(null))
            siteManager.Delete(siteManager.GetWebPage(companyId, pageId));
    }

    #region Nested type: FilterType

    private enum FilterType
    {
        Date = 0,
        Hierarchy
    }

    #endregion

    #region Nested type: Status

    private enum Status
    {
        Open = 0,
        Closed,
        All
    }

    #endregion
}