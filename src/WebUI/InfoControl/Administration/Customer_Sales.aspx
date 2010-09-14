<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Administration_WebUserControl"
    CodeBehind="Customer_Sales.aspx.cs" Title="" MasterPageFile="~/infocontrol/Default.master" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <asp:GridView ID="grdSaleByCustomer" runat="server" DataSourceID="odsCustomerSales"
                    Width="50%" AutoGenerateColumns="False" RowSelectable="false" DataKeyNames="SaleId,Value"
                    OnRowDataBound="grdSaleByCustomer_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="SaleDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data da Compra" />
                        <asp:BoundField DataField="Value" HeaderText="Total" />
                        <asp:BoundField DataField="Discount" HeaderText="Desconto" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="Vendedor" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem vendas registradas para este cliente.<br />
                            &nbsp;</div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <%--       <br />
                <h3>
                    Detalhes:</h3>
                <br />
                <asp:GridView ID="grdDetailedSales" runat="server" DataSourceID="odsDetailedSale"
                   Width="100%" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="ProductCode" HeaderText="Código"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Produto"></asp:BoundField>
                        <asp:BoundField DataField="UnitPrice" HeaderText="Valor Unitário"></asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantidade"></asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem detalhes sobre as vendas registradas para este cliente.<br />
                            &nbsp;</div>
                    </EmptyDataTemplate>
                </asp:GridView>--%>
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
    <%--    <VFX:BusinessManagerDataSource ID="odsSaleHistory" runat="server" OnSelecting="odsSaleHistory_Selecting"
        SelectMethod="GetSaleHistory" TypeName="Vivina.Erp.BusinessRules.SaleManager">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
        <asp:Parameter Name="customerId" Type="Int32" />
        <asp:Parameter Name="dateTimeInterval" Type="Object" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>--%>
    <VFX:BusinessManagerDataSource ID="odsCustomerSales" runat="server" OnSelecting="odsCustomerSales_Selecting"
        SelectMethod="GetSaleByCustomer" TypeName="Vivina.Erp.BusinessRules.SaleManager">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
