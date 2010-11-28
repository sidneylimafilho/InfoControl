<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Company_RH_Formation_Service"
    Title="Untitled Page" Codebehind="Service.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        <asp:Label ID="lblTitle" runat="server" Text="Label" Font-Size="11pt"></asp:Label></h1>
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
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
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                    ValidationGroup="Salvar" />
                <br />
                <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" EditIndex="0"
                    HorizontalAlign="Center" OnRowDeleting="grid_RowDeleting" OnRowEditing="grid_RowEditing"
                    OnRowUpdating="grid_RowUpdating" Width="100%" OnRowCancelingEdit="grid_RowCancelingEdit"
                    GridLines="None" OnRowDataBound="grid_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome">
                            <ItemStyle Wrap="True" />
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNome" CssClass="cDat11" runat="server" Width="96%" MaxLength="100"
                                    Text='<%# Bind("Name") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNome"
                                    ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Salvar">*</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Setor">
                            <ItemTemplate>
                                <asp:DropDownList ID="cboSector" runat="server" DataSourceID="BusinessManagerDataSource1"
                                    SelectedValue='<%# Bind("SectorId") %>' DataTextField="Name" DataValueField="SectorId"
                                    AppendDataBoundItems="true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboSector"
                                     ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Salvar">&nbsp;</asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            ShowDeleteButton="True">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                        <asp:CommandField CancelText="&lt;img src='../../App_Shared/modules/Glass/Controls/GridView/img/Cancel.gif' border='0' /&gt;"
                            EditText="Editar" ShowEditButton="True" UpdateText="&lt;img src='../../App_Shared/modules/Glass/Controls/GridView/img/Save.gif' border='0' /&gt;"
                            ValidationGroup="Salvar">
                            <ItemStyle Width="1%" Wrap="True" HorizontalAlign="Center" />
                        </asp:CommandField>
                    </Columns>
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
    <VFX:BusinessManagerDataSource ID="BusinessManagerDataSource1" runat="server" onselecting="BusinessManagerDataSource1_Selecting"
        SelectMethod="GetAllSectors" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
