<%@ Control Language="C#" AutoEventWireup="true" Inherits="Site_WebPage" CodeBehind="WebPage.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Src="~/App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc1" %>
<asp:Repeater ID="postList" runat="server">
    <ItemTemplate>
        <div class="page" id='post-<%#Eval("WebPageId") %>'>
            <div class="breadCrumbs">
                <%# GetBreadcrumbs(Container.DataItem as WebPage) %>
            </div>
            <div class="head">
                <h1>
                    <a title='Permanent Link to <%# Eval("Name") %>' href='<%# ResolveUrl((Container.DataItem as WebPage).Url) %>'
                        rel="bookmark">
                        <%#Eval("Name")%></a></h1>
                <span class="author">
                    <%#Eval("User.Profile.AbreviatedName")%>
                </span><span class="date"><span class="day">
                    <%#Convert.ToDateTime(Eval("PublishedDate")).Day%></span> <span class="month">
                        <%#Convert.ToDateTime(Eval("PublishedDate")).ToString("MMM").ToUpper()%></span>
                    <span class="year">
                        <%#Convert.ToDateTime(Eval("PublishedDate")).Year%></span> </span>&nbsp;
                
                <a href='<%# ResolveUrl((Container.DataItem as WebPage).Url) %>#comments' class="comment" runat="server"  Visible='<%#CanShowComments(Eval("CanComment"), Eval("WebPageId")) %>' >
                    <uc1:Comments ID="Comments" runat="server" PageName="comments.aspx" SubjectId='<%#Eval("WebPageId") %>'
                        ShowStatistics="true" />
                </a>
            </div>
            <div class="body">
                <%# Eval("Description")%>
            </div>
            <div class="foot">
                <asp:ListView runat="server" DataSource='<%#(Container.DataItem as WebPage).PageTags.Select(x => x.Name).ToArray() %>'>
                    <LayoutTemplate>
                        <span class="tag">
                            <label>
                                Tags:</label>
                            <span runat="server" id="itemPlaceHolder"></span></span>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <span><a title='<%# Container.DataItem %>' href='?tag=<%# Server.UrlEncode((string) Container.DataItem ) %>'
                            rel="bookmark">
                            <%#Container.DataItem%></a></span>
                    </ItemTemplate>
                    <ItemSeparatorTemplate>
                        ,&nbsp;
                    </ItemSeparatorTemplate>
                </asp:ListView>                
            </div>
            <uc1:Comments ID="Comments1" runat="server" PageName="comments.aspx" ShowButtons="true"
                SubjectId='<%#Eval("WebPageId") %>' Visible='<%#CanShowComments(Eval("CanComment"), Eval("WebPageId")) %>' />
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:ListView ID="pageListLinks" runat="server">
    <LayoutTemplate>
        <div class="seeMore">
            <ul>
                <li runat="server" id="itemPlaceHolder"></li>
            </ul>
        </div>
    </LayoutTemplate>
    <ItemTemplate>
        <li><a title='<%# Eval("Name") %>' href='<%# ResolveUrl((Container.DataItem as WebPage).Url) %>'
            rel="bookmark">
            <%#Eval("Name")%></a></li>
    </ItemTemplate>
</asp:ListView>
<asp:Repeater ID="pageListArchived" runat="server">
    <ItemTemplate>
        <a class="webLinkList" title='<%# Eval("Date") %> - <%# Convert.ToDateTime(Eval("Date")).AddMonths(1).AddDays(-1) %>'
            href='?type=blog&month=<%# Convert.ToDateTime(Eval("Date")).ToShortDateString() %>&max=<%#Eval("Count")%>'
            rel="bookmark">
            <%#Convert.ToDateTime(Eval("Date")).ToString("MMM yyyy").ToUpper()%>&nbsp;(<%#Eval("Count")%>)</a><br />
    </ItemTemplate>
</asp:Repeater>
<div id="pageView" runat="server" class="page"></div>
