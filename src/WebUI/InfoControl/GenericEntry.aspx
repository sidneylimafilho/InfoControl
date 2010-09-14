<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Generico_Cadastro"
    Title="" Codebehind="GenericEntry.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        <%=Request["Title"] %>
        <%--        <a href="javascript:void(0);" class="HelpTip">
            <img id="Img2" runat="server" border="0" src="~/App_themes/_global/ico_ajuda.gif" />
            <span class="msg">• Voltado à gestão contábil da empresa, “CENTRO DE CUSTOS” é onde
                você divide de onde vem cada ganho e cada custo da empresa, separando assim em setores
                (administrativo, operacional, etc.). Assim como foi citado em “PLANO DE CONTAS”,
                essa informação é importante para o controle de todas as contas da empresa. Subdividindo
                seu faturamento e seu custo por área de participação. <span class="footer"></span>
            </span></a>--%>
    </h1>
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNome"
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
