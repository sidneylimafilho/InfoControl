<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    Inherits="Company_Supplier_General" CodeBehind="Supplier_General.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="../Profile.ascx" TagName="Profile" TagPrefix="uc3" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc2" %>

<%@ Register Src="~/App_Shared/Date.ascx" TagName="Date" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
     <h1> <asp:Literal ID="litTitle" runat="server" Text="Fornecedor" />  </h1>
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
            </td>
            <td class="center">
                <asp:Panel ID="pnlRanking" runat="server">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                Categoria:
                                <asp:DropDownList runat="server" ID="cboSupplierCategory" DataSourceID="odsSupplierCategory"
                                    DataTextField="Name" DataValueField="SupplierCategoryId" Width="100px" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                                Classificação:
                            </td>
                            <td style="width: 100px">
                                <ajaxToolkit:Rating ID="rtnRanking" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                    WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                    ToolTip="Classificação" CurrentRating="3">
                                </ajaxToolkit:Rating>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
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
                                <td>
                                    Cliente Desde:<br /> 
                                       <uc4:Date ID="ucAccountCreatedDate" runat="server" />
                                </td>
                            </tr>                        
                            <tr>
                                <td>
                                    Agência:<br />
                                    <asp:TextBox ID="txtAgency" runat="server" MaxLength="10"></asp:TextBox>
                                </td>
                                <td>
                                    Conta:<br />
                                    <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <uc2:Comments ID="Comments" runat="server" />
                </asp:Panel>
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" ValidationGroup="save" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" ValidationGroup="Cancel"
                        UseSubmitBehavior="false" /></div>
            </td>
            <td class="right">
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
        <VFX:BusinessManagerDataSource ID="odsBank" runat="server" SelectMethod="GetAllBanks"
            TypeName="Vivina.Erp.BusinessRules.BankManager">
        </VFX:BusinessManagerDataSource>
        <VFX:BusinessManagerDataSource ID="odsSupplierCategory" runat="server" SelectMethod="GetSupplierCategory"
            TypeName="Vivina.Erp.BusinessRules.SupplierManager">
        </VFX:BusinessManagerDataSource>
    </table>
</asp:Content>
