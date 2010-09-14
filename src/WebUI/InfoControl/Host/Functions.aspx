<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Host_FunctionList" Title="Funções" CodeBehind="Functions.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="width: 100%">
       
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
                  
                    <asp:GridView ID="grdFunctions" runat="server" AutoGenerateColumns="False" DataSourceID="odsFunctions"
                        AllowSorting="True" OnRowDataBound="grdFunctions_RowDataBound" OnSorting="grdFunctions_Sorting"
                        DataKeyNames="FunctionId" OnSelectedIndexChanging="grdFunctions_SelectedIndexChanging"
                        PageSize="20" Width="100%" AllowPaging="True" EnableViewState="False">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                            <asp:BoundField DataField="CodeName" HeaderText="Código" SortExpression="CodeName" />
                            <asp:TemplateField HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                                SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" id='<%# Eval("FunctionId") %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No momento nao ha dados para exibicao,
                            <br />
                            se quiser cadastrar um novo registro, clique novamente no menu funcoes.
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
        <VFX:BusinessManagerDataSource ID="odsFunctions" runat="server" onselecting="odsFunctions_Selecting"
            SelectCountMethod="GetFunctionsCount" SelectMethod="GetFunctions" 
            TypeName="Vivina.Erp.BusinessRules.FunctionManager" EnablePaging="True"
            SortParameterName="sortExpression">
            <selectparameters>
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
        </VFX:BusinessManagerDataSource>
        
    </div>
</asp:Content>
