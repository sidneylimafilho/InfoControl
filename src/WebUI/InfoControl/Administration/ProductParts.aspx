<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_Administration_ProductParts" Title="Peças de Produtos" Codebehind="ProductParts.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI"
    Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
   
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                &nbsp;</td>
            <td class="right">
                &nbsp;</td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="200px" style="border: 1px solid #00AAAA">
                            <radT:RadTreeView ID="RadTreeView1" runat="server" DataFieldID="ProductPartId" DataFieldParentID="ParentId"
                                DataTextField="Name" DataValueField="ProductPartId" Height="400px" Width="100%"
                                OnNodeBound="RadTreeView1_NodeBound">
                            </radT:RadTreeView>
                        </td>
                        <td style="padding-left: 30px;" valign="top">
                            Para inserir uma peça que constitui outra peça,
                            <br />
                            clique na de nível superior e depois em &quot;Adicionar&quot;:<br />
                            <asp:TextBox ID="txtProductPart" runat="server" Text="" Columns="40" MaxLength="50" />
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator1" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;" ControlToValidate="txtProductPart"></asp:RequiredFieldValidator>
                            <br />
                            Quantidade:<br />
                            <input id="btnDown" class="cUpDown11" tabindex="10" type="button" value="-" />
                            <asp:TextBox ID="txtQuantity" runat="server" Columns="5" MaxLength="5" TabIndex="0"></asp:TextBox>
                            <input id="btnUp" class="cUpDown11" tabindex="11" type="button" value="+" />
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtQuantity"
                                 MaximumValue="10000" MinimumValue="0" Type="Integer" ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Grid"></asp:RangeValidator>
                            <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" Maximum="10000"
                                Minimum="0" TargetButtonDownID="btnDown" TargetButtonUpID="btnUp" TargetControlID="txtQuantity"
                                Width="60">
                            </ajaxToolkit:NumericUpDownExtender>
                            <br />
                            <br />
                            <asp:Button ID="btnAdd" runat="server" Text="Adicionar " OnClick="btnAdd_Click" permissionRequired="ProductParts" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpdate" runat="server" Text="Alterar "
                                OnClick="btnUpdate_Click" permissionRequired="ProductParts" />
                            <br />
                            <br />
                            <br />
                            <asp:Literal ID="lblDelete" runat="server" Text="Selecione a categoria e clique em Excluir:" />
                            <br />
                            <asp:Button ID="btnDelete" runat="server" Text="Excluir " OnClick="btnDelete_Click"
                                CausesValidation="False" permissionRequired="ProductParts" />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="right">
                &nbsp;</td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                &nbsp;</td>
            <td class="right">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
