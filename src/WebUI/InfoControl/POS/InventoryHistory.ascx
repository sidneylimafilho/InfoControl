<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="InfoControl_POS_InventoryHistory" Codebehind="InventoryHistory.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                        <asp:GridView ID="grdInventotyHistory"  runat="server" Width="100%"  AutoGenerateColumns="False"
                            DataSourceID="odsInventoryHistory" DataKeyNames="DummyId,ProductId,CurrencyRateId,UnitPrice,Localization,Quantity,LogDate,MinimumRequired,RealCost,Profit,SupplierId,DestinationDepositId,DepositId,FiscalNumber,InventoryDropTypeId,InventoryEntryTypeId,CompanyId,QuantityInReserve,AverageCosts,SaleId"
                            OnRowCreated="grdInventotyHistory_RowCreated" RowSelectable="false" OnRowDataBound="grdInventotyHistory_RowDataBound"
                            OnSelectedIndexChanging="grdInventotyHistory_SelectedIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="UnitPrice" HeaderText="Preço" SortExpression="UnitPrice" />
                                <asp:BoundField DataField="Localization" HeaderText="Local" SortExpression="Localization" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantidade" SortExpression="Quantity" />
                                <asp:BoundField DataField="LogDate" HeaderText="Data de Modificação" SortExpression="LogDate" />
                                <asp:BoundField DataField="MinimumRequired" HeaderText="Quantidade Minima" SortExpression="MinimumRequired" />
                                <asp:BoundField DataField="RealCost" HeaderText="Custo Real" SortExpression="RealCost" />
                                <asp:BoundField DataField="Profit" HeaderText="Margem de Lucro" SortExpression="Profit" />
                                <asp:BoundField DataField="FiscalNumber" HeaderText="Nº da nota fiscal" SortExpression="FiscalNumber" />
                                <asp:BoundField DataField="userName" HeaderText="Usuário" SortExpression="userName" />                              
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="text-align: center">
                                    Não existem dados a serem exibidos ...
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
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
<VFX:BusinessManagerDataSource ID="odsInventoryHistory" runat="server" SelectMethod="GetInventoryHistoriesByProduct"
    TypeName="Vivina.Erp.BusinessRules.InventoryManager" OnSelecting="odsInventoryHistory_Selecting"
    OnUpdated="odsInventoryHistory_Updated" OnDeleted="odsInventoryHistory_Deleted"
    OnDeleting="odsInventoryHistory_Deleting">
    <SelectParameters>
        <asp:Parameter Name="productId" Type="Int32" />
        <asp:Parameter Name="depositId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
