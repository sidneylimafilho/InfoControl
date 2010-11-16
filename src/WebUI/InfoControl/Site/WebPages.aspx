<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Site_SiteMaps" Title="Páginas do Site"
    CodeBehind="WebPages.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content" ContentPlaceHolderID="Header" runat="server">
    <style>
        .rtTemplate
        {
            padding: 0px !important;
        }
        .inline, .inline:hover
        {
            display: inline;
            text-decoration: none !important;
        }
        .rtIn, .rtHover .rtIn, .rtSelected .rtIn, .rtHover .rtIn, .rtHover div, .rtSelected .rtIn, .rtSelected div
        {
            border: 0px solid white !important;
            margin: 0px !important;
            padding: 0px !important;
            background-image: none !important;
            background-color: white !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div runat="server" id="configWebsite" class="cLoading11" style="top: 0px; left: 0px;">
        <div style="border: 2px solid teal; margin: auto; width: 400px; height: 300px;">
            <p>
                Sua empresa ainda não tem um endereço web configurado. Um endereço web é o que nós
                chamamos de dominio, esse dominio é seu nome na internet, geralmente éum nome seguido
                de ".com.br"</p>
            <p>
                Para saber mais sobre como obter seu dominio clique aqui</p>
            <br />
            <br />
            Endereço Web <i>(nomedaempresa.com.br)</i>:<br />
            <asp:TextBox ID="txtWebSite" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"
                CssClass="cErr11" ControlToValidate="txtWebSite"></asp:RequiredFieldValidator>
            <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
        </div>
    </div>
    <table class="cLeafBox21" width="100%">
        <tr>
            <td>
                <tr class="top">
                    <td class="left">
                        &nbsp;
                    </td>
                    <td class="center">
                        &nbsp;
                    </td>
                    <td class="right">
                        &nbsp;
                    </td>
                </tr>
                <tr class="middle">
                    <td class="left">
                        &nbsp;
                    </td>
                    <td class="center">
                        <table width="100%">
                            <tr>
                                <td>
                                    <radT:RadTreeView runat="server" ID="rtvSiteMaps" DataFieldID="WebPageId" DataFieldParentID="ParentPageId"
                                        DataSourceID="odsSiteMaps" DataTextField="Name" DataValueField="WebPageId" OnNodeDataBound="rtvSitePages_NodeDataBound"
                                        CheckBoxes="False" AllowNodeEditing="false" MultipleSelect="false" LoadingStatusPosition="AfterNodeText"
                                        OnNodeExpand="rtvSiteMaps_NodeExpand">
                                        <NodeTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <a class="delete" runat="server" onclick="DeletePage(this)" companyid='<%# Company.CompanyId %>'
                                                            pageid='<%# Eval("WebPageId") %>' visible='<%#!Convert.ToBoolean(Eval("IsMainPage")) %>'>
                                                        </a>
                                                    </td>
                                                    <td>
                                                        <a class="InfoTip1 inline" runat="server" webpageid='<%#Eval("WebPageId")%>'
                                                            id="lnkSiteMaps"><font runat="server" id="name">
                                                                <%#Eval("Name").ToString().Trim('-')%></font></a>
                                                    </td>
                                                    <td>
                                                        <a class="inline" id="lnkExternal" runat="server" href='<%# (Container.DataItem as WebPage).Url %>'
                                                            target="_new">&nbsp;</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NodeTemplate>
                                        <CollapseAnimation Type="OutQuint" Duration="100"></CollapseAnimation>
                                        <ExpandAnimation Duration="100"></ExpandAnimation>
                                    </radT:RadTreeView>
                                </td>
                                <td valign="top" align="middle">
                                    <center>
                                        <fieldset runat="server" id="fieldWebSite">
                                            <legend>Administre seu Site</legend>
                                            <br />
                                            <br />
                                            <asp:Button ID="btnNewTask" runat="server" Text="Inserir novo Item" plugin="lightbox" source='Site/WebPage.aspx?lightbox[iframe]=true' OnClientClick=" return false;" />
                                            <br />
                                            <br />
                                            <asp:Button ID="Button1" runat="server" Text="Gerenciador de Arquivos" UseSubmitBehavior="false"
                                                OnClientClick="window.open('filemanager/ckfinder.html', '_blank', 'width=' + top.$(top).width()-80 + ', height=' + top.$(top).height()-100); return false;" />
                                        </fieldset>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="right">
                        &nbsp;
                    </td>
                    <tr class="bottom">
                        <td class="left">
                            &nbsp;
                        </td>
                        <td class="center">
                            &nbsp;
                        </td>
                        <td class="right">
                            &nbsp;
                        </td>
                    </tr>
                </tr>
            </td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource ID="odsSiteMaps" runat="server" SelectMethod="GetChildPages"
        TypeName="Vivina.Erp.BusinessRules.WebSites.SiteManager" OnSelecting="odsSiteMaps_Selecting"
        OldValuesParameterFormatString="original_{0}">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="parentId" Type="Int32" />
            <asp:Parameter Name="recursive" Type="Boolean" DefaultValue="false" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
