<%@ Page EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    Language="C#" AutoEventWireup="true" Inherits="InfoControl_POS_LightningX" Title="Raio-X" CodeBehind="XRay.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="DundasWebGauge" Namespace="Dundas.Gauges.WebControl" TagPrefix="DGWC" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
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
                            <h3>
                                Valor Médio por venda:
                            </h3>
                        </td>
                        <td>
                            <h3>
                                Produtos Vendidos:
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:GridView ID="grdProductsSale" runat="server" Width="100%" DataSourceID="odsProductSale"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="media" HeaderText="Média" SortExpression="media" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Tipo de Cliente:</h3>
                        </td>
                        <td>
                            <h3>
                                Por Vendedor:</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="CustomerType" runat="server" Width="100%" DataSourceID="odsCustomerType"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="tipo" HeaderText="Tipo" SortExpression="tipo" />
                                    <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:GridView ID="grdVendor" runat="server" Width="100%" DataSourceID="odsEmployeeType"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="Vendedor" HeaderText="Vendedor" SortExpression="Vendedor" />
                                    <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                            &nbsp;&nbsp;
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
    <VFX:businessmanagerdatasource ID="odsConsumptionBySale" runat="server" onselecting="odsLightningx_Selecting"
        SelectMethod="GetConsumptionBySale" TypeName="Vivina.Erp.BusinessRules.SaleManager"
        OldValuesParameterFormatString="original_{0}">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource ID="odsProductSale" runat="server" onselecting="odsLightningx_Selecting"
        SelectMethod="GetProductBySale" TypeName="Vivina.Erp.BusinessRules.ProductManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource ID="odsCustomerType" runat="server" onselecting="odsLightningx_Selecting"
        SelectMethod="summarizeSaleValueByCustomerType" TypeName="Vivina.Erp.BusinessRules.CustomerManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource ID="odsEmployeeType" runat="server" onselecting="odsLightningx_Selecting"
        SelectMethod="GetConsumptionByEmployee" TypeName="Vivina.Erp.BusinessRules.SaleManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:businessmanagerdatasource>
</asp:Content>
