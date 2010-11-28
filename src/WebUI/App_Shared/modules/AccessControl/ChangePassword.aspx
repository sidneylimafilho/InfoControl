<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="ChangePassword" Title=" Alterar Senha" CodeBehind="ChangePassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21">
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
                <asp:ChangePassword ID="ChangePassword2" runat="server" CancelButtonText="Cancelar"
                    ChangePasswordButtonText="Trocar" ConfirmNewPasswordLabelText="Confirmar nova senha"
                    ChangePasswordTitleText="Trocar Senha" ConfirmPasswordCompareErrorMessage="A Nova Senha e a Confirmação de Senha estã diferentes. "
                    ConfirmPasswordRequiredErrorMessage="Confirmação de Senha obrigatória." NewPasswordLabelText="Nova Senha:"
                    NewPasswordRegularExpressionErrorMessage="Por favor entre com uma senha diferente."
                    NewPasswordRequiredErrorMessage="Nova Senha obrigatória." PasswordLabelText="Senha:"
                    PasswordRequiredErrorMessage="Senha obrigatória." SuccessText="Torca de Senha obrigatória!"
                    SuccessTitleText="Troca de Senha efetuada com sucesso!" UserNameLabelText="Usuário:"
                    UserNameRequiredErrorMessage="Usuário obrigatório.." MembershipProvider="InfoControlMembershipProvider"
                    DisplayUserName="true" 
                    ChangePasswordFailureText="Senha inválida. Mínimo de caracteres: {0}. Caracteres não alfanuméricos obrigatórios: {1}.">
                    <ChangePasswordTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuário:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="UserName" runat="server" MaxLength="50" Columns="20"></asp:TextBox> 
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    ErrorMessage="Usuário obrigatório.." ToolTip="Usuário obrigatório.." ValidationGroup="ChangePassword2">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Senha:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                                    ErrorMessage="Senha obrigatória." ToolTip="Senha obrigatória." ValidationGroup="ChangePassword2">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">Nova Senha:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                                    ErrorMessage="Nova Senha obrigatória." ToolTip="Nova Senha obrigatória." ValidationGroup="ChangePassword2">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirmar nova senha</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                                    ErrorMessage="Confirmação de Senha obrigatória." ToolTip="Confirmação de Senha obrigatória."
                                                    ValidationGroup="ChangePassword2">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                                    ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="A Nova Senha e a Confirmação de Senha estã diferentes. "
                                                    ValidationGroup="ChangePassword2"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
                                                    Text="Trocar" ValidationGroup="ChangePassword2" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ChangePasswordTemplate>
                    <SuccessTemplate>
                        <table border="0" cellpadding="10" cellspacing="0" style="width: 250px; border-collapse: collapse;">
                            <tr>
                                <td align="center" colspan="2">
                                    Troca de Senha efetuada com sucesso!
                                </td>
                            </tr>
                        </table>
                    </SuccessTemplate>
                </asp:ChangePassword>
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
