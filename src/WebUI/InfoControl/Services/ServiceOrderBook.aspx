<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="InfoControl_Services_ServiceOrderBook" Title="Controle do Talão de OS"
    EnableEventValidation="false" CodeBehind="ServiceOrderBook.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
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
                    <asp:GridView ID="grdServiceOrderBook" runat="server" AutoGenerateColumns="False"
                        EditIndex="0" HorizontalAlign="Center" Width="100%" DataKeyNames="ServiceOrderBookId,CompanyId,RepresentantId,EmployeeId,StartNumber,FinishNumber,Quantity,MinimumQuantity,RepresentantName"
                        OnRowUpdating="grdServiceOrderBook_RowUpdating" OnRowDataBound="grdServiceOrderBook_RowDataBound"
                        OnSelectedIndexChanging="grdServiceOrderBook_SelectedIndexChanging"
                         OnRowCancelingEdit="grdServiceOrderBook_RowCancelingEdit">
                        <Columns>
                            <asp:TemplateField HeaderText="Representante">
                                <ItemStyle Wrap="false" />
                                <EditItemTemplate>
                                    <%--If State equals Edit hide cboSupplier and show lblSupplierItem--%>
                                    <asp:DropDownList ID="cboRepresentant" runat="server" DataSourceID="odsRepresentant"
                                        AppendDataBoundItems="True" DataTextField="Name" DataValueField="RepresentantId"
                                        SelectedValue='<%# Eval("RepresentantId") %>' Visible='<%# !(grdServiceOrderBook.Attributes["State"] == "Edit") %>'>
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblRepresentantItem" runat="server" Text='<%# !String.IsNullOrEmpty(Eval("RepresentantName").ToString()) ? Eval("RepresentantName") : Company.LegalEntityProfile.CompanyName %>'
                                        Visible='<%# (grdServiceOrderBook.Attributes["State"] == "Edit") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRepresentant" runat="server" Text='<%# !String.IsNullOrEmpty(Eval("RepresentantName").ToString()) ? Eval("RepresentantName") : Company.LegalEntityProfile.CompanyName %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Funcionário">
                                <ItemStyle Wrap="false" />
                                <EditItemTemplate>
                                    <%--If State equals Edit hide cboEmployee and show lblEmployeeItem--%>
                                    <asp:DropDownList ID="cboEmployee" runat="server" DataSourceID="odsEmployee" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="EmployeeId" SelectedValue='<%# Eval("EmployeeId") %>'
                                        Visible='<%# !(grdServiceOrderBook.Attributes["State"] == "Edit") %>'>
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblEmployeeItem" runat="server" Text='<%# Eval("EmployeeName") %>'
                                        Visible='<%# (grdServiceOrderBook.Attributes["State"] == "Edit") %>'></asp:Label>
                                    <asp:RequiredFieldValidator ID="reqCboEmployee" runat="server" ControlToValidate="cboEmployee"
                                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployee" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Número Inicial">
                                <ItemStyle Wrap="false" />
                                <EditItemTemplate>
                                    <uc1:CurrencyField ID="ucCurrFieldStartNumber" ValidationGroup="save" Text='<%# Eval("StartNumber") %>'
                                        Mask="99999" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartNumber" runat="server" Text='<%# Eval("StartNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Número Final">
                                <ItemStyle Wrap="false" />
                                <EditItemTemplate>
                                    <uc1:CurrencyField ID="ucCurrFieldFinishNumber" ValidationGroup="save" Text='<%# Eval("FinishNumber") %>'
                                        Mask="99999" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFinishNumber" runat="server" Text='<%# Eval("FinishNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantidade">
                                <ItemStyle Wrap="false" />
                                <EditItemTemplate>
                                    <uc1:CurrencyField ID="ucCurrFieldQuantity" ValidationGroup="save" Text='<%# Eval("Quantity") %>'
                                        Mask="99999" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qtd. Minima">
                                <ItemStyle Wrap="false" />
                                <EditItemTemplate>
                                    <uc1:CurrencyField ID="ucCurrFieldMinimumQuantity" ValidationGroup="save" Text='<%# Eval("MinimumQuantity") %>'
                                        Mask="99999" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMinimumQuantity" runat="server" Text='<%# Eval("MinimumQuantity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField>
                             <ItemTemplate> 
                              <div id="divDelete" onclick="event.cancelBubble=true" class="delete" serviceOrderBookId='<%# Eval("ServiceOrderBookId") %>' > 
                              </div>
                               <ItemStyle Width="1%" />
                             </ItemTemplate> 
                            </asp:TemplateField>
                        
                            <asp:CommandField CancelText="&lt;div class=&quot;cancel&quot; title=&quot;cancelar&quot;&gt;&lt;/div&gt;"
                                EditText="" ShowEditButton="True" UpdateText="&lt;div class=&quot;save&quot; title=&quot;salvar&quot;&gt;&lt;/div&gt;"
                                ValidationGroup="save" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left">                           
                                <ItemStyle Width="1%" Wrap="True" HorizontalAlign="Center" />
                                  <%--   <ItemStyle CssClass="actions" />--%>
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                    <VFX:BusinessManagerDataSource ID="odsEmployee" runat="server" onselecting="odsEmployee_Selecting"
                        SelectMethod="GetTechnicalEmployeeAsDataTable" TypeName="Vivina.Erp.BusinessRules.HumanResourcesManager">
                        <selectparameters> 
                            <asp:Parameter Name="companyId" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsRepresentant" runat="server" onselecting="odsRepresentant_Selecting"
                        SelectMethod="GetRepresentantsByCompany" TypeName="Vivina.Erp.BusinessRules.RepresentantManager">
                        <selectparameters>
                            <asp:Parameter Name="CompanyId" Type="Int32" />
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
