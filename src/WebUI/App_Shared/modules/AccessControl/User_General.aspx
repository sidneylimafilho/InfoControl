<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" Inherits="User_General"
    MasterPageFile="~/infocontrol/Default.master" Title="Cadastro de Usuário" CodeBehind="User_General.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Profile_NaturalPerson.ascx" TagName="Profile_NaturalPerson"
    TagPrefix="uc2" %>
<%@ Register Src="~/InfoControl/RH/SelectEmployee.ascx" TagName="SelectEmployee"
    TagPrefix="uc3" %>
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
                <uc2:Profile_NaturalPerson ID="ucProfile" ValidationGroup="save" runat="server" />
                <br />
                <fieldset>
                    <legend>Dados do Usuário:</legend>
                    <table width="60%">
                        <tr>
                            <td>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="txtUserName" CssClass="cLab11">E-mail:</asp:Label>
                                <br />
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" Columns="30"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="CreateUserWizard1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtUserName"
                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ToolTip="E-mail obrigatório."
                                    ValidationGroup="save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlPasswords" Visible="false" runat="server">
                        <table width="60%">
                            <tr>
                                <td>
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="txtPassword" CssClass="cLab11">Senha:</asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                        CssClass="cErr21" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ToolTip="Senha obrigatória."
                                        ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="txtConfirmPassword"
                                        CssClass="cLab11">Confirma Senha:</asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="txtConfirmPassword"
                                        CssClass="cErr21" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ToolTip="Senha obrigatória."
                                        ValidationGroup="save" Width="16px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="color: red; text-align: center">
                                    <br />
                                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword"
                                        ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="A senha e o confirma senha não são iguais!"
                                        ValidationGroup="save"></asp:CompareValidator><br />
                                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table width="60%">
                        <tr>
                            <td>
                                Acessa o estoque:<br />
                                <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                    DataValueField="DepositId" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Representante:
                                <br />
                                <asp:DropDownList runat="server" DataTextField="Name" DataSourceID="odsRepresentant"
                                    DataValueField="RepresentantId" ID="cboRepresentant" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="">    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsActiveCheckBox" runat="server" Checked='<%# Bind("IsActive") %>'
                                    CssClass="cChk11" Text=" Ativo ?" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap">
                                <asp:CheckBox ID="chkHasChangePassword" runat="server" Checked='<%# Bind("HasChangePassword")%>'
                                    CssClass="cChk11" Text=" Trocar senhar no proximo login ?" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsLockedOut" runat="server" Checked='<%# Bind("IsLockedOut")%>'
                                    CssClass="cChk11" Text=" Bloqueado ?" /><br />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnSave" CssClass="cBtn11" runat="server" Text="Salvar" OnClick="btnSave_Click"
                        ValidationGroup="save" _permissionRequired="Users" />&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" CssClass="cBtn11" runat="server" Text="Cancelar" CausesValidation="False"
                        OnClick="btnCancel_Click" />
                </div>
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
    <script>

        $(function () {
            var txtCpf = "ctl00_ContentPlaceHolder_ucProfile_txtCPF";
            $(txtCpf).attr("readOnly", "false");
        });
    
    </script>
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" SelectMethod="GetDepositByCompany"
        TypeName="Vivina.Erp.BusinessRules.DepositManager" OnSelecting="odsDeposit_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsRepresentant" runat="server" TypeName="Vivina.Erp.BusinessRules.RepresentantManager"
        SelectMethod="GetRepresentantsByCompany" onselecting="odsRepresentant_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
         </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
