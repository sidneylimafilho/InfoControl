<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    Inherits="AccessControl_EventInsertion" Title="Evento" EnableEventValidation="false"
    CodeBehind="EventAdd.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rbtError" runat="server" Text="Erro" GroupName="rbtsType" />
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtAlert" runat="server" Text="Alerta" GroupName="rbtsType" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtInformation" runat="server" Text="Informação" GroupName="rbtsType" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtSugestion" runat="server" Text="Sugestão de funcionalidade"
                                    GroupName="rbtsType" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <table id="tblPriority" runat="server">
                                    <tr>
                                        <td>
                                            Prioridade:
                                        </td>
                                        <td>
                                            <ajaxToolkit:Rating ID="rtnPriority" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                                WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                                ToolTip="Classificação" CurrentRating="3">
                                            </ajaxToolkit:Rating>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table id="tblProcess" runat="server" visible="true">
                        <tr>
                            <%--<td>
                                Técnico:<br />
                                <asp:DropDownList ID="cboTechnicalUser" runat="server" DataSourceID="odsTechnicalUser"
                                    DataTextField="Name" DataValueField="UserId" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            <td>
                                Status:<br />
                                <asp:DropDownList ID="cboEventStatus" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="odsEventStatus" DataTextField="Name" DataValueField="EventStatusId">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqcboEventStatus" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    ControlToValidate="cboEventStatus" ValidationGroup="saveEventAdd"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <table id="tblName" width="100%" runat="server" visible="false">
                        <tr>
                            <td>
                                Assunto:<br />
                                <asp:TextBox ID="txtName" runat="server" Columns="100" MaxLength="1024"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valTxtName" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                    ControlToValidate="txtName"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <table id="tblMessage" width="100%" runat="server" visible="false">
                        <tr>
                            <td>
                                Mensagem:<br />
                                <textarea runat="server" id="txtMessage" name="txtMessage" plugin="htmlbox"
                                    style="width: 100%; height: 150px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="dvException" runat="server" visible="false">
                        <h2>
                            Error Message:
                            <asp:Literal ID="ltrErrorMessage" runat="server"></asp:Literal>
                        </h2>
                        <br />
                        <b>Company Name:</b>
                        <asp:HyperLink ID="lnkOpenModal" runat="server"></asp:HyperLink>
                        <br />
                        <b>User:</b>
                        <asp:Literal ID="ltrUserName" runat="server"></asp:Literal>
                        <br />
                        <b>Path:</b>
                        <asp:Literal ID="ltrPath" runat="server"></asp:Literal><br />
                        <b>Data:</b>
                        <asp:Literal ID="ltrCurrentDate" runat="server"></asp:Literal><br />
                        <b>Stack Trace:</b><br />
                        <asp:Literal ID="ltrStackTrace" runat="server"></asp:Literal>
                    </div>
                    <br />
                    <uc1:Comments ID="Comments" Visible="false" runat="server" />
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSave" ValidationGroup="saveEventAdd" runat="server" Text="Salvar"
                                    OnClick="btnSave_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click"
                                    CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <VFX:BusinessManagerDataSource ID="odsEventStatus" runat="server" SelectMethod="GetAllEventStatus"
                        TypeName="Vivina.Erp.BusinessRules.EventManager">
                    </VFX:BusinessManagerDataSource>
                    <VFX:BusinessManagerDataSource ID="odsTechnicalUser" runat="server" SelectMethod="GetTechnicalUserAsDataTable"
                        TypeName="Vivina.Erp.BusinessRules.CompanyManager" onselecting="odsTechnicalUser_Selecting">
                        <selectparameters>
                            <asp:Parameter Name="companyId" Type="Int32" />
                        </selectparameters>
                    </VFX:BusinessManagerDataSource>
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
