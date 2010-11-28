<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Host_Report"
    Title="Relatório" Codebehind="Report.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        </h1>
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
                <!--conteudo  -->
                <table width="100%">
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox runat="server" ID="txtName" Columns="30" MaxLength="1024">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtName" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtName"  ValidationGroup="Report"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Url:<br />
                            <asp:TextBox runat="server" ID="txtReportUrl" Columns="30" MaxLength="1024">
                            </asp:TextBox>
                        </td>
                        <td>
                            Identificação da Tabela:<br />
                            <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="cboReportTablesSchemaId"
                                DataSourceID="odsReportTablesSchema" DataTextField="Name" DataValueField="ReportTablesSchemaId">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSave" ValidationGroup="Report" runat="server" Text="Salvar" OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsReportTablesSchema" runat="server" SelectMethod="GetReportTablesSchema"
        TypeName="Vivina.Erp.BusinessRules.Reports.ReportsManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
