<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" CodeBehind="Employee_ProfessionalData.aspx.cs"
    AutoEventWireup="true" Inherits="Vivina.Erp.WebUI.RH.Employee_ProfessionalData" %>

<%@ Register Src="../../App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc4" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc5" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <%--Conteudo--%>
                <table width="100%">
                    <tr>
                        <td valign="top">
                            <fieldset>
                                <legend>Relação Institucional</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Organograma:<br />
                                            <uc2:ComboTreeBox ID="cboOrgLevel" DataSourceID="odsOrganizationLevel" DataFieldParentID="ParentId"
                                                DataFieldID="OrganizationLevelId" DataValueField="OrganizationLevelId" DataTextField="Name"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Situação Funcional:<br />
                                            <asp:RadioButton ID="rbtActive" runat="server" Checked="True" GroupName="status"
                                                Text="Ativo"></asp:RadioButton>&#160;&#160;
                                            <asp:RadioButton ID="rbtAway" runat="server" GroupName="status" Text="Afastado">
                                            </asp:RadioButton>
                                        </td>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlEmployeeAway" Style="display: none" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Data do Afastamento:<br />
                                                            <uc4:Date ID="ucDtEmployeeAwayDate" ValidationGroup="Save" runat="server" />
                                                        </td>
                                                        <td>
                                                            Motivo do Afastamento:<br />
                                                            <asp:DropDownList ID="cboEmployeeAlienationCause" runat="server" DataSourceID="odsAlienation"
                                                                DataTextField="Name" DataValueField="AlienationId">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CTPS:<br />
                                            <asp:TextBox ID="txtCtps" runat="server" Columns="12" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td>
                                            Série:<br />
                                            <asp:TextBox ID="txtCtpsSerial" runat="server" Columns="12" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td>
                                            Pis:<br />
                                            <asp:TextBox ID="txtPis" runat="server" Columns="12" MaxLength="18"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Matrícula:<br />
                                            <asp:TextBox ID="txtEnrollment" runat="server" Columns="12" MaxLength="20" />
                                        </td>
                                        <td>
                                            Data de Admissão:<br />
                                            <uc4:Date ID="ucDtAdmissionDate" Required="true" ValidationGroup="Save" runat="server" />
                                        </td>
                                        <td>
                                            Vínculo:<br />
                                            <asp:DropDownList ID="cboBond" runat="server" AppendDataBoundItems="True" DataSourceID="odsBond"
                                                DataTextField="Name" DataValueField="BondId">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Salário:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldSalary" Required="true" ValidationGroup="Save"
                                                runat="server" />
                                        </td>
                                        <td>
                                            H/H:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldtxtHH" runat="server" />
                                        </td>
                                        <td>
                                            Comissão(%):<br />
                                            <uc5:CurrencyField ID="ucCurrComission" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Posto de Trabalho</legend>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            Cargo:<br />
                                            <asp:DropDownList ID="cboPost" runat="server" AppendDataBoundItems="True" DataSourceID="odsPosts"
                                                DataTextField="Name" DataValueField="PostId">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsSalesPerson" runat="server" Text="Vendedor?"></asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsTechnical" Text="Técnico?" runat="server"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Dados Bancários</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Banco:<br />
                                            <asp:DropDownList ID="cboBank" runat="server" AppendDataBoundItems="True" DataSourceID="odsBank"
                                                DataTextField="Name" DataValueField="BankId">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Agência:<br />
                                            <asp:TextBox ID="txtAgency" Columns="10" MaxLength="10" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            Numero da conta:<br />
                                            <asp:TextBox ID="txtAccountingNumber" runat="server" Columns="15" MaxLength="10"
                                                TabIndex="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Organização do Trabalho</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Turno:<br />
                                            <asp:DropDownList ID="cboShift" runat="server" AppendDataBoundItems="True" DataSourceID="odsShift"
                                                DataTextField="Name" DataValueField="ShiftId">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Jornada de Trabalho:<br />
                                            <asp:DropDownList ID="cboWorkJourney" runat="server" AppendDataBoundItems="True"
                                                DataSourceID="odsWorkJourney" DataTextField="Name" DataValueField="WorkJourneyId">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Carga Horária:<br />
                                            <uc5:CurrencyField ID="ucCurrrFieldHoursWeek" Mask="999" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Início da Jornada:<br />
                                            <asp:TextBox ID="txtJourneyBegin" Plugin="Mask" Mask="99:99" Columns="2" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator CssClass="cErr21" ErrorMessage="&nbsp&nbsp&nbsp" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ID="regtxtJourneyBegin" runat="server" ControlToValidate="txtJourneyBegin"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            Fim da Jornada:<br />
                                            <asp:TextBox ID="txtJourneyEnd" Plugin="Mask" Mask="99:99" Columns="2" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator CssClass="cErr21" ErrorMessage="&nbsp&nbsp&nbsp" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ID="regtxtJourneyEnd" runat="server" ControlToValidate="txtJourneyEnd"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Início do intervalo:<br />
                                            <asp:TextBox ID="txtIntervalBegin" Plugin="Mask" Mask="99:99" Columns="2" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator CssClass="cErr21" runat="server" ControlToValidate="txtIntervalBegin"
                                                ErrorMessage="&nbsp&nbsp&nbsp" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$" />
                                        </td>
                                        <td>
                                            Fim do intervalo:<br />
                                            <asp:TextBox ID="txtIntervalEnd" Plugin="Mask" Mask="99:99" Columns="2" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator CssClass="cErr21" ErrorMessage="&nbsp&nbsp&nbsp" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ID="regtxtIntervalEnd" runat="server" ControlToValidate="txtIntervalEnd"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Outros </legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Insalubridade:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldHealthFuless" runat="server" />
                                        </td>
                                        <td>
                                            Periculosidade:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldHazardPay" runat="server" />
                                        </td>
                                        <td>
                                            FGTS:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldFGTS" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Outros Rendimentos :<br />
                                            <uc5:CurrencyField ID="ucCurrFieldOtherIncomes" runat="server" />
                                        </td>
                                        <td>
                                            Anuênio:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldAnuency" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Horas por Semana:<br />
                                            <uc5:CurrencyField ID="ucCurrFieldWorkingHours" Mask="9999" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </fieldset>

                            <fieldset runat="server" id="pnlAdditionalFields">
                                <legend>Campos Adicionais </legend>
                                <asp:DataList ID="dtlAdditionalInformation" runat="server" DataSourceID="odsAdditionalInformation"
                                    OnItemDataBound="dtlAdditionalInformation_ItemDataBound" RepeatColumns="3" Width="100%"
                                    DataKeyField="AddonInfoId" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddonInfoId" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblAddonInfoText" Text='<%#Bind("Name") %>' runat="server"> </asp:Label>
                                        <br />
                                        <asp:DropDownList ID="AddonInfoComboBox" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Value="" Text=" "> </asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:DataList>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right; margin-right: 8px">
                    <asp:Button ID="btnSave" runat="server" CausesValidation="True" CommandName="Update"
                        CssClass="cBtn11" Text="Salvar" ValidationGroup="Save" _permissionRequired="Employee"
                        OnClick="btnSave_Click"></asp:Button>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="cBtn11" Text="Cancelar" OnClick="CancelButton_Click"></asp:Button>
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
    <script>

        $(function () {

            var rbtActive = $get('<%=rbtActive.ClientID %>');
            var rbtAway = $get('<%=rbtAway.ClientID %>');
            var pnlEmployeeAway = $get('<%=pnlEmployeeAway.ClientID %>');

            if (rbtAway.checked)
                $(pnlEmployeeAway).show();

            $(rbtActive).click(function () {
                $(pnlEmployeeAway).hide();
            });

            $(rbtAway).click(function () {
                $(pnlEmployeeAway).show();
            });

        });
    
    </script>
    <VFX:BusinessManagerDataSource ID="odsOrganizationLevel" runat="server" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        SelectMethod="GetAllOrganizationLevelToDataTable" OnSelecting="odsOrganizationLevel_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32">
            </asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAlienation" runat="server" SelectMethod="GetAlienations"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBond" runat="server" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        SelectMethod="GetBond">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsPosts" runat="server" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        OnSelecting="odsEmployeeFunction_Selecting" SelectMethod="GetPosts">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32">
            </asp:Parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBank" runat="server" TypeName="Vivina.Erp.BusinessRules.BankManager"
        SelectMethod="GetAllBanks">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsShift" runat="server" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        SelectMethod="GetShifts">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsWorkJourney" runat="server" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        SelectMethod="GetWorkJourneys">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAdditionalInformation" runat="server" OnSelecting="odsAdditionalInformation_Selecting"
        SelectMethod="GetAdditionalInformationToDataTable" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
