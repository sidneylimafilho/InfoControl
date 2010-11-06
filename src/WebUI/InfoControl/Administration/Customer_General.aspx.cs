using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl.Web.Security;
using System.Web.Security;
using Exception = Resources.Exception;
using User = InfoControl.Web.Security.DataEntities.User;

public partial class Company_Customer_General : Vivina.Erp.SystemFramework.PageBase
{
    CustomerManager customerManager;
    Customer originalCustomer;
    MembershipManager membershipManager;
    CompanyManager companyManager;
    MembershipCreateStatus status;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["w"]))
            btnCancel.OnClientClick = "top.$.LightBoxObject.close();return false;";
        else
        {
            btnCancel.OnClientClick = "location='Customers.aspx';return false;";

            if (!String.IsNullOrEmpty(Request["CustomerId"]))
                btnCancel.OnClientClick = "parent.location='Customers.aspx';return false;";
        }

        customerManager = new CustomerManager(this);
        lblMessage.Text = String.Empty;

        if (!String.IsNullOrEmpty(Request["CustomerId"]))
            Page.ViewState["CustomerId"] = Request["CustomerId"].DecryptFromHex();

        //
        // Update
        //
        if (Page.ViewState["CustomerId"] != null)
        {
            litTitle.Visible = false;
            Page.ViewState["loaded"] = true;
            originalCustomer = customerManager.GetCustomer(Convert.ToInt32(Page.ViewState["CustomerId"]), Company.MatrixId.Value);

            if (!IsPostBack && originalCustomer != null)
            {
                customerComments.SubjectId = originalCustomer.CustomerId;
                customerComments.Visible = true;
                pnlCreateDate.Visible = true;

                if (originalCustomer.User != null)
                {
                    txtUserName.Text = originalCustomer.User.UserName;
                    txtPassword.Text = originalCustomer.User.Password;
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    chkRemoveUser.Visible = true;
                }

                //
                // The code below checks the type of profile(LegalEntityProfile/Profile)
                //
                if (originalCustomer.LegalEntityProfile != null)
                    ucProfile.CompanyProfileEntity = originalCustomer.LegalEntityProfile;
                else
                    ucProfile.ProfileEntity = originalCustomer.Profile;

                pnlOtherData.Visible = true;

                ucCurrFieldCreditLimit.CurrencyValue = originalCustomer.CreditLimit;

                if (originalCustomer.RepresentantId.HasValue)
                    cboRepresentant.SelectedValue = Convert.ToString(originalCustomer.RepresentantId);

                if (originalCustomer.CreatedDate.HasValue)
                    lblCreatedDate.Text = originalCustomer.CreatedDate.Value.Date.ToShortDateString();

                if (originalCustomer.CustomerTypeId.HasValue)
                    cboCustomerType.SelectedValue = originalCustomer.CustomerTypeId.ToString();


                ShowCustomerConfiguration(originalCustomer);
                //
                //load ranking value
                //
                if (originalCustomer.Ranking != null)
                    rtnRanking.CurrentRating = Convert.ToInt32(originalCustomer.Ranking);
            }
        }
        else
        {
            customerComments.Visible = false;

            //
            //  Legal Entity
            //
            if (Page.ViewState["LegalEntityProfileId"] != null)
            {
                originalCustomer = customerManager.GetCustomerByLegalEntityProfile(Company.MatrixId.Value, Convert.ToInt32(Page.ViewState["LegalEntityProfileId"]));
                if (originalCustomer != null)
                {
                    Page.ViewState["ProfileExists"] = "0";

                    /*
                     * if isn't a postback set the values of company in profile_LegalEntity1
                     * else the values are reload in all postback
                     * 
                     */
                    if (!IsPostBack)
                    {
                        //SetVendorValue(originalcustomer);
                        ucProfile.CompanyProfileEntity = originalCustomer.LegalEntityProfile;
                    }
                }
            }

            //
            // Natural Person
            //
            if (Page.ViewState["ProfileId"] != null)
            {
                originalCustomer = customerManager.GetCustomerByProfile(Company.MatrixId.Value, Convert.ToInt32(Page.ViewState["ProfileId"]));
                if (originalCustomer != null)
                {
                    Page.ViewState["ProfileExists"] = "0";
                    /*
                     * if isn't a postback set the values of company in profile
                     * else the values are reload in all postback
                     *
                     */
                    if (!IsPostBack)
                        ucProfile.ProfileEntity = originalCustomer.Profile;

                }
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        pnlRanking.Visible = ucProfile.ProfileEntity != null || ucProfile.CompanyProfileEntity != null;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Customer customer = new Customer();

        // Clone the original customer for the linq track changes 
        if (originalCustomer != null)
        {
            customer.CopyPropertiesFrom(originalCustomer);
        }

        customer.BlockSalesInDebit = true;

        if (Company.MatrixId.HasValue)
            customer.CompanyId = Company.MatrixId.Value;
        else
            customer.CompanyId = Company.CompanyId;

        customer.BankId = null;
        if (!String.IsNullOrEmpty(cboBank.SelectedValue))
            customer.BankId = Convert.ToInt32(cboBank.SelectedValue);

        customer.Agency = null;
        if (!String.IsNullOrEmpty(txtAgency.Text))
            customer.Agency = txtAgency.Text;

        customer.AccountNumber = null;
        if (!String.IsNullOrEmpty(txtAccountNumber.Text))
            customer.AccountNumber = txtAccountNumber.Text;

        customer.AccountCreatedDate = null;
        if (!String.IsNullOrEmpty(txtAccountCreatedDate.Text))
            customer.AccountCreatedDate = Convert.ToDateTime(txtAccountCreatedDate.Text);

        ///vendor
        if (!String.IsNullOrEmpty(cboVendors.SelectedValue))
            customer.SalesPersonId = Convert.ToInt32(cboVendors.SelectedValue);

        if (Page.ViewState["SalesPersonId"] != null)
            customer.SalesPersonId = Convert.ToInt32(Page.ViewState["SalesPersonId"]);

        if (ucVendorComission.CurrencyValue.HasValue)
            customer.SalesPersonCommission = ucVendorComission.CurrencyValue;
      
        if (!String.IsNullOrEmpty(cboSupplementalVendor.SelectedValue))
            customer.SupplementalSalesPersonId = Convert.ToInt32(cboSupplementalVendor.SelectedValue);

        if (Page.ViewState["supplementalSalesPersonId"] != null)
            customer.SupplementalSalesPersonId = Convert.ToInt32(Page.ViewState["supplementalSalesPersonId"]);
        
        if (ucSupplementalVendorComission.CurrencyValue.HasValue)
            customer.SupplementalSalesPersonCommission = ucSupplementalVendorComission.CurrencyValue;

        customer.RepresentantId = null;
        if (!String.IsNullOrEmpty(cboRepresentant.SelectedValue))
            customer.RepresentantId = Convert.ToInt32(cboRepresentant.SelectedValue);

        customer.CreditLimit = ucCurrFieldCreditLimit.CurrencyValue;
        
        customer.CustomerTypeId = null;
        if (!String.IsNullOrEmpty(cboCustomerType.SelectedValue))
            customer.CustomerTypeId = Convert.ToInt32(cboCustomerType.SelectedValue);

        //fill field ranking
        customer.Ranking = rtnRanking.CurrentRating;

        if (ucProfile.ProfileEntity != null)
        {
            if (String.IsNullOrEmpty(ucProfile.ProfileEntity.Phone.Trim('-', '_', '(', ')')) && String.IsNullOrEmpty(ucProfile.ProfileEntity.CellPhone.Trim('-', '_', '(', ')')) && String.IsNullOrEmpty(ucProfile.ProfileEntity.HomePhone.Trim('-', '_', '(', ')')))
            {
                ShowError("Ao menos um telefone deve ser preenchido!");
                return;
            }

            // Add the entity to Insert
            if (ucProfile.ProfileEntity.ProfileId == 0)
                customer.Profile = ucProfile.ProfileEntity;
        }
        else
        {
            // Add the entity to Insert
            customer.LegalEntityProfileId = ucProfile.CompanyProfileEntity.LegalEntityProfileId;
            if (ucProfile.CompanyProfileEntity.LegalEntityProfileId == 0)
                customer.LegalEntityProfile = ucProfile.CompanyProfileEntity;
        }

        //
        //Insert
        //

        if (Page.ViewState["CustomerId"] == null && Page.ViewState["ProfileExists"] != "0")
        {
            membershipManager = new MembershipManager(this);
            companyManager = new CompanyManager(this);

            if (!String.IsNullOrEmpty(txtUserName.Text))
            {
                customer.CreatedByUser = User.Identity.UserName;
                status = customerManager.Insert(customer, FillUser());

                ShowMessage(status);
                chkRemoveUser.Visible = (status == MembershipCreateStatus.Success);
                lblPassword.Visible = txtPassword.Visible = !(status == MembershipCreateStatus.Success);


                if (status == MembershipCreateStatus.Success)
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Customer.aspx?CustomerId=" + customer.CustomerId.EncryptToHex() + "';", true);

                return;
            }

            customer.CreatedByUser = User.Identity.UserName;
            customerManager.Insert(customer);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Customer.aspx?CustomerId=" + customer.CustomerId.EncryptToHex() + "';", true);

        } // Update
        else
        {
            membershipManager = new MembershipManager(this);
            customerManager = new CustomerManager(this);
            var user = FillUser();

            customer.ModifiedByUser = User.Identity.UserName;
            //
            //Delete the user account of customer
            //
            if (chkRemoveUser.Checked)
            {                
                customer.User = null;
                lblMessage.Text = String.Empty;
                chkRemoveUser.Checked = false;
                chkRemoveUser.Visible = false;
                txtUserName.Text = String.Empty;
                lblPassword.Visible = txtPassword.Visible = true;

                customerManager.Update(originalCustomer, customer);
                return;
            }

            if (originalCustomer.UserId.HasValue)
            {
                if (originalCustomer.User.UserName != txtUserName.Text)
                {
                    if (customerManager.GetCustomerByUserName(Company.CompanyId, txtUserName.Text) != null)
                    {
                        lblMessage.Text = "Já existe um cliente com esse usuário nesta empresa!";
                        return;
                    }

                    customerManager.UpdateUserNameOfCustomer(originalCustomer, txtUserName.Text);
                    return;
                }
            }

            if (!String.IsNullOrEmpty(txtUserName.Text) && !originalCustomer.UserId.HasValue)
            {
                if (customerManager.GetCustomerByUserName(Company.CompanyId, txtUserName.Text) != null)
                {
                    lblMessage.Text = "Já existe um cliente com esse usuário nesta empresa!";
                    return;
                }

                membershipManager.Insert(user, out status, true);

                if (status == MembershipCreateStatus.Success)
                {
                    customer.UserId = user.UserId;
                    customerManager.Update(originalCustomer, customer);

                    chkRemoveUser.Visible = true;
                    txtPassword.Visible = false;
                    lblPassword.Visible = false;
                }

            }
         
            customerManager.Update(originalCustomer, customer);

            //
            // In case of cnpj/cpf already exists in another category but the same cnpj/cpf is not yet a customer
            // the data will be loaded and save as update not insert, then this code redirects correctly to finalize
            // the register.
            //
            if (Page.ViewState["CustomerId"] == null)
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "location='Customer.aspx?CustomerId=" + customer.CustomerId.EncryptToHex() + "';", true);
        }
    }

    /// <summary>
    /// This method just show the message returned of out parameter status 
    /// of insert's membership method
    /// </summary>
    /// <param name="status"></param>
    private void ShowMessage(MembershipCreateStatus status)
    {
        switch (status)
        {
            case MembershipCreateStatus.DuplicateEmail:

                lblMessage.Text = Exception.ExistentMail;
                return;

            case MembershipCreateStatus.DuplicateUserName:

                lblMessage.Text = Exception.ExistUser;
                return;

            case MembershipCreateStatus.InvalidPassword:

                lblMessage.Text = Exception.InvalidUserPassword;
                return;

            case MembershipCreateStatus.Success:
                lblMessage.Text = Exception.UserCreatedIsValid;
                break;
        }
    }

    /// <summary>
    /// This method convert the information of VivinaFramework's User type to DataClasses's User type
    /// </summary>
    /// <param name="membershipEntity"></param>
    /// <param name="user"></param>
    private void CopyMembershipEntityToUser(User membershipEntity, Vivina.Erp.DataClasses.User user)
    {
        user.CreationDate = membershipEntity.CreationDate;
        user.Email = membershipEntity.Email;
        user.FailedPasswordAttemptCount = membershipEntity.FailedPasswordAttemptCount;
        user.HasChangePassword = membershipEntity.HasChangePassword;
        user.IsActive = membershipEntity.IsActive;
        user.IsLockedOut = membershipEntity.IsLockedOut;
        
        user.LastActivityDate = membershipEntity.LastActivityDate;
        user.LastLockoutDate = membershipEntity.LastLockoutDate;
        user.LastLoginDate = membershipEntity.LastLoginDate;
        user.LastPasswordChangedDate = membershipEntity.LastPasswordChangedDate;
        user.LastRemoteHost = membershipEntity.LastRemoteHost;
        user.Password = membershipEntity.Password;
        user.PasswordAnswer = membershipEntity.PasswordAnswer;
        user.PasswordQuestion = membershipEntity.PasswordQuestion;
        user.ProfileId = membershipEntity.ProfileId;
        user.UserId = membershipEntity.UserId;
        user.UserName = membershipEntity.UserName;
    }

    /// <summary>
    /// This method fill an object with informations of form
    /// </summary>
    /// <returns></returns>
    private InfoControl.Web.Security.DataEntities.User FillUser()
    {
        var user = new InfoControl.Web.Security.DataEntities.User();

        user.UserName = txtUserName.Text;
        user.Password = txtPassword.Text;
        user.PasswordAnswer = txtPassword.Text;
        user.Email = txtUserName.Text;

        user.LastLoginDate = DateTime.Now;
        user.LastPasswordChangedDate = DateTime.Now;
        user.LastLockoutDate = DateTime.Now;
        user.LastActivityDate = DateTime.Now;
        user.CreationDate = DateTime.Now;

        user.HasChangePassword = false;
        user.IsOnline = false;
        user.IsActive = false;
        user.IsLockedOut = true;

        user.FailedPasswordAttemptCount = 3;

        return user;

    }




    private bool ExistsCustomer()
    {
        Customer _customer = customerManager.GetCustomerByProfile(Company.CompanyId, Convert.ToInt32(Page.ViewState["ProfileId"]));
        return (_customer != null);
    }

    protected void odsCustomerType_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void odsSalesPerson_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void cboVendors_TextChanged(object sender, EventArgs e)
    {

    }

    protected void cboVendors_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region functions
    private void SetVendorValue(Customer customer)
    {
        var humanResourcesManager = new HumanResourcesManager(this);
        Employee employee = new Employee();

        if (customer.SalesPersonId.HasValue)
        {
            lblVendor.Text = customer.Employee.Profile.Name;
            Page.ViewState["SalesPersonId"] = customer.SalesPersonId;
            cboVendors.Attributes["Style"] = "display:None";
        }
        else
            pnlShowVendor.Attributes["Style"] = "display:None";
        if (customer.SupplementalSalesPersonId.HasValue)
        {

            lblSupplementalVendor.Text = customer.Employee1.Profile.Name;
            Page.ViewState["supplementalSalesPersonId"] = customer.SupplementalSalesPersonId;
            cboSupplementalVendor.Attributes["Style"] = "display:None";

        }
        else
            pnlShowSupplementalVendor.Attributes["Style"] = "display:None";
        if (customer.SalesPersonCommission.HasValue)
            ucVendorComission.CurrencyValue = customer.SalesPersonCommission;
        if (customer.SupplementalSalesPersonCommission.HasValue)
            ucSupplementalVendorComission.CurrencyValue = customer.SupplementalSalesPersonCommission;
    }
    protected void ShowCustomerConfiguration(Customer customer)
    {
        if (customer != null)
        {
            SetVendorValue(customer);
            //
            //load bank informations
            //
            cboBank.DataBind();
            cboBank.SelectedValue = customer.BankId.ToString();
            txtAccountCreatedDate.Text = customer.AccountCreatedDate.ToString();
            txtAccountNumber.Text = customer.AccountNumber;

            txtAgency.Text = customer.Agency;
        }
    }
    #endregion

    protected void odsRepresentant_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
