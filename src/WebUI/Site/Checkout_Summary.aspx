<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Checkout_Summary.aspx.cs"
    Inherits="Vivina.Erp.WebUI.Site.CheckoutSummary" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder" runat="server">
<span class="summary">
    
        <h1>Status do Pedido</h1>
        <span class="sale">
        <span class="ticket">
            <span>Numero do seu pedido:</span>
            <span class="number">00000000</span>
            <span>A confirmação do seu pedido foi enviado para o seu e-mail:</span>
            <span class="email">aadasd@asd.com</span>
        </span>
        <span class="details">
            <span class="status">Pedido Confirmado (Pendente Pagamento)</span>
            <span class="payment">
            <table>
            <tr><td>Forma de Pagamento:</td><td>Boleto</td></tr>
            <tr><td>Parcela:</td><td></td></tr>
            <tr><td>Total da Compra:</td><td></td></tr>
            </table>
            </span>
        </span>
        </span>
    
    
    
    
    <span class="text">
            <p class="boleto">
            Você optou por efetuar o pagamento através de Boleto Bancário. Você poderá então imprimir e efetuar o pagamento em qualquer agência bancária, ou então, somente anotar o número do código de barras e pagar pelo internet banking de seu banco.
            </p>
     
             <p class="term">
            O prazo de envio informado no site só começa a contar após a data de pagamento. O seu pedido somente será enviado após a confirmação do pagamento.
            </p>

        </span>
<%--<span class="info">
<h1>Informações do seu Pedido</h1>
<span></span>
<span></span>
<span></span>
<span></span>
</span>--%>
</span>

 </asp:content>
