<%@ Page Language="C#" MasterPageFile="~/infocontrol/Default.master" AutoEventWireup="true"
    Inherits="Company_Products_Print" Title="Página Inicial" CodeBehind="BarCode_Print.aspx.cs" %>

<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
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
            
              <div style="width: 100%; margin-left: 10px; margin-top: 5px" class="noPrintable">
        Escolha o modelo da etiqueta a ser impressa:&nbsp;
        <asp:DropDownList ID="cboLabels" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboLabels_SelectedIndexChanged">
            <asp:ListItem Value="A4248">A4248 - 17,0 x 31,0</asp:ListItem>
            <asp:ListItem Value="A4249">A4249 - 15,0 x 26,0</asp:ListItem>
            <asp:ListItem Value="A4250">A4250 - 55,8 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4251">A4251 - 21,2 x 38,2</asp:ListItem>
            <asp:ListItem Value="A4254">A4254 - 25,4 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4255">A4255 - 31,0 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4256">A4256 - 25,4 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4260">A4260 - 38,1 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4262">A4262 - 33,9 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4263">A4263 - 38,1 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4267">A4267 - 288,5 x 200,0</asp:ListItem>
            <asp:ListItem Value="A4348">A4348 -	17,0 x 31,0</asp:ListItem>
            <asp:ListItem Value="A4349">A4349 -	15,0 x 26,0</asp:ListItem>
            <asp:ListItem Value="A4350">A4350 -	55,8 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4351">A4351 -	21,2 x 38,2</asp:ListItem>
            <asp:ListItem Value="A4354">A4354 -	25,4 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4355">A4355 -	31,0 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4356">A4356 -	25,4 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4360">A4360 -	38,1 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4361">A4361 -	46,5 x 63,5</asp:ListItem>
            <asp:ListItem Value="A4362">A4362 -	33,9 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4363">A4363 -	38,1 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4365">A4365 -	67,7 x 99,0</asp:ListItem>
            <asp:ListItem Value="A4367">A4367 -	288,5 x 200,0</asp:ListItem>
            <asp:ListItem Value="A4368">A4368 - 143,4 x 199,9</asp:ListItem>
            <asp:ListItem Value="A43370">A43370 - 32,83 x 69,66</asp:ListItem>
            <asp:ListItem Value="A433105">A433105 - 32,83 x 104,5</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br />    
    <VFX:PrintLabel runat="server" id="labels" LabelFormat="A4351">
        <itemtemplate>
    <b><%#(Container.DataItem as System.Data.DataRow)["Name"] %></b><br />   
    
     <VFX:BarCodeImage runat="server" id="sasd"           
         Width="120%" Height="55px" BarHeight="400" BarType='<%# (Container.DataItem as System.Data.DataRow)["BarCodeTypeId"] %>' 
        TextToEncode='<%# (Container.DataItem as System.Data.DataRow)["BarCode"] %>'></VFX:BarCodeImage>    
    </itemtemplate>
    </VFX:PrintLabel>   
    <asp:Panel ID="pnlError" runat="server">
        <h1><asp:Label ID="lblError" runat="server" Visible="false" Text="Não é possivel gerar código de barras para produtos com estoque negativo"></asp:Label></h1>
    </asp:Panel>
            
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
