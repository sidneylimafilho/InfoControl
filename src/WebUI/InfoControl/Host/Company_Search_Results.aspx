<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Title="Clientes" Inherits="Company_Search_Results" Codebehind="Company_Search_Results.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                    <asp:GridView ID="grvCustomer" RowSelectable="false" runat="server" DataSourceID="odsCustomer"
                        AutoGenerateColumns="False" Width="100%" DataKeyNames="CompanyId" OnRowDataBound="grvCustomer_RowDataBound"
                        OnSelectedIndexChanged="grvCustomer_SelectedIndexChanged" 
                        EnableModelValidation="True"  onsorting="grvCustomer_Sorting" PageSize="10000">
                        <Columns>
                            <asp:BoundField DataField="CompanyName" HeaderText="Nome" 
                                SortExpression="CompanyName"></asp:BoundField>
                            <asp:BoundField DataField="LastActivityDate" HeaderText="Ultimo Login" 
                                SortExpression="LastActivityDate"></asp:BoundField>
                            <asp:TemplateField HeaderText="Excluir">
                                <ItemTemplate>
                                    <input type="checkbox" name="chkCustomer" value='<%# Eval("CompanyId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            Não houveram dados a ser exibidos ...
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <br />
                    <br />
                    <div style="text-align: right">
                        <asp:Button ID="btnDelete" runat="server" Text="Excluir" OnClick="btnDelete_Click" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                    </div>
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
    <VFX:BusinessManagerDataSource ID="odsCustomer" runat="server" TypeName="Vivina.Erp.BusinessRules.CompanyManager"
        OnSelecting="odsCustomer_Selecting" SelectMethod="SearchCompanies">
        <selectparameters>
            <asp:Parameter Name="htCompany" Type="Object" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
