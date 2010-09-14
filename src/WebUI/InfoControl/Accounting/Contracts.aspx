<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Administration_Contracts" Title="Contratos"
    CodeBehind="Contracts.aspx.cs" %>

<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
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
                <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                    <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                        Escolha o filtro desejado: </legend>
                    <div class="body">
                        <table width="70%">
                            <tr>
                                <td>
                                    <uc2:SelectCustomer ID="sel_customer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer"></uc2:SelectCustomer>
                                </td>
                                <td>
                                    Tipo:<br />
                                    <asp:DropDownList ID="cboContractType" runat="server" AppendDataBoundItems="true"
                                        DataSourceID="odsContractType" DataTextField="Name" DataValueField="ContractTypeId">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqcboContractType" runat="server" ControlToValidate="cboContractType"
                                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Status:<br />
                                    <asp:DropDownList ID="cboContractStatus" runat="server" AppendDataBoundItems="true"
                                        DataSourceID="odsContractStatus" DataTextField="Name" DataValueField="ContractStatusId">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqcboContractStatus" runat="server" ControlToValidate="cboContractStatus"
                                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                                </td>
                                <td align="right">
                                    Exibir:
                                    <br />
                                    <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                        <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Período:</b>
                                    <br />
                                    <uc1:DateTimeInterval ID="ucDurationIntervalDate" ValidationGroup="SearchContract"
                                        Required="true" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <div align="right">
                            <asp:Button ID="btnSearchContract" ValidationGroup="SearchContract" runat="server"
                                Text="Pesquisar" OnClick="btnSearchContract_Click" />
                        </div>
                        <br />
                        <br />
                    </div>
                    <span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                        &nbsp;</span>
                </fieldset>
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:GridView ID="grdContracts" runat="server" AutoGenerateColumns="False" DataSourceID="odsContracts"
                    Width="100%" AllowPaging="True" AllowSorting="True" DataKeyNames="ContractId,CompanyId,CustomerId,BeginDate,HH,ExpiresDate,InterestDeferredPayment,Penalty,ContractValue,AdditionalValue1,AdditionalValue2,AdditionalValue3,AdditionalValue4,AdditionalValue5,InvoiceId,Observation,ContractTypeId,ContractStatusId,Parcels,FirstParcelDueDate,Periodicity,ContractNumber,FinancierConditionId,FinancierOperationId,ContractType"
                    OnRowDataBound="grdContracts_RowDataBound" PageSize="20" RowSelectable="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Cliente" SortExpression="CustomerName">
                            <ItemTemplate>
                                <%# Eval("CustomerName")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContractType" HeaderText="Tipo" />
                        <asp:BoundField DataField="BeginDate" HeaderText="Data de Início" SortExpression="BeginDate"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ExpiresDate" HeaderText="Data de Término" SortExpression="ExpiresDate"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="&lt;a href=&quot;Contract.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" id='<%# Eval("ContractId") %>' companyid='<%# Eval("CompanyId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <center>
                            Não existem contratos cadastrados...
                            <br />
                            <br />
                            <asp:Button ID="btnInsertContract" runat="server" Text="Inserir Contrato" OnClientClick="location='Contract.aspx'; return false;" />
                        </center>
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
    <VFX:BusinessManagerDataSource ID="odsContractType" runat="server" SelectMethod="GetContractTypes"
        TypeName="Vivina.Erp.BusinessRules.ContractManager" OnSelecting="odsContractType_Selecting">
        <selectparameters>

           <asp:Parameter Name="companyId" Type="Int32" />

           </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContractStatus" runat="server" SelectMethod="GetAllContractStatus"
        TypeName="Vivina.Erp.BusinessRules.ContractManager" OnSelecting="odsContractStatus_Selecting">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContracts" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetContracts" TypeName="Vivina.Erp.BusinessRules.ContractManager"
        ConflictDetection="CompareAllValues" OnSelecting="odsContracts_Selecting" MaximumRowsParameterName="MaximumRows"
        EnablePaging="True" SelectCountMethod="GetContractsCount" SortParameterName="sortExpression">
        <selectparameters>
            <asp:Parameter Name="durationIntervalDate" Type="Object" />
            <asp:Parameter Name="contractTypeId" Type="Int32" />
            <asp:Parameter Name="contractStatusId" Type="Int32" />
            <asp:Parameter Name="customerId" Type="Int32" />
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
