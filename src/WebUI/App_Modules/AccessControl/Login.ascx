<%@ Control Language="C#" AutoEventWireup="true" Inherits="Users_Login" CodeBehind="Login.ascx.cs" %>
<div id="banner">
    <div class="multiview" id="LoginForm" runat="server">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <asp:Login CssClass="login" ID="Login" runat="server" Width="100%" DestinationPageUrl="~/infocontrol/"
                    OnLoginError="Login_Error" OnLoggedIn="Login_LoggedIn" OnLoggingIn="Login_LoggingIn"
                    VisibleWhenLoggedIn="False">
                    <LayoutTemplate>
                        <table border="0" cellpadding="2" width="100%">
                            <tr>
                                <td align="left" valign="middle" width="1%" style="white-space: nowrap">
                                    <div class="username">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">E-mail:</asp:Label>
                                        <asp:TextBox ID="UserName" runat="server" CssClass="cDat11" TabIndex="1" Columns="30"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                            ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                            ToolTip="User Name is required." ValidationGroup="Login1" CssClass="cErr21" Display="Dynamic">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="password">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Senha:</asp:Label>
                                        <asp:TextBox ID="Password" runat="server" CssClass="cDat11" TextMode="Password" TabIndex="2"
                                            Columns="30"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="PasswordRequired"
                                                runat="server" ControlToValidate="Password" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                                ToolTip="Password is required." Display="Dynamic" ValidationGroup="Login1" CssClass="cErr21">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td align="center" valign="middle" rowspan="2" style="white-space: nowrap">
                                    <p>
                                        <asp:Label ID="FailureText" runat="server" CssClass="cErr11"></asp:Label>
                                    </p>
                                    <asp:Button ID="LoginButton" runat="server" CssClass="cBtn11" CommandName="Login"
                                        Text="Entrar" ValidationGroup="Login1" />
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:Login>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div class="actions">
                    <asp:Label ID="ActivationCodeLabel" runat="server" AssociatedControlID="ActivationCode">Informe o código de ativação:</asp:Label>
                    <br />
                    <asp:Label ID="lblActivationError" runat="server" CssClass="cErr11"></asp:Label>
                    <br />
                    <asp:TextBox ID="ActivationCode" runat="server" Width="150"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                        ID="ActivationCodeRequired" runat="server" ControlToValidate="ActivationCode"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ToolTip="ActivationCode is required."
                        CssClass="cErr21" ValidationGroup="Login1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator><asp:Button
                            ID="btnActivate" ValidationGroup="Login1" runat="server" Text="Ativar" OnClick="btnActivate_Click" />
                </div>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" GeneralFailureText="Não foi possível completar&lt;br /&gt; a operação. Tente novamente"
                    SuccessText="Sua nova senha foi enviada para o seu e-mail! Favor verifique sua caixa postal e também sua caixa de SPAM!"
                    UserNameFailureText="E-mail não encontrado" UserNameInstructionText="Digite o seu e-mail, para enviarmos uma nova senha"
                    UserNameLabelText="E-Mail:" UserNameRequiredErrorMessage="O e-mail é necessário"
                    UserNameTitleText="Esqueceu sua senha ?" OnSendingMail="PasswordRecovery1_SendingMail"
                    OnUserLookupError="PasswordRecovery1_UserLookupError" CssClass="recuperarSenha">
                    <MailDefinition IsBodyHtml="True" Subject="Troca de Senha do InfoControl!" BodyFileName="~/App_Modules/AccessControl/PasswordRecoveryMessageBody.htm">
                    </MailDefinition>
                    <UserNameTemplate>
                        <table border="0" cellpadding="2" width="100%">
                            <tr>
                                <td align="left" valign="top">
                                    Digite o seu e-mail para enviarmos uma nova senha&nbsp;<br />
                                    <asp:Label ID="FailureText" runat="server" EnableViewState="False" CssClass="cErr11"></asp:Label><br />
                                    <asp:TextBox ID="UserName" runat="server" Width="150"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                        ToolTip="O e-mail é necessário" ValidationGroup="PasswordRecovery1" CssClass="cErr21">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                                    <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Enviar" ValidationGroup="PasswordRecovery1" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </UserNameTemplate>
                </asp:PasswordRecovery>
            </asp:View>
            <asp:View runat="server">
                <table cellspacing="5" border="0">
                    <tr>
                        <td>
                            Aplicação temporariamente em Manutençao!
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
        <table cellspacing="5" width="100%" border="0" id="Welcome" runat="server" class="actions">
            <tr>
                <td align="center">
                    Seja bem vindo!
                </td>
            </tr>
            <tr>
                <td align="center">
                    <a href='~/infocontrol' runat="server">Entrar no InfoControl</a> &nbsp;|&nbsp; <a
                        href='~/logoff.aspx' runat="server">Sair</a>
                </td>
            </tr>
        </table>
        <table cellspacing="5" border="0" id="MaintenanceMessage" runat="server">
            <tr>
                <td>
                    Aplicação temporariamente em Manutençao!
                </td>
            </tr>
        </table>
        <div id="actions" runat="server" class="actions">
            <asp:LinkButton ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click">Esqueci minha senha</asp:LinkButton>
            &nbsp;|&nbsp;
            <asp:LinkButton ID="btnActivation" runat="server" OnClick="btnActivation_Click">Ativar cadastro</asp:LinkButton>
            &nbsp;|&nbsp;
            <asp:LinkButton ID="CancelButton" runat="server" OnClick="CancelButton_Click">Ir para Login</asp:LinkButton>
        </div>
    </div>
</div>
