<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Company_Stock" Codebehind="Stock.aspx.cs" Title="Controle de Estoque" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:FormView ID="frmStocks" runat="server" DataSourceID="odsStocks" 
		DefaultMode="Insert" oniteminserting="frmStocks_ItemInserting">
            <EditItemTemplate>
                <table>
                    <tr>
                        <td colspan="2">
                            Item:<br />
                            <asp:TextBox ID="ItemTextBox" runat="server" Text='<%# Bind("ItemId") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Moeda:<br />
                            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="odsCurrencyRate"
                                DataTextField="Name" DataValueField="CurrencyRateId">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Valor Unitário:<br />
                            <asp:TextBox ID="UnitPriceTextBox" runat="server" Text='<%# Bind("UnitPrice") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Quantidade:<br />
                            <asp:TextBox ID="QuantityTextBox" runat="server" Text='<%# Bind("Quantity") %>' />
                        </td>
                        <td>
                            Localização:<br />
                            <asp:TextBox ID="LocalizationTextBox" runat="server" Text='<%# Bind("Localization") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Fornecedor:<br />
                            <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="odsSupplier" DataTextField="Name"
                                DataValueField="SupplierId">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="ItemTextBox"
                    ServiceMethod="GetProductsCompletion" ServicePath="SearchProducts.asmx" MinimumPrefixLength="2"
                    CompletionInterval="1000" >
                </ajaxToolkit:AutoCompleteExtender>
                <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                    CssClass="cLnk11" Text="Inserir" Visible="<%# frmStocks.CurrentMode == FormViewMode.Insert %>"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                    CssClass="cLnk11" Text="Salvar" Visible="<%# frmStocks.CurrentMode == FormViewMode.Edit %>"></asp:LinkButton>
                <asp:LinkButton ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    CssClass="cLnk11" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
        </asp:FormView>
        <VFX:BusinessManagerDataSource ID="odsCurrencyRate" runat="server" SelectMethod="GetAllCurrencyRates"
            TypeName="Vivina.Erp.BusinessRules.CurrencyRateManager">
        </VFX:BusinessManagerDataSource>
        <VFX:BusinessManagerDataSource ID="odsSupplier" runat="server" SelectMethod="GetAllSuppliers"
            TypeName="Vivina.Erp.BusinessRules.SupplierManager">
        </VFX:BusinessManagerDataSource>
    </div>
    <VFX:BusinessManagerDataSource ID="odsStocks" runat="server" ConflictDetection="CompareAllValues"
        DataObjectTypeName="Vivina.Erp.DataClasses.Stock" DeleteMethod="Delete"
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetStockByCompany"
        TypeName="Vivina.Erp.BusinessRules.StockManager" UpdateMethod="Update">
        <UpdateParameters>
            <asp:parameter Name="original_entity" Type="Object" />
            <asp:parameter Name="entity" Type="Object" />
        </UpdateParameters>
        <SelectParameters>
            <asp:parameter Name="CompanyId" Type="Int32" />
        </SelectParameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
