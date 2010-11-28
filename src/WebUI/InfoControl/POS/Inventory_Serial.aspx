<%@ Page Title="" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Inventory_Serial.aspx.cs" Inherits="Vivina.Erp.WebUI.POS.Inventory_Serial" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<%@ Register Src="../../App_Shared/Date.ascx" TagName="Date" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                <table width="70%">
                    <tr>
                        <td>
                            Serial:<br />
                            <asp:TextBox ID="txtSerial" runat="server" MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="cErr21" ID="reqTxtSerial" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;"
                                ControlToValidate="txtSerial" ValidationGroup="saveInventorySerial"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Lote:<br />
                            <asp:TextBox ID="txtLot" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                        <td>
                            Data de vencimento:<br />
                            <uc4:date id="ucDtDueDate" ValidationGroup="saveInventorySerial" runat="server" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~\App_Shared/themes/glasscyan\Controls\GridView\img\Add2.gif"
                                AlternateText="Adicionar serial" OnClick="btnAdd_Click" ValidationGroup="saveInventorySerial" />
                        </td>
                    </tr>
                </table>
                <table width="70%">
                    <tr>
                        <td>
                            <asp:GridView ID="grdInventorySerial" runat="server" Width="100%" AutoGenerateColumns="False"
                                DataSourceID="odsInventorySerial" DataKeyNames="InventorySerialId"
                                AllowSorting="True" Rowselectable="false">
                                <Columns>
                                    <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
                                    <asp:BoundField DataField="Lot" HeaderText="Lote" SortExpression="Lot" />
                                    <asp:BoundField DataField="DueDate" HeaderText="Data de vencimento" SortExpression="DueDate" />
                                    <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;div class=&quot;delete&quot;title=&quot;excluir&quot;&lt;/div&gt;"
                                        SortExpression="Insert">
                                        <ItemStyle Width="1%" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
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
    
    <VFX:BusinessManagerDataSource ID="odsInventorySerial" runat="server" onselecting="odsInventorySerial_Selecting"
        SelectMethod="GetInventorySerials" TypeName="Vivina.Erp.BusinessRules.InventoryManager"
         EnablePaging="True" SelectCountMethod="GetInventorySerialsCount"
        SortParameterName="sortExpression" DeleteMethod="DeleteInventorySerial" >
        <DeleteParameters>
            <asp:Parameter Name="InventorySerialId" />
        </DeleteParameters>
        <selectparameters>
            <asp:Parameter Name="inventoryId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
