<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true" Inherits="Company_Reports_Default" Title="Untitled Page" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div style="margin: 10px">
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
                    <table border="1">
                        <tr>
                            <td valign="top">
                                <h2>
                                    Selecione um Relatório</h2>
                                <br />
                                <br />
                                <%if (Request["type"] == "Strategy")
                                  { %>
                                <a href="javascript:;" onclick="SelectReport('','../../App_Themes/Glass/Company/Reports/Accounting/Report1.gif')">
                                    Teste</a> <a href="javascript:;" onclick="SelectReport('','../../App_Themes/Glass/Company/Reports/Accounting/Report2.gif')">
                                        Teste2</a>
                                <%}
                                  else if (Request["type"] == "Sales")
                                  { %>
                                <%}
                                  else if (Request["type"] == "Purchases")
                                  { %>
                                <%}
                                  else if (Request["type"] == "Inventory")
                                  { %>
                                <%}
                                  else if (Request["type"] == "Financial")
                                  { %>
                                <%}
                                  else if (Request["type"] == "RH")
                                  { %>
                                <%} %>
                            </td>
                            <td rowspan="2" valign="top">
                                <img id="reportImage" src="#" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
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
    </div>

    <script type="text/javascript">
    var _reportUrl;
    function SelectReport(reportUrl, imageUrl)
    {
        $get("reportImage").src = imageUrl;
        _reportUrl = reportUrl;
    }    
    function ExecuteReport()
    {
        //$get("reportImage").src = imageUrl;
        //top.InfoControl.        
    }
    </script>

</asp:Content>
