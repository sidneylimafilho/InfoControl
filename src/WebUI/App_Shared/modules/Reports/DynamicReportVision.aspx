<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="DynamicReportVision.aspx.cs" Inherits="Vivina.Erp.WebUI.App_Reports.DynamicReportVision" %>

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
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <!-- conteudo -->
                <table>
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox ID="txtName" MaxLength="1024" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtName" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtName" CssClass="cErr21" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            Sql:<br />
                            <asp:TextBox ID="txtSqltext" TextMode="MultiLine" Rows="12" Width="100%" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtSqltext" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtSqltext" CssClass="cErr21" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSaveReportTableSchema" runat="server" ValidationGroup="Save" Text="Salvar"
                                OnClick="btnSave_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancelReportTablesSchema" runat="server" Text="Cancelar" OnClientClick="parent.location='dynamicReports.aspx'; return false;" />
                        </td>
                    </tr>
                </table>
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
