<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Host_Packages" Title="Pacotes" CodeBehind="Packages.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
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
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <%-- <asp:
                                <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                    <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdPackages" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    DataSourceID="odsPackages" DataKeyNames="PackageId,ModifiedDate,NumberUsers,IsActive,NumberItems,Price"
                                    AllowSorting="True" OnRowDataBound="grdPackages_RowDataBound" Width="100%" permissionRequired="Packages"
                                    RowSelectable="false" EnableViewState="False" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RequiredFieldValidator1"
                                                    runat="server" ControlToValidate="txtName" CssClass="cErr21">*</asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Usuários" SortExpression="NumberUsers">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtUsers" runat="server" Text='<%# Bind("NumberUsers") %>'></asp:TextBox>
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                                    ControlToValidate="txtUsers" CssClass="cErr21" MaximumValue="100000000" MinimumValue="0"
                                                    Type="Integer">*</asp:RangeValidator>
                                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" Maximum="100000"
                                                    Minimum="0" TargetControlID="txtUsers" Width="80">
                                                </ajaxToolkit:NumericUpDownExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("NumberUsers") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Items" SortExpression="NumberItems">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtItems" runat="server" Text='<%# Bind("NumberItems") %>'></asp:TextBox>
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtItems"
                                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" MaximumValue="100000000"
                                                    MinimumValue="0" Type="Integer">*</asp:RangeValidator>
                                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender2" runat="server" Maximum="100000"
                                                    Minimum="0" TargetControlID="txtItems" Width="80">
                                                </ajaxToolkit:NumericUpDownExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("NumberItems") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor" SortExpression="Price">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("Price", "{0:#,###,##0.00}") %>'></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" InputDirection="RightToLeft"
                                                    Mask="9,999,999.99" MaskType="Number" TargetControlID="txtPrice">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Price", "{0:#,###,##0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="&lt;a href=&quot;Package_General.aspx&quot;&gt; &lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt; &lt;/a&gt;"
                                            DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                            ShowDeleteButton="True">
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div align="center">
                                            Não existem dados a serem exibidos, clique no botão para cadastrar um pacote.<br />
                                            &nbsp;<asp:Button ID="btnTransfer" runat="server" OnClientClick="location='Package_General.aspx'; return false;"
                                                Text="Cadastrar" />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
    </div>
    <VFX:BusinessManagerDataSource ID="odsPackages" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Package"
        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetAllPackagesList"
        TypeName="Vivina.Erp.BusinessRules.PackagesManager" UpdateMethod="Update"
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" SortParameterName="sortExpression"
        ConflictDetection="CompareAllValues" ondeleted="odsPackages_Deleted">
        <updateparameters>
            <asp:Parameter Name="original_entity" Type="Object" />
            <asp:Parameter Name="entity" Type="Object" />
        </updateparameters>
        <selectparameters>
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
