<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Accounts" Title="Dados Bancários" CodeBehind="Accounts.aspx.cs" %>

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
                <asp:GridView ID="grdAccounts" runat="server" Width="100%" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataSourceID="odsAccounts" DataKeyNames="AccountId,BankId,CompanyId"
                    OnRowDataBound="grdAccounts_RowDataBound"  PageSize="20"
                    RowSelectable="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Banco">
                            <ItemTemplate>
                                <%# Eval("Bank.Name") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Agency" HeaderText="Agência" SortExpression="Agency">
                        </asp:BoundField>
                        <asp:BoundField DataField="AccountNumber" HeaderText="Conta" SortExpression="AccountNumber">
                        </asp:BoundField>
                        
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Account.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div accountId='<%# Eval("AccountId") %>' companyId='<%# Eval("CompanyId") %>' class="delete" title="Excluir">
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"/>
                        </asp:TemplateField>
                        
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar uma conta.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClientClick="location='Account.aspx'; return false;"  />
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
    <VFX:BusinessManagerDataSource ID="odsAccounts" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Account"
        DeleteMethod="Delete" EnablePaging="True" SelectMethod="GetAccounts" SortParameterName="sortExpression"
        TypeName="Vivina.Erp.BusinessRules.AccountManager" onselecting="odsAccounts_Selecting"
         ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}"
        SelectCountMethod="GetAccountsCount">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />
			<asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
			<asp:parameter Name="maximumRows" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
