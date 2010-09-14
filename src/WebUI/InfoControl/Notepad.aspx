<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Notepad" Title="" CodeBehind="Notepad.aspx.cs" %>

<%@ Register Src="~/App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/App_Shared/SelectUser.ascx" TagName="SelectUser" TagPrefix="uc1" %>
<%@ Register Src="../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc3" %>
<%@ Register Src="../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc4" %>
<%@ Register Src="../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc5" %>
<%@ Register Src="../App_Shared/LeafBox.ascx" TagName="LeafBox" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <telerik:RadEditor ID="txtDescription" runat="server" SkinID="Telerik" Width="100%"
        EnableResize="true" Height="400px">
        <Content></Content>
    </telerik:RadEditor>
    <asp:Button ID="btnSave" runat="server" Text="Salvar Texto" UseSubmitBehavior="false"
        OnClick="btnSave_Click" />
</asp:Content>
