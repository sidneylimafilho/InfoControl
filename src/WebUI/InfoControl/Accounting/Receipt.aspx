<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Administration_Receipt" EnableEventValidation="false" Title="Nota Fiscal"
    CodeBehind="Receipt.aspx.cs" %>

<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Administration/SelectTransporter.ascx" TagName="SelectTransporter"
    TagPrefix="uc2" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct"
    TagPrefix="uc3" %>
<%@ Register Src="~/InfoControl/Administration/SelectSupplier.ascx" TagName="SelectSupplier"
    TagPrefix="uc4" %>
<%@ Register Src="~/App_Shared/Date.ascx" TagName="Date" TagPrefix="uc6" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc7" %>
<%@ Register Src="../../App_Shared/SelectProductAndService.ascx" TagName="SelectProductAndService"
    TagPrefix="uc8" %>
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
                <table width="100%">
                    <tr>
                        <td>
                            Nº da nota: &nbsp;<VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                    Número único e identificador desta nota fiscal.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <uc7:CurrencyField ID="ucCurrFieldReceiptNumber" ValidationGroup="saveReceipt" Mask="999999999"
                                MaxLength="9" runat="server" />
                        </td>
                        <td>
                            Natureza da operação:&nbsp;<VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                    Selecione aqui qual natureza da operação que está levando à emissão desta nota,
                                    em outras palavras, selecione o motivo pelo qual esta nota será emitida.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <asp:DropDownList ID="cboCFOP" runat="server" DataTextField="name" DataValueField="CfopId"
                                DataSourceID="odsAccount" AppendDataBoundItems="true" Width="350px">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="valcboCFOP" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="cboCFOP" ValidationGroup="saveReceipt"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RadioButton ID="rbtSelCustomer" AutoPostBack="true" GroupName="Choose" Text="Saida:"
                                runat="server" OnCheckedChanged="rbtSelCustomer_CheckedChanged" />
                            <asp:RadioButton ID="rbtSelSupplier" AutoPostBack="true" GroupName="Choose" Text="Entrada:"
                                runat="server" OnCheckedChanged="rbtSelSupplier_CheckedChanged" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlSupplier" runat="server" Width="100%">
                                <table>
                                    <tr>
                                        <td>
                                            <uc4:SelectSupplier ID="SelSupplier" runat="server" OnSelectedSupplier="SelSupplier_SelectedSupplier" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqSelSupplier" runat="server" ControlToValidate="SelSupplier"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="saveReceipt"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCustomer" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <uc1:SelectCustomer ID="SelCustomer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqSelCustomer" runat="server" ControlToValidate="SelCustomer"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="saveReceipt"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            Data de Emissão<br />
                            <asp:Label runat="server" ID="lblIssueDate" Visible="false"></asp:Label>
                            <uc6:Date ID="ucIssueDate" Title="" runat="server" />
                            <asp:Panel ID="pnlEntryDate" runat="server">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Data de Entrada<br />
                                <uc6:Date ID="ucEntrydate" Title="" Required="true" ValidationGroup="saveReceipt"
                                    runat="server" />
                            </asp:Panel>
                            <asp:Panel ID="pnlDeliveryDate" runat="server">
                                Data de Saída<br />
                                <asp:Label runat="server" ID="lblDeliveryDate" Visible="false"></asp:Label>
                                <uc6:Date ID="ucDeliveryDate" Required="true" ValidationGroup="saveReceipt" runat="server" />
                            </asp:Panel>
                            <br />
                        </td>
                    </tr>
                </table>
                <br />
                <fieldset>
                    <legend>Produtos/Serviços</legend>
                    <table width="100%">
                        <tr runat="server" id="rowReceiptItem">
                            <td>
                                <uc8:SelectProductAndService ID="SelProductAndService" ValidationGroup="AddItem"
                                    Required="true" runat="server" />
                            </td>
                            <td>
                                Quantidade:<br />
                                <uc7:CurrencyField ID="ucCurrFieldProductQuantity" ValidationGroup="AddItem" Required="true"
                                    Mask="999" Columns="3" runat="server" />
                                <br />
                                <br />
                            </td>
                            <td>
                                Preço unitário:<br />
                                <uc7:CurrencyField ID="ucCurrFieldUnitPrice" Title="" runat="server" />
                                <br />
                                <br />
                            </td>
                            <td>
                                IPI: &nbsp;<VFX:HelpTooltip runat="server">
                                    <ItemTemplate>
                                        <h2>
                                            Ajuda:</h2>
                                        Valor percentual do imposto sobre produtos industrializados a ser cobrado.
                                    </ItemTemplate>
                                </VFX:HelpTooltip>
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldIPI" Mask="99.99" runat="server" />
                                <br />
                                <br />
                            </td>
                            <td>
                                ICMS:&nbsp;<VFX:HelpTooltip runat="server">
                                    <ItemTemplate>
                                        <h2>
                                            Ajuda:</h2>
                                        Valor percentual do imposto sobre circulação de mercadorias e prestação de serviços
                                        a ser cobrado.
                                    </ItemTemplate>
                                </VFX:HelpTooltip>
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldICMS" Title="" Mask="99.99" runat="server" />
                                <br />
                                <br />
                            </td>
                            <td style="border-right: 1px solid #1A6E6A;" nowrap="nowrap" class="style1">
                                <asp:ImageButton ID="btnReceipItemProduct" ImageUrl="~/App_Shared/themes/glasscyan/Controls/GridView/img/Add2.gif"
                                    runat="server" AlternateText="Inserir produto" ValidationGroup="AddItem" OnClick="btnReceipItemProduct_Click" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            Importar Venda/O.S:&nbsp;<VFX:HelpTooltip runat="server">
                                                <ItemTemplate>
                                                    <h2>
                                                        Ajuda:</h2>
                                                    Faça a importação dos produtos ou serviços relacionados a uma venda ou ordem de
                                                    serviço diretamente para esta nota, já com os preços calculados automaticamente.
                                                    Primeiramente selecione um cliente para depois carregar dados de vendas ou OS's
                                                    geradas a partir deste.
                                                </ItemTemplate>
                                            </VFX:HelpTooltip>
                                            <br />
                                            <asp:DropDownList ID="cboSaleAndOs" runat="server" AutoPostBack="True" Width="100px"
                                                DataValueField="Id" AppendDataBoundItems="true" OnTextChanged="cboSaleAndOs_TextChanged"
                                                DataTextField="number">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView Width="100%" ID="grdReceiptItem" runat="server" AutoGenerateColumns="False"
                                    OnRowDeleting="grdReceiptItem_RowDeleting" DataKeyNames="UnitPrice" Rowselectable="false">
                                    <Columns>
                                        <asp:BoundField DataField="Description" HeaderText="Descrição" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantidade" />
                                        <%--  <asp:BoundField DataField="Icms" HeaderText="Icms" DataFormatString="{0:P}%" />--%>
                                        <asp:BoundField DataField="UnitPrice" HeaderText="Preço" />
                                        <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                            ShowDeleteButton="True">
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Cálculo do imposto</legend>
                    <table width="100%">
                        <tr>
                            <td>
                                Base ICMS:
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldICMSBase" Title="Base ICMS:" runat="server" />
                            </td>
                            <td>
                                Valor ICMS:
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldICMSValue" Title="Valor ICMS:" runat="server" />
                            </td>
                            <td>
                                Base ICMS Subst:
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldSubstituionICMSBase" Title="Base ICMS Subst:" runat="server" />
                            </td>
                            <td>
                                ICMS Subst:<br />
                                <uc7:CurrencyField ID="ucCurrFieldSubstituionICMSValue" Title="txtSubstituionICMSBase"
                                    runat="server" />
                            </td>
                            <td>
                                Valor total dos produtos:
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldTotalProductValue" Title="Valor total dos produtos:"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Valor do frete:&nbsp;<VFX:HelpTooltip runat="server">
                                    <ItemTemplate>
                                        <h2>
                                            Ajuda:</h2>
                                        Valor pago pelo cliente pelo transporte para entrega dos produtos adquiridos.
                                    </ItemTemplate>
                                </VFX:HelpTooltip>
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldFreightValue" Title="" runat="server" />
                            </td>
                            <td>
                                Valor do Seguro: &nbsp;<VFX:HelpTooltip runat="server">
                                    <ItemTemplate>
                                        <h2>
                                            Ajuda:</h2>
                                        Indica qual o valor cobrado pelo seguro do produto ou serviço, se os mesmos possuirem
                                        seguro, obviamente.
                                    </ItemTemplate>
                                </VFX:HelpTooltip>
                                <br />
                                <uc7:CurrencyField ID="ucCurrFieldInsuranceValue" Title="" runat="server" />
                            </td>
                            <td>
                                Outros Valores:<br />
                                <uc7:CurrencyField ID="ucCurrFieldOthersChargesValue" Title="" runat="server" />
                            </td>
                            <td>
                                Valor IPI:<br />
                                <uc7:CurrencyField ID="ucCurrFieldIPITotalValue" Title="" runat="server" />
                            </td>
                            <td>
                                <b>Valor Total da Nota:</b><br />
                                <asp:Label ID="lblReceiptValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <table>
                    <tr>
                        <td>
                            <uc2:SelectTransporter ID="SelTransporter" runat="server" OnSelectedTransporter="SelTransporter_SelectedTransporter" />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSave" runat="server" ValidationGroup="saveReceipt" Text="Salvar"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
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
    <VFX:BusinessManagerDataSource ID="odsAccount" runat="server" SelectMethod="GetCFOPFormatted"
        TypeName="Vivina.Erp.BusinessRules.AccountManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
