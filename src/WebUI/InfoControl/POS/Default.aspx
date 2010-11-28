<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" 
Inherits="InfoControl_POS_Default" Title="Vendas" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table style="width: 100%;  text-align: center;">
        <tr>
            <td>
                <table border="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="ProspectBuilder.aspx"
                                CssClass="ProspectBuilder HelpTip">&nbsp;
                                <span class="buttonTip">
                                <h1>Ajuda:</h1>
                                Nessa tela você pode criar uma proposta de venda do  seu produto para um cliente/prospect. Essa proposta não é uma venda. Você pode definir o preço de venda durante a elaboração da proposta . O InfoControl armazena essa proposta , para que você possa convertê-la em venda , ou posteriormente alterá-la .<br /><br />
Só é possível elaborar uma proposta para um cliente cadastrado .  

                                <span class="footer"></span></span>
                                
                            </asp:LinkButton>
                        </td>
                        <td>
                            <div style="background: url(../../App_Shared/themes/glasscyan/Company/flow_right.gif) no-repeat;
                                background-position: center right; width: 50px; height: 32px;">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton5" runat="server" PostBackUrl="Sale.aspx" CssClass="Sales HelpTip">&nbsp;
                            
                            <span class="buttonTip">
                                <h1>Ajuda:</h1>
                                Aqui , você faz a VENDA . Quem trabalha com vendas diretas ao público , pode fazer vendas sem necessidade de cadastro prévio do cliente, e poderá imprimir seu CUPOM FISCAL o InfoControl tem integração com impressoras de CUPOM FISCAL . Em caso de dúvidas , entre em contato conosco ! Para empresas que utilizam NOTA FISCAL em formulário contínuo            ( IMPRESSO ou MANUAL ) , utilize a integração com NOTA FISCAL do InfoControl no próximo passo. <br /><br />
Durante a finalização das vendas , você pode definir a forma e os prazos de pagamento , sendo que , para toda venda efetuada sem cliente cadastrado, não serão computadas CONTAS À RECEBER ( pois , já foi recebido ou estão com pendências na empresa , como contas por cartão de crédito , quem deve à sua empresa é a prestadora) . Sempre que a VENDA for feita à um CLIENTE cadastrado no InfoControl ,  o sistema gerará automaticamente uma CONTA À RECEBER , para a melhor gestão do seu relacionamento com o cliente – isso será extremamente útil nas vendas à prazo , estas vendas não devem ser feitas sem o cadastro prévio do cliente. <br /><br />
Os próximos passos são : a emissão da NOTA FISCAL (caso não tenha sido emitido o CUPOM FISCAL ) e o controle de recebidos feitos no CONTAS À RECEBER .


                                <span class="footer"></span></span>
                                
                            </asp:LinkButton>
                        </td>
                        <td>
                            <div style="background: url(../../App_Shared/themes/glasscyan/Company/flow_right.gif) no-repeat;
                                background-position: center right; width: 50px; height: 32px;">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton8" runat="server" PostBackUrl="../Accounting/Receipt.aspx"
                                CssClass="NotaFiscal HelpTip">&nbsp;
                                
                                <span class="buttonTip">
                                <h1>Ajuda:</h1>
                                A nota fiscal pode ser relativa à uma ou mais vendas e/ou serviços. Assim, para emitir uma nota fiscal, após você informar o nome do cliente, o InfoControl resgata todas as vendas e os serviços que ainda não têm nota fiscal cadastrada , e exibe em “ IMPORTAR OS/VENDA : “ , você pode fazer um faturamento por período , onde você cria uma única nota fiscal para todas as vendas/serviços de um determinado período , ou cadastrar uma nota fiscal por venda/serviço – é totalmente opcional.
<br /><br />A NOTA FISCAL não é necessariamente uma venda/serviço, pode também ser uma simples remessa , devolução ou qualquer outra operação fiscal que envolva NOTA FISCAL .
<br /><br />Será utilizada como cadastro sempre que a empresa tenha como procedimento padrão, o uso de notas fiscais não automatizadas ( escritas manualmente , ou datilografadas ) , para que o sistema tenha sempre uma réplica da original e , controle de forma adequada seus impostos. Nesse caso, insira aqui os mesmos dados inseridos na nota fiscal que foi emitida.
<br /><br />Geração de notas fiscais, sempre que a nota fiscal for emitida pelo InfoControl, você deverá cadastrar tudo aqui, e autorizar a impressão pelo próprio InfoControl ( a NOTA FISCAL , deve atender à padrões federais , por isso , para que você possa imprimir uma nota fiscal através do InfoControl , entre em contato conosco, para que possamos conduzi-lo nesse processo ) .


                                <span class="footer"></span></span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <div style="background: url(../../App_Shared/themes/glasscyan/Company/flow_bottom.gif) no-repeat;
                                width: 24px; background-position: bottom; height: 32px;">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <div style="background: url(../../App_Shared/themes/glasscyan/Company/flow_top.gif) no-repeat;
                                width: 24px; height: 32px;">
                                &nbsp;
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton15" runat="server" PostBackUrl="../Accounting/Invoice.aspx"
                                CssClass="Invoice HelpTip">&nbsp;
                                
                                <span class="buttonTip">
                                <h1>Ajuda:</h1>
                                Aqui , você fará as conferências de cada um de seus recebimentos . Aqui , você saberá tudo o que tem à receber das operadoras de cartão , dos seus clientes, etc. O InfoControl é tão simples que as vendas cadastradas com prazo , geram automaticamente uma conta à receber, para que você tenha somente o trabalho de conferir... Se quiser, pode conferir também diretamente na CONCILIAÇÃO BANCÁRIA , assim, você vê o seu extrato no InfoControl e também no banco, compara , e pronto !
                                <span class="footer"></span></span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton12" runat="server" PostBackUrl="SaleList.aspx" CssClass="SalesHistory HelpTip">&nbsp;
                            
                            <span class="buttonTip">
                                <h1>Ajuda:</h1>
                                Aqui , você pode visualizar todas as suas vendas de um período determinado por você. Para gerar uma NOTA FISCAL à partir daqui , basta clicar em GERAR NOTA FISCAL , e seguir para o passo seguinte .<br /><br />
Essa tela também é usada em caso de consulta à notas fiscais para outros motivos , como troca , ou consulta de histórico – para a consulta ao histórico, todo cliente , na sua tela de cadastro tem uma aba chamada HISTÓRICO DE COMPRAS e outra chamadas HISTÓRICO DE PAGAMENTOS , mais detalhado para um acompanhamento minucioso do cliente.


                                <span class="footer"></span></span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="LinkButton7" runat="server" PostBackUrl="../Administration/Customer.aspx"
                                CssClass="Customer">&nbsp;</asp:LinkButton>
                        </td>
                      <%--  <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="Stock.aspx" CssClass="Stock">&nbsp;</asp:LinkButton>
                        </td>--%>
                        <td>
                            <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="StockTransfer.aspx"
                                CssClass="Transfer">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton6" runat="server" PostBackUrl="Exchange.aspx" CssClass="Exchange">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="StockRMA.aspx" CssClass="RMA">&nbsp;</asp:LinkButton>
                        </td>
                        </tr><tr>
                        <td>
                            <asp:LinkButton ID="LinkButton10" runat="server" PostBackUrl="DropPayout.aspx" CssClass="Sangria">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton11" runat="server" PostBackUrl="MapOfSales.aspx" CssClass="SalesMap">&nbsp;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton13" runat="server" PostBackUrl="DropPayoutReport.aspx"
                                CssClass="FechamentoCaixa">&nbsp;</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
