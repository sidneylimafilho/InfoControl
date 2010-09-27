<%@ Page Language="C#" AutoEventWireup="true" Inherits="Site_Boleto" CodeBehind="Boleto.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="Boleto.Net" Namespace="BoletoNet" TagPrefix="bn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" id="clean">
<head runat="server">
    <title>Boleto Bancário</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--  <VFX:HtmlBoleto runat="server" ID="boleto" Carteira="18" DataVencimento="2008-02-20" 
            NossoNumero="4001210128" Valor="250" CedenteAgencia="0183-X" Aceite="true" 
            CedenteConta="24882" CedenteContaDV="7" CedenteNome="SYSLAND INFOGROUP LTDA" 
            Contrato="1341888" DataEmissao="2008-02-20" DataProcessamento="2008-02-20" 
            Documento="123" OutrasAcrescimos="0" pEspecieDoc="DM" Quantidade="0">
        </VFX:HtmlBoleto>--%>
        <bn:BoletoBancario ID="BoletoBancario" runat="server" >
        </bn:BoletoBancario>
        <script type="text/javascript">
            window.print();
        </script>
    </div>
    </form>
</body>
</html>
