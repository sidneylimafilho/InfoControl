<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    Inherits="InfoControl_Services_Services" Title="Serviço" CodeBehind="Services.aspx.cs" %>

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
                <!-- Conteudo -->
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
                <br />
                <asp:GridView ID="grvServices" runat="server" Width="100%" AutoGenerateColumns="False"
                    AllowSorting="True" DataKeyNames="ServiceId,CompanyId,Name,Price,TimeInMinutes"
                    DataSourceID="odsServices" OnRowDataBound="grvServices_RowDataBound" OnSorting="grvServices_Sorting"
                    RowSelectable="false" EnableViewState="False" PageSize="20" AllowPaging="True">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Descrição" SortExpression="Name" />
                        <asp:BoundField DataField="Price" HeaderText="Preço" SortExpression="Price" />
                        <asp:BoundField DataField="TimeInMinutes" HeaderText="Tempo em Minutos" SortExpression="TimeInMinutes" />
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Service.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" serviceid='<%# Eval("ServiceId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar um Serviço.<br />
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
    <VFX:BusinessManagerDataSource runat="server" ID="odsServices" SelectMethod="GetServices"
        EnablePaging="True" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager"
        SelectCountMethod="GetServicesCount" ConflictDetection="CompareAllValues" SortParameterName="sortExpression"
        onselecting="odsServices_Selecting" ondeleted="odsServices_Deleted" OldValuesParameterFormatString="original_{0}">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
