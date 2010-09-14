<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" EnableEventValidation="false"
    AutoEventWireup="true" Inherits="Company_Administration_Boletu"
    Title="" Codebehind="Boletu.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content runat="server" ContentPlaceHolderID="Header">
<style>body{background: none;}</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
  <%--  <VFX:HtmlBoleto runat="server" ID="Boleto" Carteira="18" DataVencimento="2008-02-20"
        NossoNumero="4001210128" Valor="250" CedenteAgencia="0183-X" CedenteConta="24882"
        CedenteContaDV="7" CedenteNome="SYSLAND INFOGROUP LTDA" Contrato="1341888" DataEmissao="2008-02-20"
        DataProcessamento="2008-02-20" Documento="123" OutrasAcrescimos="0" pEspecieDoc="DM"
        Quantidade="0">
    </VFX:HtmlBoleto>--%>
</asp:Content>
