<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_SelectUser"
    CodeBehind="SelectUser.ascx.cs" %>
<table width="100%">
    <tr>
        <td>
            Selecione o Usuário:<br />
            <asp:TextBox ID="txtUser" runat="server" Width="300px" CssClass="cDynDat11" AutoPostBack="True"
                OnTextChanged="txtUser_TextChanged" MaxLength="100"
                 plugin="autocomplete"
                source='~/InfoControl/SearchService.svc/FindUser'
                 options="{max: 10}">  </asp:TextBox>
        </td>
        <td valign="bottom">           
            &nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
</table>
<asp:Panel ID="pnlUser" runat="server" Visible="false">
    <table border="0" width="100%">
        <tr>
            <td>
                <asp:LinkButton ID="lnkUserName" runat="server"></asp:LinkButton>
                <asp:Label ID="lblSeparator" runat="server" Text="/"></asp:Label>
                <asp:Label ID="lblCPF" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblUserAddress" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblUserLocalization" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblUserPhone" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>

