<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Aplications" Title="Cadastro de Aplicações" Codebehind="Applications.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder" runat="Server">
    
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
                    <asp:FormView ID="frmApplication" runat="server" Width="100%" DataKeyNames="ApplicationId"
                        DataSourceID="BusinessManagerDataSource1" DefaultMode="Insert" OnItemInserted="frmApplication_ItemInserted"
                        OnItemUpdated="frmApplication_ItemUpdated" OnItemCommand="frmApplication_ItemCommand">
                        <EditItemTemplate>
                            Nome da Aplicação:<br />
                            <asp:TextBox ID="NameTextBox" runat="server" CssClass="cDat11" Text='<%# Bind("Name") %>'
                                Width="300px" MaxLength="50"></asp:TextBox><br />
                            <asp:CheckBox ID="IsActiveCheckBox" runat="server" Checked='<%# Bind("IsActive") %>'
                                CssClass="cChk11" Text="Ativo?" /><br />
                            <asp:CheckBox ID="IsMaintenanceCheckBox" runat="server" Checked='<%# Bind("IsMaintenance")%>'
                                CssClass="cChk11" Text="Em manutenção?" />
                            <br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        Mensagem de Manutenção:
                                    </td>
                                    <td>
                                        Descrição:
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:TextBox ID="MaintenanceMessageTextBox" runat="server" CssClass="cDat11" Text='<%# Bind("MaintenanceMessage") %>'
                                            Width="300px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="cDat11" Height="100px"
                                            Text='<%# Bind("Description") %>' Width="300px" MaxLength="1024" TextMode="MultiLine"></asp:TextBox><br />
                                        <asp:RegularExpressionValidator CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RegularExpressionValidator1"
                                            runat="server" ControlToValidate="DescriptionTextBox" Text="Campo para apenas 1024 caracteres!"
                                            ValidationExpression=".{0,1024}" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: right">
                                <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                    Text="Inserir" Visible="<%# frmApplication.CurrentMode == FormViewMode.Insert %>">
                                </asp:Button>
                                <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Salvar" Visible="<%# frmApplication.CurrentMode == FormViewMode.Edit %>">
                                </asp:Button>
                                <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancelar"></asp:Button>
                            </div>
                        </EditItemTemplate>
                    </asp:FormView>
                    <asp:GridView ID="grdApplications" runat="server" AutoGenerateColumns="False" CssClass="cGrd11"
                        OnRowDataBound="grdApplications_RowDataBound" DataKeyNames="ApplicationId,Description,MaintenanceMessage"
                        AllowSorting="True" OnSorting="grdApplications_Sorting" 
                        Width="80%" DataSourceID="BusinessManagerDataSource1" 
                        onselectedindexchanging="grdApplications_SelectedIndexChanging">
                        <RowStyle CssClass="Items" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                            <asp:TemplateField HeaderText="Manutenção" SortExpression="IsMaintenance">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToBoolean(Eval("IsMaintenance"))?"Sim":"Não" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ativo" SortExpression="IsActive">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))?"Sim":"Não" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                                ShowDeleteButton="True" SortExpression="Insert">
                                <ItemStyle Width="20px" />
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
        <VFX:BusinessManagerDataSource ID="BusinessManagerDataSource1" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Application"
            DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetAllApplications"
            TypeName="Vivina.Erp.BusinessRules.ApplicationManager" UpdateMethod="Update"
            OnDeleted="BusinessManagerDataSource1_Deleted" ConflictDetection="CompareAllValues"
            OldValuesParameterFormatString="original_{0}">
            <UpdateParameters>
                <asp:Parameter Type="Object" Name="original_entity"></asp:Parameter>
                <asp:Parameter Type="Object" Name="entity"></asp:Parameter>
            </UpdateParameters>
        </VFX:BusinessManagerDataSource>
        <br />
        &nbsp;
    </div>
    </asp:content>
