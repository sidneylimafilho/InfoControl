<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Commons_ScaleAccessLevel" Codebehind="ScaleAccessLevel.ascx.cs" %>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" 
    runat="server" 
    BehaviorID="InfoControl_ScaleAccessLevel"    
    TargetControlID="btnOk" 
    PopupControlID="InfoControl_ScaleAccessLevel_Container"
    OkControlID="btnOk" 
    CancelControlID="btnCancel" />
<div runat="server" class="modalPopup" id="InfoControl_ScaleAccessLevel_Container"
    style="width: 320px; height: 200px; display: none;">
    sfsfsf<asp:Button ID="btnOk" runat="server" Text="Elevar Permissões" /><asp:Button
        ID="btnCancel" runat="server" Text="Cancelar" /><table cellpadding="0" 
	cellspacing="0" style="width: 100%">
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
	</table>
</div>

<script type="text/javascript">
if(!window.InfoControl)window.InfoControl = {};
window.InfoControl.ScaleAccessLevel = function()
{
    this._isClosed = true;
    this._return = true;
}

InfoControl.ScaleAccessLevel.Hide = function(ret)
{    
    this._isClosed=true;
    $find('InfoControl_ScaleAccessLevel').hide();
}
InfoControl.ScaleAccessLevel.Show = function()
{
    this._isClosed=false;
    $find('InfoControl_ScaleAccessLevel').show();
}
</script>

