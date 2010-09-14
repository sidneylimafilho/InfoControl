<%@ Page Language="C#" AutoEventWireup="true" Title=" Pesquisa de Fornecedores"
    EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master" Inherits="InfoControl_Administration_Supplier_Search" Codebehind="Supplier_Search.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
   
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
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblName" runat="server" Text="Nome/Razão social"></asp:Label>
                            :<br />
                            <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblMail" runat="server" Text="E-mail"></asp:Label>
                            :<br />
                            <asp:TextBox ID="txtMail" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCNPJ" runat="server" Text="CNPJ:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtCNPJ" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="valCNPJ" runat="server" TargetControlID="txtCNPJ"
                                CultureName="pt-BR" Mask="99,999,999/9999-99" ClearMaskOnLostFocus="False">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtCPF" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="valCPF" runat="server" TargetControlID="txtCPF"
                                CultureName="pt-BR" Mask="999,999,999-99" ClearMaskOnLostFocus="False">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPhone" runat="server" Text="Telefone"></asp:Label>
                            :<br />
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="mskTxtPhone" runat="server" TargetControlID="txtPhone"
                                CultureName="pt-BR" Mask="(99)9999-9999" ClearMaskOnLostFocus="false">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblContact" runat="server" Text="Contato"></asp:Label>
                            :<br />
                            <asp:TextBox ID="txtContact" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblVotingTitle" runat="server" Text="Titulo de Eleitor"></asp:Label>
                            :<br />
                            <asp:TextBox ID="txtVoringTitle" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblRG" runat="server" Text="RG"></asp:Label>
                            :<br />
                            <asp:TextBox ID="txtRG" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        A partir de:&nbsp;
                                    </td>
                                    <td>
                                        <ajaxToolkit:Rating ID="rtnRanking" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                            WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                            ToolTip="Classificação" CurrentRating="0">
                                        </ajaxToolkit:Rating>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <br />
                            <br />
                            <asp:Button ID="btnSearch" runat="server" Text="Pesquisar" OnClick="btnSearch_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="right">
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
