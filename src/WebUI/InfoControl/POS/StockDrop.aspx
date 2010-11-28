<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Company_Stock_StockDrop" CodeBehind="StockDrop.aspx.cs"
    Title="Baixa no Estoque" %> 

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>

<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>


<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
        <table class="cLeafBox21" width="50%">
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
                    <br />
                    <table>
                        <tr>
                            <td>
                                Estoque Físico:<br />
                                <asp:DropDownList ID="cboDeposit" runat="server" AppendDataBoundItems="True" DataSourceID="odsDeposit"
                                    DataTextField="Name" DataValueField="DepositId" AutoPostBack="True" OnSelectedIndexChanged="cboDeposit_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboDeposit" runat="server" ControlToValidate="cboDeposit"
                                     ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Produto:<br />
                                <asp:DropDownList ID="cboProducts" runat="server" DataTextField="Name" DataValueField="ProductId"
                                    Enabled="False">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboProducts" runat="server" ControlToValidate="cboProducts"
                                     ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Quantidade à ser retirada:<br />
                                <uc3:CurrencyField runat="server" ID="ucCurrFieldQuantity" Mask="99999" Required="true" ValidationGroup="Save"  />
                                
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSave" runat="server" Text="Baixa"  OnClick="btnSave_Click" ValidationGroup="Save" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                    <VFX:BusinessManagerDataSource ID="odsInventory" runat="server" SelectMethod="GetProductsByDeposit"
                        TypeName="Vivina.Erp.BusinessRules.InventoryManager" OnSelecting="odsInventory_Selecting">
                        <selectparameters>                            
                            <asp:parameter Name="depositId" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" OnSelecting="odsDeposit_Selecting"
                        SelectMethod="GetDepositByCompany" TypeName="Vivina.Erp.BusinessRules.DepositManager">
                        <selectparameters>
                            <asp:parameter Name="companyId" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
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
    <uc2:ToolTip ID="tipStockDrop" runat="server" Message="Em perfil, você pode impedir que colaboradores não autorizados tenham acesso a este campo. São essas ações que impedem extravio indevido, e mau uso do sistema. Não esqueça: ao dar baixa em um produto o sistema considera que ele entrou, foi pago e saiu. Isso influência nos cálculos e gráficos do sistema, mostrando assim informações incorretas. Se o problema foi uma inserção inexata de produto no estoque, utilize a ferramenta devida para fazer esse acerto."
        Title="Dica:" Indication="left" Top="40px" Left="200px" Visible="true" />
</asp:Content>
