<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Roles" Title="Perfil" CodeBehind="Roles.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21">
        <tr class="top">
            <td class="left">
            </td>
            <td class="center">
            </td>
            <td class="right">
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
            </td>
            <td class="center">
                <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label><br />
                <asp:GridView ID="grdRoles" runat="server" DataSourceID="odsRoles" AutoGenerateColumns="False"
                    CssClass="cGrd11" OnRowDataBound="grdRoles_RowDataBound" OnSelectedIndexChanging="grdRoles_SelectedIndexChanging"
                    DataKeyNames="RoleId,ParentRoleId,ApplicationId,LastUpdatedDate,Name,Description,CompanyId"
                    OnSorting="grdRoles_Sorting" AllowSorting="True" AllowPaging="True" Width="400"
                    _permissionRequired="Roles">
                    <RowStyle CssClass="Items" />
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Descricao" SortExpression="Description">
                        </asp:BoundField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;  title=&quot;excluir&quot; &gt;&lt;/div&gt;"
                            HeaderText="&lt;a href=&quot;Role.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot; &gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <AlternatingRowStyle CssClass="AlternateItems" />
                </asp:GridView>
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
    
    <VFX:BusinessManagerDataSource ID="odsRoles" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Role"
        DeleteMethod="Delete" SelectMethod="GetRolesByCompanyAsList" TypeName="Vivina.Erp.BusinessRules.RolesManager"
        UpdateMethod="Update" OnSelecting="odsRoles_Selecting" ConflictDetection="CompareAllValues"
        OldValuesParameterFormatString="original_{0}" SortParameterName="sortExpression"
        ondeleted="odsRoles_Deleted1">
        <updateparameters>
                <asp:parameter Name="original_entity" Type="Object" />
                <asp:parameter Name="entity" Type="Object" />
            </updateparameters>
        <selectparameters>
                <asp:parameter Name="companyId" Type="Int32" />
                <asp:parameter Name="sortExpression" Type="String" />
                <asp:parameter Name="startRowIndex" Type="Int32" />
                <asp:parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
