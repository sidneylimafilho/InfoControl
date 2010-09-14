<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Role_General" Codebehind="Role_General.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<table class="cLeafBox21" width="50%">
    <tr class="top">
        <td class="left">
            &nbsp;</td>
        <td class="center">
            &nbsp;</td>
        <td class="right">
            &nbsp;</td>
    </tr>
    <tr class="middle">
        <td class="left">
            &nbsp;</td>
        <td class="center">
            <asp:FormView ID="frmRole" runat="server" DataSourceID="odsRoles" DefaultMode="Insert"
                OnItemCommand="frmRole_ItemCommand" DataKeyNames="RoleId,CompanyId,ApplicationId,LastUpdatedDate,ParentRoleId"
                Width="100%">
                <EditItemTemplate>
                    Nome:<br />
                    <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' Columns="30" />
                    <br />
                    Descrição:<br />
                    <asp:TextBox ID="DescriptionTextBox" TextMode="MultiLine" Rows="8" runat="server" Text='<%# Bind("Description") %>'
                        Columns="60" />
                    <br />
                    <br />
                    <div style="text-align: right">
                        <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                            CssClass="cBtn11" Text="Inserir" Visible="<%# frmRole.CurrentMode == FormViewMode.Insert %>"
                            permissionRequired="Roles"></asp:Button>
                        <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                            CssClass="cBtn11" Text="Salvar" Visible="<%# frmRole.CurrentMode == FormViewMode.Edit %>"
                            permissionRequired="Roles"></asp:Button>
                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            CssClass="cBtn11" Text="Cancelar"></asp:Button>
                    </div>
                </EditItemTemplate>
            </asp:FormView>
        </td>
        <td class="right">
            &nbsp;</td>
    </tr>
    <tr class="bottom">
        <td class="left">
            &nbsp;</td>
        <td class="center">
            &nbsp;</td>
        <td class="right">
            &nbsp;</td>
    </tr>
</table>
<VFX:BusinessManagerDataSource ID="odsRoles" runat="server" SelectMethod="GetRoles"
    TypeName="Vivina.Erp.BusinessRules.RolesManager" OnSelecting="odsRoles_Selecting"
    ConflictDetection="CompareAllValues" DataObjectTypeName="Vivina.Erp.DataClasses.Role"
    InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" OnInserting="odsRoles_Inserting"
    UpdateMethod="Update" OnInserted="odsRoles_Inserted" OnUpdated="odsRoles_Updated"
    OnUpdating="odsRoles_Updating">
    <UpdateParameters>
        <asp:Parameter Name="original_entity" Type="Object" />
        <asp:Parameter Name="entity" Type="Object" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="RoleId" Type="Int32" />
        <asp:Parameter Name="CompanyId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
