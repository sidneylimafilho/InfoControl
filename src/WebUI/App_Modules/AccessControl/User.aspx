<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" Title="Usuário"
    Inherits="UserGeneral" MasterPageFile="~/infocontrol/Default.master" CodeBehind="User.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href="User_General.aspx?UserId=<%=Request["UserId"] %>" target="tabContent">Geral</a></li>
            <li><a href="User_Roles.aspx?UserId=<%=Request["UserId"] %>" target="tabContent">Perfil</a></li>
        </ul>
        <iframe id="tabContent" name="tabContent" src="User_General.aspx?UserId=<%=Request["UserId"] %>">
        </iframe>
    </div>
</asp:Content>
