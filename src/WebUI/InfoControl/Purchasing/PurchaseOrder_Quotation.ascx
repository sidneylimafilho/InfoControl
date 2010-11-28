<%@ Control Language="C#" AutoEventWireup="true" Inherits="Vivina.Erp.WebUI.Purchasing.PurchaseOrder_Quotation"
    CodeBehind="PurchaseOrder_Quotation.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectSupplier.ascx" TagName="SelectSupplier"
    TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc3" %>
<table>
    <tr>
        <td>
            <uc1:SelectSupplier ID="selSuppllier" OnSelectedSupplier="selSupplier_OnSelectedSupplier"
                runat="server" />
        </td>
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ou&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
        <td>
            Fornecedores que já apresentaram orçamento para os produtos deste processo:<br />
            <asp:DropDownList ID="cboCandidateSupplier" AppendDataBoundItems="true" DataTextField="Name"
                DataValueField="SupplierId" DataSourceID="odsSearchSuppliers" runat="server"
                AutoPostBack="True" OnSelectedIndexChanged="cboCandidateSupplier_SelectedIndexChanged">
                <asp:ListItem Selected="True" Text="" Value="0"> </asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<h2>
    Produtos</h2>
<br />
<asp:GridView ID="grdProductsQuotation" DataKeyNames="Name,Quantity,PurchaseOrderItemId"
    RowSelectable="False" ShowFooter="true" DataSourceID="odsPurchaseOrderItems"
    runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:TemplateField HeaderText="Nome">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>' />
            </ItemTemplate>
            <FooterTemplate>
                Valor Total:
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Preço">
            <ItemTemplate>
                <uc2:CurrencyField ID="ucCurrFieldPrice" Text='<%# Eval("Price") %>' runat="server" />
            </ItemTemplate>
            <FooterTemplate>
                <uc2:CurrencyField ID="ucTotalAmount" Text='' runat="server" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<table>
    <tr>
        <td>
            Data de entrega:
        </td>
        <td>
            <uc3:Date ID="datDeliveryDate" ValidationGroup="Save" Required="true" runat="server" />
        </td>
        <td>
           <%-- <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator"
                ControlToValidate="datDeliveryDate"></asp:RequiredFieldValidator>--%>
        </td>
    </tr>
</table>
<table width="50%">
    <tr>
        <td align="right">
            <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="Salvar" OnClick="btnSave_Click" />
        </td>
    </tr>
</table>
<VFX:BusinessManagerDataSource ID="odsPurchaseOrder" runat="server" TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager"
    SelectMethod="GetPurchaseOrdersByMatrix" OnSelecting="odsPurchaseOrder_Selecting">
    <SelectParameters>
        <asp:Parameter Name="matrixId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsSearchSuppliers" runat="server" TypeName="Vivina.Erp.BusinessRules.Accounting.PurchaseManager"
    SelectMethod="SearchSupplierCanditate" OnSelecting="odsSearchSuppliers_Selecting">
    <SelectParameters>
        <asp:Parameter Name="purchaseOrderId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsPurchaseOrderItems" runat="server" TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager"
    SelectMethod="GetPurchaseOrdersItemsByPurchaseOrder" OnSelecting="odsPurchaseOrderItems_Selecting"
    OldValuesParameterFormatString="original_{0}">
    <SelectParameters>
        <asp:Parameter Name="matrixId" Type="Int32" />
        <asp:Parameter Name="purchaseOrderId" Type="Int32" />
        <asp:Parameter Name="supplierId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
