using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Data;
using InfoControl.Web.Security;


[PermissionRequired("Transporters")]
public partial class Company_Transporter : Vivina.Erp.SystemFramework.PageBase
{
    Transporter originalTransporter;
    TransporterManager manager;

    protected void Page_Load(object sender, EventArgs e)
    {
        manager = new TransporterManager(this);

        if (!IsPostBack)
        {   
            //
            //retrieve the transporterId from modalPopup
            //
            if (Request["TransporterId"] != null)
                Context.Items["TransporterId"] = Request["TransporterId"];

            if (Context.Items["TransporterId"] != null)
                Page.ViewState["TransporterId"] = Context.Items["TransporterId"];

            if (Page.ViewState["TransporterId"] != null)
            {
                originalTransporter = manager.GetTransporter(Convert.ToInt32(Page.ViewState["TransporterId"]));
                if (originalTransporter != null)
                    Profile_LegalEntity1.CompanyProfileEntity = originalTransporter.LegalEntityProfile;
            }
            else if (Page.ViewState["LegalEntityProfileId"] != null)
            {
                originalTransporter = manager.GetTransporterByProfile(Convert.ToInt32(Page.ViewState["LegalEntityProfileId"]));
                if (originalTransporter != null)
                {
                    Page.ViewState["ProfileExists"] = "0";
                    Profile_LegalEntity1.CompanyProfileEntity = originalTransporter.LegalEntityProfile;
                }
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        CrossFrameCookies = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Transporter transporter = new Transporter();

        if (Profile_LegalEntity1.CompanyProfileEntity != null)
            originalTransporter = manager.GetTransporterByLegalEntityProfile(Company.CompanyId, Profile_LegalEntity1.CompanyProfileEntity.LegalEntityProfileId);
        else if (Page.ViewState["TransporterId"] != null)
            originalTransporter = manager.GetTransporter(Convert.ToInt32(Page.ViewState["TransporterId"]));

        if (originalTransporter != null)
            transporter.CopyPropertiesFrom(originalTransporter);

        transporter.CompanyId = Company.MatrixId.Value;
        transporter.ModifiedDate = DateTime.Now;
        transporter.LegalEntityProfileId = Profile_LegalEntity1.CompanyProfileEntity.LegalEntityProfileId;

        //
        // Add entity for insert
        //
        if (transporter.LegalEntityProfileId == 0)
            transporter.LegalEntityProfile = Profile_LegalEntity1.CompanyProfileEntity;

        if (Page.ViewState["TransporterId"] == null && Page.ViewState["ProfileExists"] != "0")
            manager.Insert(transporter);
        else
        {
            manager.Update(originalTransporter, transporter);
        }
        Response.Redirect("Transporters.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Transporters.aspx");
    }
}
