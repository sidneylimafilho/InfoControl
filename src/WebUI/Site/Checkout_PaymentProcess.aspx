<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Checkout_PaymentProcess.aspx.cs"
    Inherits="Vivina.Erp.WebUI.Site.CheckoutPaymentProcess" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder" runat="server">
  <div runat="server" id="pnlLoading">
    Processando</div>
<div runat="server" id="pnlSuccess" visible="false">
    Parabéns
</div>
</form>
<%=result.HtmlReturn ?? result.ErrorMessage %>  </asp:content>

