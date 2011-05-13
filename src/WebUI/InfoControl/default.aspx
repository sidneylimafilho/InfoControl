<%@ Page Language="C#" Inherits="_Frame" CodeBehind="default.aspx.cs" %>

<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" id="clean" style="height: 100%; width: 100%;
overflow: hidden;">
<head id="head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link rel="shortcut icon" href="../site/1/img/infocontrol.ico" type="image/x-icon" />
    <link rel="Stylesheet" href="../App_Shared/JS/lightbox/themes/default/jquery.lightbox.css"
        type="text/css" />
    <link rel="Stylesheet" type="text/css" href="../app_shared/filescombiner.ascx?type=text/css&base=themes/glasscyan/&f=
        common.css, 
        text.css, 
        global.css,
        Leafbox/Cyan/Leafbox.css,
        Leafbox/Cyan/Leafbox.css,
        Leafbox/Orange/Leafbox.css,
        Leafbox/Purple/Leafbox.css,
        Leafbox/Yellow/Leafbox.css,
        controls/ToolTip/ToolTip.css,
        controls/Alert/Alert.css,
        controls/Notification/jquery.jgrowl.css,
        controls/button/button.css,
        controls/calendar/calendar.css,
        controls/calendar/jquery.ui.core.css,
        controls/calendar/jquery.ui.theme.css,
        controls/gridview/gridview.css,
        controls/menu/menu.css,
        controls/numericUpDown/numericUpDown.css,
        controls/RadScheduler/RadScheduler.css,
        controls/RadPanelbar/Outlook.css,
        controls/Rating/Rating.css,
        controls/TabStrip/Tabs.css,
        controls/TextBox/textcontrol.css,        
        controls/validator/validator.css               
        " />
    <link rel="Stylesheet" type="text/css" href="../App_Shared/js/lightbox/themes/default/jquery.lightbox.css" />
    <title></title>
</head>

<script type="text/javascript" src="/app_shared/filescombiner.ascx?base=js/&f=
        jquery.js,
        jquery.cookies.js,
        jquery.dimensions.js,
        jquery.jGrowl.js,
        jquery.meioMask.js,
        jquery.validate.js,
        jquery.tooltip.js,
        jquery.serializer.js,        
        jquery.UI.core.js,
        jquery.UI.widget.js,
        jquery.UI.position.js,
        jquery.UI.tabs.js,
        jquery.UI.autocomplete.js,
        jquery.UI.duallistbox.js,
        jquery.ui.datepicker.js,
        jquery.UI.htmlbox.js,
        jquery.notification.js,
        jquery.glob.js,
        jquery.glob.pt-br.js,        
        ../modules/Alerts/Alerts.svc.js,        
        lightbox/jquery.lightbox.js,      
        smartclient/src/jquery.smartclient.js"></script>

<script defer="defer" type="text/javascript">
    function ResetAll() { ResetHeader(); ResetContent(); }
    function ResetHeader() { header.location += '?'; }
    function ResetContent() { content.location = 'StartPage.aspx?' }

    window.onload = function() {
        document.getElementById('menu').style.height = (document.body.offsetHeight - 86) + "px";
        document.getElementById('content').style.height = (document.body.offsetHeight - 86) + "px";

        $("#loading").fadeOut(2000, function() {
            InitAlertMonitor(Alerts);
        });
    }

    function ScrollToTop() { self.scrollTo(0, 0); setTimeout("ScrollToTop()", 100); }
    ScrollToTop();

    $(document).ready(function() {
        $("#btnMenu").click(function() {
            if (!$(this).data("IsClosed")) {
                $(this).data("IsClosed", true);

                $(this).css('backgroundPosition', 'left 40%');
                $("#menuContainer").hide("slow");
            }
            else {
                $(this).data("IsClosed", false);

                $("#menuContainer").show("slow").parent().css('width', '180px');
                $(this).css('backgroundPosition', 'right 40%');
            }
        });
    });    
</script>

<body style="margin: 0px; padding: 0px; height: 100%; width: 100%;">
    <form id="form1" runat="server" style="height: 100%; width: 100%;">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePageMethods="true" ScriptMode="Release"
        CombineScripts="false">
        <scripts>
        </scripts>
        <services>
            <asp:ServiceReference Path="~/App_Shared/Tooltip.svc" />
            <asp:ServiceReference Path="~/App_Shared/modules/Alerts/Alerts.svc" />
        </services>
    </ajaxToolkit:ToolkitScriptManager>
    <div id="loading" class="cLoading11">
        <table style="width: 400px; height: 300px; margin-top: 100px;" align="center">
            <tr>
                <td>
                    <img src="../App_Shared/themes/glasscyan/loading3.gif" />
                </td>
                <td id="loadingMessage">
                    <h1>
                        O sistema está carregando!</h1>
                    <br />
                    Na primeira vez que o sistema carrega ele faz o download de várias partes da aplicação
                    que irão dar agilidade no funcionamento das operações!
                </td>
            </tr>
        </table>
    </div>
    <uc2:ToolTip ID="ToolTip2" runat="server" Indication="left" Left="255px" Top="0px"
        Title="Informação:" Message="Personalize seu sistema, adicionando a logo de sua empresa! <br /><br />Clique na guia Administração > Configurações!<br /><br />" />
    <uc2:ToolTip ID="ToolTip3" runat="server" Indication="top" Left="20px" Top="90px"
        Title="Informação:" Message="Ao passar o mouse em cima deste botão, um menu se abrirá com todas as suas opções de navegabilidade pelo sistema!"
        Visible="false" />
    <uc2:ToolTip ID="ToolTip4" runat="server" Indication="left" Left="210px" Top="290px"
        Title="Dica:" Message="Este é um botão dinâmico. Feito para facilitar a navegação pelo sistema. Clique uma vez para 'escondê-lo', e clique outra vez para 'exibi-lo'."
        Visible="true" />
    <uc2:ToolTip ID="ToolTip1" runat="server" Indication="top" Right="113px" Top="80px"
        Title="Dica:" Message="Clique nesse boneco roxo para nos contactar em caso de erro no sistema, reclamação, sugestão ou elogio. Mande uma mensagem para nós!"
        Visible="true" />
    <table border="0" cellpadding="0" cellspacing="0" style="height: 100%; width: 100%">
        <tr>
            <td colspan="99" valign="top" style="height: 86px">
                <iframe name="header" src="header.aspx" style="width: 100%; height: 86px;" frameborder="0"
                    scrolling="no" allowtransparency="yes"></iframe>
            </td>
        </tr>
        <tr>
            <td valign="top" style="height: 83%;">
                <div id="menuContainer" style="height: 100%; width: 180px;">
                    <iframe id="menu" name="menu" src="menu.aspx" frameborder="0" style="float: left;
                        height: 100%; overflow-x: hidden;" allowtransparency="yes"></iframe>
                </div>
            </td>
            <td style="height: 83%;">
                <div id="btnMenu" runat="server" class="closed">
                    &nbsp;</div>
            </td>
            <td valign="top" style="width: 100%; height: 83%">
                <iframe style="width: 100%; height: 100%; z-index: -1" id="content" name="content"
                    src="startpage.aspx" frameborder="0" scrolling="yes" allowtransparency="yes">
                </iframe>
            </td>
        </tr>
    </table>
    <%----%>
    </form>
</body>
</html>
