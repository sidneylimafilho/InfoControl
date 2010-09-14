<%@ Control Language="C#" AutoEventWireup="true" Inherits="Host_Package_General"
    CodeBehind="Package_General.ascx.cs" %>
<%@ Register Assembly="Vivina.Framework.Web" Namespace="Vivina.Framework.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
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
            <asp:FormView ID="frmPackage" runat="server" DataKeyNames="PackageId,Name,NumberUsers,IsActive,NumberItems,Price,ModifiedDate"
                DataSourceID="odsPackage" DefaultMode="Insert" Width="100%" OnItemCommand="frmPackage_ItemCommand"
                OnItemInserting="frmPackage_ItemInserting">
                <EditItemTemplate>
                    <table>
                        <tr>
                            <td>
                                Nome do Pacote:<br />
                                <asp:TextBox ID="NameTextBox" Columns="30" MaxLength="30" runat="server" Text='<%# Bind("Name") %>' />
                                <asp:RequiredFieldValidator ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RequiredFieldValidator1"
                                    runat="server" ControlToValidate="NameTextBox"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Número de Usuários:<br />
                                <input type="button" value="-" id="btnDown" class="cUpDown11" tabindex="100" />
                                <asp:TextBox ID="NumberUsersTextBox" runat="server" Text='<%# Bind("NumberUsers") %>'
                                    Columns="5" MaxLength="5" /><input type="button" value="+" id="btnUp" class="cUpDown11"
                                        tabindex="101" />
                                <asp:RequiredFieldValidator ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RequiredFieldValidator2"
                                    runat="server" ControlToValidate="NumberUsersTextBox"></asp:RequiredFieldValidator>
                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" Maximum="1000"
                                    Minimum="0" TargetControlID="NumberUsersTextBox" TargetButtonDownID="btnDown"
                                    TargetButtonUpID="btnUp" Width="20">
                                </ajaxToolkit:NumericUpDownExtender>
                            </td>
                            <td>
                                Numero de Produtos:<br />
                                <input type="button" value="-" id="btnDown2" class="cUpDown11" tabindex="200" />
                                <asp:TextBox ID="NumberItemsTextBox" runat="server" Text='<%# Bind("NumberItems") %>'
                                    Columns="5" MaxLength="5" />
                                <input type="button" value="+" id="btnUp2" class="cUpDown11" tabindex="201" />
                                <asp:RequiredFieldValidator ErrorMessage="&nbsp;&nbsp;&nbsp;" ID="RequiredFieldValidator3"
                                    runat="server" ControlToValidate="NumberItemsTextBox"></asp:RequiredFieldValidator>
                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender2" runat="server" Maximum="1000"
                                    Step="50" Minimum="0" TargetControlID="NumberItemsTextBox" TargetButtonDownID="btnDown2"
                                    TargetButtonUpID="btnUp2" Width="60">
                                </ajaxToolkit:NumericUpDownExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Valor (R$):<br />
                                <uc1:CurrencyField ID="ucCurrFieldPrice" Mask="999.999.999" Text='<%# Bind("Price","{0:F}") %>'
                                    runat="server" />
                            </td>
                            <td>
                                Valor por Hora:
                                <br />
                                <uc1:CurrencyField Mask="999.999.999" ID="ucCurrFieldValueByHour" Text='<%# Bind("UserPerHourPrice","{0:F}") %>'
                                    runat="server" />
                            </td>
                            <td>
                                Taxa de Configuração:
                                <br />
                                <uc1:CurrencyField ID="ucCurrFieldSetupFee" Mask="999.999.999" Text='<%# Bind("SetupFee","{0:F}") %>'
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Preço por Produto:
                                <br />
                                <uc1:CurrencyField Mask="999.999.999" ID="ucCurrFieldProductPrice" Text='<%# Bind("ProductPrice","{0:F}") %>'
                                    runat="server" />
                                <br />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsActive" Checked='<%# Bind("IsActive") %>' Text="Ativo ?" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: right">
                        <asp:Button ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                            CssClass="cBtn11" Text="Inserir" Visible="<%# frmPackage.CurrentMode == FormViewMode.Insert %>">
                            <%--permissionRequired="Package">--%></asp:Button>
                        <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                            CssClass="cBtn11" Text="Salvar" Visible="<%# frmPackage.CurrentMode == FormViewMode.Edit %>">
                            <%--permissionRequired="Package">--%></asp:Button>
                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            CssClass="cBtn11" Text="Cancelar"></asp:Button></div>
                </EditItemTemplate>
            </asp:FormView>
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
<VFX:BusinessManagerDataSource ID="odsPackage" runat="server" ConflictDetection="CompareAllValues"
    DataObjectTypeName="Vivina.InfoControl.DataClasses.Package" InsertMethod="InsertRetrievingID"
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetPackages" TypeName="Vivina.InfoControl.BusinessRules.PackagesManager"
    UpdateMethod="Update" OnInserted="odsPackage_Inserted" OnInserting="odsPackage_Inserting"
    OnSelecting="odsPackage_Selecting" OnUpdated="odsPackage_Updated" OnUpdating="odsPackage_Updating">
    <UpdateParameters>
        <asp:Parameter Name="original_entity" Type="Object" />
        <asp:Parameter Name="entity" Type="Object" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="PackageId" Type="Int32" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
