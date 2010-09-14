<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" Inherits="InfoControl_Services_Default" Title="Serviços" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table style="width: 100%; height: 100%; text-align: center; vertical-align: middle;">
        <tr>
            <td>
                <table border="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="../CRM/CustomerCalls.aspx"
                                CssClass="CustomerCalls HelpTip">
                                &nbsp; <span class="buttonTip">
                                    <h1>
                                        Ajuda:</h1>
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Help, StartPageButton_Services%>" />
                                    <span class="footer"></span></span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            <div class="flowRight">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton5" runat="server" PostBackUrl="ServiceOrder.aspx"
                                CssClass="OrderServices HelpTip">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                            <div class="flowRight">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton8" runat="server" PostBackUrl="../Accounting/Receipt.aspx"
                                CssClass="NotaFiscal HelpTip">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                            <div class="flowRight">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton15" runat="server" PostBackUrl="../Accounting/Invoice.aspx"
                                CssClass="Invoice">&nbsp;</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <div class="flowTop">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="Service.aspx" CssClass="Services">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
