using System;
using System.Linq;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;


public partial class InfoControl_Site_SiteMap : Vivina.Erp.SystemFramework.PageBase
{

    private SiteManager _siteManager;
    public SiteManager SiteManager
    {
        get { return _siteManager ?? (_siteManager = new SiteManager(this)); }
    }

    private WebPage _originalPage;
    public WebPage OriginalPage
    {
        get
        {
            if (_originalPage == null && Page.ViewState["PageId"] != null)
            {
                _originalPage = SiteManager.GetWebPage(Company.CompanyId,
                                                       Convert.ToInt32(Page.ViewState["PageId"]));
            }
            return _originalPage;
        }
    }

    public Boolean IsLoaded
    {
        get { return OriginalPage != null; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkIsPublished.Checked = true;

            if (!String.IsNullOrEmpty(Request["PageId"]))
            {
                Page.ViewState["PageId"] = Request["PageId"].DecryptFromHex();
                ShowPage();
            }
        }
    }

    private void ShowPage()
    {
        cboMasterPage.DataBind();
        txtName.Text = OriginalPage.Name;
        chkCanComment.Checked = OriginalPage.CanComment;
        chkIsInMenu.Checked = OriginalPage.IsInMenu;
        chkIsPublished.Checked = OriginalPage.IsPublished;
        txtDescription.Value = OriginalPage.Description;
        txtRedirectUrl.Text = OriginalPage.RedirectUrl;

        if (OriginalPage.ParentPageId.HasValue)
            cboParentPages.SelectedValue = OriginalPage.ParentPageId.ToString();
        
        if (cboMasterPage.Items.FindByText(OriginalPage.MasterPage) != null)
            cboMasterPage.SelectedValue = OriginalPage.MasterPage;

        txtTags.Text = String.Join(", ", OriginalPage.PageTags.Select(x => x.Name).ToArray());
        //txtCategories.Text = String.Join(", ", OriginalPage.PageCategories.Select(x => x.Name).ToArray());
    }

    protected void odsSiteMap_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["parentId"] = null;
        e.InputParameters["recursive"] = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var page = new WebPage();

        if (IsLoaded)
            page.CopyPropertiesFrom(OriginalPage);

        page.ParentPageId = null;
        if (!String.IsNullOrEmpty(cboParentPages.SelectedValue))
            page.ParentPageId = Convert.ToInt32(cboParentPages.SelectedValue);

        page.Name = txtName.Text;        
        page.CompanyId = Company.CompanyId;
        page.Description = txtDescription.Value.Replace("$0", "<br/>");
        page.IsInMenu = chkIsInMenu.Checked;

        page.IsPublished = chkIsPublished.Checked;
        page.CanComment = chkCanComment.Checked;
        page.MasterPage = cboMasterPage.Text;

        if (page.IsPublished)
        {
            if (!page.PublishedDate.HasValue)
                page.PublishedDate = DateTime.Now;
        }
        else
            page.PublishedDate = null;

        page.RedirectUrl = null;
        if (!String.IsNullOrEmpty(txtRedirectUrl.Text))
            page.RedirectUrl = txtRedirectUrl.Text;

        page.ModifiedDate = DateTime.Now;

        if (!page.UserId.HasValue)
            page.UserId = User.Identity.UserId;

        SiteManager.Save(page, txtTags.Text);

        if (((WebControl)sender).ID == "btnSaveAndNew")
        {
            Response.Redirect("WebPage.aspx");
            return;
        }
        //
        //Close the modal popup and redirect for WebPages.aspx
        //
        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "top.$.LightboxObject.close();", true);

        //Response.Redirect("WebPages.aspx");

    }

    protected void odsMasterPages_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["company"] = Company;
    }
}