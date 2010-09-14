<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="True" Inherits="InfoControl_Services_ServiceOrder" Title="Ordem de Serviço"
    CodeBehind="~/ServiceOrder.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/SelectCustomer.ascx" TagName="SelectCustomer" TagPrefix="uc1" %>
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
                    <contenttemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 60%;">
                                                <uc1:SelectCustomer ID="SelCustomer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                                <asp:RequiredFieldValidator ID="reqSelCustomer" runat="server" ControlToValidate="SelCustomer"
                                                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
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
                                                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                            Status:<br />
                                                            <asp:DropDownList ID="cboServiceOrderStatus" runat="server" DataSourceID="odsServicesOrderStatus"
                                                                DataTextField="Name" DataValueField="ServiceOrderStatusId" AppendDataBoundItems="True">
                                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="reqcboServiceOrderStatus" runat="server" ControlToValidate="cboServiceOrderStatus"
                                                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertServiceOrder"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:Panel ID="pnlCustomerCalls" runat="server">
                                                                Chamado:<br />
                                                                <asp:DropDownList ID="cboCustomerCalls" runat="server" AppendDataBoundItems="True"
                                                                    DataTextField="CallNumber" DataValueField="CustomerCallId" AutoPostBack="True"
                                                                    OnTextChanged="cboCustomerEquipments_TextChanged">
                                                                </asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                          <asp:Panel ID="pnlCustomerContracts" runat="server">
                                                                Contrato:<br />
                                                                <asp:DropDownList ID="cboCustomerContracts" runat="server" AppendDataBoundItems="True"
                                                                    DataTextField="ContractNumber" DataValueField="ContractId" DataSourceID="odsCustomerContracts"
                                                                   >
                                                                   <asp:ListItem Text="" Value="" Selected></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="updPanelEquipment" runat="server">
                                                                <asp:UpdatePanel ID="updatePanelEquipment" runat="server">
                                                                    <ContentTemplate>
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
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                    </contenttemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Panel ID="pnlOrderServiceItem" runat="server">
                    <asp:UpdatePanel ID="upPnlService" runat="server">
                        <contenttemplate>
                            <fieldset>
                                <legend>Serviços:</legend>
                                <table width="100%">
                                    <tr class="noPrintable">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        Serviço:<br />
                                                        <asp:DropDownList ID="cboService" runat="server" DataSourceID="odsServices" DataTextField="Name"
                                                            DataValueField="ServiceId" AppendDataBoundItems="True">
                                                            <asp:ListItem></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="valService" runat="server" ControlToValidate="cboService"
                                                            ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertService"></asp:RequiredFieldValidator>
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        Técnico:<br />
                                                        <asp:DropDownList ID="cboServiceEmployee" runat="server" DataSourceID="odsServicesOrderEmployee"
                                                            DataTextField="Name" DataValueField="EmployeeId" AppendDataBoundItems="True">
                                                            <asp:ListItem></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="reqcboServiceEmployee" runat="server" ControlToValidate="cboServiceEmployee"
                                                            ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertService"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnService" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                                            runat="server" AlternateText="Inserir Serviço" OnClick="btnAddServiceItem_Click" ValidationGroup="InsertService" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:GridView ID="grdService" runat="server" AutoGenerateColumns="False" OnDataBound="grdService_DataBound"
                                                OnRowDataBound="grdService_RowDataBound" OnRowDeleting="grdService_RowDeleting"
                                                ShowFooter="True" Width="100%">
                                                <Columns>
                                                    <asp:BoundField DataField="Name" HeaderText="Nome" />
                                                    <asp:BoundField DataField="EmployeeName" HeaderText="Técnico" />
                                                    <asp:BoundField DataField="TimeInMinutes" HeaderText="Tempo" />
                                                    <asp:BoundField DataField="ServicePrice" DataFormatString="{0:f}" HeaderText="Preço">
                                                       
                                                    </asp:BoundField>
                                                    <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                                        ShowDeleteButton="True">
                                                        <ItemStyle Width="1%" />
                                                    </asp:CommandField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </contenttemplate>
                    </asp:UpdatePanel>
                    <br />
                    <%--                    <asp:UpdatePanel ID="pnlTechnical" runat="server">
                        <contenttemplate>--%>
                    <fieldset>
                        <legend>Atuação Técnica</legend>
                        <table class="noPrintable">
                            <tr>
                                <td>
                                    Hora Inicio:<br />
                                    <asp:TextBox ID="txtInitialDate" runat="server" Width="120px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="mskTxtInitialDate" runat="server" TargetControlID="txtInitialDate"
                                        CultureName="pt-BR" Mask="99/99/9999 99:99:99" MaskType="DateTime" CultureAMPMPlaceholder=""
                                        CultureCurrencySymbolPlaceholder="R$ " CultureDateFormat="DMY" CultureDatePlaceholder="/"
                                        CultureDecimalPlaceholder="," CultureThousandsPlaceholder="." CultureTimePlaceholder=":"
                                        Enabled="True">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ID="reqtxtInitialDate" runat="server" ControlToValidate="txtInitialDate"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="addTechnicalTime"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmpTxtInitialDate" runat="server" ControlToValidate="txtInitialDate"
                                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="GreaterThanEqual" ValueToCompare="1/1/1753"
                                        Type="Date" CssClass="cErr21"></asp:CompareValidator>
                                </td>
                                <td>
                                    Hora Fim:<br />
                                    <asp:TextBox ID="txtEndDate" runat="server" Width="120px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="msktxtEndDate" runat="server" TargetControlID="txtEndDate"
                                        CultureName="pt-BR" Mask="99/99/9999 99:99:99" MaskType="DateTime" CultureAMPMPlaceholder=""
                                        CultureCurrencySymbolPlaceholder="R$ " CultureDateFormat="DMY" CultureDatePlaceholder="/"
                                        CultureDecimalPlaceholder="," CultureThousandsPlaceholder="." CultureTimePlaceholder=":"
                                        Enabled="True">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ID="reqtxtEndDate" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="addTechnicalTime"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmptxtEndDate" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="GreaterThanEqual" ControlToCompare="txtInitialDate"
                                        CssClass="cErr21"></asp:CompareValidator>
                                </td>
                                <td>
                                    Técnico:<br />
                                    <asp:DropDownList ID="cboServiceOrderEmployee" runat="server" DataSourceID="odsServicesOrderEmployee"
                                        DataTextField="Name" DataValueField="EmployeeId" AppendDataBoundItems="True">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqcboServiceOrderEmployee" runat="server" ControlToValidate="cboServiceOrderEmployee"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="addTechnicalTime"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgAddTechnicalTime" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                        runat="server" AlternateText="Adicionar Técnico" ValidationGroup="addTechnicalTime"
                                        OnClick="imgAddTechnicalTime_Click" />
                                </td>
                            </tr>
                        </table>
                        <table class="noPrintable">
                            <tr>
                                <td>
                                    Descrição:<br />
                                    <asp:TextBox ID="txtTechnicalDescription" runat="server" MaxLength="300" TextMode="MultiLine"
                                        Columns="80" Rows="6"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqtxtTechnicalDescription" runat="server" ControlToValidate="txtTechnicalDescription"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="addTechnicalTime"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <asp:ListView ID="lstTechnicalAppointments" runat="server" DataKeyNames="CompanyId,AppointmentId,TaskName,BeginTime,EndTime"
                            OnItemDeleting="lstTechnicalAppointments_ItemDeleting">
                            <LayoutTemplate>
                                <fieldset runat="server" id="ItemPlaceHolder">
                                </fieldset>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <fieldset id="Fieldset2" runat="server">
                                    <legend>
                                        <%# Convert.ToDateTime(Eval("BeginTime")).ToString("dd/MM/yyyy HH:mm")%>
                                        &nbsp;&nbsp;&raquo;&nbsp;
                                        <%# Convert.ToDateTime(Eval("EndTime")).ToString("dd/MM/yyyy HH:mm")%>&nbsp;&nbsp;|&nbsp;
                                        <%# Eval("Employee.Profile.Name")%>
                                    </legend>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                Descrição:
                                                <%# Eval("TaskName")%>
                                            </td>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkDelete" ValidationGroup="None" CommandName="Delete" runat="server"
                                                    CssClass="delete" Text="">&nbsp;&nbsp;&nbsp;</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </fieldset>
                                <br />
                            </ItemTemplate>
                        </asp:ListView>
                    </fieldset>
                    <%--                      </contenttemplate>
                    </asp:UpdatePanel>--%>
                    <br />
                    <asp:UpdatePanel ID="updatePanelProduct" runat="server">
                        <contenttemplate>
                                        <fieldset>
                                            <legend>Produtos:</legend>
                                           
                                                        <table class="noPrintable">
                                                            <tr>
                                                                <td>
                                                                    Produto:<br />
                                                                    <asp:TextBox ID="txtProduct" runat="server" Columns="40" CssClass="cDynDat11"></asp:TextBox>
                                                                    <ajaxToolkit:AutoCompleteExtender ID="txtProductData_AutoCompleteExtender" runat="server"
                                                                        CompletionSetCount="5" MinimumPrefixLength="2" ServiceMethod="SearchProductInInventory"
                                                                        ServicePath="~/InfoControl/SearchService.asmx" TargetControlID="txtProduct" UseContextKey="True"
                                                                        BehaviorID="txtProductData_AutoCompleteExtender" DelimiterCharacters="" Enabled="True">
                                                                    </ajaxToolkit:AutoCompleteExtender>
                                                                    <asp:RequiredFieldValidator ID="valProduct" runat="server" ControlToValidate="txtProduct"
                                                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertProduct"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td>
                                                                    Serial:<br />
                                                                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="200"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:CheckBox ID="choProductIsApplied" runat="server" Text="Foi aplicado ?" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnServiceOrderItemProduct" ImageUrl="~/App_Themes/GlassCyan/Controls/GridView/img/Add2.gif"
                                                                        runat="server" AlternateText="Inserir produto" OnClick="btnServiceOrderItemProduct_Click"
                                                                        ValidationGroup="InsertProduct" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                   
                                                        <asp:GridView ID="grdServiceOrderItemProduct" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            OnRowDeleting="grdServiceOrderItemProduct_RowDeleting" OnDataBound="grdServiceOrderItemProduct_DataBound"
                                                            OnRowDataBound="grdServiceOrderItemProduct_RowDataBound" ShowFooter="True">
                                                            <Columns>
                                                                <asp:BoundField DataField="ProductName" HeaderText="Nome" />
                                                                <asp:BoundField DataField="Description" HeaderText="Descrição" />
                                                                <asp:BoundField DataField="ProductPrice" HeaderText="Preço">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                                                    ShowDeleteButton="True">
                                                                    <ItemStyle Width="1%" />
                                                                </asp:CommandField>
                                                            </Columns>
                                                        </asp:GridView>
                                                   
                                        </fieldset>
                                    </contenttemplate>
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
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chklstTests" runat="server" DataSourceID="odsServiceOrderTest"
                                        DataTextField="Name" DataValueField="ServiceOrderTestId" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblTechnicalDecision" runat="server" Text="Laudo Técnico:"></asp:Label><br />
                        <asp:TextBox ID="txtTechnicalDecision" runat="server" Width="100%" Rows="6" TextMode="MultiLine"></asp:TextBox>
                        <br />
                    </fieldset>
                    <br />
                </asp:Panel>
                <table width="100%">
                    <tr>
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
    <VFX:BusinessManagerDataSource ID="odsCustomerContracts" runat="server" TypeName="Vivina.InfoControl.BusinessRules.ContractManager"
        OldValuesParameterFormatString="original_{0}" onselecting="odsCustomerContracts_Selecting"
        SelectMethod="RetrieveContractsByCustomer">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="customerId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderType" runat="server" SelectMethod="GetServiceTypes"
        TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderHaltType" runat="server" onselecting="odsAdditional_Selecting"
        SelectMethod="GetServiceOrderHaltTypes" TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderInstallTypes" runat="server" onselecting="odsAdditional_Selecting"
        SelectMethod="GetServiceOrderInstallTypes" TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderProductsType" runat="server" onselecting="odsAdditional_Selecting"
        SelectMethod="GetServiceOrderProductsType" TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderProductDamage" runat="server" onselecting="odsAdditional_Selecting"
        SelectMethod="GetServiceOrderProductDamages" TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderEquipmentDamage" runat="server"
        onselecting="odsAdditional_Selecting" SelectMethod="GetServiceOrderEquipmentDamages"
        TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsServiceOrderTest" runat="server" onselecting="odsAdditional_Selecting"
        SelectMethod="GetServiceOrderTest" TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager"
        onselected="odsServiceOrderTest_Selected">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsTechnicalAppointments" runat="server" onselecting="odsTechnicalAppointments_Selecting"
        SelectMethod="GetAppointmentByServiceOrder" TypeName="Vivina.InfoControl.BusinessRules.AppointmentManager"
        DataObjectTypeName="Vivina.InfoControl.DataClasses.Appointment" DeleteMethod="DeleteAppointment">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="serviceOrderId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsServices" SelectMethod="GetServices"
        TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager" onselecting="odsServices_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsServicesOrderType" SelectMethod="GetAllServiceOrderTypes"
        TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsServicesOrderStatus" SelectMethod="getAllServiceOrderStatus"
        TypeName="Vivina.InfoControl.BusinessRules.Services.ServicesManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource runat="server" ID="odsServicesOrderEmployee" onselecting="odsServicesOrderEmployee_Selecting"
        SelectMethod="GetActiveTechnicalEmployee" TypeName="Vivina.InfoControl.BusinessRules.EmployeeManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsEquipments" runat="server" SelectMethod="GetCustomerEquipments"
        TypeName="Vivina.InfoControl.BusinessRules.CustomerManager" DataObjectTypeName="Vivina.InfoControl.DataClasses.CustomerEquipment"
        ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}"
        MaximumRowsParameterName="maximumRows" StartRowIndexParameterName="startRowIndex"
        onselecting="odsEquipments_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
