<%@ Page Title="Compania" Language="C#" MasterPageFile="~/InfoControl/Default.master" AutoEventWireup="true"
    CodeBehind="Company.aspx.cs" Inherits="Vivina.Erp.WebUI.InfoControl.Host.Company" %>

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
            
             <table width="100%">  
             <tr> 
             
              <td>
                   Nome :<br>
                   <asp:TextBox runat="server" ID="txtCompanyName" />
              
              </td>
                 <td>
                  Plano:<br>
                   <asp:TextBox runat="server" ID="txtCompanyPlan" />
              
              </td>
                 <td>
                   Quantidade de Usuários :<br>
                   <asp:TextBox runat="server" ID="txtUserQuantity" />
              
              </td>
             
             </tr>
             <tr> 
                          
              <td>
                   Data Início :<br>
                   <asp:TextBox runat="server" ID="txtStartDate" />
              
              </td>
                 <td>
                  Último Login:<br>
                   <asp:TextBox runat="server" ID="txtLastActivityDate" />
              
              </td>
                 <td>
                   Email do Responsável :<br>
                   <asp:TextBox runat="server" ID="txtEmail" />              
              </td>
             
             </tr>
             
             </table>
             
             <div align="right"> 
             
                <asp:Button runat="server" id="btnToBack"  Text="Voltar" OnClientClick="location='Companies.aspx'; return false;" />
             
             
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
</asp:Content>
