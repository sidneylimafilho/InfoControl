<%@ Page Title="Companias" Language="C#" MasterPageFile="~/InfoControl/Default.master"
    AutoEventWireup="true" CodeBehind="Companies.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Host.Companies" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
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
                        
              <fieldset id="filter" class="closed" onmouseouts='$("#filter .body").toggle(); $("#filter").attr({className:"closed"})'>
                        <legend onmouseover='$("#filter .body").show("slow"); $("#filter").attr({className:"open"})'>
                            Escolha o filtro desejado: </legend><div class="body">
                            
                            <table width="80%"> 
                                 <tr>                                    
                                       <td> 
                                         CNPJ: <br>
                                        <asp:TextBox runat="server" ID="txtCnpj"  Mask="99.999.999/9999-99" /> 
                                       </td>
                                   
                                       <td> 
                                          Nome: <br>
                                          <asp:TextBox runat="server" ID="txtCompanyName" />
                                       
                                       </td>           
                                       
                                       <td> 
                                          Email: <br>
                                          <asp:TextBox runat="server" ID="txtEmail" />
                                       
                                       </td>                              
                                 </tr>                            
                            </table>
                            <br />
                            <br />
                            <div align="right"> 
                               <asp:Button runat="server" ID="btnSearch" Text="Pesquisar" 
                                    onclick="btnSearch_Click" />
                            </div>
                            
                             </div><span class="closeButton" onmouseover='$("#filter .body").hide(500, function(){$("#filter").attr({className:"closed"})})'>
                                &nbsp;</span>
                        <br />
                        <br />
             </fieldset>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        
                        <asp:GridView Width="100%" RowSelectable="false" DataSourceID="odsCompanies" runat="server" ID="grdCompanies" AutoGenerateColumns="false" 
                            AllowSorting="True" DataKeyNames="CompanyId" onrowdatabound="grdCompanies_RowDataBound"> 
                            <Columns> 
                            
                                <asp:BoundField DataField="CNPJ" HeaderText="CNPJ" SortExpression="Cnpj" />
                                
                                <asp:BoundField DataField="CompanyName" HeaderText="Nome" SortExpression="CompanyName" />
                                <asp:BoundField DataField="NumberUsers" HeaderText="Número de Usuários" SortExpression="NumberUsers" />
                                <asp:BoundField DataField="LastActivityDate" HeaderText="Último login" SortExpression="LastActivityDate" />
                                
                                <asp:TemplateField HeaderText="Excluir">
                                <ItemTemplate>
                                    <input type="checkbox" name="chkCompany" onclick="event.cancelBubble=true" value='<%# Eval("CompanyId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>                    
                            <EmptyDataTemplate> 
                              <p align="center"> Não há dados a serem exibidos.  </p>
                            
                            <% if(grdCompanies.Rows.Count == 0) btnDelete.Visible = false;  %>
                            </EmptyDataTemplate>    
                        </asp:GridView>
                        <br>
                        <br>
                        <div align="center">                        
                          <asp:Button runat="server" ID="btnDelete" Text="Excluir Companias Selecionadas" 
                                onclick="btnDelete_Click" />
                        </div>
            
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
    <VFX:BusinessManagerDataSource ID="odsCompanies" runat="server" TypeName="Vivina.Erp.BusinessRules.CompanyManager"
        SelectMethod="SearchCompanies" OnSelecting="odsCompanies_Selecting">
        <selectparameters>
            <asp:Parameter Name="htCompany" Type="Object" />
        </selectparameters>
    </VFX:BusinessManagerDataSource>
</asp:Content>
