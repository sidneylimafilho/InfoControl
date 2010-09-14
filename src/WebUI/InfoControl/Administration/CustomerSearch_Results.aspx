<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="InfoControl_Administration_CustomerSearch_Result" EnableEventValidation="false"
    MasterPageFile="~/InfoControl/Default.master" Title="Clientes" Codebehind="CustomerSearch_Results.aspx.cs" %>

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
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdSearchCustomers" runat="server" DataSourceID="odsSearchCustomers"
                                    AutoGenerateColumns="False" OnSelectedIndexChanged="grdSearchCustomers_SelectedIndexChanged"
                                    DataKeyNames="CustomerId" Width="100%" AllowPaging="True" 
                                    AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                                        <asp:BoundField DataField="Identification" HeaderText="CPF/CNPJ" SortExpression="CPF_CNPJ" />
                                        <asp:BoundField DataField="Email" HeaderText="E-mail" SortExpression="Email" />
                                        <asp:BoundField DataField="Phone" HeaderText="Telefone" SortExpression="Phone" 
                                            ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div>
                                            Não existem dados a serem exibidos<br />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <br />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
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
                </td>
                <td class="right">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <VFX:BusinessManagerDataSource ID="odsSearchCustomers" runat="server"
        SelectMethod="SearchCustomers" TypeName="Vivina.Erp.BusinessRules.CustomerManager"
        onselecting="odsSearchCustomers_Selecting" OldValuesParameterFormatString="original_{0}"
        ConflictDetection="CompareAllValues" EnablePaging="True" 
        SelectCountMethod="SearchCustomersCount" SortParameterName="sortExpression">
        <selectparameters>
                <asp:Parameter Name="htCustomer" Type="Object" />
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
