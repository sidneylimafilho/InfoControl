<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Checkout_Steps.ascx.cs"
    Inherits="Vivina.Erp.WebUI.Site.Checkout_Steps" %>
<div class="passos">

    <span class="entrega <%=this.Page.ToString().Contains("basket")?"":"off" %>"></span>
    <span class="identificacao <%=this.Page.ToString().Contains("customer")?"":"off" %>"></span>
    <span class="pagamento <%=this.Page.ToString().Contains("payment")?"":"off" %>"></span>
</div>
