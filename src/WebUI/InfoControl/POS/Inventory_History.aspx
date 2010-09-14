<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Inventory_History.aspx.cs" Inherits="Vivina.Erp.WebUI.POS.Inventory_History" %>

<%@ Register Src="InventoryHistory.ascx" TagName="InventoryHistory" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <uc2:InventoryHistory ID="InventoryHistory1" runat="server" />
</asp:Content>
