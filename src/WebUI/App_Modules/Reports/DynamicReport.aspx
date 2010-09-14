<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="App_Reports_ReportTablesSchema" EnableEventValidation="false" Title="Relatório dinâmico"
    CodeBehind="DynamicReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href='DynamicReportVision.aspx?ReportTablesSchemaId=<%= Request["ReportTablesSchemaId"] %>'
                target="tabContent">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, Vision %>" /></a></li>
            <li><a href='DynamicReportFields.aspx?ReportTablesSchemaId=<%= Request["ReportTablesSchemaId"]%>'
                target="tabContent">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, Variables %>" /></a></li>
        </ul>
        <iframe id="tabContent" name="tabContent" src='DynamicReportVision.aspx?ReportTablesSchemaId=<%= Request["ReportTablesSchemaId"]%>'
            width="100%" height="80%"></iframe>
    </div>
</asp:Content>
