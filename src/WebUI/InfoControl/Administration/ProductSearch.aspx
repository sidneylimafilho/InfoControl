<%@ Page Language="C#" AutoEventWireup="true" Inherits="Product_Search"
    MasterPageFile="~/infocontrol/Default.master" Title="Pesquisa de Produtos" Codebehind="ProductSearch.aspx.cs" %>

<%@ Register Src="../../App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Assembly="Telerik.Web.UI"
    Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder" ID="content" runat="server">
   
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
                        <td valign="top" width="180px">
                            <div id="treRemove" runat="server" style="padding: 6px; text-decoration: underline;
                                cursor: pointer; text-align: center;">
                                <b>Remover seleção!</b></div>
                            <br />
                            Escolha a Categoria:
                            <radT:RadTreeView ID="treCategory" runat="server" DataFieldID="CategoryId" DataFieldParentID="ParentId"
                                DataTextField="Name" DataValueField="CategoryId" Height="400px" Width="100%"
                                OnNodeBound="treCategory_NodeBound" AutoPostBack="True">
                            </radT:RadTreeView>
                        </td>
                        <td valign="top" style="padding-left: 10px">
                            <!-- FIELDS -->
                            <table width="100%">
                                <tr>
                                    <td>
                                        Nome:<br />
                                        <asp:TextBox ID="txtName" runat="server" Columns="30" MaxLength="40"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td>
                                        Código:<br />
                                        <asp:TextBox ID="txtCode" runat="server" Columns="20" MaxLength="25"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Fabricante:<br />
                                        <asp:DropDownList ID="cboManufacturer" runat="server" DataSourceID="odsManufacturer"
                                            DataTextField="Name" DataValueField="ManufacturerId" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Status do Produto:<br />
                                        <asp:RadioButton ID="rbtStatusActive" runat="server" GroupName="Status" Text="Ativo" /><br />
                                        <asp:RadioButton ID="rbtStatusInactive" runat="server" GroupName="Status" Text="Inativo" />
                                    </td>
                                    <td>
                                        Quantidade:<br />
                                        <table>
                                            <tr>
                                                <td style="width: 118px">
                                                    De:<br />
                                                    <input id="btnDown2" class="cUpDown11" tabindex="200" type="button" value="-" />
                                                    <asp:TextBox ID="txtQuantityStart" runat="server" Columns="4" Width="20px"></asp:TextBox>
                                                    <input id="btnUp2" class="cUpDown11" tabindex="201" type="button" value="+" />
                                                    <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender3" runat="server" TargetButtonDownID="btnDown2"
                                                        TargetButtonUpID="btnUp2" TargetControlID="txtQuantityStart" Maximum="9999" Minimum="0"
                                                        Step="10" Width="15">
                                                    </ajaxToolkit:NumericUpDownExtender>
                                                    &nbsp;
                                                </td>
                                                <td valign="middle" style="width: 119px">
                                                    Até:<br />
                                                    <input id="btnDown1" class="cUpDown11" tabindex="202" type="button" value="-" />
                                                    <asp:TextBox ID="txtQuantityEnd" runat="server" Columns="4" Width="20px"></asp:TextBox>
                                                    <input id="btnUp1" class="cUpDown11" tabindex="203" type="button" value="+" />
                                                    <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" TargetButtonDownID="btnDown1"
                                                        TargetButtonUpID="btnUp1" TargetControlID="txtQuantityEnd" Maximum="9999" Minimum="0"
                                                        Step="10" Width="15">
                                                    </ajaxToolkit:NumericUpDownExtender>
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            Mínimo Requirido:<br />
                            <table width="100%">
                                <tr>
                                    <td>
                                        De:<br />
                                        <input id="btnDown3" class="cUpDown11" tabindex="204" type="button" value="-" />
                                        <asp:TextBox ID="txtMinimumStart" runat="server" Columns="4"></asp:TextBox>
                                        <input id="btnUp3" class="cUpDown11" tabindex="205" type="button" value="+" />
                                        <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender4" runat="server" TargetButtonDownID="btnDown3"
                                            TargetButtonUpID="btnUp3" TargetControlID="txtMinimumStart" Maximum="9999" Minimum="0"
                                            Step="10" Width="25">
                                        </ajaxToolkit:NumericUpDownExtender>
                                    </td>
                                    <td>
                                        Até:<br />
                                        <input id="btnDown" class="cUpDown11" tabindex="206" type="button" value="-" />
                                        <asp:TextBox ID="txtMinimumEnd" runat="server" Columns="4"></asp:TextBox>
                                        <input id="btnUp" class="cUpDown11" tabindex="207" type="button" value="+" />
                                        <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender2" runat="server" TargetButtonDownID="btnDown"
                                            TargetButtonUpID="btnUp" TargetControlID="txtMinimumEnd" Maximum="9999" Minimum="0"
                                            Step="10" Width="15">
                                        </ajaxToolkit:NumericUpDownExtender>
                                    </td>
                                    <td>
                                        Depósito:<br />
                                        <asp:DropDownList ID="cboDeposit" runat="server" DataSourceID="odsDeposit" DataTextField="Name"
                                            DataValueField="DepositId" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnSearch" runat="server" Text="Procurar" CssClass="cBtn11" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
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
    <VFX:BusinessManagerDataSource ID="odsCategories" runat="server" SelectMethod="GetCategoriesByCompany"
        TypeName="Vivina.Erp.BusinessRules.CategoryManager" OnSelecting="odsCategories_Selecting">
        <selectparameters>
        <asp:Parameter Name="companyId" Type="Int32" />
    </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsManufacturer" runat="server" onselecting="odsManufacturer_Selecting"
        SelectMethod="GetManufacturerByCompany" TypeName="Vivina.Erp.BusinessRules.ManufacturerManager">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsDeposit" runat="server" onselecting="odsDeposit_Selecting"
        SelectMethod="GetDepositByCompany" TypeName="Vivina.Erp.BusinessRules.DepositManager">
        <selectparameters>
            <asp:parameter Name="companyId" Type="Int32"></asp:parameter>
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
