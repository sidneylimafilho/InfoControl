<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Accounting_CashFlow" CodeBehind="CashFlow.aspx.cs" Title="Fluxo de Caixa" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td class="center">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td class="right">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <%-- 
                Aqui explicarei como funciona cada detalhe desta tela super dinamica usando o smart client

                1) É importante os controles HTML terem seu próprio ID, igual no ASP.NET pois o jquery usará para não conflitar com outras áreas do html
                2) Esta tela é dividida em duas partes uma DIV que será o filtro e uma TABLE que será o resultado.

                3) A div irá guardar os dados numa variável global que funciona como uma Session só que do lado do cliente, esta variável se chama PARAMS,
                   a forma de guardar funciona da seguinte maneira.

                   a) Configure os campos textbox, dropdown, etc... adicionando um atributo chamado PARAM no singular (no caso do exemplo abaixo PARAM=YEAR), logo
                      automaticamente o javascript irá guardar o conteúdo do controle. Com um código parecido com este: PARAMS["YEAR"]=textbox.value; Note que o 
                      attributo PARAM terá a chave que guardará a informação.

                   b) Configure com o atributo COMMAND o controle que vocÊ deseja que ao clicar dispare a requisição no servidor. Lembrando esta requisição pode 
                      ser feita diretamente atraves do atributo ACTION ou disparar um outro controle através do atributo TRIGGER. No nosso exemplo abaixo o filtro 
                      irá disparar o controle TABLE, repare que o elemento <div id="filterGrid" trigger="#cashFlow"> configura um atributo trigger, indicando o seletor
                      jQuery do controle que tem o atributo ACTION


                --%>   
                
                <%-- 
                    Foi colocada um atributo options para que o param accountPlanId seja limpo a cada vez que fizer uma nova busca
                    assim sempre começa a busca pelos planos de contas que tenham parentId null
                     --%>
                <div asform="true" id="filterGrid" trigger="#cashFlow" options="{accountPlanId:''}">
                    Escolha o ano:
                 
       
                    <input type="text" name="year" />
                    <asp:Button runat="server" command="click" OnClientClick="return false;" UseSubmitBehavior="false"
                        Text="Carregar" />
                </div>
                <br />
                <br />
                <%-- 
                
                Vamos ao controle TABLE que irá popular os resultados encontrados.

                A principal caracteristica é que ele possui um atributo ACTION que determina qual url irá disparar no servidor para buscar os dados. Vamos detalhar 
                cada atributo para facilitar o entendimento
                
                ACTION="<%=ResolveUrl("~/controller/accounting/GetCashFlowByYear") %>" 
                # Aponta para o controller que irá retornar os dados

                AUTO="true"  
                # Indica que irá disparar assim que carregar o HTML

                PARAMS="{'companyId':1, 'year':2008}" 
                # Informa que deve adicionar a variável global PARAMS as chaves "companyId" e "year", com os valores 1 e 2008,
                # seria o equivalente a seguinte instrução: PARAMS["companyId"] = 1 e PARAMS["year"] = 2008

                ONSUCESS="if (result.Data.length>0){ $('#cashFlow tfoot').html($('#cashFlow tfoot').template(total)).show() } else { $('#cashFlow tfoot').hide();}"
                # Evento que ocorre sempre quando a requisição no servidor retorna com sucesso. Sempre passa 3 parametros
                # result: Objeto de retorno contendo o resultado da requisição, para facilitar a comunicação com o SmartClient foi criado um objeto 
                #         ClientResponse no .NET que será explicado futuramente
                # status: Uma variável integer que indica o status da requisição, geralmente retorna 200
                # req:    Objeto HttpRequest responsável pela requisição
                #
                # No caso do exemplo acima está indicando o seguinte, se o resultado contiver dados "if (result.Data.length>0)" então pega a 
                # tag "$('#cashFlow tfoot')", coloca o html do template processado "$('#cashFlow tfoot').template(total)" com a variável total, que foi criada 
                # durante o template principal "#cashFlow .template"   e apresenta ".show()" caso contrário oculta o rodapé "$('#cashFlow tfoot').hide();"

                EmptyTemplate="#cashFlow .empty"
                # Define um template caso a requisição não retorne dados

                template="#cashFlow .template"
                # Define o template principal que será executado a cada requisição do ACTION da table

                target="#cashFlow .body"
                # Indica onde o template será colocado após ser processado com dados da requisição

                Agora que você viu as configurações da TABLE vamos para o TEMPLATE em <tbody class="template">

                --%>
                <table id="cashFlow" border="1" class="cGrd11" width="100%" command="load" source="AccountingService.svc"
                    action="GetCashFlowByYear" onsucess="if (result.Data.length>0){ $('#cashFlow thead').show(); $('#cashFlow tfoot').html($('#cashFlow tfoot').render(total)).show();}else{$('#cashFlow tfoot, #cashFlow thead').hide();}"
                    selectedcss="" emptytemplate="#cashFlow .empty" template="#cashFlow .template"
                    target="#cashFlow .body">
                    <thead>
                        <tr>
                            <th>
                                Plano de Contas
                            </th>
                            <th>
                                Janeiro
                            </th>
                            <th>
                                Fevereiro
                            </th>
                            <th>
                                Março
                            </th>
                            <th>
                                Abril
                            </th>
                            <th>
                                Maio
                            </th>
                            <th>
                                Junho
                            </th>
                            <th>
                                Julho
                            </th>
                            <th>
                                Agosto
                            </th>
                            <th>
                                Setembro
                            </th>
                            <th>
                                Outubro
                            </th>
                            <th>
                                Novembro
                            </th>
                            <th>
                                Dezembro
                            </th>
                        </tr>
                    </thead>
                    <tbody class="template">
                        <%-- 

                        Interessante reparar que o template é um conjunto normal de tags TR e TD de uma tabela, porém dentro da tag TR contém 
                        atributos do SmartClient, isso significa que quando o template for renderizado ele irá ganhar vida e implementará outras caracteristicas
                        No caso abaixo ele contém os seguintes atributos:

                        COMMAND="load"
                        # Isto torna cada linha gerada, clicável, ou seja cada linha criada pelo template poderá ser clicada e irá disparar o action da tag pai
                        # mais próxima que no caso é da TABLE. O "load" pode ser um nome qualquer, que só serve para identificar o objetivo da ação

                        ONCE="true"
                        # Este atributo indica que esta linha só pode ser clicada uma vez e não dispará mais requisição, serve para evitar que o template gere
                        # inumeras requisições e fique adicionando HTML exageradamente e sem controle. Aconselho experimentar tirar este atributo para perceber 
                        # o efeito que ocorre

                        PARAMS="{`accountPlanId`:<$=AccountingPlanId$>}"
                        # Aqui temos uma coisa interessante, este atributo está informando que ao clicar deve adicionar o resultado que virá atraves da variável
                        # AccountingPlanId, ou seja, quando a TABLE for preenchida pela primeira vez o SmartClient irá substituir a tag <$=AccountingPlanId$> por um
                        # valor, logo o html se tornará por exemplo {`accountPlanId`:1234}, já que o html agora tem um valor, é este valor que irá para a variável
                        # PARAMS global

                        TARGET="this"
                        # O atributo target nos indica onde o html será colocado, como não queremos colocar o conteúdo dos filhos de cada linha na TBODY então 
                        # indicamos para usar a própria TR como referencia 

                        MODE="after"
                        # O atributo MODE serve para indicar qual será a forma de adicionar conteúdo a tela, no nosso caso será colocado depois da linha "after", isso
                        # significa que o resultado do click nessa linha ao invés de adicionar ao TBODY pai, irá colocar novas TR depois desta. Este efeito será 
                        # crucial para nosso exemplo, assim dará o efeito de "arvore" se expandindo

                        --%>
                        <!-- 
                        <tr once="true" options="{'accountPlanId':<$=AccountingPlanId$>}" style="cursor:pointer;" 
                            target="this" mode="after" emptytemplate="clear">
                            <td><$=Name$></td>
                            <td><$=$.format(January, "c")$></td>
                            <td><$=$.format(February,"C")$></td>
                            <td><$=$.format(March,"C")$></td>
                            <td><$=$.format(April,"C")$></td>
                            <td><$=$.format(May,"C")$></td>
                            <td><$=$.format(June,"C")$></td>
                            <td><$=$.format(July,"C")$></td>
                            <td><$=$.format(August,"C")$></td>
                            <td><$=$.format(September,"C")$></td>
                            <td><$=$.format(October,"C")$></td>
                            <td><$=$.format(November,"C")$></td>
                            <td><$=$.format(December,"C")$></td>
                        </tr>
                        -->
                        <%-- 
                        Aqui está uma parte interessante do conteúdo do template, pois não estamos escrevendo HTML nenhum, ao invés disso estamos programando pois 
                        todo o conteúdo dentro das tags <$ $> são código javascript processado pelo template, no exemplo abaixo estamos criando uma variável "total"
                        que acumulará os valores de cada coluna. 
                        
                        Vamos entender melhor o macete -> window.total = window["total"] || {} ?

                        window.total será a variável que terão os totais, caso window["total"] seja undefined então window.total receberá um objeto vazio {}. Para
                        facilitar vamos escrever um código similar
                                                
                        if(window["total"] == undefined)
                            window.total = {};
                        else
                            window.total = window["total"];

                        
                        Assim estamos criando a variável global "TOTAL", se lembra do $('#cashFlow tfoot').template(total), se não tiver lembrando leia 
                        novamente a linha 70.
                        

                        Depois para cada mês temos o seguinte:
                        total.January = (total.January || 0) + January; 

                        Onde January é uma variável que contém o valor de Janeiro, e novamente temos o macete (total.January || 0), que verifica se total.January 
                        for undefined retorna 0 e soma com o valor atual de January

                        --%>
                        <!--
                        <$
                        window.total = window["total"] || {};
                        total.January = (total.January || 0) + January;
                        total.February = (total.February || 0) + February;
                        total.March = (total.March || 0) + March;
                        total.April = (total.April || 0) + April;
                        total.May = (total.May || 0) + May;
                        total.June = (total.June || 0) + June;
                        total.July = (total.July || 0) + July;
                        total.August = (total.August || 0) + August;
                        total.September = (total.September || 0) + September;
                        total.October = (total.October || 0) + October;
                        total.November = (total.November || 0) + November;
                        total.December = (total.December || 0) + December;                        
                        $>
                       -->
                    </tbody>
                    <%--  Modelo de HTML para quando não houver resultados disponiveis na consulta.  --%>
                    <tbody class="empty" style="display: none;">
                        <tr>
                            <td colspan="99">
                                Nenhum resultado encontrado!
                            </td>
                        </tr>
                    </tbody>
                    <%-- Aqui será o lugar onde entrará o html gerado pelo resultado da consulta --%>
                    <tbody class="body">
                    </tbody>
                    <%-- Aqui está o rodapé da tabela, que será gerado pelo código descrito na linha 70, usando a variável TOTAL criada --%>
                    <tfoot style="font-weight: bold">
                        <!--
                        <tr>
                            <td>SubTotal</td>
                            <td><$=$.format(total.January || 0, "c")$></td>
                            <td><$=$.format(total.February || 0, "c")$></td>
                            <td><$=$.format(total.March || 0, "c")$></td>
                            <td><$=$.format(total.April || 0, "c")$></td>
                            <td><$=$.format(total.May || 0, "c")$></td>
                            <td><$=$.format(total.June || 0, "c")$></td>
                            <td><$=$.format(total.July || 0, "c")$></td>
                            <td><$=$.format(total.August || 0, "c")$></td>
                            <td><$=$.format(total.September || 0, "c")$></td>
                            <td><$=$.format(total.October || 0, "c")$></td>
                            <td><$=$.format(total.November || 0, "c")$></td>
                            <td><$=$.format(total.December || 0, "c")$></td>
                        </tr>
                        <$                      
                        
                        total.February = total.February + total.January;
                        total.March = total.March + total.February;
                        total.April = total.April  + total.March;
                        total.May = total.May  + total.April;
                        total.June = total.June  + total.May;
                        total.July = total.July + total.June;
                        total.August = total.August  + total.July;
                        total.September = total.September + total.August;
                        total.October = total.October  + total.September;
                        total.November = total.November  + total.October;
                        total.December = total.December + total.November;
                        
                        $>
                        <tr>
                            <td>Total Acumulado:</td>
                            <td><$=$.format(total.January, "c")$></td>
                            <td><$=$.format(total.February, "c")$></td>
                            <td><$=$.format(total.March, "c")$></td>
                            <td><$=$.format(total.April, "c")$></td>
                            <td><$=$.format(total.May, "c")$></td>
                            <td><$=$.format(total.June, "c")$></td>
                            <td><$=$.format(total.July, "c")$></td>
                            <td><$=$.format(total.August, "c")$></td>
                            <td><$=$.format(total.September, "c")$></td>
                            <td><$=$.format(total.October, "c")$></td>
                            <td><$=$.format(total.November, "c")$></td>
                            <td><$=$.format(total.December, "c")$></td>
                        </tr>
                        <$ window.total = {};$>
                        -->
                    </tfoot>
                </table>
                <uc2:ToolTip ID="tipCashFlow" runat="server" Message="Garanta a qualidade de vida do seu negócio.No fluxo de caixa que o sistema mostra algumas das informações mais importantes."
                    Title="Dica:" Indication="top" Top="90px" Left="200px" Visible="true" />
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
