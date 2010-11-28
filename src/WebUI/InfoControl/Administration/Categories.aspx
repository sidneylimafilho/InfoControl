<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="True" Title="Categorias" CodeBehind="Categories.aspx.cs" Inherits="Company_Categories" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
                            <telerik:RadTreeView ID="treCategory" DataSourceID="odsCategory" runat="server" DataFieldID="CategoryId" DataFieldParentID="ParentId"
                                DataTextField="Name" DataValueField="CategoryId" Height="400px" Width="100%"
                                AfterClientClick="AfterClickHandler" AutoPostBack="True">
                                <CollapseAnimation Type="OutQuint" Duration="100" />
                                <ExpandAnimation Duration="100" />
                                <NodeTemplate>
                                    <table>
                                        <tr>
                                            <td style="padding-right: 4px;">
                                                <asp:LinkButton runat="server" ID="lnkDelete" CommandArgument='<%# Eval("CategoryId") %>'
                                                    CommandName="delete" CssClass="delete" OnCommand="lnkDelete_Command">
                                                </asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkName" CommandArgument='<%# Eval("CategoryId") %>'
                                                    CommandName="select" OnCommand="lnkDelete_Command" Text='<%# Eval("Name") %>'>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </NodeTemplate>
                            </telerik:RadTreeView>
                        </td>
                        <td style="padding-left: 10px;" valign="top">
                            Selecione uma categoria para alterar ou digite uma nova categoria para inserir:<br />
                            <asp:TextBox ID="txtCategory" runat="server" Text="" MaxLength="50" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtCategory" ValidationGroup="Add" runat="server"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" ControlToValidate="txtCategory" Text="&nbsp;"></asp:RequiredFieldValidator>
                            <br />
                            Selecione uma categoria pai:<br />
                            <uc2:ComboTreeBox ID="cboTreeCategory" runat="server" SelectedValue="" DataFieldID="CategoryId" DataFieldParentID="ParentId"
                                DataTextField="name"  DataValueField="CategoryId" DataSourceID="odsCategory" />
                            <asp:Button ID="btnAdd" runat="server" Text="Salvar" OnClick="btnAdd_Click" ValidationGroup="Add"
                                _permissionRequired="Categories" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancelSelection" ValidationGroup="Add"
                                runat="server" Text="Cancelar" _permissionRequired="Categories" OnClick="btnCancelSelection_Click" />
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
    <uc1:ToolTip ID="ToolTip1" runat="server" Indication="left" Left="100px" Message=" Monte aqui a cadeia de categorias e suas sub-categorias relevantes. Esse procedimento facilita a distinção dos produtos e segmentos da empresa. E, enquanto existir um produto associado à uma categoria, aquela categoria não poderá ser excluída.!"
        Title="Atenção:" Top="50px" />
    <VFX:BusinessManagerDataSource ID="odsCategory" runat="server" SelectMethod="GetCategoriesByCompanyAsDataTable"
        TypeName="Vivina.Erp.BusinessRules.CategoryManager" onselecting="odsCategory_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
