<%@ Page Language="C#"
 MasterPageFile="~/InfoControl/Default.master" 
 AutoEventWireup="true" Inherits="InfoControl_Host_SiteTemplate"
  Title="Modelo de sites" Codebehind="SiteTemplate.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        
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
                <!-- conteudo -->
                <table width="100%">
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox ID="txtName" MaxLength="128" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtName" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtName" ValidationGroup="saveSiteTemplate"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Ramo de Atividades:<br />
                            <asp:DropDownList ID="cboBranchId" runat="server" DataSourceID="BranchDataSource"
                                AppendDataBoundItems="True" DataTextField="Name" DataValueField="BranchId">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqcboBranchId" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="cboBranchId" ValidationGroup="saveSiteTemplate"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Modelo:<br />
                            <asp:FileUpload ID="fupSiteTemplate" runat="server" />
                            <asp:RequiredFieldValidator ID="reqfupSiteTemplate" runat="server" ControlToValidate="fupSiteTemplate"
                                ValidationGroup="saveSiteTemplate" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                            &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSave" ValidationGroup="saveSiteTemplate" runat="server" Text="Salvar"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <VFX:BusinessManagerDataSource ID="BranchDataSource" runat="server" SelectMethod="GetAllBranches"
                    TypeName="Vivina.Erp.BusinessRules.BranchManager" />
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
