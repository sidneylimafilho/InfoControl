<%@ Page Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    Inherits="Site_Categories" CodeBehind="Categories.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src='~/site/Categories.ascx' TagName='Categories' TagPrefix='vfx' %>
<%@ Register Src='~/site/Products.ascx' TagName='Products' TagPrefix='vfx' %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <VFX:Categories runat="server" />
</asp:Content>
