<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencyField.ascx.cs"
    Inherits="InfoControl.Web.UI.CurrencyField" %>
<asp:TextBox ID="txtCurrencyValue" runat="server" ToolTip="decimal" />
<asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtCurrencyValue" runat="server" ControlToValidate="txtCurrencyValue"
    ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Display="Dynamic"></asp:RequiredFieldValidator> 


