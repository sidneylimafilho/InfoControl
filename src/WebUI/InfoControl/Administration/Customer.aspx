<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" ViewStateEncryptionMode="Never" Inherits="Company_Customer"
    CodeBehind="Customer.aspx.cs" Title="Cliente" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href="Customer_General.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Dados Gerais</a></li>
            <li><a href="Customer_Equipment.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Equipamentos</a></li>
            <li><a href="Customer_Payment.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Histórico de Pagamentos</a></li>
            <li><a href="Contacts.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Contatos</a></li>
            <li><a href="Contracts.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Contratos</a></li>
            <li><a href="Customer_Sales.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Histórico de Compras</a></li>
            <li><a href="CustomerCalls.aspx?CustomerId=<%=Request["CustomerId"] %>" target="tabContent">
                Chamados</a></li>
        </ul>
      
        <iframe id="tabContent" name="tabContent" src="Customer_General.aspx?CustomerId=<%=Request["CustomerId"] %>">
        </iframe>
    </div>
</asp:Content>
