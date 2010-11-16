<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Administration_Representants" Title="Representantes"
    CodeBehind="Representants.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
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
                <table width="100%">
                    <tr>
                        <td>
                            <uc1:AlphabeticalPaging ID="ucAlphabeticalPaging" OnSelectedLetter="ucAlphabeticalPaging_SelectedLetter"
                                runat="server" />
                        </td>
                        <td align="right">
                            Exibir:&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grdRepresentants" Width="100%" runat="server" DataSourceID="odsRepresentant"
                    AutoGenerateColumns="False" AllowPaging="True" RowSelectable="false" DataKeyNames="RepresentantId,CompanyId,ProfileId,LegalEntityProfileId,ModifiedDate,BankId,Agency,AccountNumber,Rating"
                    AllowSorting="True" PageSize="20" OnRowDataBound="grdRepresentants_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Código" SortExpression="RepresentantId">
                            <ItemTemplate>
                                <%# Eval("RepresentantId") %>&nbsp;
                            </ItemTemplate>
                            <ItemStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                            <ItemTemplate>
                                <%# Eval("Name") %></ItemTemplate>
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
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Representant.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" id='<%# Eval("RepresentantId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            <br />
                            Não existem representantes cadastrados, que obedeçam ao critério do filtro para
                            serem exibidos!<br />
                            &nbsp;<br />
                            <asp:Button ID="btnTransfer" runat="server" Text="Adicionar Representante" OnClientClick="location='representant.aspx'; return false;" />
                            <br />
                            <br />
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
    <VFX:BusinessManagerDataSource ID="odsRepresentant" runat="server" ConflictDetection="CompareAllValues"
        DataObjectTypeName="Vivina.Erp.DataClasses.Representant" DeleteMethod="Delete"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetRepresentantsByCompany"
        TypeName="Vivina.Erp.BusinessRules.RepresentantManager" EnablePaging="True" SelectCountMethod="GetRepresentantsByCompanyCount"
        SortParameterName="sortExpression" onselecting="odsRepresentant_Selecting" ondeleted="odsRepresentant_Deleted">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="initialLetter" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
