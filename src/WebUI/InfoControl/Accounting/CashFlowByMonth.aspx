<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Accounting_CashFlowByMonth" Title="Untitled Page" Codebehind="CashFlowByMonth.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Fluxo de Caixa de <%=new DateTime(2000, int.Parse(Request["month"]), 1).ToString("MMMM").ToUpper() %></h1>
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td class="center">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td class="right">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <asp:GridView ID="GridView1" Width="100%" OnRowDataBound="GridView1_RowDataBound" 
                    runat="server" DataSourceID="BusinessManagerDataSource1">
                </asp:GridView>
                <VFX:BusinessManagerDataSource ID="BusinessManagerDataSource1" runat="server" SelectMethod="RetrieveCashFlowByMonth"
                    TypeName="Vivina.Erp.BusinessRules.FinanceManager">
                    <selectparameters>
                       <asp:QueryStringParameter DefaultValue="1" Name="month" 
                           QueryStringField="month" Type="Int32" />
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
