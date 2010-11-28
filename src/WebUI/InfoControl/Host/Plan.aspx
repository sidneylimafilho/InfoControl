<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="True"
    CodeBehind="Plan.aspx.cs" Inherits="Vivina.Erp.WebUI.Host.Plan"
    Title="Untitled Page" %>

<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h1>
        Plano
    </h1>
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
                <%--Conteúdo--%>
                <table width="100%">
                    <tr>
                        <td>
                            Nome:
                            <br />
                            <asp:TextBox runat="server" ID="txtName" MaxLength="50" ValidationGroup="Save"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtName" ControlToValidate="txtName" runat="server"
                                ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="Save" />
                        </td>
                        <td>
                            <uc1:DateTimeInterval ID="ucDateTimeInterval" Required="true" ValidationGroup="Save"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Aplicação:<br />
                            <asp:DropDownList runat="server" DataTextField="Name" DataValueField="ApplicationId"
                                DataSourceID="odsApplications" ID="cboApplication" AppendDataBoundItems="true">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqCboApplication" ControlToValidate="cboApplication"
                                runat="server" ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="Save" />
                        </td>
                        <td>
                            Pacote:<br />
                            <asp:DropDownList runat="server" DataTextField="Name" DataValueField="PackageId"
                                DataSourceID="odsPackages" ID="cboPackage" AppendDataBoundItems="true">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqCboPackage" ControlToValidate="cboPackage" runat="server"
                                ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="Save" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Salvar" ValidationGroup="Save" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='Plans.aspx'; return false;" />
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
    <VFX:BusinessManagerDataSource ID="odsPackages" runat="server" SelectMethod="GetAllPackages"
        TypeName="Vivina.Erp.BusinessRules.PackagesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsBraches" runat="server" SelectMethod="GetAllBranches"
        TypeName="Vivina.Erp.BusinessRules.BranchManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsApplications" runat="server" SelectMethod="GetAllApplications"
        TypeName="Vivina.Erp.BusinessRules.ApplicationManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
