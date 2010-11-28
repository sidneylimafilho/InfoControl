<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Sale2.aspx.cs" Inherits="Vivina.Erp.WebUI.POS.Sale2"
    EnableEventValidation="false" Title="Ponto de Venda" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc3" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <table>
                    <tr>
                        <td>
                            Qtd:<br />
                            <uc1:CurrencyField ID="ucCurrFieldQuantityData" columns="3" Required="true" Mask="999"
                                ValidationGroup="AddProductToCart" runat="server" />
                        </td>
                        <td>
                            <uc3:SelectProduct ID="selProduct" Required="true" ValidationGroup="AddProductToCart"
                                runat="server" />
                        </td>
                        <td>
                            Preço:<br />
                            <uc1:CurrencyField ID="ucCurrFieldUnitPrice" runat="server" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~\App_Shared/modules\_global\Company\shoppingbasket_add.gif"
                                OnClick="btnAdd_Click" ValidationGroup="AddProductToCart" />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            Tipo de venda:<br />
                            <asp:DropDownList ID="cboUnitPriceName" runat="server" AutoPostBack="True" OnTextChanged="cboUnitPriceName_TextChanged"
                                AppendDataBoundItems="true">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdSaleItens" Width="100%" BorderStyle="Solid" runat="server" AutoGenerateColumns="False"
                                OnRowDataBound="grdSaleItens_RowDataBound" RowSelectable="false" DataKeyNames="ProductId"
                                OnRowDeleting="grdSaleItens_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="Quantity" HeaderText="Qtd." />
                                    <asp:TemplateField HeaderText="Nome">
                                        <ItemTemplate>
                                            &nbsp;&nbsp;&nbsp;<%#Eval("Name")%>
                                            <asp:Image ID="imgProduct_warning" ImageUrl="~/App_Shared/themes/glasscyan/Company/Product_warning.gif"
                                                AlternateText="Este produto não se encontra cadastrado!" runat="server" Visible='<%# Eval("Code")=="0" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Price" HeaderText="V.Unitário" />
                                    <asp:TemplateField HeaderText="Série">
                                        <ItemTemplate>
                                            <center>
                                                <asp:DropDownList ID="cboSerialNumber" AutoPostBack="true" runat="server" DataValueField="InventorySerialId"
                                                    AppendDataBoundItems="true" DataTextField="Serial" OnTextChanged="cboSerialNumber_TextChanged">
                                                </asp:DropDownList>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProfitPrice" HeaderText="Valor Total" />
                                    <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                        ShowDeleteButton="True">
                                        <ItemStyle Width="20px" />
                                    </asp:CommandField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Adicione um Item
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <uc2:SelectCustomer ID="sel_customer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                        </td>
                        <td>
                            <b>Orçamentos:</b><br />
                            <asp:DataList ID="lstBudget" runat="server" DataSourceID="odsBudgets" OnSelectedIndexChanged="lstBudget_SelectedIndexChanged"
                                DataKeyField="BudgetId">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:LinkButton ID="link" runat="server" Text='<%# Eval("BudgetCode") + " - " + Convert.ToDateTime(Eval("ModifiedDate")).ToShortDateString()%>'
                                                    CommandName="Select" ToolTip='<%# Eval("BudgetCode") %>' />
                                                -
                                            </td>
                                            <td valign="top">
                                                <asp:LinkButton ID="LinkButton1" CssClass="delete" runat="server" CommandArgument='<%# Eval("BudgetId") %>'
                                                    CommandName="Delete" OnCommand="btnDeleteBudget_Command" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                        <td style="padding-left: 15px;" valign="top">
                            <asp:UpdatePanel ID="upDiscount" runat="server">
                                <contenttemplate>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        
                                        <img src="../../App_Shared/themes/glasscyan/Company/subtotal.gif" alt="Subtotal" />
                                    </td>
                                    <td style="text-align: right; padding-right: 32px;">
                                        <asp:Label ID="lblSubtotal" runat="server" Text="00.000,00" CssClass="cTxt42b"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src="../../App_Shared/themes/glasscyan/Company/desconto.gif" alt="Desconto" />
                                    </td>
                                    
                                    <td nowrap="nowrap" style="text-align: right; padding-right: 25px;">
                                        <asp:TextBox ID="txtDiscount" AutoPostBack="true" CssClass="cDat21" runat="server"
                                            Font-Size="16pt" Font-Bold="true" ForeColor="#1A6E6A" Columns="7" MaxLength="9"
                                            Text="00.000,00" OnTextChanged="txtDiscount_TextChanged"></asp:TextBox>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" nowrap="nowrap" valign="bottom" align="center" >
                                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                        <asp:Label ID="lblTotal" runat="server" Text="00.000,00" CssClass="cTxt44b"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                             </contenttemplate>
                            </asp:UpdatePanel>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <td>
                                            <asp:ImageButton ID="btnPayment" AlternateText="Efetuar venda" runat="server" ImageUrl="../../App_Shared/themes/glasscyan/Company/bt_concluir_venda.gif"
                                                OnClick="btnPayment_Click" />
                                        </td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <VFX:BusinessManagerDataSource ID="odsBudgets" runat="server" OnSelecting="odsBudgets_Selecting"
                    SelectMethod="GetBudgetByCustomerExcludingSales" TypeName="Vivina.Erp.BusinessRules.SaleManager">
                    <selectparameters>
            <asp:Parameter Name="CustomerId" Type="Int32" />
            <asp:Parameter Name="CompanyId" Type="Int32" />
        </selectparameters>
                </VFX:BusinessManagerDataSource>
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
</asp:Content>
