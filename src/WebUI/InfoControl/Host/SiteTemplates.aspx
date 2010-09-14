<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Host_SiteTemplates" Title="Modelos de sites"
    CodeBehind="SiteTemplates.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
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
                <!-- conteudo -->
                <asp:GridView Width="100%" ID="grdSiteTemplates" DataKeyNames="SiteTemplateId,Name,BranchId"
                    runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSelectedIndexChanging="grdSiteTemplates_SelectedIndexChanging"
                    OnSorting="grdSiteTemplates_Sorting" OnRowDataBound="grdSiteTemplates_RowDataBound"
                    AllowPaging="True">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                            SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar um template.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <%--<VFX:BusinessManagerDataSource ID="odsSiteTemplates" runat="server" SelectMethod="GetSiteTemplates"
                    TypeName="Vivina.Erp.BusinessRules.SiteBuilder.SiteManager" DataObjectTypeName="Vivina.Erp.DataClasses.SiteTemplate"
                    DeleteMethod="DeleteSiteTemplate" ConflictDetection="CompareAllValues" EnablePaging="True"
                    SelectCountMethod="GetSiteTemplatesCount" SortParameterName="sortExpression">
                    <selectparameters>
                        <asp:Parameter Name="sortExpression" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                    </selectparameters>
                </VFX:BusinessManagerDataSource>--%>
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
</asp:Content>
