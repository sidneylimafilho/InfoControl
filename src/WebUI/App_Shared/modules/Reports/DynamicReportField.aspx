<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="DynamicReportField.aspx.cs" Inherits="Vivina.Erp.WebUI.App_Reports.DynamicReportField" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <table width="100%">
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox ID="txtReportColumnsSchemaName" MaxLength="1024" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtReportColumnsSchemaName" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtReportColumnsSchemaName" CssClass="cErr21" ValidationGroup="SaveReportColumnsSchema"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Fonte:<br />
                            <asp:TextBox ID="txtSource" MaxLength="1024" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtSource" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtSource" CssClass="cErr21" ValidationGroup="SaveReportColumnsSchema"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Tipo:<br />
                            <asp:DropDownList ID="cboReportDataType" runat="server" DataSourceID="odsReportDataType"
                                DataTextField="Name" DataValueField="ReportDataTypeId" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqcboReportDataType" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="cboReportDataType" CssClass="cErr21" ValidationGroup="SaveReportColumnsSchema"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <table id="tbOptionalsFields" width="100%" style="display: none">
                    <tr>
                        <td>
                            Chave estrangeira:<br />
                            <asp:TextBox ID="txtForeignKey" MaxLength="1024" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            chave primária:<br />
                            <asp:TextBox ID="txtPrimaryKey" MaxLength="1024" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            tabela primária:<br />
                            <asp:TextBox ID="txtPrimaryTable" MaxLength="1024" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Nome da coluna primária:<br />
                            <asp:TextBox ID="txtPrimaryLabelColumn" MaxLength="1024" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSaveReportColumnsSchema" ValidationGroup="SaveReportColumnsSchema"
                                runat="server" Text="Salvar" OnClick="btnSaveReportColumnsSchema_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancelReportColumnsSchema" runat="server" OnClientClick="parent.location='dynamicReports.aspx'; return false;"
                                Text="Cancelar" />
                        </td>
                    </tr>
                </table>
                <VFX:BusinessManagerDataSource ID="odsReportDataType" TypeName="InfoControl.Web.Reporting.ReportsManager"
                    SelectMethod="RetrieveDataTypes" runat="server">
                </VFX:BusinessManagerDataSource>
                <VFX:BusinessManagerDataSource ID="odsReportColumnsSchema" TypeName="InfoControl.Web.Reporting.ReportsManager"
                    SelectMethod="RetrieveAllColumnsSchema" runat="server">
                </VFX:BusinessManagerDataSource>
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

    <script type="text/javascript">

        $().ready(function() {

            if ($("#ctl00_ContentPlaceHolder_cboReportDataType").attr("value") == "4")
                $("#tbOptionalsFields").attr("style", "display:block");

            $("#ctl00_ContentPlaceHolder_cboReportDataType").change(function() {

                if ($(this).attr("value") == "4")
                    $("#tbOptionalsFields").show("slow");
                else
                    $("#tbOptionalsFields").hide("slow");

            });

        });
    
    </script>

</asp:Content>
