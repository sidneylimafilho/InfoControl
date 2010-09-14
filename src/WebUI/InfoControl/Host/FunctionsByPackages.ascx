<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="InfoControl_Host_PakageByFunctions" Codebehind="FunctionsByPackages.ascx.cs" %>
<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                        <div style="text-align: right">
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" onclick="btnSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
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
    TypeName="Vivina.InfoControl.BusinessRules.PackageFunctionManager" 
    onselecting="odsRemainingFunctions_Selecting">
    <SelectParameters>
        <asp:Parameter Name="packageId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsFunctionsByPackages" runat="server" SelectMethod="GetFunctionByPackages"
    TypeName="Vivina.InfoControl.BusinessRules.PackageFunctionManager" 
    onselected="odsFunctionsByPackages_Selected" 
    onselecting="odsFunctionsByPackages_Selecting">
    <SelectParameters>
        <asp:Parameter Name="PackageId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
