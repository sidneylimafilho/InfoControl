<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_Sale_Parcels" Title="Parcelas" EnableEventValidation="false"
    CodeBehind="Sale_Parcels.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/Date.ascx" TagName="Date" TagPrefix="uc2" %>

<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc8" %>

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
            <td class="center" align="left">
                <table>
                    <tr>
                        <td align="left" style="white-space: nowrap;">
                            Vendedor:<br />
                            <asp:DropDownList ID="cboEmployee" runat="server" DataSourceID="odsSalesPerson" DataTextField="AbreviatedName"
                                AppendDataBoundItems="true" DataValueField="EmployeeId">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="cboEmployee" ValidationGroup="addParcel">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <tr>
                            <td colspan="2" align="left" style="white-space: nowrap;">
                                <asp:Label ID="lblCfop" runat="server" Visible="false">CFOP:<br /></asp:Label>
                                <asp:DropDownList ID="cboCFOP" runat="server" DataTextField="name" DataValueField="CfopId"
                                    DataSourceID="odsAccount" Visible="false">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    ControlToValidate="cboCFOP" ValidationGroup="Payment">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </tr>
                </table>
                <div style="text-align: center; width: 100%">
                    <fieldset style="width: 90%">
                        <legend>Dados de Pagamento (Parcelas)</legend>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    Forma de Pagamento:<br />
                                    <asp:DropDownList ID="cboFinancierOperations" runat="server" DataSourceID="odsPaymentMethods"
                                        DataTextField="PaymentMethodName" DataValueField="FinancierOperationId">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqcboFinancierOperations" runat="server" ControlToValidate="cboFinancierOperations"
                                        CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="addParcel">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td align="left">
                                      Data da Parcela:<br />
                                     <uc2:Date ID="ucDueDate" Required="true" ValidationGroup="addParcel" runat="server" />
                                </td>
                                <td align="left">
                                    Valor Total:<br />
                                  <uc8:CurrencyField ID="ucCurrFieldAmount" Required="true" ValidationGroup="addParcel" runat="server" />
                                
                                </td>
                                <td align="left">
                                    Qtd:<br />
                                    <uc8:CurrencyField ID="ucCurrFieldQuantityParcels" Mask="999" Required="true" ValidationGroup="addParcel" runat="server" />
                                        
                                </td>
                                <td>
                                    <br />
                                    <asp:ImageButton ID="btnAddParcel" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                        runat="server" AlternateText="Inserir Parcela" OnClick="btnAddParcel_Click" ValidationGroup="addParcel" /><br />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td colspan="5">
                                    <div style="text-align: left">
                                        <asp:GridView ID="grdParcels" runat="server" AutoGenerateColumns="False" Width="100%"
                                            DataKeyNames="FinancierOperationId,Amount" PageSize="20" OnRowDataBound="grdParcels_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="FinancierOperationName" HeaderText="Forma de Pagamento"
                                                    SortExpression="FinancierOperationName"></asp:BoundField>
                                                <asp:BoundField DataField="DueDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Vencimento"
                                                    SortExpression="DueDate"></asp:BoundField>
                                                <asp:BoundField DataField="Amount" DataFormatString="{0:c}" HeaderText="Valor" SortExpression="Amount">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                Ainda não há parcelas cadastradas para esta venda</EmptyDataTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <br />
                    <div style="text-align: right">
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblTotalSale" runat="server" Text=""></asp:Label><br />
                                    <asp:Label ID="lblTotalParcels" runat="server" Text=""></asp:Label><br />
                                    <br />
                                    <asp:Label ID="lblDif" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <div style="text-align: center;">
                        <asp:CheckBox ID="chkPrint" runat="server" Text="Imprimir Comprovante" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkReceipt" runat="server" Text="Gerar nota fiscal" Enabled="true" Checked="true" />
                        <br />
                        <br />
                        &nbsp;&nbsp;
                        <input id="" accesskey="F7" onclick="top.$.LightBoxObject.close();top.content.focus();return false;"
                            src="../../app_Themes/_global/Company/bt_cancelar_pagamento.gif" type="image" />
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="btnFinishSale" runat="server" ImageUrl="~/app_Themes/_global/Company/bt_concluir_venda2.gif"
                            ValidationGroup="addParcel" OnClick="btnFinishSale_Click" />
                    </div>
                    <VFX:BusinessManagerDataSource ID="odsPaymentMethods" runat="server" OnSelecting="odsFinancierOperation_Selecting"
                        SelectMethod="GetPaymentMethods" TypeName="Vivina.Erp.BusinessRules.AccountManager">
                        <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
            </selectparameters>
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsAccount" runat="server" SelectMethod="GetCFOPFormatted"
                        TypeName="Vivina.Erp.BusinessRules.AccountManager">
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsParcels" runat="server">
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsSalesPerson" runat="server" OnSelecting="odsSalesPerson_Selecting"
                        SelectMethod="GetSalesPerson" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
                        <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
            </selectparameters>
                    </VFX:BusinessManagerDataSource>
                    <VFX:Ecf ID="Ecf" runat="server">
                    </VFX:Ecf>
                    <uc1:ToolTip ID="ttpEmployee" runat="server" Left="105px" Top="58px" Message="Para efetuar uma venda é necessário ter um funcionário vendedor<br />Se deseja cadastrar um vendedor agora, <a href=javascript:top.$.modal.Hide();top.content.location.href='../RH/Employee.aspx'>clique aqui !</a>" />
                </div>
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
    <script type="text/javascript">
        window.focus();
    </script>
    <uc1:ToolTip ID="ToolTip1" runat="server" Left="200px" Top="130px" Indication="left"
        Message=" &quot;Formas de Pagamento &quot;, refere-se à Cartão de Crédito, Dinheiro , entre outras que sua empresa pode trabalhar. Para adicionar as &quot; Formas de Pagamento &quot; necessárias, vá em : &quot; ADMINISTRAÇÃO/FORMAS DE PAGAMENTO &quot; e configure de acordo com as suas necessidades." />
</asp:Content>
