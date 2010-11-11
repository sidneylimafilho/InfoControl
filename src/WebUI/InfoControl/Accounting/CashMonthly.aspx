<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    CodeBehind="CashMonthly.aspx.cs" Title="Fluxo de Caixa Mensal" Inherits="Vivina.Erp.WebUI.InfoControl.Accounting.CashMonthly" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                &#160;
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                <%--Conteúdo--%>
                <table class="cGrd11" cellspacing="0" style="width: 100%; border-collapse: collapse;">
                    <tr style="text-align: right;">
                        <th>
                        </th>
                        <th>
                            <%=  DateTime.Now.ToString("MM/yyyy") %>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(1).ToString("MM/yyyy") %>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(2).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(3).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(4).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(5).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(6).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(7).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(8).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(9).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(10).ToString("MM/yyyy")%>
                        </th>
                        <th>
                            <%= DateTime.Now.AddMonths(11).ToString("MM/yyyy")%>
                        </th>
                    </tr>
                    <asp:ListView runat="server" ID="lstMonthlyCash" DataSourceID="odsMonthlyCash" ItemPlaceholderID="itemPlaceHolder">
                        <LayoutTemplate>
                            <tr runat="server" id="itemPlaceHolder">
                            </tr>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr style="text-align: right;">
                                <td style="text-align: Left;">
                                    Conta a Receber
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("firstMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("secondMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("thirdMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("fourthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("fifthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("sixthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("seventhMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("eighthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("ninthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("tenthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("eleventhMonthSumInvoice", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text='<%# Eval("twelfthMonthSumInvoice", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr style="text-align: right;">
                                <td style="text-align: Left;">
                                    Conta a Pagar
                                </td>
                                <td>
                                    <asp:Literal ID="Literal13" runat="server" Text='<%# Eval("firstMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal14" runat="server" Text='<%# Eval("secondMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal15" runat="server" Text='<%# Eval("thirdMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal16" runat="server" Text='<%# Eval("fourthMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal17" runat="server" Text='<%#Eval("fifthMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal18" runat="server" Text='<%# Eval("sixthMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal19" runat="server" Text='<%# Eval("seventhMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal20" runat="server" Text='<%# Eval("eighthMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal21" runat="server" Text='<%# Eval("ninthMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal22" runat="server" Text='<%# Eval("tenthMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal23" runat="server" Text='<%# Eval("eleventhMonthSumBill", "{0:c}") %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal24" runat="server" Text='<%# Eval("twelfthMonthSumBill", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr style="font-weight: bold; text-align: right;">
                                <td style="font-weight: bold; text-align: Left;">
                                    Subtotal
                                </td>
                                <td>
                                    <asp:Literal ID="Literal25" runat="server" Text='<%# (((decimal)Eval("firstMonthSumInvoice")) - ((decimal)Eval("firstMonthSumBill")))  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal26" runat="server" Text='<%#  ((decimal)Eval("secondMonthSumInvoice")) - ((decimal)Eval("secondMonthSumBill"))  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal27" runat="server" Text='<%# ((decimal)Eval("thirdMonthSumInvoice")) - ((decimal)Eval("thirdMonthSumBill"))  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal28" runat="server" Text='<%#  ((decimal)Eval("fourthMonthSumInvoice")) - ((decimal)Eval("fourthMonthSumBill")) %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal29" runat="server" Text='<%# ((decimal)Eval("fifthMonthSumInvoice")) - ((decimal)Eval("fifthMonthSumBill")) %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal30" runat="server" Text='<%# ((decimal)Eval("sixthMonthSumInvoice")) - ((decimal)Eval("sixthMonthSumBill"))  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal31" runat="server" Text='<%# Page.ViewState["storedCashSeventhMonth"] = ((decimal)Eval("seventhMonthSumInvoice")) - ((decimal)Eval("seventhMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCashSixthMonth"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal32" runat="server" Text='<%# Page.ViewState["storedCashEighthMonth"] = ((decimal)Eval("eighthMonthSumInvoice")) - ((decimal)Eval("eighthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCashSeventhMonth"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal33" runat="server" Text='<%# Page.ViewState["storedCashNinthMonth"] = ((decimal)Eval("ninthMonthSumInvoice")) - ((decimal)Eval("ninthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCashEighthMonth"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal34" runat="server" Text='<%# Page.ViewState["storedCashTenthMonth"] = ((decimal)Eval("tenthMonthSumInvoice")) - ((decimal)Eval("tenthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCashNinthMonth"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal35" runat="server" Text='<%# Page.ViewState["storedCashEleventhMonth"] = ((decimal)Eval("eleventhMonthSumInvoice")) - ((decimal)Eval("eleventhMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCashTenthMonth"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal36" runat="server" Text='<%# Page.ViewState["storedCashTwelfthMonth"] = ((decimal)Eval("twelfthMonthSumInvoice")) - ((decimal)Eval("twelfthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCashEleventhMonth"])  %>' />
                                </td>
                            </tr>
                            <tr style="font-weight: bold; text-align: right;">
                                <td style="font-weight: bold; text-align: Left;">
                                    Saldo
                                </td>
                                <td>
                                    <asp:Literal ID="Literal37" runat="server" Text='<%# Page.ViewState["storedCash"] = (((decimal)Eval("firstMonthSumInvoice")) - ((decimal)Eval("firstMonthSumBill")))  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal38" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("secondMonthSumInvoice")) - ((decimal)Eval("secondMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal39" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("thirdMonthSumInvoice")) - ((decimal)Eval("thirdMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal40" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("fourthMonthSumInvoice")) - ((decimal)Eval("fourthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal41" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("fifthMonthSumInvoice")) - ((decimal)Eval("fifthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal42" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("sixthMonthSumInvoice")) - ((decimal)Eval("sixthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal43" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("seventhMonthSumInvoice")) - ((decimal)Eval("seventhMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal44" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("eighthMonthSumInvoice")) - ((decimal)Eval("eighthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal45" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("ninthMonthSumInvoice")) - ((decimal)Eval("ninthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal46" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("tenthMonthSumInvoice")) - ((decimal)Eval("tenthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal47" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("eleventhMonthSumInvoice")) - ((decimal)Eval("eleventhMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                                <td>
                                    <asp:Literal ID="Literal48" runat="server" Text='<%# Page.ViewState["storedCash"] = ((decimal)Eval("twelfthMonthSumInvoice")) - ((decimal)Eval("twelfthMonthSumBill")) + Convert.ToDecimal(Page.ViewState["storedCash"])  %>' />
                                </td>
                            </tr>
                     
                        </ItemTemplate>
                    </asp:ListView>
                </table>
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &#160;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource ID="odsMonthlyCash" runat="server" OnSelecting="odsMonthlyCash_Selecting"
        SelectMethod="GetMonthlyCashMoney" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
