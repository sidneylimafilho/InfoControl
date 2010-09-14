<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Administration_Customer_Payment"
    CodeBehind="Customer_Payment.aspx.cs" Title="" MasterPageFile="~/infocontrol/Default.master" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
            <table width="50%">
                <tr>
                    <td>
                        <asp:RadioButton ID="rbtOpen" runat="server" Text="Em Aberto" GroupName="Payment"
                            Checked="true" AutoPostBack="True" 
                            oncheckedchanged="rbtOpen_CheckedChanged" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtClosed" runat="server" Text="Efetuados" GroupName="Payment"
                            AutoPostBack="True" oncheckedchanged="rbtClosed_CheckedChanged" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <asp:GridView ID="grdPayment" runat="server" DataSourceID="odsCustomerPayment" Width="100%"
                AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="DueDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data">
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Parcela"></asp:BoundField>
                   <%-- <asp:BoundField DataField="DocumentNumber" HeaderText="Nº do Documento"></asp:BoundField>--%>
                    <asp:BoundField DataField="ReceiptNumber" HeaderText="Nº Nota Fiscal"></asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Valor" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                </Columns>
                <EmptyDataTemplate> 
                 <div align="center"> 
                   Não há dados a serem exibidos
                 </div>
                </EmptyDataTemplate>
            </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsCustomerPayment" runat="server" OnSelecting="odsCustomerPayment_Selecting"
        SelectMethod="GetInvoicesByCustomer" TypeName="Vivina.Erp.BusinessRules.FinancialManager">
        <selectparameters>
        <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="closed" Type="Boolean" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
