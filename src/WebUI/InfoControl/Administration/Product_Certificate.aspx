<%@ Page  Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" CodeBehind="Product_Certificate.aspx.cs" Inherits="Vivina.Erp.WebUI.Administration.Product_Certificate" %>

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
                            Certificação:<br />
                            <asp:TextBox runat="server" ID="txtCertification" Columns="30" MaxLength="200"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtCertification" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtCertification" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td valign="bottom">
                            <asp:ImageButton runat="server" ID="btnSaveCetification" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Adicionar Certificação" OnClick="btnSaveCetification_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="grdProductCertificates" runat="server" DataSourceID="odsProductCertificates" HorizontalAlign="Right"
                                AutoGenerateColumns="False" Width="180px" RowSelectable="false" DataKeyNames="ProductCertificateId">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Certificações" />
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
    <VFX:BusinessManagerDataSource ID="odsProductCertificates" runat="server" onselecting="odsProductCertificates_Selecting"
        SelectMethod="GetProductCertificates" TypeName="Vivina.Erp.BusinessRules.ProductManager"
        DeleteMethod="DeleteProductCertificate">
        <selectparameters>
        <asp:Parameter Name="productId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
