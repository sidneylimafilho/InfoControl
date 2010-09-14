<%@ Page Title="Estoques" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" CodeBehind="Deposits.aspx.cs" Inherits="Vivina.Erp.WebUI.Administration.Deposits" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <!-- Conteudo -->
                <asp:GridView ID="grdDeposits" runat="server" RowSelectable="false" AutoGenerateColumns="False"
                    DataKeyNames="DepositId,Name,CompanyId,MonthlyGoal,FirstWeekGoal,SecondWeekGoal,ThirdWeekGoal,ForthWeekGoal,FifthWeekGoal"
                    DataSourceID="odsDeposit" Width="100%" AllowPaging="True" AllowSorting="True"
                    OnRowDataBound="grdDeposits_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                        <asp:BoundField DataField="MonthlyGoal" HeaderText="Metas do mês" SortExpression="MonthlyGoal" />
                        <asp:BoundField DataField="FirstWeekGoal" HeaderText="1º semana" SortExpression="FirstWeekGoal" />
                        <asp:BoundField DataField="SecondWeekGoal" HeaderText="2º semana" SortExpression="SecondWeekGoal" />
                        <asp:BoundField DataField="ThirdWeekGoal" HeaderText="3º semana" SortExpression="ThirdWeekGoal" />
                        <asp:BoundField DataField="ForthWeekGoal" HeaderText="4º semana" SortExpression="ForthWeekGoal" />
                        <asp:BoundField DataField="FifthWeekGoal" HeaderText="5º semana" SortExpression="FifthWeekGoal" />
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Deposit.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" depositid='<%# Eval("DepositId") %>' companyid='<%# Eval("CompanyId") %>'
                                    title="Apagar">
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="1%" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <center>
                            Não existem depósitos cadastrados, clique no botão abaixo para cadastrar
                            <br />
                            <asp:Button ID="btnInsertDeposit" runat="server" Text="Cadastrar" OnClientClick="location='Deposit.aspx'; return false;" />
                        </center>
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
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" DataObjectTypeName="Vivina.Erp.DataClasses.Deposit"
        SelectMethod="GetDepositsByCompanyAsList" TypeName="Vivina.Erp.BusinessRules.DepositManager"
        EnablePaging="True" OldValuesParameterFormatString="original{0}" SortParameterName="sortExpression"
        onselecting="odsDeposit_Selecting">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32" />
            <asp:parameter Name="sortExpression" Type="String" />
            <asp:parameter Name="startRowIndex" Type="Int32" />
            <asp:parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
