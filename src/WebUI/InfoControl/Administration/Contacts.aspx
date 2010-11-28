<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/infocontrol/Default.master"
    Inherits="Company_Administration_Contacts" Title="" CodeBehind="Contacts.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="../CRM/SelectContact.ascx" TagName="SelectContact" TagPrefix="uc1" %>
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
                <table width="40%">
                    <tr>
                        <td>
                            <uc1:SelectContact ID="selContact" runat="server" Required="true" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="btnAddContact" ImageUrl="~/App_Shared/themes/glasscyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Adicionar Contato" ValidationGroup="add" OnClick="btnAddContact_Click" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdContacts" runat="server" AutoGenerateColumns="False" DataSourceID="odsContacts"
                                Width="100%" DataKeyNames="ContactId" OnRowDataBound="grdContacts_RowDataBound"
                                AllowSorting="True" AllowPaging="True" RowSelectable="false" PageSize="20">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                                    <asp:BoundField DataField="Phone" HeaderText="Telefone" SortExpression="Phone"></asp:BoundField>
                                    <asp:BoundField DataField="Email" HeaderText="E-mail" SortExpression="Email"></asp:BoundField>
                                    <asp:BoundField DataField="Sector" HeaderText="Setor" SortExpression="Sector"></asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="Nome do Usuário" SortExpression="UserName">
                                    </asp:BoundField>
                                    <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot; onclick=&quot;event.cancelBubble=true&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                        ShowDeleteButton="True" ItemStyle-HorizontalAlign="Left">
                                        <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                                    </asp:CommandField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="text-align: center">
                                        Não existem contatos cadastrados ...
                                    </div>
                                </EmptyDataTemplate>
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
    <VFX:BusinessManagerDataSource ID="odsContacts" runat="server" SelectMethod="GetCustomerContacts"
        TypeName="Vivina.Erp.BusinessRules.ContactManager" OnSelecting="odsContacts_Selecting"
        SortParameterName="sortExpression" SelectCountMethod="GetCustomerContactsCount"
        EnablePaging="True" DeleteMethod="DeleteCustomerContact">
        <deleteparameters>
            <asp:Parameter Name="contactId" />
        </deleteparameters>
        <selectparameters>        
        <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="sortExpression" Type="String"></asp:Parameter>
        <asp:Parameter Name="startRowIndex" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="maximumRows" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
