<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_POS_SaleList" Title="Histórico de Vendas"
    CodeBehind="SaleList.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <fieldset id="filter" class="closed">
                    <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                        Escolha o filtro desejado: </legend><div class="body">
                            <table width="500px">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="uPnlSelectCustomer" runat="server">
                                            <contenttemplate>
                                                <uc2:SelectCustomer ID="sel_customer" runat="server" EnabledToolTip="false" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                            </contenttemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Nº da Nota fiscal:<br />
                                        <asp:TextBox ID="txtFiscalNumber" Mask="9999999999" runat="server" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkShowCanceledSale" runat="server" Text="Canceladas" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc1:DateTimeInterval ID="ucDateTimeInterval" ValidationGroup="Search" Required="true"
                                            runat="server" />
                                    </td>
                                    <td>
                                        Status:
                                        <br />
                                        <asp:DropDownList DataSourceID="odsSaleStatus" runat="server" ID="cboSaleStatus"
                                            DataValueField="SaleStatusId" DataTextField="Name" AppendDataBoundItems="true">
                                            <asp:ListItem Text="" Value=""> </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Exibir:<br />
                                        <asp:DropDownList ID="cboPageSize" AutoPostBack="False" runat="server">
                                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                            <asp:ListItem Value="All" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div align="right">
                                <asp:Button ID="btnSearch" ValidationGroup="Search" runat="server" Text="Filtrar"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                        </span>
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:GridView ID="grdSalesList" RowSelectable="false" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsSaleList" AllowSorting="True" OnRowDataBound="grdSalesList_RowDataBound"
                    DataKeyNames="SaleId,IsCanceled" PageSize="20" Width="100%" AllowPaging="True">
                    <Columns>
                        <asp:BoundField DataField="customerName" SortExpression="customerName" HeaderText="Cliente" />
                        <asp:BoundField DataField="employeeName" SortExpression="employeeName" HeaderText="Vendedor" />
                        <asp:BoundField DataField="SaleDate" HeaderText="Data" SortExpression="SaleDate"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                        <asp:BoundField DataField="InvoiceValue" HeaderText="Total" SortExpression="InvoiceValue">
                        </asp:BoundField>
                        <asp:BoundField DataField="ReceiptNumber" SortExpression="ReceiptNumber" HeaderText="Nº da Nota fiscal"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Convert.ToBoolean(Eval("IsCanceled")) ?  "Cancelada"  :  "Em aberto" %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem vendas para o período e/ou cliente selecionado
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
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
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource runat="server" ID="odsSaleList" OnSelecting="odsSaleList_Selecting"
        SelectMethod="GetSaleHistory" TypeName="Vivina.Erp.BusinessRules.SaleManager"
        SelectCountMethod="GetSaleHistoryCount" OldValuesParameterFormatString="original_{0}"
        EnablePaging="True" SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="customerId" Type="Int32" />
            <asp:Parameter Name="representantId" Type="Int32" />         
            <asp:Parameter Name="receiptNumber" Type="Int32" />
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            <asp:Parameter Name="showCanceled" Type="Boolean" />
             <asp:Parameter Name="saleStatusId" Type="Int32" />
           
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
           
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" id="odsSaleStatus" SelectMethod="GetAllSaleStatus"
        TypeName="Vivina.Erp.BusinessRules.SaleManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
