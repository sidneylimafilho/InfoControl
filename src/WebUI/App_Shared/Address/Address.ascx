<%@ Control Language="C#" AutoEventWireup="true" Inherits="Address" CodeBehind="Address.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<fieldset class="campos">
    <legend class="legenda" style="cursor: pointer" onclick="$('#<%=addressPanel.ClientID %>').show('slow');">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1"></asp:Label></legend>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="cLoading11" style="position: absolute; width: 530px; height: 120px;">
                <img id="Img1" runat="server" alt="Carregando" src="~/App_Themes/_global/loading3.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="painel" id="addressPanel" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:PostalCode %>" />:
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap" valign="bottom" style="vertical-align: bottom;" colspan="3">
                            <asp:TextBox CssClass="cDat11" ID="txtPostalCode" runat="server" MaxLength="9" Columns="7" OnTextChanged="txtPostalCode_TextChanged"
                                AutoPostBack="True" meta:resourcekey="txtPostalCodeResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valPostalCode" ControlToValidate="txtPostalCode"
                                runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" CssClass="cErr21" meta:resourcekey="valPostalCodeResource1"></asp:RequiredFieldValidator>&nbsp;
                            &nbsp;&nbsp;
                            <!--
                             <a id="btnGoogle" runat="server" style="cursor: pointer;">
                                <asp:Literal runat="server" Text="<%$ Resources:SeeInMap %>" />
                            </a>&nbsp; &nbsp;&nbsp; <a id="btnCorreios" runat="server" style="cursor: pointer;"
                                onclick="top.tb_show('Pesquisa por CEP','../App_Shared/Address_Search.htm');">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: SearchPostalCode %>" />
                            </a>
                            -->
                            <asp:CustomValidator ID="checkCep" runat="server" OnServerValidate="customValidator_OnServerValidate"
                                ControlToValidate="txtPostalCode" ErrorMessage="*" CssClass="cErr21" Visible="False"
                                meta:resourcekey="checkCepResource1">
                                <asp:Literal ID="Literal17" runat="server" Text="<%$ Resources:PostalCodeNotFoundMessage %>" />
                            </asp:CustomValidator>
                            <ajaxToolkit:MaskedEditExtender ID="PostalCode_MaskedEditExtender" runat="server"
                                TargetControlID="txtPostalCode" Mask="99999-999" MaskType="Number" CultureAMPMPlaceholder=""
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                Enabled="True">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Address %>" />:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtAddress" runat="server" Columns="40" MaxLength="50" meta:resourcekey="txtAddressResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valAddress" ControlToValidate="txtAddress" runat="server"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" Display="Dynamic" meta:resourcekey="valAddressResource1"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:AddressNumber %>" />:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtNumber" Text='<%# Bind("AddressNumber") %>' runat="server" MaxLength="8"
                                Columns="5" meta:resourcekey="txtNumberResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valNumber" ControlToValidate="txtNumber" runat="server"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" Display="Dynamic" meta:resourcekey="valNumberResource1"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:AddressComp %>" />:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtSubName" Width="160px" runat="server" Columns="8" MaxLength="50"
                                Text='<%# Bind("AddressComp") %>' meta:resourcekey="txtSubNameResource1"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Neighborhood %>" />:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtNeighborhood" runat="server" Columns="25" MaxLength="50" meta:resourcekey="txtNeighborhoodResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valNeighborhood" ControlToValidate="txtNeighborhood"
                                runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" Display="Dynamic" meta:resourcekey="valNeighborhoodResource1"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:City %>" />:
                            <br />
                            <asp:TextBox CssClass="cDat11" ID="txtCity" runat="server" Columns="18" MaxLength="50" meta:resourcekey="txtCityResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valCity" ControlToValidate="txtCity" runat="server"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" Display="Dynamic" meta:resourcekey="valCityResource1"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:Literal ID="Literal8" runat="server" Text="Estado" meta:resourcekey="Literal8Resource1" />:
                            <br />
                            <asp:DropDownList ID="cboStates" runat="server" Width="100px" meta:resourcekey="cboStatesResource1">
                                <asp:ListItem Value="AC" meta:resourcekey="ListItemResource1">Acre</asp:ListItem>
                                <asp:ListItem Value="AL" meta:resourcekey="ListItemResource2">Alagoas</asp:ListItem>
                                <asp:ListItem Value="AP" meta:resourcekey="ListItemResource3">Amapá</asp:ListItem>
                                <asp:ListItem Value="AM" meta:resourcekey="ListItemResource4">Amazonas</asp:ListItem>
                                <asp:ListItem Value="BA" meta:resourcekey="ListItemResource5">Bahia</asp:ListItem>
                                <asp:ListItem Value="CE" meta:resourcekey="ListItemResource6">Ceará</asp:ListItem>
                                <asp:ListItem Value="DF" meta:resourcekey="ListItemResource7">Distrito Federal</asp:ListItem>
                                <asp:ListItem Value="GO" meta:resourcekey="ListItemResource8">Goiás</asp:ListItem>
                                <asp:ListItem Value="ES" meta:resourcekey="ListItemResource9">Espírito Santo</asp:ListItem>
                                <asp:ListItem Value="MA" meta:resourcekey="ListItemResource10">Maranhão</asp:ListItem>
                                <asp:ListItem Value="MT" meta:resourcekey="ListItemResource11">Mato Grosso</asp:ListItem>
                                <asp:ListItem Value="MS" meta:resourcekey="ListItemResource12">Mato Grosso do Sul</asp:ListItem>
                                <asp:ListItem Value="MG" meta:resourcekey="ListItemResource13">Minas Gerais</asp:ListItem>
                                <asp:ListItem Value="PA" meta:resourcekey="ListItemResource14">Pará</asp:ListItem>
                                <asp:ListItem Value="PB" meta:resourcekey="ListItemResource15">Paraiba</asp:ListItem>
                                <asp:ListItem Value="PR" meta:resourcekey="ListItemResource16">Paraná</asp:ListItem>
                                <asp:ListItem Value="PE" meta:resourcekey="ListItemResource17">Pernambuco</asp:ListItem>
                                <asp:ListItem Value="PI" meta:resourcekey="ListItemResource18">Piauí</asp:ListItem>
                                <asp:ListItem Value="RJ" meta:resourcekey="ListItemResource19">Rio de Janeiro</asp:ListItem>
                                <asp:ListItem Value="RN" meta:resourcekey="ListItemResource20">Rio Grande do Norte</asp:ListItem>
                                <asp:ListItem Value="RS" meta:resourcekey="ListItemResource21">Rio Grande do Sul</asp:ListItem>
                                <asp:ListItem Value="RO" meta:resourcekey="ListItemResource22">Rondônia</asp:ListItem>
                                <asp:ListItem Value="RR" meta:resourcekey="ListItemResource23">Rorâima</asp:ListItem>
                                <asp:ListItem Value="SP" meta:resourcekey="ListItemResource24">São Paulo</asp:ListItem>
                                <asp:ListItem Value="SC" meta:resourcekey="ListItemResource25">Santa Catarina</asp:ListItem>
                                <asp:ListItem Value="SE" meta:resourcekey="ListItemResource26">Sergipe</asp:ListItem>
                                <asp:ListItem Value="TO" meta:resourcekey="ListItemResource27">Tocantins</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="valStates" ControlToValidate="cboStates" runat="server"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" Display="Dynamic" meta:resourcekey="valStatesResource1"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>           
        </asp:UpdatePanel>
    </div>
    <div style="width: 530px; height: 1px;">
        &nbsp;</div>
</fieldset>
