<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="App_Shared_AccountingPlan" Codebehind="AccountingPlan.ascx.cs" %>
<%@ Register TagPrefix="radT" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
Plano de Contas:<br />
<span id="cboAcountingPlan" class="comboDivBox" onclick="$get('cboAcountingPlanMenu').style.display = ($get('cboAcountingPlanMenu').style.display==''?'none':'');">
    <asp:Literal ID="lblAccountingPlanName" runat="server"></asp:Literal>
</span>
<div id="cboAcountingPlanMenu" class="comboDivBox_menu" style="display: none;">

    <radT:RadTreeView ID="treeAcountingPlan" runat="server" DataFieldID="AccountingPlanId"
        DataFieldParentID="ParentId" DataTextField="Name" DataValueField="AccountingPlanId"
        Height="200px" Width="200px" BeforeClientClick="nodeClicking">
    </radT:RadTreeView>
</div>
<input id="<%=this.ClientID %>" type="text" value="<%=this.AccountingPlanId %>" style="display:none" />

