<%@ Page Title="" Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    CodeBehind="Checkout_Identification.aspx.cs" Inherits="Vivina.Erp.WebUI.Site.CheckoutIdentification" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Src="~/InfoControl/Profile_NaturalPerson.ascx" TagName="Profile_NaturalPerson"
    TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Profile.ascx" TagName="Profile" TagPrefix="uc2" %>
<%@ Register Src="~/InfoControl/Profile_LegalEntity.ascx" TagName="Profile_LegalEntity"
    TagPrefix="uc3" %>
<%@ Register Src="~/App_Modules/AccessControl/Login.ascx" TagName="Login" TagPrefix="uc4" %>
<%@ Register Src="~/Site/Checkout_Steps.ascx" TagName="CheckoutSteps" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="cadastro">
        <uc1:CheckoutSteps runat="server" />
        <span class="error">
            <asp:Label ID="lblMessage" runat="server" CssClass="erroMessage"></asp:Label>
        </span>
        <asp:Panel runat="server" ID="pnlLogin">
            <uc4:Login ID="Login" runat="server" OnLoggedIn="Login_LoggedIn" />
            <br />
            <br />
            <hr />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRegister">
            <h1>
                Cadastro de Usuário
            </h1>
            <uc2:Profile ID="ucProfiles" runat="server" ValidationGroup="Save" />
            <table width="100%" class="tableStyle">
                <tr>
                    <td>Email do Usuário:
                        <br />
                        <asp:TextBox ID="txtUserEmail" runat="server" Columns="30" MaxLength="256">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtUserEmail" ID="reqTxtUserName"
                            ValidationGroup="Save" runat="server" CssClass="cErr21">&nbsp;&nbsp;&nbsp;&nbsp;
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Senha:
                        <br />
                        <asp:TextBox ID="txtPassword" TextMode="Password" MaxLength="256" runat="server">
&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPassword" ID="reqTxtPassword" ValidationGroup="Save"
                            runat="server" ErrorMessage="*" CssClass="cErr21">
&nbsp;&nbsp;&nbsp;&nbsp; </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Senha Novamente:
                        <br />
                        <asp:TextBox ID="TextBox1" TextMode="Password" MaxLength="256" runat="server">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtPassword" ID="RequiredFieldValidator1"
                            ValidationGroup="Save" runat="server" ErrorMessage="*" CssClass="cErr21">
&nbsp;&nbsp;&nbsp;&nbsp; </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="Save"
                            ControlToCompare="txtPassword" ControlToValidate="TextBox1" ErrorMessage="*"
                            CssClass="cErr21"></asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <table width="100%" class="TbuttonStyle">
                <tr align="right">
                    <td>
                        <asp:Button ID="btnSave" ValidationGroup="Save" Text="Salvar" runat="server" OnClick="btnSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" Text="Cancelar" runat="server" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
