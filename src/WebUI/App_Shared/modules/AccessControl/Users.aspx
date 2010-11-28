<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="ListUsers" Title="Usuários" CodeBehind="Users.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc2" %>
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
                <asp:GridView ID="grdListUsers" runat="server" DataSourceID="odsUsers" AutoGenerateColumns="False"
                    DataKeyNames="UserId,Password,UserName,Email,LastLoginDate,LastPasswordChangedDate,LastLockoutDate,CreationDate,FailedPasswordAttemptCount,IsLockedOut,IsActive,HasChangePassword,PasswordQuestion,PasswordAnswer,LastRemoteHost,LastActivityDate,IsOnline,ProfileId"
                    OnRowDataBound="grdListUsers_RowDataBound" Width="100%" AllowPaging="True" AllowSorting="True"
                    CssClass="cGrd11" OnSorting="grdListUsers_Sorting" RowSelectable="false">
                    <RowStyle CssClass="Items" />
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="Nome do Usuário" SortExpression="UserName">
                        </asp:BoundField>
                        <asp:BoundField DataField="LastLoginDate" HeaderText="Ultimo Login" SortExpression="LastLoginDate">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Ativo" SortExpression="IsActive">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))?"Sim":"Não" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&lt;a href=&quot;User_General.aspx&quot;&gt;&lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" id='<%# Eval("UserId") %>' companyid='<%= Company.CompanyId %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="AlternateItems" />
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            <br />
                            Não existem usuários cadastrados ...
                            <br />
                            <br />
                            <asp:Button ID="btnInsert" runat="server" Text="Inserir Novo Usuário" OnClientClick="location='User_General.aspx'; return false;" />
                            <br /><br />
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
    <uc2:ToolTip ID="tipUsers" runat="server" Message="Não esqueça de garantir que estes novos usuários do seu sistema tenham o acesso que lhes é devido. Para isso, basta criar um 'PERFIL' que atenda somente às necessidades deste usuário."
        Title="Dica:" Indication="top" Top="110px" Left="200px" Visible="true" />
    <VFX:BusinessManagerDataSource ID="odsUsers" runat="server" SelectMethod="GetUserByCompanyAsList"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager" ConflictDetection="CompareAllValues"
        EnablePaging="True" OnSelecting="odsUsers_Selecting" SortParameterName="sortExpression">
        <selectparameters>
                <asp:parameter Name="companyId" Type="Int32" />
                <asp:parameter Name="sortExpression" Type="String" />
                <asp:parameter Name="startRowIndex" Type="Int32" />
                <asp:parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
