<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="True" Inherits="InfoControl_Services_ServiceOrder" Title="Ordem de Serviço"
    CodeBehind="ServiceOrder.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtServiceOrderNumber" runat="server" MaxLength="50" Columns="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valtxtServiceOrderNumber" runat="server" ControlToValidate="txtServiceOrderNumber"
                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Literal runat="server" ID="lblCreatedDate"></asp:Literal>
            </td>
        </tr>
    </table>
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
                <asp:UpdatePanel ID="upCustomerDetail" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td style="width: 50%;">
                                    <table>
                                        <td>
                                            <td>
                                                <uc1:SelectCustomer ID="SelCustomer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="reqSelCustomer" runat="server" ControlToValidate="SelCustomer"
                                                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
                                            </td>
                                        </td>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                Tipo:<br />
                                                <asp:DropDownList ID="cboServiceOrderType" runat="server" DataSourceID="odsServicesOrderType"
                                                    DataTextField="Name" DataValueField="ServiceOrderTypeId" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqcboServiceOrderType" runat="server" ControlToValidate="cboServiceOrderType"
                                                    ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                Status:<br />
                                                <asp:DropDownList ID="cboServiceOrderStatus" runat="server" DataSourceID="odsServicesOrderStatus"
                                                    DataTextField="Name" DataValueField="ServiceOrderStatusId" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqcboServiceOrderStatus" runat="server" ControlToValidate="cboServiceOrderStatus"
                                                    ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
                                                <VFX:BusinessManagerDataSource ID="odsServicesOrderStatus" runat="server" SelectMethod="getAllServiceOrderStatus"
                                                    TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
                                                </VFX:BusinessManagerDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlCustomerCalls" runat="server">
                                                    Chamado:<br />
                                                    <asp:DropDownList ID="cboCustomerCalls" runat="server" AppendDataBoundItems="True"
                                                        DataTextField="CallNumber" DataValueField="CustomerCallId" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cboCustomerCalls_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlCustomerContracts" runat="server">
                                                    Contrato:<br />
                                                    <asp:DropDownList ID="cboCustomerContracts" runat="server" AppendDataBoundItems="True"
                                                        DataTextField="ContractNumber" DataValueField="ContractId" DataSourceID="odsCustomerContracts">
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="btnShowAppointments" runat="server" Text="Atuação Técnica" />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="updPanelEquipment" runat="server">
                                        <table>
                                            <tr>
                                                <td valign="bottom">
                                                    Equipamento:<br />
                                                    <asp:DropDownList ID="cboCustomerEquipments" runat="server" AppendDataBoundItems="true"
                                                        DataTextField="Name" DataValueField="CustomerEquipmentId" AutoPostBack="true"
                                                        OnTextChanged="cboCustomerEquipments_TextChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlShowEquipment" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEquipmentModelText" runat="server" Text="Modelo:" Visible="False"></asp:Label>
                                                                    <asp:Label ID="lblEquipmentModel" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEquipmentManufacturerText" runat="server" Text="Fabricante:" Visible="False"></asp:Label>
                                                                    <asp:Label ID="lblEquipmentManufacturer" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNumberText" runat="server" Text="Número de série:" Visible="False"></asp:Label>
                                                                    <asp:Label ID="lblSerialNumber" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEquipmentDescriptionText" runat="server" Text="Descrição:" Visible="False"></asp:Label>
                                                                    <asp:Label ID="lblEquipmentDescription" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="pnlOrderServiceItem" runat="server">
                    <fieldset>
                        <legend>Serviços:</legend>
                        <table class="noPrintable" width="100%">
                            <tr runat="server" id="rowService">
                                <td>
                                    Serviço:<br />
                                    <asp:DropDownList ID="cboService" runat="server" DataSourceID="odsServices" DataTextField="Name"
                                        DataValueField="ServiceId" AppendDataBoundItems="True">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="valService" runat="server" ControlToValidate="cboService"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertService"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;&nbsp;
                                    <VFX:BusinessManagerDataSource runat="server" ID="odsServices" SelectMethod="GetServices"
                                        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager" onselecting="dataSource_Selecting">
                                        <selectparameters>
                                                    <asp:Parameter Name="companyId" Type="Int32" />
                                                </selectparameters>
                                    </VFX:BusinessManagerDataSource>
                                </td>
                                <td>
                                    Técnico Responsável:<br />
                                    <asp:DropDownList ID="cboServiceEmployee" runat="server" DataSourceID="odsServicesOrderEmployee"
                                        DataTextField="Name" DataValueField="EmployeeId" AppendDataBoundItems="True">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqcboServiceEmployee" runat="server" ControlToValidate="cboServiceEmployee"
                                        ErrorMessage="&nbsp&nbsp&nbsp" ValidationGroup="InsertService"></asp:RequiredFieldValidator>
                                    <VFX:BusinessManagerDataSource runat="server" ID="odsServicesOrderEmployee" onselecting="dataSource_Selecting"
                                        SelectMethod="GetActiveTechnicalEmployee" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
                                        <selectparameters>
                                            <asp:Parameter Name="companyId" Type="Int32" />
                                        </selectparameters>
                                    </VFX:BusinessManagerDataSource>
                                </td>
                                <td>
                                    Preço:<br />
                                    <uc3:CurrencyField ID="ucCurrFieldServicePrice" Required="true" ValidationGroup="InsertService"
                                        runat="server" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnAddServiceItem" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                        runat="server" AlternateText="Inserir Serviço" OnClick="btnAddServiceItem_Click"
                                        ValidationGroup="InsertService" />
                                </td>
                            </tr>
                        </table>
                        <asp:UpdatePanel ID="upPnlService" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdService" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdService_RowDataBound" 
                                    OnRowDeleting="grdService_RowDeleting"
                                    ShowFooter="True" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Nome" />
                                        <asp:BoundField DataField="EmployeeName" HeaderText="Técnico" />
                                        <asp:BoundField DataField="TimeInMinutes" HeaderText="Tempo" />
                                        <asp:BoundField DataField="ServicePrice" DataFormatString="{0:f}" HeaderText="Preço">
                                        </asp:BoundField>                                         

                                        <asp:CommandField DeleteText="<span class='delete' title='excluir'> </span>"
                                            ShowDeleteButton="True"> 
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </fieldset>
                    <asp:UpdatePanel ID="updatePanelProduct" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Produtos:</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Despósito:<br />
                                            <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                                DataValueField="DepositId" AppendDataBoundItems="true">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <font color="red">
                                                <asp:Literal Visible="false" runat="server" ID="litErrorMessage" />
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="80%" class="noPrintable">
                                    <tr runat="server" id="rowProduct">
                                        <td>
                                            Produto:<br />
                                            <asp:TextBox ID="txtProduct" runat="server" Columns="40" CssClass="cDynDat11" plugin="autocomplete"
                                                source='~/InfoControl/SearchService.svc'                                                
                                                action='FindProductInInventory'
                                                options="{max: 10}">
                                                
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="valProduct" runat="server" ControlToValidate="txtProduct"
                                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertProduct"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Descrição<br />
                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="200"></asp:TextBox>
                                        </td>
                                        <td>
                                            Quantidade:<br />
                                            <uc3:CurrencyField ID="ucCurrFieldQuantity" Mask="9999" Required="true" ValidationGroup="InsertProduct"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:CheckBox ID="choProductIsApplied" runat="server" Text=" Foi aplicado ?" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnServiceOrderItemProduct" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                                runat="server" AlternateText="Inserir produto" OnClick="btnServiceOrderItemProduct_Click"
                                                ValidationGroup="InsertProduct" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdServiceOrderItemProduct" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnRowDeleting="grdServiceOrderItemProduct_RowDeleting" OnRowDataBound="grdServiceOrderItemProduct_RowDataBound"
                                     ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="ProductName" HeaderText="Produto" />
                                        <asp:BoundField DataField="Description" HeaderText="Descrição" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantidade" />
                                        <asp:BoundField DataField="ProductPrice" HeaderText="Preço">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:CommandField DeleteText="<span class='delete' title='excluir'> </span>"
                                            ShowDeleteButton="True">
                                            <ItemStyle Width="1%" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlServiceType" runat="server">
                    <fieldset>
                        <legend>Tipo de Serviço:</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkServiceType" runat="server" RepeatDirection="Horizontal"
                                        DataSourceID="odsServiceOrderType" DataTextField="Name" DataValueField="ServiceTypeId">
                                    </asp:CheckBoxList>
                                    <VFX:BusinessManagerDataSource ID="odsServiceOrderType" runat="server" SelectMethod="GetServiceTypes"
                                        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager" onselecting="dataSource_Selecting">
                                        <selectparameters>
                                            <asp:Parameter Name="companyId" Type="Int32" />
                                        </selectparameters>
                                    </VFX:BusinessManagerDataSource>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlProductType" runat="server">
                    <fieldset>
                        <legend>Tipo de produto :</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkProductType" runat="server" RepeatDirection="Horizontal"
                                        DataSourceID="odsServiceOrderProductsType" DataTextField="Name" DataValueField="ServiceOrderProductTypeId">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlInstallType" runat="server">
                    <fieldset>
                        <legend>Tipo de instalação :</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkInstallType" runat="server" RepeatDirection="Horizontal"
                                        DataSourceID="odsServiceOrderInstallTypes" DataTextField="Name" DataValueField="ServiceOrderInstallTypeId">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlHaltType" runat="server">
                    <fieldset>
                        <legend>Tipo de corte :</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkHaltType" runat="server" RepeatDirection="Horizontal" DataSourceID="odsServiceOrderHaltType"
                                        DataTextField="Name" DataValueField="ServiceOrderHaltTypeId">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlEquipmentDamage" runat="server">
                    <fieldset>
                        <legend>Defeitos de equipamentos:</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chklstEquipmentDamage" runat="server" RepeatDirection="Horizontal"
                                        DataSourceID="odsServiceOrderEquipmentDamage" DataTextField="Name" DataValueField="ServiceOrderEquipmentDamageId">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlProductDamage" runat="server">
                    <fieldset>
                        <legend>Defeitos de produtos:</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chklstProductDamage" runat="server" RepeatDirection="Horizontal"
                                        DataSourceID="odsServiceOrderProductDamage" DataTextField="Name" DataValueField="ServiceOrderProductDamageId">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlTestType" runat="server">
                    <fieldset>
                        <legend>Tipos de Teste:</legend>
                        <asp:CheckBoxList ID="chklstTests" runat="server" DataSourceID="odsServiceOrderTest"
                            DataTextField="Name" DataValueField="ServiceOrderTestId" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </fieldset>
                    <br />
                </asp:Panel>
                <uc2:Comments ID="ucComments" runat="server" />
                <table width="100%">
                    <tr runat="server" id="rowButtons">
                        <td align="right">
                            <asp:Button ID="btnGenerateReceipt" Visible="False" runat="server" Text="Gerar nota fiscal"
                                ValidationGroup="InsertServiceOrder" OnClick="btnGenerateReceipt_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" ValidationGroup="InsertServiceOrder" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <!-- Conteudo -->
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
    <VFX:BusinessManagerDataSource ID="odsCustomerContracts" runat="server" TypeName="Vivina.Erp.BusinessRules.ContractManager"
        OldValuesParameterFormatString="original_{0}" onselecting="odsCustomerContracts_Selecting"
        SelectMethod="RetrieveContractsByCustomer">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="customerId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderHaltType" runat="server" onselecting="dataSource_Selecting"
        SelectMethod="GetServiceOrderHaltTypes" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderInstallTypes" runat="server" onselecting="dataSource_Selecting"
        SelectMethod="GetServiceOrderInstallTypes" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderProductsType" runat="server" onselecting="dataSource_Selecting"
        SelectMethod="GetServiceOrderProductsType" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderProductDamage" runat="server" onselecting="dataSource_Selecting"
        SelectMethod="GetServiceOrderProductDamages" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderEquipmentDamage" runat="server"
        onselecting="dataSource_Selecting" SelectMethod="GetServiceOrderEquipmentDamages"
        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderTest" runat="server" onselecting="dataSource_Selecting"
        SelectMethod="GetServiceOrderTest" TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsServicesOrderType" SelectMethod="GetServiceOrderTypes"
        TypeName="Vivina.Erp.BusinessRules.Services.ServicesManager" OldValuesParameterFormatString="original_{0}"
        onselecting="dataSource_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" SelectMethod="GetDepositByCompany"
        TypeName="Vivina.Erp.BusinessRules.DepositManager" OnSelecting="dataSource_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsEquipments" runat="server" SelectMethod="GetCustomerEquipments"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" DataObjectTypeName="Vivina.Erp.DataClasses.CustomerEquipment"
        ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}"
        MaximumRowsParameterName="maximumRows" StartRowIndexParameterName="startRowIndex"
        onselecting="odsEquipments_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
