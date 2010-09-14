using System;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;


public partial class Commons_menu : Vivina.Erp.SystemFramework.PageBase
{
    private string imagesBaseDir = "";
    private int level;

    protected void Page_Load(object sender, EventArgs e)
    {

        RecursivePopulatePanelbar(rdpMenu, rdpMenu.Items, SiteMap.RootNode);
        rdpMenu.DataBind();
    }

    private void RecursivePopulatePanelbar(RadPanelBar parent, RadPanelItemCollection items, SiteMapNode node)
    {
        level++;
        imagesBaseDir = ResolveClientUrl("~/app_themes/_global/Menu/");

        foreach (SiteMapNode child in node.ChildNodes)
        {
            if (child.IsAccessibleToUser(HttpContext.Current))
            {
                var panelItem = new RadPanelItem();

                //
                // The NavigateUrl, are using a replace of "¨" to "&", cause the .NET has a Bug that
                // not allow the SiteMap to have & to concatenate a querystring
                //
                panelItem.NavigateUrl = child.Url.Replace("¨", "&");
                panelItem.Target = "content";
                panelItem.Enabled = true;
                panelItem.Expanded = false;
                panelItem.ImagePosition = RadPanelItemImagePosition.Left;

                panelItem.CssClass = "CssClass";
                panelItem.FocusedCssClass = "FocusedCssClass";
                panelItem.ExpandedCssClass = "ExpandedCssClass";
                panelItem.DisabledCssClass = "DisabledCssClass";
                panelItem.SelectedCssClass = "SelectedCssClass";

                if (String.IsNullOrEmpty(node.Title))
                {
                    panelItem.CssClass = "HeaderCssClass";
                    panelItem.FocusedCssClass = "HeaderFocusedCssClass";
                    panelItem.ExpandedCssClass = "HeaderExpandedCssClass";
                    panelItem.DisabledCssClass = "HeaderDisabledCssClass";
                    panelItem.SelectedCssClass = "HeaderSelectedCssClass";
                }

                if (!String.IsNullOrEmpty(child["ImageCollapsed"]))
                    panelItem.ImageUrl = imagesBaseDir + child["ImageCollapsed"];
                else
                    panelItem.ImageUrl = imagesBaseDir + "blank" + (level - 1) + "x.gif";


                if (!String.IsNullOrEmpty(child["ImageExpanded"]))
                    panelItem.ExpandedImageUrl = imagesBaseDir + child["ImageExpanded"];

                if (!String.IsNullOrEmpty(child["ImageDisabled"]))
                    panelItem.DisabledImageUrl = imagesBaseDir + child["ImageDisabled"];

                if (!String.IsNullOrEmpty(child["ImageHoverCollapsed"]))
                    panelItem.HoveredImageUrl = imagesBaseDir + child["ImageHoverCollapsed"];

                panelItem.Text = child.Title;

                if (!String.IsNullOrEmpty(child["startPageUrl"]))
                    panelItem.Text += "</span></a><a class='startPage' href='" + ResolveClientUrl(child["startPageUrl"]) + "' target='content'><span>";

                items.Add(panelItem);

                RecursivePopulatePanelbar(panelItem.PanelBar, panelItem.Items, child);

                if (String.IsNullOrEmpty(child.Url) && child.ChildNodes.Count > 0 && panelItem.Items.Count == 0)
                    panelItem.Visible = false;
            }
        }
        level--;
    }

    protected void odsCompanies_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Page.User != null)
            e.InputParameters["userId"] = User.Identity.UserId;
    }

    protected void cboCompanies_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var manager = new CompanyManager(null))
            manager.ChangeCompany(Page.User.Identity.UserId, int.Parse(cboCompanies.SelectedValue));

        RefreshCompany();
        RefreshPlan();
        RefreshDeposit();
        Page.ClientScript.RegisterStartupScript(GetType(), "ResetAll", "top.ResetAll();", true);

        if (Page.User.Identity.UserId > 1)
            User.RefreshCredentials();
    }

    protected void cboCompanies_PreRender(object sender, EventArgs e)
    {
        if (cboCompanies.Items.Count == 1)
            lblAdminCompany.Visible = cboCompanies.Visible = false;
        else
        {
            lblAdminCompany.Visible = cboCompanies.Visible = true;

            if (cboCompanies.Items.FindByValue(Company.CompanyId.ToString()) != null)
                cboCompanies.Items.FindByValue(Company.CompanyId.ToString()).Selected = true;
        }
    }
}