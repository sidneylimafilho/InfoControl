<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" Inherits="InfoControl_POS_AbcCurve"
    MasterPageFile="~/infocontrol/Default.master" CodeBehind="AbcCurve.aspx.cs" Title="Curva ABC" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<%@ Register Src="~/App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc8" %>
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
                            <table>
                                <tr>
                                    <td>
                                        Estoque Físico:<br />
                                        <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                            DataValueField="DepositId" Width="140px" AppendDataBoundItems="true" OnTextChanged="cboDeposit_TextChanged">
                                            <asp:ListItem Value="0" Text="Todos desta empresa"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                       <uc1:DateTimeInterval  ID="ucDateTimeInterval" runat="server" />
                                    </td>
                          </tr>
                            </table>
                   
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblValA" runat="server" Text="A:"></asp:Label>
                                          <uc8:CurrencyField ID="ucCurrFieldValA" Mask="99" Columns="4" ValidationGroup="AddProductToCart"
                                          runat="server" />%
                                        
                                     <%--   <asp:TextBox ID="txtValA" runat="server" Width="20px"></asp:TextBox>%
                                        <ajaxToolkit:MaskedEditExtender ID="MsktxtValA" runat="server" InputDirection="RightToLeft"
                                            Mask="99" MaskType="Number" TargetControlID="txtValA" PromptCharacter=" ">
                                        </ajaxToolkit:MaskedEditExtender>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValB" runat="server" Text="B:"></asp:Label>
                                          <uc8:CurrencyField ID="ucCurrFieldValB" Mask="99" Columns="4"
                                          runat="server" />%
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValC" runat="server" Text="C:"></asp:Label>
                                         <uc8:CurrencyField ID="ucCurrFieldValC" Mask="99" Columns="4" 
                                          runat="server" />%
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValD" runat="server" Text="D:"></asp:Label>
                                         <uc8:CurrencyField ID="ucCurrFieldValD" Mask="99" Columns="4" 
                                          runat="server" />%
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValE" runat="server" Text="E:"></asp:Label>
                                          <uc8:CurrencyField ID="ucCurrFieldValE" Mask="999" Columns="4"
                                          runat="server" />%
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSelectParameter" runat="server" Text="Atualizar" OnClick="btnSelectParameter_Click"
                                            ValidationGroup="AbcCurve" />
                                    </td>
                                </tr>
                            </table>
                  
                            <br />
                            <asp:GridView ID="grdAbcCurve" runat="server" Width="100%" AutoGenerateColumns="False"
                                OnRowCreated="grdAbcCurve_RowCreated" DataKeyNames="productName,Quantity,unitPrice,sumSubTotalCost,sumSubtotal,Percentage,profit,profitMargin"
                                OnRowDataBound="grdAbcCurve_RowDataBound" OnSorting="grdAbcCurve_Sorting">
                                <Columns>
                                    <asp:BoundField DataField="productName" SortExpression="productName" HeaderText="Produto" />
                                    <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Quantidade" />
                                    <asp:BoundField DataField="unitPrice" SortExpression="unitPrice" DataFormatString="{0:c}"
                                        HeaderText="Preço Unitario" />
                                    <asp:BoundField DataField="sumSubTotalCost" SortExpression="sumSubTotalCost" DataFormatString="{0:c}"
                                        HeaderText="Custo" />
                                    <asp:BoundField DataField="sumSubtotal" SortExpression="sumSubtotal" DataFormatString="{0:c}"
                                        HeaderText="Total" />
                                    <asp:BoundField DataField="Percentage" SortExpression="Percentage" DataFormatString="{0:f}"
                                        HeaderText="% de venda" />
                                    <asp:BoundField DataField="profit" SortExpression="profit" DataFormatString="{0:c}"
                                        HeaderText="Lucro" />
                                    <asp:BoundField DataField="profitMargin" SortExpression="profitMargin" DataFormatString="{0:f}"
                                        HeaderText="% de Lucro" />
                                    <asp:TemplateField HeaderText="Classificação"></asp:TemplateField>
                                </Columns>
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
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" onselecting="odsDeposit_Selecting"
        SelectMethod="GetDepositByCompany" TypeName="Vivina.Erp.BusinessRules.DepositManager">
        <selectparameters>
			<asp:parameter Name="companyId" Type="Int32" />
		</selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Header">
</asp:Content>
