<%@ Control Language="C#" AutoEventWireup="true" Inherits="ReportGenerator_Start"
    CodeBehind="ReportGenerator_Start.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<table cellspacing="10" cellpadding="0" width="100%" border="0">
    <tr>
        <td class="cTxt11b" style="width: 1%" align="left" valign="top">
            <h1>
                1</h1>
        </td>
        <td class="cTxt11b">
            <asp:DropDownList ID="cboReports" runat="server" Width="100%" DataTextField="Name"
                DataValueField="ReportId" AppendDataBoundItems="True" DataSourceID="odsReports"
                AutoPostBack="True" OnSelectedIndexChanged="cboReports_SelectedIndexChanged">
                <asp:ListItem Text="<%$ Resources: Resource, SelectSomeReportCreatedByTeam %>" Value=""
                    Selected="True"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, Or %>" />
        </td>
    </tr>
    <tr>
        <td class="cTxt11b" style="width: 1%" align="left" valign="top">
            <h1>
                2</h1>
        </td>
        <td class="cTxt11b">
            <asp:DropDownList ID="cboTables" runat="server" Width="100%" DataTextField="name"
                DataValueField="ReportTablesSchemaId" AppendDataBoundItems="True" DataSourceID="BusinessObjectDataSource1">
                <asp:ListItem Text="<%$ Resources: Resource, CreateYourOwnReportDescription %>" Value=""
                    Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboTables"
                Display="Dynamic" ErrorMessage="<%$ Resources: Resource, ChooseOfFactRequiredMessage %>"
                CssClass="cVal21"></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
<table align="center">
    <tr>
        <td valign="top" class="cTxt11b" style="padding-right: 10px; width: 150px;" align="center">
            <asp:ImageButton ID="btnNominalReport" runat="server" ImageUrl="../images/NominalReport.gif"
                OnClick="btnNominalReport_Click" />
            <br />
            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, NominalReport %>" />
        </td>
        <td valign="top" class="cTxt11b" style="padding-right: 10px; width: 150px;" align="center">
            <asp:ImageButton ID="btnStatisticsReport" runat="server" ImageUrl="../images/StatisticReport.gif"
                OnClick="btnStatisticsReport_Click" />
            <br />
            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Resource, StatisticsReport %>" />
        </td>
    </tr>
</table>
<VFX:BusinessManagerDataSource ID="BusinessObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="RetrieveAllTablesSchema" TypeName="InfoControl.Web.Reporting.ReportsManager">
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsReports" runat="server" OldValuesParameterFormatString="original_{0}"
    SelectMethod="RetrieveAllReports" TypeName="InfoControl.Web.Reporting.ReportsManager">
</VFX:BusinessManagerDataSource>
