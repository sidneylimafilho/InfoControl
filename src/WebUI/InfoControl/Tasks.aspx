<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Project_Tasks" Title="Tarefas" CodeBehind="Tasks.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<asp:Content ID="Content" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <fieldset id="filter" class="closed">
                    <legend onmouseover='setTimeout("$(\"#filter .body\").show(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"open\"})", 300);'>
                        Escolha o filtro desejado: </legend>
                    <div class="body" asform="true" trigger="#tasks">
                        Modo de Exibição:<br />
                        <table>
                            <tr>
                                <td colspan="9" style="border-bottom: 1px solid #099">
                                    <asp:RadioButton ID="rbtHierarchy" Text="Hierarquia" GroupName="rbtlTaskView" Value="2"
                                        runat="server" />
                                    <asp:RadioButton Text="Data" ID="rbtDate" GroupName="rbtlTaskView" Value="1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Parte do nome:
                                    <br />
                                    <asp:TextBox ID="txtTask" runat="server" field="name"></asp:TextBox>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                Início:<br />
                                                <input type="text" ID="txtBeginDate" columns="8" MaxLength="10" plugin="calendar" mask="99/99/9999"
                                                     field="inicio" options="{relatedCalendar:'#txtEndDate'}" />
                                            </td>
                                            <td>
                                                Fim:<br />
                                                <input type="text" ID="txtEndDate" columns="8" MaxLength="10" plugin="calendar" mask="99/99/9999"
                                                     field="fim" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <asp:RadioButtonList ID="rbtlTaskStatus" GroupName="rbtlTaskStatus" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" runat="server">
                                        <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Abertas"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Completas"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btSearch" command="click" runat="server" Text="Pesquisar" OnClientClick="return false;" />
                    </div>
                    <span class="closeButton" onmouseover='setTimeout("$(\"#filter .body\").hide(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"closed\"})", 950);'>
                        &nbsp;</span>
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <%-- 
                        - Este template separado pois será usado recursivamente sendo colocado uma UL dentro de outra UL criando assim 
                          uma arvore de tarefas.
                        - Este template está com display none para não ser apresentado na tela
                        --%>
                            <ul class="template" style="display: none">
                                <!-- -->
                                <li>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="chkCompleteTask" onclick="CompleteTask(this)" />
                                                &nbsp;
                                            </td>
                                            <td>
                                                <font runat="server" id="date"></font>
                                            </td>
                                            <td style="white-space: nowrap">
                                                <a class="inline" id="lnkTask"><font><$= Name$> </font></a>&nbsp;
                                            </td>
                                            <td style="width: 100px;">
                                                <div id="rating" runat="server" title="Classificação" class="inline">
                                                </div>
                                                &nbsp;
                                                <div href='javascript:;' id="shared" class='shared' title="Tarefa Compartilhada Aguardando">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                            </ul>
                            <ul id="tasks" source="~/Infocontrol/TaskService.svc" action="GetTasks" command="load"
                                options="{name:''}" template=".template">
                            </ul>
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                        <td valign="top" align="middle">
                            <asp:Button ID="btnNewTask" runat="server" Text="Inserir nova tarefa" OnClientClick="location='task.aspx'; return;"
                                UseSubmitBehavior="false" />
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
    <%--<VFX:BusinessManagerDataSource ID="odsTaskByUser" runat="server" TypeName="Vivina.Erp.BusinessRules.TaskManager"
        OldValuesParameterFormatString="original_{0}" onselecting="odsTaskByUser_Selecting"
        SelectMethod="GetTasksByUser">
        <selectparameters>
            <asp:Parameter Name="userId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" defaultValue="TaskStatusId, Deadline, FinishDate, Priority Desc" />
            <asp:Parameter Name="status" Type="Int32" />
            <asp:Parameter Name="filterType" Type="Object" />
            <asp:Parameter Name="name" Type="String" />
             <asp:Parameter Name="dtInterval" Type="Object" />
             <asp:Parameter Name="subjectId" Type="Int32" />
             <asp:Parameter Name="pageName" Type="String" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>--%>
    <VFX:BusinessManagerDataSource ID="odsTaskStatus" runat="server" SelectMethod="GetTaskStatus"
        TypeName="Vivina.Erp.BusinessRules.TaskManager">
    </VFX:BusinessManagerDataSource>
</asp:Content>
