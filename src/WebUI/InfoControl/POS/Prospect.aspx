<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_Prospect" Title="Proposta" CodeBehind="Prospect.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc1" %>
<asp:Content ID="head1" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        body
        {
            background: White !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="Prospect">
        <uc1:SelectCustomer ID="SelCustomer" Enabled="false" runat="server" />
        <br />
        <asp:Label ID="lblLocalDate" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblProspectCode" runat="server" Text=""></asp:Label>
        <br />
       
        <asp:Label ID="lblProspectNumber" runat="server"></asp:Label>
        <asp:Label ID="lblVendor" runat="server"></asp:Label>
        <asp:Label ID="lblContactName" runat="server"></asp:Label>
        <asp:Label ID="lblCover" runat="server" Text=""></asp:Label>
        <br />
        <asp:Repeater ID="rptProduct" runat="server" OnItemDataBound="rptProduct_ItemDataBound">
            <ItemTemplate>
                <fieldset>
                    <legend><b>Código:</b> <b>
                        <asp:Label ID="lblProductCode" runat="server" Text='<%#Bind("ProductCode") %>'></asp:Label></b>
                        -
                        <asp:Label ID="lblProductName" runat="server" Text='<%#Bind("SpecialProductName") %>'></asp:Label>
                    </legend>
                    <table cellspacing="5px" width="100%">
                        <tr>
                            <td style="text-align: left; width: 9%">
                                <asp:Label ID="lblRef" runat="server" Text="Ref.:" Font-Bold="true"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblReference" runat="server" Text='<%#Bind("Reference") %>'></asp:Label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 9%">
                                <b>Quant.:</b>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblProductQuantity" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: justify;">
                                <asp:Label ID="lblDescription" runat="server" Text="Descrição:" Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("ProductDescription"))) %>'
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblProductDescription" Width="100%" runat="server" Text='<%# Eval("ProductDescription") %>'
                                    Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("ProductDescription")))  %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 9%" nowrap>
                                <b>
                                    <asp:Label ID="lblValue" runat="server" Text="Valor(R$):"></asp:Label></b>
                            </td>
                            <td>
                                <asp:Label ID="lblProductPrice" runat="server" Text='<%#Bind("UnitPrice","{0:###,##0.00}") %>'></asp:Label>
                            </td>
                            <td style="white-space: nowrap;" align="right">
                                <b>Total(R$):&nbsp;</b><asp:Label ID="lblTotalValue" runat="server" Text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(Eval("UnitPrice")) * Convert.ToInt32(Eval("Quantity"))) %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <table width="100%">
            <tr>
                <td style="text-align: right; width: 90%;">
                    <asp:Label ID="lblSubTotalMessage" runat="server"><b>SubTotal(R$):</b></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblSubTotal" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAditionalMessage" runat="server"> <b>Adicional(R$):</b></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblAditional" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDiscountMessage" runat="server"> <b>Desconto(R$):</b></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblDiscount" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblIPIMessage" runat="server"> <b>IPI(R$):</b></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblIPI" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblTotalMessage" runat="server">  <b>Valor Total da Proposta(R$):</b></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlDelivery" runat="server">
            <fieldset>
                <legend>Dados da Fornecimento</legend>
                <table width="100%" cellspacing="5px">
                    <tr>
                        <td style="width: 10%; white-space: nowrap; text-align: right; vertical-align: top">
                            <asp:Label ID="lblDeliveryText" runat="server" Text="Prazo de Entrega:" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 40%; white-space: nowrap;">
                            <asp:Label ID="lblEntrega" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 40%; white-space: nowrap; text-align: right;">
                            <asp:Label ID="lblWarrantText" runat="server" Text="Garantia do Produto:" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 10%; white-space: nowrap;">
                            <asp:Label ID="lblGarantia" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; text-align: right; vertical-align: top" nowrap>
                            <asp:Label ID="lblProspectValidate" runat="server" Text="Validade da Proposta:" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblValidade" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 20%; text-align: right;" nowrap>
                            <asp:Label ID="lblpayment" runat="server" Text="Pagamento:" Font-Bold="true"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblPaymentAditional" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPagamento" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="tblMontagem">
                        <td style="width: 20%; text-align: right; vertical-align: top">
                            <asp:Label ID="lblDeliveryDescriptionText" runat="server" Text="Entrega:" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblDeliveryDescription" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="tblContato">
                        <td style="width: 20%; text-align: right; vertical-align: top">
                            <asp:Label ID="lblContactText" runat="server" Text="Contato:" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblContato" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="tblObs">
                        <td style="width: 20%; text-align: right; vertical-align: top">
                            <b>OBS:</b>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblOBS" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <br />
        <asp:Label ID="lblProspect" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblSummary" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblFooter" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnSendTOCustomer" runat="server" Text="Enviar para o Cliente" 
            onclick="btnSendTOCustomer_Click" />
    </div>
</asp:Content>
