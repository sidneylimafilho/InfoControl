<%@ Control Language="C#" AutoEventWireup="True" Inherits="Company_POS_PurchaseOrder_Product"
    CodeBehind="PurchaseOrder_Product.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/InfoControl/Administration/SelectSupplier.ascx" TagName="SelectSupplier"
    TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc3" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/RH/SelectEmployee.ascx" TagName="SelectEmployee"
    TagPrefix="uc4" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc5" %>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td valign="top">
            Proposta Número:<br />
            <asp:TextBox ID="txtPurchaseOrderCode" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtPurchaseOrderCode" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                runat="server" ControlToValidate="txtPurchaseOrderCode">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
        </td>
        <%--<td>
            Status:<br />
            <asp:DropDownList ID="cboPurchaseOrderStatus" runat="server" DataSourceID="odsPurchaseOrderStatus"
                DataTextField="Name" DataValueField="PurchaseOrderStatusId">
            </asp:DropDownList>
        </td>--%>
        <td>
            &nbsp; &nbsp;
        </td>
    </tr>
</table>
<br />
<fieldset>
    <legend>Produtos:
        <asp:DropDownList ID="cboPurchaseOrderDecision" runat="server" AutoPostBack="True"
            OnTextChanged="cboPurchaseOrderDecision_TextChanged">
            <asp:ListItem Value="1" Text="Menor Preço Unitário"></asp:ListItem>
            <%--<asp:ListItem Value="2" Text="Menor Preço total"></asp:ListItem>--%>
            <asp:ListItem Value="3" Text="Melhor prazo"></asp:ListItem>
        </asp:DropDownList>
    </legend>
    <br />
    <table runat="server" visible="false">
        <tr>
            <td>
                Quant:<br />
                <uc1:CurrencyField ID="ucCurrFieldQuantityData" Mask="9999" ValidationGroup="Add"
                    runat="server" /><br />
                <br />
            </td>
            <td style="white-space: nowrap;">
                <uc3:SelectProduct ID="selProduct" Required="true" ValidationGroup="Add" runat="server" /></td>
            <td>
                <%--<asp:ListItem Value="2" Text="Menor Preço total"></asp:ListItem>--%>
                <asp:ImageButton ID="btnAddPurchaseOrderProduct" ImageUrl="~/App_Shared/themes/glasscyan/Controls/GridView/img/Add2.gif"
                    runat="server" AlternateText="Adicionar" OnClick="btnAdd_Click" ValidationGroup="Add" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
        </tr>
    </table>
    <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="Name,Quantity,SupplierName,Price"
        OnRowDeleting="grdProducts_RowDeleting" Width="100%" Rowselectable="false">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Produto"></asp:BoundField>
            <asp:BoundField DataField="SupplierName" HeaderText="Fornecedor"></asp:BoundField>
            <asp:BoundField DataField="DeliveryDate" HeaderText="Data Entrega"></asp:BoundField>
            <asp:BoundField DataField="Quantity" HeaderText="Qtd" ItemStyle-Width="1%"></asp:BoundField>
            <asp:TemplateField HeaderText="Qtd Receb." Visible="false">
                <ItemTemplate>
                    <uc1:CurrencyField ID="ucQtdReceived" Width="30px" Required="true" ValidationGroup="AddRequestItem"
                        Mask="999" runat="server" CurrencyValue='<%# Convert.ToDouble(Eval("QuantityReceived")) %>' />
                </ItemTemplate>
                <ItemStyle BackColor="DarkCyan" Width="1%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Valor">
                <ItemTemplate>
                    <%# Convert.ToDouble(Eval("Price")).ToString("C") %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Valor Total">
                <ItemTemplate>
                    <%# (Convert.ToDouble(Eval("Price")) * Convert.ToDouble(Eval("Quantity"))).ToString("C")  %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;">
                <ItemStyle Width="1px" />
            </asp:CommandField>
        </Columns>
        <EmptyDataTemplate>
            <center>
                Não há produtos adicionados na proposta</center>
        </EmptyDataTemplate>
    </asp:GridView>
    <VFX:BusinessManagerDataSource ID="odsPurchaseOrderStatus" runat="Server" SelectMethod="GetPurchaseOrderStatus"
        TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsSupplierContacts" runat="Server" OnSelecting="odsSupplierContacts_Selecting"
        SelectMethod="GetContactsBySupplier" TypeName="Vivina.Erp.BusinessRules.ContactManager">
        <SelectParameters>
            <asp:Parameter Name="companyID" Type="Int32" />
            <asp:Parameter Name="supplierID" Type="Int32" />
        </SelectParameters>
    </VFX:BusinessManagerDataSource>
