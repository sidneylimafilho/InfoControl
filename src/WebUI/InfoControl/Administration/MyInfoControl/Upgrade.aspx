<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true" Inherits="Company_Administration_MyInfoControl_Upgrade"
    Title="Upgrade" Codebehind="Upgrade.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
   
    <table class="cLeafBox21">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
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
                <asp:Label ID="lblInsertPackage" runat="server" Text="Selecione o pacote à ser adquirido:"></asp:Label>
                <asp:DropDownList ID="cboPackage" runat="server" DataSourceID="odsPackages" DataTextField="Name"
                    DataValueField="PackageId">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboPackage"
                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;"></asp:RequiredFieldValidator>
                <asp:ImageButton ID="btnInsert" runat="server" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                    OnClick="btnInsert_Click" />
                <asp:GridView ID="grdPackageAdditional" runat="server" Width="100%" AutoGenerateColumns="False"
                    DataSourceID="odsAdditionalPackages" DataKeyNames="AddonId" OnRowDataBound="grdPackageAdditional_RowDataBound"
                    OnRowDeleting="grdPackageAdditional_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Pacote" SortExpression="Name"></asp:BoundField>
                        <asp:BoundField DataField="Price" DataFormatString="{0:c}" HeaderText="Valor" SortExpression="Price">
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="&lt;img src=&quot;../../../App_Themes/Glass/Controls/GridView/img/Pixel_bg.gif&quot; class=&quot;delete&quot; border='0' /&gt;"
                                    Visible='<%# Convert.ToInt32(Eval("value")) == 1 %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
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
    <VFX:BusinessManagerDataSource ID="odsPackages" runat="server" SelectMethod="GetUpdatePackages"
        TypeName="Vivina.Erp.BusinessRules.PackagesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsAdditionalPackages" runat="server" onselecting="odsAdditionalPackages_Selecting"
        SelectMethod="GetAdditionalPackages" TypeName="Vivina.Erp.BusinessRules.PackagesManager">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
