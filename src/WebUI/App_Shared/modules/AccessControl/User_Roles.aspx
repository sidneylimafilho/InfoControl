<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" Inherits="User_Roles"
    MasterPageFile="~/infocontrol/Default.master" Codebehind="User_Roles.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
<table class="cLeafBox21" width="50%">
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:DropDownList ID="cboRoles" runat="server" DataSourceID="odsRoles" DataTextField="Name"
                            DataValueField="RoleId">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:Button ID="Button1" runat="server" ValidationGroup="AddRoles" Text="Adicionar"
                            OnClick="Button1_Click" />
                    </asp:Panel>
                    <br />
                    <asp:GridView ID="grdRolesByUser" runat="server" DataSourceID="odsUserInRoles" AutoGenerateColumns="False"
                        OnRowDataBound="grdRolesByUser_RowDataBound" OnRowCommand="grdRolesByUser_RowCommand"
                        Width="50%" DataKeyNames="RoleId,CompanyId,UserId" OnRowDeleting="grdRolesByUser_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Perfil">
                                <ItemTemplate>
                                    <%#Eval("Name")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField DeleteText="&lt;span alt=&quot;Apagar&quot; class=&quot;delete&quot; border=0&gt;&lt;/span&gt;"
                                ShowDeleteButton="True">
                                <ItemStyle Width="1%" />
                            </asp:CommandField>
                        </Columns>
                        <EmptyDataTemplate>
                            Não há dados a serem exibidos
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <VFX:BusinessManagerDataSource ID="odsUserInRoles" runat="server" TypeName="Vivina.Erp.BusinessRules.RolesManager"
                OldValuesParameterFormatString="original_{0}" OnSelecting="odsUserInRoles_Selecting"
                SelectMethod="GetUserInRoles" ConflictDetection="CompareAllValues" OnDeleting="odsUserInRoles_Deleting"
                DataObjectTypeName="Vivina.Erp.DataClasses.UsersInRole" DeleteMethod="DeleteUserInRoles"
                OnDeleted="odsUserInRoles_Deleted">
                <SelectParameters>
                    <asp:Parameter Name="UserId" Type="Int32" />
                    <asp:Parameter Name="companyId" Type="Int32" />
                </SelectParameters>
            </VFX:BusinessManagerDataSource>
            <VFX:BusinessManagerDataSource ID="odsRoles" runat="server" SelectMethod="GetRemainingRolesByUser"
                TypeName="Vivina.Erp.BusinessRules.RolesManager" OnSelecting="odsRoles_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="userId" Type="Int32" />
                    <asp:Parameter Name="companyId" Type="Int32" />
                </SelectParameters>
            </VFX:BusinessManagerDataSource>
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
</asp:Content>