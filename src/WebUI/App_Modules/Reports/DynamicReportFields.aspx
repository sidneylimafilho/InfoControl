<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="DynamicReportFields.aspx.cs" Inherits="Vivina.Erp.WebUI.App_Reports.DynamicReportVariables" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h2>
        Variáveis
    </h2>
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
                <asp:GridView ID="grdReportColumnsSchema" Width="100%" RowSelectable="false" runat="server"
                    AutoGenerateColumns="False" DataSourceID="odsReportColumnsSchemas" AllowSorting="True"
                    DataKeyNames="ReportColumnsSchemaId,Name,Source,ReportTablesSchemaId,ReportDataTypeId,ForeignKey,PrimaryKey,PrimaryTable,PrimaryLabelColumn"
                    OnRowDataBound="grdReportColumnsSchema_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" />
                        <asp:BoundField DataField="Source" HeaderText="Fonte" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar uma coluna.<br />
                            &nbsp;
                            <asp:Button ID="btnShowEditColumns" OnClick="btnShowEditColumns_Click" runat="server"
                                Text="Cadastrar" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <VFX:BusinessManagerDataSource ID="odsReportColumnsSchemas" SelectMethod="RetrieveColumnsSchema"
                    DeleteMethod="Delete" DataObjectTypeName="InfoControl.Web.Reporting.DataClasses.ReportColumnsSchema"
                    TypeName="InfoControl.Web.Reporting.ReportsManager" runat="server" onselecting="odsReportColumnsSchemas_Selecting"
                    ondeleted="odsReportColumnsSchemas_Deleted">
                    <selectparameters>
                        <asp:Parameter Name="tableId" Type="Int32" />
                            
                    </selectparameters>
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
</asp:Content>
