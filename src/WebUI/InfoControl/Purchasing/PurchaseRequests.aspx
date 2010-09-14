<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequests.aspx.cs"
    MasterPageFile="~/infocontrol/Default.master" Inherits="Vivina.Erp.WebUI.Accounting.PurchaseRequests"
    Title="Requisições de Compra" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <center>
                    <asp:Button ID="btnDelete" runat="server" Text="Excluir selecionados" OnClick="btnDelete_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnRequest" runat="server" Text="Nova Requisição de Compra" OnClientClick="location='PurchaseRequest.aspx';return false;"
                        UseSubmitBehavior="false" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Text="Solicitar Cadastro de Produto"
                        OnClientClick="location='../Administration/Product_General.aspx?IsTemp=1';return false;"
                        UseSubmitBehavior="false" />
                    <br />
                </center>
                <asp:CheckBoxList ID="chkGrouping" Visible="false" runat="server" AutoPostBack="True"
                    OnTextChanged="chkGrouping_TextChanged" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="Request" Selected="True">Requisição</asp:ListItem>
                    <asp:ListItem Value="City">Cidade</asp:ListItem>
                    <asp:ListItem Value="CostCenter">Centro de Custo</asp:ListItem>
                    <asp:ListItem Value="Category">Categoria</asp:ListItem>
                </asp:CheckBoxList>
                <br />
                <telerik:RadGrid ID="grdPurchaseReq" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    GridLines="Horizontal" showgroupfooter="False" EnableTheming="True" DataSourceID="odsPurchaseRequests"
                    ShowGroupPanel="False" EnableViewState="False" AllowPaging="True">
                    <MasterTableView GroupLoadMode="Client" ShowGroupFooter="false" AllowMultiColumnSorting="false"
                        DataSourceID="odsPurchaseRequests" NoMasterRecordsText="&lt;div style=&quot;text-align: center&quot;&gt; Não existem dados a serem exibidos.&lt;br /&gt;&lt;/div&gt;"
                        EnableViewState="False" GridLines="Horizontal" GroupsDefaultExpanded="true">
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="False" Resizable="False">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="TemplateColumn">
                                <ItemTemplate>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%# (Eval("PurchaseOrderCode") != null) ?"<!--":"" %>
                                    <input type="checkbox" name="requestItems" class='<%# Eval("PurchaseRequestId") %>'
                                        value='<%# Eval("ProductId") + "|" +
                                                     Eval("ProductPackageId") + "|" +
                                                     Eval("ProductManufacturerId") + "|" +
                                                     Convert.ToInt32(Eval("Amount")) + "|" +
                                                     Eval("CostCenter") + "|" +
                                                     Eval("CityId") + "|" +
                                                     Eval("PurchaseRequestItemId") + "|" +
                                                     Eval("PurchaseRequestId") + "|" +
                                                     Eval("Name").EncryptToHex() %>' />&nbsp;&nbsp;
                                    <%# (Eval("PurchaseOrderCode") != null) ?"-->":"" %>
                                    <%#Eval("Name") %>
                                    <span style="color:red">
                                       <a href='PurchaseOrder.aspx?pid=<%#Convert.ToString(Eval("PurchaseOrderId")).EncryptToHex() %>'>
                                        <%# String.IsNullOrEmpty(Convert.ToString(Eval("PurchaseOrderCode")))? "" : "&nbsp;&nbsp;&nbsp;&nbsp;&raquo;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToString(Eval("PurchaseOrderCode")) + "&nbsp;&nbsp;&nbsp;&nbsp;&raquo;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToString(Eval("PurchaseOrderStatus"))%></a></span>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridBoundColumn DataField="CostCenter" GroupByExpression="Group By CostCenter"
                                HeaderText="Centro de Custo" Groupable="true" SortExpression="CostCenter">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CategoryName" GroupByExpression="Group By CategoryName"
                                HeaderText="Categoria" Groupable="False" SortExpression="CategoryName">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="Amount" HeaderText="Qtd." DataFormatString="{0:N0}&nbsp;&nbsp;&nbsp;"
                                AllowFiltering="true" Aggregate="sum" FooterText="Total: " SortExpression="Amount"
                                UniqueName="Amount" Groupable="True" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Right"
                                HeaderStyle-HorizontalAlign="Center">
                                <FooterStyle Font-Bold="True" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" Width="30px"></ItemStyle>
                            </telerik:GridBoundColumn>
                        </Columns>
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="City" FieldName="City" HeaderText="Cidade"
                                        FormatString="<b>{0}</b>" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="City" SortOrder="Ascending"></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="CostCenter" FieldName="CostCenter" HeaderText="Centro de Custo"
                                        FormatString="<b>{0}</b>" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="CostCenter" SortOrder="Ascending"></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                            
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldName="PurchaseRequestId" HeaderText="Req." FormatString="&lt;input type='checkbox' name='request' id='request' value='{0}'  /&gt;&nbsp;&nbsp;<b>{0}</b>" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="PurchaseRequestId" SortOrder="Ascending"></telerik:GridGroupByField>
                                </GroupByFields>                            
                            </telerik:GridGroupByExpression>
                            
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="CategoryName" FieldName="CategoryName" FormatString="<b>{0}</b>"
                                        HeaderText="Categoria" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="CategoryName" SortOrder="Ascending"></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                            
                        </GroupByExpressions>
                        <EditFormSettings>
                            <PopUpSettings ScrollBars="None"></PopUpSettings>
                        </EditFormSettings>
                    </MasterTableView>
                    <GroupingSettings CaseSensitive="False" CollapseTooltip="" ExpandTooltip="" GroupByFieldsSeparator=""
                        GroupContinuedFormatString="" GroupContinuesFormatString="" GroupSplitDisplayFormat="Showing {0} "
                        UnGroupTooltip="" />
                    <GroupHeaderItemStyle Wrap="False" />
                    <ClientSettings AllowDragToGroup="False">
                    </ClientSettings>
                </telerik:RadGrid>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnGeneratePurchaseOrder" runat="server" permissionRequired="PurchaseOrder"
                                Text="Próximo" OnClick="btnGeneratePurchaseOrder_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsPurchaseRequests" runat="server" SelectMethod="GetPurchaseRequestItemsByCompany"
        TypeName="Vivina.Erp.BusinessRules.Accounting.PurchaseManager" SelectCountMethod="GetPurchaseRequestItemsByCompanyCount"
        SortParameterName="sortExpression" onselecting="odsPurchaseRequests_Selecting"
        OldValuesParameterFormatString="original_{0}">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="employeeId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>

    <script>
    $("input:checkbox").click(function(){ 
        $("." + this.value).each(function(){        
            this.checked=!this.checked;
        });
    })
    </script>

</asp:Content>
