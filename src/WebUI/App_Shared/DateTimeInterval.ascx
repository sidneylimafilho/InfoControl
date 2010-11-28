<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimeInterval.ascx.cs"
    Inherits="InfoControl.Web.UI.DateTimeIntervalControl" %>
<table>
    <tr>
        <td>
            Início:<br />
            <table cellpadding="0" cellspacing="0" nowrap="true">
                <tr>
                    <td>
                        <asp:TextBox ID="txtBeginDate" Columns="8" MaxLength="10" plugin="calendar" mask="99/99/9999" runat="server"></asp:TextBox>
                    </td>
                    <td valign="middle">
                        <asp:CompareValidator ID="cmptxtBeginDate" runat="server" Display="Dynamic" ControlToValidate="txtBeginDate"
                             Operator="GreaterThanEqual"  ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                            Type="Date" ValueToCompare="1/1/1753">
                        </asp:CompareValidator>
                        <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtBeginDate"  runat="server" ControlToValidate="txtBeginDate" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            Fim:<br />
            <table cellpadding="0" cellspacing="0"  nowrap="true">
                <tr>
                    <td>
                        <asp:TextBox ID="txtEndDate" Columns="8" MaxLength="10" plugin="calendar" mask="99/99/9999" runat="server"></asp:TextBox>
                    </td>                  
                    <td valign="middle">
                        <asp:CompareValidator ID="cmptxtEndDate" runat="server" ControlToValidate="txtEndDate"
                             Operator="GreaterThanEqual" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                            Type="Date" ValueToCompare="1/1/1753" Display="Dynamic">
                        </asp:CompareValidator>
                        <asp:CompareValidator ID="cmpDates" runat="server" ControlToValidate="txtEndDate"
                            ControlToCompare="txtBeginDate"  Operator="GreaterThan" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                            Type="Date" Display="Dynamic">
                        </asp:CompareValidator>
                        <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtEndDate"  runat="server" ControlToValidate="txtBeginDate" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>           
        </td>
    </tr>
</table>
