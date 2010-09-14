<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_SelectEmployee"
    CodeBehind="SelectEmployee.ascx.cs" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<table width="100%">
    <tr>
        <td>
            <asp:TextBox ID="txtEmployee" runat="server" Width="300px" CssClass="cDynDat11" AutoPostBack="True"
                OnTextChanged="txtEmployee_TextChanged" MaxLength="100"></asp:TextBox>
                <p style="font-size: 7pt; color: gray">
                Dica: Digite parte do texto, que o completará automaticamente!</p>
        </td>
        <td valign="bottom">
            <img id="imgAddEmployee" src="~/App_Themes/_global/Company/user_add.gif" runat="server"
                alt="Inserir Funcionário" border="0" onclick="top.tb_show('Cadastro de Funcionário','RH/Employee_PersonalData.aspx?w=modal');" />
            &nbsp;&nbsp;&nbsp;&nbsp;
        </td>
    </tr>
</table>
<asp:Panel ID="pnlEmployee" runat="server" Visible="false">
    <table border="0" width="100%">
        <tr>
            <td>
                <asp:LinkButton ID="lnkEmployeeName" runat="server"></asp:LinkButton>
                <asp:Label ID="lblCPF" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblEmployeeAddress" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblEmployeeLocalization" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblEmployeePhone" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<input id="<%=this.ClientID %>" type="text" value="<%=ViewState["EmployeeId"] %>"
    style="display: none" />

<ajaxToolkit:AutoCompleteExtender ID="completetxtEmployee" runat="server" CompletionInterval="1000"
    CompletionSetCount="10" MinimumPrefixLength="3" ServiceMethod="SearchEmployee"
    ServicePath="~/InfoControl/SearchService.svc" TargetControlID="txtEmployee"
    BehaviorID="completetxtCustomer">
</ajaxToolkit:AutoCompleteExtender>
