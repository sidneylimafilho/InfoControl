<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" Inherits="Company_Reports_Static_Default" Title="Untitled Page" Codebehind="Default.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Relatórios</h1>
    <table class="cLeafBox21" width="100%">
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
                <table border="0">
                    <tr>
                        <td colspan="2">
                            Informe o Relatório desejado: (Clientes por Estado, Vendas por Departamento, etc.)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtReporter" runat="server" Width="200"></asp:TextBox>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtReporter" />
                                </triggers>
                                <contenttemplate>
                                    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="BusinessManagerDataSource1">
                                        <ItemTemplate>
                                            <p class="cTxt21b" style="cursor: hand;" onclick="SelectReport('<%#Eval("ReportUrl")%>', '<%#Eval("ImageUrl")%>')">
                                                <%#Eval("Name").ToString().ToUpper().Replace(txtReporter.Text.ToUpper(), "<font color='#663333'>" + txtReporter.Text.ToUpper() + "</font>")%></p>
                                            <p>
                                                <%#Eval("Description") %></p>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <VFX:BusinessManagerDataSource ID="BusinessManagerDataSource1" runat="server" onselecting="BusinessManagerDataSource1_Selecting"
                                        SelectMethod="SearchReport" TypeName="Vivina.Erp.BusinessRules.Reports.ReportsManager"
                                        OldValuesParameterFormatString="original_{0}">
                                        <selectparameters>
                                                        <asp:Parameter Name="name" Type="String" />
                                                        <asp:Parameter DefaultValue="10" Name="maximumRows" Type="Int32" />
                                                    </selectparameters>
                                    </VFX:BusinessManagerDataSource>
                                </contenttemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="right" rowspan="2">
                            <img style="border: 1px solid black; display: none" id="reportImage" src="#" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="reportFilter" style="display: none">
                                <tr>
                                    <td nowrap="nowrap">
                                        Data de Inicio:<br />
                                        <asp:TextBox ID="txtDueDate" runat="server" Columns="8" MaxLength="10"></asp:TextBox>
                                        <asp:CompareValidator ID="valtxtDueDate" runat="server" ControlToValidate="txtDueDate"
                                            CssClass="cErr21" Operator="GreaterThanEqual" Type="Date" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                            ValueToCompare="1/1/1753">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:CompareValidator>
                                    </td>
                                    <td nowrap="nowrap" valign="bottom">
                                        <asp:Image ID="btnCalendar" ImageUrl="~/App_Themes/GlassCyan/Controls/Calendar/img/btncalendar.gif"
                                            runat="server" Style="cursor: pointer;" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDueDate"
                                            PopupButtonID="btnCalendar" CssClass="cCal11">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" ClearMaskOnLostFocus="False"
                                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtDueDate" CultureName="pt-BR">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td nowrap="nowrap">
                                        Data Término:<br />
                                        <asp:TextBox ID="txtEndDate" runat="server" Columns="8" MaxLength="10"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtDueDate"
                                            CssClass="cErr21" Operator="GreaterThanEqual" Type="Date" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                            ValueToCompare="1/1/1753">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:CompareValidator>
                                    </td>
                                    <td nowrap="nowrap" valign="bottom">
                                        <asp:Image ID="btnCalendar2" ImageUrl="~/App_Themes/GlassCyan/Controls/Calendar/img/btncalendar.gif"
                                            runat="server" Style="cursor: pointer;" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                                            PopupButtonID="btnCalendar2" CssClass="cCal11">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="False"
                                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtEndDate" CultureName="pt-BR">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExecuteReport" runat="server" Text="Gerar" OnClientClick="ExecuteReport();"
                                            PostBackUrl="../ReportViewer.aspx" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
    <input type="hidden" name="r" value="" />
    <input type="hidden" name="@CustomerId" value="<%=Company.CompanyId %>" />
    <input type="hidden" name="f" value="" />
    <input type="hidden" name="g" value="" />

    <script type="text/javascript">
    function SelectReport(reportUrl, imageUrl)
    {
        $get("r").value=reportUrl;
        $get("reportImage").src = imageUrl;
        $get("reportImage").style.display='';
        $("#reportFilter").show("slow");
    }    
    
    function ExecuteReport()
    {
        window.open('', 'report', 'toolbars=no, menubar=no, status=no')        
        document.forms[0].target = "report";        
    }
  
    </script>

</asp:Content>
