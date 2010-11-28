<%@ Page Language="C#" MasterPageFile="~/site/1/Site.master" AutoEventWireup="true"
    Inherits="Vivina.Erp.WebUI.Site.Login" CodeBehind="login.aspx.cs" %>

<%@ Register Src="~/App_Shared/modules/AccessControl/Login.ascx" TagName="Login" TagPrefix="uc1" %> 
<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Controle de Acesso
    </h1>   
    <div class="logon">
        <fieldset>
            <uc1:Login ID="Login1" runat="server" />
        </fieldset>
    </div>
</asp:Content>
