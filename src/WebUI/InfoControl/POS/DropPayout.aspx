<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_DropPayout" Title="Sangria / Suplemento" CodeBehind="DropPayout.aspx.cs" %>

<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
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
                <div style="float: left; margin-left: 10px">
                    <asp:RadioButton ID="rbtSangria" runat="server" Checked="True" GroupName="DropPayout"
                        Text="Sangria" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rbtSupplement" runat="server" GroupName="DropPayout" Text="Suplemento" />
                </div>
                <br />
                <br />
                <table width="70%">
                    <tr>
                        <td>
                            Valor(R$):<br />
                            <uc1:CurrencyField ID="ucCurrFieldAmount" ValidationGroup="Save" runat="server" />
                        </td>
                        <td>
                        <div runat="server" id="pnlDeposit" visible="false"> 
                        
                            Depósito:
                            <br />
                            <asp:DropDownList runat="server" DataTextField="Name" DataValueField="DepositId"
                                ID="cboDeposit" AutoPostBack="false" DataSourceID="odsDeposits">
                            </asp:DropDownList>
                           <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ID="reqCboDeposit" ErrorMessage="&nbsp&nbsp&nbsp"
                            ControlToValidate="cboDeposit"> 
                           
                           </asp:RequiredFieldValidator>
                           </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Motivo / Descrição:<br />
                            <asp:TextBox ID="txtReason" runat="server" Width="280px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right; margin-right: 10px">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" ValidationGroup="Save" OnClick="btnSave_Click" />
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
    <VFX:BusinessManagerDataSource runat="server" ID="odsDeposits" TypeName="Vivina.Erp.BusinessRules.DepositManager"
        SelectMethod="GetDepositByCompany" onselecting="odsDeposits_Selecting">
        <selectparameters>
         <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
