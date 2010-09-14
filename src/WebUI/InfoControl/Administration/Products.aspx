<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_Products" Title="Produtos" CodeBehind="Products.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/AlphabeticalPaging.ascx" TagName="AlphabeticalPaging"
    TagPrefix="uc2" %>
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
                    <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend><div class="body">
                                <table>
                                    <tr>
                                        <td>
                                            Nome:
                                            <br />
                                            <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
                                        </td>
                                        <td valign="top">
                                            Categoria:<br />
                                            <uc1:ComboTreeBox ID="cboTreeCategories" DataFieldID="CategoryId" DataFieldParentID="ParentId"
                                                DataTextField="Name" DataSourceID="odsCategories" DataValueField="CategoryId"
                                                runat="server" />
                                        </td>
                                        <td>
                                            Fabricante:
                                            <br />
                                            <asp:DropDownList DataSourceID="odsManufacturer" ID="cboManufacturer" runat="server"
                                                DataTextField="Name" DataValueField="ManufacturerId" AppendDataBoundItems="True"
                                                Width="200">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Exibir:
                                            <br />
                                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboSelectPageSize_SelectedIndexChanged">
                                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            Descrição:
                                            <br />
                                            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsTemp" runat="server" Text=" Provisórios?" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="style2">
                                            <asp:Button ID="Button1" runat="server" Text="Pesquisar" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <br />
                                <asp:Button ID="btnSearch" runat="server" Text="Pesquisar" OnClick="btnSearch_Click" />
                            </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                        <br />
                        <br />
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />                                      
                    <uc2:AlphabeticalPaging ID="ucAlphabeticalPaging" OnSelectedLetter="ucAlphabeticalPaging_SelectedLetter"
                        runat="server" />
                    <br />
                    <asp:GridView ID="grdProducts" DataSourceID="odsProducts" runat="server" AutoGenerateColumns="False"
                        RowSelectable="false" DataKeyNames="ProductId,CompanyId,ProductCode,Name" Width="100%"
                        AllowPaging="True" AllowSorting="True" EnableViewState="false" OnRowDataBound="grdProducts_RowDataBound"
                        PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                                <ItemTemplate>
                                    <%# Eval("Name")%>
                                    <br />
                                    <small>
                                        <%# String.Join(", ", (Container.DataItem as Product).ProductPackages.Select(pp => pp.Name).ToArray()) %></small>
                                    | <small>
                                        <%# String.Join(", ", (Container.DataItem as Product).ProductManufacturers.Select(pp => pp.Name).ToArray()) %></small>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidade" SortExpression="Unit">
                                <ItemTemplate>
                                    <%# Eval("Unit")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Identificação" SortExpression="Name">
                                <ItemTemplate>
                                    <%# Eval("IdentificationOrPlaca")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="&lt;a href=&quot;Product_General.aspx&quot;&gt;&lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                                SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" productid='<%# Eval("ProductId") %>' companyid='<%# Eval("CompanyId") %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos, clique no botão para cadastrar um produto.<br />
                                &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click"
                                    UseSubmitBehavior="false" OnClientClick="location='Product_General.aspx'; return false;" />
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <br />
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
    <VFX:BusinessManagerDataSource ID="odsCategories" runat="server" SelectMethod="GetCategoriesByCompanyAsDataTable"
        ConflictDetection="CompareAllValues" TypeName="Vivina.Erp.BusinessRules.CategoryManager"
        OnSelecting="odsCategories_Selecting" OldValuesParameterFormatString="original_{0}">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsProducts" runat="server" SelectMethod="GetProducts"
        TypeName="Vivina.Erp.BusinessRules.ProductManager" OnSelecting="odsProducts_Selecting"
        EnablePaging="True" SelectCountMethod="GetProductsCount" SortParameterName="sortExpression">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32" />
			<asp:Parameter Name="categoryId" Type="Int32" />   
			<asp:Parameter Name="manufacturerId" Type="Int32" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="isTemp" Type="Boolean" />
			<asp:Parameter Name="initialLetter" Type="String" />
			<asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
			<asp:parameter Name="maximumRows" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsManufacturer" runat="server" onselecting="odsManufacturer_Selecting"
        SelectMethod="GetManufacturerByCompany" TypeName="Vivina.Erp.BusinessRules.ManufacturerManager">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
