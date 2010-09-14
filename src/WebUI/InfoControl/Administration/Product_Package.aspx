<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Product_Package.aspx.cs" Inherits="Vivina.Erp.WebUI.Administration.Product_Package" %>

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
                <%--Conteudo--%>
                <table>
                    <tr>
                        <td>
                            Embalagem:<br />
                            <asp:TextBox runat="server" ID="txtPackage" Columns="30" MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtPackage" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtPackage" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td valign="bottom">
                            <asp:ImageButton runat="server" ID="btnSavePackage" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Adicionar Embalagem" OnClick="btnSavePackage_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" Text=" Requer Cotação?" ID="chkRequiredQuotation" Checked="false" />
                        </td>
                    </tr>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:GridView runat="server" ID="grdProductPackage" DataSourceID="odsProductPackage"
                    rowselectable="false" AutoGenerateColumns="False" Width="180px" DataKeyNames="ProductPackageId"
                    HorizontalAlign="Right" onrowdeleted="grdProductPackage_RowDeleted">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Embalagens" />
                        <asp:TemplateField HeaderText="Cotar">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblIsRequiredQuotationInPurchasing" Text='<%# Convert.ToBoolean(Eval("RequiresQuotationInPurchasing")) ? "Sim" : "Não" %>'>  </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="true" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;Excluir&quot;&lt;/div&gt;">
                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="1%" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <%--fim da celula center--%>
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
    <VFX:BusinessManagerDataSource ID="odsProductPackage" runat="server" DeleteMethod="DeleteProductPackage"
        onselecting="ods_Selecting" SelectMethod="GetProductPackages" TypeName="Vivina.Erp.BusinessRules.ProductManager">
        <selectparameters>
            <asp:Parameter Name="productId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
