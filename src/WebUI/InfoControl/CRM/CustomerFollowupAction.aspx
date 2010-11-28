<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" Inherits="InfoControl_CRM_CustomerFollowupAction"
    MasterPageFile="~/InfoControl/Default.master" CodeBehind="CustomerFollowupAction.aspx.cs"
    Title="Ações de venda" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
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
                <!-- principal -->
                <asp:FormView ID="frmFollowupAction" runat="server" DataSourceID="odsCustomerFollowupAction"
                    Width="100%" DefaultMode="Insert" DataKeyNames="CustomerFollowupActionId,Name,Probability,CompanyId"
                    OnItemInserting="frmFollowupAction_ItemInserting" OnItemUpdating="frmFollowupAction_ItemUpdating"
                    OnItemCommand="frmFollowupAction_ItemCommand" OnItemInserted="frmFollowupAction_ItemInserted"
                    OnItemUpdated="frmFollowupAction_ItemUpdated">
                    <EditItemTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Nome:<br />
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="120" Text='<%# Bind("Name") %>' />
                                    <asp:RequiredFieldValidator CssClass="cErr21" ID="valName" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="FollowupAction"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Probabilidade: (%)
                                    <br />
                                    <asp:TextBox ID="txtProbability" runat="server" Text='<%# Bind("Probability") %>' />
                                    <ajaxToolkit:MaskedEditExtender AcceptNegative="Left" ID="mskTxtProbability" runat="server"
                                        TargetControlID="txtProbability" CultureName="pt-BR" InputDirection="RightToLeft"
                                        Mask="99.99" MaskType="Number">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                    <br />
                                    <br />
                                    <div style="text-align: right">
                                        <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                            ValidationGroup="FollowupAction" CssClass="cBtn11" Text="Inserir" Visible="<%# frmFollowupAction.CurrentMode == FormViewMode.Insert %>" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                            ValidationGroup="FollowupAction" CssClass="cBtn11" Text="Salvar" Visible="<%# frmFollowupAction.CurrentMode == FormViewMode.Edit %>" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="cBtn11" Text="Cancelar" />
                                    </div>
                                </td>
                            </tr>
                        </table>
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
    <VFX:BusinessManagerDataSource runat="server" ID="odsCustomerFollowupAction" SelectMethod="GetCustomerFollowupAction"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" UpdateMethod="UpdateCustomerFollowupAction"
        onselecting="odsCustomerFollowupAction_Selecting" ConflictDetection="CompareAllValues"
        DataObjectTypeName="Vivina.Erp.DataClasses.CustomerFollowupAction" OldValuesParameterFormatString="original_{0}"
        onupdating="odsCustomerFollowupAction_Updating" InsertMethod="InsertCustomerFollowupAction">
        <updateparameters>
            <asp:Parameter Name="original_entity" Type="Object" />
            <asp:Parameter Name="entity" Type="Object" />
        </updateparameters>
        <selectparameters>
            <asp:Parameter Name="customerFollowupActionId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
