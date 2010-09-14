<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Title="Resultado da Pesquisa"
    Inherits="Company_ProductSearch_Results" Codebehind="ProductSearch_Results.aspx.cs" %>

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
                    <asp:GridView ID="grdProductSearch" runat="server" DataSourceID="odsProductSearch"
                        AutoGenerateColumns="False" Width="100%" DataKeyNames="ProductId,depositName,Name,Quantity,MinimumRequired,ProductCode,categoryName,manufacturerName,isActive"
                        OnRowDataBound="grdProductSearch_RowDataBound" RowSelectable="false"
                        permissionRequired="Products" AllowPaging="True" AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="depositName" SortExpression="depositName" HeaderText="Depósito">
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Nome"></asp:BoundField>
                            <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Quantidade">
                            </asp:BoundField>
                            <asp:BoundField DataField="MinimumRequired" SortExpression="MinimumRequired" HeaderText="Minimo Req.">
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="Código">
                            </asp:BoundField>
                            <asp:BoundField DataField="categoryName" SortExpression="categoryName" HeaderText="Categoria">
                            </asp:BoundField>
                            <asp:BoundField DataField="manufacturerName" SortExpression="manufacturerName" HeaderText="Fabricante">
                            </asp:BoundField>
                            <asp:BoundField DataField="isActive" SortExpression="isActive" HeaderText="Ativo ?" ItemStyle-HorizontalAlign="Left">
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            Nenhum ítem foi encontrado.
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <br />
                    <br />
                    <div style="text-align: right">
                        <asp:Button ID="btnVoltar" runat="server" Text="Cancelar" OnClick="btnVoltar_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsProductSearch" runat="server" SelectMethod="SearchProducts"
        TypeName="Vivina.Erp.BusinessRules.ProductManager" OnSelecting="BusinessManagerDataSource1_Selecting"
        ConflictDetection="CompareAllValues" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
        SelectCountMethod="SearchProductsCount" SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="ht" Type="Object" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
