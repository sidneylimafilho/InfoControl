<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequest.aspx.cs"
    MasterPageFile="~/infocontrol/Default.master" Title="Requisição de Compra" Inherits="Vivina.Erp.WebUI.Purchasing.PurchaseRequest" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/InfoControl/Administration/SelectProduct.ascx" TagName="SelectProduct" TagPrefix="uc1" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc2" %>
<%@ Register Src="../../App_Shared/ComboTreeBox.ascx" TagName="ComboTreeBox" TagPrefix="uc3" %>
<%@ Register Src="../../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc5" %>
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
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                
                </contenttemplate>
                </asp:UpdatePanel>
                <table runat="server" id="tbForm">
                    <tr>
                        <td>
                            Selecione o centro de custo desta requisição:<br />
                            <uc3:ComboTreeBox DataSourceID="odsCostCenter" DataTextField="Name" DataFieldParentID="ParentId"
                                DataFieldID="CostCenterId" DataValueField="CostCenterId" ID="cboTreeBoxCostCenter"
                                runat="server" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqcboCostCenter" ValidationGroup="SaveRequest" ControlToValidate="cboTreeBoxCostCenter"
                                runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="AddRequestItem"
                                ControlToValidate="cboTreeBoxCostCenter" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Pesquise o Local de Entrega por Estoque:<br />
                            <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                DataValueField="DepositId" Width="200" AutoPostBack="true" AppendDataBoundItems="true"
                                OnSelectedIndexChanged="cboDeposit_SelectedIndexChanged">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" OnSelecting="odsCostCenter_Selecting"
                                SelectMethod="GetDepositByCompanyAsDataTable" TypeName="Vivina.Erp.BusinessRules.DepositManager">
                                <selectparameters>
                                    <asp:parameter Name="companyId" Type="Int32" />
                                </selectparameters>
                            </VFX:BusinessManagerDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <uc5:Address ID="Address1" Required="true" ValidationGroup="SaveRequest"  FieldsetTitle="Local de Entrega" runat="server" />                        
                        </td>
                    </tr>
                    <tr>
                    <td colspan="99">
                    <table>
                    <tr>
                        <td>
                            <uc1:SelectProduct ID="selProduct" Required="true" ValidationGroup="AddRequestItem"
                                runat="server" />
                        </td>
                        <td>
                            Quantidade:<br />
                            <uc2:CurrencyField ID="ucCurrFieldAmount" Required="true" ValidationGroup="AddRequestItem"
                                Mask="999999" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAddPurchaseRequestItem" ValidationGroup="AddRequestItem" runat="server"
                                Text="Adicionar" OnClick="btnAddPurchaseRequestItem_Click" />
                        </td>
                    </tr>
                </table>
                    </td>
                    </tr>
                </table>
                <asp:GridView RowSelectable="false" ID="dtlPurchaseRequestItem" runat="server" AutoGenerateColumns="False"
                    OnRowDeleting="dtlPurchaseRequestItem_RowDeleting" Width="100%" OnRowDataBound="dtlPurchaseRequestItem_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Produto">
                            <ItemTemplate>
                                <%#Eval("Product.Name" ) %>
                                -
                                <%#Eval("ProductPackage.Name" ) %>
                                -
                                <%#Eval("ProductManufacturer.Name" ) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtd">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox2" ReadOnly="true" Width="30px" runat="server" Text='<%#Eval("Amount" ) %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='Reduzir'>
                            <ItemTemplate>
                                <uc2:CurrencyField ID="TextBox1" Width="30px" Required="true" ValidationGroup="AddRequestItem"
                                    Mask="999" runat="server" Visible='<%# !tbForm.Visible %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="&nbsp;" ItemStyle-CssClass="delete" />
                    </Columns>
                </asp:GridView>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="Button1" runat="server" Text="Cancelar" OnClientClick="location='PurchaseRequests.aspx'; return false;" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSave" runat="server" ValidationGroup="SaveRequest" Text="Salvar"
                                OnClick="btnSave_Click" />
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
    <VFX:BusinessManagerDataSource ID="odsCostCenter" runat="server" onselecting="odsCostCenter_Selecting"
        SelectMethod="GetCostsCenterAsDataTable" TypeName="Vivina.Erp.BusinessRules.AccountManager">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
