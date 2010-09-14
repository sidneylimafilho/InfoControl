<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Vivina.Erp.WebUI.Purchasing.CompanyPosPurchase" Title="Ordem de Compra" CodeBehind="Purchase.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="Purchase">
        <table width="100%">
            <tr>
                <td>
                    <asp:Image ID="imgLogo" Width="220px" Height="50px" runat="server" ImageUrl="~/InfoControl/ImageHandler.aspx" />
                </td>
                <td>
                    <asp:Label ID="lblHeader" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Label ID="lblLocalDate" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblPurchaseCode" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblSupplierName" runat="server" Text="" Font-Bold="True"></asp:Label>
        <asp:Label ID="lblCNPJ_CPF" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="lblSupplierAddress" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="lblSupplierLocalization" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="lblSupplierPhone" runat="server" Text=""></asp:Label><br />
        <br />
        <asp:Label ID="lblContactName" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblCover" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblProductText" runat="server" Text="Segue abaixo a lista dos produtos:"></asp:Label>
        <br />
        <br />
        <asp:Repeater ID="rptProduct" runat="server">
            <ItemTemplate>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblProductName" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text='('></asp:Label>
                        <asp:Label ID="Label4" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label>
                        <asp:Label ID="Label3" runat="server" Text=')'></asp:Label>
                    </legend>
                </fieldset>
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <table width="94%">
        </table>
        <br />
        <asp:Label ID="lblProspect" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblSummary" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="lblFooter" runat="server" Text=""></asp:Label>
        <br />
    </div>
</asp:Content>
