<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Administration_Contract" Title="Contrato"
    CodeBehind="Contract.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer" TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc3" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtContractNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="cErr21" ID="valtxtContractNumber" runat="server" ControlToValidate="txtContractNumber"
                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
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
                <table>
                    <tr>
                        <td>
                            <%--Nome do Contrato:<br />--%>
                            <%--<asp:TextBox ID="txtName" runat="server" Columns="40" MaxLength="50" Visible="false" /> --%>
                        </td>
                        <td>
                            <%--<asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;--%>
                        </td>
                        <td>
                            Tipo:<br />
                            <asp:DropDownList ID="cboContractType" runat="server" AppendDataBoundItems="true"
                                DataSourceID="odsContractType" DataTextField="Name" DataValueField="ContractTypeId">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboContractType" runat="server" ControlToValidate="cboContractType"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            Status:<br />
                            <asp:DropDownList ID="cboContractStatus" runat="server" AppendDataBoundItems="true"
                                DataSourceID="odsContractStatus" DataTextField="Name" DataValueField="ContractStatusId">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboContractStatus" runat="server" ControlToValidate="cboContractStatus"
                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <fieldset>
                    <legend>Dados do Cliente:</legend>
                    <table width="100%">
                        <tr>
                            <td valign="top">
                                <uc2:SelectCustomer ID="SelCustomer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="reqSelCustomer" runat="server" ControlToValidate="SelCustomer"
                                     ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="InsertContract">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            Representantes/Corretores:<br />
                                            <asp:DropDownList ID="cboRepresentants" AppendDataBoundItems="true" runat="server"
                                                DataSourceID="odsRepresentants" DataTextField="Name" DataValueField="RepresentantId">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Operador:<br />
                                            <asp:DropDownList ID="cboVendors" runat="server" AppendDataBoundItems="true" DataSourceID="odsSalesPerson"
                                                DataTextField="Name" DataValueField="EmployeeId">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upContractTemplate" runat="server">
                                    <contenttemplate>
                                        Modelo:<br />
                                    <asp:DropDownList ID="cboContractTemplate" runat="server" 
                                            AppendDataBoundItems="True" DataSourceID="odsContractTemplates" 
                                            DataTextField="FileName" DataValueField="DocumentTemplateId">
                                        </asp:DropDownList>
                                        &nbsp;
                                        
                                </contenttemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upParcelsConfiguration">
                    <progresstemplate>
                        <div class="cLoading11" style="position: absolute; width: 95%; height: 160px;">
                            <img id="Img1" runat="server" alt="Carregando" src="~/App_Shared/themes/glasscyan/loading3.gif" />
                        </div>
                    </progresstemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upParcelsConfiguration" runat="server">
                    <contenttemplate>
                         <fieldset>
                            <legend>Parcelas:</legend>
                            <div id="dvFieldParcels" style="width: 100%" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            Total (R$):<br />
                                            <uc4:CurrencyField ID="ucContractValue" ValidationGroup="InsertContract" Required="true"
                                                runat="server" />
                                        </td>
                                        <td>
                                            1º parcela:<br />
                                            <uc1:Date ID="ucParcelDueDate" Required="true" ValidationGroup="InsertContract" runat="server" />
                                        </td>
                                        <td>
                                            Forma de Pagamento:<br />
                                            <asp:DropDownList ID="cboPaymentMethods" runat="server" DataSourceID="odsFinancierOperation"
                                                DataTextField="PaymentMethodName" DataValueField="FinancierOperationId" AppendDataBoundItems="true"
                                                OnTextChanged="cboFinancierOperationId_TextChanged" AutoPostBack="True">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboFinancierOperationId2" runat="server" Display="Dynamic"
                                                ControlToValidate="cboPaymentMethods" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"
                                                ValidationGroup="valCalculateParcel"></asp:RequiredFieldValidator>
                                                
                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboFinancierOperationId" runat="server" Display="Dynamic"
                                                ControlToValidate="cboPaymentMethods" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"
                                                ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Qtd de parcelas:&nbsp;<br />
                                            <asp:DropDownList ID="cboParcels" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                                DataSourceID="odsParcels" DataTextField="ParcelInterval" DataValueField="FinancierConditionId"
                                                OnSelectedIndexChanged="cboParcels_SelectedIndexChanged" CausesValidation="true">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqParcels" runat="server" Display="Dynamic"
                                                ControlToValidate="cboParcels" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;"
                                                ValidationGroup="InsertContract"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="right">
                                            &nbsp;<asp:HyperLink ID="lnkParcelValue" Visible="false" runat="server"></asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="dvGrdParcels" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdParcels" runat="server" AutoGenerateColumns="False" Width="100%">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Desc." DataField="Description" />
                                                    <asp:TemplateField HeaderText="Forma de Pagamento">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cboPaymentMethods" Width="80px" runat="server" DataSourceID="odsFinancierOperation"
                                                                DataTextField='<%# Bind("PaymentMethodName") %>' DataValueField="FinancierOperationId" SelectedValue='<%# Bind("FinancierOperationId") %>'
                                                                Enabled="false">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DueDate" HeaderText="Vencimento" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Valor" />
                                                    <asp:BoundField DataField="EffectedDate" HeaderText="Quitada em" DataFormatString="{0:dd/MM/yyyy}"
                                                        ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    Não existem parcelas
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <br />
                        <fieldset>
                            <legend>Regência:</legend>
                            <table>
                                <tr>
                                    <td>
                                        <uc3:DateTimeInterval ID="ucdtIntervalContract" runat="server" Required="true" ValidationGroup="InsertContact" />
                                    </td>
                                    <td>
                                        Multa (R$):<br />
                                        <uc4:CurrencyField Required="false" ID="ucPenalty" runat="server" />
                                    </td>
                                    <td>
                                        Juros (%):<br />
                                        <asp:TextBox ID="txtInterestDeferredPayment" Columns="6" runat="server" />
                                        <ajaxToolkit:MaskedEditExtender ID="mskTxtInterestDeferredPayment" runat="server"
                                            InputDirection="RightToLeft" Mask="999.99" MaskType="Number" TargetControlID="txtInterestDeferredPayment">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        Valor do Pecúlio:
                                        <br />
                                        <uc4:CurrencyField ID="ucMoneyReserves" runat="server" />
                                    </td>
                                    <td>
                                        Mensalidade:
                                        <br />
                                        <uc4:CurrencyField ID="ucMonthlyFee" runat="server" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Valor do Seguro:
                                        <br />
                                        <uc4:CurrencyField ID="ucInsurance" runat="server" />
                                    </td>
                                    <td>
                                        Valor/Hora (R$):<br />
                                        <uc4:CurrencyField Required="false" ID="ucHH" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </contenttemplate>
                </asp:UpdatePanel>
                <br />
                <fieldset runat="server" id="fdsAdditional">
                    <legend>Valores Adicionais </legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlContractAdditionalValue1" runat="server">
                                    <asp:Literal ID="lblContractAdditionalValue1" runat="server"></asp:Literal><br />
                                    <uc4:CurrencyField Required="false" ID="ucContractAdditionalValue1" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlContractAdditionalValue2" runat="server">
                                    <asp:Literal ID="lblContractAdditionalValue2" runat="server"></asp:Literal><br />
                                    <uc4:CurrencyField Required="false" ID="ucContractAdditionalValue2" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlContractAdditionalValue3" runat="server">
                                    <asp:Literal ID="lblContractAdditionalValue3" runat="server"></asp:Literal><br />
                                    <uc4:CurrencyField Required="false" ID="ucContractAdditionalValue3" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlContractAdditionalValue4" runat="server">
                                    <asp:Literal ID="lblContractAdditionalValue4" runat="server"></asp:Literal><br />
                                    <uc4:CurrencyField Required="false" ID="ucContractAdditionalValue4" runat="server" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlContractAdditionalValue5" runat="server">
                                    <asp:Literal ID="lblContractAdditionalValue5" runat="server"></asp:Literal><br />
                                    <uc4:CurrencyField Required="false" ID="ucContractAdditionalValue5" runat="server" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            Observação:<br />
                            <asp:TextBox Width="100%" ID="txtObservation" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <%--<asp:UpdatePanel runat="server" ID="pnlAssociatedContract">
            <contenttemplate>--%>
                <fieldset>
                    <legend>Contratos relacionados</legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            Contratos:<br />
                                            <asp:DropDownList ID="cboCustomerContracts" runat="server" DataSourceID="odsContractsByCustomer"
                                                DataTextField="ContractNumber" AppendDataBoundItems="true" DataValueField="ContractId">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Valor:<br />
                                            <uc4:CurrencyField Required="True" ValidationGroup="AddAssociatedContract" ID="ucCUrrFieldContractValue"
                                                runat="server" />
                                        </td>
                                        <td>
                                            Data do Vencimento:<br />
                                            <uc1:Date ID="ucDtDueDate" Required="true" ValidationGroup="AddAssociatedContract"
                                                runat="server" />
                                        </td>
                                        <td>
                                            Data do Pagamento:<br />
                                            <uc1:Date ID="ucDtPaidDate" Required="true" ValidationGroup="AddAssociatedContract"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAdd" ValidationGroup="AddAssociatedContract" runat="server" Text="Adicionar"
                                                OnClick="btnAdd_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grdAssociatedContracts" Width="100%" AllowPaging="True" AllowSorting="True"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="ContractAssociatedId,Amount,PaidDate,DueDate,ContractId,CompanyId"
                                    OnSorting="grdAssociatedContracts_Sorting" OnRowCommand="grdAssociatedContracts_RowCommand"
                                    OnRowDeleting="grdAssociatedContracts_RowDeleting" OnRowDataBound="grdAssociatedContracts_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="Amount" HeaderText="Valor" SortExpression="Amount" />
                                        <asp:BoundField DataField="DueDate" HeaderText="Data de Vencimento" SortExpression="DueDate" />
                                        <asp:BoundField DataField="PaidDate" HeaderText="Data de Pagamento" SortExpression="PaidDate" />
                                        <asp:CommandField ShowDeleteButton="True" DeleteText="<span class='delete' title='excluir'> </span>" 
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <center>
                                            Não existem registros</center>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <%--         </contenttemplate>
        </asp:UpdatePanel>--%>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSave" ValidationGroup="InsertContract" runat="server" Text="Salvar"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsContractsByCustomer" TypeName="Vivina.Erp.BusinessRules.ContractManager"
        SelectMethod="RetrieveUnassociatedContractsByCustomer" onselecting="odsContractsByCustomer_Selecting"
        runat="server">
        <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
            </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsRepresentants" runat="server" SelectMethod="GetRepresentantsByCompany"
        TypeName="Vivina.Erp.BusinessRules.RepresentantManager" onselecting="odsGeneric_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsSalesPerson" runat="server" SelectMethod="GetSalesPerson"
        TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="odsGeneric_Selecting">
        <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32"></asp:Parameter>
            </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContractTemplates" runat="server" onselecting="odsContractTemplates_Selecting"
        SelectMethod="GetContractTemplatesByCompany" TypeName="Vivina.Erp.BusinessRules.ContractManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContractPeriod" runat="server" SelectMethod="GetContractPeriod"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsFinancierOperation" runat="server" SelectMethod="GetPaymentMethods"
        TypeName="Vivina.Erp.BusinessRules.AccountManager" 
        OnSelecting="odsFinancierOperation_Selecting" 
        ConflictDetection="CompareAllValues">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContractType" runat="server" SelectMethod="GetContractTypes"
        TypeName="Vivina.Erp.BusinessRules.ContractManager" OnSelecting="odsFinancierOperation_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsContractStatus" runat="server" SelectMethod="GetAllContractStatus"
        TypeName="Vivina.Erp.BusinessRules.ContractManager" OnSelecting="odsContractStatus_Selecting">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsParcels" runat="server" SelectMethod="GetFormatedFinancierConditionsByFinancierOperation"
        TypeName="Vivina.Erp.BusinessRules.AccountManager" OnSelecting="odsParcels_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="financierOperationId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>

    <script language="javascript" type="text/javascript">

        //    var cboContractTemplate = document.getElementById("<%= this.cboContractTemplate.ClientID %>");
        //    
        //    
        //   // cboContractTemplate.onchange = showContract();
        //    
        //    function showContract()
        //    {
        //        location = "ContractTemplateBuilder.aspx?ContractId=1&ContractTemplateId=21";
        //       
        //        
        //    }
    
    
    </script>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Header">
</asp:Content>
