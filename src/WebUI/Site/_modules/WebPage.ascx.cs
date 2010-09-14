using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using InfoControl;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.WebUI.Site;

public partial class Site_WebPage : Vivina.Erp.SystemFramework.UserControlBase
{
    private SiteManager manager;
    public string queryString;

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

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string Type { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string Tag { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string Category { get; set; }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public int MaxCount { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        Type = (Type ?? "view").ToLower();

        if (MaxCount == 0) MaxCount = Int16.MaxValue;

        if (WebPage == null) WebPage = (Page as SitePageBase).WebPage;

        if (Visible)
        {
            List<WebPage> list = GetChildPages(WebPage);

            if (Type == "archive")
            {
                pageListArchived.DataSource = from arc in list
                                              group arc by
                                                  new DateTime(arc.PublishedDate.Value.Year,
                                                               arc.PublishedDate.Value.Month, 1)
                                                  into g
                                                  select new
                                                             {
                                                                 Date = g.Key,
                                                                 Count = g.Count()
                                                             };
            }
            else if (Type == "blog")
            {
                postList.DataSource = list.Take(MaxCount);
            }
            else if (Type == "list")
            {
                pageListLinks.DataSource = list;
            }
            else if (Type == "post")
                postList.DataSource = (new[] { WebPage });
            else
                pageView.InnerHtml = WebPage.Description;
        }

        pageView.Visible = !String.IsNullOrEmpty(pageView.InnerHtml);
        postList.DataBind();
        pageListLinks.DataBind();
        pageListArchived.DataBind();
    }

    public string GetBreadcrumbs(WebPage page)
    {
        WebPage parent = page;
        string breacrumbsHtml = "";
        int i = 0;
        while ((parent = parent.WebPage1) != null)
        {
            breacrumbsHtml = "<a href='" + Page.ResolveUrl(Util.GenerateWebPageUrl(parent)) + "'>" + parent.Name +
                             "</a> &raquo; " + breacrumbsHtml;
            i++;
        }

        return i <= 1 ? "" : breacrumbsHtml;
    }

    private List<WebPage> GetChildPages(WebPage webPage)
    {
        IEnumerable<WebPage> childPages = webPage.WebPages;
        if (!childPages.Any())
            if (webPage.WebPage1 != null)
                childPages = webPage.WebPage1.WebPages;

        childPages = from p in childPages
                     where p.IsPublished
                     orderby p.WebPageId descending
                     select p;

        if (!String.IsNullOrEmpty(Tag))
        {
            Type = "blog";
            childPages = childPages.Where(wp => wp.PageTags.Any(tag => tag.Name == Tag));
        }

        if (!String.IsNullOrEmpty(Category))
        {
            Type = "blog";
            childPages = childPages.Where(wp => wp.PageCategories.Any(cat => cat.Name == Category));
        }

        // sibling 
        //if (webPage.WebPage1 != null)
        //    childPages.Union(webPage.WebPage1.WebPages.Where(x => x.WebPageId != webPage.WebPageId));

        return childPages.Take(MaxCount).ToList();
    }
}