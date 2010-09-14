<%@ Page Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    Inherits="Vivina.Erp.WebUI.Site.SitePageBase" CodeBehind="Default.aspx.cs" %>

<%@ Import Namespace="Vivina.Erp.DataClasses" %>
<%@ Import Namespace="Vivina.Erp.BusinessRules.WebSites" %>
<%@ Register Src="~/site/_modules/WebPage.ascx" TagName="WebPage" TagPrefix="uc2" %>
<asp:Content ContentPlaceHolderID="Head" runat="server">
    <meta name="keywords" content='<%# WebPage.Name %>' />
    <meta name="robots" content="ALL" />
    <meta name="revisit after" content="1 days" />
    <meta name="distribution" content="Global" />
    <meta name="author" content="<%# Company.LegalEntityProfile.Website %>" />
    <meta name="language" content="pt-br" />
    <meta name="generator" content="Vivina InfoControl" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="ExternalContent" runat="server" id="ExternalContent" visible="false">
    </div>
    <uc2:WebPage ID="ucWebPage" Type="post" runat="server" MaxCount="7" WebPage='<%# WebPage %>' />
        
</asp:Content>
