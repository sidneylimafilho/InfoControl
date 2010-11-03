using System;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class App_Shared_SelectUser : Vivina.Erp.SystemFramework.UserControlBase
{
    CompanyManager companyManager;
    ProfileManager profileManager;
    User user;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void onSelectingUser(Object sender, SelectingUserEventArgs e)
    {
        if (SelectingUser != null)
            SelectingUser(sender, e);
    }

    protected void OnSelectedUser(Object sender, SelectedUserEventArgs e)
    {
        if (SelectedUser != null)
            SelectedUser(sender, e);
    }

    protected void txtUser_TextChanged(object sender, EventArgs e)
    {
        onSelectingUser(this, new SelectingUserEventArgs() { UserName = txtUser.Text });

        if (txtUser.Text.Contains("|"))
        {
            string[] identifications = txtUser.Text.Split('|');
            string identification = identifications[0].ToString().Trim();
            companyManager = new CompanyManager(this);
            profileManager = new ProfileManager(this);

            Profile profile = profileManager.GetProfile(identification);

            if (profile != null)
                user = companyManager.GetUserByProfile(profile.ProfileId);

            ShowUser(user);
            //ShowEmployee(employee);
        }

    }
#warning implementar uma propriedade para definir se os dados devem ser visualizados
    public void ShowUser(User user)
    {
        

        txtUser.Text = String.Empty;
        OnSelectedUser(this, new SelectedUserEventArgs() { User = user });
    }
    public event EventHandler<SelectingUserEventArgs> SelectingUser;
    public event EventHandler<SelectedUserEventArgs> SelectedUser;




}

public class SelectingUserEventArgs : EventArgs
{
    public String UserName { get; set; }
}

public class SelectedUserEventArgs : EventArgs
{
    public User User { get; set; }
}

