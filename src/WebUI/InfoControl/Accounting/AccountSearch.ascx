<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountSearch.ascx.cs"
    Inherits="InfoControl.Web.UI.AccountSearch" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc3" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<table width="80%">
    <tr>
        <td colspan="2">
            Escolhas quais parcelas deseja visualizar:
            <asp:RadioButtonList ID="rdbListStatus" ForeColor="" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1"><font color="red"> Vencidos</font></asp:ListItem>
                <asp:ListItem Value="2"><font color="tomato"> Vencendo este mês</font></asp:ListItem>
                <asp:ListItem Value="3"><font color="red"> Em aberto</font></asp:ListItem>
                <asp:ListItem Value="4"><font color="green"> Quitados</font></asp:ListItem>
                <asp:ListItem Value=""> Todos</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            Centro de custos: &nbsp;<VFX:HelpTooltip ID="HelpTooltip7" runat="server" visible="False">
                <itemtemplate>
                                    <h2>
                                        Ajuda:</h2>
                                      Encontre as contas que estão relacionadas a um determinado centro de custo.
                                </itemtemplate>
            </VFX:HelpTooltip><br />
            <uc3:ComboTreeBox ID="cboCostCenter" runat="server" DataSourceID="odsCostCenter"
                DataFieldID="CostCenterId" DataFieldParentID="ParentId" DataTextField="Name"
                DataValueField="CostCenterId" />
        </td>
        <td>
            Plano de contas: &nbsp;<VFX:HelpTooltip ID="HelpTooltip8" runat="server" visible="False">
                <itemtemplate>
                                    <h2>
                                        Ajuda:</h2>
                                      Encontre as contas que estão relacionadas a um determinado plano de contas.
                                </itemtemplate>
            </VFX:HelpTooltip>
            <br />
            <uc3:ComboTreeBox ID="cboAccountPlan" runat="server" DataSourceID="odsAccountingPlan"
                DataFieldID="AccountingPlanId" DataFieldParentID="ParentId" DataTextField="Name"
                DataValueField="AccountingPlanId" />
        </td>
    </tr>
    <tr>
        <td>
            <uc1:DateTimeInterval ID="ucDateInterval" ValidationGroup="searchInvoice" Required="true"
                runat="server" />
        </td>
        <td>
            Fornecedor/Cliente:&nbsp;<VFX:HelpTooltip ID="HelpTooltip5" runat="server">
                <itemtemplate>
                                    <h2>
                                        Ajuda:</h2>
                                     Encontre parcelas de contas que estão relacionadas a um determinado cliente ou fornecedor 
                                     digitando apenas parte do nome dos mesmos.
                                     
                                </itemtemplate>
            </VFX:HelpTooltip><br />
            <asp:TextBox runat="server" ID="txtName" Columns="32"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            Valor da Parcela: &nbsp;<VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                <itemtemplate>
                                    <h2>
                                        Ajuda:</h2>
                                     Encontre parcelas de contas por deteminado valor.
                                </itemtemplate>
            </VFX:HelpTooltip>
            <br>
            <uc2:CurrencyField ID="ucCurrFieldParcelValue" runat="server" />
        </td>
        <td>
            Conta Bancária:&nbsp;
            <br>
            <asp:DropDownList ID="cboAccount" AppendDataBoundItems="true" runat="server" DataValueField="AccountId"
                DataTextField="ShortName" DataSourceID="odsAccountType">
                <asp:ListItem Value="" Text=""></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            Identificação: &nbsp;<VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                <itemtemplate>
                                    <h2>
                                        Ajuda:</h2>
                                     Encontre parcelas de contas por sua informação de identificação.
                                </itemtemplate>
            </VFX:HelpTooltip>
            <br>
            <asp:TextBox ID="txtIdentification" runat="server" Width="50"></asp:TextBox>
        </td>
    </tr>
</table>
<table width="100%">
    <tr>
        <td align="right">
            <asp:Button ID="btnSearchInvoice" runat="server" Text="Pesquisar" ValidationGroup="searchInvoice"
                OnClick="btnSearchInvoice_Click" EnableViewState="False" />
        </td>
    </tr>
</table>
<VFX:BusinessManagerDataSource ID="odsCostCenter" runat="server" OnSelecting="dataSource_Selecting"
    SelectMethod="GetCostsCenterAsDataTable" TypeName="Vivina.Erp.BusinessRules.AccountManager">
    <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
    </selectparameters>
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsAccountingPlan" runat="server" OnSelecting="dataSource_Selecting"
    SelectMethod="GetOutboundPlan" TypeName="Vivina.Erp.BusinessRules.AccountManager">
    <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
    </selectparameters>
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsAccountType" runat="server" SelectMethod="GetAccountsWithShortName"
    TypeName="Vivina.Erp.BusinessRules.AccountManager" OnSelecting="odsAccountType_Selecting">
    <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
</VFX:BusinessManagerDataSource>
