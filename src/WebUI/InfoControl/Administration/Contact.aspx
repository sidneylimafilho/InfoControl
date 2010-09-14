<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Contact.aspx.cs" Inherits="Vivina.Erp.WebUI.Administration.Contact"
    Title="Untitled Page" %>

<%@ Register Src="../../App_shared/address/address.ascx" TagName="Address" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h1> 
     <asp:Literal runat="server" Visible="false" Text="Contato" ID="litTitle" ></asp:Literal>
    
    </h1>
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
                <%--Conteúdo--%>
                <table width="100%">
                    <tr>
                        <td>
                            Nome:<br />
                            <asp:TextBox ID="txtName" runat="server" Columns="34" MaxLength="50" ValidationGroup="Save" />
                            <asp:RequiredFieldValidator ID="reqTxtName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            E-mail:<br />
                            <asp:TextBox ID="txtMail" runat="server" Columns="20" MaxLength="50" ValidationGroup="Save" />
                            <asp:RegularExpressionValidator ID="reqTxtMail" runat="server" ControlToValidate="txtMail"
                                ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td> 
                         <td>
                            Skype:<br />
                            <asp:TextBox ID="txtSkype" runat="server" Columns="20" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Msn:<br />
                            <asp:TextBox ID="txtMsn" runat="server" Columns="20" MaxLength="50" />
                            <asp:RegularExpressionValidator ID="ValEmail" runat="server" ErrorMessage="&nbsp&nbsp&nbsp"
                                ControlToValidate="txtMsn" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            Telefone:<br />
                            <asp:TextBox ID="txtPhone" runat="server" Mask="phone" Columns="10" MaxLength="50" />
                            <asp:RequiredFieldValidator runat="server" ID="reqTxtPhone" ControlToValidate="txtPhone"
                                Display="Dynamic" ValidationGroup="InsertContact"
                                ErrorMessage="&nbsp;&nbsp;">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Telefone Celular:<br />
                            <asp:TextBox ID="txtCellPhone" runat="server" Mask="phone" Columns="10" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                         <td>
                            Telefone Adicional:<br />
                            <asp:TextBox ID="txtPhone2" runat="server" Mask="phone" Columns="10" MaxLength="50" />
                        </td>
                         <td>
                            Setor:<br />
                            <asp:TextBox ID="txtSector" runat="server" Columns="30" MaxLength="50" />
                        </td>
                    </tr>
                </table>
                <br />
                Observação:<br />
                <asp:TextBox ID="txtObservation" Width="100%" TextMode="MultiLine" Rows="6" runat="server"></asp:TextBox>
                <br />
                <uc1:Address ID="ucAddress" Required="false" runat="server" FieldsetTitle="Endereço:" />
                <br />
                <br />
                <div style="text-align: right">
                    <asp:Button ID="btnSave" runat="server" CausesValidation="True" CssClass="cBtn11"
                        Text="Salvar" ValidationGroup="Save" OnClick="btnSave_Click"></asp:Button>
                    
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancelar"></asp:Button> 
                </div>
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
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
