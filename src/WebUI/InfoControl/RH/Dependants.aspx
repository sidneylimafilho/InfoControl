<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Dependants.aspx.cs" Inherits="Vivina.Erp.WebUI.RH.dependants" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc4" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
            </td>
            <td class="center">
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </td>
            <td class="right">
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
            </td>
            <td class="center">
                <table width="70%">
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox ID="txtName" runat="server" Columns="30" MaxLength="128"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="InsertDependent"
                                runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;" ControlToValidate="txtName">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Data de Nascimento:<br />
                            <uc4:Date ID="ucDtBirthDate" Required="true" ValidationGroup="InsertDependent" runat="server" />
                        </td>
                        <td>
                            Parentesco:<br />
                            <asp:TextBox ID="txtFamilyTree" runat="server" Columns="30" MaxLength="128"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFamilyTree"
                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertDependent">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td valign="bottom">
                            &#160;&#160;&#160;
                            <asp:ImageButton ID="btnAddDependent" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                runat="server" AlternateText="Inserir Dependente" ValidationGroup="InsertDependent"
                                OnClick="btnAddDependent_Click" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdDependents" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                PageSize="20" AllowPaging="True" DataSourceID="odsDependents" Width="100%" DataKeyNames="EmployeeDependentId,Name,BirthDate,FamilyTree,EmployeeId,CompanyId"
                                OnRowDataBound="grdDependents_RowDataBound" rowselectable="false">
                                <Columns>
                                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Nome" />
                                    <asp:BoundField DataField="BirthDate" SortExpression="BirthDate" HeaderText="Data de aniversário"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="FamilyTree" SortExpression="FamilyTree" HeaderText="Parentesco" />
                                    <asp:CommandField ShowDeleteButton="True" DeleteText="<span class='delete' title='excluir'> </span>">
                                        <ItemStyle Width="1%" HorizontalAlign="Left" />
                                    </asp:CommandField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="text-align: center">
                                        Não existem dependentes cadastrados
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="right">
                &#160;&#160;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &#160;&#160;
            </td>
            <td class="center">
                &#160;&#160;
            </td>
            <td class="right">
                &#160;&#160;
            </td>
        </tr>
    </table>
    <VFX:BusinessManagerDataSource runat="server" ID="odsDependents" EnablePaging="True"
        SortParameterName="sortExpression" ConflictDetection="CompareAllValues" DataObjectTypeName="Vivina.Erp.DataClasses.EmployeeDependent"
        DeleteMethod="DeleteEmployeeDependent" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetEmployeeDependents" SelectCountMethod="GetEmployeeDependentsCount"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="odsDependents_Selecting">
        <selectparameters>
            <asp:Parameter Name="employeeId" Type="Int32" />
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
            <asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="maximumRows" Type="Int32" />
			<asp:parameter Name="startRowIndex" Type="Int32" />            
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
