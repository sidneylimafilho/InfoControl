<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Product" EnableEventValidation="false"
    MasterPageFile="~/infocontrol/Default.master" Title="Cadastro de produto" CodeBehind="Product.aspx.cs" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div class="tabs cTabs11">
        <ul class="tabNavigation">
            <li><a href="Product_General.aspx?pid=<%=Request["ProductId"] %>" target="tabContent">
                Dados Gerais</a></li>
            <li><a href="Product_Composite.aspx?pid=<%=Request["ProductId"] %>" target="tabContent">
                Composição</a></li>
            <li><a href="Product_Package.aspx?pid=<%=Request["ProductId"] %>" target="tabContent">
                Embalagens</a></li>
            <li><a href="Product_Certificate.aspx?pid=<%=Request["ProductId"] %>" target="tabContent">
                Certificações</a></li>
            <li><a href="Product_Image.aspx?pid=<%=Request["ProductId"] %>" target="tabContent">
                Imagem do Produto</a></li>    
        </ul>
        <iframe id="tabContent" name="tabContent" src="Product_General.aspx?pid=<%=Request["ProductId"] %>">
        </iframe>
    </div>
</asp:Content>