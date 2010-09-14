<%@ Page Title="Certificação" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="ProductCertificate.aspx.cs" Inherits="Vivina.InfoControl.WebUI.InfoControl.Administration.Product_Certificate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls" TagPrefix="VFX" %>

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
                        <td valign="top">
                            Certificação:<br />
                            <asp:TextBox runat="server" ID="txtCertification" Columns="50"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnSaveCetification" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Adicionar Certificação" onclick="btnSaveCetification_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Certificações deste produto:<br />
                            <asp:GridView ID="grdProductCertificates" runat="server" DataSourceID="odsProductCertificates" 
                                AutoGenerateColumns="False" Width="105px" RowSelectable="false" DataKeyNames="ProductCertificateId"
                                onrowdeleting="grdProductCertificates_RowDeleting" >
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
    
<VFX:BusinessManagerDataSource ID="odsProductCertificates" runat="server" 
        SelectMethod="GetProductCertificates" 
        TypeName="Vivina.InfoControl.BusinessRules.ProductManager" 
        OldValuesParameterFormatString="original_{0}" 
        onselecting="odsProductCertificates_Selecting" 
        DeleteMethod="DeleteProductCertificate">
    <DeleteParameters>
        <asp:Parameter Name="productCertificateId" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="productId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>

</asp:Content>
