using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using InfoControl.Web;

public partial class AccessControl_CreateUser : InfoControl.Web.UI.DataUserControl
{
    public TextBox Bairro;
    public TextBox Email;
    public TextBox Nome;
    public TextBox CPF;
    public TextBox Telefone;
    public TextBox CEP;
    public TextBox Estado;
    public TextBox Cidade;
    public TextBox Senha;
    public TextBox Endereco;
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        Bairro = CreateUserWizard1.FindControl<TextBox>("txtBairro");
        Email = CreateUserWizard1.FindControl<TextBox>("UserName");
        Nome = CreateUserWizard1.FindControl<TextBox>("Email");
        CPF = CreateUserWizard1.FindControl<TextBox>("txtCPF");
        Telefone = CreateUserWizard1.FindControl<TextBox>("txtTelefone");
        CEP = CreateUserWizard1.FindControl<TextBox>("txtCEP");
        Estado = CreateUserWizard1.FindControl<TextBox>("txtEstado");
        Cidade = CreateUserWizard1.FindControl<TextBox>("txtCidade");
        Senha = CreateUserWizard1.FindControl<TextBox>("Password");
        Endereco = CreateUserWizard1.FindControl<TextBox>("txtEndereco");
        e.Cancel = true;
    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {

    }
}
