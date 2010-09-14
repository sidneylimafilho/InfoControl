<%@ Control Language="C#" AutoEventWireup="True" Inherits="Site_ProductsControl"
    CodeBehind="Products.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<div class="produtos">
    <asp:ListView ID="lstProducts" runat="server" DataSourceID="odsProducts">
        <LayoutTemplate>
            <div runat="server" id="itemPlaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
            <div class="produto">
                <div class="borda_superior">
                </div>
                <div class="corpo">
                    <div class="imagem">
                        <a id="A1" class="" runat="server" href='<%#Eval("Url") %>'>
                            <img runat="server" alt='<%#Eval("Name")%>'  height="120" src='<%#Eval("ImageUrl") %>'
                                border="0" />
                        </a>
                    </div>
                    <div class="nome">
                        <a id="A3" runat="server" href='<%#Eval("Url") %>'>
                            <%#Eval("Name")%>
                        </a>
                    </div>
                    <span class="preco">Preço: <span>
                        <%#Convert.ToDecimal(Eval("UnitPrice")).ToString("C")%>
                    </span></span>
                    <%--<div class="rodape">
                            <%# Convert.ToInt32(Eval("Inventory.Quantity")) == 0 ? "(Produto Esgotado!)" : String.Empty %>
                        </div>--%>
                </div>
                <div class="borda_inferior">
                </div>
                <div class="rodape_detalhes">
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
    <br />
    <div class="paginacao" style="display:<%= lstProducts.Items.Count > dtpProducts.PageSize?"":"none" %>">
        <asp:DataPager ID="dtpProducts" QueryStringField="page" PagedControlID="lstProducts"
            PageSize="20" runat="server">
            <Fields>
                <asp:NumericPagerField ButtonCount="15" CurrentPageLabelCssClass="atual" NumericButtonCssClass="pagina"
                    ButtonType="Link" NextPreviousButtonCssClass="acao" NextPageText="..." PreviousPageText="..." />
                <%--
                <asp:TemplatePagerField>
                    <PagerTemplate>
                        <a href="<%=Page.GetQueryStringNavigateUrl("orderBy", "Name") %>">
                            <%#Container.StartRowIndex %></a>
                    </PagerTemplate>
                </asp:TemplatePagerField>--%>
            </Fields>
            
        </asp:DataPager>
    </div>
</div>
<VFX:BusinessManagerDataSource ID="odsProducts" runat="server" SelectMethod="GetProducts"
    TypeName="Vivina.Erp.BusinessRules.ProductManager" OnSelecting="odsProducts_Selecting"
    OldValuesParameterFormatString="original_{0}" EnablePaging="True" SelectCountMethod="GetProductsCount"
    SortParameterName="sortExpression">
    <SelectParameters>
        <asp:Parameter Name="companyId" Type="Int32" />
        <asp:Parameter Name="depositId" Type="Int32" />
        <asp:Parameter Name="categoryId" Type="Int32" />
        <asp:Parameter Name="categoriesRecursive" Type="Boolean" DefaultValue="true" />
        <asp:QueryStringParameter Name="sortExpression" QueryStringField="orderBy" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
