<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Title="Clientes" Inherits="Company_Customers" CodeBehind="Customers.aspx.cs" %>

<%@ Register Src="../../App_Shared/AlphabeticalPaging.ascx" TagName="AlphabeticalPaging"
    TagPrefix="uc1" %>
<%@ Register Src="SelectCustomer.ascx" TagName="SelectCustomer" TagPrefix="uc2" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
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
                    <fieldset id="filter" class="closed">
                        <legend><span id="accountSearchLegend">Escolha o filtro desejado: </span>&nbsp&nbsp&nbsp
                            | &nbsp&nbsp&nbsp <span id="importDataLegend">Importação de Clientes: </span>
                        </legend>
                        <div id="accountSearchBody" class="body">
                            <table width="100%">
                                <tr>
                                    <td>
                                        Nome:
                                        <br />
                                        <asp:TextBox ID="txtSelectedCustomer" runat="server" MaxLength="100"> </asp:TextBox>
                                    </td>
                                    <td>
                                        Tipo de Cliente:
                                        <br />
                                        <asp:DropDownList ID="cboCustomerType" AppendDataBoundItems="true" runat="server"
                                            DataSourceID="odsCustomerType" DataTextField="Name" DataValueField="CustomerTypeId">
                                            <asp:ListItem> &nbsp;  </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        E-mail:<br />
                                        <asp:TextBox ID="txtMail" Columns="20" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        CNPJ:<br />
                                        <asp:TextBox ID="txtCNPJ" Columns="14" Mask="99.999.999/9999-99" runat="server"></asp:TextBox>                                      
                                    </td>
                                    <td>
                                        CPF:<br />
                                        <asp:TextBox ID="txtCPF" Columns="10" Mask="999.999.999-99" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        Telefone:<br />
                                        <asp:TextBox ID="txtPhone" Mask="phone" Columns="10" runat="server"></asp:TextBox>                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Equipamento:<br />
                                        <asp:TextBox ID="txtEquipment" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        A partir de:&nbsp;
                                        <ajaxToolkit:Rating ID="rtnRanking" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                            WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                            ToolTip="Classificação" CurrentRating="0">
                                        </ajaxToolkit:Rating>
                                    </td>
                                    <td>
                                        Exibir:<br />
                                        <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnSearch" runat="server" Text="Pesquisar" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="importDataBody" class="body">
                            Arquivo:
                            <br>
                            <asp:FileUpload ID="fupImportExcelFile" runat="server" />
                            <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ValidationGroup="addExcelFile" ErrorMessage="&nbsp&nbsp&nbsp"
                                ControlToValidate="fupImportExcelFile" />
                            <asp:Button runat="server" ID="btnImportExcelFile" Text="Importar Planilha" ValidationGroup="addExcelFile"
                                OnClick="btnImportExcelFile_Click" />
                        </div>
                        <span id="closeFilter" class="closeButton">&nbsp;</span>
                        <br />
                        <br />
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <uc1:AlphabeticalPaging ID="ucAlphabeticalPaging" OnSelectedLetter="ucAlphabeticalPaging_SelectedLetter"
                        runat="server" />
                    <br />
                    <asp:GridView ID="grdCustomers" runat="server" AutoGenerateColumns="False" DataSourceID="odsCustomers"
                        DataKeyNames="CustomerId,BlockSalesInDebit,CustomerTypeId,ProfileId,LegalEntityProfileId,ModifiedDate,CompanyId,SalesPersonId,SalesPersonCommission,supplementalSalesPersonId,supplementalSalesPersonCommission,UserId,BankId,Agency,AccountNumber,Ranking,Observation"
                        OnRowDataBound="grdCustomers_RowDataBound" OnSorting="grdCustomers_Sorting" AllowSorting="True"
                        Width="100%" RowSelectable="false" permissionRequired="Customers" AllowPaging="True"
                        PageSize="20" EnableViewState="False">
                        <Columns>
                            <asp:TemplateField HeaderText="Nome" SortExpression="Name">
                                <ItemTemplate>
                                    <%# 
                                Eval("ProfileName") ?? "<b>" + Convert.ToString(Eval("FantasyName")) + "</b> (" + Convert.ToString(Eval("CompanyName")) + ")"%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CPF/CNPJ" SortExpression="Identification">
                                <ItemTemplate>
                                    <%# Eval("Identification")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="E-mail" SortExpression="Email">
                                <ItemTemplate>
                                    <%# Eval("Email")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Telefone" SortExpression="Phone">
                                <ItemTemplate>
                                    <%# Eval("Phone")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="&lt;a href=&quot;Customer_General.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div class="delete" title="Apagar" customerid='<%# Eval("CustomerId") %>' companyid='<%# Eval("CompanyId") %>'>
                                        &nbsp;
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="1%" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="text-align: center">
                                Não existem dados a serem exibidos, clique no botão para cadastrar um cliente.<br />
                                &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClientClick="location='Customer_General.aspx'; return false;" />
                            </div>
                        </EmptyDataTemplate>
                        <PagerStyle CssClass="Pager" />
                        <FooterStyle CssClass="Footer" />
                    </asp:GridView>
                    <br />
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
        <VFX:businessmanagerdatasource id="odsSearchCustomer" runat="server" enablepaging="True"
            onselecting="odsSearchCustomer_Selecting" selectcountmethod="SearchCustomersCount"
            selectmethod="SearchCustomers" typename="Vivina.Erp.BusinessRules.CustomerManager">
            <selectparameters>
                <asp:Parameter Name="htCustomer" Type="Object" />
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
        </VFX:businessmanagerdatasource>
        <VFX:businessmanagerdatasource id="odsCustomers" runat="server" onselecting="odsCustomers_Selecting"
            selectmethod="GetCustomerByCompany" typename="Vivina.Erp.BusinessRules.CustomerManager"
            ondeleted="odsCustomers_Deleted" enablepaging="True" sortparametername="sortExpression"
            oldvaluesparameterformatstring="original_{0}" selectcountmethod="GetCustomerByCompanyCount"
            dataobjecttypename="Vivina.Erp.DataClasses.Customer" deletemethod="Delete">
            <selectparameters>
				<asp:parameter Name="companyId" Type="Int32" />
                <asp:parameter Name="representantId" Type="Int32" />
			    <asp:Parameter Name="sortExpression" Type="String" />
			    <asp:Parameter Name="initialLetter" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
			</selectparameters>
        </VFX:businessmanagerdatasource>
        <VFX:businessmanagerdatasource id="odsAppointment" runat="server" selectmethod="GetAppointments"
            typename="Vivina.Erp.BusinessRules.AppointmentManager">
            <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32" />
                <asp:Parameter Name="employeeID" Type="Int32" />
            </selectparameters>
        </VFX:businessmanagerdatasource>
        <VFX:businessmanagerdatasource id="odsCustomerType" runat="server" selectmethod="GetAllCustomerType"
            typename="Vivina.Erp.BusinessRules.CustomerManager" onselecting="odsCustomerType_Selecting">
            <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32" />
            </selectparameters>
        </VFX:businessmanagerdatasource>
    </div>
</asp:Content>
