<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    Inherits="InfoControl_Services_Service" Title="Serviço" CodeBehind="Service.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <asp:FormView ID="frmServices" permissionRequired="Service" runat="server" DataSourceID="odsServices"
                    DefaultMode="Insert" DataKeyNames="ServiceId,CompanyId,Name,Price,TimeInMinutes,ISS"
                    Width="100%" OnItemInserting="frmServices_ItemInserting" OnItemUpdating="frmServices_ItemUpdating"
                    OnItemCommand="frmServices_ItemCommand">
                    <EditItemTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    Descrição:<br />
                                    <asp:TextBox ID="txtName" Columns="60" Rows="5" Width="300" runat="server" Text='<%# Bind("Name") %>'
                                        MaxLength="120" /><asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator1" ErrorMessage="&nbsp&nbsp&nbsp"
                                            runat="server" ControlToValidate="txtName"  ValidationGroup="Service"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="40%">
                                        <tr>
                                            <td>
                                                Preço(R$):<br />
                                                <uc3:CurrencyField ID="ucCurrFieldServicePrice" Required="true" Text='<%# Bind("Price") %>'
                                                    ValidationGroup="Service" runat="server" />                                               
                                            </td>
                                            <td>
                                                Tempo Estimado:<br />
                                                <asp:TextBox ID="txtTimeInMinutes" Mask="99999" runat="server" Text='<%# Bind("TimeInMinutes") %>'
                                                    Columns="5" />
                                                &nbsp;&nbsp;&nbsp;min
                                                <asp:RequiredFieldValidator CssClass="cErr21" ID="valTxtTimeInMinutes" ErrorMessage="&nbsp&nbsp&nbsp"
                                                    runat="server" ControlToValidate="txtTimeInMinutes"  ValidationGroup="Service" />                                               
                                            </td>
                                            <td>
                                                ISS(%):<br />
                                                <uc3:CurrencyField ID="ucCurrFieldISS" Mask="99.99" Text='<%# Bind("ISS") %>' ValidationGroup="Service"
                                                    runat="server" />                                               
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="text-align: right">
                                        <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                            ValidationGroup="Service" CssClass="cBtn11" Text="Inserir" Visible="<%# frmServices.CurrentMode == FormViewMode.Insert %>" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                            ValidationGroup="Service" CssClass="cBtn11" Text="Salvar" Visible="<%# frmServices.CurrentMode == FormViewMode.Edit %>" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="cBtn11" Text="Cancelar" OnClick="CancelButton_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </EditItemTemplate>
                </asp:FormView>
                <!-- Conteudo -->
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
    <VFX:BusinessManagerDataSource runat="server" ID="odsServices" SelectMethod="GetService"
        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager" UpdateMethod="UpdateService"
        oninserted="odsServices_Inserted" OldValuesParameterFormatString="original_{0}"
        onselecting="odsServices_Selecting" onupdating="odsServices_Updating" oninserting="odsServices_Inserting"
        ConflictDetection="CompareAllValues" DataObjectTypeName="Vivina.Erp.DataClasses.Service"
        InsertMethod="InsertService" MaximumRowsParameterName="" onupdated="odsServices_Updated"
        StartRowIndexParameterName="">
        <updateparameters>
            <asp:Parameter Name="original_entity" Type="Object" />
            <asp:Parameter Name="entity" Type="Object" />
        </updateparameters>
        <selectparameters>
            <asp:Parameter Name="ServiceId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
