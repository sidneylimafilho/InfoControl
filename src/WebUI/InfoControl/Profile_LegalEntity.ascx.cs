using System;
using System.ComponentModel;
using System.Web.UI;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;

public partial class Profile_LegalEntity : Vivina.Erp.SystemFramework.UserControlBase
{
    private LegalEntityProfile _companyProfile;
    private ProfileManager _profileManager;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public LegalEntityProfile CompanyProfileEntity
    {
        get
        {
            if (_companyProfile == null)
            {
                string cnpj = txtCNPJ.Text.Replace(".", "").Replace("-", "").Replace("_", "").Replace("/", "");
                // ReSharper disable PossibleNullReferenceException
                if (IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack && !String.IsNullOrEmpty(cnpj))
                // ReSharper restore PossibleNullReferenceException
                {
                    _companyProfile = _profileManager.GetLegalEntityProfile(Convert.ToInt32(Page.ViewState["LegalEntityProfileId"])) ?? new LegalEntityProfile();
                    _companyProfile.CompanyName = txtCompanyName.Text;

                    _companyProfile.FantasyName = null;
                    if (!String.IsNullOrEmpty(txtFantasyName.Text))
                        _companyProfile.FantasyName = txtFantasyName.Text;

                    _companyProfile.CNPJ = txtCNPJ.Text;
                    _companyProfile.IE = txtIE.Text;
                    _companyProfile.Phone = txtPhone.Text.RemoveMask();
                    _companyProfile.Fax = txtFax.Text.RemoveMask();
                    _companyProfile.MunicipalRegister = txtMunicipalRegister.Text;

                    //get Address and Commercialddress
                    _companyProfile.PostalCode = Address.PostalCode;
                    _companyProfile.AddressComp = Address.AddressComp;
                    _companyProfile.AddressNumber = Address.AddressNumber;
                    _companyProfile.RecoveryPostalCode = RecoveryAddress.PostalCode;
                    _companyProfile.RecoveryAddressComp = RecoveryAddress.AddressComp;
                    _companyProfile.RecoveryAddressNumber = RecoveryAddress.AddressNumber;
                    _companyProfile.Email = txtEmail.Text;

                     if (Page.ViewState["LegalEntityProfileId"] != null)
                        _companyProfile.LegalEntityProfileId = (int)Page.ViewState["LegalEntityProfileId"];
                    _companyProfile.ModifiedDate = DateTime.Now;


                    //
                    // Save the last modification
                    //
                    _profileManager.DbContext.SubmitChanges();
                }

            }
            return _companyProfile;
        }
        set
        {
            _companyProfile = value;

            if (_companyProfile != null)
                BindControls(_companyProfile);
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ValidationGroup
    {
        get { return _validationGroup; }
        set
        {
            _validationGroup = value;
            valCompanyName.ValidationGroup = value;
            Address.ValidationGroup = value;

            valCnpj.ValidationGroup = value;
            reqTxtCNPJ.ValidationGroup = value;
        }
    }
    private string _validationGroup;

    protected void Page_Load(object sender, EventArgs e)
    {
        _profileManager = new ProfileManager(this);

        valCnpj.ValidationGroup = ValidationGroup;
        reqTxtCNPJ.ValidationGroup = ValidationGroup;
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        CompanyProfileEntity = _profileManager.GetLegalEntityProfile(txtCNPJ.Text);
        //reqTxtCNPJ.Enabled = true;
        //reqTxtCNPJ.ValidationGroup = ValidationGroup;
    }

    private void BindControls(LegalEntityProfile legalEntityProfile)
    {
        string cnpj = legalEntityProfile.CNPJ.Replace(".", "").Replace("-", "").Replace("_", "").Replace("/", "");
        if (!String.IsNullOrEmpty(cnpj))
        {

            //
            // Populate the Control
            //
            txtCompanyName.Text = legalEntityProfile.CompanyName;
            txtFantasyName.Text = legalEntityProfile.FantasyName;
            txtMunicipalRegister.Text = legalEntityProfile.MunicipalRegister;
            txtCNPJ.Text = legalEntityProfile.CNPJ;
            txtIE.Text = legalEntityProfile.IE;
            txtPhone.Text = legalEntityProfile.Phone.RemoveMask();
            txtFax.Text = legalEntityProfile.Fax.RemoveMask();
           //Set Address and Commercialddress
            Address.PostalCode = legalEntityProfile.PostalCode;
            Address.AddressComp = legalEntityProfile.AddressComp;
            Address.AddressNumber = legalEntityProfile.AddressNumber;
            RecoveryAddress.PostalCode = legalEntityProfile.RecoveryPostalCode;
            RecoveryAddress.AddressComp = legalEntityProfile.RecoveryAddressComp;
            RecoveryAddress.AddressNumber = legalEntityProfile.RecoveryAddressNumber;
            txtEmail.Text = legalEntityProfile.Email;
           
            Page.ViewState["LegalEntityProfileId"] = legalEntityProfile.LegalEntityProfileId;
            Page.ViewState["ModifiedDate"] = legalEntityProfile.ModifiedDate;
            //Page.ViewState["CNPJ"] = txtCNPJ.Text;

            //
            // Hide the search form, cause yet load the LegalEntity
            //
            entryForm.Visible = true;
            searchForm.Visible = false;
        }
    }
}
