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
            <td class="center">
                <div asform="true" trigger="#pnlCustomers" params="{'limit':15}">
                    <center>
                        <h2 style='font-weight: lighter'>
                            Busque rapidamente o que está no sistema:</h2>
                        <br />
                        (Dica: Pesquise por clientes, fornecedores, contas a pagar/receber, páginas, contatos,
                        produtos, e muito mais)<br />
                        <br />
                        <input type="text" name="txtSearch" />
                        <asp:Button runat="server" Text="ENCONTRAR AGORA!" UseSubmitBehavior="false" OnClientClick="return false;"
                            command="click" /></center>
                    <br />
                    <br />
                </div>
                <%--Conteúdo--%>
                <fieldset id="pnlCustomers" controller="../Controller/SearchService/" action="FindCustomers"
                    template="#lvCustomer" target="#lvCustomer" onsucess="$('#pnlCustomers').show(); $('#pnlCustomers legend > span').text(result.Data.length);"
                    style="display: none">
                    <legend>Clientes (<span>0</span>)</legend>
                    <div id="lvCustomer">
                        <div class="namedListItem">
                            <a href="Administration/Customer.aspx?CustomerId=<$=Id$>"><$=Name$></a>
                        </div>
                    </div>
                </fieldset>
                <%-- 
                <fieldset id="pnlSuppliers" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchSuppliers"
                    dataview:itemtemplate="#lvSupplier" dataview:itemplaceholder="#lvSupplier" dataview:fetchparameters="{{ {text: ''} }}"
                    class="sys-template">
                    <legend>Fornecedores <span>(0)</span></legend>
                    <div id="lvSupplier">
                        <div class="namedListItem">
                            <a sys:href="{{ 'Administration/Supplier.aspx?SupplierId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="pnlContacts" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchContacts"
                    dataview:itemtemplate="#lvContact" dataview:itemplaceholder="#lvContact" dataview:fetchparameters="{{ {text: ''} }}"
                    class="sys-template">
                    <legend>Contatos <span>(0)</span></legend>
                    <div id="lvContact">
                        <div class="namedListItem">
                            <a sys:href="{{ 'Administration/Contact.aspx?ContactId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="pnlProducts" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchProducts"
                    dataview:itemtemplate="#lvProduct" dataview:itemplaceholder="#lvProduct" dataview:fetchparameters="{{ {text: ''} }}"
                    class="sys-template">
                    <legend>Produtos <span>(0)</span></legend>
                    <div id="lvProduct">
                        <div class="namedListItem">
                            <a sys:href="{{ 'Administration/Product.aspx?ProductId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="pnlEmployees" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchEmployees"
                    dataview:itemtemplate="#lvEmployee" dataview:itemplaceholder="#lvEmployee" dataview:fetchparameters="{{ {text: '', limit : 12} }}"
                    class="sys-template">
                    <legend>Empregados <span>(0)</span></legend>
                    <div id="lvEmployee">
                        <div class="namedListItem">
                            <a sys:href="{{ 'RH/Employee.aspx?EmployeeId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>
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
                </fieldset>
                <fieldset id="pnlHelpPages" sys:attach="dataview" dataview:onrendered="{{ onrendered }}"
                    dataview:autofetch="false" dataview:dataprovider="SearchService.svc" dataview:fetchoperation="SearchHelpPages"
                    dataview:itemtemplate="#lvHelpPage" dataview:itemplaceholder="#lvHelpPage" dataview:fetchparameters="{{ {text: ''} }}"
                    class="sys-template">
                    <legend>Páginas de Ajuda <span>(0)</span></legend>
                    <div id="lvHelpPage">
                        <div class="namedListItem">
                            <a sys:href="{{ 'Site/WebPage.aspx?WebPageId=' + Id }}">{{ Name }}</a>
                        </div>
                    </div>
                </fieldset>
                --%>
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
