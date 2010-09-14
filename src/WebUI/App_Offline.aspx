<%@ Page Language="C#" AutoEventWireup="True" StylesheetTheme="Site" Codebehind="App_Offline.aspx.cs"
    Inherits="App_Offline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sistema em MANUTENÇÃO!</title>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" LoadScriptsBeforeUI="false" CombineScripts="false">
        <Scripts>
            <asp:ScriptReference Path="~/App_Shared/js/jquery.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>
    <div id="loadingMessage" style="display: none;">
        <h1>
            InfoControl está entrando em modo de manutenção!</h1>
        Estamos começando o processo de troca de versão. Para sua segurança a sua operação
        não foi realizada, garantindo assim que os dados não sejam corrompidos.
        <br />
        <br />
        <span class="cTxt21b">Você será redirecionado em&nbsp;<span class="cTxt41b" id="seconds"></span>&nbsp;segundos!</span>
    </div>
    <div id="main">
        <div id="header">
        </div>
        <table border="0" style="border: 0px solid; border-collapse: collapse;">
            <tr>
                <td valign="top">
                    <div id="menu">
                    </div>
                </td>
                <td valign="middle">
                </td>
                <td valign="top">
                    <div id="impulse">
                    </div>
                </td>
            </tr>
        </table>
        <div id="footer">
            <img id="Img2" runat="server" src="~/App_Themes/_global/Ooops.gif" alt="Ooops" />
            <table class="cLeafBox21" width="500px">
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
                        <p class="cTxt12b">
                            Estamos realizando um upgrade no sistema! Em breve estaremos com mais e mais funcionalidades
                            no ar!</p>
                        <br />
                        Enquanto você aguarda! Você pode continuar se aprofundando no conhecimento de conceitos
                        na informatização de uma empresa, dicas do InfoControl, melhores práticas de bom
                        uso do computador, etc.
                        <center>
                            <h3>
                                <a href="http://blogdoinfocontrol.blogspot.com">Acesse nosso blog!</a></h3>
                        </center>
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
            ©<%=DateTime.Now.Year %>
            Todos os direitos reservados. Proibida a cópia total ou parcial.
        </div>
    </div>
    </form>
</body>

<script type="text/javascript">
var i=20;
function SetSeconds()
{
    top.$("#seconds").html(i--);   
    if(i==0)
    {
        top.location = window.location;
        i=-1;
    }
    else
    {             
        setTimeout("SetSeconds()", 1000);        
    }
}

if(top.$("#loading"))
{
    top.$("#loadingMessage").html($("#loadingMessage").html());
    top.$("#loading").fadeIn(3000);
    if(top != window)
        SetSeconds();
}    
</script>

</html>
