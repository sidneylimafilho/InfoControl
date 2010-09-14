<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Accounting_FinancierOperations"
    Title="Formas de Pagamento" CodeBehind="FinancierOperations.aspx.cs" %>

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
                <!-- conteudo -->
                <asp:GridView ID="grdFinancierOperations" Width="100%" runat="server" AllowPaging="True"
                    AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsFinancierOperations"
                    DataKeyNames="FinancierOperationId"
                    OnSelectedIndexChanging="grdFinancierOperations_SelectedIndexChanging" OnSorting="grdFinancierOperations_Sorting"
                    OnSelectedIndexChanged="grdFinancierOperations_SelectedIndexChanged">
                    <Columns>     
                        <asp:BoundField DataField="PaymentMethodName" HeaderText="Formas de Pagamento Aceitas" SortExpression="PaymentMethodName" />        
                        
                        <asp:TemplateField ItemStyle-Width="1px" HeaderText="&lt;a href=&quot;FinancierOperation.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" companyid='<%# Eval("companyId") %>' financieroperationid='<%# Eval("FinancierOperationId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar uma operação
                            Financeira.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClientClick="location='FinancierOperation.aspx';return false;" />
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
    <VFX:BusinessManagerDataSource ID="odsFinancierOperations" runat="server" ConflictDetection="CompareAllValues"
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" SelectCountMethod="GetFinancierOperationsCount"
        SelectMethod="GetFinancierOperations" SortParameterName="sortExpression"
        TypeName="Vivina.Erp.BusinessRules.AccountManager" onselecting="odsFinancierOperations_Selecting1">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
