<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true"
    Inherits="InfoControl_CRM_CustomerFollowupActions" MasterPageFile="~/InfoControl/Default.master"
    Codebehind="CustomerFollowupActions.aspx.cs" Title="Ações de venda" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
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
                    <!--Principal -->
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
                    <asp:GridView ID="grdCustomerFollowupAction" runat="server" Width="100%" AutoGenerateColumns="False"
                        AllowSorting="True" AllowPaging="True" DataSourceID="odsCustomerFollowupAction"
                        DataKeyNames="CustomerFollowupActionId,Name,Probability,CompanyId" OnRowDataBound="grdCustomerFollowupAction_RowDataBound"
                        OnSorting="grdCustomerFollowupAction_Sorting" RowSelectable="false"
                        
                        OnRowDeleted="grdCustomerFollowupAction_RowDeleted" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                            <asp:BoundField DataField="Probability" HeaderText="Probabilidade" SortExpression="Probability" />
                            <asp:TemplateField HeaderText="&lt;a href=&quot;CustomerFollowupAction.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                                SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" id='<%# Eval("CustomerFollowupActionId") %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos, clique no botão para cadastrar uma ação de venda.<br />
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
    </div>
    <VFX:BusinessManagerDataSource ID="odsCustomerFollowupAction" runat="server" SelectMethod="GetCustomerFollowupActions"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" ConflictDetection="CompareAllValues"
        SortParameterName="sortExpression" onselecting="odsCustomerFollowupAction_Selecting"
        OldValuesParameterFormatString="original_{0}" SelectCountMethod="GetCustomerFollowupActionsCount"
        DataObjectTypeName="Vivina.Erp.DataClasses.CustomerFollowupAction" DeleteMethod="DeleteCustomerFollowUpAction">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
