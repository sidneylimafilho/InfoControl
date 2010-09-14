<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="XRay.aspx.cs" Inherits="Vivina.Erp.WebUI.Accounting.XRay" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="DundasWebGauge" Namespace="Dundas.Gauges.WebControl" TagPrefix="DGWC" %>
<%@ Register assembly="DundasWebChart" namespace="Dundas.Charting.WebControl" tagprefix="DCWC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Raio-X</h1>
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
                <!-- content -->
                Contratos por Averbador:<br />
                <asp:GridView ID="grdTeste" runat="server" DataSourceID="odsTeste" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="FinancierName" HeaderText="Financeira" />
                        <asp:BoundField DataField="ContractQuantity" HeaderText="Qtd de contratos" />
                        <asp:BoundField DataField="ContractAmount" HeaderText="Total" />
                    </Columns>
                </asp:GridView>
                <br />
                <br />
                Contratos por Representante:<br />
                <asp:GridView ID="grdTeste2" runat="server" DataSourceID="odsTeste2" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="RepresentantName" HeaderText="Representante" />
                        <asp:BoundField DataField="ContractQuantity" HeaderText="Qtd de contratos" />
                        <asp:BoundField DataField="ContractAmount" HeaderText="Total" />
                    </Columns>
                </asp:GridView>
                <br />
                <br />
                Contratos por Tipo de cliente:<br />
                <asp:GridView ID="grdTeste3" runat="server" DataSourceID="odsTeste3" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="CustomerTypeName" HeaderText="Tipo de cliente" />
                        <asp:BoundField DataField="ContractQuantity" HeaderText="Qtd de contratos" />
                        <asp:BoundField DataField="ContractAmount" HeaderText="Total" />
                    </Columns>
                </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsTeste2" runat="server" onselecting="odsTeste_Selecting"
        SelectMethod="GetContractsByRepresentant" TypeName="Vivina.Erp.BusinessRules.ContractManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsTeste3" runat="server" onselecting="odsTeste_Selecting"
        SelectMethod="GetContractsByTypeCustomer" TypeName="Vivina.Erp.BusinessRules.ContractManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
