<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Services_ServiceOrders" Title="Ordem de Serviço"
    CodeBehind="ServiceOrders.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                <td class="left">
                    &nbsp;
                </td>
                <td class="center">
                    <!--Principal -->
                    <fieldset id="filter" class="closed">
                        <legend onmouseover='setTimeout("$(\"#filter .body\").show(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"open\"})", 300);'>
                            Escolha o filtro desejado: </legend>
                        <div class="body">
                            <table>
                                <tr>
                                    <td>
                                    Cliente: <br />
                                        <asp:TextBox runat="server" ID="txtCustomer" /> 
                                    </td>
                                    <td>
                                        Status:<br />
                                        <asp:DropDownList ID="cboStatus" runat="server" AppendDataBoundItems="true" DataSourceID="odsServiceOrdersStatus"
                                            DataTextField="Name" DataValueField="ServiceOrderStatusId">
                                            <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Tipo:<br />
                                        <asp:DropDownList ID="cboServiceOrderType" runat="server" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="ServiceOrderTypeId" DataSourceID="odsServiceOrdersType">
                                            <asp:ListItem Value="0" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Nível Organizacional:<br />
                                        <uc1:ComboTreeBox ID="cboOrganizationLevelTree" runat="server" DataSourceID="odsOrganizationLevel"
                                            DataTextField="Name" DataFieldID="OrganizationLevelId" DataFieldParentID="ParentId"
                                            DataValueField="OrganizationLevelId" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="left">
                                        Técnico:<br />
                                        <asp:DropDownList ID="cboTechnicalUser" runat="server" DataSourceID="odsTechnicalEmployee"
                                            DataTextField="Name" DataValueField="EmployeeId" AppendDataBoundItems="true"
                                            ValidationGroup="grpFilter">
                                              <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Competência:
                                        <br />
                                        <asp:DropDownList runat="server" AppendDataBoundItems="true" DataTextField="CompetencyName"
                                            DataSourceID="odsCompetency" ID="cboCompetency">
                                            <asp:ListItem Text="" Value=""> </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <uc2:DateTimeInterval ID="ucDateTimeInterval" ValidationGroup="grpFilter" Required="true"
                                            runat="server" />
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
                        </div>
                        <span class="closeButton" onmouseover='setTimeout("$(\"#filter .body\").hide(1000);", 0); setTimeout("$(\"#filter\").attr({className:\"closed\"})", 950);'>
                            &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <telerik:RadGrid ID="grdServiceOrders" DataSourceID="odsServiceOrders" OnRowDataBound="grdServiceOrders_RowDataBound"
                        AllowPaging="true" RowSelectable="false" AllowSorting="true" AutoGenerateColumns="false"
                        runat="server" OnItemDataBound="grdServiceOrders_ItemDataBound" AllowAutomaticDeletes="false"
                        OnItemCommand="grdServiceOrders_ItemCommand">
                        <MasterTableView DataKeyNames="ServiceOrderId,CustomerId,customerName,CompanyId,ServiceOrderNumber,CustomerCallId,ServiceOrderTypeId,ServiceOrderStatusId,CustomerEquipmentId,OpenedDate,ClosedDate,ReceiptId,TechnicalDecision,PhysicalServiceOrder,PhysicalServiceOrderName,ServiceOrderTestId,ServiceOrderEquipmentDamageId,ServiceOrderProductDamageId,ServiceType,ServiceOrderProductType,ServiceOrderHaltType,ServiceOrderInstallType"
                            NoMasterRecordsText="&lt;div style=&quot;text-align: center&quot;&gt; Não existem dados a serem exibidos.&lt;br /&gt;&lt;/div&gt;">
                            <Columns>
                                <telerik:GridBoundColumn DataField="ServiceOrderNumber" HeaderText="Número" SortExpression="ServiceOrderNumber" />
                                <telerik:GridBoundColumn DataField="customerName" HeaderText="Cliente" SortExpression="customerName" />
                                <telerik:GridBoundColumn DataField="type" HeaderText="Tipo" SortExpression="type" />
                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" SortExpression="status" />
                                <telerik:GridButtonColumn HeaderText="<a href='ServiceOrder.aspx' class='insert'>  &nbsp;&nbsp;&nbsp;&nbsp;</a>"
                                    SortExpression="Insert" CommandName="Delete" Text="Excluir" UniqueName="DeleteColumn">
                                    <itemstyle horizontalalign="Center" />
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="False" AllowDragToGroup="True">
                            <Selecting AllowRowSelect="True" />
                            <ClientEvents OnRowMouseOver="RowMouseOver" OnRowMouseOut="RowMouseOut" />
                        </ClientSettings>
                    </telerik:RadGrid>
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
    <VFX:BusinessManagerDataSource ID="odsServiceOrders" runat="server" ConflictDetection="CompareAllValues"
        OldValuesParameterFormatString="original_{0}" SortParameterName="sortExpression"
        SelectMethod="GetServiceOrders" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager"
        DeleteMethod="DeleteServiceOrder" SelectCountMethod="GetServiceOrdersCount" onselecting="odsServiceOrders_Selecting"
        ondeleted="odsServiceOrders_Deleted" ondeleting="odsServiceOrders_Deleting" EnablePaging="True"
        EnableViewState="False">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="customerName" Type="String" />
            <asp:Parameter Name="serviceOrderStatusId" Type="Int32" />
            <asp:Parameter Name="serviceOrderTypeId" Type="Int32" />
            <asp:Parameter Name="organizationLevelId" Type="Int32" />
            <asp:Parameter Name="employeeId" Type="Int32" />
            <asp:Parameter Name="competency" Type="String" />
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            <asp:parameter Name="sortExpression" Type="String" />
			<asp:parameter Name="startRowIndex" Type="Int32" />
			<asp:parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>

    <VFX:BusinessManagerDataSource ID="odsServiceOrdersStatus" runat="server" SelectMethod="getAllServiceOrderStatus"
        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrdersType" runat="server" SelectMethod="GetAllServiceOrderTypes"
        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
    </VFX:BusinessManagerDataSource>

    <VFX:BusinessManagerDataSource ID="odsTechnicalEmployee" runat="server" SelectMethod="GetTechnicalEmployeeAsDataTable"
        onselecting="dataSource_Selecting" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsOrganizationLevel" runat="server" SelectMethod="GetAllOrganizationLevelToDataTable"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="dataSource_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCompetency" runat="server" onselecting="dataSource_Selecting"
        SelectMethod="GetEmployeeCompetenciesByCompany" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <script>
        function RowMouseOver(sender, eventArgs) { $get(eventArgs.get_id()).className += " RadGrid_Items_Hover"; }
        function RowMouseOut(sender, eventArgs) { $get(eventArgs.get_id()).className = "Item"; }
    </script>
</asp:Content>
