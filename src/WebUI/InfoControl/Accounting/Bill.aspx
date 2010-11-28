<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Bill" Title="Contas à Pagar" CodeBehind="Bill.aspx.cs" %>

<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc4" %>
<%@ Register Src="~/InfoControl/Administration/SelectSupplier.ascx" TagName="SelectSupplier"
    TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc3" %>
<%@ Register Src="~/App_Shared/Parcels.ascx" TagName="Parcels" TagPrefix="uc1" %>
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
                <table>
                    <tr>
                        <td style="width: 33%">
                            <asp:RadioButton ID="rbtGuia" runat="server" Text="Guia de Recolhimento" GroupName="Type" />
                        </td>
                        <td style="width: 33%">
                            <asp:RadioButton ID="rbtReceipt" runat="server" Text="Nota Fiscal" GroupName="Type" />
                        </td>
                        <td style="width: 33%">
                            <asp:RadioButton ID="rbtOthers" runat="server" Text="Outros" GroupName="Type" Checked='<%# Convert.ToInt32(Eval("DocumentType")) == 3%>' />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="75%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <uc2:SelectSupplier ID="selSupplier" runat="server" OnSelectedSupplier="SelSupplier_SelectedSupplier" />
                                    </td>
                                    <td>
                                        <VFX:HelpTooltip ID="HelpTooltip3" runat="server">
                                            <ItemTemplate>
                                                <h2>
                                                    Ajuda:</h2>
                                                No caso de uma conta que envolva fornecimento de produtos para sua empresa, digite
                                                aqui o nome do fornecedor com qual você comprou tais produtos.
                                            </ItemTemplate>
                                        </VFX:HelpTooltip>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            Descrição:<br />
                            <asp:TextBox ID="txtDescription" runat="server" Columns="50" MaxLength="1024"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtDescription" runat="server" ControlToValidate="txtDescription"
                                 ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="AddBill">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
                            N.Doc:<br />
                            <asp:TextBox ID="txtDocumentNumber" runat="server" Columns="50" Visible="false" MaxLength="10" />
                            &nbsp;&nbsp;&nbsp;
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    Centro de custos:
                                                </td>
                                                <td>
                                                    <VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                                                        <ItemTemplate>
                                                            <h2>
                                                                Ajuda:</h2>
                                                            Centro de Custos indicam quais são os departamentos da sua empresa estão envolvidos
                                                            com os custos gerados. Por exemplo, departamento de vendas, de recursos humanos,
                                                            da presidência, etc.
                                                        </ItemTemplate>
                                                    </VFX:HelpTooltip>
                                                </td>
                                            </tr>
                                        </table>
                                        <uc3:ComboTreeBox ID="cboCostCenter" runat="server" DataSourceID="odsCostCenter"
                                            DataFieldID="CostCenterId" DataFieldParentID="ParentId" DataTextField="Name"
                                            DataValueField="CostCenterId" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator CssClass="cErr21" ID="valcboCostCenter" runat="server" ControlToValidate="cboCostCenter"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" EnableClientScript="true"
                                            ValidationGroup="AddBill" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    Plano de contas:
                                                </td>
                                                <td>
                                                    <VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                                                        <ItemTemplate>
                                                            <h2>
                                                                Ajuda:</h2>
                                                            Com o plano de contas é possível controlar e categorizar as contas da sua empresa.
                                                            Por exemplo gastos funcionais como água, luz telefone, etc.
                                                        </ItemTemplate>
                                                    </VFX:HelpTooltip>
                                                </td>
                                            </tr>
                                        </table>
                                        <uc3:ComboTreeBox ID="cboAccountPlan" runat="server" DataSourceID="odsAccountingPlan"
                                            DataFieldID="AccountingPlanId" DataFieldParentID="ParentId" DataTextField="Name"
                                            DataValueField="AccountingPlanId" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator CssClass="cErr21" ID="valcboAccountPlan" runat="server" ControlToValidate="cboAccountPlan"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" EnableClientScript="true" Display="Dynamic"
                                            ValidationGroup="AddBill"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <uc1:Parcels ID="ucParcels" runat="server" />
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnNew" runat="server" Text="Salvar e Continuar" ValidationGroup="AddBill"
                                OnClick="btnNew_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" ValidationGroup="AddBill" OnClick="btnSave_Click"
                                CausesValidation="true" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
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
    <VFX:BusinessManagerDataSource ID="odsAccountingPlan" runat="server" OnSelecting="dataSource_Selecting"
        SelectMethod="GetOutboundPlan" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCostCenter" runat="server" SelectMethod="GetCostsCenterAsDataTable"
        TypeName="Vivina.Erp.BusinessRules.AccountManager" onselecting="dataSource_Selecting">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsPaymentReader" runat="server" OnSelecting="dataSource_Selecting"
        SelectMethod="GetCompanyPaymentMethodsReader" TypeName="Vivina.Erp.BusinessRules.PaymentMethodManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAccount" runat="server" SelectMethod="GetCFOP"
        TypeName="Vivina.Erp.BusinessRules.AccountManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
