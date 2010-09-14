<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_Companies"
    Title="Empresas" Codebehind="Companies.aspx.cs" %>

<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
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
                <table width="100%">
                    <tr>
                        <td align="right">
                            Exibir:<br />
                            <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grdCompanies" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    DataSourceID="odsCompanies" PageSize="10" Width="100%" DataKeyNames="CompanyId"
                    OnRowDataBound="grdCompanies_RowDataBound" RowSelectable="false" _permissionRequired="Companies">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("LegalEntityProfile.CompanyName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CNPJ" SortExpression="CNPJ">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("LegalEntityProfile.CNPJ") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="E-Mail" SortExpression="Mail">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("LegalEntityProfile.Email") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Telefone" SortExpression="Telephone">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("LegalEntityProfile.Phone") %>'></asp:Label>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Company.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                            <ItemTemplate>
                                <asp:LinkButton CommandName="Delete" runat="server" Visible='<%# (int)Eval("LegalEntityProfileId")!=Company.LegalEntityProfileId %>'><div class="delete" title="excluir"></div></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar uma empresa.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClientClick="location='Company.aspx'; return false;" />
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
    <uc1:ToolTip ID="tipCompanies" runat="server" Message="Cadastre todas as suas empresas aqui, o InfoControl é totalmente preparado para gerir sua(s) empresa(s). Durante o cadastro é ideal que seja feita a ligação entre empresa-matriz e filial(is)."
        Title="Dica:" Indication="top" Top="125px" Left="320px" Visible="true" />
    <VFX:BusinessManagerDataSource runat="server" ID="odsCompanies" DataObjectTypeName="Vivina.Erp.DataClasses.Company"
        DeleteMethod="Delete" SelectMethod="GetCompaniesByUser" TypeName="Vivina.Erp.BusinessRules.CompanyManager"
        onselecting="odsCompanies_Selecting" EnablePaging="True" SortParameterName="sortExpression"
        ondeleted="odsCompanies_Deleted" ConflictDetection="CompareAllValues" ondeleting="odsCompanies_Deleting"
        SelectCountMethod="GetCompaniesByUserCount">
        <selectparameters>
			<asp:parameter Name="userId" Type="Int32" />
			<asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
			<asp:parameter Name="maximumRows" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
