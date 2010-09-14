<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Date.ascx.cs" Inherits="InfoControl.Web.UI.Date" %>
<table nowrap="true" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:TextBox ID="txtDate" Columns="8" MaxLength="10" plugin="calendar" mask="99/99/9999"
                runat="server"></asp:TextBox> 
        </td>
        <td valign="bottom">           
            <asp:CompareValidator ID="cmptxtDate" runat="server" ControlToValidate="txtDate"
                CssClass="cErr21" Operator="GreaterThanEqual" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                Type="Date" ValueToCompare="1/1/1753" Display="Dynamic">
            </asp:CompareValidator>
        </td>
        <td valign="middle">
            <asp:RequiredFieldValidator ID="reqtxtDate" runat="server" ControlToValidate="txtDate"
                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic"></asp:RequiredFieldValidator>
            &nbsp;&nbsp;&nbsp;&nbsp;
        </td>
        <td>
            <asp:DropDownList ID="cboTime" runat="server" Visible="false">
                <asp:ListItem Text="00:00" Value="00:00"></asp:ListItem>
                <asp:ListItem Text="00:15" Value="00:15"></asp:ListItem>
                <asp:ListItem Text="00:30" Value="00:30"></asp:ListItem>
                <asp:ListItem Text="00:45" Value="00:45"></asp:ListItem>
                <asp:ListItem Text="01:00" Value="01:00"></asp:ListItem>
                <asp:ListItem Text="01:15" Value="01:15"></asp:ListItem>
                <asp:ListItem Text="01:30" Value="01:30"></asp:ListItem>
                <asp:ListItem Text="01:45" Value="01:45"></asp:ListItem>
                <asp:ListItem Text="02:00" Value="02:00"></asp:ListItem>
                <asp:ListItem Text="02:15" Value="02:15"></asp:ListItem>
                <asp:ListItem Text="02:30" Value="02:30"></asp:ListItem>
                <asp:ListItem Text="02:45" Value="02:45"></asp:ListItem>
                <asp:ListItem Text="03:00" Value="03:00"></asp:ListItem>
                <asp:ListItem Text="03:15" Value="03:15"></asp:ListItem>
                <asp:ListItem Text="03:30" Value="03:30"></asp:ListItem>
                <asp:ListItem Text="03:45" Value="03:45"></asp:ListItem>
                <asp:ListItem Text="04:00" Value="04:00"></asp:ListItem>
                <asp:ListItem Text="04:15" Value="04:15"></asp:ListItem>
                <asp:ListItem Text="04:30" Value="04:30"></asp:ListItem>
                <asp:ListItem Text="04:45" Value="04:45"></asp:ListItem>
                <asp:ListItem Text="05:00" Value="05:00"></asp:ListItem>
                <asp:ListItem Text="05:15" Value="05:15"></asp:ListItem>
                <asp:ListItem Text="05:30" Value="05:30"></asp:ListItem>
                <asp:ListItem Text="05:45" Value="05:45"></asp:ListItem>
                <asp:ListItem Text="06:00" Value="06:00"></asp:ListItem>
                <asp:ListItem Text="06:15" Value="06:15"></asp:ListItem>
                <asp:ListItem Text="06:30" Value="06:30"></asp:ListItem>
                <asp:ListItem Text="06:45" Value="06:45"></asp:ListItem>
                <asp:ListItem Text="07:00" Value="07:00"></asp:ListItem>
                <asp:ListItem Text="07:15" Value="07:15"></asp:ListItem>
                <asp:ListItem Text="07:30" Value="07:30"></asp:ListItem>
                <asp:ListItem Text="07:45" Value="07:45"></asp:ListItem>
                <asp:ListItem Text="08:00" Value="08:00"></asp:ListItem>
                <asp:ListItem Text="08:15" Value="08:15"></asp:ListItem>
                <asp:ListItem Text="08:30" Value="08:30"></asp:ListItem>
                <asp:ListItem Text="08:45" Value="08:45"></asp:ListItem>
                <asp:ListItem Text="09:00" Value="09:00"></asp:ListItem>
                <asp:ListItem Text="09:15" Value="09:15"></asp:ListItem>
                <asp:ListItem Text="09:30" Value="09:30"></asp:ListItem>
                <asp:ListItem Text="09:45" Value="09:45"></asp:ListItem>
                <asp:ListItem Text="10:00" Value="10:00"></asp:ListItem>
                <asp:ListItem Text="10:15" Value="10:15"></asp:ListItem>
                <asp:ListItem Text="10:30" Value="10:30"></asp:ListItem>
                <asp:ListItem Text="10:45" Value="10:45"></asp:ListItem>
                <asp:ListItem Text="11:00" Value="11:00"></asp:ListItem>
                <asp:ListItem Text="11:15" Value="11:15"></asp:ListItem>
                <asp:ListItem Text="11:30" Value="11:30"></asp:ListItem>
                <asp:ListItem Text="11:45" Value="11:45"></asp:ListItem>
                <asp:ListItem Text="12:00" Value="12:00"></asp:ListItem>
                <asp:ListItem Text="12:15" Value="12:15"></asp:ListItem>
                <asp:ListItem Text="12:30" Value="12:30"></asp:ListItem>
                <asp:ListItem Text="12:45" Value="12:45"></asp:ListItem>
                <asp:ListItem Text="13:00" Value="13:00"></asp:ListItem>
                <asp:ListItem Text="13:15" Value="13:15"></asp:ListItem>
                <asp:ListItem Text="13:30" Value="13:30"></asp:ListItem>
                <asp:ListItem Text="13:45" Value="13:45"></asp:ListItem>
                <asp:ListItem Text="14:00" Value="14:00"></asp:ListItem>
                <asp:ListItem Text="14:15" Value="14:15"></asp:ListItem>
                <asp:ListItem Text="14:30" Value="14:30"></asp:ListItem>
                <asp:ListItem Text="14:45" Value="14:45"></asp:ListItem>
                <asp:ListItem Text="15:00" Value="15:00"></asp:ListItem>
                <asp:ListItem Text="15:15" Value="15:15"></asp:ListItem>
                <asp:ListItem Text="15:30" Value="15:30"></asp:ListItem>
                <asp:ListItem Text="15:45" Value="15:45"></asp:ListItem>
                <asp:ListItem Text="16:00" Value="16:00"></asp:ListItem>
                <asp:ListItem Text="16:15" Value="16:15"></asp:ListItem>
                <asp:ListItem Text="16:30" Value="16:30"></asp:ListItem>
                <asp:ListItem Text="16:45" Value="16:45"></asp:ListItem>
                <asp:ListItem Text="17:00" Value="17:00"></asp:ListItem>
                <asp:ListItem Text="17:15" Value="17:15"></asp:ListItem>
                <asp:ListItem Text="17:30" Value="17:30"></asp:ListItem>
                <asp:ListItem Text="17:45" Value="17:45"></asp:ListItem>
                <asp:ListItem Text="18:00" Value="18:00"></asp:ListItem>
                <asp:ListItem Text="18:15" Value="18:15"></asp:ListItem>
                <asp:ListItem Text="18:30" Value="18:30"></asp:ListItem>
                <asp:ListItem Text="18:45" Value="18:45"></asp:ListItem>
                <asp:ListItem Text="19:00" Value="19:00"></asp:ListItem>
                <asp:ListItem Text="19:15" Value="19:15"></asp:ListItem>
                <asp:ListItem Text="19:30" Value="19:30"></asp:ListItem>
                <asp:ListItem Text="19:45" Value="19:45"></asp:ListItem>
                <asp:ListItem Text="20:00" Value="20:00"></asp:ListItem>
                <asp:ListItem Text="20:15" Value="20:15"></asp:ListItem>
                <asp:ListItem Text="20:30" Value="20:30"></asp:ListItem>
                <asp:ListItem Text="20:45" Value="20:45"></asp:ListItem>
                <asp:ListItem Text="21:00" Value="21:00"></asp:ListItem>
                <asp:ListItem Text="21:15" Value="21:15"></asp:ListItem>
                <asp:ListItem Text="21:30" Value="21:30"></asp:ListItem>
                <asp:ListItem Text="21:45" Value="21:45"></asp:ListItem>
                <asp:ListItem Text="22:00" Value="22:00"></asp:ListItem>
                <asp:ListItem Text="22:15" Value="22:15"></asp:ListItem>
                <asp:ListItem Text="22:30" Value="22:30"></asp:ListItem>
                <asp:ListItem Text="22:45" Value="22:45"></asp:ListItem>
                <asp:ListItem Text="23:00" Value="23:00"></asp:ListItem>
                <asp:ListItem Text="23:15" Value="23:15"></asp:ListItem>
                <asp:ListItem Text="23:30" Value="23:30"></asp:ListItem>
                <asp:ListItem Text="23:45" Value="23:45"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>

