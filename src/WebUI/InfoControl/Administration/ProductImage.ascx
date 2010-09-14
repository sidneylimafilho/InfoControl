<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="InfoControl_Administration_ProductImage" Codebehind="ProductImage.ascx.cs" %>
<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
            <table>
                <tr>
                    <td>
                        <asp:FileUpload ID="fupProductImage" runat="server" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblDescription" runat="server" Text="Descrição:"></asp:Label><br />
                        <asp:TextBox ID="txtDescription" Columns="15" runat="server" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="grdProductImage" runat="server" Width="100%" AutoGenerateColumns="False"
                            DataKeyNames="ProductImageId,ProductId,ImageUrl,Description" DataSourceID="odsProductImage"
                            AllowPaging="True" AllowSorting="True" 
                            OnRowDeleting="grdProductImage_RowDeleting" 
                            onrowdatabound="grdProductImage_RowDataBound">
                            <Columns>
                              <%--  <asp:BoundField DataField="ImageUrl" HeaderText="Caminho da imagem" SortExpression="ImageUrl" />--%>
                                <asp:BoundField DataField="Description" HeaderText="Descrição" SortExpression="Description" />
                                <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;">
                                    <ItemStyle Width="1%" />
                                </asp:CommandField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="text-align: center">
                                    Não existem dados a serem exibidos<br />
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSaveImage" runat="server" Text="Salvar" OnClick="btnSaveImage_Click" />
                        &nbsp;&nbsp;&nbsp;
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
            &nbsp;
        </td>
        <td class="right">
            &nbsp;
        </td>
    </tr>
</table>
<VFX:BusinessManagerDataSource ID="odsProductImage" runat="server" OnSelecting="odsProductImage_Selecting"
    SelectMethod="GetProductImages" TypeName="Vivina.InfoControl.BusinessRules.ProductManager"
    OldValuesParameterFormatString="original_{0}" SelectCountMethod="GetProductImagesCount"
    SortParameterName="sortExpression" ConflictDetection="CompareAllValues" EnablePaging="True"
    DeleteMethod="DeleteProductImage" OnDeleting="odsProductImage_Deleting">
    <DeleteParameters>
        <asp:Parameter Name="companyId" Type="Int32" />
        <asp:Parameter Name="entity" Type="Object" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="productId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
