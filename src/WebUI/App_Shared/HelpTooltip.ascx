<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpTooltip.ascx.cs"
    Inherits="Vivina.Erp.WebUI.App_Shared.HelpTooltip" %>
<span class="helpToolTip"><a href="javascript:void(0);" id="imgHelp" runat="server" class="HelpTip">&nbsp;&nbsp;
</a><span runat="server" id="lblHelpText" style="display:none;">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</span></span>