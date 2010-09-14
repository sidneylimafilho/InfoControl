<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="CompanySymbol.aspx.cs" Inherits="Vivina.Erp.WebUI.CompanySymbol" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                            <asp:Image ID="imgLogo" ImageUrl="~/InfoControl/ImageHandler.aspx" Width="183px"
                                Height="51px" runat="server" />
                        </td>
                        <td valign="top">
                            <asp:Label ID="Label5" runat="server" Text="Logotipo:&amp;nbsp;" Font-Bold="True"
                                Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Orange"></asp:Label>
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="(Arquivo de imagem que contenha o logotipo da empresa&lt;br /&gt;O arquivo de imagem, deverá ter 51px de altura e 183px de largura)"
                                Font-Size="Smaller"></asp:Label>
                            <br />
                            <asp:FileUpload ID="txtImageUpload" runat="server" ToolTip="Arquivos de Imagens" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="lblWelcomeText" runat="server" Text="Digite o texto que aparecerá na caixa de boas-vindas dos usuários desta empresa"></asp:Label>
                            <br />
                            <telerik:RadEditor Height="250px" ID="txtWelcomeText" runat="server" SkinID="Telerik">
                                <Content>
                                </Content>
                            </telerik:RadEditor>
                        </td>
                    </tr>
                </table>
                <div align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
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
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
