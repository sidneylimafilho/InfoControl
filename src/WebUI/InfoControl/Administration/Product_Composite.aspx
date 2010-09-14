<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Product_Composite.aspx.cs" Inherits="Company_Products_Composite" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc2" %>
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
                <table>
                    <tr>
                        <td>
                            Quantidade:<br />
                            <uc1:CurrencyField ID="ucCurrFieldQuantity" Columns="4" Mask="9999" ValidationGroup="Add"
                                Required="true" runat="server" />
                            <br />
                            <br />
                        </td>
                        <td>
                            <uc2:SelectProduct ID="ucSelectProduct" runat="server" ValidationGroup="Add" Required="true" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~\App_Themes\GlassCyan\Controls\GridView\img\Add2.gif"
                                AlternateText="Adicionar composição do produto" OnClick="btnAdd_Click" ValidationGroup="Add" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:GridView ID="grdComposites" runat="server" AutoGenerateColumns="False" permissionRequired="Products"
                                DataSourceID="odsComposite" DataKeyNames="CompositeId,CompositeProductId,ProductId,Amount"
                                OnRowDataBound="grdComposites_RowDataBound" OnRowDeleting="grdComposites_RowDeleting"
                                Width="100%" HorizontalAlign="Right">
                                <Columns>
                                    <asp:TemplateField HeaderText="Qtd" SortExpression="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Composição:" SortExpression="ProductName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Product.Name") +" - "+ Eval("ProductPackage.Name") +" - "+ Eval("ProductManufacturer.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                        ShowDeleteButton="True">
                                        <ItemStyle HorizontalAlign="Left" Width="1%" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsComposite" runat="server" OnSelecting="odsComposite_Selecting"
        SelectMethod="GetChildCompositeProducts" TypeName="Vivina.Erp.BusinessRules.ProductManager"
        DeleteMethod="RemoveComposite" ConflictDetection="CompareAllValues" ondeleting="odsComposite_Deleting">
        <%--DataObjectTypeName="Vivina.Erp.DataClasses.CompositeProduct"--%>
        <deleteparameters>
            <asp:Parameter Name="compositeId" Type="Int32" />
        </deleteparameters>
        <selectparameters>
            <asp:Parameter Name="CompositeProductId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
