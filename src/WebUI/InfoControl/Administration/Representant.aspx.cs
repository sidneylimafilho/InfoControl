using System;
using System.Web.UI;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;


[PermissionRequired("Representants")]
public partial class InfoControl_Representant : Vivina.Erp.SystemFramework.PageBase
{
    RepresentantManager manager;
    Representant original_representant, representant;

    protected void Page_Load(object sender, EventArgs e)
    {
        manager = new RepresentantManager(this);

        //retrieve the RepresentantId from Modal Popup
        if (!String.IsNullOrEmpty(Request["RepresentantId"]))
        {
            Page.ViewState["RepresentantId"] = Request["RepresentantId"].DecryptFromHex();
            lblRepresentantCode.Visible = true;
            lblRepresentantCode.Text = "Código do Representante: " + Request["RepresentantId"];
        }

        if (Context.Items["RepresentantId"] != null)
            Page.ViewState["RepresentantId"] = Context.Items["RepresentantId"];

        if (Page.ViewState["RepresentantId"] != null)
        {
            original_representant = manager.GetRepresentant(Convert.ToInt32(Page.ViewState["RepresentantId"]));

            if (!IsPostBack && original_representant != null)
            {
                /*
                 * The code below checks the type of profile(LegalEntityProfile/Profile)
                */
                if (original_representant.LegalEntityProfile != null)
                    Profile1.CompanyProfileEntity = original_representant.LegalEntityProfile;
                else
                    Profile1.ProfileEntity = original_representant.Profile;

                //
                //load bank informations
                //

                if (original_representant.BankId.HasValue)
                    cboBank.SelectedValue = Convert.ToString(original_representant.BankId);

                txtAccountNumber.Text = original_representant.AccountNumber;
                txtAgency.Text = original_representant.Agency;

                //
                //load ranking value
                //
                if (original_representant.Rating != null)
                    rtnRating.CurrentRating = Convert.ToInt32(original_representant.Rating);
            }
        }
        else
        {
            //
            //  Legal Entity
            //
            if (Page.ViewState["LegalEntityProfileId"] != null)
            {
                original_representant = manager.GetRepresentantByLegalEntityProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["LegalEntityProfileId"]));
                if (original_representant != null)
                {
                    Page.ViewState["ProfileExists"] = "0";

                    /*
                     * if isn't a postback set the values of company in profile_LegalEntity1
                     * else the values are reload in all postback
                     * 
                     */
                    if (!IsPostBack)
                        Profile1.CompanyProfileEntity = original_representant.LegalEntityProfile;

                }
            }

            //
            // Natural Person
            //
            if (Page.ViewState["ProfileId"] != null)
            {
                original_representant = manager.GetRepresentantByProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProfileId"]));
                if (original_representant != null)
                {
                    Page.ViewState["ProfileExists"] = "0";
                    /*if isn't a postback set the values of company in profile
                     * else the values are reload in all postback
                     */
                    if (!IsPostBack)
                        Profile1.ProfileEntity = original_representant.Profile;
                }
            }
            if (original_representant != null)
                Page.ViewState["RepresentantId"] = original_representant.RepresentantId;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        representant = new Representant();
        manager = new RepresentantManager(this);
        //
        // Clone the original representant for the linq track changes
        //

        if (Page.ViewState["RepresentantId"] != null)
        {
            original_representant = manager.GetRepresentant(Convert.ToInt32(Page.ViewState["RepresentantId"]));
            representant.CopyPropertiesFrom(original_representant);
        }

        representant.ModifiedDate = DateTime.Now;
        representant.CompanyId = Company.CompanyId;
        //
        //fill fields of account
        //
        if (!String.IsNullOrEmpty(cboBank.SelectedValue))
            representant.BankId = Convert.ToInt32(cboBank.SelectedValue);
        else
            representant.BankId = null;

        if (!String.IsNullOrEmpty(txtAccountNumber.Text))
            representant.AccountNumber = txtAccountNumber.Text;
        else
            representant.AccountNumber = null;

        if (!String.IsNullOrEmpty(txtAgency.Text))
            representant.Agency = txtAgency.Text;
        else
            representant.Agency = null;

        //
        //fill rating value
        //
        representant.Rating = rtnRating.CurrentRating;

        if (Profile1.ProfileEntity != null)
        {
            representant.ProfileId = Profile1.ProfileEntity.ProfileId;
            if (representant.ProfileId == 0)
                representant.Profile = Profile1.ProfileEntity;
        }
        else
        {
            representant.LegalEntityProfileId = Profile1.CompanyProfileEntity.LegalEntityProfileId;
            if (representant.LegalEntityProfileId == 0)
                representant.LegalEntityProfile = Profile1.CompanyProfileEntity;
        }

        if (Page.ViewState["RepresentantId"] == null && Page.ViewState["ProfileExists"] != "0")
            manager.Insert(representant);
        else
            manager.update(original_representant, representant);

        if (!String.IsNullOrEmpty(Request["RepresentantId"]))
            Response.Redirect("Representants.aspx");
        else
            Response.Redirect("Representant.aspx?RepresentantId=" + representant.RepresentantId.EncryptToHex());
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        pnlRating.Visible = pnlAccount.Visible = Profile1.ProfileEntity != null || Profile1.CompanyProfileEntity != null;
    }

    /// <summary>
    /// this method return true if the representant exist
    /// </summary>
    /// <returns></returns>
    private bool ExistRepresentant()
    {
        Boolean existRepresentant = false;
        manager = new RepresentantManager(this);

        if (manager.GetRepresentantByProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProfileId"])) != null)
            existRepresentant = true;

        if (manager.GetRepresentantByProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["LegalEntityProfileId"])) != null)
            existRepresentant = true;

        return existRepresentant;
    }

}
