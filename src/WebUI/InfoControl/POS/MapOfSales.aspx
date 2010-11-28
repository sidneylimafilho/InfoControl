<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_POS_MapOfSales" Title="Mapa de Vendas"
    CodeBehind="MapOfSales.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox31" width="100%">
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
            <td class="center" align="left">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td valign="top" width="50%">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <h3>
                                                    Por Forma de Pgto:</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="white-space: nowrap; width: 1%">
                                                Data de Início:
                                                <asp:TextBox ID="txtPaymentStartDate" runat="server" Columns="8" MaxLength="10">
                                                </asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtPaymentStartDate"
                                                    CultureName="pt-BR" Mask="99/99/9999" MaskType="Date">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td valign="top" style="white-space: nowrap;">
                                                <asp:Image ID="btnCalendar" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Controls/Calendar/img/btncalendar.gif"
                                                    Style="cursor: pointer;" /><ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                                        CssClass="cCal11" PopupButtonID="btnCalendar" TargetControlID="txtPaymentStartDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                &nbsp;
                                                <asp:ImageButton ID="ImageButton3" runat="server" OnClick="txtPaymentStartDate_Click"
                                                    ImageUrl="~/App_Shared/themes/glasscyan/Company/view.gif" />&nbsp;&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnPaymentChart" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Company/chart-pie.gif"
                                                    OnClick="btnPaymentChart_Click" />
                                                <asp:CompareValidator ID="CompareValidator1" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                                    ControlToValidate="txtPaymentStartDate"  Operator="DataTypeCheck"
                                                    Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grdPayment" runat="server" AutoGenerateColumns="False" DataSourceID="odsSelect_Department"
                                        Width="100%" OnRowDataBound="grdPayment_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Forma de Pagamento"></asp:BoundField>
                                            <asp:BoundField DataField="Value" HeaderText="Total">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <center class="cTxt13">
                                                Não há dados a serem exibidos</center>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <DCWC:Chart ID="chartPayment" runat="server" BackColor="White" BackGradientEndColor="White"
                                        BorderLineColor="LightGray" DataSourceID="odsSelect_Department" Height="200px"
                                        Palette="SemiTransparent" ImageType="Jpeg" Visible="False" BackGradientType="DiagonalLeft">
                                        <BorderSkin FrameBackColor="LightSkyBlue" FrameBackGradientEndColor="DodgerBlue" />
                                        <Series>
                                            <DCWC:Series Name="Default" ChartType="Doughnut" ValueMemberX="Name" ValueMembersY="Value"
                                                ShowLabelAsValue="True" BorderColor="120, 50, 50, 50">
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
                                        <Legend BorderColor="Gray" BorderWidth="1"></Legend>
                                    </DCWC:Chart>
                                    <div style="text-align: right">
                                        <b>
                                            <asp:Label ID="lblPaymentTotal" runat="server"></asp:Label></b>
                                    </div>
                                    <VFX:businessmanagerdatasource ID="odsSelect_Department" runat="server" onselecting="odsSelect_Department_Selecting"
                                        selectmethod="MapOfSale_Payment" typename="Vivina.Erp.BusinessRules.SaleManager">
                                        <selectparameters>
						<asp:parameter Name="companyId" Type="Int32" />
						<asp:parameter Name="dateInterval" Type="Object" />
						<%--<asp:parameter Name="endDate" Type="DateTime" />--%>
									</selectparameters>
                                    </VFX:businessmanagerdatasource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <h3>
                                                    Por Vendedor:</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="white-space: nowrap; width: 1%">
                                                Data de Início:
                                                <asp:TextBox ID="txtSalStartDate" runat="server" Columns="8" MaxLength="10">
                                                </asp:TextBox><ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server"
                                                    TargetControlID="txtSalStartDate" CultureName="pt-BR" Mask="99/99/9999" MaskType="Date">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td style="white-space: nowrap;">
                                                <asp:Image ID="btnCalendar1" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Controls/Calendar/img/btncalendar.gif"
                                                    Style="cursor: pointer;" /><ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                                        CssClass="cCal11" PopupButtonID="btnCalendar1" TargetControlID="txtSalStartDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                &nbsp;
                                                <asp:ImageButton ID="ImageButton2" runat="server" OnClick="txtSalStartDate_Click"
                                                    ImageUrl="~/App_Shared/themes/glasscyan/Company/view.gif" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnChartSalesperson" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Company/chart-pie.gif"
                                                    OnClick="btnChartSalesperson_Click" />
                                                <asp:CompareValidator ID="CompareValidator2" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                                    ControlToValidate="txtSalStartDate"  Operator="DataTypeCheck"
                                                    Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grdSalesMan" runat="server" DataSourceID="odsSelect_SalesMan" Width="100%"
                                        AutoGenerateColumns="False" OnRowDataBound="grdSalesMan_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Vendedor"></asp:BoundField>
                                            <asp:BoundField DataField="Value" HeaderText="Total">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <center class="cTxt13">
                                                Não há dados a serem exibidos</center>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <DCWC:Chart ID="chartSalesperson" runat="server" BackColor="White" BackGradientEndColor="White"
                                        BorderLineColor="LightGray" DataSourceID="odsSelect_SalesMan" Height="200px"
                                        Palette="SemiTransparent" ImageType="Jpeg" Visible="False" BackGradientType="DiagonalLeft">
                                        <BorderSkin FrameBackColor="LightSkyBlue" FrameBackGradientEndColor="DodgerBlue" />
                                        <Series>
                                            <DCWC:Series Name="Default" ChartType="Doughnut" ValueMemberX="Name" ValueMembersY="Value"
                                                ShowLabelAsValue="True" BorderColor="120, 50, 50, 50">
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
                                        <Legend BorderColor="Gray" BorderWidth="1"></Legend>
                                    </DCWC:Chart>
                                    <div style="text-align: right">
                                        <b>
                                            <asp:Label ID="lblSalesPersonTotal" runat="server"></asp:Label></b>
                                    </div>
                                    <VFX:businessmanagerdatasource ID="odsSelect_SalesMan" runat="server" onselecting="odsSelect_SalesMan_Selecting"
                                        SelectMethod="MapOfSale_SalesPerson" TypeName="Vivina.Erp.BusinessRules.SaleManager">
                                        <selectparameters>
									<asp:parameter Name="companyId" Type="Int32" />
									<asp:parameter Name="startDate" Type="DateTime" />
										</selectparameters>
                                    </VFX:businessmanagerdatasource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <h3>
                                                    Por Categoria:</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="white-space: nowrap; width: 1%">
                                                Data de Início:
                                                <asp:TextBox ID="txtCatStartDate" runat="server" Columns="8" MaxLength="10">
                                                </asp:TextBox></asp:TextBox><ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3"
                                                    runat="server" TargetControlID="txtCatStartDate" CultureName="pt-BR" Mask="99/99/9999"
                                                    MaskType="Date">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td style="white-space: nowrap;">
                                                <asp:Image ID="btnCalendar2" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Controls/Calendar/img/btncalendar.gif"
                                                    Style="cursor: pointer;" /><ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server"
                                                        CssClass="cCal11" PopupButtonID="btnCalendar2" TargetControlID="txtCatStartDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                &nbsp;
                                                <asp:ImageButton ID="ImageButton1" runat="server" OnClick="txtCatStartDate_Click"
                                                    ImageUrl="~/App_Shared/themes/glasscyan/Company/view.gif" />&nbsp;&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnCategoryChart" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Company/chart-pie.gif"
                                                    OnClick="btnCategoryChart_Click" />
                                                <asp:CompareValidator ID="CompareValidator3" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                                    ControlToValidate="txtCatStartDate"  Operator="DataTypeCheck"
                                                    Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grdCategory" runat="server" DataSourceID="odsSelect_Category" Width="100%"
                                        AutoGenerateColumns="False" OnRowDataBound="grdCategory_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Categoria"></asp:BoundField>
                                            <asp:BoundField DataField="Value" HeaderText="Total">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <center class="cTxt13">
                                                Não há dados a serem exibidos</center>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <DCWC:Chart ID="chartCategory" runat="server" BackColor="White" BackGradientEndColor="White"
                                        BorderLineColor="LightGray" DataSourceID="odsSelect_Category" Height="200px"
                                        Palette="SemiTransparent" ImageType="Jpeg" Visible="False" BackGradientType="DiagonalLeft">
                                        <BorderSkin FrameBackColor="LightSkyBlue" FrameBackGradientEndColor="DodgerBlue" />
                                        <Series>
                                            <DCWC:Series Name="Default" ChartType="Doughnut" ValueMemberX="Name" ValueMembersY="Value"
                                                ShowLabelAsValue="True" BorderColor="120, 50, 50, 50">
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
                                        <Legend BorderColor="Gray" BorderWidth="1"></Legend>
                                    </DCWC:Chart>
                                    <div style="text-align: right">
                                        <b>
                                            <asp:Label ID="lblCategoryTotal" runat="server"></asp:Label>
                                        </b>
                                    </div>
                                    <VFX:businessmanagerdatasource ID="odsSelect_Category" runat="server" onselecting="odsSelect_Category_Selecting"
                                        SelectMethod="MapOfSale_Category" TypeName="Vivina.Erp.BusinessRules.SaleManager">
                                        <selectparameters>
									<asp:parameter Name="companyId" Type="Int32" />
									<asp:parameter Name="startDate" Type="DateTime" />
												</selectparameters>
                                    </VFX:businessmanagerdatasource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <h3>
                                                    Totais :</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="white-space: nowrap; width: 1%">
                                                Data de Início:
                                                <asp:TextBox ID="txtTtlStartDate" runat="server" Columns="8" MaxLength="10">
                                                </asp:TextBox><ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server"
                                                    TargetControlID="txtTtlStartDate" CultureName="pt-BR" Mask="99/99/9999" MaskType="Date">
                                                </ajaxToolkit:MaskedEditExtender>
                                            </td>
                                            <td style="white-space: nowrap;">
                                                <asp:Image ID="btnCalendar3" runat="server" ImageUrl="~/App_Shared/themes/glasscyan/Controls/Calendar/img/btncalendar.gif"
                                                    Style="cursor: pointer;" /><ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server"
                                                        CssClass="cCal11" PopupButtonID="btnCalendar3" TargetControlID="txtTtlStartDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                <asp:ImageButton ID="btnViewTotal" runat="server" OnClick="txtTtlStartDate_Click"
                                                    ImageUrl="~/App_Shared/themes/glasscyan/Company/view.gif" />
                                                <asp:CompareValidator ID="CompareValidator4" ErrorMessage="&nbsp;&nbsp;&nbsp;" runat="server"
                                                    ControlToValidate="txtTtlStartDate"  Operator="DataTypeCheck"
                                                    Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grdTotals" runat="server" DataSourceID="odsSelect_Totals" Width="100%"
                                        AutoGenerateColumns="False" OnRowDataBound="grdTotals_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="SaleDate" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Value" HeaderText="Total">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <center class="cTxt13">
                                                Não há dados a serem exibidos</center>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div style="text-align: right">
                                        <b>
                                            <asp:Label ID="lblTotals" runat="server"></asp:Label>
                                        </b>
                                    </div>
                                    <VFX:businessmanagerdatasource ID="odsSelect_Totals" runat="server" onselecting="odsSelect_Totals_Selecting"
                                        SelectMethod="MapOfSale_Totals" TypeName="Vivina.Erp.BusinessRules.SaleManager">
                                        <selectparameters>
									<asp:parameter Name="companyId" Type="Int32" />
									<asp:parameter Name="startDate" Type="DateTime" />
												</selectparameters>
                                    </VFX:businessmanagerdatasource>
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
    </table>
    <% //<uc2:ToolTip ID="tipMapOfSales" runat="server" Message=" Através deste quadro pode-se ter uma 'visão' mais abrangente das vendas. Utilize esta ferramenta para conhecer melhor a performance das vendas. Cruzando estas informações, a empresa poderá investir melhor em treinamento e propaganda, e trabalhar sua margem e diversidade de produtos."
        // Title="Dica:" Indication="top" Top="160px" Left="150px" Visible="true" /-->%>
    <%--<uc2:ToolTip ID="tipPaypamentMethod" runat="server" Message="<b>Forma de Pagamento</b> – Serve para a empresa saber quais as formas de pagamentos que seus clientes mais utilizam e moldar seu ponto de venda, margens de lucro e prazos de pagamento de acordo com essa preferência de seus clientes."
        Title="Dica:" Indication="left" Top="30px" Left="55px" Visible="true" />
    <uc2:ToolTip ID="tipSalesman" runat="server" Message="<b>Vendedor</b> – Importante para o empresário saber o desempenho de cada um de seus vendedores. Acompanhe sua “Força de Vendas” bem de perto – vendedores bem treinados trazem vendas mais saudáveis e lucrativas"
        Title="Dica:" Indication="left" Top="30px" Left="450px" Visible="true" />
    <uc2:ToolTip ID="tipCategory" runat="server" Message="<b>Categoria</b> – Com esse gráfico a empresa sabe qual o desempenho de cada uma de suas categorias de produtos. Assim, você saberá como incentivar sua “Força de Vendas”, em quais categorias atacar, margens e promoções."
        Title="Dica:" Indication="left" Top="10px" Left="55px" Visible="true" />
    <uc2:ToolTip ID="tipTotals" runat="server" Message="<b>Totais</b> – Importante para a empresa saber quanto foi o valor das suas vendas – conheça suas metas, e saiba o quanto falta para atingi-lás."
        Title="Dica:" Indication="left" Top="10px" Left="450px" Visible="true" />--%>
</asp:Content>
