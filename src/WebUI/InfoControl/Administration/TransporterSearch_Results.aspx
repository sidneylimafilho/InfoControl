<%@ Page Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true" Inherits="InfoControl_Administration_TransporterSearch_Results"
    Title="Transportadoras" EnableEventValidation="false" Codebehind="TransporterSearch_Results.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
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
                <td class="left" style="height: 97px">
                    &nbsp;
                </td>
                <td class="center" style="height: 97px">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grdTransporters" runat="server" Width="100%" DataSourceID="odsSearchTransporter"
                                    AutoGenerateColumns="False"  RowSelectable = "false"
                                    DataKeyNames="TransporterId,CompanyName,CNPJ,Email,Phone" 
                                    
                                    AllowSorting="True" AllowPaging="True" PageSize="20" 
                                    onrowdatabound="grdTransporters_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="CompanyName" HeaderText="Nome" SortExpression="CompanyName" />
                                        <asp:BoundField DataField="CNPJ" HeaderText="CNPJ" SortExpression="CNPJ" />
                                        <asp:BoundField DataField="Email" HeaderText="E-Mail" SortExpression="Email" />
                                        <asp:BoundField DataField="Phone" HeaderText="Telefone" SortExpression="Phone" 
                                            ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="text-align: center">
                                            Não existem dados a serem exibidos<br />
                                            &nbsp;
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" 
                                    onclick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                    <!--conteudo-->
                </td>
                <td class="right" style="height: 97px">
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
        <VFX:BusinessManagerDataSource ID="odsSearchTransporter" runat="server" onselecting="odsSearchTransporter_Selecting"
            SelectMethod="SearchTransporters" 
            TypeName="Vivina.Erp.BusinessRules.TransporterManager" 
            SortParameterName="sortExpression" EnablePaging="True" 
            SelectCountMethod="SearchTransportersCount">
            <selectparameters>
                <asp:Parameter Name="htTransporter" Type="Object" />
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </selectparameters>
        </VFX:BusinessManagerDataSource>
    </div>
</asp:Content>
