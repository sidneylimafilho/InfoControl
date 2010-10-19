<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Host_ScheduleTask" Title="Agendar Tarefa" EnableEventValidation="false"
    CodeBehind="ScheduleTask.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
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
                <td class="left" style="height: 97px">
                    &nbsp;
                </td>
                <td class="center" style="height: 97px">
                    Nome:<br />
                    <asp:TextBox runat="server" ID="txtName" MaxLength="120" Columns="50"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="reqTxtName" ControlToValidate="txtName"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <table>
                        <tr>
                            <td>
                                Início:<br />
                                <uc1:Date ID="ucPeriodDate" Required="true" ShowTime="true" runat="server" ValidationGroup="Save" />
                            </td>
                            <td>
                                Período em Minutos:
                                <br />
                                <uc3:CurrencyField ID="ucCurrFieldTxtPeriod" runat="server" ValidationGroup="Save"
                                    Mask="9999" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkEnabled" Text="Habilitada" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                Nome da Classe com Namespace:<br />
                                <asp:TextBox runat="server" ID="txtTypeFullName" Columns="80" MaxLength="200" ValidationGroup="Save"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="ReqtxtTypeFullName" ControlToValidate="txtTypeFullName"
                                    ErrorMessage="&nbsp;&nbsp;&nbsp;" CssClass="cErr21" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Último Status:<br />
                                <%--<asp:Label runat="server" ID="lblLastRunStatus" BorderWidth="1" Height="50" Width="510"></asp:Label>--%>
                                <div style="overflow: auto">
                                     <textarea plugin="htmlbox" runat="server"  ID="txtLastRunStatus" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" style="text-align: right">
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Salvar" ValidationGroup="Save" CausesValidation="true"
                                    OnClick="btnSave_Click" />
                                <asp:Button runat="server" ID="btnCancel" Text="Cancelar" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="right" style="height: 97px">
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
        <VFX:BusinessManagerDataSource ID="odsScheduledTask" runat="server" onselecting="odsScheduledTask_Selecting"
            SelectMethod="GetScheduleTask" TypeName="InfoControl.Web.ScheduledTasks.SchedulerManager">
            <selectparameters>
                <asp:Parameter Name="ScheduleTaskId" Type="Int32" />
            </selectparameters>
        </VFX:BusinessManagerDataSource>
        <%--<VFX:BusinessManagerDataSource ID="odsScheduleTask" runat="server" SelectMethod="GetScheduleTask"
            TypeName="InfoControl.Web.ScheduledTasks.SchedulerManager" ConflictDetection="CompareAllValues"
            DataObjectTypeName="InfoControl.Web.ScheduledTasks.ScheduledTask" UpdateMethod="Update"
             OldValuesParameterFormatString="original_{0}"
            oninserted="odsScheduleTask_Inserted" onupdated="odsScheduleTask_Updated" onselecting="odsScheduleTask_Selecting"
            DeleteMethod="Delete" InsertMethod="Insert">
            <updateparameters>
                <asp:Parameter Name="original_entity" Type="Object" />
                <asp:Parameter Name="entity" Type="Object" />
            </updateparameters>
            <selectparameters>
                <asp:Parameter Name="ScheduleTaskId" Type="Int32" />
            </selectparameters>
        </VFX:BusinessManagerDataSource>--%>
    </div>
</asp:Content>
