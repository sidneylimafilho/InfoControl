<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Company_Stock_Movement" Codebehind="StockMovement.aspx.cs" Title="Transferência entre Estoques" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
        <table class="cLeafBox21" width="100%">
            <tr class="top">
                <td class="left">
                    &nbsp;
                </td>
                <td class="center">
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Orange" Visible="False"></asp:Label>
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
                                Depósito de Destino:<br />
                                <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                    DataValueField="DepositId" AppendDataBoundItems="True" Width="150px">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    runat="server" ControlToValidate="cboDeposit" CssClass="cErr21" ValidationGroup="Outside"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                                <center>
                                <asp:GridView ID="grdInventory" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnRowDataBound="grdInventory_RowDataBound" DataKeyNames="ProductId" OnRowUpdating="grdInventory_RowUpdating"
                                    OnRowDeleting="grdInventory_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Produto">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProduct" runat="server" AutoCompleteType="Disabled" TabIndex="0"
                                                    Text='<%# Bind("productName") %>' Columns="50" CssClass="cDynDat11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProduct"
                                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="1000"
                                                     FirstRowSelected="True" MinimumPrefixLength="3" ServiceMethod="SearchProductInInventory"
                                                    ServicePath="SearchService.asmx" TargetControlID="txtProduct">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("productName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade">
                                            <EditItemTemplate>
                                                <input id="btnDown" class="cUpDown11" tabindex="10" type="button" value="-" /><asp:TextBox
                                                    ID="txtQuantity" runat="server" Columns="4" MaxLength="6" TabIndex="0" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                                                <input id="btnUp" class="cUpDown11" tabindex="11" type="button" value="+" />
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtQuantity"
                                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" MaximumValue="100000000"
                                                    MinimumValue="0" Type="Integer" ValidationGroup="Grid">*</asp:RangeValidator>
                                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" Maximum="100000"
                                                    Minimum="0" TargetButtonDownID="btnDown" TargetButtonUpID="btnUp" TargetControlID="txtQuantity"
                                                    Width="60">
                                                </ajaxToolkit:NumericUpDownExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;" />
                                        <asp:CommandField ShowEditButton="True" UpdateText="&lt;img src='../../App_Themes/Glass/Controls/GridView/img/Add2.gif' border='0'  tabindex='15' /&gt;"
                                            CancelText="" ShowCancelButton="False" ValidationGroup="Grid" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: right">
                                <br />
                                <asp:Button ID="btnSave" CssClass="cBtn11" Text="Salvar" runat="server" OnClick="btnSave_Click"
                                    ValidationGroup="Outside" TabIndex="99" UseSubmitBehavior="False" />
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
    </div>
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" onselecting="odsDeposit_Selecting"
        SelectMethod="GetRemainingDeposit" TypeName="Vivina.Erp.BusinessRules.DepositManager">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />
		    <asp:parameter Name="userId" Type="Int32"></asp:parameter>
		</selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsSupplier" runat="server" OnSelecting="odsSupplier_Selecting"
        SelectMethod="GetSupplierByCompany" TypeName="Vivina.Erp.BusinessRules.SupplierManager">
        <selectparameters>
            <asp:parameter Name="CompanyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCurrencyRate" runat="server" SelectMethod="GetAllCurrencyRates"
        TypeName="Vivina.Erp.BusinessRules.CurrencyRateManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
