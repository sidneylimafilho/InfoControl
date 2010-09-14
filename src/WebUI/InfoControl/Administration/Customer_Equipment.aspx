<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Administration_Customer_Equipment"
    CodeBehind="Customer_Equipment.aspx.cs" Title="" MasterPageFile="~/infocontrol/Default.master" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <asp:FormView ID="frmEquipment" runat="server" DataSourceID="odsEquipments" DefaultMode="Insert"
                    Width="100%" DataKeyNames="CustomerEquipmentId,Name,CustomerId,CompanyId,Model,Manufacturer,SerialNumber,Description,Comments,Color,Patrimonio,
                FactoringYear,ContractId,WarrantyBeginDate,WarrantyEndDate,IdentificationOrPlaca,ModelYear"
                    OnItemCommand="frmEquipment_ItemCommand" OnItemInserting="frmEquipment_ItemInserting"
                    OnItemUpdating="frmEquipment_ItemUpdating">
                    <EditItemTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    Nome do Equipamento/Carro:<br />
                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' Columns="50" 
                                        MaxLength="100" ValidationGroup="InsertEquipment"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertEquipment"
                                        Width="16px"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Modelo:<br />
                                    <asp:TextBox ID="txtModel" runat="server" Text='<%# Bind("Model") %>' Columns="30"
                                        MaxLength="50" ValidationGroup="InsertEquipment"></asp:TextBox>
                                </td>
                                <td>
                                    Ano do Modelo:<br />
                                    <asp:TextBox ID="txtModelYear" Text='<%# Bind("ModelYear") %>' runat="server" Columns="8"
                                        MaxLength="4" ValidationGroup="InsertEquipment"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="mskModelYear" runat="server" CultureName="pt-BR"
                                        Mask="9999" MaskType="Number" TargetControlID="txtModelYear">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Descrição:<br />
                                    <asp:TextBox ID="txtDescription" Text='<%# Bind("Description") %>' runat="server"
                                        Columns="40" MaxLength="50" ValidationGroup="InsertEquipment"></asp:TextBox>
                                </td>
                                <td>
                                    Número serial/Chassi:<br />
                                    <asp:TextBox ID="txtSerialNumber" Text='<%# Bind("SerialNumber") %>' runat="server"
                                        Columns="30" MaxLength="50" ValidationGroup="InsertEquipment"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqtxtSerialNumber" runat="server" ControlToValidate="txtSerialNumber"
                                        ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertEquipment"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Fabricante:<br />
                                    <asp:TextBox ID="txtManufacturer" Text='<%# Bind("Manufacturer") %>' runat="server"
                                        Columns="30" MaxLength="50" ValidationGroup="InsertEquipment"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Patrimônio/Renavam:<br />
                                    <asp:TextBox ID="txtPatrimonio" Text='<%# Bind("Patrimonio") %>' runat="server" Columns="30"
                                        MaxLength="50" ValidationGroup="InsertEquipment"></asp:TextBox>
                                </td>
                                <td>
                                    Ano de fabricação:<br />
                                    <asp:TextBox ID="txtFactorinYear" Text='<%# Bind("FactoringYear") %>' runat="server"
                                        MaxLength="4" Columns="8" ValidationGroup="InsertEquipment"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFactorinYear_MaskedEditExtender" runat="server"
                                        CultureName="pt-BR" Mask="9999" MaskType="Number" TargetControlID="txtFactorinYear">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                                <td>
                                    Cor:<br />
                                    <asp:TextBox ID="txtColor" Text='<%# Bind("Color") %>' runat="server" Columns="30"
                                        MaxLength="50" ValidationGroup="InsertEquipment"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                                Início da garantia:
                                                <br />
                                                <uc2:Date ID="ucWarrantyBeginDate" runat="server" ValidationGroup="InsertEquipment" Text='<%# Bind("WarrantyBeginDate") %>' />
                                             
                                  </td>
                                            
                               <td>
                                    
                                                Fim da garantia:
                                                <br />
                                                <uc2:Date ID="ucWarrantyEndDate" runat="server" ValidationGroup="InsertEquipment" Text='<%# Bind("WarrantyEndDate") %>' />
                                           
                                </td>
                               <td>
                                    Contrato:<br />
                                    <asp:DropDownList ID="cboContract" Width="150px" runat="server" AppendDataBoundItems="True"
                                        SelectedValue='<%#Bind("ContractId") %>' DataSourceID="odsContracts" DataTextField="ContractNumber"
                                        DataValueField="ContractId">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                             
                             </tr>
                        
                            <tr>
                                <td>
                                    Identificação/Placa:<br />
                                    <asp:TextBox MaxLength="18" ID="txtIdentificationOrPlaca" Columns="30" Text='<%# Bind("IdentificationOrPlaca") %>'
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    Observações:<br />
                                    <asp:TextBox ID="txtComments" runat="server" Text='<%# Bind("Comments") %>' Columns="100"
                                        MaxLength="200" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <br />
                                    <fieldset>
                                        <legend>Ordem de serviço:</legend>
                                        <asp:GridView ID="grdServiceOrder" runat="server" AutoGenerateColumns="False" Width="100%"
                                            DataSourceID="odsServiceOrder" DataKeyNames="ServiceOrderId" OnRowDataBound="grdServiceOrder_RowDataBound"
                                            OnSelectedIndexChanged="grdServiceOrder_SelectedIndexChanged" AllowPaging="true">
                                            <Columns>
                                                <asp:BoundField DataField="ServiceOrderNumber" HeaderText="Numero de ordem" SortExpression="ServiceOrderNumber" />
                                                <asp:BoundField DataField="ServiceOrderStatus" HeaderText="Status" SortExpression="ServiceOrderStatus" />
                                                <asp:BoundField DataField="ServiceOrderType" HeaderText="Tipo" SortExpression="ServiceOrderType" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div style="text-align: center">
                                                    Não existem dados a serem exibidos<br />
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div style="text-align: right">
                                        <br />
                                        <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                                            CssClass="cBtn11" Text="Inserir" Visible="<%# frmEquipment.CurrentMode == FormViewMode.Insert %>"
                                            permissionRequired="Customers" ValidationGroup="InsertEquipment"></asp:Button> 
                                        <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                                            CssClass="cBtn11" Text="Salvar" ValidationGroup="InsertEquipment" Visible="<%# frmEquipment.CurrentMode == FormViewMode.Edit %>"
                                            permissionRequired="Customers"></asp:Button>
                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="cBtn11" Text="Cancelar"></asp:Button></div>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </asp:FormView>
                <asp:GridView ID="grdEquipments" runat="server" RowSelectable="false" AutoGenerateColumns="False"
                    DataSourceID="odsEquipments" Width="100%" DataKeyNames="CustomerEquipmentId,Name,CustomerId,CompanyId,Model,Manufacturer,SerialNumber,Description,Comments,Color,Patrimonio,FactoringYear,ContractId,WarrantyBeginDate,WarrantyEndDate,ModelYear,IdentificationOrPlaca"
                    OnRowDataBound="grdEquipments_RowDataBound1" OnSelectedIndexChanged="grdEquipments_SelectedIndexChanged1"
                    OnSorting="grdEquipments_Sorting" AllowSorting="True">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"></asp:BoundField>
                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model"></asp:BoundField>
                        <asp:BoundField DataField="Manufacturer" HeaderText="Fabricante" SortExpression="Manufacturer">
                        </asp:BoundField>
                        <asp:BoundField DataField="SerialNumber" HeaderText="Número serial" SortExpression="SerialNumber">
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Descrição" SortExpression="Description">
                        </asp:BoundField>
                        <asp:CommandField DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                            HeaderText="&lt;div class=&quot;insert&quot;title=&quot;inserir&quot; &lt;/div&gt;"
                            ShowDeleteButton="True" SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                        </asp:CommandField>
                        
                       
                        
                        
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem equipamentos cadastrados ...
                            <br />
                            <br />
                            <asp:Button ID="btnAddCustomerEquipment" runat="server" Text="Inserir equipamento"
                                OnClick="btnAddCustomerEquipment_Click" />
                            <br />
                            <br />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
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
    <VFX:BusinessManagerDataSource ID="odsServiceOrder" runat="server" OnSelecting="odsServiceOrder_Selecting"
        SelectMethod="GetServiceOrdersByCustomerEquipment" TypeName="Vivina.Erp.BusinessRules.CustomerManager"
        SelectCountMethod="GetServiceOrdersByCustomerEquipmentCount">
        <selectparameters>
        <asp:Parameter Name="customerEquipmentId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsEquipments" runat="server" SelectMethod="GetCustomerEquipments"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" OnSelecting="odsEquipments_Selecting"
        DataObjectTypeName="Vivina.Erp.DataClasses.CustomerEquipment" DeleteMethod="DeleteCustomerEquipment"
        ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}"
        InsertMethod="InsertCustomerEquipment" OnInserted="odsEquipments_Inserted" OnInserting="odsEquipments_Inserting"
        OnUpdated="odsEquipments_Updated" UpdateMethod="UpdateCustomerEquipment" MaximumRowsParameterName="maximumRows"
        StartRowIndexParameterName="startRowIndex" SortParameterName="sortExpression"
        OnDeleted="odsEquipments_Deleted">
        <updateparameters>
        <asp:Parameter Name="original_entity" Type="Object"></asp:Parameter>
        <asp:Parameter Name="entity" Type="Object"></asp:Parameter>
    </updateparameters>
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="sortExpression" Type="String"></asp:Parameter>
        <asp:Parameter Name="maximumRows" Type="Int32"></asp:Parameter>
        <asp:Parameter Name="startRowIndex" Type="Int32"></asp:Parameter>
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContracts" runat="server" SelectMethod="GetContractsByCustomer"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" OnSelecting="odsContracts_Selecting">
        <selectparameters>
        <asp:Parameter Name="customerId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
