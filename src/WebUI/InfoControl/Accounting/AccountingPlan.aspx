<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_Accounting_AccountingPlan" Title="Plano de contas" CodeBehind="AccountingPlan.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
    <%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc1" %>
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
                <!-- This table contains all the controls of the page -->
                <table width="100%">
                    <tr>
                        <td valign="top">
                            <%--<asp:Panel runat="server" ID="panel">
                                <div id="treRemove" runat="server" style="padding: 6px; text-decoration: underline;
                                    cursor: pointer; text-align: center;">
                                    <b>Remover seleção!<br />
                                    </b>
                                </div>
                                <br />--%>
                                <radT:RadTreeView ID="treAccountingPlan" runat="server" DataFieldID="AccountingPlanId"
                                    DataFieldParentID="ParentId" DataTextField="Name" DataValueField="AccountingPlanId"
                                    Width="100%" AutoPostBack="True" OnNodeBound="treAccountingPlan_NodeBound">
                                    <NodeTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="padding-right: 10px">
                                                    <asp:LinkButton ID="LinkButton1" CssClass="delete" runat="server" CommandArgument='<%# Eval("AccountingPlanId") %>'
                                                        CommandName="Delete" OnCommand="btnDeleteAccoutingPlan_Command" OnClientClick="return confirm('Deseja realmente excluir este plano de contas?')"  />&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("AccountingPlanId") %>'
                                                        CommandName="Select" OnCommand="btnDeleteAccoutingPlan_Command" Text='<%# Eval("Name") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </NodeTemplate>
                                </radT:RadTreeView>
                            </asp:Panel>
                        </td>
                        <td style="padding-left: 5px;" valign="top">
                            <fieldset>
                                <legend>Cadastro de Plano de contas</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Selecione um plano de contas para alterar ou digite um novo plano de contas para inserir:
                                            <br />
                                            <asp:TextBox ID="txtName" runat="server" Columns="80" MaxLength="50" Width="180px"></asp:TextBox>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="cErr21"
                                                ControlToValidate="txtName" ValidationGroup="ValidateAccountingPlan" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        Selecione um plano de contas pai:&nbsp;<VFX:HelpTooltip runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                        Para encadear um plano de contas, ou seja para colocar um plano de contas dentro de outro, basta selecionar
                                        um plano de contas "pai" desejado.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                                            <uc1:ComboTreeBox ID="cboTreeAccountingPlan" DataFieldID="AccountingPlanId" DataFieldParentID="ParentId"
                                                DataTextField="Name" DataValueField="AccountingPlanId" DataSourceID="odsTreeAccountingPlan"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Button ID="btnAddAccountingPlan" runat="server" Text="Salvar" ValidationGroup="ValidateAccountingPlan"
                                                OnClick="btnAddAccountingPlan_Click" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnCancelSelection" runat="server" Text="Cancelar" ValidationGroup="ValidateAccountingPlan"
                                                OnClick="btnCancelSelection_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <br />
                            <br />
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
    <VFX:BusinessManagerDataSource ID="odsTreeAccountingPlan" runat="server" OldValuesParameterFormatString="original_{0}"
        onselecting="odsCostCenters_Selecting" SelectMethod="GetAccountsPlanAsDataTable" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
