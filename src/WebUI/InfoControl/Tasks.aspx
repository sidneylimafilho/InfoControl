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
                    <div class="body" form="true" trigger="#tasks">
                        Modo de Exibição:<br />
                        <table>
                            <tr>
                                <td colspan="9" style="border-bottom: 1px solid #099">
                                    <input type="radio" value="2" name="view" field="view" checked="checked" /><label>Hierarquia</label>
                                    <input type="radio" value="1" name="view" field="view" /><label>Data</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Parte do nome:
                                    <br />
                                    <input type="text" name="name" />
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                Início:<br />
                                                <input type="text" name="inicio" columns="8" maxlength="10" plugin="calendar" mask="99/99/9999"
                                                    options="{relatedCalendar:'#txtEndDate'}" />
                                            </td>
                                            <td>
                                                Fim:<br />
                                                <input type="text" name="fim" columns="8" maxlength="10" plugin="calendar" mask="99/99/9999" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <input type="radio" value="" name="status" /><label>Todos</label>
                                    <input type="radio" value="1" name="status" checked="checked" /><label>Abertas</label>
                                    <input type="radio" value="3" name="status" /><label>Completas</label>
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
                        <td source="~/Infocontrol/TaskService.svc">
                            <%-- 
                        - Este template separado pois será usado recursivamente sendo colocado uma UL dentro de outra UL criando assim 
                          uma arvore de tarefas.
                        - Este template está com display none para não ser apresentado na tela
                        --%>
                            <ul class="template" style="display: none">
                                <!-- 
                                <li id="task_<$=TaskId$>">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:30px">
                                                <$ if(!HasChildTasks){ $>
                                                <input type="checkbox" command="click" action="CompleteTask" options="{companyId:<%=Company.CompanyId%>, taskId:<$=TaskId$>, userId:<%=User.Identity.UserId%>}" onsucess="$('#task_<$=TaskId$>').hide()" />
                                                &nbsp; 
                                                <$}else{$> 
                                                <a href="javascript:;" command="click"  trigger="#task_<$=TaskId$> > ul">+++</a> <$}$>
                                            </td>
                                            <td>
                                                <font id="date"><$=$.format((FinishDate||"").JsonToDate(), "d")$></font>
                                            </td>
                                            <td style="white-space: nowrap; font-weight:<$=HasChildTasks?"bold":"normal"$>">
                                                <a href="task.aspx?taskid=<$=TaskId$>" class="inline" id="lnkTask"><font><$=Name$> </font></a>&nbsp;
                                            </td>
                                            <td style="width: 100px;">
                                                <div id="rating" title="Classificação" class="inline">
                                                    <$ for (var i=0; i < Priority; i++){$> <span class='ratingStar emptyRatingStar' style='float: left;'>
                                                        &nbsp;</span> <$}$>
                                                </div>
                                                &nbsp;
                                                <div href='javascript:;' id="shared" class='shared' title="Tarefa Compartilhada Aguardando">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <ul action="GetTasks" options="{parentId:<$=TaskId$> }"
                                        template=".template">
                                    </ul>
                                </li>-->
                            </ul>
                            <ul id="tasks" action="GetTasks" command="load" options="{name:'', view:2, status:1}"
                                template=".template">
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
