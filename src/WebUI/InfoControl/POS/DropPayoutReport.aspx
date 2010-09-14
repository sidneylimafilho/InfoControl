<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_DropPayoutReport" Title="Fechamento do Caixa" CodeBehind="DropPayoutReport.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
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
                <fieldset id="filter" class="closed">
                    <legend onmouseover='setTimeout("$(\"#filter .body\").show(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"open\"})", 300);'>
                        Escolha o filtro desejado: </legend><div class="body">
                            <table style="text-align: left">
                                <tr>
                                    <td>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <uc2:DateTimeInterval ID="ucDateTimeInterval" Required="true" ValidationGroup="Show"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                    </td>
                                </tr>
                            </table>
            </td>
            <td>
                <asp:Button ID="btnVisualize" runat="server" Text="Filtrar" ValidationGroup="Show" />
            </td>
        </tr>
    </table>
    <br />
    </div><span class="closeButton" onmouseover='setTimeout("$(\"#filter .body\").hide(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"closed\"})", 950);'>
        &nbsp;</span> </fieldset>
    <br />
    <br />
    <br />
    <table width="100%">
        <tr>
            <td style="width: 50%; padding-right: 10px; vertical-align: top">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>
                                    <h3>
                                        Entradas por formas de pagamento:</h3>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptFormasPagamento" runat="server">
                                                <ItemTemplate>
                                                    <b>
                                                        <asp:Label ID="lblFormasPagamento" runat="server" Text='<%#Bind("Name") %>'></asp:Label><br />
                                                    </b>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Repeater ID="rptValores" runat="server">
                                                <ItemTemplate>
                                                    R$&nbsp;<asp:Label ID="lblValores" runat="server" Text='<%#Bind("Value") %>'></asp:Label><br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <h3>
                                &nbsp;&nbsp;Total:</h3>
                        </td>
                        <td style="text-align: right; white-space: nowrap;">
                            <b>
                                <asp:Label ID="lblTotalPagamento" runat="server" Text="0,00"></asp:Label>
                            </b>&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>
                                    <h3>
                                        Sangria:</h3>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptSangria" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSangria" runat="server" Text='<%#Bind("Comment") %>'></asp:Label><br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Repeater ID="rptSangriaValores" runat="server">
                                                <ItemTemplate>
                                                    <p style="color: red">
                                                        R$&nbsp;<asp:Label ID="lblSangriaValores" runat="server" Text='<%#Bind("Amount", "{0:###,##0.00}") %>'></asp:Label></p>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>
                                    <h3>
                                        Suplemento:</h3>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptSuplemento" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSuplemento" runat="server" Text='<%#Bind("Comment") %>'></asp:Label><br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Repeater ID="rptSuplementoValores" runat="server">
                                                <ItemTemplate>
                                                    R$&nbsp;<asp:Label ID="lblSuplementoValores" runat="server" Text='<%#Bind("Amount", "{0:###,##0.00}") %>'></asp:Label><br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <h3>
                                &nbsp;&nbsp;&nbsp;Dinheiro no caixa:</h3>
                        </td>
                        <td style="text-align: right; white-space: nowrap;">
                            <b>
                                <asp:Label ID="lblCaixa" runat="server" Text="0,00"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%; vertical-align: top; border-left: double 3px orange">
                <table cellpadding="10" cellspacing="10" width="100%">
                    <tr>
                        <td>
                            <h2>
                                Extrato:</h2>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataList ID="lstExtract" runat="server" Width="100%" DataSourceID="odsExtract"
                                OnItemDataBound="lstExtract_ItemDataBound">
                                <HeaderTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 25%">
                                                <h3>
                                                    Data</h3>
                                            </td>
                                            <td style="width: 50%; text-align: left">
                                                <h3>
                                                    Cliente</h3>
                                            </td>
                                            <td style="width: 25%; text-align: right">
                                                <h3>
                                                    Total</h3>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="lblDate" runat="server" Text='<%#Bind ("SaleDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td style="width: 50%; text-align: left">
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%#Bind ("Customer") %>'></asp:Label>
                                            </td>
                                            <td style="width: 25%; text-align: right">
                                                <asp:Label ID="lblTtl" runat="server" Text='<%#Bind("Total","{0:F2}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <b>
                                <asp:Label ID="lblFinalTotal" runat="server" Text=""></asp:Label>
                            </b>
                        </td>
                    </tr>
                </table>
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
    <VFX:BusinessManagerDataSource ID="odsExtract" runat="server" OnSelecting="odsExtract_Selecting"
        SelectMethod="GetSaleHistory" TypeName="Vivina.Erp.BusinessRules.SaleManager"
        OldValuesParameterFormatString="original_{0}">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            <asp:Parameter Name="showCanceled" Type="Boolean" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
