<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" Inherits="InfoControl_Host_ScheduleTasks" 
    EnableEventValidation="false" Title="Tarefas Agendadas" Codebehind="ScheduleTasks.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    
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
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdScheduleTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="ScheduledTaskId,Name,StartTime,Period,Enabled,TypeFullName"
                                    DataSourceID="odsScheduleTasks" Width="100%" AllowSorting="True" OnRowDataBound="grdScheduleTasks_RowDataBound"
                                    OnRowDeleted="grdScheduleTasks_RowDeleted" OnSorting="grdScheduleTasks_Sorting" 
                                    AllowPaging="True" PageSize="20" RowSelectable="false">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name" />
                                        <asp:BoundField DataField="StartTime" HeaderText="Início" SortExpression="StartTime" />
                                        <asp:BoundField DataField="Period" HeaderText="Período" SortExpression="Period" />
                                        <asp:CheckBoxField DataField="Enabled" HeaderText="Habilitada" ReadOnly="True" SortExpression="Enabled">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:CheckBoxField>
                                        <asp:BoundField DataField="TypeFullName" HeaderText="Type Full Name" SortExpression="TypeFullName" />
                                        <asp:BoundField DataField="LastRunStatus" HeaderText="Último Status" SortExpression="LastRunStatus" />
                                        <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                            HeaderText="&lt;a href=&quot;ScheduleTask.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                                            SortExpression="Insert">
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="text-align: center">
                                            Não existem dados a serem exibidos, clique no botão para cadastrar uma tarefa.<br />
                                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
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
        <VFX:BusinessManagerDataSource ID="odsScheduleTasks" runat="server" SelectMethod="GetAllScheduleTasks"
            TypeName="InfoControl.Web.ScheduledTasks.SchedulerManager" ConflictDetection="CompareAllValues"
            DataObjectTypeName="InfoControl.Web.ScheduledTasks.ScheduledTask" DeleteMethod="Delete"
            OldValuesParameterFormatString="original_{0}" SortParameterName="sortExpression">
            <selectparameters>
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
        </VFX:BusinessManagerDataSource>
    </div>
</asp:Content>
