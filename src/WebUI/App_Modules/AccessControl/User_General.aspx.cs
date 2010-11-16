using System;
using System.Web.UI.WebControls;
using InfoControl.Web.Security;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using Exception = Resources.Exception;
using User = InfoControl.Web.Security.DataEntities.User;
using InfoControl;

public partial class User_General : Vivina.Erp.SystemFramework.PageBase
{

    #region Useful fields and properties

    private CompanyManager _companyManager;
    private MembershipManager _membershipManager;
    private ProfileManager _profileManager;
    private RepresentantManager _representantManager;


    public ProfileManager ProfileManager
    {
        get
        {
            if (_profileManager == null)
                _profileManager = new ProfileManager(this);

            return _profileManager;
        }
    }
    public MembershipManager MembershipManager
    {
        get
        {
            if (_membershipManager == null)
                _membershipManager = new MembershipManager(this);

            return _membershipManager;
        }
    }
    public CompanyManager CompanyManager
    {
        get
        {
            if (_companyManager == null)
                _companyManager = new CompanyManager(this);

            return _companyManager;
        }
    }
    public RepresentantManager RepresentantManager
    {
        get
        {
            if (_representantManager == null)
                _representantManager = new RepresentantManager(this);

            return _representantManager;
        }
    }

    #endregion


    #region functions


    private void LoadUser()
    {

        User user = MembershipManager.GetUser(Convert.ToInt32(Page.ViewState["UserId"]));

        // load the profile data
        ucProfile.ProfileEntity = ProfileManager.GetProfileByUser(Convert.ToInt32(Page.ViewState["UserId"]));

        chkHasChangePassword.Checked = user.HasChangePassword;

        var representantUser = RepresentantManager.GetRepresentantUser(Convert.ToInt32(Page.ViewState["UserId"]));

        if (representantUser != null)
            cboRepresentant.SelectedValue = Convert.ToString(representantUser.RepresentantId);

        chkIsActiveCheckBox.Checked = user.IsActive;
        chkIsLockedOut.Checked = user.IsLockedOut;
        txtUserName.Text = user.UserName;
        txtPassword.ValidationGroup = "dummy";
        txtConfirmPassword.ValidationGroup = "dummy";

        Deposit deposit = CompanyManager.GetCurrentDeposit(Convert.ToInt32(Page.ViewState["UserId"]), Company.CompanyId);

        if (deposit != null)
            cboDeposit.SelectedValue = deposit.DepositId.ToString();
    }

    private User SaveUser()
    {
        //
        // cria o objeto usuário que irá receber os dados vindo da tela
        //
        var user = new User
        {
            UserName = txtUserName.Text,
            Email = txtUserName.Text,
            Password = txtPassword.Text,
            PasswordAnswer = txtPassword.Text,
            CreationDate = DateTime.Now,
            LastActivityDate = DateTime.Now,
            LastLockoutDate = DateTime.Now,
            LastLoginDate = DateTime.Now,
            LastPasswordChangedDate = DateTime.Now
        };

        if (ViewState["EmployeeProfileId"] != null)
            user.ProfileId = Convert.ToInt32(ViewState["EmployeeProfileId"]);

        user.HasChangePassword = chkHasChangePassword.Checked;
        user.IsActive = chkIsActiveCheckBox.Checked;
        user.IsLockedOut = chkIsLockedOut.Checked;

        return user;
    }

    #endregion

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        (ucProfile.FindControl("txtCPF") as TextBox).Enabled = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ViewState["FilledControl"] = false;
        if (!String.IsNullOrEmpty(Request["UserId"]))
        {
            Title = "";
            Page.ViewState["UserId"] = Convert.ToInt32(Request["UserId"]);

            if (!IsPostBack)
                LoadUser();
        }
        else
        {
            chkIsActiveCheckBox.Checked = true;
            pnlPasswords.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int? depositId = null;
        int? representantId = null;

        if (!String.IsNullOrEmpty(cboDeposit.SelectedValue))
            depositId = Convert.ToInt32(cboDeposit.SelectedValue);

        if (!String.IsNullOrEmpty(cboRepresentant.SelectedValue))
            representantId = Convert.ToInt32(cboRepresentant.SelectedValue);

        //
        //Update User
        //
        if (!String.IsNullOrEmpty(Request["UserId"]))
        {
            var originalUser = CompanyManager.GetUser(Company.CompanyId, Convert.ToInt32(Page.ViewState["UserId"]));

            if (originalUser.UserName != txtUserName.Text && CompanyManager.ExistsUserInCompany(Company.CompanyId, txtUserName.Text))
            {
                ShowError(Exception.ExistentMail);
                return;
            }

            CompanyManager.UpdateUser(Company.CompanyId, Convert.ToInt32(Page.ViewState["UserId"]), depositId, representantId, ucProfile.ProfileEntity, SaveUser());

            RefreshDeposit();
            RefreshCredentials();
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "", "parent.location='Users.aspx'", true);
            return;
        }

        //
        // Verifies if the User already exist in company
        //

        if (CompanyManager.ExistsUserInCompany(Company.CompanyId, txtUserName.Text))
        {
            ShowError(Exception.ExistentMail);
            return;
        }

        var oldUser = MembershipManager.GetUserByName(txtUserName.Text);

        if (oldUser != null)
        {
            CompanyManager.UpdateUser(Company.CompanyId, oldUser.UserId, depositId, representantId, ucProfile.ProfileEntity, SaveUser());

            RefreshCredentials();
            Response.Redirect("User.aspx?UserId=" + oldUser.UserId);
            return;
        }

        //
        // Insert user
        //

        var newUser = SaveUser();
        InsertCompanyStatus status = CompanyManager.InsertUser(Company.CompanyId, depositId, representantId,

newUser, ucProfile.ProfileEntity);

        switch (status)
        {
            case InsertCompanyStatus.InvalidPassword:
                ShowError(Exception.InvalidPassword);
                break;
            case InsertCompanyStatus.InvalidUser:
            case InsertCompanyStatus.DuplicateCNPJ:
            case InsertCompanyStatus.DuplicatedAdminEmail:
            case InsertCompanyStatus.DuplicatedUserName:
                ShowError(Exception.ExistentMail);
                break;
            case InsertCompanyStatus.Success:

                Response.Redirect("User.aspx?UserId=" + newUser.UserId);
                break;
        }
        RefreshCredentials();
    }

    protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Page.ViewState["UserId"]) == 0)
            Response.Redirect("Users.aspx");
        else
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "", "parent.location='Users.aspx'", true);
    }

    protected void SelEmployee_SelectedEmployee(object sender, SelectedEmployeeEventArgs e)
    {
        if (e.Employee != null)
        {
            ViewState["EmployeeProfileId"] = e.Employee.ProfileId;

            var user = CompanyManager.GetUserByProfile(e.Employee.ProfileId);
            if (user != null)
            {
                txtUserName.Text = user.UserName;
                txtUserName.Enabled = false;
            }
        }
    }

    protected void odsRepresentant_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
