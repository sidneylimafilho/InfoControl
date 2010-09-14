<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="Company_POS_StockRMA"
    Codebehind="StockRMA.aspx.cs" Title="R M A" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                <td class="left">
                    &nbsp;
                </td>
                <td class="center">
                    <table width="100%">
                        <tr>
                            <td>
                                <br />
                                <center>
                                </center>
                                <asp:GridView ID="grdInventory" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnRowDataBound="grdInventory_RowDataBound" DataKeyNames="ProductId" OnRowUpdating="grdInventory_RowUpdating"
                                    OnRowDeleting="grdInventory_RowDeleting" 
                                    onrowcancelingedit="grdInventory_RowCancelingEdit" rowselectable="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Produto">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProduct" runat="server" AutoCompleteType="Disabled" TabIndex="0"
                                                    Text='<%# Bind("productName") %>' Columns="50" CssClass="cDynDat11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProduct"
                                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="1000"
                                                    FirstRowSelected="True" MinimumPrefixLength="3" ServiceMethod="SearchProductInInventory"
                                                    ServicePath="~/InfoControl/SearchService.asmx" TargetControlID="txtProduct">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("productName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade">
                                            <EditItemTemplate>
                                                <input id="btnDown" class="cUpDown11" tabindex="10" type="button" value="-" /><asp:TextBox
                                                    ID="txtQuantity" runat="server" Columns="4" MaxLength="6" TabIndex="0" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                                                <input id="btnUp" class="cUpDown11" tabindex="11" type="button" value="+" />
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtQuantity"
                                                    CssClass="cErr21" ErrorMessage="&nbsp;&nbsp;&nbsp;" MaximumValue="100000000"
                                                    MinimumValue="0" Type="Integer" ValidationGroup="Grid">*</asp:RangeValidator>
                                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" Maximum="100000"
                                                    Minimum="0" TargetButtonDownID="btnDown" TargetButtonUpID="btnUp" TargetControlID="txtQuantity"
                                                    Width="60">
                                                </ajaxToolkit:NumericUpDownExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:CommandField DeleteText="&lt;span alt=&quot;Apagar&quot;class=&quot;delete&quot;border=0&gt;&lt;/span&gt;"
                                            ShowDeleteButton="True">
                                            <ItemStyle Width="50px" Wrap="false" />
                                        </asp:CommandField>
                                        <asp:CommandField CancelText="&lt;span class=&quot;cancel&quot;title=&quot;cancelar&quot;&gt;&lt;/span&gt;"
                                            EditText="" ShowEditButton="True" UpdateText="&lt;span class=&quot;save&quot;title=&quot;salvar&quot;&gt;&lt;/span&gt;"
                                            ValidationGroup="Salvar" ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="50px" Wrap="false" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <br />
                                <asp:Button ID="btnSave" CssClass="cBtn11" Text="Salvar" runat="server" OnClick="btnSave_Click"
                                    ValidationGroup="Outside" TabIndex="99" UseSubmitBehavior="False" />
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
    </div>
</asp:Content>
