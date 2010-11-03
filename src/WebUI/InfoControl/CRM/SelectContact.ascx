<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectContact.ascx.cs"
    Inherits="Vivina.Erp.WebUI.InfoControl.CRM.SelectContact" %>
<table id="pnlContactSearch" class="pnlContactSearch" runat="server">
    <tr>
        <td>
            Contato:<br />
            <asp:TextBox ID="txtContact" runat="server" Width="250px" CssClass="cDynDat11" AutoPostBack="True"
                OnTextChanged="txtContact_TextChanged" plugin="autocomplete" source='~/InfoControl/SearchService.svc/FindContacts'
                options="{max: 10}" MaxLength="100"></asp:TextBox>
            <p style="font-size: 7pt; color: gray">
                Dica: Digite parte do texto, que o mesmo será completado automaticamente!</p>
        </td>
        <td>
            <img id="Img1" src="~/app_themes/_global/company/user_add.gif" runat="server" alt="Inserir Contato"
                border="0" class="noprintable" style="cursor: pointer" onclick="top.$.lightbox('Administration/Contact.aspx?w=true&lightbox[iframe]=true');" />
        </td>
    </tr>
</table>
<asp:Panel ID="pnlContact" runat="server" Visible="false">
    <table border="0">
        <tr>
            <td>
                <b>
                    <asp:LinkButton ID="lnkContactName" runat="server">  </asp:LinkButton>
                </b>
                <br />
                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label><br />
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <img src="~/App_Themes/_global/p_univ.gif" id="imgUnselect" onclick="$('.pnlContactSearch').show(); "
                    runat="server" />
            </td>
        </tr>
    </table>
 
</asp:Panel>
