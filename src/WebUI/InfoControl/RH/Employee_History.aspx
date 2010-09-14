<%@ Page AutoEventWireup="true" Inherits="Company_RH_Employee_History" CodeBehind="Employee_History.aspx.cs"
    Language="C#" MasterPageFile="~/InfoControl/Default.master" EnableEventValidation="false" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolder">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
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
                <table width="100%" cellspacing="10px">
                    <tr>
                        <td valign="top">
                            <h3>
                                Status de Atividade:</h3>
                            <asp:GridView ID="grdEmployeeStatusHistory" runat="server" Width="100%" AutoGenerateColumns="False"
                                DataSourceID="odsStatusHistory" DataKeyNames="StatusHistoryId,AlienationName"
                                OnRowDataBound="employeeHistory_RowDataBound" RowSelectable="false" OnRowDeleting="grdEmployeeStatusHistory_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("AlienationId") == null ? "Ativo" : "Afastado" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AlienationName" HeaderText="Motivo do Afastamento"></asp:BoundField>
                                    <%-- <asp:BoundField DataField="AlienationDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data do Afastamento"></asp:BoundField>--%>
                                    <asp:BoundField DataField="ModifiedDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data de Modificação">
                                    </asp:BoundField>
                                    <asp:CommandField DeleteText="<span class='delete' title='excluir'> </span>"
                                        ShowDeleteButton="True"> 
                                        <ItemStyle Width="1%" />
                                    </asp:CommandField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <p align="center">
                                        Não há dados à serem exibidos</p>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Status de Funções:</h3>
                            <asp:GridView ID="grdEmployeeFunctionHistories" runat="server" Width="100%" AutoGenerateColumns="False"
                                DataSourceID="odsEmployeeFunctionHistories" DataKeyNames="EmployeeFunctionHistoryId,EmployeeFunctionId"
                                OnRowDataBound="employeeHistory_RowDataBound" RowSelectable="false">
                                <Columns>
                                    <asp:BoundField DataField="EmployeeFunctionName" HeaderText="Função" />
                                    <asp:BoundField DataField="ModifedDate" HeaderText="Data de Modificação" />
                                    <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                        ShowDeleteButton="True">
                                        <ItemStyle Width="1%" />
                                    </asp:CommandField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <p align="center">
                                        Não há dados à serem exibidos</p>
                                </EmptyDataTemplate>
                            </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsStatusHistory" runat="server" SelectMethod="GetEmployeeStatusHistories"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" OnSelecting="odsHistory_Selecting"
        DataObjectTypeName="Vivina.Erp.DataClasses.StatusHistory" DeleteMethod="DeleteStatusHistory">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="employeeId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsEmployeeFunctionHistories" runat="server" SelectMethod="GetEmployeeFunctionHistories"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" OnSelecting="odsHistory_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="employeeId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
