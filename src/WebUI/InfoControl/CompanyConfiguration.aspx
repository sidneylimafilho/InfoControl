<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="Company_RH_HeaderFooter" Title="Configurações da Empresa" EnableEventValidation="false"
    CodeBehind="CompanyConfiguration.aspx.cs" %>

<%@ Register Src="../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href="CompanySymbol.aspx" target="tabContent">Logo</a></li>
            <li><a href="CompanyReports.aspx" target="tabContent">Relatórios</a></li>
            <li><a href="CompanyPrints.aspx" target="tabContent">Impressão</a></li>
            <li><a href="CompanySales.aspx" target="tabContent">Vendas</a></li>
            <li><a href="CompanyContracts.aspx" target="tabContent">Contrato</a></li>
            <li><a href="CompanyTemplates.aspx" target="tabContent">Modelos</a></li>
        </ul>
        <iframe id="tabContent" name="tabContent" src="CompanySymbol.aspx"></iframe>
    </div>
   
</asp:Content>
