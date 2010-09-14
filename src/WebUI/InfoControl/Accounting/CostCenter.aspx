<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Accounting_CostCenter" Title="Centro de custo"
    CodeBehind="CostCenter.aspx.cs" %>

<%@ Register TagPrefix="radT" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
    
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
                <!-- conteudo -->
                <table width="100%" cellpadding="10" cellspacing="0">
                    <tr>
                        <td width="1%" valign="top" style="padding-right: 10px; border-right: 1px solid #00AAAA">
                            <radT:RadTreeView ID="treCostCenter" runat="server" DataFieldID="CostCenterId" DataFieldParentID="ParentId"
                                AutoPostBack="True" DataTextField="Name" DataValueField="CostCenterId" Width="100%"
                                OnNodeBound="treCostCenter_NodeBound">                                
                                <NodeTemplate>
                                    <table>
                                        <tr>
                                            <td style="padding-right: 4px">
                                                <asp:LinkButton ID="LinkButton1" CssClass="delete" runat="server" CommandArgument='<%# Eval("CostCenterId") %>'
                                                    CommandName="Delete" OnCommand="btnDeleteCostCenter_Command" OnClientClick="return confirm('Deseja realmente excluir este centro de custos?')"  />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("CostCenterId") %>'
                                                    CommandName="Select" OnCommand="btnDeleteCostCenter_Command" Text='&nbsp;&nbsp;&nbsp;<%# Eval("Name") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </NodeTemplate>
                            </radT:RadTreeView>
                        </td>
                        <td style="padding-left: 10px;" valign="top">
                            Selecione um centro de custo para alterar ou digite um novo centro de custo para
                            inserir:<br />
                            <asp:TextBox ID="txtName" runat="server" Columns="80" MaxLength="50" Width="180px"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;
                            <asp:RequiredFieldValidator ID="reqtxtName" runat="server" CssClass="cErr21" ControlToValidate="txtName"
                                ValidationGroup="save" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                            <br />
                            Selecione um centro de custo pai:&nbsp;<VFX:HelpTooltip runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                                        Para encadear um centro de custo, ou seja para colocar um centro de custo dentro de outro, basta selecionar
                                        um centro de custo "pai" desejado.
                                </ItemTemplate>
                            </VFX:HelpTooltip>
                            <br />
                            <uc1:ComboTreeBox ID="cboTreeCostCenters" DataFieldID="CostCenterId" DataFieldParentID="ParentId"
                                DataTextField="Name" DataValueField="CostCenterId" DataSourceID="odsTreeCostCenters"
                                runat="server" />
                            <br />
                            <asp:Button ID="btnAddCostCenter" runat="server" Text="Salvar" ValidationGroup="save"
                                OnClick="btnAddCostCenter_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" ValidationGroup="save"
                                OnClick="btnCancel_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
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
    <VFX:BusinessManagerDataSource ID="odsTreeCostCenters" runat="server" OldValuesParameterFormatString="original_{0}"
        onselecting="odsCostCenters_Selecting" SelectMethod="GetCostsCenterAsDataTable"
        TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
