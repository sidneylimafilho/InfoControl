<%@ Control Language="C#" AutoEventWireup="true" Inherits="Profile_NaturalPerson"
    CodeBehind="Profile_NaturalPerson.ascx.cs" %>
<%@ Register Src="../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register Src="../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Src="../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Panel ID="searchForm" runat="server" Visible="false">
    CPF:<br />
    <asp:TextBox ID="txtSearchCPF" runat="server" Text="" Columns="18" MaxLength="18" />
    <asp:RequiredFieldValidator CssClass="cErr21" ID="valCnpj" runat="server" ControlToValidate="txtSearchCPF"
        ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
    <VFX:CpfValidator ID="CpfValidator2" runat="server" ControlToValidate="txtSearchCPF"
        Display="Dynamic" Enabled="true" ValidationGroup="valNext" >&nbsp;&nbsp;&nbsp;</VFX:CpfValidator>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnSelect" runat="server" Text="Avançar" OnClick="btnSelect_Click"
        ValidationGroup="valNext" />
    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtSearchCPF"
        CultureName="pt-BR" Mask="999,999,999-99" ClearMaskOnLostFocus="False">
    </ajaxToolkit:MaskedEditExtender>
</asp:Panel>
<div id="entryForm" runat="server">
    <fieldset>
        <legend>Dados gerais</legend>
        <table>
            <tr>
                <td>CPF:<br />
                    <asp:TextBox ID="txtCPF" runat="server" Text="" Columns="10" MaxLength="14" EnableViewState="true"
                        AutoPostBack="True" OnTextChanged="txtCPF_TextChanged" />
                    <span class="btnReceitaFederal" onclick="ConsultaReceitaFederal('<%=txtCPF.Text.Replace(".", "").Replace("-", "").Replace("/", "") %>');">
                        &nbsp; </span>
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="valCpf" runat="server" Display="Dynamic" ControlToValidate="txtCPF"
                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;"  ></asp:RequiredFieldValidator>
                    <VFX:CpfValidator ID="CpfValidator" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                        ControlToValidate="txtCPF" >
                    </VFX:CpfValidator>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtCPF"
                        CultureName="pt-BR" Mask="999,999,999-99" ClearMaskOnLostFocus="False">
                    </ajaxToolkit:MaskedEditExtender>
                </td>
                <td>Nome:<br />
                    <asp:TextBox ID="txtName" runat="server" Width="250" MaxLength="100" />
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="valName" runat="server" ControlToValidate="txtName"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>E-mail:<br />
                    <asp:TextBox ID="txtEmail" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                    <asp:RegularExpressionValidator CssClass="cErr21" ID="ValEmail" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;"
                        ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        <table width="50%">
            <tr>
                <td>Telefone:<br />
                    <asp:TextBox ID="txtPhone" Plugin="mask" Mask="(99)9999-9999" runat="server" Columns="10"
                        MaxLength="13" />
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqPhone" runat="server" ControlToValidate="txtPhone"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                </td>
                <td>Telefone Residencial:<br />
                    <asp:TextBox ID="txtHomePhone" Plugin="mask" Mask="(99)9999-9999" runat="server"
                        Columns="10" MaxLength="13" />
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqHomePhone" runat="server" ControlToValidate="txtHomePhone"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                </td>
                <td>Telefone Celular:<br />
                    <asp:TextBox ID="txtCellPhone" runat="server" Plugin="mask" Mask="(99)9999-9999"
                        Columns="10" MaxLength="13" />
                </td>
                <td>Fax:<br />
                    <asp:TextBox ID="txtFax" runat="server" Plugin="mask" Mask="(99)9999-9999" Columns="10"
                        MaxLength="13" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>Sexo:<br />
                    <asp:DropDownList ID="cboSex" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Masculino" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Feminino" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqSex" runat="server" ControlToValidate="cboSex"
                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Estado Civil:<br />
                    <asp:DropDownList ID="cboMaritalStatus" runat="server" AppendDataBoundItems="True"
                        DataSourceID="odsMaritalStatus" DataTextField="Name" DataValueField="MaritalStatusId">
                        <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                            Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqMaritalStatus" runat="server" ControlToValidate="cboMaritalStatus"
                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Escolaridade:<br />
                    <asp:DropDownList ID="cboEducation" runat="server" AppendDataBoundItems="True" DataSourceID="odsEducation"
                        DataTextField="Name" DataValueField="EducationLevelId">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqEducation" runat="server" ControlToValidate="cboEducation"
                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" Display="Dynamic" ></asp:RequiredFieldValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
            </tr>
        </table>
        <table runat="server" visible="false">
            <tr>
                <td>Nacionalidade:<br />
                    <asp:TextBox ID="txtBornCountry" MaxLength="100" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>Naturalidade:<br />
                    <asp:TextBox ID="txtBornCity" MaxLength="100" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <table runat="server" visible="false">
            <tr>
                <td>RG:<br />
                    <asp:TextBox ID="txtRG" runat="server" Columns="15" MaxLength="20"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Orgão emissor:<br />
                    <asp:TextBox ID="txtIssueBureau" MaxLength="50" Columns="15" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Data de Emissão:<br />
                    <uc2:Date ID="ucCnhRgCreateDate" runat="server" />
                </td>
            </tr>
        </table>
        <table runat="server" visible="false">
            <tr>
                <td>Reg. Profissional:<br />
                    <asp:TextBox ID="txtProfessionalRegister" Columns="15" runat="server" MaxLength="50"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Título de eleitor:<br />
                    <asp:TextBox ID="txtVoltingTitle" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                </td>
                <td>Data de Nascimento:<br />
                    <uc2:Date ID="ucBirthDate" runat="server" />
                </td>
            </tr>
        </table>
        <table runat="server" visible="false">
            <tr>
                <td>CNH:<br />
                    <asp:TextBox ID="txtCnhNumber" MaxLength="50" Columns="15" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Tipo de CNH:<br />
                    <asp:TextBox ID="txtCnhClass" MaxLength="50" Columns="15" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Data de Expiração:<br />
                    <uc2:Date ID="ucCnhExpiresDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp; </td>
                <td>&nbsp; </td>
            </tr>
        </table>
        <table runat="server" visible="false">
            <tr>
                <td>Nome do pai:<br />
                    <asp:TextBox ID="txtFatherName" Width="250" MaxLength="100" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">Nome da mãe:<br />
                    <asp:TextBox ID="txtMotherName" Width="250" MaxLength="100" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <uc1:Address ID="Address" runat="server" Required="True" FieldsetTitle="Endereço Entrega:" />
    <br />
    <uc1:Address ID="RecoveryAddress" Required="false" Visible="True" runat="server"
        FieldsetTitle="Endereço Cobrança:" />
    <br />
    <fieldset visible="false" runat="server">
        <legend style="cursor: pointer" onclick="$('#pnlDadosAdicionais').show('slow')">Dados
            Adicionais:</legend>
        <table id="pnlDadosAdicionais" style="display: none">
            <tr>
                <td>
                    <asp:CheckBox ID="chkHasOwnHome" runat="server" Text="Possui Casa Própria?" />&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>Valor do Aluguel:<br />
                    <uc3:CurrencyField ID="ucCurrFieldRentHouseValue" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td>Valor do Financiamento do Carro:<br />
                    <uc3:CurrencyField ID="ucCurrFieldCarLeasingValue" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkHasOwnCar" runat="server" Text="Possui Carro?" />&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>Nº Dependentes:<br />
                    <uc3:CurrencyField ID="ucCurrFieldDependentNumber" Mask="9999" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Referência Pessoal:<br />
                    <asp:TextBox ID="txtPersonalReference1" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td align="left">Referência Pessoal:<br />
                    <asp:TextBox ID="txtPersonalReference2" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
            </tr>
            <tr>
                <td>Referência Comercial:<br />
                    <asp:TextBox ID="txtComercialReference1" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td align="left">Referência Comercial:<br />
                    <asp:TextBox ID="txtComercialReference2" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <fieldset visible="false" runat="server">
        <legend style="cursor: pointer" onclick="$('#pnlDadosProfissionais').show('slow')">Dados
            Profissionais:</legend>
        <div id="pnlDadosProfissionais" style="display: none">
            <table>
                <tr>
                    <td>Empresa:<br />
                        <asp:TextBox ID="txtCompanyName" MaxLength="50" Columns="20" runat="server"></asp:TextBox>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp; </td>
                    <td>Telefone:<br />
                        <asp:TextBox ID="txtCompanyPhone" Plugin="mask" Mask="(99)9999-9999" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>Salário:<br />
                        <uc3:CurrencyField ID="ucCurrFieldSalary" runat="server" />
                        &nbsp;&nbsp;&nbsp; </td>
                    <td>Data de Admissão:<br />
                        <uc2:Date ID="ucDtAdmissionDate" runat="server" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>Profissão:<br />
                        <asp:TextBox ID="txtProfission" runat="server" MaxLength="50" Columns="35"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>Cargo:<br />
                        <asp:TextBox ID="txtPost" runat="server" MaxLength="50" Columns="35"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <uc1:Address ID="companyAddress" ValidationGroup="_none" runat="server" FieldsetTitle="Endereço:" />
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
</div>
<VFX:BusinessManagerDataSource ID="odsEducation" runat="server" ConflictDetection="CompareAllValues"
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetAllEducationLevel"
    TypeName="Vivina.Erp.BusinessRules.ProfileManager">
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsMaritalStatus" runat="server" ConflictDetection="CompareAllValues"
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetMaritalStatus"
    TypeName="Vivina.Erp.BusinessRules.ProfileManager">
</VFX:BusinessManagerDataSource>
<script type="text/javascript">
    function ConsultaReceitaFederal(cpf) {
        window.open("http://www.receita.fazenda.gov.br/Aplicacoes/ATCTA/CPF/ConsultaPublica.asp?cpf=" + cpf, "JANELA", "scrollbars=yes, height=480, width=680");
    }
</script>
