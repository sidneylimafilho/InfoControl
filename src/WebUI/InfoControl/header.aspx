<%@ Page Language="C#" EnableViewState="true" AutoEventWireup="true" Inherits="Commons_header"
    CodeBehind="header.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html id="clean" xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    
    <link type="text/css" rel="Stylesheet" href="~/app_shared/themes/glasscyan/filescombiner.ascx" />


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
        
    <style type="text/css">
        body
        {
            background-color: transparent;
        }
        .startPage
        {
            position: absolute;
            height: 16px;
            width: 16px;
            right: 5px;
            margin-top: -26px;
            background: url(../App_Shared/themes/glasscyan/menu/goToStartPage.gif) no-repeat right top;
        }
        .startPage:hover
        {
            background-position: 0px -16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table id="header" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td id="left">
                <img id="imgLogo" src="../Site/LogoHandler.aspx?<%=Guid.NewGuid() %>" class="logoCliente"></img>
                <div id="menu">
                    <a href="<%=System.Web.SiteMap.RootNode.Url %>" target="content" class="search" title="Página Inicial">
                    </a><a plugin="lightbox" href="Appointments.aspx?lightbox[iframe]=true" target="content"
                        class="calendar" title="Agenda / Calendário"></a><a plugin="lightbox" href="Tasks.aspx?lightbox[iframe]=true"
                            target="content" class="tasks" title="Tarefas"></a><a plugin="lightbox" href="http://vivina.com.br/site/Ajuda/InfoControl-ERP,33.aspx?lightbox[iframe]=true"
                                target="content" class="help" title="Ajuda"></a>
                </div>
            </td>
            <td align="center" valign="top">
                &nbsp;
            </td>
            <td id="right">
                <div id="Div1" style="position: absolute; top: 4px; right: 329px; color: White; height: 100%;
                    text-align: right; z-index: 2">
                    <img src="../App_Shared/themes/glasscyan/logo_infocontrol.gif" alt="loading" />
                </div>
                <div id="loading" style="display: none; position: absolute; top: 4px; right: 329px;
                    color: White; height: 100%; text-align: right; z-index: 2">
                    <img src="../App_Shared/themes/glasscyan/loading3.gif" alt="loading" />
                </div>
                <asp:LoginName ID="LoginName1" runat="server" FormatString="&lt;b&gt;Usu&aacute;rio:&lt;/b&gt; {0}" />
            </td>
        </tr>
    </table>
    <a id="fechar" href="../logoff.aspx?ReturnUrl=http://infocontrol.com.br" target="_top">
        &nbsp; </a>
    <%-- <img alt="Fale Conosco" style="position: absolute; top: 26px; right: 438px;" src="../App_Shared/themes/glasscyan/user_headset.gif"
        onclick="window.open('http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=f0d8f6e6831bbd@apps.messenger.live.com&mkt=pt-PT&useTheme=true&foreColor=333333&backColor=DCF2E5&linkColor=333333&borderColor=8ED4AB&buttonForeColor=2C0034&buttonBackColor=CFE9D9&buttonBorderColor=8ED4AB&buttonDisabledColor=CFE9D9&headerForeColor=006629&headerBackColor=92D6AE&menuForeColor=006629&menuBackColor=FFFFFF&chatForeColor=333333&chatBackColor=F4FBF7&chatDisabledColor=F6F6F6&chatErrorColor=760502&chatLabelColor=6E6C6C', null, 'width=620, height=420');" />
    --%>
    <img alt="Crie sua sugestão" style="position: absolute; top: 56px; right: 438px;
        cursor: pointer" src="../App_Shared/themes/glasscyan/user_message.gif" plugin="lightbox"
        source="CRM/CustomerCall.aspx?lightbox[iframe]=true" />
    </form>
</body>
</html>
