<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_SelectTransporter"
    CodeBehind="SelectTransporter.ascx.cs" %>
<%@ Register Src="~/App_Shared/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<asp:UpdatePanel ID="upPanelTransporter" runat="server">
    <ContentTemplate>
        <table width="100%" id="pnlTransporterSearch" runat="server">
            <tr>
                <td>
                    Transportadora:<br />
                    <asp:TextBox ID="txtTransporter" runat="server" Width="300px" CssClass="cDynDat11"
                        AutoPostBack="True" OnTextChanged="txtTransporter_TextChanged" MaxLength="100"
                          plugin="autocomplete"
                          source='~/InfoControl/SearchService.svc/FindTransporter'
                          options="{max: 10}"> </asp:TextBox>
                        
                    
                  
                  <p style="font-size: 7pt; color: gray">
                        
                Dica: Digite parte do texto, que o completará automaticamente!</p>
                </td>
                <td>
                    <img id="Img1" src="~/App_Themes/_global/Company/user_add.gif" runat="server" alt="Inserir Transportadora"
                        border="0" onclick="top.tb_show('Cadastro de Transportadora','Administration/Transporter.aspx?w=modal);" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlTransporter" runat="server" Visible="false">
            <table border="0" width="100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkTransporterName" runat="server"></asp:LinkButton>
                        <asp:Label ID="lblSeparator" runat="server" Text="/"></asp:Label>
                        <asp:Label ID="lblCNPJ" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblTransporterAddress" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblTransporterLocalization" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblTransporterPhone" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;<img src="<%=ResolveUrl("~/App_Themes/_global/p_univ.gif")%>"
                            onclick="$('#<%=pnlTransporterSearch.ClientID %>').show(); " />
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
