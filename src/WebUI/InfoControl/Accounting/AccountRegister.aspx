<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Accounting_AccountRegister" Title="Conciliação Bancária"
    EnableEventValidation="false" CodeBehind="AccountRegister.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<%@ Register Src="~/App_Shared/Date.ascx" TagName="Date" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
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
                <td class="left" style="height: 97px">
                    &nbsp;
                </td>
                <td class="center" style="height: 97px">
                    <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend><div class="body">
                                <table>
                                    <tr>
                                        <td>
                                            Conta Bancária&nbsp;<VFX:HelpTooltip ID="HelpTooltip4" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                      Selecione aqui a conta bancária em que as contas que você quer encontrar estão associadas.
                                </ItemTemplate>
                            </VFX:HelpTooltip> <br />
                                            <asp:DropDownList ID="cboAccount" runat="server" DataSourceID="odsAccount" DataTextField="ShortName"
                                                DataValueField="AccountId" CausesValidation="True" ValidationGroup="AccountRegister">
                                            </asp:DropDownList>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        
                                            <uc1:DateTimeInterval ID="ucDateTimeInterval" ValidationGroup="AccountRegister" runat="server" />
                                        <div style="margin-left:90.7%; margin-top:-22px">   
                                            &nbsp;<VFX:HelpTooltip ID="HelpTooltip3" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                       Utilize "Início e Fim" para selecionar as contas que tem parcelas com data de pagamento dentro desse intervalo de 
                                       data escolhido.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">
                                            <asp:Button ID="btnSearch" runat="server" Text="Filtrar" ValidationGroup="AccountRegister"
                                                OnClick="btnSearch_Click" />
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                              <fieldset> 
                               <legend> Acerto de Saldo &nbsp;<VFX:HelpTooltip ID="HelpTooltip5" runat="server">
                                    <ItemTemplate>
                                        <h2> Ajuda: </h2>
                                          Acerto de saldo serve para você sincronizar o saldo bancário que o InfoControl
                                          está marcando de acordo com os valores de extratos bancários da sua empresa. 
                                          Isso é feito através de adição e abatimento de saldo abaixo. Quando o botão
                                          "Adicionar" é clicado, é gerada uma conta a receber com o valor em questão; 
                                          quando o "Remover" é clicado, é gerada uma conta a pagar. 
                                    </ItemTemplate>
                                </VFX:HelpTooltip>
                              </legend>                              
                                 <uc2:CurrencyField runat="server" Required="true" ValidationGroup="balanceValue" ID="ucCurrFieldBalance" /> 
                                 &nbsp&nbsp&nbsp
                                 <asp:Button Text="Adicionar" runat="server" ValidationGroup="balanceValue" ID="btnAdd" onclick="btnAdd_Click" />
                                 &nbsp;&nbsp
                                 <asp:Button Text="Remover" runat="server" ValidationGroup="balanceValue" ID="btnRemove" 
                                      onclick="btnRemove_Click" />&nbsp;&nbsp
                                      <asp:Literal runat="server" Visible="false" ID="litErrorMessage"></asp:Literal>
                              
                              </fieldset>
                    <br>
                    <br>
                    
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdAccountRegister" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="odsAccountRegister" Width="100%" OnRowDataBound="grdAccountRegister_RowDataBound"
                                    RowSelectable="false" DataKeyNames="ParcelId,EffectedDate,DueDate,Amount,EffectedAmount,Description,InvoiceId,BillId,AccountId,IsRecurrent,RecurrentPeriod,CompanyId,PaymentMethodId,IdentificationNumber,IsRegistered, OperationDate"
                                    ShowFooter="False">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkconciliate" runat="server" Visible='<%# !Convert.ToBoolean(Eval("IsRegistered")) %>'
                                                    onclick='<%# Eval("EffectedDate") == null?"alert(\"Parcela ainda não foi quitada!\"); this.checked=false;":"" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descrição" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lblParcelDescription" Font-Bold="true" runat="server" Text='<%# Eval("BillId")!= null ? Eval("Bill.Description") : Eval("Invoice.Description") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IdentificationNumber" DataFormatString="{0:c}" HeaderText="Nº Doc."
                                            SortExpression="IdentificationNumber" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="EffectedDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Pagamento"
                                            SortExpression="EffectedDate" ItemStyle-Width="13%" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Data Mov." SortExpression="OperationDate" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateMov" runat="server" Visible='<%# Convert.ToBoolean(Eval("IsRegistered"))%>'
                                                    Text='<%# Eval("OperationDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                                <uc3:Date ID="ucDtDateMov" Text='<%# Eval("EffectedDate")!=null ? Eval("EffectedDate", "{0:dd/MM/yyyy}") : Eval("DueDate", "{0:dd/MM/yyyy}")%>'
                                                    Visible='<%# !Convert.ToBoolean(Eval("IsRegistered")) && Eval("EffectedAmount")!=null %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Eval("BillId")!= null?"-":"" %><%#Eval("Amount","{0:c}")%>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="Saldo" HeaderStyle-HorizontalAlign="Right">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="1%" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate> 
                                        <div style="text-align: center">
                                            Não existem dados a serem exibidos.<br />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                 <table width="100%">
                                    <tr>
                                        <td style="text-align: right">
                                            Realizado:
                                        </td>
                                        <td style="text-align: right; wwidth: 1%; white-space:nowrap;">
                                            <B><asp:Literal runat="server" ID="txtDone"></asp:Literal></B>
                                            &nbsp;<VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2> Indica o resultado das contas que já foram concilidas. Esse valor pode somente chegar ao valor apresentado
                                        abaixo em "Possível".
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            Possivel:
                                        </td>
                                        <td style="text-align: right; width: 1%;  white-space:nowrap;">
                                            <B><asp:Literal runat="server" ID="txtPossible"></asp:Literal></B>
                                            &nbsp;<VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                                <ItemTemplate>
                                    <h2>
                                       Ajuda:</h2>  Esse valor representa a diferença entre as contas a receber e a pagar. 
                                       "Contas a receber - Contas a pagar = Possível".
                                  
                                   
                                  
                                </ItemTemplate> 
                            </VFX:HelpTooltip>
                                        </td>
                                    </tr>
                                </table>
                             
                                
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnRegister" runat="server" ValidationGroup="AccountRegister" Text="Conciliar os selecionados"
                                    OnClick="btnRegister_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />                  
                    <VFX:BusinessManagerDataSource ID="odsAccountRegister" runat="server" onselecting="odsAccountRegister_Selecting"
                        TypeName="Vivina.Erp.BusinessRules.ParcelsManager" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetParcelsByAccountInPeriod" SelectCountMethod="GetParcelsByAccountInPeriodCount">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
                            <asp:Parameter Name="accountId" Type="Int32" />
                            <asp:Parameter Name="dateTimeInterval" Type="Object" />
                            <asp:Parameter Name="sortExpression" Type="String" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsAccount" runat="server" onselecting="odsAccount_Selecting"
                        SelectMethod="GetAccountsWithShortName" TypeName="Vivina.Erp.BusinessRules.AccountManager">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
                </td>
                <td class="right" style="height: 97px">
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
    </div>
</asp:Content>
