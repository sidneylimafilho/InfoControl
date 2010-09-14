<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InfoControl/Default.master"
    EnableEventValidation="false" CodeBehind="PurchaseCompetencyValue.aspx.cs" Title="Competência de Compras"
    Inherits="Vivina.Erp.WebUI.Purchasing.PurchaseCompetencyValue" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                        <td align="left">
                            <telerik:RadTreeView Height="100%" ID="rtvCompetencyValue" runat="server" DataFieldID="Id"
                                DataFieldParentID="ParentId" AutoPostBack="True" DataTextField="Name" DataValueFieldId="CategoryId"
                                DataSourceID="odsCompetency" DataValueField="Id" OnNodeClick="rtvCompetencyValue_NodeClick"
                                OnNodeDataBound="rtvCompetencyValue_NodeDataBound">
                                <NodeTemplate>
                                    <%# !String.IsNullOrEmpty(Convert.ToString(Eval("CentralBuyer"))) &&
                                         Convert.ToBoolean(Eval("CentralBuyer"))
                                                ? String.Format("<h3>{0}</h3>", Eval("Name"))
                                                : Eval("Name")%>
                                    <%#Convert.ToString(Eval("Value")) != ""
                                                ? (" - <b>" + Convert.ToDecimal(Eval("Value")).ToString("f")) + "</b>"
                                                : ""%>
                                </NodeTemplate>
                            </telerik:RadTreeView>
                        </td>
                        <td valign="top" style="border-left: 1px solid gray; padding-left: 10px">
                            <asp:Panel runat="server" ID="pnlPurchaseCeilingValue">
                                Valor de Competência:<br />
                                <uc1:CurrencyField ID="ucCurrFieldPurchaseCeilingValue" runat="server" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="rbtCentralBuyer" runat="server" Text=" Principal Comprador?"></asp:CheckBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        
                        </contenttemplate>
                </asp:UpdatePanel>
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
    <VFX:BusinessManagerDataSource ID="odsCompetency" runat="server" onselecting="odsCompetency_Selecting"
        SelectMethod="GetPurchaseCompetencyTree" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
