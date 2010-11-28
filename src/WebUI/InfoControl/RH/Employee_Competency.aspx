<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Employee_Competency.aspx.cs" Inherits="Vivina.Erp.WebUI.RH.Employee_Competency" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <%--Conteudo--%>
                <table>
                    <tr>
                        <td>
                            Habilidade:<br />
                            <asp:TextBox runat="server" ID="txtSkillName" MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ID="reqSkillName"  ErrorMessage="&nbsp;&nbsp;" Display="Dynamic"
                                ControlToValidate="txtSkillName"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Classificação:<br />
                            <ajaxToolkit:Rating runat="server" ID="rtnRanking" MaxRating="5" StarCssClass="ratingStar"
                                WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                ToolTip="Classificação">
                            </ajaxToolkit:Rating>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/App_Shared/themes/glasscyan/Controls/GridView/img/Add2.gif"
                                AlternateText="Salvar" OnClick="btnSave_Click" CausesValidation="true" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <asp:GridView runat="server" ID="grdCompetency" AutoGenerateColumns="False" DataSourceID="odsCompetency"
                    rowselectable="false" DataKeyNames="EmployeeCompetencyId">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Habilidade" SortExpression="Name" />
                        <asp:BoundField DataField="Rating" HeaderText="Classificação" SortExpression="Rating" /> 
                        <asp:CommandField DeleteText="<span class='delete' title='excluir'> </span>"
                            ShowDeleteButton="true" ItemStyle-Width="1%" />
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
    <VFX:BusinessManagerDataSource ID="odsCompetency" runat="server" SelectMethod="GetEmployeeCompetency"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="odsCompetency_Selecting"
        DeleteMethod="DeleteEmployeeCompetency">
        <deleteparameters>
            <asp:Parameter Name="EmployeeCompetencyId" />
        </deleteparameters>
        <selectparameters>
            <asp:Parameter Name="employeeId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
