<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_SelectSupplier"
    CodeBehind="SelectSupplier.ascx.cs" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<table id="pnlSupplierSearch" class="pnlSupplierSearch" runat="server">
    <tr>
        <td>
            Fornecedor:<br />
            <asp:TextBox ID="txtSupplier" runat="server" Width="250px" CssClass="cDynDat11" 
                OnTextChanged="txtSupplier_TextChanged" AutoPostBack="true"                 
                plugin="autocomplete"
                source='~/InfoControl/SearchService.svc/FindSuppliers'
                 options="{max: 10}"
                  MaxLength="100"> </asp:TextBox>
            <p style="font-size: 7pt; color: gray">
                Dica: Digite parte do texto, que o completará automaticamente!</p>
        </td>
        <td>
            <img src="~/App_Themes/_global/Company/user_add.gif" alt="Inserir Fornecedor" border="0"
                runat="server" style="cursor:pointer" plugin="lightbox" source='Administration/Supplier_General.aspx?w=modal&lightbox[iframe]=true' />&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
</table>
<asp:Panel ID="pnlSupplier" runat="server" Visible="false">
    <table>
        <tr>
            <td>
                <b>
                    <a plugin="lightbox" href="Administration/Supplier.aspx?w=modal&SupplierId=<%=Supplier.SupplierId.EncryptToHex() %>&lightbox[iframe]=true">
                    <%=Supplier.Name%></a></b><br />
                <asp:Label ID="lblCNPJ" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblSupplierAddress" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblSupplierLocalization" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblSupplierPhone" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;<img src="<%=ResolveUrl("~/App_Themes/_global/p_univ.gif")%>"
                    onclick="$('#<%=pnlSupplierSearch.ClientID %>').show(); " />
            </td>
        </tr>
    </table>
</asp:Panel>


