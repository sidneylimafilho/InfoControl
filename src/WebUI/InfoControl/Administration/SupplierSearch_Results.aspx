<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="InfoControl_Administration_SupplierSearch_Results" MasterPageFile="~/InfoControl/Default.master"
    EnableEventValidation="false" Codebehind="SupplierSearch_Results.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Fornecedores</h1>
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
                                <asp:GridView ID="grvSearchSuppliers" runat="server" AutoGenerateColumns="False"
                                    Width="100%" DataSourceID="odsSearchSupplier" DataKeyNames="SupplierId,Name,Identification,Email,Phone"
                                    OnSelectedIndexChanged="grvSearchSuppliers_SelectedIndexChanged" AllowPaging="True"
                                    AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Nome" />
                                        <asp:BoundField DataField="Identification" SortExpression="Identification" HeaderText="CPF/CNPJ" />
                                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="E-mail" />
                                        <asp:BoundField DataField="Phone" SortExpression="Phone" HeaderText="Telefone" 
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
    <VFX:BusinessManagerDataSource ID="odsSearchSupplier" runat="server" SelectMethod="SearchSuppliers"
        TypeName="Vivina.Erp.BusinessRules.SupplierManager" onselecting="odsSearchSupplier_Selecting"
        SelectCountMethod="SearchSuppliersCount" EnablePaging="True" SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="htSupplier" Type="Object" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
