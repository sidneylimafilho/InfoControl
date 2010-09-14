<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Inventory_General.aspx.cs" Inherits="Vivina.Erp.WebUI.POS.Inventory_General" %>
    
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>

<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <table>
                    <tr>
                        <td>
                            Produto:<br />
                            <b>
                                <asp:Label ID="lblProductName" runat="server"></asp:Label></b>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td valign="top">
                            Nº da Nota:<br />
                            <asp:TextBox ID="txtFiscalNumber" MaxLength="50" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td valign="top">
                            Estoque Físico:<br />
                            <asp:DropDownList ID="cboDeposit" Enabled="False" runat="server" DataSourceID="odsDeposit"
                                DataTextField="Name" DataValueField="DepositId" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
        </tr>
        <td valign="top">
            Fornecedor:<br />
            <asp:DropDownList ID="cboSupplier" runat="server" DataSourceID="odsSupplier" DataTextField="Name"
                DataValueField="SupplierId">
            </asp:DropDownList>
        </td>
        </tr>
    </table>
    <table>
        </tr>
        <td valign="top">
            Moeda:<br />
            <asp:DropDownList ID="cboCurrencyRate" runat="server" DataSourceID="odsCurrencyRate"
                DataTextField="Name" DataValueField="CurrencyRateId">
            </asp:DropDownList>
        </td>
        </tr>
    </table>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td valign="top">
                Custo Médio (R$):<br />
                <b>
                    <asp:Label ID="lblAverageCost" runat="server"></asp:Label></b>
            </td>
            <td valign="top">
                Quantidade neste depósito:<br />
                <b>
                    <asp:Label ID="lblQuantity" runat="server"></asp:Label></b>
            </td>
            <td valign="top">
                Quantidade em Reserva:<br />
                <b>
                    <asp:Label ID="lblQuantityInReserve" runat="server"></asp:Label></b>
            </td>
            <td valign="top">
                Quantidade Mínima Requirida:<br />
                <uc3:currencyfield id="ucCurrFieldMinimunRequired" columns="4" mask="9999" runat="server" />
            </td>
        </tr>   
        <tr>
            <td>
                Localização:<br />
                <asp:TextBox ID="txtLocalization" MaxLength="200" runat="server" />
            </td>
            <td>
                Preço de Custo:
                <br />
                <uc3:currencyfield id="uctxtRealCost" runat="server" />
            </td>
            <td>
                Margem de Lucro(%):
                <br />
                <uc3:currencyfield id="uctxtProfit" maxlength="6" runat="server" />
            </td>
            <td>
                <asp:Label ID="lblUnitPrice1" runat="server"></asp:Label><br />
                <uc3:currencyfield id="uctxtUnitPrice1" runat="server" />
            </td>
        </tr>  
        <tr>
            <td>
                <asp:Panel ID="pnlUnitPrice2" runat="server">
                    <asp:Label ID="lblUnitPrice2" runat="server"></asp:Label><br />
                    <uc3:currencyfield id="uctxtUnitPrice2" runat="server" />
                </asp:Panel>
            </td>           
            <td>
                <asp:Panel ID="pnlUnitPrice3" runat="server">
                    <asp:Label ID="lblUnitPrice3" runat="server"></asp:Label><br />
                    <uc3:currencyfield id="uctxtUnitPrice3" runat="server" />
                </asp:Panel>
            </td>
           
            <td>
                <asp:Panel ID="pnlUnitPrice4" runat="server">
                    <asp:Label ID="lblUnitPrice4" runat="server"></asp:Label><br />
                    <uc3:currencyfield id="uctxtUnitPrice4" runat="server" />
                </asp:Panel>
            </td>         
            <td>
                <asp:Panel ID="pnlUnitPrice5" runat="server">
                    <asp:Label ID="lblUnitPrice5" runat="server"></asp:Label><br />
                    <uc3:currencyfield id="uctxtUnitPrice5" runat="server" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td align="right">
                <asp:Button ID="btnPrintBarCode" runat="server" CssClass="cBtn11" Text="Imprimir Código de Barras"
                    permissionRequired="Inventory">
                </asp:Button>&nbsp;&nbsp;&nbsp;  
                <asp:Button ID="btnSave" runat="server" CssClass="cBtn11" Text="Salvar" permissionRequired="Inventory"
                    OnClick="btnSave_Click"></asp:Button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" CssClass="cBtn11" Text="Cancelar" permissionRequired="Inventory"
                    OnClick="btnCancel_Click"></asp:Button>
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
    
    <VFX:BusinessManagerDataSource ID="odsSupplier" runat="server" OnSelecting="odsSupplier_Selecting"
        SelectMethod="getNames" TypeName="Vivina.Erp.BusinessRules.SupplierManager">
        <selectparameters>
            <asp:Parameter Name="matrixId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCurrencyRate" runat="server" SelectMethod="GetAllCurrencyRates"
        TypeName="Vivina.Erp.BusinessRules.CurrencyRateManager" onselecting="odsCurrencyRate_Selecting">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" OnSelecting="odsDeposit_Selecting"
        SelectMethod="GetDepositByCompany" TypeName="Vivina.Erp.BusinessRules.DepositManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>

    <script language="javascript" type="text/javascript">
        var uctxtRealCost = document.getElementById("<%= this.uctxtRealCost.Controls[0].ClientID %>");

        var uctxtProfit = document.getElementById("<%= this.uctxtProfit.Controls[0].ClientID %>");
        var uctxtUnitPrice1 = document.getElementById("<%= this.uctxtUnitPrice1.Controls[0].ClientID %>");
        var uctxtUnitPrice2 = document.getElementById("<%= this.uctxtUnitPrice2.Controls[0].ClientID %>");
        var uctxtUnitPrice3 = document.getElementById("<%= this.uctxtUnitPrice3.Controls[0].ClientID %>");
        var uctxtUnitPrice4 = document.getElementById("<%= this.uctxtUnitPrice4.Controls[0].ClientID %>");
        var uctxtUnitPrice5 = document.getElementById("<%= this.uctxtUnitPrice5.Controls[0].ClientID %>");

        if (uctxtUnitPrice1 != null) {
            Sys.UI.DomEvent.addHandler(txtProfit, "blur", CalculatePrice);
            //Sys.UI.DomEvent.addHandler(txtPrice, "blur", CalculateProfit);
        }

        function FixCulture(control) {
            var value = control.value.replace(".", "").replace(",", ".").replace("_", "");
            value = parseFloat(value);
            return value;
        }

        function CalculatePrice() {
            var value = ((uctxtProfit / 100) + 1) * uctxtRealCost;

            uctxtUnitPrice1.value = parseFloat(value).localeFormat("N");
            uctxtUnitPrice2.value = parseFloat(value).localeFormat("N");
            uctxtUnitPrice3.value = parseFloat(value).localeFormat("N");
            uctxtUnitPrice4.value = parseFloat(value).localeFormat("N");
            uctxtUnitPrice5.value = parseFloat(value).localeFormat("N");
        }

        function CalculateProfit() {
            var value = Math.ceil((txtPrice / txtRealCost) * 100) - 100;
            uctxtProfit.value = parseFloat(value).localeFormat("N");
        }
    </script>
</asp:Content>
