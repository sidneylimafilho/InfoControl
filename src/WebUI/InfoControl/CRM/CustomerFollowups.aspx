<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" Inherits="InfoControl_CRM_CustomerFollowups"
    MasterPageFile="~/InfoControl/Default.master" CodeBehind="CustomerFollowups.aspx.cs"
    Title="FollowUp" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Src="~/App_Shared/DateTimeInterval.ascx" TagName="DateTimeInterval"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
                    <!--Principal -->
                      <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend><div class="body">
                                <table width="80%">
                                    <tr>
                                        <td>
                                            Nome do Contato:<br />
                                            <asp:TextBox runat="server" ID="txtContactName"></asp:TextBox>
                                        </td>
                                        <td>
                                            Ação:<br />
                                            <asp:DropDownList runat="server" DataSourceID="odsCustomerFollowupAction" 
                                            DataTextField="Name" DataValueField="CustomerFollowupActionId" ID="cboCustomerFollowupAction" 
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            
                                            </asp:DropDownList>
                                        </td>
                                        <td>                                         
                                         <uc1:DateTimeInterval ID="ucDateTimeInterval" Required="true" ValidationGroup="search"  runat="server" />
                                        </td>
                                        
                                        <td align="right">
                                            Exibir:<br />
                                            <asp:DropDownList ID="cboPageSize" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <br />
                                            <asp:Button ID="btnSearch" runat="server"  ValidationGroup="search" Text="Filtrar" 
                                                onclick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                    </fieldset>
                    
                    <br>
                    <br>
                    <br>
                    <br>
                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        
                                    </tr>
                                </table>
                                <asp:GridView ID="grdCustomerFollowup" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="odsCustomerFollowup" RowSelectable="false" DataKeyNames="CustomerFollowupId,CompanyId,ContactId,EntryDate,Description,UserId"
                                    AllowSorting="True" OnRowDataBound="grdCustomerFollowup_RowDataBound"  Width="100%" AllowPaging="True" PageSize="20">  
                                    <Columns>
                                        <asp:BoundField DataField="ContactName" HeaderText="Contato" SortExpression="ContactName" />
                                        <asp:BoundField DataField="CustomerFollowupActionName" HeaderText="Ação" SortExpression="CustomerFollowupActionName" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="Data" SortExpression="EntryDate" ItemStyle-Width="1%" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                      
                                        <asp:TemplateField HeaderText="&lt;a href=&quot;CustomerFollowup.aspx&quot;&gt; &lt;div class=&quot;insert&quot; title=&quot;Inserir&quot;&gt;&lt;/div&gt;&lt;/a&gt;"
                                             ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div class="delete" title="Apagar" id='<%# Eval("CustomerFollowupId") %>'>
                                                    &nbsp;
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="1%"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div align="center">
                                            Não existem dados a serem exibidos, clique no botão para cadastrar um followUp.
                                            <br />
                                           <asp:Button ID="btnInsertNewCustomerFollowup" runat="server" Text="Cadastrar"
                                                  OnClientClick="location='CustomerFollowup.aspx'; return false;" />
                                        </div>
                                    </EmptyDataTemplate>
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
    </div>
    <VFX:BusinessManagerDataSource ID="odsCustomerFollowup" runat="server" ConflictDetection="CompareAllValues"
        SelectMethod="GetCustomerFollowups" TypeName="Vivina.Erp.BusinessRules.CustomerManager"
        OldValuesParameterFormatString="original_{0}" SortParameterName="sortExpression"
        SelectCountMethod="GetCustomerFollowupsCount" onselecting="odsCustomerFollowup_Selecting"
        EnablePaging="True" DataObjectTypeName="Vivina.Erp.DataClasses.CustomerFollowup"
        DeleteMethod="DeleteCustomerFollowup">
        <selectparameters>
            <asp:Parameter Name="companyId" Type="Int32" />
            <asp:Parameter Name="customerFollowupActionId" Type="Int32" />
            <asp:Parameter Name="dateTimeInterval" Type="Object" />
            
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="EntryDate" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
    <VFX:BusinessManagerDataSource ID="odsCustomerFollowupAction" runat="server" SelectMethod="GetCustomerFollowupActions"
        TypeName="Vivina.Erp.BusinessRules.CustomerManager" onselecting="odsCustomerFollowupAction_Selecting">
        <selectparameters>
                <asp:Parameter Name="companyId" Type="Int32" />
           </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
