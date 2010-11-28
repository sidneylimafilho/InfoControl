<%@ Page Title="" Language="C#" MasterPageFile="~/Site/1/Site.master" AutoEventWireup="true"
    CodeBehind="Checkout_Payment.aspx.cs" Inherits="Vivina.Erp.WebUI.Site.CheckoutPayment" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<%@ Register runat="server" Src="~/Site/Checkout_Steps.ascx" TagName="CheckoutSteps"
    TagPrefix="uc1" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .error
        {
            color: Red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="pagamento" class="basket">
        <uc1:CheckoutSteps runat="server" />
        <asp:ListView ID="lstPaymentMethod" runat="server" ItemPlaceholderID="itemPlaceHolder"
            DataSourceID="odsPaymentMethods">
            <LayoutTemplate>
                <span class="metodosPgtos"><span runat="server" id="itemPlaceHolder"></span>
                    
                </span>
            </LayoutTemplate>
            <ItemTemplate>
                <table width="90%" class="financierConditions" financieroperationid="<%#Eval("FinancierOperationId") %>"
                    style="float: left; margin: 10px;">
                    <tr>
                        <td width="120px">
                            <img runat="server" id="imgPaymentMethod" alt='<%#Eval("PaymentMethod.Name") %>'
                                class='paymentMethod <%#Eval("PaymentMethod.Name").ToString().RemoveSpecialChars() %>' src='<%# "~/App_Shared/themes/glasscyan/checkout/" + Eval("PaymentMethod.Name").ToString().RemoveSpecialChars() + ".png" %>'
                                style='cursor: hand;' />
                        </td>
                        <td>
                            <%--                                           

                                    Lista de parcelas

                            --%>
                            <div class="lstParcels" style='display: none'>
                                <asp:ListView runat="server" ID="listViewFinacierConditions" ItemPlaceholderID="itemPlaceHolder"
                                    DataSource='<%# (Container.DataItem as FinancierOperation).FinancierConditions.AsQueryable().Sort("ParcelCount") %>'>
                                    <LayoutTemplate>
                                        <span style="margin-left: 20px" runat="server" id="itemPlaceHolder"></span>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <input type="radio" name="parcelCount" value='<%# Eval("ParcelCount")%>' />
                                        <%# Eval("ParcelCount") %>
                                        x
                                        <%# (Total / Convert.ToDecimal(Eval("ParcelCount"))).ToString("C") %>
                                        <br />
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div>
                                            <input type="radio" name="parcelCount" value='1' />
                                            À Vista de
                                            <%= Total.ToString("C") %>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </td>
                        <td align="right">
                            <table class="formAvancar" style="display: none; width: 1%">
                                <tr>
                                    <td id="Td1" runat="server" visible='<%# (int)Eval("PaymentMethodId") == 3 || (int)Eval("PaymentMethodId") == 4%>'>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td width="141" align="right" class="generico_txt_top">
                                                        Número:&nbsp;&nbsp;
                                                    </td>
                                                    <td colspan="3" align="left">
                                                        <input id="cardNumber" name="cardNumber" maxlength="19" class="{required:true, creditcard:true, messages:{required:'*', creditcard:'*'}}" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="141" align="right" class="generico_txt_top">
                                                        Nome do Titular:&nbsp;&nbsp;
                                                    </td>
                                                    <td align="Left" colspan="3" style="font-family: arial; color: rgb(0, 0, 0); font-size: 11px;">
                                                        <input id="cardHolder" name="cardHolder" size="26" maxlength="25" class="{required:true, messages:{required:'*'}}" />
                                                        <br />
                                                        (como consta no cartão)
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="141" align="right" class="generico_txt_top">
                                                        Validade:&nbsp;&nbsp;
                                                    </td>
                                                    <td align="Left" colspan="3">
                                                        <select id="cardMonth" name="cardMonth" size="1" class="{required:true, messages:{required:'*'}}">
                                                            <option value="" selected="" />
                                                            <option value="01">01</option>
                                                            <option value="02">02</option>
                                                            <option value="03">03</option>
                                                            <option value="04">04</option>
                                                            <option value="05">05</option>
                                                            <option value="06">06</option>
                                                            <option value="07">07</option>
                                                            <option value="08">08</option>
                                                            <option value="09">09</option>
                                                            <option value="10">10</option>
                                                            <option value="11">11</option>
                                                            <option value="12">12</option>
                                                        </select>
                                                        <strong>/</strong>
                                                        <select id="cardYear" name="cardYear" size="1" class="{required:true, messages:{required:'*'}}">
                                                            <option value="" selected="" />
                                                            <option value="2009">2009</option>
                                                            <option value="2010">2010</option>
                                                            <option value="2011">2011</option>
                                                            <option value="2012">2012</option>
                                                            <option value="2013">2013</option>
                                                            <option value="2014">2014</option>
                                                            <option value="2015">2015</option>
                                                            <option value="2016">2016</option>
                                                            <option value="2017">2017</option>
                                                            <option value="2018">2018</option>
                                                            <option value="2019">2019</option>
                                                            <option value="2020">2020</option>
                                                            <option value="2021">2021</option>
                                                            <option value="2022">2022</option>
                                                            <option value="2023">2023</option>
                                                            <option value="2024">2024</option>
                                                            <option value="2025">2025</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="141" valign="top" align="right" class="generico_txt_top">
                                                        CVC:&nbsp;&nbsp;
                                                    </td>
                                                    <td align="Left">
                                                        <input id="cardCvc2" name="cardCvc2" size="6" maxlength="4" class="{required:true, digits:true, messages:{required:'*', digits:'*'}}" />
                                                    </td>
                                                    <td align="Left">
                                                        <img id="Img1" border="0" runat="server" src="~/App_Shared/themes/glasscyan/checkout/lock.png" />
                                                    </td>
                                                    <td align="Left">
                                                        <a target="_top" href="javascript:;" onclick="window.open('checkout_whatsCVC.htm', 'cardchave', 'toolbars=0, menubar=0, scrollbars=0, statusbar=0, width=420; height=560; resizable=0; titlebar=0;');"
                                                            class="link" style="font-weight: normal;">O que é o CVC? </a>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td align="right" style='margin-left: 20px'>
                                        <a href="javascript:;" class="avancar" title='Efetuar Compra'>&nbsp;</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
            </ItemTemplate>
        </asp:ListView>
        <br />
        <VFX:BusinessManagerDataSource ID="odsPaymentMethods" runat="server" OnSelecting="odsPaymentMethods_Selecting"
            SelectMethod="GetFinancierOperations" TypeName="Vivina.Erp.BusinessRules.AccountManager"
            OldValuesParameterFormatString="original_{0}">
            <selectparameters>
                    <asp:Parameter Name="companyId" Type="Int32" />
                </selectparameters>
        </VFX:BusinessManagerDataSource>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <iframe id="gateway" style="width: 100%; height: 440px; display: none;" frameborder="0"
        src=""></iframe>
    <span class="CompraEfetuada" id="saleSuccess" runat="server" visible="false">Parabéns
        você realizou a compra com sucesso. Clique
        <asp:LinkButton runat="server" ID="lnkCustomerCentral" CommandName="Central">aqui</asp:LinkButton>
        para acessar a central do cliente. </span>
</asp:Content>
