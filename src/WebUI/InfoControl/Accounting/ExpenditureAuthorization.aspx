<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="ExpenditureAuthorization.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Accounting.ExpenditureAuthorization" %>

<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc4" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h1>
        Despesa
    </h1>
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                &#160;
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                <%--Conteúdo--%>
                <table width="60%">
                    <tr>
                        <td>
                            Número do Chamado:<br />
                            <asp:TextBox runat="server" OnTextChanged="txtCallNumber_Changed" ValidationGroup="Save"
                                AutoPostBack="true" ID="txtCallNumber"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="reqTxtCallNumber" ValidationGroup="Save"
                                ErrorMessage="&nbsp&nbsp&nbsp" ControlToValidate="txtCallNumber" />
                        </td>
                    </tr>
                    <tr>
                        <asp:Panel runat="server" ID="pnlCustomerName" Visible="false">
                            <td>
                                <asp:Literal runat="server" Text="Cliente:" ID="litCustomerName" />
                                <br />
                                <b>
                                    <asp:Literal runat="server" ID="litCustomer" />
                                </b>
                            </td>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlRepresentantName" Visible="false">
                            <td>
                                <asp:Literal runat="server" Text="Representante:" ID="litRepresentantName" />
                                <br />
                                <b>
                                    <asp:Literal runat="server" ID="litRepresentant" />
                                </b>
                            </td>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlTechnicalEmployeeName" Visible="false">
                            <td>
                                <asp:Literal runat="server" Text="Técnico:" ID="litTechnical" />
                                <br />
                                <b>
                                    <asp:Literal runat="server" ID="litTechnicalEmployee" />
                                </b>
                            </td>
                        </asp:Panel>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Descrição:
                            <br />
                            <asp:TextBox ID="txtDescription" CssClass="cDat11" runat="server" Height="100px"
                                TextMode="MultiLine" Width="90%" MaxLength="500"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="reqTxtDescription" ErrorMessage="&nbsp&nbsp&nbsp"
                                ControlToValidate="txtDescription" ValidationGroup="Save" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total:
                            <br />
                            <uc4:CurrencyField ID="ucCurrFieldAmount" ValidationGroup="Save" runat="server" />
                        </td>
                    </tr>
                </table>
                <div align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" ValidationGroup="Save" OnClick="btnSave_Click" />
                    &nbsp&nbsp
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='ExpenditureAuthorizations.aspx'; return false;" />
                </div>
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &#160;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
    </table>
</asp:Content>
