<%@ Page Language="C#" AutoEventWireup="true" Inherits="Vivina.Erp.WebUI.Administration.Product_General"
    MasterPageFile="~/infocontrol/Default.master" Title="Cadastro de Produto" CodeBehind="Product_General.aspx.cs" %>

<%@ Register Src="../../App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc3" %>
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
                                        <asp:RequiredFieldValidator ID="valCboCategories" runat="server" ControlToValidate="cboCategories"
                                            ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="Product"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            Fabricante:<br />
                            <asp:TextBox ID="txtManufacturer" Width="250px" runat="server" CssClass="cDynDat11" MaxLength="50"
                             plugin="autocomplete" source='~/InfoControl/SearchService.svc/FindManufacturer'
                                   options="{max: 10}">
                             </asp:TextBox>
                            
                            <p style="font-size: 7pt; color: gray">
                                Dica: Digite parte do texto, que o completará automaticamente!</p>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Código do Produto:<br />
                            <asp:TextBox ID="txtProductCode" runat="server" MaxLength="128" />
                            <asp:RequiredFieldValidator ID="reqtxtProductCode" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtProductCode" CssClass="cErr21" ValidationGroup="Product"></asp:RequiredFieldValidator>
                        </td>
                       </tr>
                           </table>
                           
                            <fieldSet>
                                <legend style="cursor: pointer" onclick="$('#tableAditionalData').show('slow')"> Outras Informações </legend>
                                        
                                     <table id="tableAditionalData" style="display: none">
                                            <tr>
                                                <td>
                                                
                                                    Tipo de Código de Barra:<br />
                                                    <asp:DropDownList ID="cboBarCodeType" runat="server" DataSourceID="odsBarCode" DataTextField="BarCodeName"
                                                        DataValueField="BarCodeTypeId" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Código de barras:<br />
                                                    <asp:TextBox ID="txtBarCode" runat="server" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="reqtxtBarCode" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;"
                                                        runat="server" ControlToValidate="txtBarCode" CssClass="cErr21" ValidationGroup="Product"></asp:RequiredFieldValidator>
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
                                                <td>
                                                    Identificação/Placa:<br />
                                                    <asp:TextBox ID="txtIdentificationOrPlaca" MaxLength="50" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Patrimônio/Renavam:<br />
                                                    <asp:TextBox ID="txtPatrimonioOrRenavam" MaxLength="50" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
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
                                                    Peso:<br />
                                                    <uc1:CurrencyField ID="ucCurrFieldWheight"  runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Dias de Garantia:<br />
                                                    <uc1:CurrencyField ID="ucCurrFieldWarrantyDays" Mask="9999" runat="server" />
                                                </td>
                                                <td>
                                                    Unidade:<br />
                                                    <asp:TextBox runat="server" ID="txtUnit" MaxLength="20"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Embalagem:
                                                    <br />
                                                    <asp:TextBox runat="server" MaxLength="20" ID="txtPackage"> </asp:TextBox>
                                                </td>
                                            </tr>                                           
                                     </table>
                             </fieldSet>  
                                                                                            
                  <table>
                       <tr>
                            <td colspan="4">
                               Descrição:<br />
                                  <textarea plugin="htmlbox" runat="server"  ID="DescriptionTextBox" />
                                  
                            </td>
                       </tr>
                    <tr>
                        <td colspan="3">
                            Palavra Chave:<br />
                            <asp:TextBox runat="server" ID="txtKeyWord" Rows="2" Columns="140" MaxLength="2147483647"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkIsActive" runat="server" Text="&nbsp;Produto Ativo?" Checked="true" />
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkDropCompositeInStock" runat="server" Text="&nbsp;Baixa os ítens da composição do estoque?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkIsCasting" runat="server" Text="&nbsp;Lançamento?" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAddCustomerEquipment" runat="server" Text="&nbsp;Quando vender, adiciona equipamento ao cliente?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkIsEmphasizeInHome" runat="server" Text="&nbsp;Destaque na Home?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkAllowNegativeStock" runat="server" Text="&nbsp;O estoque deste produto pode ficar negativo?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkAllowSaleBelowCost" Text="&nbsp;Permitir que este produto seja vendido abaixo do custo?"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkRequiresAuthorization" Checked="false" runat="server" Text="&nbsp;Requer autorização?" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button runat="server" ID="btnApproveProduct" Text="Aprovar" OnClick="btnSave_Click"
                                ValidationGroup="Product" Visible="false" />
                            <asp:Button ID="btnSaveAndNew" ValidationGroup="Product" runat="server" Text="Salvar Novo"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSave" ValidationGroup="Product" runat="server" Text="Salvar" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" UseSubmitBehavior="false"
                                OnClick="btnCancel_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsCategories" runat="server" SelectMethod="GetCategoriesByCompanyAsDataTable"
        TypeName="Vivina.Erp.BusinessRules.CategoryManager" OnSelecting="odsCategories_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBarCode" runat="server" TypeName="Vivina.Erp.BusinessRules.BarCodeTypeManager"
        SelectMethod="GetAllBarCodeTypes">
    </VFX:BusinessManagerDataSource>
</asp:Content>
