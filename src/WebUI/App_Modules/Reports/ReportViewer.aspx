<%@ Page Language="C#" AutoEventWireup="true" Inherits="ReportViewer" Codebehind="ReportViewer.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Visualizador de Relatórios</title>
    <style type="text/css">
     html, body, form, #ReportViewer1 {border:0px; padding:0px; margin:0px; height:100%; width:100%;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="96%" Width="100%"
        Font-Names="Verdana" ShowPromptAreaButton="False" ShowRefreshButton="False" ShowParameterPrompts="False"
        ShowPageNavigationControls="True" ShowFindControls="False" ShowDocumentMapButton="False"
        ShowCredentialPrompts="False">
        <LocalReport DisplayName="teste">
        </LocalReport>
    </rsweb:ReportViewer>
    <script>
    document.getElementById("ReportViewer1").style.display="";
    </script>
    </form>
</body>
</html>
