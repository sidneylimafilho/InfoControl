<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_RH_EmployeeWorkedTime" Title="Controle de horas Trabalhadas"
    CodeBehind="EmployeeWorkedTime.aspx.cs" %>

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
                <!-- conteudo -->
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdEmployeeWorkedTime" DataKeyNames="EmployeeId,JourneyBegin,IntervalBegin,IntervalEnd,JourneyEnd"
                                runat="server" Width="100%" AutoGenerateColumns="false" RowSelectable="false"
                                OnSelectedIndexChanging="grdEmployeeWorkedTime_SelectedIndexChanging" OnRowEditing="grdEmployeeWorkedTime_RowEditing">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nome">
                                        <ItemTemplate>
                                            <asp:Label ID="LblName" runat="server" Text='<%# Eval("Profile.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Entrada" ItemStyle-Width="1%" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txJourneyBegin" Columns="5" runat="server" Text='<%# Convert.ToDateTime(Eval("JourneyBegin")).ToShortTimeString() %>'>
                                            </asp:TextBox>
                                            <asp:CompareValidator ID="valtxtIntervalBegin" runat="server" ControlToValidate="txJourneyBegin"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="LessThan" ControlToCompare="txtIntervalBegin"
                                                Type="Date"  ValidationGroup="Update"></asp:CompareValidator>
                                            <ajaxToolkit:MaskedEditExtender ID="msktxJourneyBegin" runat="server" TargetControlID="txJourneyBegin"
                                                CultureName="pt-BR" Mask="99:99" MaskType="Time" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="R$ "
                                                CultureDateFormat="DMY" CultureDatePlaceholder="/" CultureDecimalPlaceholder=","
                                                CultureThousandsPlaceholder="." CultureTimePlaceholder=":" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Intervalo" ItemStyle-Width="1%" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIntervalBegin" Columns="5" runat="server" Text='<%#  Convert.ToDateTime(Eval("IntervalBegin")).ToShortTimeString() %>'></asp:TextBox>
                                            <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="txtIntervalBegin"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="LessThan" ControlToCompare="txtIntervalEnd"
                                                Type="Date"  ValidationGroup="Update"></asp:CompareValidator>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtIntervalBegin"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="GreaterThan" ControlToCompare="txJourneyBegin"
                                                Type="Date"  ValidationGroup="Update"></asp:CompareValidator>
                                            <ajaxToolkit:MaskedEditExtender ID="msktxtIntervalBegin" runat="server" TargetControlID="txtIntervalBegin"
                                                CultureName="pt-BR" Mask="99:99" MaskType="Time" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="R$ "
                                                CultureDateFormat="DMY" CultureDatePlaceholder="/" CultureDecimalPlaceholder=","
                                                CultureThousandsPlaceholder="." CultureTimePlaceholder=":" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Retorno" ItemStyle-Width="1%" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIntervalEnd" Columns="5" runat="server" Text='<%# Convert.ToDateTime(Eval("IntervalEnd")).ToShortTimeString() %>'></asp:TextBox>
                                            <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="txtIntervalEnd"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="GreaterThan" ControlToCompare="txtIntervalBegin"
                                                Type="Date"  ValidationGroup="Update"></asp:CompareValidator>
                                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtIntervalEnd"
                                                ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" Operator="LessThan" ControlToCompare="txtJourneyEnd"
                                                Type="Date"  ValidationGroup="Update"></asp:CompareValidator>
                                            <ajaxToolkit:MaskedEditExtender ID="msktxtIntervalEnd" runat="server" TargetControlID="txtIntervalEnd"
                                                CultureName="pt-BR" Mask="99:99" MaskType="Time" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="R$ "
                                                CultureDateFormat="DMY" CultureDatePlaceholder="/" CultureDecimalPlaceholder=","
                                                CultureThousandsPlaceholder="." CultureTimePlaceholder=":" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Saída" ItemStyle-Width="1%" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJourneyEnd" Columns="5" runat="server" Text='<%# Convert.ToDateTime(Eval("JourneyEnd")).ToShortTimeString() %>'></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="msktxtJourneyEnd" runat="server" TargetControlID="txtJourneyEnd"
                                                CultureName="pt-BR" Mask="99:99" MaskType="Time" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="R$ "
                                                CultureDateFormat="DMY" CultureDatePlaceholder="/" CultureDecimalPlaceholder=","
                                                CultureThousandsPlaceholder="." CultureTimePlaceholder=":" Enabled="True">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:CommandField UpdateText="&lt;div class=&quot;save&quot;   title=&quot;salvar&quot;&gt;  &lt;/div&gt;"
                                        CancelText="&lt;div class=&quot;cancel&quot; title=&quot;Cancelar&quot;&gt;&lt;/div&gt;"
                                        ShowEditButton="True" ShowCancelButton="true" EditText="&lt;div class=&quot;save&quot;   title=&quot;salvar&quot;&gt;  &lt;/div&gt;"
                                        ValidationGroup="Update">
                                        <ItemStyle CssClass="actions" />
                                    </asp:CommandField>--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" />
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
</asp:Content>
