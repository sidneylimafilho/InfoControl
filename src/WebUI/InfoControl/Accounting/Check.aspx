<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Accounting_Check" Title="Untitled Page" Codebehind="Check.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Controle de Cheques <a class="HelpTip" href="javascript:void(0);">
            <img id="Img2" runat="server" border="0" src="~/App_themes/_global/ico_ajuda.gif" />
            <span class="msg">• Cadastre os seus cheques emitidos e recebidos. Em breve, o InfoControl
                trará uma ferramenta para o gerenciamento completo destes cheques. Isso evitará
                inadimplência. <span class="footer"></span></span></a>
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
                <asp:FormView ID="frmCheck" runat="server" DataSourceID="odsCheck" DefaultMode="Insert"
                    Width="100%" OnItemCommand="frmCheck_ItemCommand" DataKeyNames="CheckId,EntryDate,Sender,CheckValue,CheckNumber,Agency,Returns,BankId,CompanyId">
                    <EditItemTemplate>
                        Cheque de:<br />
                        <asp:TextBox ID="SenderTextBox" runat="server" Text='<%# Bind("Sender") %>' Columns="40"
                            MaxLength="40" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                            runat="server" ControlToValidate="SenderTextBox"></asp:RequiredFieldValidator>
                        <br />
                        Valor do Cheque:<br />
                        <asp:TextBox ID="CheckValueTextBox" runat="server" Text='<%# Bind("CheckValue", "{0:###,##0.00}") %>'
                            Columns="15" MaxLength="10" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                            runat="server" ControlToValidate="CheckValueTextBox"></asp:RequiredFieldValidator>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="False"
                            InputDirection="RightToLeft" Mask="999,999.99" MaskType="Number" TargetControlID="CheckValueTextBox">
                        </ajaxToolkit:MaskedEditExtender>
                        <br />
                        Número do Cheque:<br />
                        <asp:TextBox ID="CheckNumberTextBox" runat="server" Text='<%# Bind("CheckNumber") %>'
                            Columns="10" MaxLength="10" />
                        <br />
                        Banco:<br />
                        <asp:DropDownList ID="cboBankId" SelectedValue='<%# Bind("BankId") %>' runat="server"
                            DataSourceID="odsBanks" DataTextField="Name" DataValueField="BankId" AppendDataBoundItems="true">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                            runat="server" ControlToValidate="cboBankId"></asp:RequiredFieldValidator>
                        <br />
                        Agência:<br />
                        <asp:TextBox ID="AgencyTextBox" runat="server" Text='<%# Bind("Agency") %>' Columns="10"
                            MaxLength="10" />
                        <br />
                        <br />
                        <asp:CheckBox ID="ReturnsCheckBox" runat="server" Checked='<%# Bind("Returns") %>'
                            Text=" Voltou ?" />
                        <br />
                        <div style="text-align: right">
                            <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                CssClass="cBtn11" Text="Inserir" Visible="<%# frmCheck.CurrentMode == FormViewMode.Insert %>">
                                <%--permissionRequired="Check">--%></asp:Button>
                            <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="cBtn11" Text="Salvar" Visible="<%# frmCheck.CurrentMode == FormViewMode.Edit %>">
                                <%--permissionRequired="Check">--%></asp:Button>
                            <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="cBtn11" Text="Cancelar"></asp:Button></div>
                    </EditItemTemplate>
                </asp:FormView>
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
    <VFX:BusinessManagerDataSource ID="odsBanks" runat="server" SelectMethod="GetAllBanksWithNumbers"
        TypeName="Vivina.Erp.BusinessRules.BankManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCheck" runat="server" ConflictDetection="CompareAllValues"
        DataObjectTypeName="Vivina.Erp.DataClasses.Check" InsertMethod="Insert"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetCheck" TypeName="Vivina.Erp.BusinessRules.CheckManager"
        UpdateMethod="Update" oninserted="odsCheck_Inserted" oninserting="odsCheck_Inserting"
        onselecting="odsCheck_Selecting" onupdated="odsCheck_Updated" onupdating="odsCheck_Updating">
        <updateparameters>
			<asp:parameter Name="original_entity" Type="Object" />
			<asp:parameter Name="entity" Type="Object" />
		</updateparameters>
        <selectparameters>
			<asp:parameter Name="CheckId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
