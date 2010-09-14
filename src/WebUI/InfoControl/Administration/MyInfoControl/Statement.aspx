<%@ Page Title="Extrato" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" CodeBehind="Statement.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Accounting.Statement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table class="cLeafBox21" width="100%">
        <tr class="top">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                &#160;
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="middle">
            <td class="left">
                &#160;
            </td>
            <td class="center">
                <%--Conteúdo--%>
                <table width="80%">
                    <tr>
                        <td>
                            <b>
                                <asp:Literal runat="server" ID="litCustomerName" />
                            </b>
                            <br />
                            <asp:Literal runat="server" ID="litCustomerAddress" />
                            <br />
                            <asp:Literal runat="server" ID="litCustomerEmail" />
                            <br />
                            <asp:Literal runat="server" ID="litCustomerPhone" />
                        </td>
                        <td>
                            <b>Número do Extrato: </b>&nbsp&nbsp
                            <a href="~/site/boletu.aspx" runat="server"><asp:Literal runat="server" ID="litBoletusNumber" /></a>
                            <br />
                            <b>Período: </b>&nbsp&nbsp
                            <asp:Literal runat="server" ID="litPeriodBegin" />
                            -
                            <asp:Literal runat="server" ID="litPeriodEnd" />
                            <br />
                            <%-- Número da Conta Bancária: &nbsp&nbsp <asp:Literal runat="server" id="litAccount" /> --%>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <fieldset>
                    <legend>Itens do Extrato </legend>
                    <asp:GridView runat="server" Width="100%" ID="grdStatementItems" AutoGenerateColumns="false"
                        DataKeyNames="Name,Quantity,Value" OnRowDataBound="grdStatementItems_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="Nome" DataField="Name" />
                            <asp:BoundField HeaderText="Quantidade" DataField="Quantity" />
                            <asp:BoundField HeaderText="Valor" DataField="Value" />
                        </Columns>
                    </asp:GridView>
                    <div align="right">
                        <b>Total:</b> &nbsp&nbsp
                        <asp:Label runat="server" ID="lblStatementTotal"> </asp:Label>
                    </div>
                </fieldset>
                <br />
                <br />
                <div align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClientClick="location='Statements.aspx'; return false;" />
                </div>
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
        <tr class="bottom">
            <td class="left">
                &#160;
            </td>
            <td class="center">
            </td>
            <td class="right">
                &#160;
            </td>
        </tr>
    </table>
</asp:Content>
