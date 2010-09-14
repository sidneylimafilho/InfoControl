<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Title="Planos" Inherits="Host_Plans" CodeBehind="Plans.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
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
                    <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend><div class="body">
                                <table width="70%">
                                    <tr>
                                        <td>
                                            <uc1:DateTimeInterval ID="ucDateTimeInterval" runat="server" ValidationGroup="filter" />
                                        </td>
                                        <td>
                                            Insira parte do nome:<br />
                                            <asp:TextBox runat="server" ID="txtFindPlan" ValidationGroup="filter"></asp:TextBox>
                                        </td>
                                        <td>
                                            Exibir:<br />
                                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnSearch" runat="server" Text="Filtrar" ValidationGroup="filter"
                                                OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdPlans" runat="server" AutoGenerateColumns="False" DataSourceID="odsPlans"
                                    AllowPaging="True" OnRowDataBound="grdPlans_RowDataBound" AllowSorting="True"
                                    DataKeyNames="PlanId" Width="100%" RowSelectable="false" permissionRequired="Plans"
                                    EnableViewState="True" PageSize="20">
                                    <Columns>
                                        <asp:BoundField HeaderText="Nome" DataField="PlanName" SortExpression="PlanName" />
                                        <asp:BoundField HeaderText="Pacote" DataField="PackageName" SortExpression="PackageName" />
                                        <asp:BoundField HeaderText="Data Início" DataField="AvailableStartDate" SortExpression="AvailableStartDate" />
                                        <asp:BoundField HeaderText="Data Término" DataField="AvailableEndDate" SortExpression="AvailableEndDate" />
                                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                            HeaderText="&lt;a href=&quot; Plan.aspx&quot; &lt;div class=&quot;insert&quot;title=&quot;inserir&quot; &lt;/div&gt; &lt;/a&gt"
                                            SortExpression="Insert">
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div align="center">
                                            Não existe dados a serem exibidos
                                            <br />
                                            <asp:Button runat="server" ID="btnInsert" OnClientClick="location='Plan.aspx'; return false;"
                                                Text="Inserir Novo" />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
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
        <VFX:BusinessManagerDataSource ID="odsPlans" runat="server" SelectMethod="GetPlans"
            SelectCountMethod="GetPlansCount" TypeName="Vivina.Erp.BusinessRules.PlanManager"
            EnablePaging="True" SortParameterName="sortExpression" onselecting="odsPlans_Selecting"
            DeleteMethod="Delete">
            <deleteparameters>
                <asp:Parameter Name="PlanId" />
            </deleteparameters>
            <selectparameters>
                <asp:parameter Name="dateTimeInterval" Type="Object" />
                <asp:parameter Name="name" Type="String" />
                <asp:parameter Name="sortExpression" Type="String" />
                <asp:parameter Name="startRowIndex" Type="Int32" />
                <asp:parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
        </VFX:BusinessManagerDataSource>
    </div>
</asp:Content>
