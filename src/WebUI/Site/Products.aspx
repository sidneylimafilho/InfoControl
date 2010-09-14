<%@ Page Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    Inherits="Site_Products" CodeBehind="Products.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src='~/site/_modules/Categories.ascx' TagName='Categories' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/Products.ascx' TagName='Products' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/Search.ascx' TagName='Search' TagPrefix='vfx' %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="categoryPath">
        Você está navegando por:
        <%=GetCategoryBreadcrumbs(Category) %></div>
    <div class="faixa">
        Ordernar por: &nbsp&nbsp <a href="<%=GetQueryStringNavigateUrl("orderBy", "Name") %>">
            A-Z</a> | <a href="<%=GetQueryStringNavigateUrl("orderBy", "Name DESC") %>">Z-A</a>
        | <a href="<%=GetQueryStringNavigateUrl("orderBy", "UnitPrice") %>">Produto mais barato</a>
        | <a href="<%=GetQueryStringNavigateUrl("orderBy", "UnitPrice Desc") %>">Produto mais
            caro</a>
        <br />
    </div>
    <VFX:Products runat="server" ID="ucProducts" />
</asp:Content>
