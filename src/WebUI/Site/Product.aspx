<%@ Page Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    CodeBehind="Product.aspx.cs" Inherits="Site_Product" %>

<%@ Register Src="~/site/_modules/Products.ascx" TagName="Products" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="categoryPath">
        Você está navegando por:
        <%=GetCategoryBreadcrumbs(Category) %></div>
    <div class="center_content">
        <div class="produto">
            <span class="fotos">
                <%
                    if (Product.ProductImages.Any())
                        Response.Write("<a href='" + ResolveUrl(Product.ProductImages[0].ImageUrl) + "' class='thickbox'>" +
                                       "<img src='" + ResolveUrl(Product.ProductImages[0].ImageUrl) + "' id='imgLargeProductImage' border='0' style='height: 128px' />" +
                                       "</a><br />");

                    foreach (var image in Product.ProductImages)
                        Response.Write(@"<img class='image' src='" + ResolveUrl(image.ImageUrl) + "' style='height: 32px' />");
                %>
            </span>
            <div class="nome">
                <%=Product.Name %>
            </div>
            <div class="preco">
                <label>
                    Preço:&nbsp;</label><%=Convert.ToDecimal(Product.UnitPrice).ToString("c")%>
            </div>
            <div class="codigo">
                <label>
                    Código do Produto:</label>
                <%=Product.ProductCode ?? Product.ExternalSourceProductId %>
            </div>
            <div class="formasPgto">
            </div>
            <div class="garantia">
                <label>
                    Garantia(anos):</label>
                <%=Product.WarrantyDays %>
            </div>
            <div class="fabricante">
                <label>
                    Fabricante:&nbsp;</label><%=Product.Manufacturer != null ? Product.Manufacturer.Name : ""%>
            </div>
            <div class="quantidade">
                <%= Product.Quantity == 0 ? "Produto Esgotado!" : ""%>
            </div>
            <div id="Div1" runat="server" visible='<%# (Product.Quantity > 0 || Product.AllowNegativeStock == true ) %>'>
                <input type="hidden" name="email_cobranca" value="<%=Product.ProductId %>" />
                <input type="hidden" name="tipo" value="CBR" />
                <input type="hidden" name="moeda" value="BRL" />
                <input type="hidden" name="peso" value="2" />
                <input type="hidden" name="frete" value="0" />
                <input type="hidden" name="productId" value='<%=Product.ProductId %>' />
                <input type="hidden" name="unitPrice" value='<%=Convert.ToDecimal(Product.UnitPrice).ToString("n") %>' />
                <input type="hidden" name="quantity" value="1" />
                <input type="hidden" name="name" value='<%=Product.Name %>' />
                <input type="hidden" name="modifiedDate" value='<%=Product.ModifiedDate %>' />
                <input type="hidden" name="ImageUrl" value='<%=Product.ProductImages.Count > 0 ? Product.ProductImages[0].ImageUrl : "" %>' />
                <a href="javascript:;" class="botaoComprar" onclick="document.forms[0].action='<%=ResolveUrl("~/site/Checkout_Basket.aspx")%>'; document.forms[0].submit()">
                    Comprar! </a>
            </div>
        </div>
        <asp:Panel ID="pnlDescription" runat="server" Visible='<%# Product.Description != "" %>'>
            <div class="descricao">
                <label>
                    Descrição do Produto
                </label>
                <div>
                    <%= String.IsNullOrEmpty(Product.Description) ? "(Não há descrição para esse produto.)" : Product.Description %></div>
            </div>
        </asp:Panel>
    </div>
    <div class="outrosProdutos">
        <div>
            Produtos Mais Vendidos:</div>
        <uc2:Products runat="server" Qtd="5" Ordem="QuantitySold"></uc2:Products>
    </div>

    <script type="text/javascript">

        $().ready(function() {

            $(".image").click(function() {
                $("#imgLargeProductImage").attr("src", $(this).attr("src"));
            });
        });
    </script>

</asp:Content>
