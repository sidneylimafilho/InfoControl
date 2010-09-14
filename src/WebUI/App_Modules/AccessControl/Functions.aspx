<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="FunctionList" Title="Cadastro de Funcoes" CodeBehind="Functions.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                <asp:FormView ID="frmFunctions" runat="server" DataSourceID="odsFunctions" DefaultMode="Insert"
                    OnItemCommand="frmFunctions_ItemCommand" OnItemInserted="frmFunctions_ItemInserted"
                    OnItemUpdated="frmFunctions_ItemUpdated" DataKeyNames="FunctionId,Name,CodeName,Description"
                    Width="100%">
                    <EditItemTemplate>
                        Nome:<br />
                        <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>'>
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NameTextBox"
                            ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21"></asp:RequiredFieldValidator><br />
                        Descricao:<br />
                        <asp:TextBox ID="DescriptionTextBox" runat="server" Text='<%# Bind("Description") %>'
                            Columns="80" MaxLength="1024" Rows="7" TextMode="MultiLine"></asp:TextBox><br />
                        <div style="text-align: right">
                            <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                CssClass="cBtn11" Text="Inserir" Visible="<%# frmFunctions.CurrentMode == FormViewMode.Insert %>"
                                permissionRequired="Functions"></asp:Button>
                            <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="cBtn11" Text="Salvar" Visible="<%# frmFunctions.CurrentMode == FormViewMode.Edit %>"
                                permissionRequired="Functions"></asp:Button>
                            <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="cBtn11" Text="Cancelar"></asp:Button></div>
                    </EditItemTemplate>
                </asp:FormView>
                <asp:GridView ID="grdFunctions" runat="server" AutoGenerateColumns="False" DataSourceID="odsFunctions"
                    AllowPaging="True" AllowSorting="True" OnRowCommand="grdFunctions_RowCommand"
                    OnRowDataBound="grdFunctions_RowDataBound" OnSorting="grdFunctions_Sorting" DataKeyNames="FunctionId,Name,CodeName,Description"
                    OnSelectedIndexChanging="grdFunctions_SelectedIndexChanging" PageSize="20" Width="100%"
                    permissionRequired="Functions">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                        <asp:BoundField DataField="Description" HeaderText="Descricao" SortExpression="Description" />
                        <asp:CommandField SortExpression="Insert" HeaderText="&lt;img src=&quot;../App_Themes/Glass/Controls/GridView/img/Add.gif&quot; alt=Incluir border=0&gt;"
                            ShowDeleteButton="True" DeleteText="&lt;img src=&quot;../App_Themes/Glass/Controls/GridView/img/Pixel_bg.gif&quot; alt=&quot;Apagar&quot; class=&quot;delete&quot; border=0&gt;">
                            <ItemStyle CssClass="teste" Width="20px" />
                        </asp:CommandField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
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
    <VFX:BusinessManagerDataSource ID="odsFunctions" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Function"
        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetAllFunctions" TypeName="Vivina.Erp.BusinessRules.FunctionManager"
        UpdateMethod="Update" OnDeleted="odsFunctions_Deleted" ConflictDetection="CompareAllValues"
        OldValuesParameterFormatString="original_{0}" EnablePaging="True" SortParameterName="sortExpression">
        <updateparameters>
                <asp:Parameter Name="original_entity" Type="Object" />
                <asp:Parameter Name="entity" Type="Object" />
            </updateparameters>
        <selectparameters>
				<asp:parameter Name="sortExpression" Type="String" />
				<asp:parameter Name="startRowIndex" Type="Int32" />
				<asp:parameter Name="maximumRows" Type="Int32" />
			</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
