<%@ Page Language="C#" AutoEventWireup="true" Inherits="App_Shared_comments" CodeBehind="comments.aspx.cs" %>

<%@ Register Src="Comments.ascx" TagName="Comments" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Comentários </title>
    <link id="Link1" href="~/App_Shared/themes/glasscyan/text.css" runat="server" type="text/css"
        rel="Stylesheet" />
    <link id="Link2" href="~/App_Shared/themes/glasscyan/Global.css" runat="server" type="text/css"
        rel="Stylesheet" />
    <link id="Link3" href="~/App_Shared/themes/glasscyan/Controls/BUtton/BUtton.css" runat="server"
        type="text/css" rel="Stylesheet" />
    <link id="Link4" href="~/App_Shared/themes/glasscyan/Controls/Textbox/TextControl.css" runat="server"
        type="text/css" rel="Stylesheet" />
    <script>
    var intervalID;
    //function Refresh(){ intervalID = setInterval(function(){ location +="&";}, 5000); }
    // Refresh();
    </script>
</head>
<body style="background: white;">
    <form id="form1" runat="server">
    <uc1:Comments ID="Comments1" runat="server" OpenInFrame="false" ShowButtons="true" />
    <br />
    <br />
    <br />
    </form>
</body>
</html>
