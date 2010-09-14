<%@ Page Title="Extratos" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" CodeBehind="Statements.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Accounting.Statements" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
            
            <asp:GridView show Width="100%" ID="grdStatements" RowSelectable="false" 
                runat="server" DataSourceID="odsStatement" DataKeyNames="StatementId,BoletusNumber,CustomerName" 
                AutoGenerateColumns="false" onrowdatabound="grdStatements_RowDataBound" 
                AllowPaging="True" AllowSorting="True">
            <Columns> 
            
             <asp:BoundField HeaderText="Número do Extrato" DataField="BoletusNumber"  SortExpression="BoletusNumber"/>
             <asp:BoundField HeaderText="Cliente"  DataField="CustomerName" SortExpression="CustomerName" />
             <asp:BoundField HeaderText="Total"  DataField="StatementTotal" SortExpression="StatementTotal" />
              <asp:BoundField HeaderText="Período de Início" DataField="PeriodBegin" SortExpression="PeriodBegin" />
              <asp:BoundField HeaderText="Período de Fim"  DataField="PeriodEnd" SortExpression="PeriodEnd" />
              
             
            </Columns>
            
            
            
            
            <EmptyDataTemplate> 
             <div align="center" >
              Não há dados a serem exibidos.   
              
             </div> 
            
            </EmptyDataTemplate>
            </asp:GridView>
            
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
    <VFX:BusinessManagerDataSource ID="odsStatement" runat="server" onselecting="odsStatement_Selecting"
        SelectMethod="GetStatements" TypeName="Vivina.Erp.BusinessRules.CompanyManager"
        SelectCountMethod="GetStatementsCount" EnablePaging="True" SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
