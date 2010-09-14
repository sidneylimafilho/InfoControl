<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_Deposit" Title="Estoque" EnableEventValidation="false" CodeBehind="Deposit.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc2" %>
<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc3" %>
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
                <table>
                    <tr>
                        <td colspan="2">
                            Nome:<br />
                            <asp:TextBox ID="txtName" MaxLength="50" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="reqtxtName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertDeposit" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Meta do mês:<br />
                            <uc2:CurrencyField ID="ucCurrFieldMonthlyGoal" runat="server" />
                        </td>
                    </tr>
                </table>
                <br />
                Metas Semanais:
                <br />
                <br />
                <table>
                    <tr>
                        <td style="white-space: nowrap">
                            1º semana:<br />
                            <uc2:CurrencyField ID="ucCurrFieldFirstWeekGoal" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap">
                            2º semana:<br />
                            <uc2:CurrencyField ID="ucCurrFieldSecondWeekGoal" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap">
                            3º semana:<br />
                            <uc2:CurrencyField ID="ucCurrFieldThirdWeekGoal" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap">
                            4º semana:<br />
                            <uc2:CurrencyField ID="ucCurrFieldForthWeekGoal" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap">
                            5º semana:<br />
                            <uc2:CurrencyField ID="ucCurrFieldFifthWeekGoal" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <uc3:Address ID="ucDepositAddress" Required="true" ValidationGroup="InsertDeposit"
                                FieldsetTitle="Endereço:" runat="server" />
                        </td>
                    </tr>
                </table>
                <div style="text-align: right">
                    <asp:Button ValidationGroup="InsertDeposit" ID="btnSave" OnClick="btnSave_Click"
                        runat="server" Text="Salvar"></asp:Button>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='Deposits.aspx'; return false;">
                    </asp:Button>
                </div>
                <uc1:ToolTip ID="tipOnlyDeposit" runat="server" Message="Toda empresa começa em seu cadastro com um ESTOQUE predefinido como MATRIZ e impossível de ser excluído.&lt;br /&gt;
Se você guarda seus produtos em vários lugares diferentes, crie múltiplos estoques, com diversos sentidos, como por exemplo, um estoque somente para produtos defeituosos que ainda não foram para a garantia do fornecedor."
                    Title="Dica:" Indication="left" Top="0px" Left="180px" Visible="true" />
                <br />
                <br />
                <br />
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
</asp:Content>
