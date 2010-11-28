<%@ Control Language="C#" AutoEventWireup="true" Inherits="ReportGenerator_GroupBy" Codebehind="ReportGenerator_GroupBy.ascx.cs" %>
<table cellspacing="0" cellpadding="5" width="100%" border="0" class="cTxt11">
    <tr>
        <td align="left" colspan="2">
            ORDENAÇÃO DO RELATÓRIO
        </td>
    </tr>
    <tr>
        <td class="cTxt11b" align="left" valign="top" style="width: 1%">
            5
        </td>
        <td class="cTxt11b" align="left" valign="top">
            Faça as configurações de ordem em que os dados serão apresentados.</td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView ID="grid" Width="1%" GridLines="None" runat="server" HorizontalAlign="Center"
                AutoGenerateColumns="False" EditIndex="0" OnRowDataBound="grid_RowDataBound"
                OnRowDeleting="grid_RowDeleting" OnRowUpdating="grid_RowUpdating" OnRowEditing="grid_RowEditing"
                OnRowCancelingEdit="grid_RowCancelingEdit">
                <Columns>
                    <asp:TemplateField HeaderText="Variavel" ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <asp:DropDownList ID="cboColumn" runat="server" DataTextField="Name" DataValueField="ReportColumnsSchemaId">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboColumn"
                                ErrorMessage="Coluna obrigatória" ValidationGroup="Sort">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ordena&#231;&#227;o">
                        <EditItemTemplate>
                            <asp:DropDownList ID="cboOrder" runat="server">
                                <asp:ListItem Text="Ascendente" Value="true" />
                                <asp:ListItem Text="Descendente" Value="false" />
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# (bool)Eval("Ascending")?"Ascendente":"Descendente" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ValidationGroup="Sort">
                        <ItemStyle Width="1%" />
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>