<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Host_Reports"
    Title="Relatórios" Codebehind="Reports.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <!-- conteudo -->
                <asp:GridView ID="grdReports" Width="100%" DataKeyNames="Name,ReportUrl,ReportId,ReportTablesSchemaId"
                    runat="server" AutoGenerateColumns="False" DataSourceID="odsReports" AllowPaging="True"
                    PageSize="20" AllowSorting="True" OnSorting="grdReports_Sorting" rowselectable="false">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                        <asp:BoundField DataField="ReportUrl" HeaderText="Url" SortExpression="ReportUrl" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar um relatório.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsReports" runat="server" SelectCountMethod="GetReportsCount"
        SelectMethod="GetReports" TypeName="Vivina.Erp.BusinessRules.Reports.ReportsManager"
        ConflictDetection="CompareAllValues" DataObjectTypeName="Vivina.Erp.DataClasses.Report"
        EnablePaging="True" SortParameterName="sortExpression" DeleteMethod="DeleteReport">
        <selectparameters>
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
