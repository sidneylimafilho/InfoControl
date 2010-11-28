<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="CreateUser" Title="Criação de Usuário" CodeBehind="CreateUser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;
            </td> 
            <td class="center">
                <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" AnswerLabelText="Resposta Secreta:"
                    AnswerRequiredErrorMessage="Resposta Secreta obrigatória." CancelButtonText="Cancelar"
                    CompleteSuccessText="Usuário criado com sucesso." ConfirmPasswordCompareErrorMessage="A Senha e a Confirma Senha não são iguais"
                    ConfirmPasswordLabelText="Confirma Senha:" ConfirmPasswordRequiredErrorMessage="Senha obrigatória."
                    ContinueButtonText="Continuar" CreateUserButtonText="Criar Usuário" DuplicateEmailErrorMessage="E-mail já existente. Por favor entre com E-mail diferente."
                    DuplicateUserNameErrorMessage="E-mail já existente. Por favor entre com E-mail diferente."
                    EmailRegularExpressionErrorMessage="O E-mail deve seguir o padrão nome @ domínio . localidade"
                    EmailRequiredErrorMessage="E-mail obrigatório." FinishCompleteButtonText="Finalizar"
                    FinishPreviousButtonText="Voltar" InvalidAnswerErrorMessage="Por favor entre com uma resposta secreta diferente."
                    InvalidEmailErrorMessage="E-mail Inválido." InvalidQuestionErrorMessage="Por favor entre com uma pergunta secreta diferente."
                    MembershipProvider="InfoControlMembershipProvider" PasswordLabelText="Senha:" PasswordRegularExpressionErrorMessage="Por favor entre com outra Senha."
                    PasswordRequiredErrorMessage="Senha obrigatória." QuestionLabelText="Pergunta Secreta:"
                    QuestionRequiredErrorMessage="Pergunta Secreta obrigatória." StartNextButtonText="Avançar"
                    StepNextButtonText="Avançar" StepPreviousButtonText="Voltar" UnknownErrorMessage="Seu usuário não foi criado. Por favor tente novametne."
                    UserNameLabelText="Usuário:" UserNameRequiredErrorMessage="Usuário obrigatório."
                    InvalidPasswordErrorMessage="Número minimo de caracteres: {0}. Caractér não alfanumérico requerido: {1}."
                    CssClass="cLab11" OnCreatingUser="CreateUserWizard1_CreatingUser" OnCreatedUser="CreateUserWizard1_CreatedUser"
                    RequireEmail="False" Width="100%">
                    <ContinueButtonStyle CssClass="cBtn11" />
                    <CreateUserButtonStyle CssClass="cBtn11" />
                    <TitleTextStyle CssClass="cLab11" />
                    <CancelButtonStyle CssClass="cBtn11" />
                    <WizardSteps>
                        <asp:CreateUserWizardStep runat="server">
                            <ContentTemplate>
                                <table border="0" width="100%">
                                    <tr>
                                        <td colspan="2" style="color: red; text-align: center">
                                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                                ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CreateUserWizard1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="cLab11">E-mail:</asp:Label>
                                            <br />
                                            <asp:TextBox ID="UserName" runat="server" Columns="30"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="UserName"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="UserName"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="E-mail obrigatório."
                                                ValidationGroup="CreateUserWizard1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="cLab11">Senha:</asp:Label>
                                            <br />
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="Senha obrigatória."
                                                ValidationGroup="CreateUserWizard1"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword"
                                                CssClass="cLab11">Confirma Senha:</asp:Label>
                                            <br />
                                            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="Senha obrigatória."
                                                ValidationGroup="CreateUserWizard1" Width="16px"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Nome:<br />
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            CPF:<br />
                                            <asp:TextBox ID="txtCPF" runat="server" Columns="15"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" ClearMaskOnLostFocus="False"
                                                Mask="999.999.999-99" MaskType="Number" TargetControlID="txtCPF">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <vfx:CpfValidator ID="CpfValidator1" runat="server" ControlToValidate="txtCPF" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                                CssClass="cErr21"> 											
                                            </vfx:CpfValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Endereço:<br />
                                            <asp:TextBox ID="txtAddress" runat="server" Columns="50"></asp:TextBox>
                                        </td>
                                        <td>
                                            Telefone:<br />
                                            <asp:TextBox ID="txtPhone" runat="server" Columns="14"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="False"
                                                Mask="(99)9999-9999" MaskType="Number" TargetControlID="txtPhone">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Cidade:<br />
                                            <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            Estado:<br />
                                            <asp:DropDownList ID="cboStates" runat="server">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Value="Acre">AC</asp:ListItem>
                                                <asp:ListItem Value="Alagoas">AL</asp:ListItem>
                                                <asp:ListItem Value="Amapá">AP</asp:ListItem>
                                                <asp:ListItem Value="Amazonas">AM</asp:ListItem>
                                                <asp:ListItem Value="Bahia">BA</asp:ListItem>
                                                <asp:ListItem Value="Ceará">CE</asp:ListItem>
                                                <asp:ListItem Value="Distrito Federal">DF</asp:ListItem>
                                                <asp:ListItem Value="Goiás">GO</asp:ListItem>
                                                <asp:ListItem Value="Espírito Santo">ES</asp:ListItem>
                                                <asp:ListItem Value="Maranhão">MA</asp:ListItem>
                                                <asp:ListItem Value="Mato Grosso">MT</asp:ListItem>
                                                <asp:ListItem Value="Mato Grosso do Sul">MS</asp:ListItem>
                                                <asp:ListItem Value="Minas Gerais">MG</asp:ListItem>
                                                <asp:ListItem Value="Pará">PA</asp:ListItem>
                                                <asp:ListItem Value="Paraiba">PB</asp:ListItem>
                                                <asp:ListItem Value="Paraná">PR</asp:ListItem>
                                                <asp:ListItem Value="Pernambuco">PE</asp:ListItem>
                                                <asp:ListItem Value="Piauí">PI</asp:ListItem>
                                                <asp:ListItem Value="Rio de Janeiro">RJ</asp:ListItem>
                                                <asp:ListItem Value="Rio Grande do Norte">RN</asp:ListItem>
                                                <asp:ListItem Value="Rio Grande do Sul">RS</asp:ListItem>
                                                <asp:ListItem Value="Rondônia">RO</asp:ListItem>
                                                <asp:ListItem Value="Rorâima">RR</asp:ListItem>
                                                <asp:ListItem Value="São Paulo">SP</asp:ListItem>
                                                <asp:ListItem Value="Santa Catarina">SC</asp:ListItem>
                                                <asp:ListItem Value="Sergipe">SE</asp:ListItem>
                                                <asp:ListItem Value="Tocantins">TO</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboStates"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bairro:<br />
                                            <asp:TextBox ID="txtNeighborhood" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            CEP:<br />
                                            <asp:TextBox ID="txtPostalCode" runat="server" Columns="10"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="False"
                                                Mask="99999-999" MaskType="Number" TargetControlID="txtPostalCode">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="Question" runat="server" Visible="False" Wrap="False"></asp:TextBox>
                                            <asp:TextBox ID="Answer" runat="server" Visible="False" Wrap="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CreateUserWizardStep>
                        <asp:CompleteWizardStep runat="server">
                            <ContentTemplate>
                                <table border="0" width="100%">
                                    <tr>
                                        <td class="cLab11" colspan="2" style="text-align: center;">
                                            Finalizado
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: ">
                                            Usuário criado com sucesso.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: right;">
                                            <asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
                                                CssClass="cBtn11" OnClick="ContinueButton_Click" Text="Continuar" ValidationGroup="CreateUserWizard1" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                </asp:CreateUserWizard>
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
