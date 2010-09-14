<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="Host_PaymentMethod"
    Title="Untitled Page" Codebehind="PaymentMethod.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <img src="../App_Themes/Glass/Company/paymentMethod.gif" alt="" />
    <table class="cLeafBox21" width="50%">
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
                <asp:FormView ID="frmPaymentMethod" runat="server" DataSourceID="odsPaymentMethod"
                    Width="100%" DataKeyNames="PaymentMethodId,Name,TransactionUrl,ModifiedDate"
                    DefaultMode="Insert" onitemcommand="frmPaymentMethod_ItemCommand">
                    <EditItemTemplate>
                        Nome:<br />
                        <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' Columns="30"
                            MaxLength="30" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="NameTextBox" CssClass="cErr21"></asp:RequiredFieldValidator>
                        <br />
                        Url da Operadora:<br />
                        <asp:TextBox ID="TransactionUrlTextBox" runat="server" Text='<%# Bind("TransactionUrl") %>'
                            Columns="50" MaxLength="50" />
                        <br />
                        <div style="text-align: right">
                            <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                CssClass="cBtn11" Text="Inserir" Visible="<%# frmPaymentMethod.CurrentMode == FormViewMode.Insert %>">
                                <%--permissionRequired="PaymentMethod">--%></asp:Button>
                            <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                CssClass="cBtn11" Text="Salvar" Visible="<%# frmPaymentMethod.CurrentMode == FormViewMode.Edit %>">
                                <%--permissionRequired="PaymentMethod">--%></asp:Button>
                            <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="cBtn11" Text="Cancelar"></asp:Button></div>
                    </EditItemTemplate>
                </asp:FormView>
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
    <VFX:BusinessManagerDataSource ID="odsPaymentMethod" runat="server" SelectMethod="GetPaymentMethod"
        TypeName="Vivina.Erp.BusinessRules.PaymentMethodManager" UpdateMethod="Update"
        DataObjectTypeName="Vivina.Erp.DataClasses.PaymentMethod" 
	InsertMethod="Insert" oninserted="odsPaymentMethod_Inserted" 
	oninserting="odsPaymentMethod_Inserting" 
	onselecting="odsPaymentMethod_Selecting" onupdated="odsPaymentMethod_Updated" 
	onupdating="odsPaymentMethod_Updating" ConflictDetection="CompareAllValues" 
        OldValuesParameterFormatString="original_{0}">
        <updateparameters>
			<asp:parameter Name="original_entity" Type="Object" />
			<asp:parameter Name="entity" Type="Object" />
		</updateparameters>
        <selectparameters>
			<asp:parameter Name="paymentMethodId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
