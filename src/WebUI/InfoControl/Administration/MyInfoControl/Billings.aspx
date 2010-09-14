<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="MyInfoControl_Company_Administration_Billings" Title="Pagamentos" Codebehind="Billings.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                &nbsp;</td>
            <td class="right">
                &nbsp;</td>
        </tr>
        <tr class="middle">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                <asp:GridView ID="grdBillings" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="odsBillings" Width="100%">
                    <Columns>
                        <asp:boundfield DataField="EmissionDate" DataFormatString="{0:dd/MM/yyyy}" 
                            HeaderText="Emissão" SortExpression="EmissionDate"></asp:boundfield>
                        <asp:boundfield DataField="Name" HeaderText="Nome" SortExpression="Name">
                        </asp:boundfield>
                        <asp:boundfield DataField="BoletusNumber" HeaderText="Número" 
                            SortExpression="BoletusNumber"></asp:boundfield>
                        <asp:boundfield DataField="PaymentDate" DataFormatString="{0:dd/MM/yyyy}" 
                            HeaderText="Pagamento" SortExpression="PaymentDate"></asp:boundfield>
                        <asp:boundfield DataField="PaymentValue" DataFormatString="{0:F2}" 
                            HeaderText="Valor" SortExpression="PaymentValue"></asp:boundfield>
                    </Columns>
                    <emptydatatemplate>
                        Não há registros de pagamentos anteriores ...
                    </emptydatatemplate>
                </asp:GridView>
            </td>
            <td class="right">
                &nbsp;</td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &nbsp;</td>
            <td class="center">
                &nbsp;</td>
            <td class="right">
                &nbsp;</td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource ID="odsBillings" runat="server" 
        SelectMethod="GetBillingByCompany" 
        TypeName="Vivina.Erp.BusinessRules.BillingManager" 
        onselecting="odsBillings_Selecting">
        <SelectParameters>
            <asp:parameter Name="CompanyId" Type="Int32"></asp:parameter>
        </SelectParameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
