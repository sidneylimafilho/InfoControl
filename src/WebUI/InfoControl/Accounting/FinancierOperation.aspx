<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" EnableViewStateMac="false" Inherits="InfoControl_Accounting_FinancierOperation"
    Title="Forma de Pagamento" CodeBehind="FinancierOperation.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
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
        <tr class="middle">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <!-- conteudo -->
                <table width="100%">
                    <tr>
                        <td>
                            Tipo de pagamento:&nbsp;<VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                                <ItemTemplate>
                                    <h3>
                                        Ajuda:</h3>
                                    O tipo da forma de pagamento que deseja receber os pagamentos indicarão as possibilidades
                                    para seus vendedores não praticarem formas muito diversificadas de recebimento,
                                    além de deixar o processo de venda e faturamento mais simples.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <asp:DropDownList ID="cboPaymentMethod" runat="server" DataSourceID="odsPaymentMethod"
                                DataTextField="Name" DataValueField="PaymentMethodId" AppendDataBoundItems="True">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboPaymentMethod" ControlToValidate="cboPaymentMethod"
                                runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="SaveFinancierOperation"></asp:RequiredFieldValidator>
                            <VFX:BusinessManagerDataSource ID="odsPaymentMethod" runat="server" SelectMethod="GetAllPaymentMethod"
                                TypeName="Vivina.Erp.BusinessRules.AccountManager">
                            </VFX:BusinessManagerDataSource>
                        </td>
                        <td>
                            Conta Bancária Preferencial de Recebimento:&nbsp;<VFX:HelpTooltip runat="server">
                                <ItemTemplate>
                                    <h3>
                                        Ajuda:</h3>
                                    Ao informar uma conta preferencial de recebimento, facilita ao fazer a verificação
                                    das contas recebidas que você deseja encaminhar para uma conta bancária, assim você
                                    pode designar uma conta bancária que receberá sempre transações de uma forma de
                                    pagamento.
                                    <br />
                                    <br />
                                    Obs: A qualquer momento você pode escolher uma outra conta bancária no ato do recebimento
                                    ou pagamento.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <asp:DropDownList ID="cboAccount" runat="server" DataSourceID="odsAccounts" DataTextField="ShortName"
                                DataValueField="AccountId" AppendDataBoundItems="True">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator1" ControlToValidate="cboAccount"
                                runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="SaveFinancierOperation"></asp:RequiredFieldValidator>
                            <VFX:BusinessManagerDataSource ID="odsAccounts" runat="server" SelectMethod="GetAccountsWithShortName"
                                TypeName="Vivina.Erp.BusinessRules.AccountManager" onselecting="odsAccounts_Selecting">
                                <selectparameters>
                                    <asp:Parameter Name="companyId" />
                                </selectparameters>
                            </VFX:BusinessManagerDataSource>
                        </td>
                        <td>
                            Taxa de Administração:&nbsp;<VFX:HelpTooltip ID="HelpTooltip4" runat="server">
                                <ItemTemplate>
                                    <h3>
                                        Ajuda:</h3>
                                  
                                  Esse é um valor fixo que representa a taxa que deve ser paga mensalmente para a operadora do cartão,
                                  por serviços de pagamento on-line por exemplo. Como pode-se notar esse valor é mais utilizado 
                                  quando se trata de pagamentos com cartão.
                                  
                                  <br/>
                                  <br/>

                                  Obs.: O valor dessa taxa é um valor inteiro; não utilize valores percentuais.                                  
                                  
                                </ItemTemplate>
                            </VFX:HelpTooltip> <br />
                            <uc2:CurrencyField ID="ucCurrFieldAdminTax" MaxLength="6" Required="true" ValidationGroup="SaveFinancierOperation"
                                Title="" runat="server" />
                                
                                
                        </td>
                        <td>
                            Desconto:(%)&nbsp;<VFX:HelpTooltip ID="HelpTooltip6" runat="server">
                                <ItemTemplate>
                                    <h3> Ajuda:</h3>
                                 Desconto oferecido no valor da compra do cliente com determinada forma de pagamento.
                                </ItemTemplate>
                            </VFX:HelpTooltip> <br />
                            <uc2:CurrencyField ID="ucCurrFieldDiscount" MaxLength="6" Required="true" ValidationGroup="SaveFinancierOperation"
                                Title="" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Numero de Afiliação:&nbsp;<VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                                <ItemTemplate>
                                    <h3>
                                        Ajuda:</h3>
                                    O número de afiliação é o código fornecido pelo agente emissor do tipo de forma
                                    de pagamento para identificar sua empresa como o Cedente (Boleto), Afiliado (Cartões
                                    de Crédito).
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <asp:Textbox ID="txtMembershipNUmber" MaxLength="50" Required="false" ValidationGroup=""
                                Title="" runat="server" />
                        </td>
                        <td>
                            Código da Operação:&nbsp;<VFX:HelpTooltip ID="HelpTooltip3" runat="server">
                                <ItemTemplate>
                                    <h3>
                                        Ajuda:</h3>
                                    O Código de operação é fornecido pelo agente emissor para identificar qual forma
                                    de recebimento você está operando.
                                    <br />
                                    <br />
                                    No caso de <b>Boleto</b> indica qual carteira bancária utiliza, se é registrado
                                    ou não, se é emitido pelo banco ou emitido pelo InfoControl.
                                    <br />
                                    <br />
                                    No caso de <b>Cartão de Crédito</b> não é necessário.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <asp:Textbox ID="txtOperationNumber" MaxLength="50" Required="false" ValidationGroup=""
                                Title="" runat="server" />
                        </td>
                        <td>
                            Taxa de Adm por Operação: &nbsp;
                              <VFX:HelpTooltip ID="HelpTooltip5" runat="server">
                                <ItemTemplate>
                                    <h3>
                                        Ajuda:</h3>
                                        
                                    Esse valor representa a taxa que deverá ser paga à operadora cada vez que a referida
                                    forma de pagamento for utilizada. Esse valor é mais utilizado por formas de pagamento do tipo Boleto.
                                    
                                    <br />
                                    <br />
                                    Obs.: O valor dessa taxa é um valor inteiro; não utilize valores percentuais.
                                  
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <uc2:CurrencyField ID="ucCurrFieldAdminTaxUnit" MaxLength="6" Required="true" ValidationGroup="SaveFinancierOperation"
                                Title="" runat="server" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                &nbsp;<br />
                <fieldset runat="server" id="pnlFinancierCondition" visible="false">
                    <legend>Condições de operação </legend>
                    <table>
                        <tr>
                            <td>
                                Qtd de parcelas:<br />
                                <uc2:CurrencyField ID="ucCurrFieldParcelCount" Columns="3" Mask="999" Required="true"
                                    ValidationGroup="SaveFinancierCondition" Title="" runat="server" />
                            </td>
                            <td>
                                Fator de juros: (%)<br />
                                <uc2:CurrencyField ID="ucCurrFieldMonthlyTax" Mask="999,99" Required="true" ValidationGroup="SaveFinancierCondition"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnAdd" runat="server" OnClick="btnAdd_Click" ValidationGroup="SaveFinancierCondition"
                                    ImageUrl="../../App_Shared/themes/glasscyan/Controls/GridView/img/Add2.gif" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="grdFinancierCondition" Width="100%" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsFinancierCondition"
                        DataKeyNames="FinancierConditionId">
                        <Columns>
                            <asp:BoundField DataField="ParcelCount" HeaderText="Nº de parcelas" SortExpression="ParcelCount"
                                ItemStyle-Width="1%" />
                            <asp:BoundField DataField="MonthlyTax" HeaderText="Taxa mensal" SortExpression="MonthlyTax" />
                            <asp:TemplateField ItemStyle-Width="1px">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" companyid='<%# Eval("CompanyId") %>' financierconditionid='<%# Eval("FinancierConditionId") %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="right" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div align="center">
                                Não há dados a serem exibidos
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
                <div align="right">
                    <asp:Button ID="btnSave" ValidationGroup="SaveFinancierOperation" runat="server"
                        Text="Salvar" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='FinancierOperations.aspx'; return false;" />
                </div>
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
    <VFX:BusinessManagerDataSource ID="odsFinancierCondition" runat="server" ConflictDetection="CompareAllValues"
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" OnSelecting="odsFinancierCondition_Selecting"
        SelectCountMethod="GetFinancierConditionsCountByFinancierOperation" SelectMethod="GetFinancierConditionsByFinancierOperation"
        SortParameterName="sortExpression" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="financierOperationId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBureau" runat="server" OnSelecting="generic_Selecting"
        SelectMethod="GetBureausByCompany" TypeName="Vivina.Erp.BusinessRules.BureauManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsFinanciers" runat="server" SelectMethod="GetAllFinanciers"
        TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
