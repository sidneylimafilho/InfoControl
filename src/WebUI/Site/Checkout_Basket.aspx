<%@ Page Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    CodeBehind="Checkout_Basket.aspx.cs" Inherits="Vivina.Erp.WebUI.Site.CheckoutBasket" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Src="~/App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="~/Infocontrol/Accounting/Payment.ascx" TagName="Payment" TagPrefix="uc5" %>
<%@ Register Src="~/Site/Checkout_Steps.ascx" TagName="CheckoutSteps" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="basket">
        <uc1:CheckoutSteps runat="server" />
        <div class="navegacao">
            <asp:LinkButton runat="server" ID="lnkBack1" ToolTip="Continue Comprando" CssClass="continueComprando"
                CommandName="Back" OnCommand="LinkCommand_Command">&nbsp;</asp:LinkButton>
            <asp:LinkButton runat="server" ID="lnkNext1" ToolTip="Avançar" CssClass="avancar"
                CausesValidation="true" ValidationGroup="delivery" CommandName="Next" OnCommand="LinkCommand_Command">&nbsp;</asp:LinkButton>
        </div>
        <table width="100%">
            <asp:ListView runat="server" ID="itemList" ItemPlaceholderID="itemPlaceHolder" DataKeyNames="BudgetItemId,Quantity"
                OnItemCommand="lnkDelete_Command" OnItemDataBound="itemList_ItemDataBound">
                <LayoutTemplate>
                    <thead>
                        <tr>
                            <td class="col1">
                            </td>
                            <td class="col2">
                                Imagem
                            </td>
                            <td class="col3">
                                Nome do Produto
                            </td>
                            <td class="col4">
                                Qtd
                            </td>
                            <td class="col5">
                                Preço
                            </td>
                            <td class="col6">
                                Sub-Total
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr runat="server" id="itemPlaceHolder">
                        </tr>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="col1">
                            <asp:LinkButton runat="server" ID="lnkDelete" CssClass="excluir">&nbsp;</asp:LinkButton>
                        </td>
                        <td class="col2">
                            <asp:Image runat="server" ID="imgProductImage" AlternateText='<%# Eval("ProductDescription") %>'
                                ImageUrl='<%# GetImageUrl(Container.DataItem) %>' Visible='<%# GetImageUrl(Container.DataItem)!=null %>' />
                        </td>
                        <td class="col3">
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("SpecialProductname") %>' CssClass="nomeProduto"></asp:Label><br />
                            <asp:Label ID="lblCode" runat="server" Text='<%# "Código: " + Eval("ProductCode") %>'
                                CssClass="codigo" Visible='<%# Eval("ProductCode")!=null %>'></asp:Label>
                        </td>
                        <td class="col4">
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity")%>' CssClass="quantidade"></asp:Label><br />
                        </td>
                        <td class="col5">
                            <asp:Label ID="lblPrice" runat="server" Text=' <%# ((Decimal)Eval("UnitPrice")).ToString("C") %>'
                                CssClass="preco"></asp:Label>
                        </td>
                        <td class="col6">
                            <asp:Label ID="lblTotalPrice" runat="server" Text=' <%# ((Decimal)Eval("UnitPrice") * (int)Eval("Quantity")).ToString("C") %>'
                                CssClass="totalProduto"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <tr>
                        <td>
                            O carrinho está vazio, conheça nossas ofertas <a href="~/site/loja/" runat="server">clicando aqui</a>:<br />
                        </td>
                    </tr>
                </EmptyDataTemplate>
            </asp:ListView>
            <tr runat="server" id="pnlSubTotal" class="endereco">
                <td colspan="5" class="col5">
                    <uc1:Address Required="true" ValidationGroup="delivery" runat="server" FieldsetTitle="Endereço de Entrega"
                        OnChanged="ucDeliveryAddress_Changed" ID="ucDeliveryAddress">
                    </uc1:Address>
                </td>
                <td class="col6 frete" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel runat="server" Visible="false" ID="pnlDeliveryPrices">
                                Serviços de Entrega:
                                <asp:RadioButtonList ID="rbtListDelivery" OnSelectedIndexChanged="rbtListDelivery_SelectedIndexChanged"
                                    AutoPostBack="true" runat="server" RepeatDirection="Vertical" RepeatLayout="Table"
                                    ValidationGroup="delivery">
                                    <asp:ListItem Text="Pac: R$ "></asp:ListItem>
                                    <asp:ListItem Text="Sedex: R$ "></asp:ListItem>
                                    <asp:ListItem Text="Sedex10: R$ "></asp:ListItem>
                                    <asp:ListItem Text="Sob Consulta" Value="-1"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ID="reqRbtListDeliveryPrices" ControlToValidate="rbtListDelivery"
                                    ValidationGroup="delivery" ErrorMessage="Escolha um modo de entrega!">
                                </asp:RequiredFieldValidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            </tbody>
            <tfoot>
                <tr id="pnlTotal" runat="server">
                    <td colspan="4" />
                    <td class="col5">
                        <h3>
                            Total:</h3>
                    </td>
                    <td class="col6">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <h3 class="total">
                                    <%= Convert.ToDecimal(Session["total"]).ToString("C") %></h3>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </tfoot>
        </table>
        <div class="navegacao">
            <asp:LinkButton runat="server" ID="lnkBack2" ToolTip="Continue Comprando" CssClass="continueComprando"
                CommandName="Back" OnCommand="LinkCommand_Command">&nbsp;</asp:LinkButton>
            <asp:LinkButton runat="server" ID="lnkNext2" ToolTip="Avançar" CssClass="avancar"
                CausesValidation="true" ValidationGroup="delivery" CommandName="Next" OnCommand="LinkCommand_Command">&nbsp;</asp:LinkButton>
        </div>
    </div>
</asp:Content>
