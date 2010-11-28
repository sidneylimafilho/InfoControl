<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Title="Niveís Organizacionais" Inherits="Company_Categories_OrganizationLevel"
    CodeBehind="OrganizationLevel.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc2" %>
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
                <table width="100%" cellpadding="10" cellspacing="0">
                    <tr>
                        <td width="1%" valign="top" style="padding-right: 10px; border-right: 1px solid #00AAAA">
                            <radT:RadTreeView ID="treeOL" runat="server" DataFieldID="OrganizationLevelId" AutoPostBack="true"
                                DataFieldParentID="ParentId" DataTextField="Name" DataValueField="OrganizationLevelId"
                                Height="400px" Width="100%">
                                <NodeTemplate>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 4px">
                                                <asp:LinkButton runat="server" ID="lnkDelete" CommandArgument='<%# Eval("OrganizationLevelId") %>'
                                                    CommandName="delete" CssClass="delete" OnCommand="lnkDelete_Command">
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkName" CommandArgument='<%# Eval("OrganizationLevelId") %>'
                                                    CommandName="select" OnCommand="lnkDelete_Command" Text='<%# Eval("name") %>'>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </NodeTemplate>
                            </radT:RadTreeView>
                        </td>
                        <td style="padding-left: 10px;" valign="top">
                            Selecione um nível para alterar ou digite um novo nível para inserir:<br />
                            <asp:TextBox ID="txtOL" MaxLength="50" runat="server" Text="" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="reqtxtOL" runat="server"
                                ControlToValidate="txtOL" ValidationGroup="save"></asp:RequiredFieldValidator>
                            <br />
                            Selecione um nível pai:<br />
                            <uc2:ComboTreeBox ID="cboTreeOL" runat="server" DataSourceID="odsTreeOrganizationLevel"
                                DataFieldID="OrganizationLevelId" DataFieldParentID="ParentId" DataTextField="Name"
                                DataValueField="OrganizationLevelId" />
                            
                            <br />
                            <asp:Button ID="btnAdd" runat="server" Text="Salvar" ValidationGroup="save" OnClick="btnAdd_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnCancelSelection" Text="Cancelar" ValidationGroup="save"
                                OnClick="btnCancelSelection_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsTreeOrganizationLevel" runat="server" onselecting="odsTreeOrganizationLevel_Selecting"
        SelectMethod="GetAllOrganizationLevelToDataTable" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
