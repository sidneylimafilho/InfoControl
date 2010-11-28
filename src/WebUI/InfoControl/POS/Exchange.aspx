<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true" 
Inherits="Company_POS_Exchange" Title="Devolução" Codebehind="Exchange.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
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
                    <table width="100%" cellpadding="10px" cellspacing="10px">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 5%">
                                Nº da Venda:<br />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSaleNumber" runat="server"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="txtSaleNumber_MaskedEditExtender" runat="server"
                                    CultureName="pt-BR" InputDirection="RightToLeft" Mask="999999999" MaskType="Number"
                                    TargetControlID="txtSaleNumber">
                                </ajaxToolkit:MaskedEditExtender>
                                <asp:ImageButton ID="btnAddSale" runat="server" ImageUrl="~\App_Shared/themes/glasscyan\Controls\GridView\img\Add2.gif"
                                    OnClick="btnAddSale_Click" />
                                <br />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlExchange" runat="server" Visible="False">
                        <table width="100%" cellpadding="10px" cellspacing="10px">
                            <tr>
                                <td colspan="2">
                                    <h3>
                                        Dados da Venda:</h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Data da Venda:</b>&nbsp;<asp:Label ID="lblSaleDate" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <b>Loja de Origem:</b>&nbsp;<asp:Label ID="lblSourceStore" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Total da Venda:</b>&nbsp;R$&nbsp;<asp:Label ID="lblSaleTotal" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <b>Venda efetuada por:</b>&nbsp;<asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <b>Total de Descontos na nota:</b>&nbsp;R$&nbsp;<asp:Label ID="lblDiscount" runat="server"
                                        Text=""></asp:Label><br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <h3>
                                        Produtos:</h3>
                                    <asp:GridView ID="grdSaleItems" runat="server" Width="90%" AutoGenerateColumns="False"
                                        DataKeyNames="ProductId,UnitPrice,UnitCost">
                                        <Columns>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Código"></asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Nome"></asp:BoundField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantidade"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td valign="top">
                                    <h3>
                                        Pagamento:</h3>
                                    <asp:GridView ID="grdPaymentMethod" runat="server" Width="90%" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Forma de Pagamento">
                                                <HeaderStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Amount" HeaderText="Valor">
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="10px" cellspacing="10px" width="100%">
                            <tr>
                                <td style="width: 5%">
                                    Quant.:
                                </td>
                                <td>
                                    Produto(s) a ser(em) trocado(s):
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtQuantityIN" runat="server" MaxLength="5" Width="25px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboProducts" runat="server">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnDevolution" runat="server" ImageUrl="~\App_Shared/modules\Glass\Controls\GridView\img\Add2.gif"
                                        OnClick="btnDevolution_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:GridView ID="grdDevolution" runat="server" Width="80%" AutoGenerateColumns="False"
                                        DataKeyNames="ProductId,UnitPrice,UnitCost,Quantity">
                                        <Columns>
                                            <asp:BoundField DataField="ProductName" HeaderText="Produto"></asp:BoundField>
                                            <asp:BoundField DataField="ProductId" HeaderText="" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="UnitPrice" HeaderText="" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="UnitCost" HeaderText="" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantidade"></asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            Insira os produtos que estão sendo devolvidos ...
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                                <td valign="top" style="text-align: center">
                                    <asp:Label ID="lblDevoltionValue" runat="server" Text="Valor à ser Devolvido" Visible="False"></asp:Label><br />
                                    <asp:TextBox ID="txtDevolutionValue" runat="server" Visible="False"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <div style="text-align: right; width: 98%">
                        <asp:Button ID="btnOK" runat="server" Text="Devolver" Visible="false" OnClick="btnOK_Click" />
                    </div>
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
</asp:Content>
