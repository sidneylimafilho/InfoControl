<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Project_Tasks" Title="Tarefas" CodeBehind="Tasks.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<asp:Content ID="Content" ContentPlaceHolderID="Header" runat="server">
    <style>
        #tasks ul, #tasks li
        {
            list-style: none;
        }
        #tasks .plus, #tasks .minus, #tasks .line
        {
            width: 22px;
        }
        #tasks .plus
        {
            background: url(../App_Shared/themes/glasscyan/Controls/treeview/plus.gif) no-repeat;
        }
        #tasks .minus
        {
            background: url(../App_Shared/themes/glasscyan/Controls/treeview/minus.gif) no-repeat;
        }
        #tasks td
        {
            padding: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%" source="~/Infocontrol/TaskService.svc">
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
                <fieldset id="filter" class="closed" asform="true" smart='{
                    load:{
                        source: "~/Infocontrol/TaskService.svc/GetTasks",
                        template: ".template",
                        target: "#tasks",
                        onresponse: function(responseBody){
                            return responseBody.Data;
                        }
                    }
                }'>
                    <legend onmouseover='setTimeout("$(\"#filter .body\").show(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"open\"})", 300);'>
                        Escolha o filtro desejado: </legend>
                    <div class="body">
                        Modo de Exibição:<br />
                        <table width="100%">
                            <tr>
                                <td>
                                    <input type="radio" value="1" name="view" field="view" checked="checked" onclick="$('#otherFilters').fadeIn('slow')" /><label>Data</label>
                                </td>
                                <td>
                                    <table id="otherFilters">
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
                                                            <input type="text" name="inicio" columns="8" maxlength="10" plugin="datepicker" mask="99/99/9999"
                                                                options="{relatedCalendar:'#txtEndDate'}" />
                                                        </td>
                                                        <td>
                                                            Fim:<br />
                                                            <input type="text" name="fim" columns="8" maxlength="10" plugin="datepicker" mask="99/99/9999" />
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
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9" style="border-top: 1px solid #099">
                                    <input type="radio" value="2" name="view" field="view" onclick="$('#otherFilters').hide('slow')" /><label>Hierarquia</label>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btSearch" smart='{click:{trigger:"#filter"}}' runat="server" Text="Pesquisar" OnClientClick="$('#filter').toggleClass('closed').find('.body').hide('slow');return false;" />
                    </div>
                    <span class="closeButton" onmouseover="$('#filter').toggleClass('closed').find('.body').hide('slow');">
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
                                <li id="task_<$=TaskId$>">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class='<$=HasChildTasks?"plus":"line"$>' smart="{
                                                click:{
                                                    target: '#task_<$=TaskId$> > ul',
                                                    trigger: '#filter',
                                                    sourceparams: {parentId:<$=TaskId$>},
                                                    show: '#task_<$=TaskId$> > ul',
                                                    onbounded:function(){
                                                        this.attr({className:'minus'});
                                                    }
                                                }
                                            }">
                                            </td>
                                            <td>
                                                <input type="checkbox" smart="{
                                                    click:{
                                                        source: '~/Infocontrol/TaskService.svc/CompleteTask',
                                                        sourceparams: {
                                                            companyId: <%=Company.CompanyId%>, 
                                                            taskId: <$=TaskId$>, 
                                                            userId: <%=User.Identity.UserId%>
                                                        },
                                                        hide: '#task_<$=TaskId$>',
                                                        onbinding: function(){
                                                            return confirm('Esta tarefa está realmente COMPLETA?');
                                                        }
                                                    }
                                                }" />
                                                &nbsp; <font id="date"><$=$.format((FinishDate||Deadline||"").JsonToDate(), "d")$></font>
                                            </td>
                                            <td style='white-space: nowrap; font-weight: <$=HasChildTasks?"bold":"normal"$>'>
                                                <a href="task.aspx?taskid=<$=TaskId$>" class="inline" id="lnkTask"><font><$=Name$> </font>
                                                </a>&nbsp;
                                            </td>
                                            <td style="width: 100px;">
                                                <div id="rating" title="Classificação" class="inline">
                                                    <$for (var i=0; i < Priority; i++) {$> 
                                                        <span class='ratingStar emptyRatingStar' style='float: left;'>&nbsp;</span> 
                                                    <$}$>
                                                </div>
                                                &nbsp;
                                                <div href='javascript:;' id="shared" class='shared' title="Tarefa Compartilhada Aguardando">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <ul style="display: none">
                                        <li>Carregando...</li>
                                    </ul>
                                </li>
                            </ul>
                            <ul id="tasks">
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
</asp:Content>
