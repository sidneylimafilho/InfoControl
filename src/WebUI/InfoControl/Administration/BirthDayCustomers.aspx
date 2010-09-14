<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master" 
EnableEventValidation="false" CodeBehind="BirthDayCustomers.aspx.cs" 
Inherits="Vivina.Erp.WebUI.Administration.BirthDayCustomers" Title="Aniversariantes do mês" %>


<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
    
<%@ Register src="../../App_Shared/Date.ascx" tagname="Date" tagprefix="uc1" %>
    
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
            <!-- conteudo -->
             <asp:GridView ID="grdBirthDayCustomers" RowSelectable="false" runat="server" Width="100%" 
                DataKeyNames="companyId,CustomerId" DataSourceID="odsBirthDayCustomers" 
                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"  
                onrowdatabound="grdBirthDayCustomers_RowDataBound" > 
                <Columns>
                    
                    <asp:BoundField HeaderText="Nome" DataField="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Telefone" />
                    <asp:BoundField DataField="BirthDate" HeaderText="Data de Nascimento" />
                    
                    
                </Columns>
                <EmptyDataTemplate> 
                    <center>  Não há aniveriantes nesse mês !!        </center>
                
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
            <vfx:BusinessManagerDataSource ID="odsBirthDayCustomers" runat="server" 
                SelectMethod="GetCustomersBirthDay" 
                TypeName="Vivina.Erp.BusinessRules.CustomerManager" 
                DataObjectTypeName="Vivina.Erp.BusinessRules.CustomerManager" 
                SelectCountMethod="GetCustomersBirthDayCount" 
                SortParameterName="sortExpression"
                
                 EnablePaging="True" 
                onselecting="odsBirthDayCustomers_Selecting">
                
                <SelectParameters>
                    <asp:Parameter Name="companyId" Type="Int32" />
                    <asp:Parameter Name="sortExpression" Type="String" />
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                </SelectParameters>
            </vfx:BusinessManagerDataSource>
        </td>
        <td class="center">
            &nbsp;
        </td>
        <td class="right">
            &nbsp;
        </td>
    </tr>
</table>
</asp:Content>