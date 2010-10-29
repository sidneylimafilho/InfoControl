<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_ComboTreeBox"
    CodeBehind="ComboTreeBox.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div>
    <span id="cboTreeBox" class="comboDivBox" runat="server">
        <asp:Literal ID="lblAccountingPlanName" runat="server"></asp:Literal>
    </span>
    <div id="<%=this.MenuID %>" class="comboDivBox_menu" style="width: 400px; display: none">
        
        <telerik:RadTreeView ID="tree" runat="server" Height="100%" Width="100%" BeforeClientClick="nodeClicking"
            OnNodeDataBound="tree_NodeDataBound">
        </telerik:RadTreeView>
    </div>
    <input id="<%=this.ClientID %>" type="hidden" value="<%=this.SelectedValue %>" style="display: none" />
</div>
