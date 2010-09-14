<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="SystemParametersList"
    Title="Parâmetros do Sistema" Codebehind="SystemParameters.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <td class="right" style="width: 1px">
                    &nbsp;
                </td>
            </tr>
            <tr class="middle">
                <td class="left">
                    &nbsp;
                </td>
                <td class="center">
                    <asp:FormView ID="frmSystemParameter" runat="server" DataSourceID="odsSystemParameters"
                        DefaultMode="Insert" Width="100%" DataKeyNames="SystemParameterId,Name,Value,Description,InitialValue,ApplicationId"
                        OnItemCommand="frmSystemParameter_ItemCommand" OnItemInserted="frmSystemParameter_ItemInserted"
                        OnItemUpdated="frmSystemParameter_ItemUpdated">
                        <EditItemTemplate>
                            Nome:<br />
                            <asp:TextBox ID="NameTextBox" runat="server" CssClass="cDat11" Text='<%# Bind("Name") %>'
                                Width="300px"></asp:TextBox><br />
                            Valor Inicial:<br />
                            <asp:TextBox ID="InitialValueTextBox" runat="server" CssClass="cDat11" Text='<%# Bind("InitialValue") %>'
                                Width="300px"></asp:TextBox><br />
                            Valor:<br />
                            <asp:TextBox ID="ValueTextBox" runat="server" CssClass="cDat11" Text='<%# Bind("Value") %>'
                                Width="300px"></asp:TextBox><br />
                            Aplicação:<br />
                            <asp:DropDownList ID="cboApplication" runat="server" DataSourceID="odsApplication"
                                DataTextField="Name" DataValueField="ApplicationId" SelectedValue='<%# Bind("ApplicationId") %>'>
                            </asp:DropDownList>
                            <br />
                            Descrição:<br />
                            <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="cDat11" Height="100px"
                                Text='<%# Bind("Description") %>' Width="300px" TextMode="MultiLine"></asp:TextBox><br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="DescriptionTextBox"
                                Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;" Text="Campo para apenas 1024 caracteres!"
                                ValidationExpression=".{0,1024}"></asp:RegularExpressionValidator><br />
                            <br />
                            <div style="text-align: right">
                                <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                    CssClass="cBtn11" Text="Inserir" Visible="<%# frmSystemParameter.CurrentMode == FormViewMode.Insert %>"
                                    permissionRequired="SystemParameters"></asp:Button>
                                <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                    CssClass="cBtn11" Text="Salvar" Visible="<%# frmSystemParameter.CurrentMode == FormViewMode.Edit %>"
                                    permissionRequired="SystemParameters"></asp:Button>
                                <asp:Button ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                    CssClass="cBtn11" Text="Cancelar"></asp:Button></div>
                        </EditItemTemplate>
                    </asp:FormView>
                    <asp:GridView ID="grdSystemParameters" runat="server" DataSourceID="odsSystemParameters"
                        AutoGenerateColumns="False" OnRowDataBound="grdSystemParameters_RowDataBound"
                        DataKeyNames="SystemParameterId,ApplicationId,Description" AllowSorting="True"
                        OnRowCommand="grdSystemParameters_RowCommand" OnSorting="grdSystemParameters_Sorting"
                        Width="100%" permissionRequired="SystemParameters">
                        <RowStyle CssClass="Items" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                            <asp:BoundField DataField="InitialValue" HeaderText="Valor inicial" SortExpression="InitialValue">
                            </asp:BoundField>
                            <asp:BoundField DataField="Value" HeaderText="Valor" SortExpression="Value"></asp:BoundField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                                SortExpression="Insert">
                                <ItemStyle Width="1%" />
                            </asp:CommandField>
                        </Columns>
                        <AlternatingRowStyle CssClass="AlternateItems" />
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos, clique no botão para cadastrar.<br />
                                &nbsp;<asp:Button ID="btnCreateSystemParameter" runat="server" Text="Cadastrar" 
                                    onclick="btnCreateSystemParameter_Click" />
                            </div>
                        </EmptyDataTemplate>
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
        <VFX:BusinessManagerDataSource ID="odsSystemParameters" runat="server" SelectMethod="GetAllSystemParametersTable"
            TypeName="Vivina.Erp.BusinessRules.SystemParameterManager" UpdateMethod="Update"
            OnSelected="BusinessManagerDataSource1_Selected" OnDeleted="BusinessManagerDataSource1_Deleted"
            ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}"
            DataObjectTypeName="Vivina.Erp.DataClasses.SystemParameter" DeleteMethod="Delete"
            InsertMethod="Insert">
            <updateparameters>
                <asp:Parameter Name="original_entity" Type="Object" />
                <asp:Parameter Name="entity" Type="Object" />
            </updateparameters>
        </VFX:BusinessManagerDataSource>
        <VFX:BusinessManagerDataSource ID="odsApplication" runat="server" SelectMethod="GetAllApplications"
            TypeName="Vivina.Erp.BusinessRules.ApplicationManager">
        </VFX:BusinessManagerDataSource>
        <br />
    </div>
</asp:Content>
