<%@ Page Language="C#" AutoEventWireup="true"
    EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master" 
    Inherits="InfoControl_Administration_Customer_Search" Title="Pesquisa de Clientes" Codebehind="Customer_Search.aspx.cs" %>

<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        <a href="javascript:void(0);" class="HelpTip">
            <img id="Img1" runat="server" border="0" src="~/App_Shared/themes/glasscyan/ico_ajuda.gif" /><span
                class="msg">Aqui, você também poderá fazer uma pesquisa pelos seus clientes, com
                diversos parâmetros, o que facilitará na hora de encontrar “aquele” cliente que
                não aparece há muito tempo, onde preenchendo apenas um campo, o sistema faz a filtragem
                no sistema e encontra o cliente desejado.<span class="footer"></span></span></a></h1>
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
                            Nome/Razão social:<br />
                            <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            E-mail:<br />
                            <asp:TextBox ID="txtMail" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            CNPJ:<br />
                            <asp:TextBox ID="txtCNPJ" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="valCNPJ" runat="server" TargetControlID="txtCNPJ"
                                CultureName="pt-BR" Mask="99,999,999/9999-99" ClearMaskOnLostFocus="False">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                        <td>
                            CPF:<br />
                            <asp:TextBox ID="txtCPF" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="valCPF" runat="server" TargetControlID="txtCPF"
                                CultureName="pt-BR" Mask="999,999,999-99" ClearMaskOnLostFocus="False">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Telefone:<br />
                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="mskPhone" runat="server" TargetControlID="txtPhone"
                                CultureName="pt-BR" Mask="(99)9999-9999" MaskType="Number" ClearMaskOnLostFocus="false">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                        <td>
                            Contato:<br />
                            <asp:TextBox ID="txtContact" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Titulo de Eleitor:<br />
                            <asp:TextBox ID="txtVoringTitle" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            RG:<br />
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
