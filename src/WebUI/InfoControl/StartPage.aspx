<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="Default.master"
    AutoEventWireup="true" Inherits="StartPage" Title="Página Inicial" CodeBehind="StartPage.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Src="RH/SelectEmployee.ascx" TagName="SelectEmployee" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Header">
    <style>
        legend
        {
            font-size: 16px;
            font-weight: lighter;
        }
        legend span
        {
            font-size: 13px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;
            </td>
            <td class="center" source="SearchService.svc">
                <div asform="true" trigger="#pnlCustomers, #pnlHelpPages, #pnlEmployees, #pnlSuppliers, #pnlContacts, #pnlProducts" params="{'limit':15}">
                    <center>
                        <h2 style='font-weight: lighter'>
                            Busque rapidamente o que está no sistema:</h2>
                        <br />
                        (Dica: Pesquise por clientes, fornecedores, contas a pagar/receber, páginas, contatos,
                        produtos, e muito mais)<br />
                        <br />
                        <input type="text" name="q" />
                        <asp:Button runat="server" Text="ENCONTRAR AGORA!" UseSubmitBehavior="false" OnClientClick="return false;"
                            command="click" /></center>
                    <br />
                    <br />
                </div>
                <%--Conteúdo--%>
                <fieldset id="pnlCustomers" action="FindCustomers"
                    template="#pnlCustomers .template" target="#pnlCustomers .target" 
                    onsucess="$('#pnlCustomers').toggle(result.Data.length>0).find('span').text(result.Data.length);"
                    style="display: none">
                    <legend>Clientes (<span>0</span>)</legend>
                    <div class="template">
                        <!--<div class="namedListItem">
                            <a href="Administration/Customer.aspx?CustomerId=<$=Id$>"><$=Name$></a>
                        </div>-->
                    </div>
                    <div class="target"></div>
                </fieldset>
                
                 <fieldset id="pnlHelpPages" action="FindHelpPages"
                    template="#pnlHelpPages .template" target="#pnlHelpPages .target" 
                    onsucess="$('#pnlHelpPages').toggle(result.Data.length>0).find('span').text(result.Data.length);"
                    style="display: none">
                    <legend>Páginas de Ajuda (<span>0</span>)</legend>
                    <div class="template">
                        <!--<div class="namedListItem">
                            <a href="<$=Id$>"><$=Name$></a>
                        </div>-->
                    </div>
                    <div class="target"></div>
                </fieldset>
                
                 <fieldset id="pnlEmployees" action="FindEmployees"
                    template="#pnlEmployees .template" target="#pnlEmployees .target" 
                    onsucess="$('#pnlEmployees').toggle(result.Data.length>0).find('span').text(result.Data.length);"
                    style="display: none">
                    <legend>Funcionários (<span>0</span>)</legend>
                    <div class="template">
                        <!--<div class="namedListItem">
                            <a href="RH/Employee.aspx?EmployeeId=<$=Id$>"><$=Name$></a>
                        </div>-->
                    </div>
                    <div class="target"></div>
                </fieldset>
                
                 <fieldset id="pnlSuppliers" action="FindSuppliers"
                    template="#pnlSuppliers .template" target="#pnlSuppliers .target" 
                    onsucess="$('#pnlSuppliers').toggle(result.Data.length>0).find('span').text(result.Data.length);"
                    style="display: none">
                    <legend>Fornecedores (<span>0</span>)</legend>
                    <div class="template">
                        <!--<div class="namedListItem">
                            <a href="Administration/Supplier.aspx?SupplierId=<$=Id$>"><$=Name$></a>
                        </div>-->
                    </div>
                    <div class="target"></div>
                </fieldset>
                
                <fieldset id="pnlContacts" action="FindContacts"
                    template="#pnlContacts .template" target="#pnlContacts .target" 
                    onsucess="$('#pnlContacts').toggle(result.Data.length>0).find('span').text(result.Data.length);"
                    style="display: none">
                    <legend>Contatos (<span>0</span>)</legend>
                    <div class="template">
                        <!--<div class="namedListItem">
                            <a href="Administration/Contact.aspx?ContactId=<$=Id$>"><$=Name$></a>
                        </div>-->
                    </div>
                    <div class="target"></div>
                </fieldset>
                
                <fieldset id="pnlProducts" action="FindProducts"
                    template="#pnlProducts .template" target="#pnlProducts .target" 
                    onsucess="$('#pnlProducts').toggle(result.Data.length>0).find('span').text(result.Data.length);"
                    style="display: none">
                    <legend>Produtos (<span>0</span>)</legend>
                    <div class="template">
                        <!--<div class="namedListItem">
                            <a href="Administration/Product.aspx?ProductId=<$=Id$>"><$=Name$></a>
                        </div>-->
                    </div>
                    <div class="target"></div>
                </fieldset>
                
                <%-- 
                
                
               
                
                <fieldset id="pnlBills" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchBills"
                    dataview:itemtemplate="#lvBill" dataview:itemplaceholder="#lvBill" dataview:fetchparameters="{{ {text: ''} }}"
                    class="sys-template">
                    <legend>Contas à pagar <span>(0)</span></legend>
                    <div id="lvBill">
                        <div class="namedListItem">
                            <a sys:href="{{ 'Accounting/Bill.aspx?BillId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="pnlInvoices" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchInvoices"
                    dataview:itemtemplate="#lvInvoice" dataview:itemplaceholder="#lvInvoice" dataview:fetchparameters="{{ {text: ''} }}"
                    class="sys-template">
                    <legend>Contas à receber <span>(0)</span></legend>
                    <div id="lvInvoice">
                        <div class="namedListItem">
                            <a sys:href="{{ 'Accounting/Invoice.aspx?InvoiceId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>--%>
               
                
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
