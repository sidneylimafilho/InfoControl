<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_LeafBox" Codebehind="LeafBox.ascx.cs" %>
<table class="<%=this.CssClass %>" width="100%">
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
            <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
        </td>
        <td class="right">
            &nbsp;
        </td>
    </tr>
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
</table>
