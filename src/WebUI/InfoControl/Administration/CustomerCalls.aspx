<%@ Page Language="C#" AutoEventWireup="true" Inherits="Company_Administration_CustomerCalls"
    CodeBehind="CustomerCalls.aspx.cs" Title="" MasterPageFile="~/infocontrol/Default.master"  %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <!-- conteudo -->
                <asp:GridView ID="grdCustomerCalls" EnableViewState="False" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsCustomerCalls" DataKeyNames="CompanyId,CustomerCallId,ModifiedDate,CustomerId,CallNumber,CallNumberAssociated,Sector,OpenedDate,ClosedDate,CustomerEquipmentId,Description,CustomerCallTypeId,Subject,UserId,technicalEmployeeId,Source,Rating,CustomerCallStatusId"
                    AllowSorting="True" Width="100%" RowSelectable="false" PageSize="20" AllowPaging="True" 
                    onrowdatabound="grdCustomerCalls_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Prioridade" SortExpression="Rating">
                            <ItemTemplate>
                                <ajaxToolkit:Rating ID="rtnPriority" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                    WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                    ToolTip="Classificação" ReadOnly="true" CurrentRating='<%# Convert.ToInt32(Eval("Rating")) %>'
                                    Enabled="false">
                                </ajaxToolkit:Rating>
                            </ItemTemplate>
                            <ItemStyle Width="9.3%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%# Eval("customerCallStatusName") %>
                            </ItemTemplate>
                             <ItemStyle Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assunto">
                            <ItemTemplate>
                                <b>
                                    <%# Eval("Subject") %>
                                </b>
                                <br />
                                <span class="moreInfo">&nbsp;&nbsp;|&nbsp;&nbsp;Usuário:&nbsp;<%# Eval("UserName")%></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Técnico">
                            <ItemTemplate>
                                <%# Eval("TechnicalUserName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Data da Ocorrência" DataField="OpenedDate" SortExpression="OpenedDate" />

                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align: center">
                            Não existem dados a serem exibidos.
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
    <VFX:BusinessManagerDataSource ID="odsCustomerCalls" runat="server" OnSelecting="odsCustomerCalls_Selecting"
        SelectCountMethod="GetCustomerCallsByCustomerCount" SelectMethod="GetCustomerCallsByCustomer"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" ConflictDetection="CompareAllValues"
        EnablePaging="True" OldValuesParameterFormatString="original_{0}">
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
