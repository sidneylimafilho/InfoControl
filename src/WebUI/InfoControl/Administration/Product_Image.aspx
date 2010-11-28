<%@ Page AutoEventWireup="true" Language="C#" Inherits="InfoControl_Administration_ProductImage"
    CodeBehind="Product_Image.aspx.cs" MasterPageFile="~/infocontrol/Default.master"
    Title="" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        .productImages a
        {
            text-decoration: none;
        }
        .productImages a span
        {
            display: none;
            font-size: 72px;
            color: Red;
            height: 100px;
        }
        .productImages a:hover img
        {
            -moz-opacity: 0.3;
            opacity: 0.3;
            filter: alpha(opacity=30);
        }
        .productImages a:hover span
        {
            position: absolute;
            display: inline;
            margin-top: 30px;
            margin-left: -12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                            Descrição:<br />
                            <asp:TextBox ID="txtDescription" Columns="45" runat="server" MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ErrorMessage="&nbsp&nbsp&nbsp"
                             ControlToValidate="txtDescription" ValidationGroup="Save" ID="reqTxtDescription" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton runat="server" ID="btnSaveImage" ImageUrl="~/App_Shared/themes/glasscyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Adicionar Imagem" ValidationGroup="Save" OnClick="btnSaveImage_Click" />
                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Imagem:<br />
                            <asp:FileUpload ID="fupProductImage" runat="server" />
                             <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ErrorMessage="&nbsp&nbsp&nbsp"
                             ControlToValidate="fupProductImage" ValidationGroup="Save" ID="reqFupProductImage" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
                <span class="productImages">
                    <asp:ListView ID="lstProductImages" ItemPlaceholderID="span" runat="server" DataKeyNames="ProductImageId,ProductId,ImageUrl,Description"
                        DataSourceID="odsProductImage" OnItemDeleting="lstProductImages_ItemDeleting">
                        <LayoutTemplate>
                            <span runat="server" id="span"></span>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnkDeleteImage" CommandName="Delete" OnClientClick='return confirm("Deseja realmente deletar essa imagem?")'>
                                <span>&times;</span>
                                <img id="Img1" runat="server" border="0" src='<%# Eval("ImageUrl") %>' alt='<%# Eval("Description") %>'
                                    height="96" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:ListView>
                </span>
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
        SelectMethod="GetProductImages" TypeName="Vivina.Erp.BusinessRules.ProductManager"
        OldValuesParameterFormatString="original_{0}" SelectCountMethod="GetProductImagesCount"
        SortParameterName="sortExpression" ConflictDetection="OverwriteChanges" EnablePaging="True"
        DeleteMethod="DeleteProductImage" OnDeleting="odsProductImage_Deleting">
        <deleteparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
        <asp:Parameter Name="productImageId" Type="Int32" />
    </deleteparameters>
        <selectparameters>
        <asp:Parameter Name="productId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
