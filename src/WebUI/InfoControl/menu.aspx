<%@ Page Language="C#" AutoEventWireup="true" Inherits="Commons_menu" CodeBehind="menu.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%--<%@ Register Assembly="Telerik.WebControls" Namespace="Telerik.WebControls" TagPrefix="radP" %>--%>
<%@ Register Assembly="Telerik.Web.UI, Version=2008.1.515.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="radP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html id="clean" xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        body
        {
            background-color: transparent;
        }
        .startPage
        {
            position: absolute;
            height: 16px;
            width: 16px;
            right: 5px;
            margin-top: -26px;
            background: url(../App_Shared/themes/glasscyan/menu/help.png) no-repeat right top;
        }
        .startPage:hover
        {
            background-position: 0px -16px;
        }
    </style>
    
    <link type="text/css" rel="Stylesheet" href="~/app_shared/themes/glasscyan/filescombiner.ascx" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="menu">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/App_Shared/js/jquery.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:Label ID="lblAdminCompany" runat="server" Text="Administrar a empresa:">
        </asp:Label><br />
        <asp:DropDownList ID="cboCompanies" runat="server" Width="177px" DataTextField="CompanyName"
            DataValueField="CompanyId" DataSourceID="odsCompanies" OnPreRender="cboCompanies_PreRender"
            OnSelectedIndexChanged="cboCompanies_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
        <VFX:BusinessManagerDataSource runat="server" ID="odsCompanies" SelectMethod="GetCompaniesNames"
            TypeName="Vivina.Erp.BusinessRules.CompanyManager" OnSelecting="odsCompanies_Selecting">
            <SelectParameters>
                <asp:Parameter Name="userId" Type="Int32" />
            </SelectParameters>
        </VFX:BusinessManagerDataSource>
        <!--
        <a href="<%=System.Web.SiteMap.RootNode.Url %>" target="content" class="homeLink">
        </a>
         <a href="<%=System.Web.SiteMap.RootNode.Url %>" target="content" class="homeLink">
        </a>
        -->
        <radP:RadPanelBar ID="rdpMenu" runat="server" Width="100%" ExpandEffect="Fade" ExpandEffectSettings="duration=1"
            ExpandMode="MultipleExpandedItems" SingleExpandedPanel="False" EnableViewState="False"
            EnableEmbeddedSkins="false">
            <CollapseAnimation Type="None" Duration="100"></CollapseAnimation>
            <ExpandAnimation Type="None" Duration="100"></ExpandAnimation>
        </radP:RadPanelBar>
    </div>
    </form>

    <script type="text/javascript">window.focus();</script>

</body>
</html>
