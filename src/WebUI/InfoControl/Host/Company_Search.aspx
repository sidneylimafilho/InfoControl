<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Search"
    MasterPageFile="~/infocontrol/Default.master" Title="Pesquisa de Empresas"
     Codebehind="Company_Search.aspx.cs" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" ID="content" runat="server">
    <h1>
        </h1>
    <br />
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
            </td>
            <td class="center">
            </td>
            <td class="right">
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
            </td>
            <td class="center">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblEmpresa" runat="server" Text="Empresa:" AssociatedControlID="txtEmpresa"></asp:Label><br />
                            <asp:TextBox ID="txtEmpresa" runat="server" Columns="30" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblEmail" runat="server" Text="Email:" AssociatedControlID="TxtEmail"></asp:Label><br />
                            <asp:TextBox ID="TxtEmail" runat="server" Columns="30" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblCNPJ" runat="server" Text="CNPJ:" AssociatedControlID="txtCNPJ"></asp:Label><br />
                            <asp:TextBox ID="TxtCNPJ" runat="server" Columns="20" MaxLength="18"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTelefone" runat="server" Text="Telefone:" AssociatedControlID="txtTelefone"></asp:Label><br />
                            <asp:TextBox ID="txtTelefone" runat="server" Columns="15" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblFantasia" runat="server" Text="Nome Fantasia:" AssociatedControlID="txtFantasia"></asp:Label><br />
                            <asp:TextBox ID="TxtFantasia" runat="server" Columns="40" MaxLength="20"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblSite" runat="server" Text="Endereço do Site:" AssociatedControlID="txtSite"></asp:Label><br />
                            <asp:TextBox ID="TxtSite" runat="server" Columns="30" MaxLength="300"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblIE" runat="server" Text="Inscrição Estadual:" AssociatedControlID="txtIE"></asp:Label><br />
                            <asp:TextBox ID="TxtIE" runat="server" Columns="15" MaxLength="20"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblContato" runat="server" Text="Nome do Contato:" AssociatedControlID="txtContato"></asp:Label><br />
                            <asp:TextBox ID="TxtContato" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="LblDtInicio" runat="server" Text="Data de Início:" AssociatedControlID="txtDtInicio"></asp:Label><br />
                            <asp:TextBox ID="TxtDtInicio" runat="server" Columns="11" MaxLength="10"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="TxtDtInicio"
                                CultureName="pt-BR" Mask="99/99/9999" ClearMaskOnLostFocus="True" MaskType="Date">
                            </ajaxToolkit:MaskedEditExtender>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TxtDtInicio"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="GreaterThanEqual" ValueToCompare="1/1/1753"
                                Type="Date" ></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='3' align="right">
                            <br />
                            <br />
                            <div style="text-align: right">
                                <asp:Button ID="BtnPesquisar" runat="server" Text="Pesquisar" OnClick="BtnPesquisar_Click"
                                    CommandName="Insert" />
                                <span class="cBtn11">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="reset" class="cDat21" value="Limpar" /><span></span></span></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="right">
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
            </td>
            <td class="center">
            </td>
            <td class="right">
            </td>
        </tr>
    </table>
</asp:Content>
