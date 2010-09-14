<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Host_Package" Title="Pacote" CodeBehind="Package.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a target="tabContent" href='Package_General.aspx?PackageId=<%= Request["PackageId"] %>'>
                Geral </a></li>
            <li><a target="tabContent" href='FunctionsByPackages.aspx?PackageId=<%= Request["PackageId"] %>'>
                Funções por Pacote </a></li>
        </ul>
        <iframe id="tabContent" name="tabContent" src="Package_General.aspx?PackageId=<%= Request["PackageId"] %>">
        </iframe>
    </div>
</asp:Content>
