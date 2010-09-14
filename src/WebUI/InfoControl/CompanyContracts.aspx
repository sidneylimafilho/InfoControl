<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="CompanyContracts.aspx.cs" Inherits="Vivina.Erp.WebUI.CompanyContracts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <!-- conteudo -->
                <fieldset>
                    <legend>Tabelas de valores adicionais</legend>
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                Adicional 1:<br />
                                <asp:TextBox ID="txtContractAdicionalValue1Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Adicional 2:<br />
                                <asp:TextBox ID="txtContractAdicionalValue2Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Adicional 3:<br />
                                <asp:TextBox ID="txtContractAdicionalValue3Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Adicional 4:<br />
                                <asp:TextBox ID="txtContractAdicionalValue4Name" runat="server" MaxLength="50"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                Adicional 5:<br />
                                <asp:TextBox ID="txtContractAdicionalValue5Name" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <br />
                <div align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
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
</asp:Content>
