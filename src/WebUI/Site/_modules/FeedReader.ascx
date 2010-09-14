<%@ Control Language="C#" AutoEventWireup="true" Inherits="Site_FeedReader" CodeBehind="FeedReader.ascx.cs" %>
<asp:Repeater ID="pageList" runat="server">
    <ItemTemplate>
        <div class="feedItem">
            <a title='<%# Eval("title") %>' href='<%#Eval("link") %>' rel="bookmark">
                <%#Eval("title")%></a><%#Eval("Html")%>
        </div>
    </ItemTemplate>
</asp:Repeater>

