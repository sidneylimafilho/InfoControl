<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="POS_Company_Inventory" Title="Inventário" CodeBehind="Inventory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    
    <div class="tabs cTabs11" style="padding-left: 10px;">
        <ul class="tabNavigation">
            <li><a href="Inventory_General.aspx?ProductId=<%=Request["ProductId"] %>&InventoryId=<%=Request["InventoryId"] %>&DepositId=<%=Request["DepositId"] %>" target="tabContent">
                Inventário</a></li>
            <li><a href="Inventory_History.aspx?ProductId=<%=Request["ProductId"] %>&InventoryId=<%=Request["InventoryId"] %>&DepositId=<%=Request["DepositId"] %>" target="tabContent">
                Histórico</a></li>
            <li><a href="Inventory_Serial.aspx?ProductId=<%=Request["ProductId"] %>&InventoryId=<%=Request["InventoryId"] %>&DepositId=<%=Request["DepositId"] %>" target="tabContent">
                Serial</a></li>
        </ul>
    <iframe id="tabContent" name="tabContent" src="Inventory_General.aspx?ProductId=<%=Request["ProductId"] %>&DepositId=<%=Request["DepositId"] %>">
    </iframe>
</div>
</asp:Content>
