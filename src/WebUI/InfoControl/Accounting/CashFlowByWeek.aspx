<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Accounting_CashFlowByWeek" Title="Untitled Page" Codebehind="CashFlowByWeek.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Fluxo de Caixa</h1>
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
                asdf
                <asp:ListView ID="ListView1" runat="server" DataSourceID="BusinessManagerDataSource1">
                    <LayoutTemplate>
                        <table>
                            <tr>
                                <td>Centro de Custo
                                </td>
                                <td>Centro de Custo
                                </td>
                                <td>Centro de Custo
                                </td>
                                <td>Centro de Custo
                                </td>
                                <td>Centro de Custo
                                </td>
                                <td>Centro de Custo
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:ListView>
                <VFX:BusinessManagerDataSource ID="BusinessManagerDataSource1" runat="server">
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
