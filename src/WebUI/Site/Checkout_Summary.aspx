<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Checkout_Summary.aspx.cs"
    Inherits="Vivina.Erp.WebUI.Site.CheckoutSummary" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder" runat="server">
<div class="summary">
    
        <h1>Status do Pedido</h1>
        <div class="sale">
        <div class="ticket">
            <div>Numero do seu pedido:</div>
            <div class="number"><%=Sale.SaleId %></div>
            <div>A confirmação do seu pedido foi enviado para o seu e-mail:</div>
            <div class="email"><%=User.Identity.Email %></div>
        </div>
        <div class="details">
            <div class="status">Pedido Confirmado (Pendente Pagamento)</div>
            <div class="payment">
            <table>
            <tr><td>Forma de Pagamento:</td><td><%=Sale.FinancierOperation.PaymentMethod.Name %></td></tr>
            <tr><td>Parcela:</td><td><%=Sale.Invoice.Parcels.Count() + " x " + Sale.Invoice.Parcels.Average(x => x.Amount).ToString("c")%> </td></tr>
            <tr><td>Total da Compra:</td><td><%=Sale.Total.ToString("c")%></td></tr>
            </table>
            
            <div><a href="Checkout_PaymentProcess.aspx?sale=<%=Sale.SaleId %>" target="_blank">Clique aqui para realizar o pagamento!</a></div>
            </div>
        </div>
        </div>
    
    
    
    
    <div class="text">
    
    <%if (Sale.FinancierOperation.PaymentMethodId == PaymentMethod.Boleto)
      { %>
            <p>
            Você optou por efetuar o pagamento através de Boleto Bancário. Você poderá então imprimir e efetuar o pagamento em qualquer agência bancária, ou então, somente anotar o número do código de barras e pagar pelo internet banking de seu banco.
            </p>
            <%} %>
     
             <p class="term">
            O prazo de envio informado no site só começa a contar após a data de pagamento. O seu pedido somente será enviado após a confirmação do pagamento.
            </p>

        </div>
<%--<div class="info">
<h1>Informações do seu Pedido</h1>
<div></div>
<div></div>
<div></div>
<div></div>
</div>--%>
</div>

 </asp:content>
