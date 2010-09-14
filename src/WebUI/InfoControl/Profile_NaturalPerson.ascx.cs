using System;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;

public partial class Profile_NaturalPerson : Vivina.Erp.SystemFramework.UserControlBase
{
    private Profile _profile;
    private ProfileManager profileManager;

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ValidationGroup
    {
        get;
        set;
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public Profile ProfileEntity
    {
        get
        {
            //Modified by Higor when 04-08-2008. always Update the _profile object (null or filled)

            string cpf = txtCPF.Text.Replace(".", "").Replace("-", "").Replace("_", "");
            if (_profile == null && IsPostBack && !ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
            {
                _profile = new Profile();
                if (Page.ViewState["ProfileId"] != null)
                    _profile = profileManager.GetProfile(Convert.ToInt32(Page.ViewState["ProfileId"]));

                _profile.Name = txtName.Text;
                _profile.AddressComp = Address.AddressComp;
                _profile.AddressNumber = Address.AddressNumber;
                _profile.RecoveryAddressComp = RecoveryAddress.AddressComp;
                _profile.RecoveryAddressNumber = RecoveryAddress.AddressNumber;

                if (ucBirthDate.DateTime.HasValue)
                    _profile.BirthDate = ucBirthDate.DateTime;

                _profile.CellPhone = txtCellPhone.Text;
                _profile.CPF = txtCPF.Text;

                if (!String.IsNullOrEmpty(cboEducation.SelectedValue))
                    _profile.EducationLevelId = int.Parse(cboEducation.SelectedValue);

                _profile.Email = String.Empty;
                _profile.HomePhone = txtHomePhone.Text;

                if (!String.IsNullOrEmpty(cboMaritalStatus.SelectedValue))
                    _profile.MaritalStatusId = int.Parse(cboMaritalStatus.SelectedValue);

                _profile.Phone = txtPhone.Text.RemoveMask();
                _profile.Fax = txtFax.Text.RemoveMask();
                _profile.PostalCode = Address.PostalCode;
                _profile.RecoveryPostalCode = RecoveryAddress.PostalCode;
                _profile.RG = txtRG.Text;
                _profile.IssueBureau = txtIssueBureau.Text;
                _profile.Email = txtEmail.Text;
                _profile.ProfessionalRegister = txtProfessionalRegister.Text;
                _profile.VotingTitle = txtVoltingTitle.Text;
                _profile.CnhNumber = txtCnhNumber.Text;
                _profile.CnhClass = txtCnhClass.Text;
                _profile.FatherName = txtFatherName.Text;
                _profile.MotherName = txtMotherName.Text;


                if (ucCnhExpiresDate.DateTime.HasValue)
                    _profile.CnhExpiresDate = ucCnhExpiresDate.DateTime.Value;
                else
                    _profile.CnhExpiresDate = null;

                if (ucCnhRgCreateDate.DateTime.HasValue)
                    _profile.RgCreatedDate = ucCnhRgCreateDate.DateTime.Value;
                else
                    _profile.RgCreatedDate = null;

                if (Page.ViewState["ProfileId"] != null)
                    _profile.ProfileId = Convert.ToInt32(Page.ViewState["ProfileId"]);

                _profile.ModifiedDate = DateTime.Now;

                if (!String.IsNullOrEmpty(cboSex.SelectedValue))
                    _profile.SexId = Int32.Parse(cboSex.SelectedValue);

                _profile.BornCity = txtBornCity.Text;
                _profile.BornCountry = txtBornCountry.Text;

                //
                //fill professional informations
                //
                _profile.Profission = txtProfission.Text;
                _profile.Post = txtPost.Text;


                _profile.Salary = ucCurrFieldSalary.CurrencyValue;


                _profile.AdmissionDate = ucDtAdmissionDate.DateTime;

                _profile.CompanyPostalCode = companyAddress.PostalCode;
                _profile.CompanyAddressComp = companyAddress.AddressComp;
                _profile.CompanyAddressNumber = companyAddress.AddressNumber;
                _profile.CompanyPhone = txtCompanyPhone.Text;
                _profile.CompanyName = txtCompanyName.Text;

                //
                //fill aditional informations
                //
                _profile.HasOwnCar = chkHasOwnCar.Checked;
                _profile.HasOwnHouse = chkHasOwnHome.Checked;
                _profile.PersonalReferences1 = txtPersonalReference1.Text;
                _profile.PersonalReferences2 = txtPersonalReference2.Text;
                _profile.ComercialReferences1 = txtComercialReference1.Text;
                _profile.ComercialReferences2 = txtComercialReference2.Text;

                _profile.CarLeasingValue = ucCurrFieldCarLeasingValue.CurrencyValue;
                _profile.HouseRentValue = ucCurrFieldRentHouseValue.CurrencyValue;
                _profile.DependentNumber = ucCurrFieldDependentNumber.IntValue;



                //
                // Save the last modification
                //
                profileManager.DbContext.SubmitChanges();

            }
            //}
            return _profile;
        }
        set
        {
            _profile = value;
            //if (!IsPostBack && _profile != null) Modified by Higor when 19-05-2008, this Line is the OriginalLine

            // Modified by Higor when 04-08-2008, set the ProfileObjects only if Profile isn't filled
            if (_profile != null && Convert.ToBoolean(Page.ViewState["FilledControl"]) == false)
                BindControls(_profile);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        profileManager = new ProfileManager(this);

        ValEmail.ValidationGroup = ValidationGroup;
        reqHomePhone.ValidationGroup = ValidationGroup;
        reqPhone.ValidationGroup = ValidationGroup;

        reqSex.ValidationGroup = ValidationGroup;
        reqMaritalStatus.ValidationGroup = ValidationGroup;
        reqEducation.ValidationGroup = ValidationGroup;

        valCpf.ValidationGroup = ValidationGroup;
        valName.ValidationGroup = ValidationGroup;
        CpfValidator.ValidationGroup = ValidationGroup;
        CpfValidator2.ValidationGroup = ValidationGroup;
        btnSelect.ValidationGroup = ValidationGroup;
        Address.ValidationGroup = ValidationGroup;
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        ProfileEntity = profileManager.GetProfile(txtSearchCPF.Text) ?? new Profile();

        txtCPF.Text = txtSearchCPF.Text;
        txtCPF.Enabled = false;
        txtSearchCPF.Text = "";

        //
        // Hide the search form, cause yet load the LegalEntity
        //
        entryForm.Visible = true;
        searchForm.Visible = false;
    }

    private void BindControls(Profile profile)
    {
        //
        // Realize Data bind because the combobox are empty
        //
        cboEducation.DataBind();
        cboMaritalStatus.DataBind();
        cboSex.DataBind();

        //
        // Populate the Control
        //

        ucBirthDate.DateTime = _profile.BirthDate;

        txtCellPhone.Text = profile.CellPhone;
        txtCPF.Text = profile.CPF;
        txtCPF.Enabled = false;
        txtHomePhone.Text = profile.HomePhone;
        txtFax.Text = profile.Fax;
        txtName.Text = profile.Name;
        txtPhone.Text = profile.Phone;
        txtRG.Text = profile.RG;
        //txtObservations.Text = profile.Observations;
        txtEmail.Text = profile.Email;
        txtProfessionalRegister.Text = profile.ProfessionalRegister;
        txtVoltingTitle.Text = profile.VotingTitle;
        txtMotherName.Text = profile.MotherName;
        txtFatherName.Text = profile.FatherName;


        ucCnhRgCreateDate.DateTime = profile.RgCreatedDate;



        txtIssueBureau.Text = profile.IssueBureau;
        txtCnhNumber.Text = profile.CnhNumber;
        txtCnhClass.Text = profile.CnhClass;

        ucCnhExpiresDate.DateTime = profile.CnhExpiresDate;

        if (profile.EducationLevelId != null)
            cboEducation.SelectedValue = profile.EducationLevelId.ToString(); //.Items.FindByValue(Convert.ToString(profile.EducationLevelId)).Selected = true;

        if (profile.MaritalStatusId != null)
            cboMaritalStatus.SelectedValue = profile.MaritalStatusId.ToString(); //.Items.FindByValue(Convert.ToString(profile.MaritalStatusId)).Selected = true;

        if (profile.SexId != null)
            cboSex.SelectedValue = profile.SexId.ToString();//.Items.FindByValue(Convert.ToString(profile.SexId)).Selected = true;

        txtBornCity.Text = profile.BornCity;
        txtBornCountry.Text = profile.BornCountry;


        //
        //populate the controls of profissional informations
        //


        txtCompanyName.Text = profile.CompanyName;
        txtProfission.Text = profile.Profission;
        txtPost.Text = profile.Post;
        companyAddress.PostalCode = profile.CompanyPostalCode;
        companyAddress.AddressNumber = profile.CompanyAddressNumber;
        companyAddress.AddressComp = profile.CompanyAddressComp;
        txtCompanyPhone.Text = profile.CompanyPhone;

        //if (profile.AdmissionDate != null)
        //    txtAdmissionDate.Text = profile.AdmissionDate.ToString();

        ucDtAdmissionDate.DateTime = profile.AdmissionDate;

        ucCurrFieldSalary.CurrencyValue = profile.Salary;



        //
        //populate the controls of aditional informations
        //
        chkHasOwnCar.Checked = Convert.ToBoolean(profile.HasOwnCar);
        chkHasOwnHome.Checked = Convert.ToBoolean(profile.HasOwnHouse);
        ucCurrFieldDependentNumber.CurrencyValue = profile.DependentNumber;
        ucCurrFieldRentHouseValue.CurrencyValue = profile.HouseRentValue;
        ucCurrFieldCarLeasingValue.CurrencyValue = profile.CarLeasingValue;
        txtPersonalReference1.Text = profile.PersonalReferences1;
        txtPersonalReference2.Text = profile.PersonalReferences2;
        txtComercialReference1.Text = profile.ComercialReferences1;
        txtComercialReference2.Text = profile.ComercialReferences2;


        Address.PostalCode = profile.PostalCode;
        Address.AddressComp = profile.AddressComp;
        Address.AddressNumber = profile.AddressNumber;

        RecoveryAddress.PostalCode = profile.RecoveryPostalCode;
        RecoveryAddress.AddressComp = profile.RecoveryAddressComp;
        RecoveryAddress.AddressNumber = profile.RecoveryAddressNumber;

        Page.ViewState["ProfileId"] = profile.ProfileId;
        Page.ViewState["ModifiedDate"] = profile.ModifiedDate;
        Page.ViewState["CPF"] = txtCPF.Text;
        Page.ViewState["FilledControl"] = true;



        //
        // Hide the search form, cause yet load the LegalEntity
        //
        entryForm.Visible = true;
        searchForm.Visible = false;
    }


    protected void txtCPF_TextChanged(object sender, EventArgs e)
    {
        ProfileEntity = profileManager.GetProfile(Page.Company.CompanyId, txtCPF.Text);        
    }
}
