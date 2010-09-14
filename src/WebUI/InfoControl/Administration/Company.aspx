<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Users_Register" Title="Empresa" CodeBehind="Company.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="../Profile_LegalEntity.ascx" TagName="Profile_LegalEntity" TagPrefix="uc2" %>
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
                <uc2:Profile_LegalEntity ID="Profile_LegalEntity1" ValidationGroup="SaveCompany"
                    runat="server" />
                <asp:Panel ID="pnlOtherData" runat="server">
                    Selecione a Empresa Matriz:<br />
                    <asp:DropDownList ID="cboMatrixId" runat="server" AppendDataBoundItems="True" DataTextField="CompanyName"
                        DataValueField="CompanyId">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>             
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnSave" runat="server" ValidationGroup="SaveCompany" CausesValidation="True"
                        CommandName="Update" CssClass="cBtn11" Text="Salvar" permissionRequired="Companies"
                        OnClick="btnSave_Click"></asp:Button>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="cBtn11" Text="Cancelar" OnClick="CancelButton_Click"></asp:Button></div>
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
    <VFX:BusinessManagerDataSource ID="odsCnae" runat="server" SelectMethod="GetAllCnae"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsJudicialNature" runat="server" SelectMethod="GetAllJudicialNature"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsProfitAssessment" runat="server" SelectMethod="GetAllProfitAssessment"
        TypeName="Vivina.Erp.BusinessRules.CompanyManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
