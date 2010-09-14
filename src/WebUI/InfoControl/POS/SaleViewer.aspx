<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_POS_SaleViewer" Title="Visualizador de Vendas"
    CodeBehind="SaleViewer.aspx.cs" %>

<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <table width="100%">
                    <tr>
                        <td>
                            <fieldset style="height: 150px;">
                                <legend>Informações da venda</legend>
                                <br />
                                Nº da Venda:
                                <asp:Label ID="lblSaleNumber" runat="server"></asp:Label>
                                <br />
                                Data da Venda:
                                <asp:Label ID="lblSaleDate" runat="server"></asp:Label>
                                <br />
                                Nota Fiscal:&nbsp;
                                <asp:HyperLink ID="lnkReceipt" runat="server"></asp:HyperLink>
                                <br />
                                Data da Emissão:
                                <asp:Label ID="lblReceiptDate" runat="server"></asp:Label>
                                <br />
                                Status:
                                <asp:Label ID="lblSaleStatus" runat="server"></asp:Label>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset style="height: 150px;">
                                <legend>Cliente</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <uc2:SelectCustomer ID="sel_customer" OnSelectedCustomer="SelCustomer_SelectedCustomer"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <br />
                <fieldset>
                    <legend>Produtos</legend>
                    <asp:GridView ID="grdSaleItems" runat="server" DataSourceID="odsSaleItems" Width="100%"
                        AutoGenerateColumns="False" RowSelectable="false" OnRowDataBound="grdSaleItems_RowDataBound"
                        AllowPaging="True" AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="Código">
                            </asp:BoundField>
                            <asp:BoundField DataField="productName" SortExpression="productName" HeaderText="Produto" />
                            <asp:BoundField DataField="quantity" SortExpression="quantity" HeaderText="Quantidade">
                            </asp:BoundField>
                            <asp:BoundField DataField="unitPrice" SortExpression="unitPrice" DataFormatString="{0:c}"
                                HeaderText="Unitário"></asp:BoundField>
                            <asp:BoundField DataField="total" SortExpression="total" DataFormatString="{0:c}"
                                HeaderText="Total" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Formas de Pagamento</legend>
                    <asp:GridView ID="grdPaymentMethod" runat="server" Width="100%" DataSourceID="odsPaymentMethod"
                        AllowSorting="True" AutoGenerateColumns="False" RowSelectable="false" AllowPaging="True" PageSize="20"> 
                        <Columns>
                            <asp:BoundField DataField="DueDate" HeaderText="Vence em" SortExpression="DueDate"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="EffectedDate" HeaderText="Pago em" SortExpression="EffectedDate"
                                DataFormatString="{0:dd/MM/yyyy}" NullDisplayText="Em Aberto"></asp:BoundField>
                            <asp:BoundField DataField="Amount" DataFormatString="{0:C}" HeaderText="Valor" SortExpression="Amount"
                                ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
                <div style="text-align: right">
                    <br />
                    <br />
                    <asp:Button ID="btnGenerateFiscalSale" runat="server" Text="Gerar nota fiscal" />
                    <asp:Button ID="btnCancelSale" runat="server" Text="Cancelar Venda" OnClick="btnCancelSale_Click" />
                </div>
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
    <VFX:BusinessManagerDataSource ID="odsSaleItems" runat="server" onselecting="odsSaleItems_Selecting"
        SelectMethod="GetSaleProductsAsDataTable" TypeName="Vivina.Erp.BusinessRules.SaleManager"
        ConflictDetection="CompareAllValues" SelectCountMethod="GetSaleProductsAsDataTableCount"
        EnablePaging="True" SortParameterName="sortExpression">
        <selectparameters>
            <asp:parameter Name="CompanyId" Type="Int32"></asp:parameter>
            <asp:parameter Name="SaleId" Type="Int32"></asp:parameter>
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsPaymentMethod" runat="server" onselecting="odsPaymentMethod_Selecting"
        SelectMethod="GetParcelsBySale" TypeName="Vivina.Erp.BusinessRules.ParcelsManager"
        ConflictDetection="CompareAllValues" EnablePaging="True" SelectCountMethod="GetParcelsBySaleCount"
        SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:parameter Name="SaleId" Type="Int32"></asp:parameter>
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCustomer" runat="server" onselecting="odsSaleData_Selecting"
        SelectMethod="GetCustomerBySale" TypeName="Vivina.Erp.BusinessRules.CustomerManager">
        <selectparameters>
            <asp:Parameter Name="saleId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
