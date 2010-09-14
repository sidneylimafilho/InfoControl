<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Host_PaymentMethods"
    Title="Untitled Page" Codebehind="PaymentMethods.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <img src="../App_Themes/Glass/Company/paymentMethod.gif" alt="" />
    <table class="cLeafBox21" width="50%">
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
                <asp:GridView ID="grdPaymentMethod" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" OnRowDataBound="grdPaymentMethod_RowDataBound" OnSelectedIndexChanged="grdPaymentMethod_SelectedIndexChanged"
                    OnSorting="grdPaymentMethod_Sorting" Width="100%" 
				DataSourceID="odsPaymentMethods" 
				DataKeyNames="PaymentMethodId,Name,TransactionUrl,ModifiedDate" PageSize="20">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                        <asp:BoundField DataField="TransactionUrl" HeaderText="Url da Operadora" SortExpression="TransactionUrl">
                        </asp:BoundField>
                        <asp:CommandField DeleteText="&lt;img src=&quot;../App_Themes/Glass/Controls/GridView/img/Pixel_bg.gif&quot; alt=&quot;Apagar&quot; class=&quot;delete&quot; border=0&gt;"
                            HeaderText="&lt;img src='../App_Themes/Glass/Controls/GridView/img/Add.gif' border='0' /&gt;"
                            ShowDeleteButton="True" SortExpression="Insert">
                            <ItemStyle Width="1%" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                      <div style="text-align: center">
                          Não existem dados a serem exibidos, clique no botão para cadastrar um método de 
                          pagamento.<br />
                          &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
                      </div>
                    </EmptyDataTemplate>
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
    <VFX:BusinessManagerDataSource ID="odsPaymentMethods" runat="server" ConflictDetection="CompareAllValues"
        EnablePaging="True" SelectMethod="GetPaymentMethods" 
        TypeName="Vivina.Erp.BusinessRules.PaymentMethodManager" 
        DataObjectTypeName="Vivina.Erp.DataClasses.PaymentMethod" 
        DeleteMethod="Delete">
        <selectparameters>
			<asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
			<asp:parameter Name="maximumRows" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
