using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vivina.Erp.SystemFramework;


public partial class InfoControl_Default : MasterPageBase
{
    public Queue<SiteMapNode> UrlRefererQueue
    {
        get
        {
            if (Session["UrlReferer"] == null)
                Session["UrlReferer"] = new Queue<SiteMapNode>(5);
            return Session["UrlReferer"] as Queue<SiteMapNode>;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        lblTitle.InnerHtml = Page.Title;
        imgPrint.Visible = lblTitle.Visible = (!String.IsNullOrEmpty(Page.Title) && Page.Title != "Untitled Page");

        SetHelpText();
        CreateBreadcrumbs();
        ConfigureHeader();



    }

    private void CreateBreadcrumbs()
    {
        //
        // BreadCrumbs
        //
        if (!IsPostBack && !String.IsNullOrEmpty(Page.Title))
        {
            if (SiteMap.CurrentNode != null)
            {
                if (!String.IsNullOrEmpty(SiteMap.CurrentNode.Title))
                    if (!UrlRefererQueue.Contains(SiteMap.CurrentNode))
                        UrlRefererQueue.Enqueue(SiteMap.CurrentNode);

                if (UrlRefererQueue.Count > 5)
                    UrlRefererQueue.Dequeue();

                string[] array = (from url in UrlRefererQueue
                                  where url.Title != SiteMap.CurrentNode.Title
                                  select "<a href='" + url.Url + "' class='breadcrumb'>" + url.Title + "</a>").ToArray();
                Breadcumbs.InnerHtml = String.Join("&nbsp;&nbsp;&laquo;&nbsp;&nbsp;", array);
                Breadcumbs.InnerHtml = (String.IsNullOrEmpty(Breadcumbs.InnerHtml)
                                            ? ""
                                            : "&nbsp;&nbsp;&nbsp;&nbsp;&laquo;&nbsp;&nbsp; ") + Breadcumbs.InnerHtml;
            }
        }
    }

    private void ConfigureHeader()
    {
        lblReportHeader.Text = "";
        if (Page is PageBase)
        {
            var legalEntityProfile = PageBase.Company.LegalEntityProfile;


            lblReportHeader.Text = @"<b>" + legalEntityProfile.CompanyName + @"</b><br />
                CNPJ:" + legalEntityProfile.CNPJ;

            if (!String.IsNullOrEmpty(legalEntityProfile.IE))
                lblReportHeader.Text += @"&nbsp;&nbsp;&nbsp;IE:" + legalEntityProfile.IE;

            lblReportHeader.Text += @"<br />";

            if (!String.IsNullOrEmpty(legalEntityProfile.Phone))
                lblReportHeader.Text += @"Tel: " + legalEntityProfile.Phone + @"&nbsp;&nbsp;&nbsp;";

            if (!String.IsNullOrEmpty(legalEntityProfile.Fax))
                lblReportHeader.Text += @"Fax:" + legalEntityProfile.Fax;

            lblReportHeader.Text += @"<br />";

            if (!String.IsNullOrEmpty(legalEntityProfile.Email))
                lblReportHeader.Text += @"E-mail:" + legalEntityProfile.Email + @"<br />";

            var address = legalEntityProfile.Address;
            if (address != null)
            {
                lblReportHeader.Text += address.Name;
                if (!String.IsNullOrEmpty(legalEntityProfile.AddressNumber))
                    lblReportHeader.Text += @", " + legalEntityProfile.AddressNumber;
                if (!String.IsNullOrEmpty(legalEntityProfile.AddressComp))
                    lblReportHeader.Text += @", " + legalEntityProfile.AddressComp;

                lblReportHeader.Text += @"<br />" + address.PostalCode + @" - " + address.Neighborhood + @" - " + address.City + @" - " + address.StateId + @"<br /><br />";
            }
            lblReportHeader.Text += PageBase.Company.CompanyConfiguration.ReportHeader;
        }
    }

    private void SetHelpText()
    {
        helpTooltip.Visible = false;
        if (SiteMap.CurrentNode != null && !String.IsNullOrEmpty(Page.Title))
        {
            helpTooltip.Visible = !String.IsNullOrEmpty(SiteMap.CurrentNode.Description);
            helpTooltip.Text = SiteMap.CurrentNode.Description;
        }
    }
}