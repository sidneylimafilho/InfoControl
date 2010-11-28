<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" Inherits="InfoControl_RH_XRay" Title="Untitled Page" Codebehind="XRay.aspx.cs" %>

<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1>
        Raio-X</h1>
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
                <table width="100%" cellspacing="5">
                    <tr>
                        <td valign="top" width="50%">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <h3>
                                                    Funcionários ativos / inativos:</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="white-space: nowrap; width: 1%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Data de Início:
                                                            <asp:TextBox ID="txtStartDate" runat="server" Columns="8" MaxLength="10" ValidationGroup="SearchResults"></asp:TextBox>
                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtStartDate"
                                                                CultureName="pt-BR" Mask="99/99/9999" MaskType="Date">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                        </td>
                                                        <td valign="top" style="white-space: nowrap;">
                                                            <asp:Image ID="btnCalendarStart" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Controls/Calendar/img/btncalendar.gif"
                                                                Style="cursor: pointer;" /><ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                    CssClass="cCal11" PopupButtonID="btnCalendarStart" TargetControlID="txtStartDate">
                                                                </ajaxToolkit:CalendarExtender>
                                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator2" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                                                ControlToValidate="txtStartDate" ValidationGroup="SearchResults"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator1" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                                                ControlToValidate="txtStartDate"  Operator="DataTypeCheck" Type="Date"
                                                                ValidationGroup="SearchResults"></asp:CompareValidator>
                                                        </td>
                                                        <td>
                                                            Data fim:
                                                            <asp:TextBox ID="txtEndDate" runat="server" Columns="8" MaxLength="10" ValidationGroup="SearchResults"></asp:TextBox>
                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtEndDate"
                                                                CultureName="pt-BR" Mask="99/99/9999" MaskType="Date">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                        </td>
                                                        <td valign="top" style="white-space: nowrap;">
                                                            <asp:Image ID="btnCalendarEnd" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Controls/Calendar/img/btncalendar.gif"
                                                                Style="cursor: pointer;" /><ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                                                    CssClass="cCal11" PopupButtonID="btnCalendarEnd" TargetControlID="txtEndDate">
                                                                </ajaxToolkit:CalendarExtender>
                                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="RequiredFieldValidator1" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                                                ControlToValidate="txtEndDate" ValidationGroup="SearchResults"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="CompareValidator2" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                                                ControlToValidate="txtEndDate"  Operator="DataTypeCheck" Type="Date"
                                                                ValidationGroup="SearchResults"></asp:CompareValidator>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnChart" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Company/chart-pie.gif"
                                                                ValidationGroup="SearchResults" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <asp:Panel ID="pnlMessage" runat="server" Visible="False">
                                        <h1>
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label></h1>
                                    </asp:Panel>
                                    <DCWC:Chart ID="chartWonLostCauses" runat="server" BackGradientEndColor="White" BorderLineColor="LightGray"
                                        Height="200px" Palette="SemiTransparent" ImageType="Jpeg" BackGradientType="DiagonalLeft"
                                        Visible="False">
                                        <Legends>
                                            <DCWC:Legend BorderColor="Gray" Name="Default">
                                            </DCWC:Legend>
                                        </Legends>
                                        <BorderSkin FrameBackColor="LightSkyBlue" FrameBackGradientEndColor="DodgerBlue" />
                                        <Series>
                                            <DCWC:Series Name="Default" ChartType="Doughnut" ShowLabelAsValue="True" BorderColor="120, 50, 50, 50"
                                                BackGradientEndColor="102, 255, 255" Color="Red" ValueMemberX="Result" ValueMembersY="Quantity">
                                            </DCWC:Series>
                                        </Series>
                                        <ChartAreas>
                                            <DCWC:ChartArea Name="Default" BackColor="Transparent" BorderColor="Transparent"
                                                BorderStyle="Solid">
                                                <AxisY LineColor="DimGray">
                                                    <MajorGrid LineColor="DimGray" LineStyle="Dot" />
                                                    <MajorTickMark LineColor="DimGray" />
                                                </AxisY>
                                                <AxisX LineColor="DimGray">
                                                    <MajorGrid LineColor="DimGray" LineStyle="Dot" />
                                                    <MajorTickMark LineColor="DimGray" />
                                                </AxisX>
                                                <AxisX2 LineColor="DimGray">
                                                </AxisX2>
                                                <AxisY2 LineColor="DimGray">
                                                </AxisY2>
                                                <Area3DStyle Enable3D="True" />
                                            </DCWC:ChartArea>
                                        </ChartAreas>
                                    </DCWC:Chart>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
    <uc1:ToolTip ID="tipConfiguration" runat="server" Message="Este apanhado geral dos negócios, mostrará valiosos gráficos de desempenho por setor. Esteja sempre atento a esta ferramenta, pois esta é sua primeira indicadora de desempenho – seja este bom ou ruim."
        Title="Dica:" Indication="left" Top="40px" Left="480px" Visible="true" />
</asp:Content>
