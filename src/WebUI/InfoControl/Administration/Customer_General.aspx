<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Customer_General"
    CodeBehind="Customer_General.aspx.cs" Title="" MasterPageFile="~/infocontrol/Default.master" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../Profile.ascx" TagName="Profile" TagPrefix="uc3" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div style="width: 100%">
        <h1>
            <asp:Literal runat="server" ID="litTitle" Text="Cliente" />
        </h1>
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
                    <asp:Panel ID="pnlRanking" runat="server">
                        <table style="right: 30px; position: absolute;">
                            <tr>
                                <td runat="server" id="pnlCreateDate" visible="False">
                                    Criado em:
                                    <asp:Label ID="lblCreatedDate" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    Tipo de Cliente:&nbsp;
                                    <asp:DropDownList ID="cboCustomerType" runat="server" AppendDataBoundItems="True"
                                        DataSourceID="odsCustomerType" DataTextField="Name" DataValueField="CustomerTypeId">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    Classificação:
                                </td>
                                <td align="right">
                                    <ajaxToolkit:Rating ID="rtnRanking" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                        WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                        ToolTip="Classificação" CurrentRating="3">
                                    </ajaxToolkit:Rating>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <uc3:Profile ID="ucProfile" ValidationGroup="save" runat="server" />
                    <br />
                  
                    <fieldset>
                        <legend>Dados de Usuário:</legend>
                        <table>
                            <tr>
                                <td>
                                    Email de Usuário:
                                    <br />
                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="50">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="ValEmail" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;"
                                        ControlToValidate="txtUserName" ValidationGroup="save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPassword" Text="Senha:" />
                                    <br />
                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="256" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" Visible="false" ID="chkRemoveUser" Text="Remover conta de Usuário deste cliente" />
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblMessage" runat="server"> </asp:Label>
                    </fieldset>
                    <asp:Panel ID="pnlOtherData" runat="server" Visible="false">
                        <fieldset>
                            <legend style="cursor: pointer" onclick="$('#CommercialData').show('slow')">Dados Comerciais:</legend>
                            <table id="CommercialData" style="display: none">
                                <tr>
                                    <td>
                                        Vendedor:<br />
                                        <asp:Panel ID="pnlShowVendor" runat="server" Visible="true">
                                            <asp:Label ID="lblVendor" runat="server"></asp:Label>&nbsp;&nbsp;<img id="imgUndoSelectedEmployee"
                                                alt="Desfazer" src="../../App_Themes/_global/undo.gif" onclick="EditVendor()" /></asp:Panel>
                                        <asp:DropDownList ID="cboVendors" runat="server" DataSourceID="odsSalesPerson" DataTextField="Name"
                                            DataValueField="EmployeeId" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Comissão:<br />
                                        <uc1:CurrencyField ID="ucVendorComission" runat="server" />
                                        <%--  <asp:TextBox ID="txtVendorComission" runat="server"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="msktxtVendorComission" runat="server" InputDirection="RightToLeft"
                                        Mask="99.99" MaskType="Number" TargetControlID="txtVendorComission" ClearMaskOnLostFocus="true">
                                    </ajaxToolkit:MaskedEditExtender>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Vendedor adicional:<br />
                                        <asp:Panel ID="pnlShowSupplementalVendor" runat="server">
                                            <asp:Label ID="lblSupplementalVendor" runat="server"></asp:Label>&nbsp;&nbsp;<img
                                                id="imgUndoSelectedSupplementalEmployee" alt="Desfazer" src="../../App_Themes/_global/undo.gif"
                                                onclick="EditSupplementalVendor()" /></asp:Panel>
                                        <asp:DropDownList ID="cboSupplementalVendor" runat="server" DataSourceID="odsSalesPerson"
                                            DataTextField="Name" DataValueField="EmployeeId" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Comissão:<br />
                                        <uc1:CurrencyField ID="ucSupplementalVendorComission" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                      Limite de Crédito: <br>
                                        <uc1:CurrencyField ID="ucCurrFieldCreditLimit" runat="server" />
                                        &nbsp 
                                        <VFX:HelpTooltip ID="HelpTooltip1" runat="server">
                                            <itemtemplate>
                                               <h2>Ajuda:</h2>
                                                 Quando este valor for maior que 0(zero), permite que a forma de pagamento do cliente seja do tipo fatura,
                                                 ou seja, esse cliente não será obrigado a pagar imediatamente no ato de compra. 
                                                 Indica até que valor pode-se efetuar uma venda a crédito!
                                             </itemtemplate>
                                        </VFX:HelpTooltip>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <fieldset>
                            <legend style="cursor: pointer" onclick="$('#AccountData').show('slow')">Dados Bancários:</legend>
                            <table id="AccountData" style="display: none">
                                <tr>
                                    <td>
                                        Banco:<br />
                                        <asp:DropDownList ID="cboBank" runat="server" DataSourceID="odsBank" DataTextField="Name"
                                            DataValueField="BankId" AppendDataBoundItems="true">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Agência:<br />
                                        <asp:TextBox ID="txtAgency" runat="server" MaxLength="10" Columns="10"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Conta:<br />
                                        <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10" Columns="10"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Cliente desde:<br />
                                        <asp:TextBox ID="txtAccountCreatedDate" runat="server" Columns="10"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="mskTxtAccountCreatedDate" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtAccountCreatedDate">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <asp:CompareValidator ID="cmpTxtAccountCreatedDate" runat="server" ControlToValidate="txtAccountCreatedDate"
                                            ValueToCompare="1/1/1753" Operator="GreaterThanEqual" CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"
                                            Type="Date" ValidationGroup="save">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <br />
                    
                    <fieldset> 
                     <legend>Outras informações</legend>
                      <table> 
                       <tr> 
                         <td>   
                           Representante: <br>
                             <asp:DropDownList runat="server" ID="cboRepresentant" DataSourceID="odsRepresentant" DataTextField="Name"
                                DataValueField="RepresentantId" AppendDataBoundItems="true">                  
                                 <asp:ListItem Text="" Value="" ></asp:ListItem>
                           </asp:DropDownList>
                         </td>
                       </tr>
                      </table>
                    </fieldset>
                    <uc2:Comments ID="customerComments" runat="server" />
                    <div style="text-align: right;">
                        <asp:Button ID="btnSave" runat="server" Text="Salvar" ValidationGroup="save" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" ValidationGroup="Cancel" /></div>
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
    </div>
    <VFX:BusinessManagerDataSource ID="odsSalesPerson" runat="server" SelectMethod="GetSalesPerson"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" OnSelecting="odsSalesPerson_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBank" runat="server" SelectMethod="GetAllBanksWithNumbers"
        TypeName="Vivina.Erp.BusinessRules.BankManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCustomerType" runat="server" OnSelecting="odsCustomerType_Selecting"
        SelectMethod="GetAllCustomerType" TypeName="Vivina.Erp.BusinessRules.CustomerManager">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsRepresentant" runat="server" TypeName="Vivina.Erp.BusinessRules.RepresentantManager"
        SelectMethod="GetRepresentantsByCompany" onselecting="odsRepresentant_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
         </selectparameters>
    </VFX:BusinessManagerDataSource>
    <script type="text/javascript">
        function EditVendor() {
            var cboVendors = $get('<%=cboVendors.ClientID %>');
            cboVendors.style.display = 'block';

        }

    </script>
</asp:Content>
