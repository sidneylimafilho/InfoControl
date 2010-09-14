<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Company_POS_PurchaseOrder" Title=" Processo de Compra" CodeBehind="PurchaseOrder.aspx.cs" %>

<%@ Register Src="PurchaseOrder_Product.ascx" TagName="PurchaseOrder_Product" TagPrefix="uc3" %>
<%@ Register Src="PurchaseOrder_Summary.ascx" TagName="PurchaseOrder_Summary" TagPrefix="uc4" %>
<%@ Register Src="PurchaseOrder_Finish.ascx" TagName="PurchaseOrder_Finish" TagPrefix="uc5" %>
<%@ Register Src="PurchaseOrder_Quotation.ascx" TagName="PurchaseOrder_Quotation"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox31" width="100%">
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
            <td class="center" align="left">
                <asp:Wizard ID="wzdPurchaseOrder" runat="server" BorderWidth="0px" CancelButtonText="Cancelar"
                    FinishCompleteButtonText="Cadastrar" FinishPreviousButtonText="Anterior" SkipLinkText="Pular os Links"
                    StartNextButtonText="Próxima" StepNextButtonText="Próxima" StepPreviousButtonText="Anterior"
                    ActiveStepIndex="0" Width="100%" OnNextButtonClick="wzdPurchaseOrder_NextButtonClick">
                    <StartNextButtonStyle CssClass="cBtn11" />
                    <FinishCompleteButtonStyle CssClass="cBtn11" />
                    <StepNextButtonStyle CssClass="cBtn11" />
                    <FinishPreviousButtonStyle CssClass="cBtn11" />
                    <FinishNavigationTemplate>
                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            CssClass="cBtn11" Text="Anterior" />
                    </FinishNavigationTemplate>
                    <StepPreviousButtonStyle CssClass="cBtn11" />
                    <CancelButtonStyle CssClass="cBtn11" />
                    <SideBarStyle Width="70px" Wrap="false" />
                    <WizardSteps>
                        <asp:WizardStep ID="ProspectProducts" runat="server" Title="Resumo">
                            <uc3:PurchaseOrder_Product ID="PurchaseOrder_Product1" runat="server" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="ProductQuotation" runat="server" Title="Cotações">
                            <uc1:PurchaseOrder_Quotation ID="PurchaseOrder_Quotation1" runat="server" />
                        </asp:WizardStep>
                       <%-- <asp:WizardStep ID="ProspectSummary" runat="server" Title="Sumário">
                            <uc4:PurchaseOrder_Summary ID="PurchaseOrder_Summary1" runat="server" />
                        </asp:WizardStep>--%>
                        <%--<asp:WizardStep ID="ProspectFinish" runat="server" Title="Decisão">
                            <uc5:PurchaseOrder_Finish ID="PurchaseOrder_Finish1" runat="server" />
                        </asp:WizardStep>--%>
                    </WizardSteps>
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
    <br />
    <br />
</asp:Content>
