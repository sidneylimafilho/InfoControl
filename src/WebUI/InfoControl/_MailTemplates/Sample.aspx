<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/_mailtemplates/MailTemplate.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>
                    <b>Olidia</b>, venho por meio deste relatar a cobrança devida ao Desenvolvimento<span
                        class="Apple-converted-space">&nbsp;</span> de uma<br />
                    rotina de migração de informações entre a base de dados da Venetillo e o sistema
                    de loja virtual. O escopo total<br />
                    da proposta dessa rotina importa dados como lista de produtos, imagens, estoque,
                    etc, no valor total de R$ 2000,00.</p>
                <p>
                    A loja virtual que recebe esses dados, já está no ar a 3 meses e ainda não foi<span
                        class="Apple-converted-space">&nbsp;</span> registrado nenhum pagamento. A Loja
                    Virtual<br />
                    hoje utiliza bastante recursos dos nossos servidores, com trafego de informações
                    entre as bases e com armazenamento de<br />
                    imagens, porém não dá continuar a permitir a loja permanecer no ar sem pagamentos.</p>
                <p>
                    O custo de uma loja virtual com a quantidade de produtos da Venetillo está demonstrada
                    na tabela abaixo.</p>
                <table align="center" style="font-family: 'Segoe UI'; font-size: 14px !important;
                    color: rgb(0, 68, 68);">
                    <tr>
                        <td colspan="2" style="border: 1px solid rgb(0, 68, 68);">
                            <font size="4">Items</font>
                        </td>
                        <td style="border: 1px solid rgb(0, 68, 68); text-align: center;">
                            <font size="4">&nbsp;&nbsp;&nbsp; Val. Unit&nbsp; </font>
                        </td>
                        <td style="border: 1px solid rgb(0, 68, 68); text-align: center;">
                            <font size="4">Qtd</font>
                        </td>
                        <td style="border: 1px solid rgb(0, 68, 68); text-align: center;">
                            <font size="4">Total</font>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border: 1px solid rgb(0, 68, 68);">
                            Manutenção de Site:&nbsp;
                        </td>
                        <td style="border: 1px solid rgb(0, 68, 68); text-align: right;">
                            R$ 50,00
                        </td>
                        <td style="border: 1px solid rgb(0, 68, 68); text-align: center;">
                            1
                        </td>
                        <td style="border: 1px solid rgb(0, 68, 68); text-align: right;">
                            R$ 50,00
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="border: 1px solid rgb(0, 68, 68);">
                            Manutenção de Loja Virtual:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp; <span class="Apple-converted-space">&nbsp;&nbsp;</span>
                        </td>
                        <td>
                            &nbsp;&nbsp; Produto
                        </td>
                        <td colspan="1" style="text-align: right;">
                            &nbsp;R$ 0,04
                        </td>
                        <td style="text-align: center;">
                            &nbsp;&nbsp;&nbsp; 25651&nbsp; &nbsp; <span class="Apple-converted-space">&nbsp;&nbsp;</span>
                        </td>
                        <td style="text-align: right;">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; R$ 1026,04
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;&nbsp; Imagem do Produto
                        </td>
                        <td colspan="1" style="text-align: right;">
                            &nbsp;R$ 0,02
                        </td>
                        <td style="text-align: center;">
                            2502
                        </td>
                        <td style="text-align: right;">
                            R$ 50,04
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            Formas de Pagamento:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;&nbsp; MasterCard
                        </td>
                        <td style="text-align: right;">
                            R$ 30,00
                        </td>
                        <td style="text-align: center;">
                            0
                        </td>
                        <td style="text-align: right;">
                            R$ 0,00
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;&nbsp; Visa
                        </td>
                        <td style="text-align: right;">
                            R$ 30,00
                        </td>
                        <td style="text-align: center;">
                            0
                        </td>
                        <td style="text-align: right;">
                            R$ 0,00
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            &nbsp;&nbsp; American Express
                        </td>
                        <td style="text-align: right;" valign="top">
                            R$ 30,00
                        </td>
                        <td style="text-align: center;" valign="top">
                            0
                        </td>
                        <td style="text-align: right;" valign="top">
                            R$ 0,00
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                            &nbsp;&nbsp; Boleto
                        </td>
                        <td style="text-align: right;" valign="top">
                            R$ 10,00
                        </td>
                        <td style="text-align: center;" valign="top">
                            0
                        </td>
                        <td style="text-align: right;" valign="top">
                            R$ 0,00
                        </td>
                    </tr>
                    <tr>
                        <td style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);"
                            valign="top">
                        </td>
                        <td style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);"
                            valign="top">
                            &nbsp;&nbsp; PagSeguro/PagCerto
                        </td>
                        <td style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);
                            text-align: right;" valign="top">
                            R$ 10,00
                        </td>
                        <td style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);
                            text-align: center;" valign="top">
                            0
                        </td>
                        <td style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);
                            text-align: right;" valign="top">
                            R$ 0,00
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);"
                            valign="top">
                            <font size="4">Total:</font>
                        </td>
                        <td style="border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(0, 68, 68);
                            text-align: right;" valign="top">
                            <font size="4">R$ 1126,08</font>
                        </td>
                    </tr>
                </table>
                <p>
                    Esse valor porque a Venetillo, pegou o plano antigo e com valores desatualizados.
                    Se fosse montar uma loja hoje com a mesma quantidade de produtos sairia por mais
                    do que o dobro disso.</p>
                <p>
                    Olidia, peço que entre em contato com o pessoal da Venetillo, pois senão terei que
                    desativar todo o ambiente.</p>
</asp:Content>
