<%@ Page EnableEventValidation="false" EnableViewStateMac="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Administration_Company_Supplier" Title="Fornecedor"
    CodeBehind="Supplier.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div id="lblTitle" runat="server">
    </div>
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href="Supplier_General.aspx?SupplierId=<%=Request["SupplierId"]%>" target="tabContent">
                Geral </a></li>
            <li><a href="Contacts.aspx?SupplierId=<%=Request["SupplierId"]%>" target="tabContent">
                Contatos </a></li>
        </ul>
        <iframe id="tabContent" name="tabContent" src="Supplier_General.aspx?SupplierId=<%=Request["SupplierId"] %>">
        </iframe>
    </div>
</asp:Content>
