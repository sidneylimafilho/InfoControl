<%@ Page Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    Inherits="Site_ContactUs" CodeBehind="ContactUs.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <style type="text/css">
        label.error
        {
            margin-left: 10px;
            width: auto;
            display: inline;
            color: Red;
        }
        input.error, textarea.error
        {
            border: 1px dotted red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="faleConosco">
        <label for="Nome">
            Nome</label>
        <input id="Nome" maxlength="50" name="Nome" class="{required:true, messages:{required:'*'}}" /><br />
        <label for="Email">
            E-mail:</label>
        <input id="Email" maxlength="50" name="Email" class="{required:true, email:true, messages:{required:'*', email:'E-mail Inválido!'}}" /><br />
        <label for="Mensagem">
            Mensagem:</label>
        <textarea id="Mensagem" cols="40" name="Mensagem" rows="5" class="{required:true, messages:{required:'*'}}"
            wrap="virtual"></textarea><br />
        <input id="Enviar" name="Enviar" type="submit" value=" Enviar " />
    </div>
    <!-- 
    Este campo input configura que é para processar o formulário
    -->
    <input type="hidden" name="action" value="FORM" />
</asp:Content>
