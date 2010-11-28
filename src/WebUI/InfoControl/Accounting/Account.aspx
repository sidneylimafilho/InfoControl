<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Account" Title="Dados Bancários"
    CodeBehind="Account.aspx.cs" %>

<%@ Register Src="~/App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
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
                                <td>
                                    Banco:<br />
                                    <asp:DropDownList ID="cboBank" runat="server" DataSourceID="odsBank" DataTextField="Name"
                                        DataValueField="BankId">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqBankComboBox" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                         runat="server" ValidationGroup="Save" ControlToValidate="cboBank"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Agência:<br />
                                    <asp:TextBox ID="txtAgency" runat="server" Columns="10"
                                        MaxLength="10" />
                                    <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator2" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                        ControlToValidate="txtAgency" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>                           
                                <td>
                                    Número da Conta:<br />
                                    <asp:TextBox ID="txtAccountNumber" runat="server" 
                                        Columns="10" MaxLength="10" />
                                    <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator3" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                        runat="server" ValidationGroup="Save" ControlToValidate="txtAccountNumber"></asp:RequiredFieldValidator>
                                </td>
                                <tr>
                                <td>
                                    E-mail da Agência:<br />
                                    <asp:TextBox ID="txtAgencyMail" runat="server" Columns="18" MaxLength="18" />
                                    <asp:RegularExpressionValidator CssClass="cErr21"  ID="RegularExpressionValidator1" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                        runat="server" ControlToValidate="txtAgencyMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                          
                                <td>
                                    Telefone da Agência:<br />
                                    <asp:TextBox ID="txtAgencyPhone" runat="server"  Columns="12" MaxLength="12" />
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtAgencyPhone"
                                        ClearMaskOnLostFocus="true" Mask="(99)9999-9999" MaskType="Number">
                                    </ajaxToolkit:MaskedEditExtender>
                                  <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ValidationGroup="Save" ControlToValidate="txtAgencyPhone"
                                  ErrorMessage="&nbsp&nbsp&nbsp"> 
                                  
                                  </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Gerente da Conta:<br />
                                    <asp:TextBox ID="txtAgencyManager" runat="server"    Columns="18" MaxLength="18" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                    <uc1:Address FieldsetTitle="Endereço:" ID="ucAddress" Required="true" runat="server" />
                            </tr>
                        </table>                    
                        <br />
                        <div  align="right">                         
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" onclick="btnSave_Click"  ValidationGroup="Save"></asp:Button>
                            <asp:Button ID="btnCancel" runat="server" OnClientClick="location='Accounts.aspx'; return false;" Text="Cancelar"></asp:Button>
                        </div>
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
    <VFX:BusinessManagerDataSource ID="odsBank" runat="server" SelectMethod="GetAllBanksWithNumbers"
        TypeName="Vivina.Erp.BusinessRules.BankManager" ConflictDetection="CompareAllValues">
    </VFX:BusinessManagerDataSource>   
</asp:Content>
