<%@ Control Language="C#" AutoEventWireup="true" Inherits="ReportGenerator_Sort"
    CodeBehind="ReportGenerator_Sort.ascx.cs" %>
<table cellspacing="0" cellpadding="5" width="100%" border="0">
    <%--<tr>
        <td align="left" colspan="2">
            <h2>
                ORDENAÇÃO DO RELATÓRIO</h2>
        </td>
    </tr>--%>
    <tr>
        <td class="cTxt11b" align="left" valign="middle" style="padding-right: 10px; width: 1%">
            <h1>
                5</h1>
        </td>
        <td align="left" valign="middle">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, OrderConfigurationDataReportDescription %>" />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="cboColumn" runat="server" DataTextField="Name" DataValueField="ReportColumnsSchemaId">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboColumn"
                            ErrorMessage="<%$ Resources: Resource, ColumnRequiredMessage %>" ValidationGroup="Sort">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="cboOrder" runat="server">
                            <asp:ListItem Text="<%$ Resources: Resource, AscOrder %>" Value="true" />
                            <asp:ListItem Text="<%$ Resources: Resource, DescOrder %>" Value="false" />
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnAddSortColumn" ImageUrl="~/App_Themes/_global/Company/Reports/funnel_add.gif"
                            runat="server" OnClick="btnAddSortColumn_Click" ValidationGroup="Sort" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="grid" Width="300px" GridLines="None" runat="server" HorizontalAlign="Center"
                            AutoGenerateColumns="False" EditIndex="-1" OnRowDeleting="grid_RowDeleting" OnRowCancelingEdit="grid_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources: Resource, Variable %>" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Resource, Ordination %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# (bool)Eval("Ascending")?"Ascendente":"Descendente" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="Delete" ImageUrl="~/App_Themes/_global/Company/Reports/funnel_delete.gif"
                                    ButtonType="Image" Visible="true"></asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
