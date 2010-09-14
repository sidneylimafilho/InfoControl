<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_RH_Employees" Title="Funcionários" CodeBehind="Employees.aspx.cs" %>

<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/AlphabeticalPaging.ascx" TagName="AlphabeticalPaging"
    TagPrefix="uc2" %>
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
                                <table style="height: 26px; width: 60%">
                                    <tr>
                                        <td class="style1">
                                            <span>
                                                <asp:RadioButtonList ID="rbtStatus" runat="server" RepeatDirection="Horizontal" Height="44px"
                                                    Width="347px">
                                                </asp:RadioButtonList>
                                            </span>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            Exibir:<br />
                                            <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnSearch" Text="Pesquisar" runat="server" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <uc2:AlphabeticalPaging ID="ucAlphabeticalPagingEmployees" OnSelectedLetter="ucAlphabeticalPagingEmployees_SelectedLetter"
                        runat="server" />
                    <br />
                    <br />
                    <asp:GridView ID="grdEmployees" runat="server" DataKeyNames="EmployeeId" AutoGenerateColumns="False"
                        DataSourceID="odsHumanResources" rowselectable="false" OnRowDataBound="grdEmployees_RowDataBound"
                        OnSorting="grdEmployees_Sorting" Width="100%" AllowSorting="True" EnableViewState="False"
                        AllowPaging="True" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="Enrollment" HeaderText="Matrícula" SortExpression="Enrollment">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                                <ItemTemplate>
                                    <asp:Label ID="LblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CPF" SortExpression="CPF">
                                <ItemTemplate>
                                    <asp:Label ID="LblCpf" runat="server" Text='<%# Eval("CPF") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                                SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" id='<%# Eval("EmployeeId") %>' companyid='<%# Eval("CompanyId") %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos, clique no botão para cadastrar um funcionário.<br />
                                &nbsp;<asp:Button ID="btnInsertNewEmployee" CommandName="InsertNew" runat="server"
                                    Text="Cadastrar" OnClick="btnInsertNewEmployee_Click" />
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
    </div>
    <uc1:ToolTip ID="tipEmployees" runat="server" Message=" A empresa sabe exatamente, quanto custa sua folha de pagamento mensal? Se tem setores com colaboradores à mais, à menos? Seus colaboradores, batem suas metas? Para saber estas e outras características sobre as pessoas que contribuem para a saúde da empresa, mantenha sempre todos os cadastros atualizados e com o máximo de informações possíveis"
        Title="Dica:" Indication="left" Top="0px" Left="150px" Visible="true" />
    <VFX:BusinessManagerDataSource ID="odsHumanResources" runat="server" onselecting="odsHumanResources_Selecting"
        SelectMethod="GetEmployeeByEnum" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        SortParameterName="sortExpression" ondeleted="odsHumanResources_Deleted" EnablePaging="True"
        SelectCountMethod="GetEmployeeByEnumCount" DeleteMethod="DeleteEmployeeById"
        OldValuesParameterFormatString="original_{0}">
        <deleteparameters>
            <asp:Parameter Name="EmployeeId" Type="Int32" />
        </deleteparameters>
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
            <asp:Parameter Name="employeeStatus" Type="Object" />
            <asp:Parameter Name="initialLetter" Type="String" />
            <asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="maximumRows" Type="Int32" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Header">
    <style type="text/css">
        .style1
        {
            width: 176px;
        }
    </style>
</asp:Content>
