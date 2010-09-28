using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using InfoControl.Web.Configuration;
using InfoControl.Web.Security;

using Vivina.Erp.BusinessRules;

public partial class Users_Login : InfoControl.Web.UI.DataUserControl
{
    private enum Actions
    {
        Login,
        Activation,
        ChangePassword,
        ShowLink,
        Wellcome
    }

    public string UserName
    {
        get { return Login.UserName; }
        set { Login.UserName = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MaintenanceMessage.Visible = Page.Application.IsMaintenance;
        LoginForm.Visible = !MaintenanceMessage.Visible;
        Welcome.Visible = Page.User.IsAuthenticated;
        actions.Visible = !Page.User.IsAuthenticated;

        if (Page.User != null && !String.IsNullOrEmpty(Request["logoff"]))
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect(Request.Url.AbsoluteUri); // Refresh
        }
        else
        {
            Login.Focus();
            lblActivationError.Visible = false;
            Login.FailureText = "Usuário e/ou<BR /> senha inválidos!";
        }
    }

    protected void btnActivation_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = (int)Actions.Activation;
    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = (int)Actions.Login;
    }
    protected void btnActivate_Click(object sender, EventArgs e)
    {
        try
        {
            MembershipManager manager = new MembershipManager(this);
            InfoControl.Web.Security.DataEntities.User user = manager.GetUser(Convert.ToInt32(ActivationCode.Text.DecryptFromHex()));
            if (user != null)
            {
                manager.UnlockUser(user.UserName);
            }
            MultiView1.ActiveViewIndex = (int)Actions.Login;
        }
        catch (Exception ex)
        {
            MultiView1.ActiveViewIndex = (int)Actions.Activation;
            lblActivationError.Text = "Código de Ativação incorreto!";
            lblActivationError.Visible = true;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = (int)Actions.Login;
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = (int)Actions.ChangePassword;
    }
    protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
    {
        e.Cancel = true;
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
        smtp.EnableSsl = (WebConfig.Web.Smtp.Network.Port == 587);
        smtp.Credentials = new System.Net.NetworkCredential(WebConfig.Web.Smtp.Network.UserName, WebConfig.Web.Smtp.Network.Password);
        smtp.Send(e.Message);
    }

    protected void Login_Error(object sender, EventArgs e)
    {
        using (var manager = new CompanyManager(null))
        {
            //
            // Verifies if the user has relationship with companyUsers table
            //
            var user = new CompanyManager(null).GetUserByUserName(Login.UserName);


            if (user == null)
            {
                Login.FailureText = "Usuário inexistente!";
                return;
            }

            if (!user.CompanyUsers.Any())
            {
                Login.FailureText = "Usuário sem empresa!";
                return;
            }

            if (user.IsOnline)
            {
                Login.FailureText = "Usuário logado <br /> em outro terminal!";
                return;
            }

            if (user.IsLockedOut)
            {
                Login.FailureText = "Usuário BLOQUEADO <br /> Contate o administrador!";
                return;
            }

            Login.FailureText = "Senha incorreta!";
        }
    }
    protected void PasswordRecovery1_UserLookupError(object sender, EventArgs e)
    {

    }

    public event LoginCancelEventHandler LoggingIn;
    protected void Login_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        if (LoggingIn != null)
            LoggingIn(sender, e);
    }

    public event EventHandler LoggedIn;
    protected void Login_LoggedIn(object sender, EventArgs e)
    {
        if (LoggedIn != null)
            LoggedIn(sender, e);
    }
}
