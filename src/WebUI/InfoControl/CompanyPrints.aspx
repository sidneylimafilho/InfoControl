<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="CompanyPrints.aspx.cs" Inherits="Vivina.Erp.WebUI.CompanyPrints" %>

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
                Rodapé do Comprovante de Venda:<br />
                <asp:TextBox ID="txtPrinterFooter" Columns="40" MaxLength="200" runat="server"></asp:TextBox>
                
                <div align="right">
                        <asp:Button runat="server" ID="btnSave" Text="Salvar" onclick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancelar" />
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
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
