<%@ Control Language="C#" AutoEventWireup="true" Inherits="AccessControl_CreateUser" Codebehind="CreateUser.ascx.cs" %>

<asp:CreateUserWizard ID="CreateUserWizard1" runat="server" AnswerLabelText="Resposta Secreta:"
    AnswerRequiredErrorMessage="Resposta Secreta obrigatória." CancelButtonText="Cancelar"
    CompleteSuccessText="Usuário criado com sucesso." ConfirmPasswordCompareErrorMessage="A Senha e a Confirma Senha não são iguais"
    ConfirmPasswordLabelText="Confirma Senha:" ConfirmPasswordRequiredErrorMessage="Senha obrigatória."
    ContinueButtonText="Continuar" CreateUserButtonText="Criar Usuário" DuplicateEmailErrorMessage="E-mail já existente. Por favor entre com E-mail diferente."
    DuplicateUserNameErrorMessage="Usuário já existente. Por favor entre com nome de Usuário diferente."
    EmailRegularExpressionErrorMessage="Por favor entre com outro e-mail." EmailRequiredErrorMessage="E-mail obrigatório."
    FinishCompleteButtonText="Finalizar" FinishPreviousButtonText="Voltar" InvalidAnswerErrorMessage="Por favor entre com uma resposta secreta diferente."
    InvalidEmailErrorMessage="E-mail Inválido." InvalidQuestionErrorMessage="Por favor entre com uma pergunta secreta diferente."
    MembershipProvider="InfoControlMembershipProvider" PasswordLabelText="Senha:" PasswordRegularExpressionErrorMessage="Por favor entre com outra Senha."
    PasswordRequiredErrorMessage="Senha obrigatória." QuestionLabelText="Pergunta Secreta:"
    QuestionRequiredErrorMessage="Pergunta Secreta obrigatória." StartNextButtonText="Avançar"
    StepNextButtonText="Avançar" StepPreviousButtonText="Voltar" UnknownErrorMessage="Sua Usuário não foi criado. Por favor tente novametne."
    UserNameLabelText="Usuário:" UserNameRequiredErrorMessage="Usuário obrigatório."
    InvalidPasswordErrorMessage="Número minimo de caracteres: {0}. Caractér não alfanumérico requerido: {1}."
    CssClass="cLab11" OnCreatingUser="CreateUserWizard1_CreatingUser" OnCreatedUser="CreateUserWizard1_CreatedUser">
    <CreateUserButtonStyle CssClass="cBtn11" />
    <ContinueButtonStyle CssClass="cBtn11" />
    <CancelButtonStyle CssClass="cBtn11" />
    <TitleTextStyle CssClass="cLab11" />
    <WizardSteps>
        <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
            <ContentTemplate>
                <table border="0">
                    <tr>
                        <td align="center" class="cTxt11b" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        Nome:<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="Email"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator><br />
                                        <asp:TextBox ID="Email" runat="server" Columns="25" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="cLab11">E-mail:</asp:Label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="E-mail obrigatório." ValidationGroup="CreateUserWizard1"></asp:RequiredFieldValidator><br />
                                        <asp:TextBox ID="UserName" runat="server" CssClass="cDat11"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Endereço:<br />
                                        <asp:TextBox ID="txtEndereco" runat="server" Columns="70" MaxLength="100"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        CPF:<br />
                                        <asp:TextBox ID="txtCPF" runat="server" MaxLength="14"></asp:TextBox>
                                        <vfx:CpfValidator ID="CpfValidator1" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                            ControlToValidate="txtCPF">
                                        </vfx:CpfValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCPF"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        Telefone:<br />
                                        <asp:TextBox ID="txtTelefone" runat="server" MaxLength="20"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        CEP:<br />
                                        <asp:TextBox ID="txtCEP" runat="server" MaxLength="9"></asp:TextBox></td>
                                    <td>
                                        Cidade:<br />
                                        <asp:TextBox ID="txtCidade" runat="server" Columns="25" MaxLength="50"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Estado:<br />
                                        <asp:TextBox ID="txtEstado" runat="server" Columns="25" MaxLength="50"></asp:TextBox></td>
                                    <td>
                                        Bairro:<br />
                                        <asp:TextBox ID="txtBairro" runat="server" Columns="25" MaxLength="50"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="cLab11">Senha:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="Password" runat="server" CssClass="cDat11" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="Senha obrigatória." ValidationGroup="CreateUserWizard1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword"
                                            CssClass="cLab11">Confirma Senha:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="cDat11" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="Senha obrigatória." ValidationGroup="CreateUserWizard1"
                                            Width="16px"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="Question" runat="server" Visible="False" Wrap="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="Answer" runat="server" Visible="False" Wrap="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" style="color: red">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CreateUserWizard1" />
                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:CreateUserWizardStep>
        <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
            <ContentTemplate>
                <table border="0" style="font-size: 100%;">
                    <tr>
                        <td align="center" class="cLab11" colspan="2">
                            Complete</td>
                    </tr>
                    <tr>
                        <td>
                            Usuário criado com sucesso.</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
                                CssClass="cBtn11" Text="Continuar" ValidationGroup="CreateUserWizard1" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:CompleteWizardStep>
    </WizardSteps>
</asp:CreateUserWizard>
