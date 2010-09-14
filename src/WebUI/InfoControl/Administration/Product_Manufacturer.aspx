<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Product_Manufacturer.aspx.cs" Inherits="Vivina.Erp.WebUI.Administration.Product_Manufacturer" %>

<%@ register Assembly="InfoControl" namespace="InfoControl.Web.UI.WebControls" tagprefix="VFX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                            Fabricante:<br />
                            <asp:TextBox runat="server" ID="txtManufacturer" Columns="30" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtManufacturer" runat="server" ControlToValidate="txtManufacturer"
                             ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="Save" />
                        </td>
                        <td valign="bottom">
                            <asp:ImageButton runat="server" ID="btnSaveManufacturer" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Adicionar Fabricante" ValidationGroup="Save" OnClick="btnSaveFabricante_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:GridView runat="server" ID="grdManufacturer" rowselectable="false" 
                                DataSourceID="odsProductManufactures" AutoGenerateColumns="False" DataKeyNames="productManufacturerId"
                                HorizontalAlign="Right" Width="180px">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                    <asp:CommandField ShowDeleteButton="true" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;">
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
    <vfx:BusinessManagerDataSource ID="odsProductManufactures" runat="server" 
        SelectMethod="GetProductManufacturers" 
        TypeName="Vivina.Erp.BusinessRules.ProductManager" 
        onselecting="odsProductManufactures_Selecting" 
        DeleteMethod="DeleteProductManufacturer">
        <SelectParameters>
            <asp:Parameter Name="productId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </SelectParameters>
    </vfx:BusinessManagerDataSource>
</asp:Content>