</fieldset>
<br />
<fieldset runat="server" id="fdsSentToSupplier">
    <legend>Solicitação de Cotação / Autorização de Fornecimento</legend>
    <table>
        <tr>
            <td>
                <uc2:SelectSupplier ID="selSupplier" Required="false" runat="server" OnSelectedSupplier="SelSupplier_SelectedSupplier" />
            </td>
            <td valign="top">
                <asp:Panel runat="server" ID="pnlSupplierContacts">
                    Selecione o Contato:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                    <asp:DropDownList ID="cboSupplierContacts" DataTextField="Name" DataValueField="ContactId"
                        runat="server">
                        <asp:ListItem Text="            " Value=""></asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
            </td>
            <td align="center">
                <asp:Button ID="BtnSendQuotationRequest" runat="server" Text="Enviar por E-mail"
                    OnClick="BtnSendQuotationRequest_Click" />
                <br />
                <br />
                <asp:Button ID="btnDownloadRequestBudget" runat="server" Text="Download" OnClick="btnDownloadRequestBudget_Click" />&nbsp;
            </td>
        </tr>
    </table>
</fieldset>
<br />
<fieldset runat="server" id="fdsDecision">
    <legend>Decisão:</legend>
    <asp:MultiView ID="actionsViews" runat="server">
        <asp:View ID="View1" runat="server">
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:Button ID="btnApprove" runat="server" Text="Aprovar" OnClick="btnApprove_Click" />
            &nbsp; &nbsp;
            <asp:Button ID="btnReprove" runat="server" Text="Reprovar" OnClick="btnReprove_Click" />
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:UpdatePanel runat="server">
                <contenttemplate></contenttemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td>
                        Selecione o fornecedor vencedor:<br />
                        <asp:DropDownList ID="cboSupplierWinner" runat="server" DataSourceID="odsSuppliers"
                            DataTextField="Name" DataValueField="SupplierId" AutoPostBack="true" AppendDataBoundItems="true"
                            OnSelectedIndexChanged="cboSupplierWinner_SelectedIndexChanged">
                            <asp:ListItem Text="            " Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <VFX:BusinessManagerDataSource ID="odsSuppliers" runat="server" OldValuesParameterFormatString="original_{0}"
                            OnSelecting="odsSuppliers_Selecting" SelectMethod="GetSuppliersByPurchaseOrder"
                            TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager">
                            <SelectParameters>
                                <asp:Parameter Name="companyId" Type="Int32" />
                                <asp:Parameter Name="purchaseOrderId" Type="Int32" />
                            </SelectParameters>
                        </VFX:BusinessManagerDataSource>
                    </td>
                    <td>
                        Selecione o Contato:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                        <asp:DropDownList ID="cboContactWinner" DataTextField="Name" DataValueField="ContactId"
                            runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSendPurchaseOrderByMail" runat="server" Text="Enviar por E-mail"
                OnClick="btnSendPurchaseOrderByMail_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDownloadPurchaseOrder" runat="server" Text="Download da Ordem de Compra"
                OnClick="btnDownloadPurchaseOrder_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </asp:View>
        <asp:View runat="server">
            <table>
                <tr>
                    <td>
                        Numero da Nota:<br />
                        <asp:TextBox ID="txtReceiptNumber" runat="server" Width="242px"></asp:TextBox>
                    </td>
                    <td>
                        Valor da Nota:<br />
                        <uc1:CurrencyField ID="ucReceiptTotalValue" runat="server" />
                    </td>
                    <td>
                        Data &nbsp;Recebimento:<br />
                        <uc5:Date ID="datReceiptDate" runat="server" />
                    </td>
                    <td>
                        Funcionário Recebedor:
                        <uc4:SelectEmployee ID="selEmployee" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSentToDeposit" runat="server" Text="Enviar para o Estoque / Confirmar Recebimento"
                OnClick="btnSentToDeposit_Click" Visible='<%#Page.PurchaseOrder.PurchaseOrderStatusId == PurchaseOrderStatus.Bought%>' />
        </asp:View>
    </asp:MultiView></fieldset>
<br />
