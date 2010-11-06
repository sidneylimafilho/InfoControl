using System;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;


public partial class Company_Supplier_General : Vivina.Erp.SystemFramework.PageBase
{
    private SupplierManager _manager;
    private Supplier _originalsupplier;

    protected void Page_Load(object sender, EventArgs e)
    {
        _manager = new SupplierManager(this);



        //retrieve the SupplierId from Modal Popup
        if (!String.IsNullOrEmpty(Request["SupplierId"]))
            Page.ViewState["SupplierId"] = Request["SupplierId"].DecryptFromHex();

        if (Context.Items["SupplierId"] != null)
            Page.ViewState["SupplierId"] = Context.Items["SupplierId"];

        if (Page.ViewState["SupplierId"] != null)
        {
            litTitle.Visible = false;
            Comments.SubjectId = Convert.ToInt32(Page.ViewState["SupplierId"]);
            btnCancel.OnClientClick = "parent.location='Suppliers.aspx'; return false;";

            _originalsupplier = _manager.GetSupplier(Convert.ToInt32(Page.ViewState["SupplierId"]), Company.CompanyId);

            if (!IsPostBack && _originalsupplier != null)
            {
                //
                // After the verification if a supplier is selected, verify if the supplier has category
                // and set the right category to the DropDownList
                //
                if (_originalsupplier.SupplierCategoryId != null)
                {
                    if (cboSupplierCategory.Items.FindByValue(Convert.ToString(_originalsupplier.SupplierCategoryId)) != null)
                        cboSupplierCategory.SelectedValue = Convert.ToString(_originalsupplier.SupplierCategoryId);
                }

                /*
                 * The code below checks the type of profile(LegalEntityProfile/Profile)
                */
                if (_originalsupplier.LegalEntityProfile != null)
                    Profile1.CompanyProfileEntity = _originalsupplier.LegalEntityProfile;
                else
                    Profile1.ProfileEntity = _originalsupplier.Profile;

                //
                //load bank informations
                //

                cboBank.DataBind();
                ListItem listItem = null;

                if (_originalsupplier.BankId.HasValue)
                    listItem = cboBank.Items.FindByValue(Convert.ToString(_originalsupplier.BankId));

                if (listItem != null)
                    cboBank.SelectedValue = listItem.Value;

                txtAccountNumber.Text = _originalsupplier.AccountNumber;
                txtAgency.Text = _originalsupplier.Agency;
                ucAccountCreatedDate.DateTime = _originalsupplier.AccountCreatedDate;

                //
                //load ranking value
                //
                if (_originalsupplier.Ranking != null)
                    rtnRanking.CurrentRating = Convert.ToInt32(_originalsupplier.Ranking);
            }
        }
        else
        {
            btnCancel.OnClientClick = "location='Suppliers.aspx'; return false;";

            //
            //  Legal Entity
            //
            if (Page.ViewState["LegalEntityProfileId"] != null)
            {
                _originalsupplier = _manager.GetSuppliersByLegalEntityProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["LegalEntityProfileId"]));
                if (_originalsupplier != null)
                {
                    Page.ViewState["ProfileExists"] = "0";

                    /*
                     * if isn't a postback set the values of company in profile_LegalEntity1
                     * else the values are reload in all postback
                     * 
                     */
                    if (!IsPostBack)
                        Profile1.CompanyProfileEntity = _originalsupplier.LegalEntityProfile;
                }
            }

            //
            // Natural Person
            //
            if (Page.ViewState["ProfileId"] != null)
            {
                _originalsupplier = _manager.GetSupplierByProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProfileId"]));
                if (_originalsupplier != null)
                {
                    Page.ViewState["ProfileExists"] = "0";
                    /*if isn't a postback set the values of company in profile
                     * else the values are reload in all postback
                     */
                    if (!IsPostBack)
                        Profile1.ProfileEntity = _originalsupplier.Profile;
                }
            }
        }
        //if (Session["w"] != null)
        //    btnCancel.OnClientClick = "top.tb_remove(); return false;";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var supplier = new Supplier();

        //
        // Clone the original supplier for the linq track changes
        //
        if (_originalsupplier != null)
            supplier.CopyPropertiesFrom(_originalsupplier);

        supplier.CompanyId = Company.MatrixId.Value;

        //
        //fill fields of account
        //
        supplier.BankId = String.IsNullOrEmpty(cboBank.SelectedValue)
                              ? (int?)null
                              : Convert.ToInt32(cboBank.SelectedValue);

        supplier.AccountNumber = String.IsNullOrEmpty(txtAccountNumber.Text)
                                     ? null
                                     : txtAccountNumber.Text;

        supplier.Agency = String.IsNullOrEmpty(txtAgency.Text)
                              ? null
                              : txtAgency.Text;


        supplier.AccountCreatedDate = null;

        if (ucAccountCreatedDate.DateTime.HasValue)
            supplier.AccountCreatedDate = ucAccountCreatedDate.DateTime;

        //
        //fill ranking value
        //
        supplier.Ranking = rtnRanking.CurrentRating;

        if (Profile1.ProfileEntity != null)
        {
            supplier.ProfileId = Profile1.ProfileEntity.ProfileId;
            if (supplier.ProfileId == 0)
                supplier.Profile = Profile1.ProfileEntity;
        }
        else
        {
            supplier.LegalEntityProfileId = Profile1.CompanyProfileEntity.LegalEntityProfileId;
            if (supplier.LegalEntityProfileId == 0)
                supplier.LegalEntityProfile = Profile1.CompanyProfileEntity;
        }

        //
        // Verify if a category is selected and associate to supplier
        //
        if (!String.IsNullOrEmpty(cboSupplierCategory.SelectedValue))
            supplier.SupplierCategoryId = Convert.ToInt32(cboSupplierCategory.SelectedValue);

        //
        // Verify if exists this supplier and mode not equals update
        //
        if (ExistsSupplier() && Page.ViewState["SupplierId"] == null)
            Server.Transfer("Suppliers.aspx");

        if (Page.ViewState["SupplierId"] == null && Page.ViewState["ProfileExists"] == null)
        {
            supplier.CreatedByUser = User.Identity.UserName;
            _manager.Insert(supplier);
        }
        else
        {
            supplier.ModifiedByUser = User.Identity.UserName;
            _manager.Update(_originalsupplier, supplier);
        }

        if (String.IsNullOrEmpty(Request["w"]))
            Page.ClientScript.RegisterStartupScript(GetType(), "", "top.location = Supplier.aspx?SupplierId=" + supplier.SupplierId.EncryptToHex(), true);
        else
            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "CloseModal",
                "top.$.LightBoxObject.close();" +
                "top.content.location.href+='?';",
                true);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        pnlRanking.Visible = pnlAccount.Visible = Profile1.ProfileEntity != null || Profile1.CompanyProfileEntity != null;
    }

    //
    // This method verify if already exists this supplier.
    // Return true if already exists supplier for this profile
    //
    private bool ExistsSupplier()
    {
        Supplier supplier = _manager.GetSupplierByProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProfileId"]));

        return (supplier != null);
    }
}