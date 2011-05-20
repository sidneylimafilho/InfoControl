<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" Inherits="InfoControl_Administration_Transporter_Search"
    Title="Pesquisa de Transportadora" EnableEventValidation="false" Codebehind="Transporter_Search.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
  
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
                <td class="left" style="height: 97px">
                    &nbsp;
                </td>
                <td class="center" style="height: 97px">
                    <table width="100%">
                        <tr>
                            <td>
                                Razão Social:
                                <br />
                                <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>
                                Nome Fantasia:
                                <br />
                                <asp:TextBox ID="txtFantasyName" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CNPJ:
                                <br />
                                <asp:TextBox ID="txtCNPJ" Plugin="mask" Mask="99.999.999/9999-99" Columns="14"  runat="server"></asp:TextBox> 
                            </td>
                            <td>
                                Telefone:
                                <br />
                                <asp:TextBox ID="txtPhone" Plugin="mask" Mask="(99)9999-9999" Columns="10" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email:
                                <br />
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                Site:<br />
                                <asp:TextBox ID="txtWebSite" runat="server" MaxLength="1024"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSearch" runat="server" Text="Pesquisar" OnClick="btnSearch_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="right" style="height: 97px">
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
