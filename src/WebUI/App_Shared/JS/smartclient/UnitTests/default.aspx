<%@ Page Language="C#" ValidateRequest="false" StylesheetTheme="GlassCyan" Inherits="InfoControl.Web.UI.Page" %>

<%@ Register Src='~/site/_modules/Menu.ascx' TagName='Menu' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/Categories.ascx' TagName='Categories' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/FeedReader.ascx' TagName='FeedReader' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/Products.ascx' TagName='Products' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/Tags.ascx' TagName='Tags' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/Search.ascx' TagName='Search' TagPrefix='vfx' %>
<%@ Register Src='~/App_Modules/AccessControl/Login.ascx' TagName='Login' TagPrefix='vfx' %>
<%@ Register Src='~/site/_modules/WebPage.ascx' TagName='WebPage' TagPrefix='vfx' %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vivina</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link rel="shortcut icon" href="themes/vivina/img/vivina.ico" type="image/x-icon" />
    <meta name='generator' content='Vivina InfoControl' />
</head>

<script type="text/javascript" src="../scriptCombiner.ascx"></script>

<body>
    <div >
        <fieldset>
            <legend>Navegação</legend>
            <a href="GridSample.ascx" target="#content" command="click">Grid</a> | 
            <a href="../../AlphabeticalPaging.ascx" target="#content" command="click">Alfa</a> |
            <a href="SampleAll.ascx" target="#content" command="click">Inicio</a>
        </fieldset>
        <div id="content" command="load" controller="SampleAll.ascx">
        
        
        </div>
    </div>
</body>
</html>
