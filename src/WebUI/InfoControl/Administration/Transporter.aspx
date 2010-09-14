<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_Transporter" Title="Transportadora"
    CodeBehind="Transporter.aspx.cs" %>

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
            <td class="center" valign="top">
                <uc2:Profile_LegalEntity ID="Profile_LegalEntity1" runat="server" />
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" Visible="False" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" ValidationGroup="Cancel"
                                OnClick="btnCancel_Click" Visible="False" />
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
</asp:Content>
