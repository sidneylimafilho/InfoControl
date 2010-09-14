<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="CompanyTemplates.aspx.cs" Inherits="Vivina.Erp.WebUI.CompanyModels" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <!-- conteudo --> 
                <%--<fieldset>
                    <legend>Modelo de Ordem de compra: </legend>
                    <br />
                    <asp:FileUpload ID="fupPurchaseOrderFile" runat="server" />
                    <asp:RequiredFieldValidator ID="reqfupPurchaseOrderFile" runat="server" ControlToValidate="fupPurchaseOrderFile"
                        ValidationGroup="SaveContractTemplate" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                    &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnPurchaseOrderFileName" runat="server" Text="Salvar" OnClick="btnPurchaseOrderFileName_Click" />
                </fieldset>--%>
                <fieldset>
                    <legend>Modelos </legend>
                    <br />
                    <table>
                        <tr>
                            <%-- <td>
                                Nome:<br />
                                <asp:TextBox ID="txtContractTemplateFileName" runat="server" MaxLength="200"></asp:TextBox>
                                
                            </td>--%>
                            <td>
                                Tipo :
                                <br />
                                <asp:DropDownList ID="cboDocumentTemplateTypes" DataSourceID="odsDocumentTemplateTypes"
                                    DataTextField="Name" AppendDataBoundItems="true" DataValueField="DocumentTemplateTypeId"
                                    runat="server">
                                    <asp:ListItem Value="" Text=""> </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqTxtContractTemplateFileName" runat="server" ControlToValidate="cboDocumentTemplateTypes"
                                    ValidationGroup="SaveContractTemplate" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Selecione o modelo:<br />
                                <asp:FileUpload ID="fupDocumentTemplate" runat="server" />
                                <asp:RequiredFieldValidator ID="reqFupContractTemplate" runat="server" ControlToValidate="fupDocumentTemplate"
                                    ValidationGroup="SaveContractTemplate" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:ImageButton ID="btnAddContractTemplate" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                    runat="server" AlternateText="Salvar Modelo" OnClick="btnAddContractTemplate_Click"
                                    ValidationGroup="SaveContractTemplate" />
                            </td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdDocumentsTemplate" runat="server" RowSelectable="false" AutoGenerateColumns="False"
                                    DataSourceID="odsDocumentTemplates" Width="80%" DataKeyNames="DocumentTemplateId,FileUrl" 
                                    onrowdeleting="grdDocumentsTemplate_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Arquivo" SortExpression="FileName">
                                            
                                            <ItemTemplate>
                                                <a href='<%# Eval("FileUrl") %>' runat='server' target="_blank"><%# Eval("FileName") %></a>
                                            </ItemTemplate>
                                            <ItemStyle Width="80%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentTemplateTypeName" HeaderText="Tipo"></asp:BoundField>
                                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                            SortExpression="Insert">
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="text-align: center">
                                            Não existem dados a serem exibidos
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
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
    <VFX:businessmanagerdatasource id="odsDocumentTemplateTypes" runat="server" SelectMethod="GetAllDocumentTemplateTypes"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager" OldValuesParameterFormatString="original_{0}">
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource id="odsDocumentTemplates" SelectMethod="GetDocumentsTemplatesByCompany"
        runat="server" TypeName="Vivina.Erp.BusinessRules.CompanyManager" onselecting="odsDocumentTemplates_Selecting"
        DeleteMethod="DeleteDocumentTemplate" 
        ondeleting="odsDocumentTemplates_Deleting">
        <deleteparameters>
            <asp:Parameter Name="DocumentTemplateId" />
            <asp:Parameter Name="FileUrl" />
        </deleteparameters>
        <selectparameters>
            <asp:Parameter Name="companyID" Type="Int32" />
        </selectparameters>
    </VFX:businessmanagerdatasource>
</asp:Content>
