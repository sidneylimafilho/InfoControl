<%@ Control Language="C#" AutoEventWireup="true" Inherits="Site_TagsControl" CodeBehind="Tags.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<div class="tags">
    <div class="titulo">
        Tags
    </div>
    <asp:ListView ID="list" runat="server" OnItemDataBound="list_ItemDataBound" DataSourceID="odsCategory">
        <LayoutTemplate>
            <ul class="list">
                <li runat="server" id="itemPlaceholder"></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="tag"><a runat="server" href='<%# (Container.DataItem as WebPage).Url + "?tag="+ Eval("TagName") %>'
                ><%# Eval("TagName")%>&nbsp;<span>(<%# Eval("Count")%>)</span></a> </li>
        </ItemTemplate>
    </asp:ListView>
</div>
<VFX:BusinessManagerDataSource ID="odsCategory" runat="server" SelectMethod="GetTagsByPage"
    TypeName="Vivina.Erp.BusinessRules.WebSites.SiteManager" OnSelecting="odsCategory_Selecting"
    OldValuesParameterFormatString="original_{0}">
    <selectparameters>
        <asp:Parameter Name="CompanyId" Type="Int32" />
        <asp:Parameter Name="webpageid" Type="Int32" />
    </selectparameters>
</VFX:BusinessManagerDataSource>
