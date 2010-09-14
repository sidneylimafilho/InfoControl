<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_POS_StockMovement_IN"
    Title="Recebimento" Codebehind="StockTransferRECEIVE.aspx.cs" %>

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
                Escolha de qual empresa vêm o recebimento:<br />
                <asp:DropDownList ID="cboCompany" runat="server" DataSourceID="odsStockCompany" DataTextField="CompanyName"
                    DataValueField="CompanyId" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <br />
                <asp:GridView ID="grdReceive" runat="server" AutoGenerateColumns="False" DataSourceID="odsStockTransfer"
                    DataKeyNames="InventoryMovementId,ProductId,CompanyDestinationId" OnRowDeleting="grdReceive_RowDeleting"
                    Width="100%" AllowPaging="True" AllowSorting="True" PageSize="20">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Produto" SortExpression="Name"></asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantidade Enviada" SortExpression="Quantity">
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Data do Envio" SortExpression="ModifiedDate">
                        </asp:BoundField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <center>
                            Não há nehum produto à ser recebido da empresa selecionada.</center>
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnReceive" runat="server" Text="Receber" OnClick="btnReceive_Click" />
                </div>
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
    <VFX:BusinessManagerDataSource ID="odsStockCompany" runat="server" onselecting="odsStockCompany_Selecting"
        SelectMethod="GetCompaniesNames" TypeName="Vivina.Erp.BusinessRules.CompanyManager">
        <selectparameters>
            <asp:parameter Name="UserId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsStockTransfer" runat="server" onselecting="odsStockTransfer_Selecting"
        SelectMethod="RetrievePendingTransfers" TypeName="Vivina.Erp.BusinessRules.InventoryManager"
        EnablePaging="True" SelectCountMethod="RetrievePendingTransfersCount" SortParameterName="sortExpression"
        StartRowIndexParameterName="startRowIndex" MaximumRowsParameterName="maximumRows">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
            <asp:Parameter Name="destinationCompanyId" Type="Int32" />
            <asp:Parameter Name="destinationDepositId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
