<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_SelectCustomer"
    CodeBehind="SelectCustomer.ascx.cs" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<table id="pnlCustomerSearch" class="pnlCustomerSearch" runat="server">
    <tr>
        <td>
            Cliente:<br />
            <asp:TextBox ID="txtCustomer" runat="server" Width="250px" CssClass="cDynDat11" AutoPostBack="True"
                OnTextChanged="txtCustomer_TextChanged" plugin="autocomplete" 
                source='~/InfoControl/SearchService.svc'
                action='FindCustomers'
                options="{max: 10}" MaxLength="100"></asp:TextBox>
            <p style="font-size: 7pt; color: gray">
                Dica: Digite parte do texto, que o completará automaticamente!</p>
        </td>
        <td valign="bottom">
            <br />
            <img src="~/App_Themes/_global/Company/user_add.gif" runat="server" alt="Inserir Cliente"
                border="0" class="noPrintable" source="Administration/Customer_General.aspx?w=modal"
                plugin="lightbox" />
            <br />
        </td>
    </tr>
</table>
<asp:Panel ID="pnlCustomer" runat="server" Visible="false">
    <table border="0">
        <tr>
            <td>
                <b><a plugin="lightbox" href="Administration/Customer.aspx?CustomerId=<%=Customer.CustomerId.EncryptToHex() %>&lightbox[iframe]=true">
                    <%=Customer.Name  %></a></b>
                <br />
                <asp:Label ID="lblCNPJ" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblCustomerAddress" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblCustomerLocalization" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblCustomerPhone" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <img src="~/App_Themes/_global/p_univ.gif" id="imgUnselect" onclick="$('.pnlCustomerSearch').show(); "
                    runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>
