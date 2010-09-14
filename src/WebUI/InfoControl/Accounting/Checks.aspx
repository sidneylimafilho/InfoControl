<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Checks"
    Title="Untitled Page" Codebehind="Checks.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Controle de Cheques<a class="HelpTip" href="javascript:void(0);">
            <img id="Img2" runat="server" border="0" src="~/App_themes/_global/ico_ajuda.gif" />
            <span class="msg">• Cadastre os seus cheques emitidos e recebidos. Em breve, 
        o InfoControl trará uma ferramenta para o gerenciamento completo destes cheques. 
        Isso evitará inadimplência. <span class="footer"></span></span></a>
    </h1>
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
                &nbsp;
                <asp:GridView ID="grdChecks" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataKeyNames="CheckId,EntryDate,Sender,CheckValue,CheckNumber,Agency,Returns,BankId,CompanyId"
                    DataSourceID="odsChecks" Width="100%" OnRowDataBound="grdChecks_RowDataBound"
                    OnSelectedIndexChanged="grdChecks_SelectedIndexChanged" 
                    OnSorting="grdChecks_Sorting" PageSize="20">
                    <Columns>
                        <asp:BoundField DataField="Sender" HeaderText="Cheque de:" SortExpression="Sender">
                        </asp:BoundField>
                        <asp:BoundField DataField="CheckValue" HeaderText="Valor" SortExpression="CheckValue">
                        </asp:BoundField>
                        <asp:CheckBoxField DataField="Returns" HeaderText="Voltou ?" SortExpression="Returns">
                        </asp:CheckBoxField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot; &lt;/div&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar um cheque.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsChecks" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Check"
        DeleteMethod="Delete" EnablePaging="True" SelectMethod="GetChecks" SortParameterName="sortExpression"
        TypeName="Vivina.Erp.BusinessRules.CheckManager" onselecting="odsChecks_Selecting">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />
			<asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
			<asp:parameter Name="maximumRows" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
