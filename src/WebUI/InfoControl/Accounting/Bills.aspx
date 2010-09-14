<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Bills" Title="Contas à Pagar" CodeBehind="Bills.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="AccountSearch.ascx" TagName="AccountSearch" TagPrefix="uc1" %>
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
                <fieldset id="filter" class="closed">
                    <legend><span id="filterLegend">Escolha o filtro desejado:</span></legend><div class="body"
                        id="filterBody">
                        <uc1:AccountSearch ID="ucAccountSearch" OnSelectedSearchAccountParameters="SearchAccount_SelectedParameters"
                            runat="server" />
                    </div><span id="closeFilter" class="closeButton">&nbsp;</span>
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <telerik:RadGrid ID="grdBill" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    DataSourceID="odsSearchBills" ShowGroupPanel="True" OnItemCommand="grdBill_ItemCommand"
                    OnSortCommand="grdBill_SortCommand" AllowAutomaticDeletes="True" showgroupfooter="true"
                    OnItemDataBound="grdBill_ItemDataBound" ShowFooter="True" GridLines="Horizontal"
                    Width="100%" EnableViewState="False" PageSize="20" RowSelectable="false" AllowPaging="True"> 
                    <MasterTableView GroupLoadMode="Client" ShowGroupFooter="true" AllowMultiColumnSorting="false"
                        DataKeyNames="BillId,AccountId,AccountingPlanId,CompanyId,DocumentType,DocumentNumber,EntryDate,CostCenterId,DueDate,Description,SupplierName"
                        NoMasterRecordsText="&lt;div style=&quot;text-align: center&quot;&gt; Não existem dados a serem exibidos.&lt;br /&gt;&lt;/div&gt;"
                        DataSourceID="odsSearchBills" EnableViewState="False">
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="False" Resizable="False">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="Data" FieldName="DueDate" FormatString="{0:dd/MM/yyyy}">
                                    </telerik:GridGroupByField>
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="DueDate" SortOrder="Descending" FormatString="{0:dd/MM/yyyy}">
                                    </telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>
                        <Columns>
                            <telerik:GridBoundColumn DataField="DueDate" HeaderText="Data de vencimento" SortExpression="DueDate"
                                UniqueName="DueDate" FilterImageToolTip="" HeaderButtonType="TextButton" Resizable="False"
                                ShowSortIcon="False" DataFormatString="{0:dd/MM/yyyy}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DocumentNumber" HeaderText="Nº documento" SortExpression="DocumentNumber"
                                UniqueName="DocumentNumber">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" HeaderText="Descrição" SortExpression="Description"
                                UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SupplierName" HeaderText="Fornecedor" SortExpression="SupplierName"
                                UniqueName="SupplierName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Amount" HeaderText="Valor(R$)" Aggregate="sum"
                                FooterAggregateFormatString="Total: <b>{0}</b>" SortExpression="Amount" UniqueName="Amount"
                                Groupable="False">
                                <FooterStyle Font-Bold="True" ForeColor="Black" />
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn HeaderText="<a href='Bill.aspx' class='insert'>  &nbsp;&nbsp;&nbsp;&nbsp;</a>"
                                SortExpression="Insert" CommandName="Delete" Text="Excluir" UniqueName="DeleteColumn">
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings>
                            <PopUpSettings ScrollBars="None"></PopUpSettings>
                        </EditFormSettings>
                    </MasterTableView>
                    <GroupingSettings CaseSensitive="False" CollapseTooltip="" ExpandTooltip="" GroupByFieldsSeparator=""
                        GroupContinuedFormatString="" GroupContinuesFormatString="" GroupSplitDisplayFormat="Showing {0} "
                        UnGroupTooltip="" />
                    <GroupHeaderItemStyle Wrap="False" />
                    <ClientSettings EnablePostBackOnRowClick="False" AllowDragToGroup="True">
                        <Selecting AllowRowSelect="True" />
                        <ClientEvents OnRowMouseOver="RowMouseOver" OnRowMouseOut="RowMouseOut" />
                    </ClientSettings>
                    <PagerStyle Mode="NumericPages" AlwaysVisible="true"></PagerStyle>
                </telerik:RadGrid>
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
    <VFX:BusinessManagerDataSource ID="odsSearchBills" runat="server" onselecting="odsSearchBills_Selecting"
        TypeName="Vivina.Erp.BusinessRules.FinancialManager" SelectMethod="GetBills"
        sortparametername="sortExpression" EnablePaging="True" SelectCountMethod="GetBillsCount">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="accountSearch" Type="Object" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
         </selectparameters>
    </VFX:BusinessManagerDataSource>
    
    <VFX:BusinessManagerDataSource ID="odsAccountingPlan" runat="server" OnSelecting="odsAccountingPlan_Selecting"
        SelectMethod="GetOutboundPlan" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
