<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="CompanySales.aspx.cs" Inherits="Vivina.Erp.WebUI.CompanySales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
            <td class="left" style="height: 97px">
                &nbsp;
            </td>
            <td class="center" style="height: 97px">
                <fieldset>
                    <legend>Tabelas de Preços</legend>
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                Tabela 1:<br />
                                <asp:TextBox ID="txtUnitPrice1Name" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqtxtUnitPrice1Name" runat="server" ControlToValidate="txtUnitPrice1Name"
                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="saveConfiguration"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Tabela 2:<br />
                                <asp:TextBox ID="txtUnitPrice2Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Tabela 3:<br />
                                <asp:TextBox ID="txtUnitPrice3Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Tabela 4:<br />
                                <asp:TextBox ID="txtUnitPrice4Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Tabela 5:<br />
                                <asp:TextBox ID="txtUnitPrice5Name" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" ValidationGroup="saveConfiguration" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                </div>
            </td>
            <td class="right" style="height: 97px">
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
