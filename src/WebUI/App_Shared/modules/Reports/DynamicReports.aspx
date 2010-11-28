<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="App_Reports_DinamicReports" Title="<%$ Resources: Resource, DynamicReport %>"
    CodeBehind="DynamicReports.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h2>
        &nbsp;
        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, TableSchema %>" />
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
                <asp:GridView ID="grdDinamicReports" RowSelectable="false" runat="server" DataSourceID="odsDinamicReports"
                    DataKeyNames="ReportTablesSchemaId,Name,Sqltext" AutoGenerateColumns="False"
                    AllowSorting="True" OnSorting="grdDinamicReports_Sorting" OnRowDataBound="grdDinamicReports_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources: Resource, Name %>" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            HeaderText="&lt;a href=&quot; DynamicReportVision.aspx&quot; &lt;div class=&quot;insert&quot;title=&quot;inserir&quot; &lt;/div&gt; &lt;/a&gt"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, WithoutDataForDisplayMessage %>" />
                            <br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="<%$ Resources: Resource, Register %>"
                                OnClientClick="location='DynamicReportVision.aspx'; return false;" />
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
    <VFX:BusinessManagerDataSource ID="odsDinamicReports" runat="server" DataObjectTypeName="InfoControl.Web.Reporting.DataClasses.ReportTablesSchema"
        TypeName="InfoControl.Web.Reporting.ReportsManager" SelectMethod="RetrieveAllTablesSchema"
        DeleteMethod="Delete" ondeleted="odsDinamicReports_Deleted">
    </VFX:BusinessManagerDataSource>
</asp:Content>
