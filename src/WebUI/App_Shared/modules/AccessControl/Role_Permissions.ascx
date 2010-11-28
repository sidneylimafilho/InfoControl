<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AccessControl_Role_Permissions" Codebehind="Role_Permissions.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<table class="cLeafBox21">
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
        <td class="center" align="right">
            <asp:UpdatePanel ID="upFuncations" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdFunctions" runat="server" DataSourceID="odsFunctions" Width="100%"
                        AutoGenerateColumns="False" RowSelectable="false" DataKeyNames="FunctionId" OnRowDataBound="grdFunctions_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Função" SortExpression="Name"></asp:BoundField>
                            <asp:TemplateField HeaderText="Ver">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRead" runat="server" Checked='<%# Convert.ToInt32(Eval("PermissionTypeId")) == 1 %>' />
                                </ItemTemplate>
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Alterar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWrite" runat="server" Checked='<%# Convert.ToInt32(Eval("PermissionTypeId")) == 2 %>' />
                                </ItemTemplate>
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <div style="text-align: right">
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                <asp:Button ID="btnVoltar" runat="server" Text="Cancelar" OnClick="btnVoltar_Click" /></div>
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
<VFX:BusinessManagerDataSource ID="odsFunctions" runat="server" SelectMethod="GetAllPermissionsByPlan"
    TypeName="Vivina.Erp.BusinessRules.PermissionManager" ConflictDetection="CompareAllValues"
    OldValuesParameterFormatString="original_{0}" OnSelecting="odsFunctions_Selecting">
    <SelectParameters>
        <asp:Parameter Name="companyId" Type="Int32" />
        <asp:Parameter Name="roleId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
