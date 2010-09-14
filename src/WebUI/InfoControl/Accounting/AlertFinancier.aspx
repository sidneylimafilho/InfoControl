<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" Inherits="InfoControl_Accounting_AlertFinancier"
    MasterPageFile="~/InfoControl/Default.master" CodeBehind="AlertFinancier.aspx.cs" Title="Alerta de contas" %>

<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="~/App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
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
                <asp:UpdatePanel ID="upBill" runat="server">
                    <contenttemplate>
                        <h3>
                            Contas à Pagar</h3>
                        <asp:GridView ID="grdBill" runat="server" DataKeyNames="BillId" DataSourceID="odsBills"
                            OnSelectedIndexChanged="grdBill_SelectedIndexChanged" Width="100%" AutoGenerateColumns="False"
                            AllowSorting="True" AllowPaging="True" PageSize="20">
                            <Columns>
                                <asp:BoundField DataField="DueDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data"
                                    SortExpression="DueDate"></asp:BoundField>
                                <asp:BoundField DataField="Description" HeaderText="Parcela" SortExpression="Description">
                                </asp:BoundField>
                                <asp:BoundField DataField="DocumentNumber" HeaderText="Nº do Documento" SortExpression="DocumentNumber">
                                </asp:BoundField>
                                <asp:BoundField DataField="SupplierName" HeaderText="Fornecedor" SortExpression="SupplierName">
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="Valor(R$)" SortExpression="Amount"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="text-align: center">
                                    Não existem dados a serem exibidos &nbsp;
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </contenttemplate>
                </asp:UpdatePanel>
                <br />
                <br />
                <asp:UpdatePanel ID="upInvoice" runat="server">
                    <contenttemplate>
                        <h3>
                            Contas à receber</h3>
                        <asp:GridView ID="grdInvoices" runat="server" Width="100%" AllowSorting="True" AutoGenerateColumns="False"
                            DataSourceID="odsInvoices" DataKeyNames="InvoiceId,CompanyId,DocumentNumber,Creditor,InvoiceValue,EntryDate,Description,ReceiveLocal,CustomerId,CostCenterId"
                            OnSelectedIndexChanged="grdInvoices_SelectedIndexChanged" AllowPaging="True"
                            PageSize="20">
                            <Columns>
                                <asp:BoundField DataField="DueDate" SortExpression="DueDate" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderText="Data"></asp:BoundField>
                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Parcela">
                                </asp:BoundField>
                                <asp:BoundField DataField="DocumentNumber" SortExpression="DocumentNumber" HeaderText="Nº do Documento">
                                </asp:BoundField>
                                <asp:BoundField DataField="customerName" SortExpression="customerName" HeaderText="Cliente">
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Valor(R$)"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="text-align: center">
                                    Não existem dados a serem exibidos<br />
                                    &nbsp;</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </contenttemplate>
                </asp:UpdatePanel>
                <br />
                <br />
                <asp:UpdatePanel ID="upProducts" runat="server">
                    <contenttemplate>
                        <h3>
                            Produtos com estoque mínimo</h3>
                        <asp:GridView ID="grdStock" runat="server" DataSourceID="odsStock" AllowSorting="True"
                            OnSelectedIndexChanged="grdStock_SelectedIndexChanged" AutoGenerateColumns="False"
                            DataKeyNames="ProductId,Name,ProductCode,Description" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="Codigo" />
                                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Nome" ItemStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%-- <asp:BoundField DataField="Description" HeaderText="Descrição" />--%>
                                <%--<asp:BoundField DataField="QuantityinReserve" HeaderText="Quantidade" />--%>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="text-align: center">
                                    Não existem dados a serem exibidos<br />
                                    &nbsp;</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </contenttemplate>
                </asp:UpdatePanel>
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
    <uc1:ToolTip ID="tipEmployees" runat="server" Message="Para saber como está a “vida” financeira do seu negócio, lance todas as contas e os ganhos da empresa no seu InfoControl. Com esta atitude simples, haverá certeza dos valores gerados em relatórios. E ainda, você terá o InfoControl para alertar cada vencimento cadastrado."
        Title="Dica:" Indication="left" Top="0px" Left="180px" Visible="true" />
    <VFX:BusinessManagerDataSource ID="odsBills" runat="server" ConflictDetection="CompareAllValues"
        onselecting="dataSource_Selecting" SelectMethod="GetExpiredBills" TypeName="Vivina.Erp.BusinessRules.BillManager"
        OldValuesParameterFormatString="original_{0}" EnablePaging="True" SelectCountMethod="GetExpiredBillsCount"
        SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="CompanyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" id="odsInvoices" ConflictDetection="CompareAllValues"
        onselecting="odsInvoices_Selecting" SelectMethod="GetExpiredInvoicesInPeriod"
        TypeName="Vivina.Erp.BusinessRules.InvoicesManager" OldValuesParameterFormatString="original_{0}"
        EnablePaging="True" SortParameterName="SortExpression" SelectCountMethod="GetExpiredInvoicesInPeriodCount">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsStock" runat="server" ConflictDetection="CompareAllValues"
        SelectMethod="RetrievingShortageProducts" TypeName="Vivina.Erp.BusinessRules.ProductManager"
        OldValuesParameterFormatString="original_{0}" SortParameterName="sortExpression"
        onselecting="odsStock_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
