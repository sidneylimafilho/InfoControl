<%@ Control Language="C#" AutoEventWireup="true" Inherits="ReportGenerator_Finish"
    CodeBehind="ReportGenerator_Finish.ascx.cs" %>
<table cellspacing="0" cellpadding="5" width="100%" border="0">
    <%--<tr>
        <td class="cTxt11b" colspan="2" >
            <h2>FINALIZAÇÃO</h2></td>
    </tr>--%>
    <tr>
        <td class="cTxt11b" valign="middle">
            <h1>
                6</h1>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
        <td valign="middle">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, GenerateReportDescription %>" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnGeraRelatorio" runat="server" Text="<%$ Resources: Resource, GenerateReport %>"
                UseSubmitBehavior="false" OnClientClick="window.open('../ReportViewer.aspx', 'NewWindow', 'toolbars=no'); return false;" /><br />
            <br />
            <br />
        </td>
    </tr>
</table>
<table cellspacing="0" cellpadding="5" width="100%" border="0" runat="server" id="tblSave">
    <tr>
        <td class="cTxt11b" valign="middle">
            <h1>
                7</h1>
            &nbsp;&nbsp;
        </td>
        <td>
            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, ReportAppreciatedDescription %>" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Resource, ReportName %>" />:
            &nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtReportTitle" runat="server" Width="200"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReportTitle"
                ErrorMessage="*" ValidationGroup="Save"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnGravaRelatorio" runat="server" Text="Gravar Relat&#243;rio" OnClick="btnGravaRelatorio_Click"
                ValidationGroup="Save" />
        </td>
    </tr>
</table>
