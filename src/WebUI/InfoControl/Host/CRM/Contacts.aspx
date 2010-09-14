<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_CRM_Contacts" Title="Untitled Page" EnableEventValidation="false"
    CodeBehind="Contacts.aspx.cs" %>

<%@ Register Src="../../App_Shared/AlphabeticalPaging.ascx" TagName="AlphabeticalPaging"
    TagPrefix="uc1" %>
<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Contatos
    </h1>
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
                <td class="left" style="height: 97px">
                    &nbsp;
                </td>
                <td class="center" style="height: 97px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdContacts" runat="server" DataKeyNames="ownerId,isCustomerContact"
                                    AutoGenerateColumns="False" DataSourceID="odsContacts" Width="100%" AllowPaging="True"
                                    AllowSorting="True" PageSize="20" OnSelectedIndexChanging="grdContacts_SelectedIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                                        <asp:TemplateField HeaderText="Cliente/Fornecedor" SortExpression="OwnerName">
                                            <ItemTemplate>
                                                <%# Eval("OwnerName")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Phone" HeaderText="Telefone" SortExpression="Phone" />
                                        <asp:BoundField DataField="Email" HeaderText="E-mail" SortExpression="Email" />
                                        <asp:BoundField DataField="Sector" HeaderText="Setor" SortExpression="Sector" ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
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
                    <br />
                    <uc1:AlphabeticalPaging ID="ucAlphabeticalPaging" OnSelectedLetter="ucAlphabeticalPaging_SelectedLetter"
                        runat="server" />
                    <br />
                    <br />
                    <VFX:BusinessManagerDataSource ID="odsContacts" runat="server" SelectMethod="GetDetailedContactsByCompany"
                        TypeName="Vivina.InfoControl.BusinessRules.ContactManager" EnablePaging="True"
                        onselecting="odsContacts_Selecting" SelectCountMethod="GetDetailedContactsByCompanyCount"
                        SortParameterName="sortExpression" OldValuesParameterFormatString="original_{0}">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
                            <asp:Parameter Name="sortExpression" Type="String" />
                            <asp:Parameter Name="initialLetter" Type="String" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
                </td>
                <td class="right" style="height: 97px">
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
</asp:Content>
