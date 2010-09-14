<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_Accounting_PayParcel" Title="Untitled Page" Codebehind="PayParcel.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;Data do Pagamento/Recebimento:
            </td>
            <td>
                Valor<b>(R$):</b>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtEffectedDate" runat="server" Columns="10" MaxLength="10"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server" ControlToValidate="txtEffectedDate"
                    CssClass="cErr21" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" ClearMaskOnLostFocus="False"
                    Mask="99/99/9999" CultureName="pt-BR" MaskType="Date" TargetControlID="txtEffectedDate">
                </ajaxToolkit:MaskedEditExtender>
            </td>
            <td>
                <asp:TextBox ID="txtEffectedAmount" runat="server"></asp:TextBox>
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" InputDirection="RightToLeft"
                    Mask="9,999,999.99" MaskType="Number" TargetControlID="txtEffectedAmount">
                </ajaxToolkit:MaskedEditExtender>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlAccount" runat="server" Visible="false">
        <br />
        <br />
        <table width="100%">
            <tr>
                <td colspan="4">
                    <asp:CheckBox ID="chkRecurrent" runat="server" Text="Recorrente" AutoPostBack="True"
                        OnCheckedChanged="chkRecurrent_CheckedChanged" /><br />
                    <asp:Label ID="lblRecurrent" runat="server" Text="&nbsp;&nbsp;&nbsp;Escolha o prazo da repetição da parcela:"
                        Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="Recurrent" Text="Semanal"
                        Visible="false" />
                </td>
                <td>
                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="Recurrent" Text="Quinzenal"
                        Visible="false" />
                </td>
                <td>
                    <asp:RadioButton ID="RadioButton3" runat="server" GroupName="Recurrent" Text="Mensal"
                        Visible="false" />
                </td>
                <td>
                    <asp:RadioButton ID="RadioButton4" runat="server" GroupName="Recurrent" Text="Anual"
                        Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblAccount" runat="server" Text="Escolha de qual conta será debitada a quantia acima:"></asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="cboAccount" runat="server" DataSourceID="odsAccount" DataTextField="value"
            DataValueField="AccountId">
        </asp:DropDownList>
    </asp:Panel>
    <br />
    <br />
    <div style="text-align: right">
        <asp:Button ID="btnOK" runat="server" Text="Efetuar" OnClick="btnOK_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CausesValidation="false"
            OnClick="btnCancel_Click" />&nbsp;&nbsp;&nbsp;
    </div>
    <VFX:BusinessManagerDataSource ID="odsAccount" runat="server" onselecting="odsAccount_Selecting"
        SelectMethod="GetAccounts" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
