<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_ProspectBuilder" Title="Gerador de Proposta" CodeBehind="ProspectBuilder.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %><%@ Register
    Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer" TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %><%@ Register Src="~/App_Shared/HelpTooltip.ascx"
    TagName="HelpTooltip" TagPrefix="vfx" %><%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"
        TagPrefix="telerik" %><%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct"
            TagPrefix="uc3" %><%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/SelectProductAndService.ascx" TagName="SelectProductAndService" TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %><%@ Register
    Src="../../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Header"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">&nbsp;</td>
            <td class="center">&nbsp;</td>
            <td class="right">&nbsp;</td>
        </tr>
        <tr class="middle">
            <td class="left">&nbsp;</td>
            <td class="center"><%--Conteúdo--%>
                <fieldset><legend class="cTxt32">Cliente</legend>
                    <table>
                        <tr>
                            <td>Proposta Número:<br />
                                <asp:TextBox ID="txtBudgetCode" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                    ValidationGroup="NonSave" ControlToValidate="txtBudgetCode" />
                            </td>
                            <td>Vendedor:<br />
                                <asp:DropDownList ID="cboVendor" runat="server" AppendDataBoundItems="true" DataTextField="Name" DataValueField="EmployeeId">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqCboVendor" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;" ControlToValidate="cboVendor"
                                    CssClass="cErr21" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <uc2:SelectCustomer ID="sel_customer" runat="server" />
                            </td>
                            <td style="border-left: 1px solid #009999">
                                <table>
                                    <tr>
                                        <td>Nome:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCustomerName" Columns="22" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>E-mail:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCustomerMail" Columns="22" MaxLength="200"></asp:TextBox>
                                            <asp:RegularExpressionValidator runat="server" ID="regTxtCustomerMail" ControlToValidate="txtCustomerMail"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Telefone:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPhone" MaxLength="13" Columns="10"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender runat="server" ID="txtPhone_MaskedEditExtender" Mask="(99)9999-9999"
                                                MaskType="Number" TargetControlID="txtPhone" ClearMaskOnLostFocus="false" AutoComplete="false"></ajaxToolkit:MaskedEditExtender>
                                            <asp:RegularExpressionValidator ID="RegTxtPhone" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;"
                                                ControlToValidate="txtPhone" ValidationExpression="((\([0-9_]{2}\))([0-9_]{4})\-([0-9_]{4}))?">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset><legend class="cTxt32">Produto</legend>
                    <table>
                        <tr>
                            <td colspan="2">Termo de Referência:<br />
                                <asp:TextBox ID="txtReference" runat="server" Columns="25" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Quantidade:<br />
                                <uc1:CurrencyField ID="ucCurrFieldQuantityData" Columns="4" Mask="9999" ValidationGroup="Add" Required="true"
                                    runat="server" />
                                <br />
                                <br />
                            </td>
                            <td>
                                <uc2:SelectProductAndService ID="SelProductAndService" Required="true" ValidationGroup="Add" runat="server" />
                            </td>
                            <td>Preço Unitário(R$):<br />
                                <uc1:CurrencyField ID="ucCurrFieldUnitPrice" MaxLength="9" runat="server" />
                                <br />
                                <br />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnShowProductDescription" ValidationGroup="search" runat="server" ImageUrl="~/App_Themes/_global/Company/view.gif"
                                    AlternateText="Visualizar a descrição do produto" OnClick="btnShowProductDescription_Click" Style="height: 18px" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~\App_Themes\GlassCyan\Controls\GridView\img\Add2.gif"
                                    AlternateText="Adicionar o produto à proposta" OnClick="btnAdd_Click" ValidationGroup="Add" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlEdit" runat="server" Visible="false"><br />
                        <br />
                         <textarea plugin="htmlbox" runat="server"  ID="DescriptionTextBox" />
                    </asp:Panel>
                    <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="BudgetItemId,BudgetId,ProductId,ServiceId,UnitCost,UnitPrice,Quantity,SpecialProductName,Observation,ProductDescription,Sector,ModifiedDate,ProductCode,Reference"
                        Width="100%" OnRowDataBound="grdProducts_RowDataBound" Rowselectable="false" OnRowDeleting="grdProducts_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ProductCode" HeaderText="Código" />
                            <asp:BoundField DataField="SpecialProductName" HeaderText="Produto/Serviço" />
                            <asp:BoundField DataField="Quantity" HeaderText="Quantidade" />
                            <asp:BoundField DataField="Reference" HeaderText="Referência" />
                            <asp:BoundField DataField="UnitPrice" HeaderText="Valor Unit(R$)" DataFormatString="{0:F2}" />
                            <asp:BoundField HeaderText="Total por Produto" />
                            <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;">
                                <ItemStyle HorizontalAlign="Left" Width="1px" />
                            </asp:CommandField>
                        </Columns>
                        <FooterStyle Font-Bold="True"></FooterStyle>
                        <EmptyDataTemplate>
                            <center>Não há produtos adicionados na proposta</center>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <table width="100%">
                        <tr>
                            <td>Custo Adicional(R$):<br />
                                <uc1:CurrencyField ID="ucCurrFieldAdditionalCost" Required="false" ValidationGroup="" runat="server" />
                            </td>
                            <td>Desconto:(R$ ou %)<br />
                                <asp:TextBox ID="txtDiscount" Width="56px" MaxLength="6" runat="server" onblur="CalculateDiscount();"></asp:TextBox>
                            </td>
                            <td>Preço Total(R$):&nbsp;&nbsp;
                                <asp:Label ID="lblTotal" runat="server" CssClass="cTxt44b"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%">
                    <tr>
                        <td>Contato:<br />
                            <asp:TextBox ID="txtContactName" MaxLength="50" runat="server" Columns="15"></asp:TextBox><br />
                        </td>
                        <td>Prazo de Entrega:<br />
                            <asp:TextBox ID="txtDeliveryDate" MaxLength="128" runat="server" Columns="10"></asp:TextBox>
                        </td>
                        <td style="vertical-align: middle">Garantia:<br />
                            <asp:TextBox ID="txtWarrant" runat="server" Columns="10" MaxLength="50"></asp:TextBox>&nbsp; <br />
                        </td>
                        <td>Validade da Proposta: <br />
                            <uc1:CurrencyField ID="ucCurrFieldExpirationDate" Mask="9999" runat="server" />
                            (Em Dias)</td>
                        <td>Pagamento:<br />
                            <asp:TextBox ID="txtPaymentMethod" runat="server" Columns="10" MaxLength="50"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>Local de Entrega:<br />
                            <asp:TextBox ID="txtDeliveryDescription" MaxLength="128" runat="server" TextMode="MultiLine" Width="96%"
                                Rows="3"></asp:TextBox><br />
                        </td>
                        <td>Pintura/Tratamento:<br />
                            <asp:TextBox ID="txtTreatment" runat="server" MaxLength="8000" TextMode="MultiLine" Width="96%" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Observação:
                            <asp:TextBox runat="server" ID="txtObservation" Rows="4" MaxLength="2147483647" TextMode="MultiLine"
                                Width="100%"></asp:TextBox>
                            <br />
                            <uc4:Comments ID="prospectComments" runat="server" />
                        </td>
                    </tr>
                </table>
                <div align="right">
                 <table>
                        <tr>
                            <td>Modelo de Proposta: &nbsp;
                                <VFX:HelpTooltip ID="HelpTooltip3" runat="server">
                                    <ItemTemplate>
                                        <h2>Ajuda:</h2>
                                        Escolha aqui o modelo de proposta que você deseja utilizar para este cliente. Para mais informações,
                                        procure por "O que é modelo de documentos?" na página de ajuda (para isso, clique no símbolo de "?"
                                        no canto superior esquerdo da tela).</ItemTemplate>
                                </VFX:HelpTooltip>
                                <asp:DropDownList runat="server" DataSourceID="odsBudgetModels" DataValueField="DocumentTemplateId" DataTextField="FileName"
                                    ID="cboBudgetModels"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnModel" runat="server" Text="Download" ValidationGroup="Save" OnClick="btnModel_Click" />&nbsp&nbsp&nbsp
                            </td>
                            <td>
                                <asp:Button ID="btnSend" runat="server" Text="Enviar para Cliente" ValidationGroup="Save" OnClick="btnSend_Click" />&nbsp&nbsp&nbsp
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" ValidationGroup="Save" Text="Salvar" OnClick="btnSave_Click" />
                                &nbsp&nbsp&nbsp</td>
                            <td>
                                <asp:Button runat="server" ID="btnCancel" Text="Cancelar" OnClientClick="location='Prospects.aspx'; return false;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td class="right">&nbsp;</td>
        </tr>
        <tr class="bottom">
            <td class="left">&nbsp;</td>
            <td class="center"></td>
            <td class="right">&nbsp;</td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource ID="odsBudgetModels" runat="server" SelectMethod="GetDocumentTemplates"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager" OnSelecting="odsBudgetModels_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="documentTemplateTypeId" Type="Int32" />
     </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBudgetItems" runat="server" OnSelecting="odsBudgetItens_Selecting"
        SelectMethod="GetBudgetItemByBudget" DeleteMethod="DeleteBudgetItem" DataObjectTypeName="Vivina.Erp.DataClasses.BudgetItem"
        TypeName="Vivina.Erp.BusinessRules.SaleManager">
        <deleteparameters>
            <asp:ControlParameter ControlID="grdProducts" Name="BudgetItem" PropertyName="SelectedValue" />
     </deleteparameters>
        <selectparameters>
            <asp:Parameter Name="budgetId" Type="Int32" />
            <asp:Parameter Name="CompanyId" Type="Int32" />
     </selectparameters>
    </VFX:BusinessManagerDataSource>
    <script type="text/javascript">

        var txtDiscount = $get('<%=txtDiscount.ClientID %>');

        var txtAdditionalCost = document.getElementById("<%= this.ucCurrFieldAdditionalCost.Controls[0].ClientID %>");
        var lblTotal = $get('<%=lblTotal.ClientID %>');

        var subTotal = $(lblTotal).html().replace(",", ".");
        CalculateDiscount();
    </script>
</asp:Content>
