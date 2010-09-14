<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_Trasporters"
    Title="Transportadoras" Codebehind="Transporters.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                        <td align="right">
                            Exibir:<br />
                            <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grdTransporters" runat="server" AutoGenerateColumns="False" DataSourceID="odsTransporters"
                    Width="100%" AllowSorting="True" OnRowDataBound="grdTransporters_RowDataBound"
                    RowSelectable="false" _permissionRequired="Transporters"
                    OnSorting="grdTransporters_Sorting" DataKeyNames="TransporterId,CompanyId,Vendor,ModifiedDate,LegalEntityProfileId"
                    AllowPaging="True" PageSize="20">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CNPJ" SortExpression="CNPJ">
                            <ItemTemplate>
                                <asp:Label ID="lblCNPJ" runat="server" Text='<%# Eval("CNPJ") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="E-mail" SortExpression="Email">
                            <ItemTemplate>
                                <%# Eval("Email")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Telefone" SortExpression="Phone">
                            <ItemTemplate>
                                <%# Eval("Phone")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Transporter.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" transporterid='<%# Eval("TransporterId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"/>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar uma transportadora.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsTransporters" runat="server" SelectMethod="GetTransportersByCompany"
        TypeName="Vivina.Erp.BusinessRules.TransporterManager" onselecting="odsTransporters_Selecting"
        ondeleted="odsTransporters_Deleted" ConflictDetection="CompareAllValues" DataObjectTypeName="Vivina.Erp.DataClasses.Transporter"
        DeleteMethod="Delete" EnablePaging="True"
        SelectCountMethod="GetTransporterByCompanyCount" SortParameterName="sortExpression">
        <selectparameters>
            <asp:parameter Name="CompanyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
