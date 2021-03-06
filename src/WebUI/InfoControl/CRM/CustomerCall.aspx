<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" Inherits="InfoControl_Services_CustomerCall"
    Title="Chamado" CodeBehind="CustomerCall.aspx.cs" ValidateRequest="false" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/InfoControl/Administration/SelectCustomer.ascx" TagName="SelectCustomer"
    TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/Comments.ascx" TagName="Comments" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtCallNumber" runat="server" Columns="12" MaxLength="30"></asp:TextBox>
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
                <table>
                    <asp:UpdatePanel ID="upSelCustomer" runat="server">
                        <ContentTemplate>
                            <tr runat="server" id="tblAdditionalInformation">
                                <td valign="top" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <uc1:SelectCustomer ID="selCustomer" runat="server" OnSelectedCustomer="SelCustomer_SelectedCustomer" />
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator CssClass="cErr21" ID="reqSelCustomer" runat="server" ControlToValidate="selCustomer"
                                                    ErrorMessage="&amp;nbsp;&amp;nbsp;&amp;nbsp;" ValidationGroup="InsertCustomerCall"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" id="pnlEquipament" runat="server">
                                    <asp:Label ID="lblCustomerName" runat="server"> </asp:Label>
                                    <br />
                                    <br />
                                    <span>
                                        <br />
                                        <asp:Literal runat="server" Text="Equipamento"></asp:Literal><br>
                                        <asp:DropDownList ID="cboCustomerEquipments" DataSourceID="" runat="server" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="CustomerEquipmentId" Width="120px">
                                            <asp:ListItem Value="" Text="">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp&nbsp&nbsp&nbsp </span>
                                </td>
                                <td valign="top">
                                    Chamado Associado:<br />
                                    <asp:TextBox ID="txtCallNumberAssociated" runat="server" Columns="30" MaxLength="50"></asp:TextBox>
                                    <br />
                                    Setor:<br />
                                    <asp:TextBox ID="txtSector" runat="server" Columns="30" MaxLength="50" ValidationGroup=""></asp:TextBox>
                                </td>
                            </tr>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <tr id="pnlStatusTechnical" runat="server">
                        <td>
                            Tipo de Chamado:<br />
                            <asp:DropDownList ID="cboCustomerCallType" runat="server" AppendDataBoundItems="True"
                                DataTextField="Name" DataValueField="CustomerCallTypeId" Width="120px" DataSourceID="odsCustomerCallType">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboCustomerCallType" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="cboCustomerCallType" ValidationGroup="InsertCustomerCall"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="litTechnical" Text="Técnico:"> </asp:Literal>
                            <br />
                            <asp:DropDownList ID="cboTechnicalEmployee" DataValueField="EmployeeId" runat="server"
                                DataTextField="Name" AppendDataBoundItems="True" DataSourceID="odsTechnicalEmployee">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="litRepresentant" Text="Representante:"> </asp:Literal>
                            <br />
                            <asp:DropDownList runat="server" DataTextField="Name" DataSourceID="odsRepresentant"
                                DataValueField="RepresentantId" ID="cboRepresentant" AppendDataBoundItems="true">
                                <asp:ListItem Text="" Value="">    </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="litPriority" Text="Prioridade:"> </asp:Literal>
                            <ajaxToolkit:Rating ID="rtnPriority" runat="server" MaxRating="5" StarCssClass="ratingStar"
                                WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar"
                                ToolTip="Classificação" CurrentRating="3">
                            </ajaxToolkit:Rating>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Assunto:<br />
                            <asp:TextBox ID="txtSubject" Columns="70" runat="server" MaxLength="1024"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtSubject" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtSubject" ValidationGroup="InsertCustomerCall"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Status:<br />
                            <asp:DropDownList ID="cboCustomerCallStatus" runat="server" AppendDataBoundItems="True"
                                DataSourceID="odsCustomerCallStatus" DataTextField="Name" DataValueField="CustomerCallStatusId">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqcboCustomerCallStatus" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="cboCustomerCallStatus" ValidationGroup="InsertCustomerCall"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Literal runat="server" ID="litCreateDate"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            Descrição:<br />
                            <div style="overflow: auto; max-width: 900px; max-height: 450px">
                                <asp:Label runat="server" ID="lblDescription" Visible="false"></asp:Label></div>

                                <textarea plugin="htmlbox" runat="server" id="txtDescription" name="txtDescription"
                                style="width: 100%; height: 400px"  />

                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtDescription" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtDescription"  ValidationGroup="InsertCustomerCall"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc2:Comments Visible="false" ID="CommentsCustomerCall" runat="server" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="5">
                            <asp:UpdatePanel ID="upTechnicalEmployee" runat="server" Visible="false">
                                <ContentTemplate>
                                    <fieldset>
                                        <legend>Agendamento de Horario Técnico:</legend>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblInitialTime" runat="server" Text="In�cio:"></asp:Label><br />
                                                    <asp:TextBox ID="txtInitialTime" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="mskInitialTime" runat="server" TargetControlID="txtInitialTime"
                                                        CultureName="pt-BR" Mask="99/99/9999 99:99:99" ClearMaskOnLostFocus="true" MaskType="DateTime">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtInitialTime" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                                        ControlToValidate="txtInitialTime" ValidationGroup="SearchEmployee"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEndTime" runat="server" Text="Fim:"></asp:Label><br />
                                                    <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="mskEndTime" runat="server" TargetControlID="txtEndTime"
                                                        CultureName="pt-BR" Mask="99/99/9999 99:99:99" ClearMaskOnLostFocus="true" MaskType="DateTime">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <asp:RequiredFieldValidator CssClass="cErr21" ID="reqtxtEndTime" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                                        ControlToValidate="txtEndTime" ValidationGroup="SearchEmployee"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSearchEmployee" runat="server" Text="Pesquisar" ValidationGroup="SearchEmployee"
                                                        OnClick="btnSearchEmployee_Click" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTechnicalEmployee" runat="server" Text="Funcion�rio:"></asp:Label><br />
                                                    <asp:Panel ID="pnlShowTechnical" runat="server">
                                                        <asp:Label ID="lblSelectedEmployee" runat="server" Text=""></asp:Label>
                                                        &nbsp;&nbsp;<img id="imgUndoSelectedEmployee" alt="Desfazer" src="../../App_Shared/themes/glasscyan/undo.gif"
                                                            onclick="EditTechnical()" /></asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnGenerateServiceOrder" Visible="false" ValidationGroup="InsertCustomerCall"
                                runat="server" Text="Gerar O.S" OnClick="btnGenerateServiceOrder_Click" />
                        </td>
                        <td align="right" colspan="3">
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Salvar" OnClick="btnSave_Click" ValidationGroup="InsertCustomerCall" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" OnClick="btnCancel_Click" />
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
    <VFX:businessmanagerdatasource id="odsCustomerCallStatus" runat="server" selectmethod="GetCustomerCallStatus"
        typename="Vivina.Erp.BusinessRules.CustomerManager">
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource id="odsCustomerCallType" runat="server" selectmethod="GetCustomerCallTypes"
        typename="Vivina.Erp.BusinessRules.CustomerManager">
    </VFX:businessmanagerdatasource>
    <VFX:businessmanagerdatasource id="odsTechnicalEmployee" runat="server" selectmethod="GetActiveTechnicalEmployee"
        typename="Vivina.Erp.BusinessRules.HumanResourcesManager" onselecting="odsTechnicalEmployee_Selecting">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
        </selectparameters>
    </VFX:businessmanagerdatasource>
    <VFX:BusinessManagerDataSource ID="odsRepresentant" runat="server" TypeName="Vivina.Erp.BusinessRules.RepresentantManager"
        SelectMethod="GetRepresentantsByCompany" onselecting="odsRepresentant_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
         </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
