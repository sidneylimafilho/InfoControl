<%@ Control Language="C#" AutoEventWireup="true" Inherits="Vivina.Erp.WebUI.Site.SiteMenu" CodeBehind="Menu.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadMenu ID="RadMenu1" runat="server" DataSourceID="odsMenu"
    DataFieldID="WebPageId" DataFieldParentID="ParentPageId" CssClass="radMenu"
    EnableAutoScroll="False" EnableEmbeddedSkins="False" Skin="Outlook">
    <ItemTemplate>
        <a runat="server" href='<%# (Container.DataItem as WebPage).Url%>'><%#Eval("Name")%></a>
    </ItemTemplate>
    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
</telerik:RadMenu>
<VFX:BusinessManagerDataSource ID="odsMenu" runat="server" TypeName="Vivina.Erp.BusinessRules.WebSites.SiteManager"
    OnSelecting="odsMenu_Selecting" SelectMethod="GetMenu">
    <SelectParameters>
        <asp:Parameter Name="companyId" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
