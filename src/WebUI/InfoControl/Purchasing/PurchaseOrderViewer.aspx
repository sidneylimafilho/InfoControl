<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_POS_PurchaseOrderViewer" Title=" Visualizador de Ordem de compra"
    CodeBehind="PurchaseOrderViewer.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectSupplier.ascx" TagName="SelectSupplier"
    TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
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
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblPurchaseOrderCode" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <uc2:SelectSupplier ID="selSupplier" OnSelectedSupplier="selSupplier_SelectedSupplier"
                                runat="server" />
                            <asp:RequiredFieldValidator ID="reqSelSupplier" runat="server" ControlToValidate="selSupplier"
                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="save">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdPurchaseOrderItems" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                DataKeyNames="PurchaseOrderItemId,ProductName,Description,QuantityOrdered,QuantityReceived,ModifiedDate,PricePaid,PurchaseOrderId,CompanyId"
                                DataSourceID="odsPurchaseOrderItems" Width="100%" OnSelectedIndexChanged="grdPurchaseOrderItems_SelectedIndexChanged"
                                OnRowUpdating="grdPurchaseOrderItems_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nome do Produto">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" Text='<%# Bind("ProductName") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantitdade Pedida">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantityOrdered" Text='<%# Bind("QuantityOrdered") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantidade Recebida">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantityReceived" Enabled="false" Columns="6" runat="server"
                                                Text='<%# Bind("QuantityReceived") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQuantityReceived" Columns="6" Enabled="true" runat="server" Text='<%# Bind("QuantityReceived") %>'></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="msktxtQuantityReceived" runat="server" InputDirection="RightToLeft"
                                                Mask="999999" MaskType="Number" TargetControlID="txtQuantityReceived">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPricePaid" Enabled="false" Columns="10" MaxLength="10" runat="server"
                                                Text='<%# Bind("PricePaid", "{0:c}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPricePaid" Columns="10" MaxLength="10" Enabled="true" runat="server"
                                                Text='<%# Bind("PricePaid") %>'></asp:TextBox>
                                            &nbsp;&nbsp;&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" InputDirection="RightToLeft"
                                                Mask="9,999,999.99" MaskType="Number" TargetControlID="txtPricePaid">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField UpdateText="&lt;div class=&quot;save&quot;   title=&quot;salvar&quot;&gt;  &lt;/div&gt;"
                                        CancelText="&lt;div class=&quot;cancel&quot; title=&quot;Cancelar&quot;&gt;&lt;/div&gt;"
                                        ShowEditButton="True" ShowCancelButton="true" EditText="" ValidationGroup="save"
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemStyle CssClass="actions" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="right">
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
    <VFX:BusinessManagerDataSource ID="odsPurchaseOrderItems" runat="server" ConflictDetection="CompareAllValues"
        EnablePaging="True" onselecting="odsPurchaseOrderItems_Selecting" SelectCountMethod="GetPurchaseOrderItemsCount"
        SelectMethod="GetPurchaseOrderItems" SortParameterName="sortExpression" TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager"
        UpdateMethod="UpdatePurchaseOrderItem" DataObjectTypeName="Vivina.Erp.DataClasses.PurchaseOrderItem"
        OldValuesParameterFormatString="original_{0}">
        <updateparameters>
            <asp:Parameter Name="original_entity" Type="Object" />
            <asp:Parameter Name="entity" Type="Object" />
        </updateparameters>
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="purchaseOrderId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsSupplier" runat="server">
    </VFX:BusinessManagerDataSource>
</asp:Content>
