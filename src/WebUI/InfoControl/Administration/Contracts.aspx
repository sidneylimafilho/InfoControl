<%@ Page Language="C#" AutoEventWireup="true" Inherits="InfoControl_Judicial_Contracts"
    CodeBehind="Contracts.aspx.cs" Title="" MasterPageFile="~/infocontrol/Default.master" %>

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
                <!-- conteudo -->
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdContracts" runat="server" AutoGenerateColumns="False" DataSourceID="odsContracts"
                                Width="100%" AllowSorting="True" OnRowDataBound="grdContracts_RowDataBound" AllowPaging="True"
                                RowSelectable="false" PageSize="20" DataKeyNames="ContractId,CompanyId,CustomerId,BeginDate,HH,ExpiresDate,InterestDeferredPayment,Penalty,ContractValue,AdditionalValue1,AdditionalValue2,AdditionalValue3,AdditionalValue4,AdditionalValue5,InvoiceId,Observation,ContractTypeId,ContractStatusId,Parcels,FirstParcelDueDate,Periodicity,ContractNumber,FinancierConditionId,FinancierOperationId,RepresentantId,EmployeeId">
                                <Columns>
                                    <asp:BoundField DataField="ContractNumber" HeaderText="Numero do Contrato" SortExpression="ContractNumber" />
                                    <asp:BoundField DataField="BeginDate" HeaderText="Data de Início" SortExpression="BeginDate"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ExpiresDate" HeaderText="Data de Vencimento" SortExpression="ExpiresDate"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <center>
                                        Não existem contratos cadastrados...
                                        <br />
                                        <br />
                                    </center>
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
    <VFX:BusinessManagerDataSource ID="odsContracts" runat="server" EnablePaging="True"
        OnSelecting="odsContracts_Selecting1" SelectCountMethod="RetrieveContractsByCustomerCount"
        SelectMethod="RetrieveContractsByCustomer" SortParameterName="sortExpression"
        TypeName="Vivina.Erp.BusinessRules.ContractManager">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
        <asp:Parameter Name="customerId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
