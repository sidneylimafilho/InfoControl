<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_InventoryPrice" Title="Ajuste de Valores do Inventário" Codebehind="InventoryPrice.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="50%">
        <tr class="top">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                &nbsp;</td>
            <td class="right">
                &nbsp;</td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Produto:" Font-Bold="true"></asp:Label><br />
                            <asp:Label ID="lblProductName" runat="server"></asp:Label><br />
                            <br />
                        </td>
                        <td>
                            <asp:Label Font-Bold="true" ID="Label6" runat="server" Text="Código:"></asp:Label><br />
                            <asp:Label ID="lblProductCode" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Font-Bold="true" ID="Label7" runat="server" Text="Quantidade:"></asp:Label><br />
                            <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Custo Médio:" Font-Bold="true"></asp:Label><br />
                            <asp:Label ID="lblAverageCost" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Margem de Lucro:" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtProfitMargin" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Preço de Venda:" Font-Bold="true"></asp:Label><br />
                            <asp:TextBox ID="txtUnitPrice" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />&nbsp;&nbsp;
                </div>
            </td>
            <td class="right">
                &nbsp;</td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                &nbsp;</td>
            <td class="right">
                &nbsp;</td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
    var averageCost = $get("<%= this.lblAverageCost.ClientID %>");
    var profitMargin = $get("<%= this.txtProfitMargin.ClientID %>");
    var unitPrice = $get("<%= this.txtUnitPrice.ClientID %>");
    
    if (unitPrice != null)
    {
    $addHandler(profitMargin, "blur", CalculatePrice);
    $addHandler(unitPrice, "blur", CalculateProfit);
    }
    
    function FixCulture(value)
    {   
        return parseFloat(value.replace(".", "").replace(",", ".").replace("_", ""));       
    }
    function CalculatePrice()
    {   
        var value = ((FixCulture(profitMargin.value) / 100) + 1) * FixCulture(averageCost.innerHTML) ;        
        unitPrice.value =  parseFloat(value).localeFormat("N");                
    }
    function CalculateProfit()
    {       
        var value = Math.ceil((FixCulture(unitPrice.value) / FixCulture(averageCost.innerHTML))*100) - 100;        
        profitMargin.value = parseFloat(value).localeFormat("N");        
    }
    </script>

</asp:Content>
