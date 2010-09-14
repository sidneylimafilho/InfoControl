<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Summary.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Services.Summary" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script src="../../App_Shared/JS/jquery.template.js" type="text/javascript"></script>
    <h1>
        Sumário
    </h1>
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                &#160;
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                <%--Conteúdo--%>
                <h2>
                    Chamados Abertos
                </h2>
                <asp:UpdatePanel runat="server" ID="pnlCustomerCalls">
                    <ContentTemplate>
                        <asp:GridView Width="100%" runat="server" AllowPaging="true" PageSize="20" AllowSorting="true"
                            ID="grdCustomerCalls" AutoGenerateColumns="false" DataSourceID="odsCustomerCalls"
                            rowSelectable="false" OnRowDataBound="grdCustomerCalls_RowDataBound" DataKeyNames="ServiceOrderId, CustomerCallId">
                            <Columns>
                                <asp:BoundField HeaderText="Número" DataField="CallNumber" SortExpression="CallNumber" />
                                <asp:BoundField HeaderText="Cliente" DataField="CustomerName" SortExpression="CustomerName" />
                                <asp:BoundField HeaderText="Técnico" DataField="TechnicalEmployee" SortExpression="TechnicalEmployee" />
                                <asp:BoundField HeaderText="Status" DataField="CustomerCallStatusName" SortExpression="CustomerCallStatusName" />
                                <asp:BoundField HeaderText="Data de Abertura" DataField="OpenedDate" DataFormatString="{0:dd/MM/yyyy}"
                                    SortExpression="OpenedDate" />
                                <asp:TemplateField HeaderText="OS Relacionada" SortExpression="ServiceOrderNumber">
                                    <ItemTemplate>
                                        <a style="cursor: pointer" onclick="event.cancelBubble=true; top.tb_show('Ordem de Serviço', '<%# "Services/ServiceOrder.aspx?ServiceOrderId=" + Eval("ServiceOrderId").EncryptToHex()  %>');">
                                            <%# Eval("ServiceOrderNumber") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <p align="center">
                                    Não há dados a serem exibidos.
                                </p>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel >
                <br />
                <br />
                <br />
                <h2>
                    Ordens de Serviço Abertas
                </h2>
                <asp:UpdatePanel runat="server" ID="pnlServiceOrders">
                    <ContentTemplate>
                        <asp:GridView Width="100%" runat="server" AllowPaging="true" PageSize="20" AllowSorting="true"
                            ID="grdServiceOrders" AutoGenerateColumns="false" DataSourceID="odsServiceOrders"
                            rowSelectable="false" OnRowDataBound="grdServiceOrders_RowDataBound" DataKeyNames="ServiceOrderId">
                            <Columns>
                                <asp:BoundField HeaderText="Número" DataField="ServiceOrderNumber" SortExpression="ServiceOrderNumber" />
                                <asp:BoundField HeaderText="Cliente" DataField="CustomerName" SortExpression="CustomerName" />
                                <asp:BoundField HeaderText="Status" DataField="ServiceOrderStatusName" SortExpression="ServiceOrderStatusName" />
                                <asp:BoundField HeaderText="Data de Abertura" DataField="OpenedDate" DataFormatString="{0:dd/MM/yyyy}"
                                    SortExpression="OpenedDate" />
                            </Columns>
                            <EmptyDataTemplate>
                                <p align="center">
                                    Não há dados a serem exibidos.
                                </p>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--    <h2>
                    Chamados Abertos
                </h2>
                <table auto="true" action='<%=ResolveUrl("~/Controller/SearchService") %>' method="GetOpenCustomerCalls"
                    template="#tbodyCustomerCalls" target="#tbodyCustomerCalls" params="{}"
                    class="cGrd11" cellspacing="0" rules="all" rowselectable="false" style="width: 100%;
                    border-collapse: collapse;">
                    <thead>
                        <tr>
                            <th>
                                Número
                            </th>
                            <th>
                                Cliente
                            </th>
                           
                            <th>
                                Técnico
                            </th> 
                            <th>
                                Status
                            </th>   
                            <th> 
                             OS Relacionada
                            </th>

                        </tr>
                    </thead>
                    <tbody id="tbodyCustomerCalls">
                        <!-- <tr>
                             <td>
                                <a href="<$='../CRM/CustomerCall.aspx?CustomerCallId=' + CustomerCallId $>"> 
                                 <$=CallNumber$>
                                </a>  
                            </td>

                            <td>
                                <a href="<$='../CRM/CustomerCall.aspx?CustomerCallId=' + CustomerCallId $>"> 
                                 <$=CustomerName$>
                                </a>  
                            </td>
                             <td>
                                <a href="<$='../CRM/CustomerCall.aspx?CustomerCallId=' + CustomerCallId $>"> 
                                 <$=TechnicalEmployee$>
                                </a>  
                            </td>
                            <td>
                                <a href="<$='../CRM/CustomerCall.aspx?CustomerCallId=' + CustomerCallId $>"> 
                                 <$=CustomerCallStatus$>
                                </a>  
                            </td>
                              <td>
                                <a href="<$='ServiceOrder.aspx?ServiceOrderId=' + ServiceOrderId $>"> 
                                 <$=ServiceOrderNumber$>
                                </a>  
                            </td>
                        </tr>-->
                    </tbody>
                </table>
                <br />
                <br />
                <h2>
                    Ordens de Serviço abertas
                </h2>
                <table auto="true" action='<%=ResolveUrl("~/Controller/SearchService") %>' method="GetServiceOrders"
                    template="#tbody" target="#tbody" params="{companyId: '1'}" class="cGrd11" cellspacing="0"
                    rules="all" rowselectable="false" style="width: 100%; border-collapse: collapse;">
                    <thead>
                        <tr>
                            <th>
                                Número
                            </th>
                            <th>
                                Status
                            </th>
                             <th>
                                Cliente
                            </th>   
                        </tr>
                    </thead>
                    <tbody id="tbody">
                        <!-- <tr>
                             <td>
                                <a href="<$='ServiceOrder.aspx?ServiceOrderId=' + ServiceOrderId $>"> 
                                 <$=ServiceOrderNumber$>
                                </a>  
                            </td>                            
                     
                             <td>
                                <a href="<$='ServiceOrder.aspx?ServiceOrderId=' + ServiceOrderId $>"> 
                                 <$=ServiceOrderStatus$>
                                </a>  
                            </td>

                               <td>
                                <a href="<$='ServiceOrder.aspx?ServiceOrderId=' + ServiceOrderId $>"> 
                                 <$=CustomerName$>
                                </a>  
                            </td>
                        </tr>
                        -->
                    </tbody>
                </table>--%>
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &#160;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource runat="server" ID="odsCustomerCalls" SelectMethod="GetOpenCustomerCalls"
        EnablePaging="True" TypeName="Vivina.Erp.BusinessRules.CustomerManager" SelectCountMethod="GetOpenCustomerCallsCount"
        SortParameterName="sortExpression" onselecting="dataSource_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsServiceOrders" SelectMethod="GetOpenServiceOrders"
        EnablePaging="True" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager"
        SelectCountMethod="GetOpenServiceOrdersCount" SortParameterName="sortExpression"
        onselecting="dataSource_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
