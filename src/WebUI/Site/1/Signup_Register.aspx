<%@ Page Language="C#" StylesheetTheme="" Inherits="Vivina.InfoControl.SystemFramework.Web.PageBase"
    AutoEventWireup="true" MasterPageFile="~/site/1/infocontrol.master" %>

<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/Address.ascx" TagName="Address" TagPrefix="uc1" %>

<script runat="server">
    protected void RegistrarEmpresa(object sender, EventArgs e)
    {
        var companyManager = new CompanyManager(this);
        var planManager = new PlanManager(this);
        var plan = planManager.GetAllPlans().Where(x => x.Name.Contains(Request["plan"])).FirstOrDefault();
        if (plan == null)
            throw new ArgumentException("O plano nao existe!");

        var company = new Company
        {
            PlanId = plan.PlanId,
            StartDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            NextDueDate = DateTime.Now.AddMonths(1),
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

        var user = new Vivina.Framework.Web.Security.DataEntities.User
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
                txtError.Text = "Sucesso";

                break;

            case InsertCompanyStatus.DuplicateCNPJ:
                txtError.Text = Resources.Exception.CNPJAlreadyExist;
                break;

            case InsertCompanyStatus.DuplicatedAdminEmail:
                txtError.Text = Resources.Exception.AdministratorEmailAlreadyExist;
                break;

            case InsertCompanyStatus.DuplicatedUserName:
                txtError.Text = Resources.Exception.AdministratorCPFAlreadyExist;
                break;

            case InsertCompanyStatus.InvalidPassword:
                txtError.Text = Resources.Exception.InvalidUserPassword;
                break;

            default:
                txtError.Text = Resources.Exception.AdministratorEmailAlreadyExist;
                break;
        }
    }
</script>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <asp:Literal runat="server" ID="txtError"></asp:Literal>
    <table>
        <tr>
            <td>
                <img id="Img1" runat="server" src="~/App_Themes/Site/Register/dados_empresa.gif"
                    alt="" /><br />
                <table>
                    <tr>
                        <td valign="top">
                            Razão Social:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtCompanyName" runat="server" Columns="30" MaxLength="50"
                                Text='' />
                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtCompanyName"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" CssClass="cErr21">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Telefone da Empresa:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtCompanyPhone" runat="server" Columns="11" MaxLength="12"
                                Text='' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCompanyPhone"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" InitialValue="(__)____-____">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptNegative="None"
                                AutoComplete="False" ClearMaskOnLostFocus="false" Mask="(99)9999-9999" TargetControlID="txtCompanyPhone">
                            </ajaxToolkit:MaskedEditExtender>
                            <br />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            CNPJ:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtCNPJ" runat="server" MaxLength="18" Text=''
                                Columns="18" />
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" ClearMaskOnLostFocus="false"
                                CultureName="pt-BR" Mask="99,999,999/9999-99" TargetControlID="txtCNPJ">
                            </ajaxToolkit:MaskedEditExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCNPJ"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" Display="Dynamic">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            <VFX:CnpjValidator ID="CnpjValidator1" runat="server" ControlToValidate="txtCNPJ"
                                CssClass="cErr21" Display="Dynamic">&nbsp;&nbsp;&nbsp;</VFX:CnpjValidator>
                            &nbsp;
                        </td>
                        <td>
                            Inscrição Estadual:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtIE" runat="server" MaxLength="20" Text='' />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="3">
                            <uc1:Address ID="adrCompanyAddress" Required="true" runat="server" FieldsetTitle="Endereço de Cobrança" />
                        </td>
                    </tr>
                </table>
                <img id="Img2" runat="server" src="~/App_Themes/Site/Register/dados_administrador.gif"
                    alt="" /><br />
                
                <table>
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtNome" runat="server" Columns="25" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtNome" runat="server" ControlToValidate="txtNome"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            CPF:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtCPF" runat="server" Columns="12" MaxLength="14"></asp:TextBox>
                            <VFX:CpfValidator ID="CpfValidatortxtCPF" runat="server" ControlToValidate="txtCPF"
                                CssClass="cErr21">&nbsp;&nbsp;&nbsp;</VFX:CpfValidator>
                            <asp:RequiredFieldValidator ID="reqtxtCPF" runat="server" ControlToValidate="txtCPF"
                                CssClass="cErr21">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="msktxtCPF" runat="server" ClearMaskOnLostFocus="false"
                                CultureName="pt-BR" Mask="999,999,999-99" TargetControlID="txtCPF">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                        <td>
                            Seu Telefone:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtPhone" runat="server" Columns="11" MaxLength="12"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPhone"
                                CssClass="cErr21" InitialValue="(__)____-____">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="msktxtTelefone" runat="server" AcceptNegative="None"
                                AutoComplete="False" ClearMaskOnLostFocus="false" Mask="(99)9999-9999" TargetControlID="txtPhone">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            E-Mail (Será enviado um e-mail de confirmação):
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtEmail" runat="server" Columns="25" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regExtxtEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="cErr21"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            Senha:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtSenha" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtSenha" runat="server" ControlToValidate="txtSenha"
                                CssClass="cErr21">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Confirmar Senha:<br />
                            <asp:TextBox CssClass="cDat11" ID="txtConfSenha" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtConfSenha" runat="server" ControlToValidate="txtConfSenha"
                                CssClass="cErr21" Display="Dynamic">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cmptxtConfSenha" runat="server" ControlToCompare="txtSenha"
                                ControlToValidate="txtConfSenha" CssClass="cErr21">&nbsp;&nbsp;&nbsp;</asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2">
                            <uc1:Address ID="adrAdminAddress" Required="true" runat="server" FieldsetTitle="Seu Endereço" />
                        </td>
                    </tr>
                </table>
                <br /><br />
                <center><asp:Button runat="server" Text="Registra Minha Empresa AGORA!" CssClass="cBtn11" OnClick="RegistrarEmpresa" /></center>
            </td>            
        </tr>
    </table>
    <VFX:BusinessManagerDataSource ID="odsUsers" runat="server" DataObjectTypeName="Vivina.Framework.Web.Security.DataEntities.User"
        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetAllUsers" TypeName="Vivina.Framework.Web.Security.UserManager"
        UpdateMethod="Update" ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}">
        <deleteparameters>
            <asp:Parameter Name="userName" Type="String" />
        </deleteparameters>
        <updateparameters>
            <asp:Parameter Name="original_x" Type="Object" />
            <asp:Parameter Name="x" Type="Object" />
        </updateparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBranches" runat="server" ConflictDetection="CompareAllValues"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAllBranches" TypeName="Vivina.InfoControl.BusinessRules.BranchManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
