<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Package_General.aspx.cs" Inherits="Vivina.Erp.WebUI.Host.Package_General"
    Title="Untitled Page" %>

<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h1>
        <asp:Literal ID="litTitle" Text="Pacote" runat="server"></asp:Literal>
    </h1>
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
                            Nome:<br />
                            <asp:TextBox ID="txtName" Columns="30" MaxLength="30" runat="server" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="reqTxtName" runat="server"
                                ValidationGroup="Save" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Número de Usuários:<br />
                            <input type="button" value="-" id="btnDown" class="cUpDown11" tabindex="100" />
                            <asp:TextBox ID="txtNumberUsers" runat="server" Columns="5" MaxLength="5" />
                            <input type="button" value="+" id="btnUp" class="cUpDown11" tabindex="101" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RequiredFieldValidator2"
                                runat="server" ControlToValidate="txtNumberUsers"></asp:RequiredFieldValidator>
                            <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" Maximum="1000"
                                Minimum="0" TargetControlID="txtNumberUsers" TargetButtonDownID="btnDown" TargetButtonUpID="btnUp"
                                Width="20">
                            </ajaxToolkit:NumericUpDownExtender>
                        </td>
                        <td>
                            Número de Produtos:<br />
                            <input type="button" value="-" id="btnDown2" class="cUpDown11" tabindex="200" />
                            <asp:TextBox ID="txtNumberItems" runat="server" Columns="5" MaxLength="5" />
                            <input type="button" value="+" id="btnUp2" class="cUpDown11" tabindex="201" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RequiredFieldValidator3"
                                runat="server" ControlToValidate="txtNumberItems"></asp:RequiredFieldValidator>
                            <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender2" runat="server" Maximum="1000"
                                Step="50" Minimum="0" TargetControlID="txtNumberItems" TargetButtonDownID="btnDown2"
                                TargetButtonUpID="btnUp2" Width="60">
                            </ajaxToolkit:NumericUpDownExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Valor (R$):<br />
                            <uc1:CurrencyField ID="ucCurrFieldPrice" runat="server" />
                        </td>
                        <td>
                            Valor por Hora:
                            <br />
                            <uc1:CurrencyField Required="true" ValidationGroup="Save" ID="ucCurrFieldValueByHour"
                                runat="server" />
                        </td>
                        <td>
                            Taxa de Configuração:
                            <br />
                            <uc1:CurrencyField ID="ucCurrFieldSetupFee" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Preço por Produto:
                            <br />
                            <uc1:CurrencyField ID="ucCurrFieldProductPrice" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsActive" Text="Ativo ?" runat="server" />
                        </td>
                    </tr>
                </table>
                <div align="right">
                    <asp:Button ID="btnSave" runat="server" ValidationGroup="Save" Text="Salvar" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click"
                         />
                </div>
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
