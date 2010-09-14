<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/infocontrol/Default.master"
    AutoEventWireup="true" Inherits="AccessControl_Role" Title="Perfil" CodeBehind="Role.aspx.cs" %>

<%@ Register Src="Role_General.ascx" TagName="Role_General" TagPrefix="uc1" %>
<%@ Register Src="Role_Permissions.ascx" TagName="Role_Permissions" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href="#geral">Perfil</a></li>
            <li  runat="server" Visible="false" id="tabPermissions"><a href="#perfil">Permissões</a></li>
        </ul>
        <div id="geral">
            <uc1:Role_General ID="Role_General" runat="server" />
        </div>
 
        <div id="perfil">
            <uc2:Role_Permissions ID="Role_Permissions" runat="server" />
        </div>
    
    </div>
</asp:Content>
