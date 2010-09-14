<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" EnableEventValidation="false"
    AutoEventWireup="true" Inherits="InfoControl_CRM_CustomerCalls" EnableViewState="false"
    Title="Chamados" CodeBehind="CustomerCalls.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/LeafBox.ascx" TagName="LeafBox" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc2" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <!-- conteudo -->
                <fieldset id="filter" class="closed">
                    <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"});'>
                        Escolha o filtro desejado: </legend>
                    <div class="body">
                        <table>
                            <tr>
                                <td>                                 
                                    <uc3:SelectCustomer runat="server" ID="selCustomer" OnSelectedCustomer="selCustomer_OnSelectedCustomer" />
                                </td>
                                <td>
                                    Status:<br />
                                    <asp:DropDownList ID="cboStatus" runat="server" AppendDataBoundItems="true" DataSourceID="odsCustomerCallStatus"
                                        DataTextField="Name" DataValueField="CustomerCallStatusId">
                                        <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    Tipo de Chamado:<br />
                                    <asp:DropDownList ID="cboCustomerCallType" runat="server" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="CustomerCallTypeId" DataSourceID="odsCustomerCallType">
                                        <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                                    </asp:DropDownList>
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
                                        <asp:ListItem>Todos</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                                <asp:DropDownList ID="cboPageSize" runat="server">
                                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                    <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                                    <asp:ListItem Value="Todos" Text="Todos"></asp:ListItem>
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
                <asp:GridView ID="grdCustomerCalls" EnableViewState="False" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsCustomerCalls" DataKeyNames="CompanyId,CustomerCallId,ModifiedDate,CustomerId,CallNumber,CallNumberAssociated,Sector,OpenedDate,ClosedDate,CustomerEquipmentId,Description,CustomerCallTypeId,Subject,UserId,technicalEmployeeId,Source,Rating,CustomerCallStatusId"
                    Width="100%" OnSorting="grdCustomerCalls_Sorting" OnRowDataBound="grdCustomerCalls_RowDataBound"
                    RowSelectable="false" PageSize="20" AllowPaging="True" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Status" SortExpression="customerCallStatusName">
                            <ItemTemplate>
                                <%# Eval("customerCallStatusName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assunto" SortExpression="Subject">
                            <ItemTemplate>
                                <b>
                                    <%# Eval("Subject") %>
                                </b>
                                <br />
                                <span class="info">
                                    <%# Eval("Sector") %>
                                </span><span class="moreInfo">&nbsp;&nbsp;|&nbsp;&nbsp;Usuário:&nbsp;<%# Eval("UserName")%>
                                </span></label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="Cliente" SortExpression="CustomerName" />
                        <asp:TemplateField HeaderText="Técnico" SortExpression="TechnicalUserName">
                            <ItemTemplate>
                                <%# Eval("technicalProfile.AbreviatedName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Data da Ocorrência" DataField="OpenedDate" SortExpression="OpenedDate" />
                        <asp:TemplateField HeaderText="&lt;a href=&quot;CustomerCall.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;">
                            <ItemTemplate>
                                <div class="delete" title="Apagar" id='<%# Eval("CustomerCallId") %>'>
                                    &nbsp;
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos, clique no botão para cadastrar um chamado.<br />
                            &nbsp;<asp:Button ID="btnTransfer" runat="server" Text="Cadastrar" OnClick="btnTransfer_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsCustomerCallStatus" runat="server" SelectMethod="GetCustomerCallStatus"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCustomerCallType" runat="server" SelectMethod="GetCustomerCallTypes"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsTechnicalEmployee" runat="server" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager"
        SelectMethod="GetTechnicalEmployeeAsDataTable" onselecting="odsTechnicalUsers_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    
    <VFX:BusinessManagerDataSource ID="odsCustomerCalls" runat="server" SelectMethod="GetCustomerCalls"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" ConflictDetection="CompareAllValues"
        SortParameterName="sortExpression" EnablePaging="True" SelectCountMethod="GetCustomerCallsCount"
        onselecting="odsCustomerCalls_Selecting" OldValuesParameterFormatString="original_{0}">
        <selectparameters>

          <asp:Parameter Name="companyId" Type="Int32" /> 
          <asp:Parameter Name="customerId" Type="Int32" />
            <asp:Parameter Name="customerCallStatusId" Type="Int32" />
             <asp:Parameter Name="customerCallType" Type="Int32" />
            <asp:Parameter Name="technicalEmployeeId" Type="Int32" />
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
      
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
