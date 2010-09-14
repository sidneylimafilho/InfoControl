<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
 Inherits="Company_POS_StockTransfer" Title="Produtos em trânsito" Codebehind="StockTransfer.aspx.cs" %>

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
                <asp:GridView ID="grdRefused" runat="server" DataSourceID="odsStockTransfer" AutoGenerateColumns="False"
                    OnRowDataBound="grdRefused_RowDataBound" DataKeyNames="InventoryMovementId,CompanyDestinationId,Quantity,MOdifiedDate,Refused,ProductName,ProductId,CompanyName"
                    Width="100%" RowSelectable="false" PageSize="20" AllowSorting="True" 
                    AllowPaging="True">
                    <Columns>
                        <asp:BoundField DataField="CompanyName" SortExpression="CompanyName" HeaderText="Empresa Destino">
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" SortExpression="ProductName" HeaderText="Produto">
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Quantidade">
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedDate" SortExpression="ModifiedDate" HeaderText="Data de Envio">
                        </asp:BoundField>
                        <asp:BoundField DataField="Refused" SortExpression="Refused" HeaderText="Devolvido ?">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboDropType" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="1">Defeito</asp:ListItem>
                                    <asp:ListItem Value="2">Extravio</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <center>
                            Não há nenhum produto em trânsito
                        </center>
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnConfirm" runat="server" Text="Confirma" OnClick="btnConfirm_Click" /></div>
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
    <VFX:BusinessManagerDataSource ID="odsStockTransfer" runat="server" onselecting="odsStockTransfer_Selecting"
        SelectMethod="RetrieveTransfers" TypeName="Vivina.Erp.BusinessRules.InventoryManager"
        SortParameterName="sortExpression" EnablePaging="True" 
        SelectCountMethod="RetrieveTransfersCount">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
            <asp:Parameter Name="DepositId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
