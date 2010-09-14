<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_POS_Prospects" Title="Propostas" EnableEventValidation="false"
    CodeBehind="Prospects.aspx.cs" %>

<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc2" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct"
    TagPrefix="uc3" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc1" %>
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
                    <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend><div class="body">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="uPnlSelectCustomer" runat="server">
                                                <contenttemplate>
                                                                <uc2:SelectCustomer ID="sel_customer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer">
                                                                </uc2:SelectCustomer>
                                                            </contenttemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rbtstatus" runat="server" Height="46px" RepeatDirection="horizontal"
                                                Width="262px">
                                            </asp:RadioButtonList>
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
                                <table width="90%">
                                    <tr>
                                        <td>
                                            Vendedor:<br />
                                            <asp:DropDownList ID="cboVendor" runat="server" AppendDataBoundItems="true" DataTextField="Name"
                                                DataValueField="EmployeeId">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            Telefone:<br />
                                            <asp:TextBox runat="server" ID="txtPhone" Plugin="Mask" Mask="(99)9999-9999"  MaxLength="13" Columns="10"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegTxtPhone" runat="server" ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;"
                                                ControlToValidate="txtPhone" ValidationExpression="((\([0-9_]{2}\))([0-9_]{4})\-([0-9_]{4}))?">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                        <td> 
                                            <uc3:SelectProduct ID="selProduct" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Data Início:<br />
                                            <uc1:Date ID="ucBeginDate" runat="server" ShowTime="false" />
                                        </td>
                                        <td>
                                            Data fim:<br />
                                            <uc1:Date ID="ucEndDate" runat="server" ShowTime="false" />
                                        </td>
                                    </tr>
                                </table> 
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <br />
                                            <asp:Button ID="btnSearch" runat="server" Text="Filtrar" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                    </fieldset>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdProspects" RowSelectable="false" runat="server" Width="100%"
                                    AutoGenerateColumns="False" DataSourceID="odsProspects" DataKeyNames="CompanyId,BudgetId,VendorId,ModifiedDate,CustomerId,BudgetCode,Discount"
                                    AllowSorting="True" OnSorting="grdProspects_Sorting" PageSize="20" OnRowDeleted="grdProspects_RowDeleted"
                                    AllowPaging="True" EnableViewState="False" OnChanging="grdProspects_SelectedIndexChanging"
                                    OnRowDataBound="grdProspects_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="BudgetCode" HeaderText="Proposta Número" SortExpression="BudgetCode" />
                                        <asp:TemplateField HeaderText="Cliente" SortExpression="CustomerName">
                                            <ItemTemplate>
                                                <%# Eval("CustomerName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data"
                                            SortExpression="CreatedDate" />
                                        <asp:TemplateField HeaderText="Valor(R$)" SortExpression="BudgetValue">
                                            <ItemTemplate>
                                                <%# Eval("BudgetValue","{0:f}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     
                                        <asp:BoundField HeaderText="Status" DataField="BudgetStatus" SortExpression="BudgetStatusId" />
                                      
                                        <asp:TemplateField HeaderText="&lt;a href=&quot;ProspectBuilder.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                                            SortExpression="Insert" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div class="delete" title="Apagar" budgetid='<%# Eval("BudgetId") %>' companyid='<%# Eval("CompanyId") %>'>
                                                    &nbsp;
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; width: 100%">
                                            Não existem dados a serem exibidos, clique no botão para cadastrar uma proposta.<br />
                                            &nbsp;<asp:Button ID="btnNewProspect" runat="server" Text="Cadastrar" OnClientClick="location='ProspectBuilder.aspx'; return false;" />
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <VFX:BusinessManagerDataSource ID="odsProspects" runat="server" SelectMethod="GetBudgets"
                        TypeName="Vivina.Erp.BusinessRules.SaleManager" ConflictDetection="CompareAllValues"
                        onselecting="odsProspects_Selecting" SortParameterName="sortExpression" OldValuesParameterFormatString="original_{0}"
                        EnablePaging="True" SelectCountMethod="GetBudgetsCount">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
                            <asp:Parameter Name="customerId" Type="Int32" />
                            <asp:Parameter Name="productName" Type="String" />
                            <asp:Parameter Name="budgetStatus" Type="Object" />
                            <asp:parameter name="vendorId" Type="Int32" />
                            <asp:parameter name="beginDate" Type="DateTime" />
                            <asp:parameter name="endDate" Type="DateTime" />
                            <asp:parameter name="telephone" Type="String" />
                            <asp:Parameter Name="sortExpression" Type="String" />
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
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
    </div>
</asp:Content>
