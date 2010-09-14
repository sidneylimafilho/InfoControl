using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class CreateUser : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        CreateUserWizard1.Email = CreateUserWizard1.UserName.ToString();
    }
    protected void ContinueButton_Click(object sender, EventArgs e)
    {

    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        InfoControl.Web.Security.MembershipManager mManager = new InfoControl.Web.Security.MembershipManager(this);
        InfoControl.Web.Security.DataEntities.User user = mManager.GetUserByName(CreateUserWizard1.UserName.ToString());

        Profile profile = new Profile();
        ProfileManager pManager = new ProfileManager(this);

        profile.Name = (CreateUserWizard1.FindControl("txtName") as TextBox).Text;
        profile.CPF = (CreateUserWizard1.FindControl("txtCPF") as TextBox).Text;
        //profile.Address = (CreateUserWizard1.FindControl("txtAddress") as TextBox).Text;
        profile.Phone = (CreateUserWizard1.FindControl("txtPhone") as TextBox).Text;
        //profile.Neighborhood = (CreateUserWizard1.FindControl("txtNeighborhood") as TextBox).Text;
        profile.PostalCode = (CreateUserWizard1.FindControl("txtPostalCode") as TextBox).Text;
        profile.ModifiedDate = DateTime.Now;        
        //profile.UserId = user.UserId;
        //profile.StateId = (CreateUserWizard1.FindControl("cboState") as DropDownList).SelectedValue;

        try
        {
            pManager.Insert(profile);
        }
        catch (Exception ex)
        {
            if (ex != null)
                return;
        }

        Context.Items.Add("UserId", user.UserId);
        Server.Transfer("User.aspx");
    }
}
