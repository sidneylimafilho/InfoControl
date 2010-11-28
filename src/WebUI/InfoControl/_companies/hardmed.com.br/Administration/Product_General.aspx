<%@ Page Language="C#" AutoEventWireup="true" Inherits="Vivina.Erp.WebUI.Administration.Product_General"
    EnableEventValidation="false" MasterPageFile="~/infocontrol/Default.master" Title="Cadastro de Produto" %>

<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc3" %>
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
                <table>
                    <tr>
                        <td valign="top">
                            <uc3:SelectProduct ID="txtProduct" runat="server" Required="true" ValidationGroup="Product" />
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        Escolha a Categoria:
                                        <uc2:ComboTreeBox ID="cboCategories" runat="server" DataFieldID="CategoryId" DataFieldParentID="ParentId"
                                            DataTextField="Name" DataValueField="CategoryId" DataSourceID="odsCategories" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator CssClass="cErr21" ID="valCboCategories" runat="server" ControlToValidate="cboCategories"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="Product"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            Fabricante:<br />
                            <asp:TextBox ID="txtManufacturer" runat="server" CssClass="cDynDat11" MaxLength="50" />
                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtManufacturer"
                                CompletionInterval="1000" FirstRowSelected="True" MinimumPrefixLength="3" ServiceMethod="SearchManufacturer"
                                source="~/InfoControl/SearchService.asmx">
                            </ajaxToolkit:AutoCompleteExtender>
                            <p style="font-size: 7pt; color: gray">
                                Dica: Digite parte do texto, que o completará automaticamente!</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Código do Produto:<br />
                            <asp:TextBox ID="txtProductCode" runat="server" MaxLength="128" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtProductCode" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtProductCode"  ValidationGroup="Product"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Tipo de Código de Barra:<br />
                            <asp:DropDownList ID="cboBarCodeType" runat="server" DataSourceID="odsBarCode" DataTextField="BarCodeName"
                                DataValueField="BarCodeTypeId" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Código de barras:<br />
                            <asp:TextBox ID="txtBarCode" runat="server" MaxLength="128" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtBarCode" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                runat="server" ControlToValidate="txtBarCode"  ValidationGroup="Product"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Código alternativo:<br />
                            <asp:TextBox ID="txtIdentificationOrPlaca" MaxLength="50" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Dias de Garantia:<br />
                            <uc1:CurrencyField ID="ucCurrFieldWarrantyDays" Mask="9999" runat="server" />
                        </td>
                        <td style="display: none">
                            Patrimônio/Renavam:<br />
                            <asp:TextBox ID="txtPatrimonioOrRenavam" MaxLength="50" runat="server"></asp:TextBox>
                        </td>
                        <td style="display: none">
                            Nº Serial/Chassi:<br />
                            <asp:TextBox ID="txtSerialNumberOrChassi" MaxLength="50" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            IPI:<br />
                            <uc1:CurrencyField ID="ucCurrFieldIPI" MaxLength="6" runat="server" />
                        </td>
                        <td>
                            ICMS:<br />
                            <uc1:CurrencyField ID="ucCurrFieldICMS" MaxLength="6" runat="server" />
                        </td>
                        <td>
                            Classe Fiscal:<br />
                            <asp:TextBox ID="txtFiscalClass" runat="server" Columns="14"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="msktxtFiscalClass" ClearMaskOnLostFocus="true"
                                runat="server" InputDirection="RightToLeft" Mask="9999.99.9999" MaskType="Number"
                                TargetControlID="txtFiscalClass">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            Descrição:<br />
                             <textarea plugin="htmlbox" runat="server"  ID="DescriptionTextBox" />
                        </td>
                        <asp:TextBox runat="server" ID="txtKeyWord" Visible="false"> </asp:TextBox>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkIsActive" runat="server" Text="&nbsp;Produto Ativo ?" Checked="true" />
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkDropCompositeInStock" runat="server" Text="&nbsp;Baixa os ítens da composição do estoque ?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkAddCustomerEquipment" runat="server" Text="&nbsp;Quando vender, adiciona equipamento ao cliente ?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkAllowNegativeStock" runat="server" Text="&nbsp;O estoque deste produto pode ficar negativo?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkAllowSaleBelowCost" Text="Permitir que este produto seja vendido abaixo do custo?"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkRequiresAuthorization" Checked="false" Visible="false" runat="server"
                                Text="Requer autorização?" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSaveAndNew" ValidationGroup="Product" runat="server" Text="Salvar Novo"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSave" ValidationGroup="Product" runat="server" Text="Salvar" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" UseSubmitBehavior="false"
                                OnClientClick="location='products.aspx'; return false;" />
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
    <VFX:BusinessManagerDataSource ID="odsCategories" runat="server" SelectMethod="GetCategoriesByCompany"
        TypeName="Vivina.Erp.BusinessRules.CategoryManager" OnSelecting="odsCategories_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBarCode" runat="server" TypeName="Vivina.Erp.BusinessRules.BarCodeTypeManager"
        SelectMethod="GetAllBarCodeTypes">
    </VFX:BusinessManagerDataSource>
</asp:Content>
