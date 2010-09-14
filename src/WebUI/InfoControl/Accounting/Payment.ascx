<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Payment.ascx.cs" Inherits="InfoControl.Web.UI.Payment" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/Address/Address.ascx" TagName="Address" TagPrefix="uc1" %>
<div class="divPayment">
    <form id="frmBasket">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
  <fieldset id="paymenMethodFieldSet">
    <legend>Formas de Pagamento </legend>
    
        <table width="40%">
            <tr>
                <td>
                    <asp:ListView ID="lstPaymentMethod" runat="server" ItemPlaceholderID="itemPlaceHolder"
                        DataSourceID="odsPaymentMethods">
                        <LayoutTemplate>
                            <div class="metodosPgtos">
                                <span runat="server" id="itemPlaceHolder"></span>
                                <br />
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <span class="metodoPgto">
                                <img runat="server" id="imgPaymentMethod" alt='<%#Eval("PaymentMethod.Name") %>'
                                    src='<%# Eval("PaymentMethod.ConfigUrl") %>' />
                                <asp:ListView runat="server" ID="listViewFinacierConditions" ItemPlaceholderID="itemPlaceHolder"
                                    DataSource='<%#Eval("FinancierConditions") %>'>
                                    <LayoutTemplate>
                                        <div style="margin-left: 20px">
                                            <span style="margin-left: 20px" runat="server" id="itemPlaceHolder"></span>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <input type="radio" id="rbtFinancierCondition" name="rbtFinancierCondition" value='<%# Eval("FinancierConditionId")%>' />
                                        <asp:Label ID="lblParcelCount" runat="server" Text='<%# Eval("ParcelCount") %>'> </asp:Label>
                                        x
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# (Convert.ToDecimal(Page.ViewState["totalWithDeliveryPrice"] == null ? total : Page.ViewState["totalWithDeliveryPrice"]) / Convert.ToDecimal(Eval("ParcelCount"))).ToString("C") %>'>  </asp:Label>
                                        
                                        <br />
                                    </ItemTemplate>
                                </asp:ListView>
                            </span>
                        </ItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
        <br />

</fieldset> </contenttemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblError" runat="server"></asp:Label>
    </asp:RequiredFieldValidator>
    <div class="avancar" style="text-align: right">
        <asp:Button runat="server" ID="btnNext" Text="Avançar" ValidationGroup="delivery"
            OnClick="btnNext_Click" />
    </div>
    </form>
</div>
<VFX:BusinessManagerDataSource ID="odsPaymentMethods" runat="server" OnSelecting="odsPaymentMethods_Selecting"
    SelectMethod="GetFinancierOperations" TypeName="Vivina.Erp.BusinessRules.AccountManager"
    OldValuesParameterFormatString="original_{0}">
    <SelectParameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
