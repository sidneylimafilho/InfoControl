<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    EnableViewState="true" Title="Visualizador de Eventos" EnableEventValidation="false"
    Inherits="EventViewer" CodeBehind="EventViewer.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/LeafBox.ascx" TagName="LeafBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    
    <div style="width: 100%">
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
                <td class="left" style="height: 97px">
                    &nbsp;
                </td>
                <td class="center" style="height: 97px">
                    <fieldset id="filter" class="closed">
                        <legend onmouseover='setTimeout("$(\"#filter .body\").show(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"open\"})", 300);'>
                            Escolha o filtro desejado: </legend><div class="body">
                                <table>
                                    <tr>
                                        <td>
                                            Status:<br />
                                            <asp:DropDownList ID="cboStatus" runat="server" EnableViewState="true">
                                                <asp:ListItem Value="1" Text="Abertos"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Fechados"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Aguardando Detalhes"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Excluidos"></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            Tipo de Evento:<br />
                                            <asp:DropDownList ID="cboEventType" runat="server" EnableViewState="true">
                                                <asp:ListItem Value="1" Text="Alertas"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Informações"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Sugestões"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Erros"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Todos" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <%--<td align="left">
                                            Técnico:<br />
                                            <asp:DropDownList ID="cboTechnicalUser" runat="server" DataSourceID="odsTechnicalUsers"
                                                DataTextField="Name" DataValueField="UserId" AppendDataBoundItems="True" ValidationGroup="grpFilter">
                                                <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>--%>
                                        <td>
                                            Data Inicial:<br />
                                            <asp:TextBox ID="txtBeginDate" runat="server" Width="60px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="mskTxtBeginDate" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtBeginDate">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="reqBeginDate" runat="server" ControlToValidate="txtBeginDate"
                                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="grpFilter">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpTxtBeginDate" runat="server" ValueToCompare="1/1/1753"
                                                ControlToValidate="txtBeginDate" Operator="GreaterThanEqual" ValidationGroup="grpFilter"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Type="Date">
                                            </asp:CompareValidator>
                                        </td>
                                        <td>
                                            Data Final:<br />
                                            <asp:TextBox ID="txtEndDate" runat="server" Width="60px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="mskTxtEndDate" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtEndDate">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="reqtxtEndDate" runat="server" ControlToValidate="txtEndDate"
                                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="grpFilter">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpTxtEndDate" runat="server" ControlToCompare="txtBeginDate"
                                                ControlToValidate="txtEndDate" Operator="GreaterThanEqual" ValidationGroup="grpFilter"
                                                CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Type="Date">
                                            </asp:CompareValidator>
                                        </td>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        Exibir:<br />
                                                        <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnFilter" runat="server" Text="Filtrar" ValidationGroup="grpFilter"
                                                OnClick="btnFilter_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div><span class="closeButton" onmouseover='setTimeout("$(\"#filter .body\").hide(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"closed\"})", 950);'>
                                &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:GridView ID="grdEventViewer" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsEventsViewer" DataKeyNames="EventId,EventType,Name,Source,Message,StackTrace,Path,RefererUrl,HelpLink,TargetSite,CurrentDate,ExceptionCode,ApplicationId,UserId,EventStatusId"
                        AllowSorting="True" Width="100%" OnSorting="grdEventViewer_Sorting" OnRowDataBound="grdEventViewer_RowDataBound"
                         PageSize="20" AllowPaging="True" RowSelectable="false"  OnPreRender="grdEventViewer_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="Prioridade" SortExpression="Rating">
                                <ItemTemplate>
                                    <ajaxToolkit:Rating ID="rtnPriority" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                        WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                        ToolTip="Classificação" ReadOnly="true" CurrentRating='<%# Convert.ToInt32(Eval("Rating")) %>'
                                        Enabled="false">
                                    </ajaxToolkit:Rating>
                                </ItemTemplate>
                                <ItemStyle Width="1%" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Status" DataField="EventStatusName" SortExpression="EventStatusName" />
                            <asp:TemplateField HeaderText="Assunto" SortExpression="Name">
                                <ItemTemplate>
                                    <b>
                                        <%# Eval("Name") %>
                                    </b>
                                    <br />
                                    <span class="info">
                                        <%# Eval("Path") %>
                                    </span><span class="moreInfo">&nbsp;&nbsp;|&nbsp;&nbsp;Usuário:&nbsp;<%# Eval("UserName")%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <%--   <asp:BoundField HeaderText="Técnico" DataField="TechnicalUserName" SortExpression="TechnicalUserName" />--%>
                            <asp:BoundField HeaderText="Data da Ocorrência" DataField="CurrentDate" SortExpression="CurrentDate" />
                            <asp:TemplateField HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot;&lt;/div&gt;"
                                SortExpression="Insert">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" id='<%# Eval("EventId") %>' userid='<%# User.Identity.UserId %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="right" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos, clique no botão para cadastrar um evento.<br />
                                &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
                <td class="right" style="height: 97px">
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
    <VFX:BusinessManagerDataSource ID="odsTechnicalUsers" runat="server" TypeName="Vivina.Erp.BusinessRules.CompanyManager"
        SelectMethod="GetTechnicalUserAsDataTable" onselecting="odsTechnicalUsers_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsEventsViewer" runat="server" SelectMethod="GetOpenEvents"
        TypeName="Vivina.Erp.BusinessRules.EventManager" ConflictDetection="CompareAllValues"
        SortParameterName="sortExpression" EnablePaging="True" SelectCountMethod="GetOpenEventsCount"
        onselecting="odsEventsViewer_Selecting" DeleteMethod="ResolveEvent" ondeleting="odsEventsViewer_Deleting">
        <deleteparameters>
            <asp:Parameter Name="entity" Type="Object" />
        </deleteparameters>
        <selectparameters>
            <asp:Parameter Name="eventStatusId" Type="Int32" />           
            <asp:Parameter Name="beginDate" Type="DateTime" />
            <asp:Parameter Name="endDate" Type="DateTime" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
