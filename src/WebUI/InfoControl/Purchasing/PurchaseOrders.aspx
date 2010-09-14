<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" EnableEventValidation="false"
    AutoEventWireup="true" Inherits="InfoControl_POS_PurchaseOrders" Title="Processo de Compra"
    CodeBehind="PurchaseOrders.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
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
                        Escolha o filtro desejado: </legend><div class="body">
                            <table width="100%">
                                <tr>
                                    <td class="style1">
                                        Status:<br />
                                        <asp:DropDownList ID="cboPurchaseOrderStatus" AppendDataBoundItems="true" runat="server"
                                            DataSourceID="odsPurchaseOrderStatus" DataTextField="Name" DataValueField="PurchaseOrderStatusId">
                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" class="style2">
                                    </td>
                                    <td>
                                        Exibir:<br />
                                        <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td align="right" class="style2">
                                        <asp:Button ID="btnFilter" runat="server" Text="Filtrar" OnClick="btnFilter_Click" />
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
                <asp:GridView ID="grdPurchaseOrders" runat="server" Width="100%" AutoGenerateColumns="False"
                    RowSelectable="false" DataSourceID="odsPurchaseOrders" DataKeyNames="PurchaseOrderId,ModifiedDate,PurchaseOrderStatusId,SupplierId,CompanyId,PurchaseOrderCode,EmployeeId"
                    OnSorting="grdPurchaseOrders_Sorting" AllowPaging="True" AllowSorting="True"
                    PageSize="20" OnRowDataBound="grdPurchaseOrders_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="PurchaseOrderCode" HeaderText="Código" SortExpression="PurchaseOrderCode" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("PurchaseOrderStatus.Name")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fornecedor Vencedor" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("Supplier.LegalEntityProfile.CompanyName")%><%# Eval("Supplier.Profile.Name")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Data de Modificação" SortExpression="ModifiedDate" />
                        <asp:TemplateField HeaderText="&lt;a href=&quot;PurchaseOrder.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="delete" style='display: <%# ((int)Eval("PurchaseOrderStatusId")==(int)PurchaseOrderStatus.InProcess) || ((int)Eval("PurchaseOrderStatusId")==(int)PurchaseOrderStatus.SentToSupplier)?"block":"none" %>'
                                    title="apagar" id='<%# Eval("PurchaseOrderId") %>' companyid='<%# Eval("CompanyId") %>'>
                                    &nbsp</div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="1%" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center;">
                            Não existem Processos de Compra a serem exibidos, para criar um processo de compra você precisa escolher as <a href='PurchaseRequests.aspx'>Requisições de Compra</a> que devem ser atendidas!
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
            <td class="right">
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
    <VFX:BusinessManagerDataSource ID="odsPurchaseOrderStatus" runat="Server" SelectMethod="GetPurchaseOrderStatus"
        TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager">
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsPurchaseOrders" runat="server" onselecting="odsPurchaseOrders_Selecting"
        SelectMethod="GetPurchaseOrdersByCompany" TypeName="Vivina.Erp.BusinessRules.PurchaseOrderManager"
        ConflictDetection="CompareAllValues" DeleteMethod="DeletePurchaseOrder" EnablePaging="True"
        SelectCountMethod="GetPurchaseOrdersByCompanyCount" SortParameterName="SortExpression"
        ondeleting="odsPurchaseOrders_Deleting" OldValuesParameterFormatString="original_{0}">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="status" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="ModifiedDate Desc" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
