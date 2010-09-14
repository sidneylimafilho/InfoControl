<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_CRM_Contacts" Title="Contatos" EnableEventValidation="false"
    CodeBehind="Contacts.aspx.cs" %>

<%@ Register Src="../../App_Shared/AlphabeticalPaging.ascx" TagName="AlphabeticalPaging"
    TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                    <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend>
                        <div class="body">
                            <table width="100%">
                                <tr>
                                    <td>
                                        Nome do Contato:<br />
                                        <asp:TextBox runat="server" ID="txtContactName"></asp:TextBox>
                                    </td>
                                    <td>
                                        Cliente/Fornecedor:<br />
                                        <asp:TextBox runat="server" ID="txtOwner"></asp:TextBox>
                                    </td>
                                    <td>
                                        Usuário:<br />
                                        <asp:DropDownList ID="cboUser" runat="server" DataSourceID="odsUsers" DataTextField="UserName"
                                            DataValueField="UserId" AutoPostBack="false" AppendDataBoundItems="true">
                                            <asp:ListItem Text="" Value="">   </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <br />
                                        <asp:Button ID="btnSearch" runat="server" Text="Filtrar" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </div>
                        <span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                            &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <uc1:AlphabeticalPaging ID="ucAlphabeticalPaging" OnSelectedLetter="ucAlphabeticalPaging_SelectedLetter"
                        runat="server" />
                    <br />
                    <asp:GridView ID="grdContacts" runat="server" DataKeyNames="ownerId,isCustomerContact,ContactId"
                        AutoGenerateColumns="False" DataSourceID="odsContacts" Width="100%" AllowPaging="True"
                        AllowSorting="True" PageSize="20" RowSelectable="false" OnRowDataBound="grdContacts_RowDataBound">
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
                            <asp:BoundField DataField="UserName" HeaderText="Nome do Usuário" SortExpression="UserName" />
                           <%-- <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                HeaderText="&lt;a href=&quot;../Administration/Contact.aspx&quot;> <div class=&quot;insert&quot;title=&quot;inserir&quot; </div>  </a>"
                                ShowDeleteButton="True" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                            </asp:CommandField>--%>
                            <asp:TemplateField HeaderText="&lt;a href=&quot;../Administration/Contact.aspx&quot;> <div class=&quot;insert&quot;title=&quot;inserir&quot; </div>  </a>">
                                <ItemTemplate>
                                    <div class="delete" title="excluir" contactid='<%# Eval("ContactId") %>'>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos<br />
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <br />
                    <VFX:BusinessManagerDataSource ID="odsContacts" runat="server" SelectMethod="GetDetailedContactsByCompany"
                        TypeName="Vivina.Erp.BusinessRules.ContactManager" EnablePaging="True" onselecting="odsContacts_Selecting"
                        SelectCountMethod="GetDetailedContactsByCompanyCount" SortParameterName="sortExpression"
                        OldValuesParameterFormatString="original_{0}">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
                            <asp:Parameter Name="userId" Type="Int32" />
                            <asp:Parameter Name="initialLetter" Type="String" />
                            <asp:Parameter Name="contactName" Type="String" />
                            <asp:Parameter Name="contactOwner" Type="String" />
                            <asp:Parameter Name="sortExpression" Type="String" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsUsers" runat="server" SelectMethod="GetUsers"
                        TypeName="Vivina.Erp.BusinessRules.CompanyManager" onselecting="odsUsers_Selecting">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
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
