<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Title="Fornecedores" Inherits="Administration_Company_Suppliers"
    CodeBehind="Suppliers.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="SelectSupplier.ascx" TagName="SelectSupplier" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../App_Shared/AlphabeticalPaging.ascx" TagName="AlphabeticalPaging"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                        Escolha o filtro desejado: </legend>
                    <div class="body">
                        <table width="100%">
                            <tr>
                                <td>
                                    Nome:
                                    <br />
                                    <asp:TextBox ID="txtSelectedSupplier" runat="server" MaxLength="100"></asp:TextBox>
                                </td>
                                <td>
                                    Telefone:<br />
                                    <asp:TextBox ID="txtPhone" Mask="phone" Columns="10" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    E-mail:<br />
                                    <asp:TextBox ID="txtMail" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    CNPJ:<br />
                                    <asp:TextBox ID="txtCNPJ" Columns="14" Mask="99.999.999/9999-99" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    CPF:<br />
                                    <asp:TextBox ID="txtCPF" Columns="10" Mask="999.999.999-99" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    A partir de:&nbsp;
                                    <ajaxToolkit:Rating ID="rtnRanking" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                        WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                        ToolTip="Classificação" CurrentRating="0">
                                    </ajaxToolkit:Rating>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Exibir:<br />
                                    <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server">
                                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                        <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnSearch" runat="server" Text="Pesquisar" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                        &nbsp;</span>
                    <br />
                    <br />
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <uc1:AlphabeticalPaging ID="ucAlphabeticalPaging" OnSelectedLetter="ucAlphabeticalPaging_SelectedLetter"
                    runat="server" />
                <br />
                <asp:GridView ID="grdSuppliers" runat="server" Width="100%" AllowSorting="True" AutoGenerateColumns="False"
                    DataSourceID="odsSearchSupplier" DataKeyNames="SupplierId,ModifiedDate,PostalCode,ProfileId,LegalEntityProfileId,CompanyId,BankId,AccountNumber,Agency,Ranking"
                    OnRowDataBound="grdSuppliers_RowDataBound" RowSelectable="false" OnSorting="grdSuppliers_Sorting"
                    _permissionRequired="Suppliers" AllowPaging="True" PageSize="20" EnableViewState="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                            <ItemTemplate>
                                <%# 
                                Eval("Name") ?? "<b>" + Convert.ToString(Eval("FantasyName")) + "</b> (" + Convert.ToString(Eval("CompanyName")) + ")"%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CPF/CNPJ " SortExpression="Identification">
                            <ItemTemplate>
                                <%# Eval("Identification")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="E-mail" SortExpression="Email">
                            <ItemTemplate>
                                <%# Eval("Email")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Telefone" SortExpression="Phone">
                            <ItemTemplate>
                                <%# Eval("Phone")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Supplier_General.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" id='<%# Eval("SupplierId") %>' companyid='<%# Eval("CompanyId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar um fornecedor.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClientClick="location='Supplier_General.aspx'; return false;" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsSearchSupplier" runat="server" EnablePaging="True"
        onselecting="odsSearchSupplier_Selecting" SelectCountMethod="SearchSuppliersCount"
        SelectMethod="SearchSuppliers" TypeName="Vivina.Erp.BusinessRules.SupplierManager"
        SortParameterName="sortExpression">
        <selectparameters>
                <asp:Parameter Name="htSupplier" Type="Object" />
                <asp:Parameter Name="initialLetter" Type="String" />
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
