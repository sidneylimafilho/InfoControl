<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_Parcels" CodeBehind="Parcels.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="CurrencyField.ascx" TagName="CurrencyField" TagPrefix="uc1" %>
<%@ Register Src="Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Src="~/App_Shared/HelpTooltip.ascx" TagName="HelpTooltip" TagPrefix="vfx" %>
<table width="100%">
    <tr>
        <td colspan="4">
            <fieldset id="fdsParcels" runat="server">
                <legend>Parcelas</legend>
                <table align="center">
                    <tr>
                        <td>
                            Qtd:<br />
                            <uc1:CurrencyField ID="ucCurrFieldQtdParcel" Mask="999" ValidationGroup="AddParcel"
                                Required="true" runat="server" />
                        </td>
                        <td>
                            Total(R$):<br />
                            <uc1:CurrencyField ID="ucAmount" ValidationGroup="AddParcel" Required="true" runat="server" />
                        </td>
                        <td>
                            1º parcela:<br />
                            <uc2:Date ID="ucDtDueDate" Required="True" ValidationGroup="AddParcel" runat="server" />
                        </td>
                        <td>
                            Condição:<br />
                            <asp:DropDownList ID="cboPeriod" runat="server">
                                <asp:ListItem Value="30">Mensal</asp:ListItem>
                                <asp:ListItem Value="7">Semanal</asp:ListItem>
                                <asp:ListItem Value="10">Decenal</asp:ListItem>
                                <asp:ListItem Value="60">Bimensal</asp:ListItem>
                                <asp:ListItem Value="15">Quinzenal</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            Pagamento:<br />
                            <asp:DropDownList ID="cboPaymentMethod" runat="server" DataSourceID="odsPaymentMethod"
                                DataTextField="Name" DataValueField="PaymentMethodId">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboFinancierOperations" runat="server" ControlToValidate="cboPaymentMethod"
                                 ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="AddParcel">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            Conta Bancária:&nbsp; <VFX:HelpTooltip ID="HelpTooltip2" runat="server">
                                <ItemTemplate>
                                    <h2>
                                        Ajuda:</h2>
                              
                               Dados bancários são informações sobre os bancos com os quais a sua empresa trabalha, ou seja são os bancos onde a sua empresa tem uma conta.
                               Escolha aqui a conta bancária que sua empresa usará para realizar operações nesta conta.
                               
                                </ItemTemplate>
                            </VFX:HelpTooltip><br />
                            <asp:DropDownList ID="cboAccount" runat="server" DataSourceID="odsAccountType" DataTextField="ShortName"
                                DataValueField="AccountId" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>                       
                        <td style="text-align: right" nowrap="nowrap">
                            &nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnAddParcel" runat="server" ImageUrl="~\App_Shared/themes/glasscyan\Controls\GridView\img\Add2.gif"
                                ValidationGroup="AddParcel" OnClick="btnAddParcel_Click" CausesValidation="true" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grdParcel" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    EmptyDataText="Não há parcelas cadastradas para esta conta" Width="100%" DataKeyNames="ParcelId"
                    OnRowDeleted="grdParcel_RowDeleted" OnRowUpdating="grdParcel_RowUpdating" OnRowDeleting="grdParcel_RowDeleting"
                    RowSelectable="true" OnSelectedIndexChanging="grdParcel_SelectedIndexChanging"
                    OnRowCancelingEdit="grdParcel_RowCancelingEdit">
                    <Columns> 
                    <asp:TemplateField>
                        <ItemTemplate>     
                          <div align="center">
                          
                                  <asp:RadioButton ID="rbtBoleto" value='<%# Eval("ParcelId") %>' runat="server"
                                   Visible='<%# Convert.ToInt32(Eval("ParcelId")) != 0 && Eval("EffectedDate") == null  %>' onclick='event.cancelBubble=true;' />                                                                
                          </div>
                        </ItemTemplate>                    
                    </asp:TemplateField>
                        <asp:TemplateField HeaderText="Desc.">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vencimento">
                            <EditItemTemplate>
                                <uc2:Date ID="txtGrdDueDate" runat="server" Text='<%# Bind("DueDate", "{0:dd/MM/yyyy}") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("DueDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount", "{0:c}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pago">
                            <EditItemTemplate>
                                <uc1:CurrencyField ID="txtEffectedAmount" Text='<%# Eval("Amount") %>' runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("EffectedAmount", "{0:c}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quitada em">
                            <EditItemTemplate>
                                <uc2:Date ID="txtEffectedDate" runat="server" Text='<%# Bind("EffectedDate", "{0:dd/MM/yyyy}") %>'
                                    Required="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("EffectedDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pagamento">
                            <EditItemTemplate>
                                <asp:DropDownList ID="cboPaymentMethod" runat="server" DataSourceID="odsPaymentMethod"
                                    DataTextField="Name" DataValueField="PaymentMethodId" SelectedValue='<%# Bind("PaymentMethodId") %>'
                                    AppendDataBoundItems="true" >
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="cErr21" ID="valcboGrdPaymentMethod" runat="server" ControlToValidate="cboPaymentMethod"
                                     ErrorMessage="&nbsp;&nbsp;&nbsp;" Display="Dynamic" ValidationGroup="save">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                  <asp:Label Text=' <%# Eval("PaymentMethod.Name") %>' runat="server" /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Identificação">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtIdentificationNumber" runat="server" MaxLength="50" Text='<%# Bind("IdentificationNumber") %>'
                                    Columns="8"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblIdentificationNumber" runat="server" Text='<%# Bind("IdentificationNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Conta">
                            <EditItemTemplate>
                                <asp:DropDownList ID="cboAccountName" runat="server" DataSourceID="odsAccountType"
                                    DataTextField="ShortName" DataValueField="AccountId" SelectedValue='<%# Bind("AccountId") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%# Eval("Account.AccountNumber") %>
                                <%# Eval("Account.Bank.ShortName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Update" ToolTip="Salvar"
                                    CssClass="save"></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Cancel" ToolTip="Cancelar"
                                    CssClass="cancel" ValidationGroup="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton3" OnClientClick="event.cancelBubble=true;" runat="server"
                                    CommandName="Delete" ToolTip="Excluir" CssClass="delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </td>
    </tr>
</table>
<input type="hidden" id="parcelId" name="parcelId"> </input>
<VFX:BusinessManagerDataSource ID="odsPaymentMethod" runat="server" SelectMethod="GetAllPaymentMethod"
    TypeName="Vivina.Erp.BusinessRules.AccountManager">
</VFX:BusinessManagerDataSource>
<VFX:BusinessManagerDataSource ID="odsAccountType" runat="server" SelectMethod="GetAccountsWithShortName"
    TypeName="Vivina.Erp.BusinessRules.AccountManager" OnSelecting="dataSource_Selecting">
    <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
</VFX:BusinessManagerDataSource>
