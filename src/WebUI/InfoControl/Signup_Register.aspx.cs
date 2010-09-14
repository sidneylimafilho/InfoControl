using System;
using System.Linq;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using System.Web.Services;


public partial class Signup_Register : Vivina.Erp.SystemFramework.PageBase
{
    protected void RegistrarEmpresa(object sender, EventArgs e)
    {
        var companyManager = new CompanyManager(this);
        var planManager = new PlanManager(this);

        var plan = planManager.GetAllPlans().Where(x => x.Name.Contains(Request["plan"])).FirstOrDefault();
        if (plan == null)
            throw new ArgumentException("O plano não existe!");

        var company = new Company
        {
            PlanId = plan.PlanId,
            StartDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            NextStatementDueDate = DateTime.Now.AddMonths(1),
            LegalEntityProfile = new LegalEntityProfile
            {
                CNPJ = txtCNPJ.Text,
                CompanyName = txtCompanyName.Text,
                Phone = txtCompanyPhone.Text,
                PostalCode = adrCompanyAddress.PostalCode,
                AddressComp = adrCompanyAddress.AddressComp,
                AddressNumber = adrCompanyAddress.AddressNumber,
                IE = txtIE.Text
            }
        };

        var user = new InfoControl.Web.Security.DataEntities.User
        {
            UserName = txtEmail.Text,
            Email = txtEmail.Text,
            Password = txtSenha.Text,
            PasswordAnswer = txtSenha.Text,
            CreationDate = DateTime.Now,
            LastLockoutDate = DateTime.Now,
            LastLoginDate = DateTime.Now,
            LastPasswordChangedDate = DateTime.Now
        };

        var profile = new Profile
        {
            CPF = txtCPF.Text,
            Name = txtNome.Text,
            ModifiedDate = DateTime.Now,
            Phone = txtPhone.Text,
            PostalCode = adrAdminAddress.PostalCode,
            AddressComp = adrAdminAddress.AddressComp,
            AddressNumber = adrAdminAddress.AddressNumber
        };

        InsertCompanyStatus status = companyManager.InsertMatrixCompany(company, user, profile);

        switch (status)
        {
            case InsertCompanyStatus.Success:
                //txtError.Text = "<b> <font color='red'> Empresa registrada com sucesso!  </font> </b>";
                pnlFormRegister.Visible = false;
                successMessage.Visible = true;
                break;

            case InsertCompanyStatus.DuplicateCNPJ:
                txtError.Text = "<b> <font color='red'>" + Resources.Exception.CNPJAlreadyExist + " </font> </b>";
                break;

            case InsertCompanyStatus.DuplicatedAdminEmail:
                txtError.Text = "<b> <font color='red'>" + Resources.Exception.AdministratorEmailAlreadyExist + " </font> </b>";
                break;

            case InsertCompanyStatus.DuplicatedUserName:
                txtError.Text = "<b> <font color='red'>" + Resources.Exception.AdministratorCPFAlreadyExist + " </font> </b>";
                break;

            case InsertCompanyStatus.InvalidPassword:
                txtError.Text = "<b> <font color='red'>" + Resources.Exception.InvalidUserPassword + " </font> </b>";
                break;

            default:
                txtError.Text = "<b> <font color='red'>" + Resources.Exception.AdministratorEmailAlreadyExist + " </font> </b>";
                break;
        }
    }




    [WebMethod]
    public static void RegisterCompany(string[] company)
    {
        /* - passar as informações da tela(dados da compania, usuário e perfil) como três vetores de string 
           - montar os objetos
         */

        //  var user= new InfoControl.Web.Security.DataEntities.User{ }    
    }
}

