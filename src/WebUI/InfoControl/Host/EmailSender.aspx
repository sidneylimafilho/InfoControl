<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="EmailSender.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Host.EmailSender" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h1>
        Envio de Email
    </h1>
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
                <table width="100%" id="tbEmail" action='<%=ResolveUrl("~/controller/host/SendEmail") %>'
                    onsucess="alert('Enviado com sucesso!');" >
                    <tr>
                        <td>
                            Para:
                            <br />
                            <asp:TextBox runat="server" ID="txtReceiver" param="receiver" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Assunto:
                            <br />
                            <asp:TextBox runat="server" ID="txtSubject" MaxLength="100" Width="200px" param="subject" />
                            
                            <asp:RequiredFieldValidator ID="reqTxtSubject" runat="server" ControlToValidate="txtSubject"
                                ErroMessage="&nbsp&nbsp&nbsp" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Mensagem:
                            <br />
                            <input type="text" plugin="htmlbox" param="message" style="width: 80%; height: 200px"
                                id="txtMessage" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                                <asp:Button ID="btnSendMail" runat="server" OnClientClick="return false;" Text="Enviar"
                                    command="sendEmail" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
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
