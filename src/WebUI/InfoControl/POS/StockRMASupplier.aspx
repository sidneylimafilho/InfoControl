<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
 Inherits="Company_POS_StockRMASupplier" Title="Produtos em trânsito" EnableEventValidation="false" Codebehind="StockRMASupplier.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <asp:GridView ID="grdRefused" runat="server" DataSourceID="odsRMA" AutoGenerateColumns="False"
                    DataKeyNames="InventoryRMAId" Width="100%" OnRowDeleting="grdRefused_RowDeleting"
                    AllowPaging="True" AllowSorting="True" PageSize="20" rowselectable="false">
                    <Columns>
                        <asp:BoundField DataField="SupplierName" SortExpression="SupplierName" HeaderText="Fornecedor">
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" SortExpression="ProductName" HeaderText="Produto">
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Quantidade">
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedDate" SortExpression="ModifiedDate" HeaderText="Envio">
                        </asp:BoundField>
                        <asp:CommandField HeaderText="Recebido ?" ShowDeleteButton="True" ShowHeader="True"
                            DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <center>
                            Não há nenhum produto em trânsito</center>
                    </EmptyDataTemplate>
                </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsRMA" runat="server" onselecting="odsRMA_Selecting"
        SelectMethod="RetrieveRMA" TypeName="Vivina.Erp.BusinessRules.InventoryManager"
        EnablePaging="True" SelectCountMethod="RetrieveRMACount" SortParameterName="sortExpression">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
            <asp:parameter Name="DepositId" Type="Int32"></asp:parameter>
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
