<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Representant" Title="Representante"
    CodeBehind="Representant.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="../Profile.ascx" TagName="Profile" TagPrefix="uc3" %>
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
                <table width="100%" id="pnlRating" runat="server" style="position: absolute;">
                    <tr>
                    <td align="right"> 
                    <asp:Label runat="server" ID="lblRepresentantCode" Visible="false"></asp:Label>                    
                    </td>
                        <td align="right">
                            Classificação:
                        </td>
                        <td width="100">
                            <ajaxToolkit:Rating ID="rtnRating" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                ToolTip="Classificação" CurrentRating="3">
                            </ajaxToolkit:Rating>
                        </td>
                    </tr>
                </table>
                <uc3:Profile ID="Profile1" ValidationGroup="save" runat="server" />
                <br />
                <asp:Panel ID="pnlAccount" runat="server">
                    <fieldset>
                        <legend>Dados Bancários:</legend>
                        <table width="70%">
                            <tr>
                                <td>
                                    Banco:<br />
                                    <asp:DropDownList ID="cboBank" runat="server" DataSourceID="odsBank" DataTextField="Name"
                                        DataValueField="BankId" AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    Agência:<br />
                                    <asp:TextBox ID="txtAgency" runat="server" MaxLength="10" Columns="10"></asp:TextBox>
                                </td>
                                <td>
                                    Conta:<br />
                                    <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10" Columns="10"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" ValidationGroup="save" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='Representants.aspx'; return false;" />                    
                 </div>
                    
                <VFX:BusinessManagerDataSource ID="odsBank" runat="server" SelectMethod="GetAllBanks"
                    TypeName="Vivina.Erp.BusinessRules.BankManager">
                </VFX:BusinessManagerDataSource>
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
