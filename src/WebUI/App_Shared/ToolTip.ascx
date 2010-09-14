<%@ Control Language="C#" AutoEventWireup="true" Inherits="Commons_ToolTip" Codebehind="ToolTip.ascx.cs" %>
<div id="tip" runat="server" class="ToolTip_left">
    <h1>
        <asp:Literal ID="lblTitle" runat="server" Text="Info:"></asp:Literal>
    </h1>
    <p>
        <asp:Literal ID="lblMessage" runat="server" Text='Mensagem'></asp:Literal>
    </p>
    <table width="100%" id="footer" cellspacing="0">
        <tr>
            <td nowrap="nowrap" width="100%" align="center">
                <p class="persistentText">
                    <asp:CheckBox ID="btnPersist" runat="server" TextAlign="Right" Text="&nbsp;&nbsp;&nbsp;Não mostrar novamente!" /><asp:Label
                        ID="lblTimer" runat="server" Text="Esta informação fechará em XX segundos!"></asp:Label></p>
            </td>
            <td nowrap="nowrap">
                <span class="cBtn13">
                    <input type="button" runat="server" id="btnFechar" value="Fechar" /><span></span></span>
            </td>
        </tr>
    </table>
</div>
<ajaxToolkit:AnimationExtender ID="AnimationExtender2" runat="server" TargetControlID="tip">
    <Animations>
    <OnLoad>
    <Sequence>
        <FadeIn  Duration="0.2" maximumOpacity="0.85" />       
    </Sequence>
    </OnLoad>
    </Animations>
</ajaxToolkit:AnimationExtender>
<ajaxToolkit:AnimationExtender ID="AnimationExtender1" runat="server" TargetControlID="btnFechar">
    <Animations>
    <OnClick>
        <Sequence>
            <FadeOut AnimationTarget="tip" Duration="0.2" />
            <StyleAction AnimationTarget="tip" Attribute="display" Value="none" />                       
        </Sequence>
    </OnClick>
    </Animations>
</ajaxToolkit:AnimationExtender>

<script type="text/javascript">
var toolTip = new InfoControl.ToolTip($get("<%=tip.ClientID %>"), $get("<%=btnFechar.ClientID %>"), $get('<%=btnPersist.ClientID %>'));
</script>

