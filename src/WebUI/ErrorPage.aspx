<%@ Page Language="C#" AutoEventWireup="true" Inherits="ErrorPage" Codebehind="ErrorPage.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
</head>
<body style="width:94%; padding:2%;">
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </ajaxToolkit:ToolkitScriptManager>
    <img id="Img1" runat="server" src="~/App_Shared/themes/glasscyan/Ooops.gif" alt="Ooops" />
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
                <table class="cTab11 ErrorPanel">
                    <tr>
                        <td class="cTxt11" style="padding-left: 120px; text-align: center;" colspan="2">
                            Ocorreu um erro no procesamento da página, já enviamos o erro para o pessoal de
                            suporte do sistema.
                            <p>
                                Em breve esta praga estará exterminada!</p>
                            <br />
                            <p>
                                Desculpe o transtorno!</p>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="cTxt11" style="padding-left: 120px;">
                            <div onclick="$get('<%= LabelError.ClientID%>').style.display=($get('<%= LabelError.ClientID%>').style.display=='none'?'':'none');"
                                style="cursor: pointer; text-align: center;">
                                <img src="~/App_Shared/themes/glasscyan/debug_view.gif" alt="Visualizar o erro!" runat="server" /><br />
                                Exibir detalhes
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 10px;">
                            <div id="LabelError" runat="server" style="text-align: left;">
                                <asp:Literal ID="lblError" runat="server"></asp:Literal>
                            </div>
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
    </form>
</body>
</html>
