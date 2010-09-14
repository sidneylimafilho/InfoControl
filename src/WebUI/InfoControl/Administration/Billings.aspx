<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    EnableEventValidation="false" Inherits="InfoControl_Company_Administration_Billings"
    Title="Pagamentos" Codebehind="Billings.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <asp:GridView ID="grdBillings" runat="server" AutoGenerateColumns="False" DataSourceID="odsBillings"
                    Width="100%">
                    <Columns>
                        <asp:BoundField DataField="EmissionDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Emissão"
                            SortExpression="EmissionDate"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                        <asp:BoundField DataField="BoletusNumber" HeaderText="Número" SortExpression="BoletusNumber">
                        </asp:BoundField>
                        <asp:BoundField DataField="PaymentDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Pagamento"
                            SortExpression="PaymentDate"></asp:BoundField>
                        <asp:BoundField DataField="PaymentValue" DataFormatString="{0:F2}" HeaderText="Valor"
                            SortExpression="PaymentValue"></asp:BoundField>
                    </Columns>
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
    <VFX:BusinessManagerDataSource ID="odsBillings" runat="server" SelectMethod="GetBillingByCompany"
        TypeName="Vivina.Erp.BusinessRules.BillingManager" onselecting="odsBillings_Selecting">
        <selectparameters>
            <asp:parameter Name="CompanyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
