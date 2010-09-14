<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="ExpenditureAuthorizations.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Accounting.ExpenditureAuthorizations" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h1>
        Autorização de Despesas
    </h1>
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                &#160;
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                <%--Conteúdo--%>
                <asp:Panel align="right" Visible="false" runat="server" ID="pnlAuthorize">
                    <asp:Button runat="server" OnClick="btnAuthorize_Click" Text="Autorizar" ID="btnAuthorize" />
                    &nbsp&nbsp
                    <asp:Button runat="server" OnClick="btnNonAuthorize_Click" Text="Não Autorizar" ID="btnNonAuthorize" />
                </asp:Panel>
                <%-- <div runat="server" id="pnlAuthorize" visible='<%# Convert.ToBoolean(grdExpenditureAuthorizations.Rows.Count != 0) %>' align="right">
                   
                </div>--%>
                <br />
                <asp:GridView Width="100%" runat="server" DataSourceID="odsExpenditureAuthorizations"
                    ID="grdExpenditureAuthorizations" AutoGenerateColumns="False" OnRowDataBound="grdExpenditureAuthorizations_RowDataBound"
                    AllowPaging="true" AllowSorting="true" PageSize="20" RowSelectable="False" DataKeyNames="ExpenditureAuthorizationId">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <input type="checkBox" onclick="event.cancelBubble=true" id="chkSelectRow" name="chkSelectRow"
                                    value='<%# Eval("ExpenditureAuthorizationId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Cliente" DataField="CustomerName" SortExpression="CustomerName" />
                        <asp:BoundField HeaderText="Número do Chamado" DataField="CallNumber" SortExpression="CallNumber" />
                        <asp:BoundField HeaderText="Data do Chamado" DataFormatString="{0:dd/MM/yyyy}" DataField="CreatedDate"
                            SortExpression="CreatedDate" />
                        <asp:BoundField HeaderText="Valor da Despesa" DataField="Amount" SortExpression="Amount" />
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" ClientIDMode="Static" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Nome do Técnico" DataField="EmployeeName" SortExpression="EmployeeName" />
                        <asp:CommandField HeaderText="&lt;a href=&quot;ExpenditureAuthorization.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            DeleteText="<span class='delete' title='excluir'> </span>"   
                            ShowDeleteButton="True">
                            <ItemStyle HorizontalAlign="Left" Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <p align="center">
                            Não há dados a serem exibidos
                            <br />
                            <asp:Button Text="Nova despesa" runat="server" OnClientClick="location='ExpenditureAuthorization.aspx';return false;" />
                        </p>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &#160;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
    </table>
    <input type="hidden" id="txtExpensureAuthorizationId" />
    <VFX:BusinessManagerDataSource ID="odsExpenditureAuthorizations" runat="server" onselecting="odsExpenditureAuthorizations_Selecting"
        SelectMethod="GetExpenditureAuthorizations" TypeName="Vivina.Erp.BusinessRules.AccountManager"
        MaximumRowsParameterName="MaximumRows" EnablePaging="True" DeleteMethod="DeleteExpenditureAuthorization"
        SelectCountMethod="GetExpenditureAuthorizationsCount" SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />           
              <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
        <deleteparameters>
            <asp:Parameter Name="expenditureAuthorizationId" Type="Int32" />
        </deleteparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
