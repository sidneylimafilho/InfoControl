<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_Administration_Agenda" CodeBehind="Appointments.aspx.cs"
    Title="Agenda" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/InfoControl/RH/SelectEmployee.ascx" TagName="SelectEmployee"
    TagPrefix="uc3" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc6" %>
<%@ Register Src="../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc4" %>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <br />
    <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
            Escolha o filtro desejado: </legend><div class="body">
                <table>
                    <tr>
                        <td>
                            Selecione um empregado ou digite TODOS:<br />
                            <asp:TextBox ID="txtEmployee" runat="server" CssClass="cDynDat11" Width="300px"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            Selecione a data:
                            <uc4:Date ID="selDate" Required="true" ValidationGroup="filter" runat="server" />
                        </td>
                        <td>
                            Competências:<br />
                            <asp:DropDownList ID="cboCompetency" runat="server" DataSourceID="odsCompetencies"
                                AppendDataBoundItems="true" DataTextField="CompetencyName">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <VFX:BusinessManagerDataSource ID="odsCompetencies" runat="server" SelectMethod="GetEmployeeCompetenciesByCompany"
                                TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="odsCompetencies_Selecting">
                                <selectparameters>
                                    <asp:Parameter Name="companyId" Type="Int32" />
                                </selectparameters>
                            </VFX:BusinessManagerDataSource>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
                <span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                    &nbsp;</span>
                <asp:Button ID="btnSelectDate" ValidationGroup="filter" OnClick="btnSelectDate_Click"
                    runat="server" Text="Atualizar" />
            </span>
        <br />
        <br />
        <br />
    </fieldset>
    <br />
    <br />
    <br />
    <table width="100%">
        <tr>
            <td valign="top">
                <telerik:RadScheduler ID="radAppointment" runat="server" Culture="Portuguese (Brazil)"
                    DataEndField="FinishDate" DataKeyField="TaskId" DataSourceID="odsAppointment"
                    DataStartField="StartDate" DataSubjectField="Name" OverflowBehavior="Expand"
                    DisplayDeleteConfirmation="true" Localization-ConfirmDeleteTitle="" Localization-ConfirmDeleteText="Tem certeza que quer excluir esse agendamento?"
                    Skin="Office2007" OnAppointmentDelete="radAppointment_AppointmentDelete" StartInsertingInAdvancedForm="true"
                    OnFormCreated="radAppointment_FormCreated" EnableTheming="True" EditFormDateFormat="d/M/yyyy"
                    Width="100%" SelectedDate="2008-08-08" ShowAllDayRow="False" DayEndTime="22:00:00"
                    OnAppointmentUpdate="radAppointment_AppointmentUpdate" MonthVisibleAppointmentsPerDay="1"
                    AllowEdit="true">
                    <ResourceTypes>
                        <telerik:ResourceType KeyField="UserId" TextField="AbreviatedName" ForeignKeyField="UserId"
                            DataSourceID="odsEmployeesList" Name="UserName" />
                    </ResourceTypes>
                    <Localization AllDay="O Dia Todo" HeaderDay="Dia" HeaderMonth="Mês" HeaderNextDay="Próximo Dia"
                        HeaderPrevDay="Dia Anterior" HeaderToday="Hoje" HeaderWeek="Semana" Show24Hours="Apresenta 24 horas" />
                    <MonthView VisibleAppointmentsPerDay="1"></MonthView>
                    <AppointmentTemplate>
                        <div class="<%#Eval("Subject").ToString().StartsWith("OS")?"serviceorder":""%> 
                        <%#Convert.ToDateTime(Eval("End")) < DateTime.Now?"delayed":""%>">
                            <a href='Task.aspx?TaskId=<%#Eval("ID").EncryptToHex() %>&app=true'>
                                <%#Eval("Subject")%>
                            </a>
                        </div>
                    </AppointmentTemplate>
                </telerik:RadScheduler>
            </td>
        </tr>
    </table>
    <ajaxToolkit:AutoCompleteExtender ID="txtEmployee_AutoCompleteExtender" runat="server"
        CompletionSetCount="5" MinimumPrefixLength="2" ServiceMethod="SearchUser" ServicePath="~/Controller/SearchService"
        TargetControlID="txtEmployee" DelimiterCharacters="" Enabled="True">
    </ajaxToolkit:AutoCompleteExtender>
    <VFX:BusinessManagerDataSource runat="server" ID="odsAppointment" OnSelecting="odsAppointment_Selecting"
        SelectMethod="GetTasks" TypeName="Vivina.Erp.BusinessRules.TaskManager" OldValuesParameterFormatString="original_{0}"
        EnablePaging="False">
        <selectparameters>
            <asp:Parameter Name="userId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />           
            <asp:Parameter Name="status" Type="Object" />
            <asp:Parameter Name="filterType" Type="Object" />
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="dtInterval" Type="Object" />
            <asp:Parameter Name="parentId" Type="Int32" />
            <asp:Parameter Name="subjectId" Type="Int32" />
            <asp:Parameter Name="pageName" Type="String" />
            <asp:Parameter Name="competency" Type="String" />
            <asp:Parameter Name="companyId" Type="Int32" />
         </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsTasksServiceOrder" onselecting="odsTasksServiceOrder_Selecting"
        SelectMethod="GetTaksByServiceOrder" TypeName="Vivina.Erp.BusinessRules.TaskManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="serviceOrderId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsEmployeesList" OnSelecting="odsEmployees_Selecting"
        SelectMethod="GetEmployeesAsProfileByCompany" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        OldValuesParameterFormatString="original_{0}" onselected="odsEmployeesList_Selected">
        <selectparameters>
<asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
                </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
