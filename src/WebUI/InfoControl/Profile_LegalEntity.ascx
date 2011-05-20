<%@ Control Language="C#" AutoEventWireup="true" Inherits="Profile_LegalEntity" CodeBehind="Profile_LegalEntity.ascx.cs" %>
<%@ Register Src="../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Panel ID="searchForm" runat="server" Visible="false">
    CNPJ:
    <br />
    <asp:TextBox ID="txtSearchCNPJ" runat="server" Columns="18" MaxLength="18" Text="" />
    <%--  <asp:RequiredFieldValidator CssClass="cErr21" ID="valCnpj" runat="server" ControlToValidate="txtSearchCNPJ"
        ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>--%>
    <VFX:CnpjValidator ID="CnpjValidator1" runat="server" ControlToValidate="txtSearchCNPJ"
         Display="Dynamic" Enabled="true" ValidationGroup="valNext">
        &nbsp;&nbsp;&nbsp;</VFX:CnpjValidator>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnSelect" runat="server" Text="Avançar " OnClick="btnSelect_Click"
        ValidationGroup="valNext" />
    <br />
</asp:Panel>
<div id="entryForm" runat="server">
    <fieldset>
        <legend>Dados gerais</legend>
        <table>
            <tr>
                <td>
                    CNPJ:<br />
                    <asp:TextBox ID="txtCNPJ" AutoPostBack="True" runat="server" Text="" OnTextChanged="btnSelect_Click"
                        Columns="14" MaxLength="18" />
                    &nbsp;<span class="btnReceitaFederal" onclick="ConsultaReceitaFederal('<%=txtCNPJ.Text.Replace(".", "").Replace("-", "").Replace("/", "") %>');">&nbsp;</span>
                   <asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtCNPJ" runat="server" Display="Dynamic" ControlToValidate="txtCNPJ"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;" />
                <td>
                    <VFX:CnpjValidator ID="valCnpj" runat="server" ControlToValidate="txtCNPJ" 
                        Display="Dynamic" Enabled="true" ValidationGroup="valNext">
        &nbsp;&nbsp;&nbsp;</VFX:CnpjValidator>
                </td>
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtCNPJ"
                    CultureName="pt-BR" Mask="99,999,999/9999-99" ClearMaskOnLostFocus="False">
                </ajaxToolkit:MaskedEditExtender>
                 
                <td>
                    Razão Social:<br />
                    <asp:TextBox ID="txtCompanyName" runat="server" Width="270" MaxLength="100" Height="13px" />
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="valCompanyName" runat="server" ControlToValidate="txtCompanyName"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>&nbsp;
                </td>
                <td>
                    Nome Fantasia:<br />
                    <asp:TextBox ID="txtFantasyName" runat="server" Width="250px" MaxLength="100" Height="13px" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    Insc. Estadual:<br />
                    <asp:TextBox ID="txtIE" runat="server" Columns="10" MaxLength="20" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    Insc. Municipal:<br />
                    <asp:TextBox ID="txtMunicipalRegister" runat="server" MaxLength="20" Columns="10">       </asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                
                <td>
                    Telefone:<br /> 
                    <asp:TextBox ID="txtPhone" Plugin="mask" Mask="(99)9999-9999" runat="server" Columns="10" MaxLength="50" />
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="valPhone" runat="server" Display="Dynamic" ControlToValidate="txtPhone"
                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="save" InitialValue="(__)____-____">
                    </asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    Fax:<br />
                    <asp:TextBox ID="txtFax" runat="server" Columns="10" Plugin="mask" Mask="(99)9999-9999"  MaxLength="13" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    Email:<br />
                    <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="50" />
                    <asp:RegularExpressionValidator CssClass="cErr21" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
       
    </fieldset>
    <br />
    <uc1:Address ID="Address" runat="server" Required="true" FieldsetTitle="Endereço Entrega:" />
    <br />
    <uc1:Address ID="RecoveryAddress" Visible="True" Required="False" runat="server"
        FieldsetTitle="Endereço Faturamento:" />
</div>

<script type="text/javascript">
    function ConsultaReceitaFederal(cnpj) {

        window.open("http://www.receita.fazenda.gov.br/PessoaJuridica/CNPJ/cnpjreva/cnpjreva_solicitacao2.asp?cnpj=" + cnpj, "JANELA", "scrollbars=yes, height=480, width=680");
    }
</script>

