<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Company_Stock_Deposit" CodeBehind="StockDeposit.aspx.cs"
    Title="Entrada no Estoque" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc3" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
        <table class="cLeafBox21" width="100%">
            <tr class="top">
                <td class="left">
                    &nbsp;
                </td>
                <td class="center">
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Orange" Visible="False"></asp:Label>
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
                                Data de entrada<br />
                                <uc3:Date ID="ucEntryDate" runat="server" />
                            </td>
                            <td>
                                Nº da Nota
                                <br>
                                <asp:TextBox ID="txtFiscalNumber" MaxLength="50" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Fornecedor:<br />
                                <asp:DropDownList ID="cboSupplier" runat="server" DataSourceID="odsSupplier" DataValueField="SupplierId"
                                    Width="200px" DataTextField="Name" AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    runat="server" ControlToValidate="cboSupplier" CssClass="cErr21" ValidationGroup="Outside"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Estoque Físico:<br />
                                <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                    DataValueField="DepositId" Width="100px" AppendDataBoundItems="true">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    runat="server" ControlToValidate="cboDeposit" CssClass="cErr21" ValidationGroup="Outside"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Moeda:<br />
                                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="odsCurrencyRate"
                                    DataTextField="Name" DataValueField="CurrencyRateId" Width="100px">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    runat="server" ControlToValidate="DropDownList1" CssClass="cErr21" ValidationGroup="Outside"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <fieldset>
                        <legend>Produto:</legend>
                        <table>
                            <tr>
                                <td>
                                    <uc2:SelectProduct ID="selProduct" runat="server" ValidationGroup="addProduct" Required="true" />
                                </td>
                                <td>
                                    Quantidade:<br />
                                    <uc4:CurrencyField ID="ucCurrFieldQuantity" Mask="9999" ValidationGroup="addProduct"
                                        Required="true" runat="server" /><br /><br />
                                </td>
                                <td>
                                    Custo:<br />
                                    <uc4:CurrencyField ID="uctxtRealCost" Title="Custo"  ValidationGroup="addProduct"
                                        runat="server" /><br /><br />
                                </td>
                                <td>
                                    Lucro:
                                    <br />
                                    <uc4:CurrencyField ID="uctxtProfit" MaxLength="6" ValidationGroup="addProduct" runat="server" />
                                    <br /><br />
                                </td>
                                <td>
                                    Localização:
                                    <br />
                                    <asp:TextBox ID="txtLocalization" runat="server" MaxLength="25">
                                    </asp:TextBox><br /><br />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table>
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="pnlUnitPrice1" runat="server">
                                    <asp:Label ID="lblPriceDefault" runat="server" Text="Preço de Venda:"> </asp:Label>
                                    <asp:Label ID="lblUnitPrice1" runat="server"></asp:Label><br />
                                    <uc4:CurrencyField ID="uctxtUnitPrice1"  runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlUnitPrice2" Visible="false" runat="server">
                                    <asp:Label ID="lblUnitPrice2" runat="server"></asp:Label><br />
                                    <uc4:CurrencyField ID="uctxtUnitPrice2" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="pnlUnitPrice3" Visible="false" runat="server">
                                    <asp:Label ID="lblUnitPrice3" runat="server"></asp:Label><br />
                                    <uc4:CurrencyField ID="uctxtUnitPrice3" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="pnlUnitPrice4" Visible="false" runat="server">
                                    <asp:Label ID="lblUnitPrice4" runat="server"></asp:Label><br />
                                    <uc4:CurrencyField ID="uctxtUnitPrice4" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="pnlUnitPrice5" Visible="false" runat="server">
                                    <asp:Label ID="lblUnitPrice5" runat="server"></asp:Label><br />
                                    <uc4:CurrencyField ID="uctxtUnitPrice5" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnAdd" runat="server" OnClick="btnAdd_Click" ValidationGroup="addProduct"
                                    ImageUrl="../../App_Themes/GlassCyan/Controls/GridView/img/Add2.gif" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <br />
                                <asp:GridView ID="grdInventory" runat="server" Width="100%" AutoGenerateColumns="False"
                                    DataKeyNames="ProductId" RowSelectable="false" OnRowDeleting="grdInventory_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Produto">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("productName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Localização">
                                            <ItemTemplate>
                                                <asp:Label ID="Label10" runat="server" Text='<%# Bind("Localization") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Custo">
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("RealCost", "{0:#,###,##0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lucro (%)">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Profit", "{0:##0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="text-align: center">
                                            Não existem dados a serem exibidos<br />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td style="text-align: right">
                                <br />
                                <asp:Button ID="btnSave" CssClass="cBtn11" Text="Salvar" runat="server" OnClick="btnSave_Click"
                                    ValidationGroup="Outside" TabIndex="99" />
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
    </div>
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" onselecting="odsDeposit_Selecting"
        SelectMethod="GetDepositByCompany" TypeName="Vivina.Erp.BusinessRules.DepositManager">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsSupplier" runat="server" OnSelecting="odsSupplier_Selecting"
        SelectMethod="getNames" TypeName="Vivina.Erp.BusinessRules.SupplierManager">
        <selectparameters>
            <asp:Parameter Name="matrixId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCurrencyRate" runat="server" SelectMethod="GetAllCurrencyRates"
        TypeName="Vivina.Erp.BusinessRules.CurrencyRateManager">
    </VFX:BusinessManagerDataSource>
    <uc1:ToolTip ID="tipStockDeposit" runat="server" Top="20px" Left="100px" Indication="left"
        Title="Dica:" Message="Entradas, saídas, transferências, são todas tarefas corriqueiras, mas, seja como for, sempre faça isso seguindo o tramites do InfoControl. Assim, nada fugirá de seu controle. Mesmo aqueles produtos que foram enviados para troca com o fornecedor/fabricante, estarão sempre dentro do controle da empresa." />
    <uc1:ToolTip ID="tipDepositEmpty" runat="server" Top="120px" Left="5px" Indication="top"
        Title="Atenção:" Message="Para dar entrada no estoque é necessário um <b>depósito de destino</b>, porém não há depósitos cadastrados!<br /><br /><a href='../Administration/Deposit.aspx' style='color: #FDCC66'>Clique aqui para cadastrar!</a>" />
    <uc1:ToolTip ID="tipSupplierEmpty" runat="server" Top="90px" Left="440px" Indication="top"
        Title="Atenção:" Message="Para dar entrada no estoque é necessário um <b>fornecedor</b>, porém não há fornecedores cadastrados!<br /><br /><a href='../Administration/Suppliers.aspx' style='color: #FDCC66'>Clique aqui para cadastrar!</a>" />

    <script language="javascript" type="text/javascript">
        var uctxtRealCost = document.getElementById("<%= this.uctxtRealCost.Controls[0].ClientID %>");


        var uctxtProfit = document.getElementById("<%= this.uctxtProfit.Controls[0].ClientID %>");

        var uctxtUnitPrice1 = document.getElementById("<%= this.uctxtUnitPrice1.Controls[0].ClientID %>");
        var uctxtUnitPrice2 = document.getElementById("<%= this.uctxtUnitPrice2.Controls[0].ClientID %>");
        var uctxtUnitPrice3 = document.getElementById("<%= this.uctxtUnitPrice3.Controls[0].ClientID %>");
        var uctxtUnitPrice4 = document.getElementById("<%= this.uctxtUnitPrice4.Controls[0].ClientID %>");
        var uctxtUnitPrice5 = document.getElementById("<%= this.uctxtUnitPrice5.Controls[0].ClientID %>");

        Sys.UI.DomEvent.addHandler(uctxtUnitPrice1, "blur", InverseCalculation);
        Sys.UI.DomEvent.addHandler(uctxtRealCost, "blur", CalculatePrice);
        Sys.UI.DomEvent.addHandler(uctxtProfit, "blur", CalculatePrice);


        function FixCulture(control) {

            var value = control.value.replace(/_/g, "").replace(/\./g, "");

            value = value.replace(",", ".");
            value = parseFloat(value);

            return value;
        }

        function CalculatePrice() {

            if (uctxtRealCost == "" || uctxtProfit.value == "")
                return;


            var value = ((FixCulture(uctxtProfit) / 100) + 1) * FixCulture(uctxtRealCost);

            SetValues(value);

        }


        function InverseCalculation() {

            if (uctxtRealCost.value == "" || uctxtUnitPrice1.value == "")
                return;

            var profit = FixCulture(uctxtUnitPrice1) / FixCulture(uctxtRealCost);

            result = (profit - 1) * 100;
            profit = result.toFixed(2);

            uctxtProfit.value = profit.replace(".", ",");
            value = FixCulture(uctxtUnitPrice1);

            SetValues(value);

        }

        function SetValues(value) {

            if (isNaN(value)) {
                uctxtUnitPrice1.value = 0;
                uctxtUnitPrice2.value = 0;
                uctxtUnitPrice3.value = 0;
                uctxtUnitPrice4.value = 0;
                uctxtUnitPrice5.value = 0;
                return;
            }

            if (uctxtUnitPrice1)
                uctxtUnitPrice1.value = value.localeFormat("N");
            if (uctxtUnitPrice2)
                uctxtUnitPrice2.value = value.localeFormat("N");
            if (uctxtUnitPrice3)
                uctxtUnitPrice3.value = value.localeFormat("N");
            if (uctxtUnitPrice4)
                uctxtUnitPrice4.value = value.localeFormat("N");
            if (uctxtUnitPrice5)
                uctxtUnitPrice5.value = value.localeFormat("N");

        }

        /* function CalculateProfit()
        {       
        if(uctxtRealCost > 0 )
        {
        var value = Math.ceil( (uctxtUnitPrice1 / uctxtRealCost) * 100) - 100;
                                
        uctxtProfit.value = parseFloat(value).localeFormat("N");        
        }
        } */
   
    </script>

</asp:Content>
