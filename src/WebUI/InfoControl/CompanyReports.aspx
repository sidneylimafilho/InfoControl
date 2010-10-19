<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="CompanyReports.aspx.cs" Inherits="Vivina.Erp.WebUI.CompanyReports" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                <fieldset>
                    <legend>Margens</legend>
                    <table width="100%">
                        <tr>
                            <td>
                                Superior:<br />
                                <asp:TextBox ID="txtReportUp" Plugin="Mask" Mask="99,99" Columns="2" MaxLength="5"
                                    runat="server"></asp:TextBox>
                                &nbsp;cm
                            </td>
                            <td>
                                Direita:<br />
                                <asp:TextBox ID="txtReportRight" Plugin="Mask" Mask="99,99" Columns="2" MaxLength="5"
                                    runat="server"></asp:TextBox>
                                &nbsp;cm
                            </td>
                            <td>
                                Inferior:<br />
                                <asp:TextBox ID="txtReportBottom" Plugin="Mask" Mask="99,99" Columns="2" MaxLength="5"
                                    runat="server"></asp:TextBox>
                                &nbsp;cm
                            </td>
                            <td>
                                Esquerda:<br />
                                <asp:TextBox ID="txtReportLeft" Plugin="Mask" Mask="99,99" Columns="2" MaxLength="5"
                                    runat="server"></asp:TextBox>
                                &nbsp;cm
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Cabeçalho:&amp;nbsp;" Font-Bold="True"
                    Font-Size="Small" ForeColor="#F79928" Font-Names="Trebuchet MS"></asp:Label><br />
                <asp:Label ID="Label2" runat="server" Text="(Dados que serão impressos no cabeçalho.)"
                    Font-Size="Smaller"></asp:Label>
                <textarea plugin="htmlbox" options="{idir:'../App_themes/glasscyan/controls/Editor/'}" runat="server" id="txtHeader" />
                <br />
                <asp:Label ID="Label3" runat="server" Text="Rodapé:&amp;nbsp;" Font-Bold="True" Font-Names="Trebuchet MS"
                    Font-Size="Small" ForeColor="Orange"></asp:Label><br />
                <asp:Label ID="Label4" runat="server" Text="(Dados que serão impressos no rodapé.)"
                    Font-Size="Smaller"></asp:Label>
                <textarea plugin="htmlbox" options="{idir:'../App_themes/glasscyan/controls/Editor/'}" runat="server"  ID="txtFooter" />
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
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
