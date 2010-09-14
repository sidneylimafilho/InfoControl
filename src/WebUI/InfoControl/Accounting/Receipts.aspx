<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Administration_Receipts" Title="Notas Fiscais"
    CodeBehind="Receipts.aspx.cs" %>

<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectSupplier.ascx" TagName="SelectSupplier"
    TagPrefix="uc3" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc4" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc5" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                    <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                        Escolha o filtro desejado: </legend><div class="body">
                            <table width="80%">
                                <tr>
                                    <td>
                                        Cliente:
                                        &nbsp;<VFX:HelpTooltip ID="HelpTooltip6" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                    Encontre notas fiscais que estão relacionadas a um determinado cliente escrevendo apenas alguns dígitos 
                                    iniciais de seu nome.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                                        <br />
                                        <asp:TextBox ID="txtSelectCustomer" runat="server" MaxLength="100"> </asp:TextBox>
                                    </td>
                                    
                                    <td>
                                        Notas fiscais:&nbsp;<VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                         Selecione qual o tipo de nota fiscal você deseja encontrar, de entrada, de saída ou todas.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                                        
                                        <br />
                                        <asp:DropDownList ID="cboReceiptType" runat="server">
                                            <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="De Entrada" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="De Saída" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                         <td>
                                        &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                        <uc5:DateTimeInterval ID="ucDateTimeinterval"  runat="server" Required="true" ValidationGroup="searchInvoice" />
                                          
                                          <div style="margin-top:-22px;margin-left:67%" >
                                          &nbsp;<VFX:HelpTooltip ID="HelpTooltip3" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                       Utilize "Início e Fim" para selecionar as notas fiscais que foram emitidas com data dentro desse intervalo
                                       de tempo escolhido.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                                        </div>
                                        <br/>
                                    </td>
                                </tr>
                          
                                <tr>
                                    <td>                                    
                                    
                                    <table cellpadding="0" cellspacing="5" nowrap="true">
                                    <tr> 
                                     <td> 
                                      Número Inicial:<br />
                                        <uc4:CurrencyField ID="ucCurrFieldInitialNumber" Mask="9999999" runat="server" />                                     
                                     </td>
                                     
                                      <td>                                     
                                      Número Final:<br />
                                        <uc4:CurrencyField ID="ucCurrFieldFinalNumber" Mask="9999999" runat="server" />
                                    &nbsp;<VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                                      <ItemTemplate>
                                           <h2> Ajuda:</h2>
                                       Em "Número Inicial" e "Número Final" encontre notais fiscais que tem seu número de identificação
                                       entre os valores estipulados nestes campos.
                                      </ItemTemplate>
                                    </VFX:HelpTooltip>
                                    </td>                                    
                                    
                                    </tr>
                                    
                                    </table>
                                    </td>
                                    <td>
                                        Exibir:<br />
                                        <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td valign="top" align="right">
                                        <asp:Button ID="btnSearchReceipt" ValidationGroup="" runat="server" Text="Pesquisar"
                                            OnClick="btnSearchReceipt_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                            &nbsp;</span>
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:GridView ID="grdReceipts" runat="server" Width="100%" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataKeyNames="ReceiptId,CompanyId,CustomerId,TransporterId,IssueDate,EntryDate,ModifiedDate,CfopId,ReceiptNumber,SubstitutionICMSBase,SubstitutionICMSValue,FreightValue,InsuranceValue,OtherschargesValue,ReceiptValue,SupplierId,DeliveryDate,isCanceled"
                    DataSourceID="odsReceipts" RowSelectable="false" OnRowDataBound="grdReceipts_RowDataBound"
                    OnSorting="grdReceipts_Sorting" PageSize="20">
                    <Columns>
                        <asp:BoundField DataField="ReceiptNumber" HeaderText="Nº da nota fiscal" SortExpression="ReceiptNumber" />
                        <asp:TemplateField HeaderText="Cliente/Fornecedor" SortExpression="CustomerName">
                            <ItemTemplate>
                                <%# Eval("CustomerName")%><%# Eval("supplierName")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IssueDate" HeaderText="Data de emissão" SortExpression="IssueDate"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="EntryDate" HeaderText="Data de entrada" SortExpression="EntryDate"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Receipt.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" id='<%# Eval("ReceiptId") %>' companyid='<%# Eval("CompanyId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar uma nota fiscal.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <br />
                <br />
                <br />
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
    <VFX:BusinessManagerDataSource ID="odsReceipts" runat="server" ConflictDetection="CompareAllValues"
        onselecting="odsReceipts_Selecting" OldValuesParameterFormatString="original_{0}"
        EnablePaging="True" SortParameterName="sortExpression" SelectMethod="GetReceipts"
        SelectCountMethod="GetReceiptsCount" TypeName="Vivina.Erp.BusinessRules.ReceiptManager"
        DataObjectTypeName="Vivina.Erp.DataClasses.Receipt" DeleteMethod="DeleteReceipt">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="customerId" Type="Int32" />
            <asp:Parameter Name="supplierId" Type="Int32" />
            
            <asp:Parameter Name="selectCustomer" Type="String" />
                        
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            <asp:Parameter Name="receiptType" Type="Int32" />
            
            <asp:Parameter Name="initialNumber" Type="Decimal" />
            <asp:Parameter Name="finalNumber" Type="Decimal" />
            
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
