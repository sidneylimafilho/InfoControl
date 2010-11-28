<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_RH_AdditionalInfo" Title="Conteúdo dos Campos Adicionais"
    CodeBehind="AdditionalInfo.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <asp:Label ID="lblErr" runat="server" Text="Label" CssClass="cErr11" Visible="False"></asp:Label><asp:ValidationSummary
                    ID="ValidationSummary1" runat="server" DisplayMode="List" ValidationGroup="Salvar" />
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
                Nome do Campo:<br />
                <asp:DropDownList ID="cboAdditionalInfo" runat="server" AutoPostBack="true" DataSourceID="odsHumanResources"
                    DataTextField="Name" OnSelectedIndexChanged="cboAdditionalInfo_SelectedIndexChanged"
                    DataValueField="AddonInfoId">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ID="reqCboAdditionalInfo" ErrorMessage="&nbsp&nbsp&nbsp"
                    ValidationGroup="Salvar" ControlToValidate="cboAdditionalInfo" />
                <br />
                <br />


                 <br />
                <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" EditIndex="0"
                    HorizontalAlign="Center" OnRowDeleting="grid_RowDeleting" OnRowUpdating="grid_RowUpdating"
                    Width="100%" OnRowCancelingEdit="grid_RowCancelingEdit" OnSelectedIndexChanging="grid_SelectedIndexChanging" 
                    onrowdatabound="grid_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome">
                            <ItemStyle Wrap="false" />
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNome" CssClass="cDat11" runat="server" Width="90%" MaxLength="50"
                                    Text='<%# Bind("Name") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNome"
                                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="Salvar" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="90%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Update" ToolTip="Salvar"
                                    CssClass="save" ValidationGroup="Salvar"></asp:LinkButton>

                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Cancel" ToolTip="Cancelar"
                                    CssClass="cancel" ValidationGroup="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" ToolTip="Excluir"
                                    CssClass="delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

         <%--       <asp:GridView ID="grid" runat="server" RowSelectable="false" AutoGenerateColumns="False"
                    EditIndex="0" HorizontalAlign="Center" OnRowDeleting="grid_RowDeleting" OnRowEditing="grid_RowEditing"
                    OnRowUpdating="grid_RowUpdating" Width="100%" OnRowCancelingEdit="grid_RowCancelingEdit"
                    GridLines="None" OnRowDataBound="grid_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources: Resource, Name %>">
                            <ItemStyle Wrap="True" />
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" CssClass="cDat11" runat="server" Width="90%" MaxLength="50"
                                    Text='<%# Bind("Name") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="Salvar" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:CommandField DeleteText="<span class='delete' title='excluir'> </span>" ShowDeleteButton="True">
                            <ItemStyle Width="1%" Wrap="True" HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:CommandField CancelText="&lt;img src='../../App_Shared/themes/glasscyan/Controls/GridView/img/Cancel.gif' border='0' /&gt;"
                            EditText="" ShowEditButton="True" UpdateText="<span class='save' title='salvar'> </span>"
                            ValidationGroup="Salvar">
                            <ItemStyle Width="1%" Wrap="True" HorizontalAlign="Center" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>--%>
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
    <VFX:BusinessManagerDataSource ID="odsHumanResources" runat="server" SelectMethod="GetAllAdditionalInformation"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="odsHumanResources_Selecting">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />			
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
