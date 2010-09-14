<%@ Control Language="C#" AutoEventWireup="true" Inherits="Site_CategoriesControl"
    CodeBehind="Categories.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<div class="categories">
    <div class="titulo_categorias">
        Categorias
    </div>
    <asp:ListView ID="list" runat="server">
        <LayoutTemplate>
            <ul class="categorias">
                <li runat="server" id="itemPlaceholder"></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="categoria"><a runat="server" href='<%# (Container.DataItem as Category).Url%>'>
                <%#Eval("Name")%></a>
                <asp:ListView ID="list" runat="server" DataSource='<%#(Container.DataItem as Category).Categories.OrderBy(x => x.Name) %>'>
                    <LayoutTemplate>
                        <ul class="subCategorias">
                            <li runat="server" id="itemPlaceholder"></li>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li><a runat="server" href='<%# (Container.DataItem as Category).Url%>'>
                            <%#Eval("Name")%>
                        </a></li>
                    </ItemTemplate>
                </asp:ListView>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>

