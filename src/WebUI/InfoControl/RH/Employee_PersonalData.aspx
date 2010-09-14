<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Employee_PersonalData.aspx.cs" Inherits="Vivina.Erp.WebUI.RH.Employee_PersonalData" %>

<%@ Register Src="../Profile_NaturalPerson.ascx" TagName="Profile_NaturalPerson"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

  <h1>  <asp:Literal Text="Funcionário" ID="litTitle" Visible="false" runat="server"></asp:Literal> </h1>
    
    
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
                <uc1:Profile_NaturalPerson ID="ucProfile" runat="server"></uc1:Profile_NaturalPerson>
                <br />
                <div style="text-align: right; margin-right: 8px">
                    <asp:Button ID="btnSave" runat="server" CausesValidation="True" CommandName="Update"
                        CssClass="cBtn11" Text="Salvar" _permissionRequired="Employee" OnClick="btnSave_Click">
                    </asp:Button>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="cBtn11" Text="Cancelar" OnClick="CancelButton_Click"></asp:Button>
                </div>
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
</asp:Content>
