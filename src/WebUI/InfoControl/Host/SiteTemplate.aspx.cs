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


using InfoControl.Data;
using InfoControl;
using Vivina.Erp.BusinessRules.WebSites;
using Vivina.Erp.DataClasses;

public partial class InfoControl_Host_SiteTemplate : Vivina.Erp.SystemFramework.PageBase
{
    SiteManager siteManager;
    //SiteTemplate siteTemplate;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Context.Items["SiteTemplateId"] != null)
        //{
        //    Page.ViewState["SiteTemplateId"] = Context.Items["SiteTemplateId"];

        //}
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //siteManager = new SiteManager(this);
        //siteTemplate = new SiteTemplate();
        //SiteTemplate original_siteTemplate = new SiteTemplate();
        //if (Page.ViewState["SiteTemplateId"] != null)
        //{
        //    original_siteTemplate = siteManager.GetSiteTemplate(Convert.ToInt32(Page.ViewState["SiteTemplateId"]));
        //    siteTemplate.CopyPropertiesFrom(original_siteTemplate);
        //}

        //siteTemplate.Name = txtName.Text;
        //if (!String.IsNullOrEmpty(cboBranchId.SelectedValue))
        //    siteTemplate.BranchId = Convert.ToInt32(cboBranchId.SelectedValue);


        //if (original_siteTemplate.SiteTemplateId != 0)
        //    siteManager.UpdateSiteTemplate(original_siteTemplate, siteTemplate);
        //else
        //    siteManager.InsertSiteTemplate(siteTemplate, fupSiteTemplate.PostedFile);

        //Server.Transfer("siteTemplates.aspx");
    }

    /// <summary>
    /// this method shows site Template
    /// </summary>
    private void showSiteTemplate()
    {
        //siteTemplate = siteManager.GetSiteTemplate(Convert.ToInt32(Page.ViewState["SiteTemplateId"]));
        //txtName.Text = siteTemplate.Name;
        //if (siteTemplate.BranchId.HasValue)
        //    cboBranchId.SelectedValue = siteTemplate.BranchId.ToString();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Server.Transfer("SiteTemplates.aspx");
    }
}
