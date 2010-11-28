<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Invoices" Title="Contas à Receber"
    CodeBehind="Invoices.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
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
                    <legend><span id="filterLegend">Escolha o filtro desejado:</span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span
                        id="exportLegend">Exportação:</span> </legend>
                    <div class="body" id="filterBody">
                        <uc1:AccountSearch ID="ucAccountSearch" runat="server" OnSelectedSearchAccountParameters="SearchAccount_SelectedParameters" />
                    </div>
                    <div class="body" id="exportBody">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbtExportBoletu" GroupName="Export" Text="Boleto" runat="server" />
                                    <asp:RadioButton ID="rbtExportAutomaticDebit" GroupName="Export" Text="Débito automatico"
                                        runat="server" />
                                </td>
                                <td>
                                    Conta bancária:<br />
                                    <asp:DropDownList ID="cboAccount" runat="server" DataSourceID="odsAccount" DataTextField="ShortName"
                                        DataValueField="AccountId" CausesValidation="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnExport" runat="server" Text="Gerar Remessa" Visible="true" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <span class="closeButton" id="closeFilter">&nbsp;</span>
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <telerik:RadGrid ID="grdInvoices" runat="server" CssClass="cGrd21" Skin="" AllowSorting="True"
                    AutoGenerateColumns="False" DataSourceID="odsSearchInvoices" GridLines="Horizontal"
                    OnSortCommand="grdInvoices_SortCommand" AllowAutomaticDeletes="True" OnItemCommand="grdInvoices_ItemCommand"
                    ShowFooter="True" showgroupfooter="true" EnableTheming="True" ShowGroupPanel="True"
                    OnItemDataBound="grdInvoices_ItemDataBound" EnableViewState="False" PageSize="20"
                    OnDataBound="grdInvoices_DataBound" AllowPaging="True">
                    <MasterTableView GroupLoadMode="Client" ShowGroupFooter="true" AllowMultiColumnSorting="false"
                        DataKeyNames="InvoiceId,CompanyId,EntryDate,Description,CustomerId,CostCenterId,DueDate"
                        NoMasterRecordsText="&lt;div style=&quot;text-align: center&quot;&gt; Não existem dados a serem exibidos.&lt;br /&gt;&lt;/div&gt;"
                        DataSourceID="odsSearchInvoices" EnableViewState="False">
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="False" Resizable="False">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="DueDate" HeaderText="Vence Em" UniqueName="column"
                                DataFormatString="{0:dd/MM/yyyy}" SortExpression="DueDate" FilterImageToolTip=""
                                HeaderButtonType="TextButton" Resizable="False" ShowSortIcon="False">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="TemplateColumn">
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Description" HeaderText="Descrição" UniqueName="column1"
                                SortExpression="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CustomerName" HeaderText="Cliente" UniqueName="column2"
                                SortExpression="CustomerName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Desc" HeaderText="Parcela" UniqueName="column4"
                                SortExpression="Desc">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Amount" HeaderText="Valor(R$)" AllowFiltering="true"
                                Aggregate="sum" FooterText="Total: " SortExpression="Amount" UniqueName="Amount"
                                Groupable="False">
                                <FooterStyle Font-Bold="True" ForeColor="Black" />
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn HeaderText="<a href='Invoice.aspx' class='insert'>&nbsp;&nbsp;&nbsp;&nbsp;</a>"
                                SortExpression="Insert" ConfirmText="Deseja realmente excluir?" ButtonType="LinkButton"
                                CommandName="Delete" Text="Excluir" UniqueName="DeleteColumn">
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
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
                        <EditFormSettings>
                            <PopUpSettings ScrollBars="None"></PopUpSettings>
                        </EditFormSettings>
                    </MasterTableView>
                    <GroupingSettings CaseSensitive="False" CollapseTooltip="" ExpandTooltip="" GroupByFieldsSeparator=""
                        GroupContinuedFormatString="" GroupContinuesFormatString="" GroupSplitDisplayFormat="Showing {0} "
                        UnGroupTooltip="" />
                    <GroupHeaderItemStyle Wrap="False" />
                    <ClientSettings EnablePostBackOnRowClick="False" AllowDragToGroup="True" EnableRowHoverStyle="True">
                        <Selecting AllowRowSelect="True" />
                        <ClientEvents OnRowMouseOver="RowMouseOver" OnRowMouseOut="RowMouseOut" />
                    </ClientSettings>
                    <HeaderStyle CssClass="Header"></HeaderStyle>
                    <ItemStyle CssClass="Item"></ItemStyle>
                    <AlternatingItemStyle CssClass="Item"></AlternatingItemStyle>
                    <FooterStyle CssClass="FooterStyle"></FooterStyle>
                    <PagerStyle CssClass="Pager" Mode="NumericPages"></PagerStyle>
                    <GroupHeaderItemStyle></GroupHeaderItemStyle>
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
    <VFX:businessmanagerdatasource id="odsAccount" runat="server" onselecting="odsAccount_Selecting"
        selectmethod="GetAccountsWithShortName" typename="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
          <asp:Parameter Name="companyId" Type="Int32" />
     </selectparameters>
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource id="odsSearchInvoices" runat="server" onselecting="odsSearchInvoices_Selecting"
        selectmethod="GetInvoices" typename="Vivina.Erp.BusinessRules.FinancialManager"
        sortparametername="sortExpression" oldvaluesparameterformatstring="original_{0}"
        SelectCountMethod="GetInvoicesCount">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="accountSearch" Type="Object" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
         
            
            
        </selectparameters>
    </VFX:businessmanagerdatasource>
</asp:Content>
