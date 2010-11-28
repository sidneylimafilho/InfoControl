<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    Inherits="InfoControl_CRM_CustomerFollowup" CodeBehind="CustomerFollowup.aspx.cs"
    Title="FollowUp" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/App_Shared/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="SelectContact.ascx" TagName="SelectContact" TagPrefix="uc2" %>
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
                <table width="100%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <uc2:SelectContact ID="selContact" OnSelectedContact="selContact_SelectedContact"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ID="reqSelContact" ErrorMessage="&nbsp&nbsp&nbsp"
                                            ControlToValidate="selContact"> </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            Ação:<br />
                            <asp:DropDownList ID="cboCustomerFollowupAction" runat="server" DataSourceID="odsCustomerFollowupAction"
                                DataTextField="Name" DataValueField="CustomerFollowupActionId" AppendDataBoundItems="true"
                                Width="200">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="txtEntryDate" runat="server"></asp:Literal>
                        </td>
                        <tr>
                            <td>
                                Assunto do Agendamento:
                                <br>
                                <asp:TextBox runat="server" ID="txtAppoitmentSubject" Width="85%" />
                                &nbsp&nbsp <span class='cErr21' id="reqTxtAppoitmentSubject" style='color: Red; display: none'>
                                    &nbsp&nbsp&nbsp </span>
                            </td>
                            <td>
                                Próximo Encontro:
                                <uc1:Date ID="ucNextMeetingDate" runat="server" ShowTime="true" />
                            </td>
                        </tr>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Assunto:
                            <br>
                            <asp:TextBox runat="server" Height="200px" TextMode="MultiLine" Width="70%" ID="txtDescription" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button runat="server" ID="btnSave" OnClientClick="return false;" Text="Salvar" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='CustomerFollowups.aspx'; return false;" />
                        </td>
                    </tr>
                </table>
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
    <input type="hidden" runat="server" id="companyId"></input>
    <input type="hidden" runat="server" id="userId"></input>
    <input type="hidden" runat="server" id="txtContactId" name="contactId"> </input>
    <input type="hidden" runat="server" id="customerFollowUpId"></input>
    <VFX:BusinessManagerDataSource runat="server" ID="odsCustomerFollowupAction" onselecting="odsCustomerFollowupAction_Selecting"
        SelectMethod="GetCustomerFollowupActions" TypeName="Vivina.Erp.BusinessRules.CustomerManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
