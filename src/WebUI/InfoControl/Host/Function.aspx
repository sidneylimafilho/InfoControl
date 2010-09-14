<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" EnableEventValidation="false"
    AutoEventWireup="True" title="Função" CodeBehind="Function.aspx.cs" Inherits="Vivina.Erp.WebUI.Host.Function" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
       
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
                            <asp:TextBox ID="txtName" MaxLength="128" Width="200" runat="server"> </asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtvalName" runat="server" ControlToValidate="txtName"
                                ValidationGroup="save" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Código:<br />
                            <asp:TextBox ID="txtCode" MaxLength="128" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            Descrição:<br />
                            <telerik:RadEditor ID="txtDescription" runat="server" SkinID="Telerik">
                                <Content></Content>
                            </telerik:RadEditor>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <br />
                            <br />
                            <asp:Button ID="btnSave" ValidationGroup="save" Text="Salvar" runat="server" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" Text="Cancelar" runat="server" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
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
