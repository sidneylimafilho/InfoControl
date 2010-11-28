<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Accouting_Invoice" Title="Contas à Receber "
    CodeBehind="Invoice.aspx.cs" %>

<%@ Register Src="~/App_Shared/Parcels.ascx" TagName="Parcels" TagPrefix="uc1" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/InfoControl/Accounting/AccountingPlan.ascx" TagName="AccountingPlan"
    TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc3" %>
<%@ Register Src="~/App_Shared/Parcels.ascx" TagName="Parcels" TagPrefix="uc4" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc5" %>
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
                <table width="75%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <uc2:SelectCustomer ID="SelCustomer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator CssClass="cErr21" ID="reqSelCustomer" runat="server" ControlToValidate="SelCustomer"
                                             ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="AddInvoice">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            Descrição:<br />
                            <asp:TextBox ID="txtSource" runat="server" Columns="40" MaxLength="1024"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="valTxtSource" runat="server" ControlToValidate="txtSource"
                                 ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="AddInvoice">&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                            ValidationGroup="AddInvoice" Display="Dynamic"></asp:RequiredFieldValidator>
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
                                                            Por exemplo gastos funcionais como água, luz, telefone, etc.
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
                                            ValidationGroup="AddInvoice"></asp:RequiredFieldValidator>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlInvoiceSource" runat="server" Visible="false">
                    <fieldset>
                        <legend>Origem:</legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSale" runat="server" Text="Venda: " Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:HyperLink ID="lnkSale" runat="server"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblReceipt" runat="server" Text="Nº Fiscal: " Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:HyperLink ID="lnkReceipt" runat="server"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblContract" runat="server" Text="Contrato: " Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:HyperLink ID="lnkContract" runat="server"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>
                <br />
                <uc1:Parcels ID="ucParcels" runat="server" />
                <br />
                <br />
                <table align="left">
                    <tr>
                        <td>
                            Modelo de Documento:
                            <br>
                            <asp:DropDownList runat="server" DataSourceID="odsInvoiceModels" DataTextField="FileName"
                                DataValueField="DocumentTemplateId" ID="cboInvoiceModel" />
                        </td>
                        <td>
                            <asp:Button ID="btnDownload" ValidationGroup="AddInvoice" OnClick="btnDownload_Click"
                                runat="server" Text="Download" />
                        </td>
                        <td>
                            <asp:Button ID="btnBoleto" runat="server" OnClick="btnBoleto_Click" ValidationGroup="AddInvoice"
                                Text="Gerar Boleto" />
                        </td>
                    </tr>
                </table>
                <table align="right">
                    <tr>
                        <td align="right">
                            &nbsp;&nbsp;
                            <asp:Button ID="btnNew" runat="server" Text="Novo" OnClick="btnNew_Click" CausesValidation="true"
                                ValidationGroup="AddInvoice" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" ValidationGroup="AddInvoice"
                                OnClick="btnSave_Click" CausesValidation="true" />
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
    <VFX:BusinessManagerDataSource ID="odsPaymentReader" runat="server" OnSelecting="dataSource_Selecting"
        SelectMethod="GetCompanyPaymentMethodsReader" TypeName="Vivina.Erp.BusinessRules.PaymentMethodManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCostCenter" runat="server" OnSelecting="dataSource_Selecting"
        SelectMethod="GetCostsCenterAsDataTable" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAccountingPlan" runat="server" OnSelecting="dataSource_Selecting"
        SelectMethod="GetInboundPlan" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAccount" runat="server" SelectMethod="GetCFOP"
        TypeName="Vivina.Erp.BusinessRules.AccountManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAccounts" runat="server" SelectMethod="GetAccountsFormatted"
        TypeName="Vivina.Erp.BusinessRules.AccountManager" onselecting="dataSource_Selecting">
        <selectparameters>
		<asp:parameter Name="companyId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsInvoiceModels" runat="server" SelectMethod="GetDocumentTemplates"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager" onselecting="odsInvoiceModels_Selecting">
        <selectparameters>
    		<asp:parameter Name="companyId" Type="Int32" />
            <asp:parameter Name="documentTemplateTypeId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
    <script>

        $(function () {
            $("input:radio").attr("name", "boleto").click(function () {
                $("#parcelId").attr("value", $(this).attr("value"));
            });
        });
    
    </script>
</asp:Content>
