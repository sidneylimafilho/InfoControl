<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/infocontrol/Default.master"

    AutoEventWireup="true" Inherits="ReportGenerator" Title=""
    CodeBehind="ReportGenerator.aspx.cs" %>

<%@ Register Src="ReportGenerator_Finish.ascx" TagName="ReportBuilder_Finish" TagPrefix="uc5" %>
<%@ Register Src="ReportGenerator_Sort.ascx" TagName="ReportBuilder_Sort" TagPrefix="uc4" %>
<%@ Register Src="ReportGenerator_Filter.ascx" TagName="ReportBuilder_Filter" TagPrefix="uc3" %>
<%@ Register Src="ReportGenerator_Start.ascx" TagName="DynamicReport_Start" TagPrefix="uc2" %>
<%@ Register Src="ReportGenerator_Columns.ascx" TagName="DynamicReport_Columns" TagPrefix="uc1" %>
<%@ Register Src="ReportGenerator_MatrixRows.ascx" TagName="ReportGenerator_MatrixRows"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="padding: 5px;">
        <h1>
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, AssistantReportingDynamic %>" />
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
                    <asp:Wizard ID="wzdMain" runat="server" ActiveStepIndex="2" CssClass="Wizard" FinishCompleteButtonText="Finalizar"
                        FinishPreviousButtonText="<%$ Resources: Resource, Back %>" StartNextButtonImageUrl="~/img/ico_editar.gif"
                        StartNextButtonText="<%$ Resources: Resource, GoForward %>" StepNextButtonImageUrl="~/img/ico_editar.gif"
                        StepNextButtonText="<%$ Resources: Resource, GoForward %>" StepPreviousButtonImageUrl="~/img/ico_editar.gif"
                        StepPreviousButtonText="<%$ Resources: Resource, Back %>" StepStyle-HorizontalAlign="Left"
                        StepStyle-VerticalAlign="Top" Width="100%" Height="250px" DisplaySideBar="False">
                        <StepStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        <WizardSteps>
                            <asp:WizardStep ID="wzrTela1" runat="server" Title="Inicio" StepType="Start">
                                <uc2:DynamicReport_Start ID="DynamicReport_Start1" runat="server" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="wzrTela2" runat="server" Title="Matrix Linhas">
                                <uc6:ReportGenerator_MatrixRows ID="ReportGenerator_MatrixRows1" runat="server" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="wzrTela3" runat="server" Title="Colunas">
                                <uc1:DynamicReport_Columns ID="ColumnsStep" runat="server" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="wzrTela4" runat="server" Title="Filtro">
                                <uc3:ReportBuilder_Filter ID="ReportBuilder_Filter1" runat="server" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="wzrTela5" runat="server" Title="Ordena&#231;&#227;o">
                                <uc4:ReportBuilder_Sort ID="ReportBuilder_Sort1" runat="server" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="wzrTela6" runat="server" Title="Finalizar" StepType="Finish">
                                <uc5:ReportBuilder_Finish ID="ReportBuilder_Finish1" runat="server" />
                            </asp:WizardStep>
                        </WizardSteps>
                        <NavigationButtonStyle CssClass="cBtn11" />
                        <FinishNavigationTemplate>
                            <table cellspacing="5" cellpadding="5" border="0">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="FinishPreviousButton" runat="server" CssClass="cBtn11" CausesValidation="False"
                                            CommandName="MovePrevious" Text="<%$ Resources: Resource, Back %> " />
                                    </td>
                                </tr>
                            </table>
                        </FinishNavigationTemplate>
                        <StartNavigationTemplate>
                        </StartNavigationTemplate>
                    </asp:Wizard>
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
    </div>
</asp:Content>
