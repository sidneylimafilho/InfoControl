<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    Inherits="InfoControl_Host_PakageByFunctions" CodeBehind="FunctionsByPackages.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder">
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
                <table>
                    <tr>
                        <td>
                            Funções:<br />
                            <VFX:DualListBox runat="server" ID="dlbFunctions" DataSourceID="odsRemainingFunctions"
                                DataTextField="Name" DataValueField="FunctionId" InsertAllImageUrl="" InsertImageUrl=""
                                LeftDataSourceID="odsRemainingFunctions" LeftDataTextField="Name" LeftDataValueField="FunctionId"
                                RemoveAllImageUrl="" RemoveImageUrl="" RightDataSourceID="odsFunctionsByPackages"
                                RightDataTextField="Name" RightDataValueField="FunctionId" Width="600px" Height="300px">
                            </VFX:DualListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <div align="right">
                                <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="parent.location='Packages.aspx'; return false;" />
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
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
    <VFX:BusinessManagerDataSource ID="odsRemainingFunctions" runat="server" SelectMethod="GetRemainingFunctionsByPackage"
        TypeName="Vivina.Erp.BusinessRules.PackageFunctionManager" OnSelecting="odsRemainingFunctions_Selecting">
        <selectparameters>
        <asp:Parameter Name="packageId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsFunctionsByPackages" runat="server" SelectMethod="GetFunctionByPackages"
        TypeName="Vivina.Erp.BusinessRules.PackageFunctionManager" OnSelected="odsFunctionsByPackages_Selected"
        OnSelecting="odsFunctionsByPackages_Selecting">
        <selectparameters>
        <asp:Parameter Name="PackageId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
