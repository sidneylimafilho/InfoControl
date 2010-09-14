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
        txtUser.Attributes["servicepath"] = ResolveUrl("~/Controller/SearchService/SearchUser");
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
        //if (user != null && user.Profile != null)
        //{


        //    lblUserAddress.Text = user.Profile.Address.Name
        //        + ", " + user.Profile.AddressNumber
        //        + ", " + user.Profile.AddressComp;

        //    lblUserLocalization.Text = user.Profile.Address.City
        //        + " - " + user.Profile.Address.Neighborhood
        //        + ", " + user.Profile.Address.StateId;

        //    lblPostalCode.Text = user.Profile.PostalCode;
        //    lnkUserName.Text = user.Profile.Name;
        //    lblUserPhone.Text = "Tel: " + user.Profile.Phone.Replace("(__)____-____", "");
        //    lblCPF.Text = user.Profile.CPF;

        //    lnkUserName.OnClientClick = "top.tb_show('Cadastro de usuário','App_AcessControl/User.aspx?UserId=" + user.UserId.ToString().Encrypt(EncodingType.BinHex) + "');return;";
        //    pnlUser.Visible = true;
        //}

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

