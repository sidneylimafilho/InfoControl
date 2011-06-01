<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_Comments" CodeBehind="Comments.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls" TagPrefix="VFX" %>
<span id="commentIframeholder" runat="server"></span>
<fieldset id="commentForm" runat="server">
    <legend>Comentários:</legend><a id="comments"></a>
    <div id="commentBody">
        <asp:DataList ID="dtlComments" Width="100%" runat="server" DataSourceID="odsComments">
            <ItemTemplate>
                <p>
                    <%# Eval("Description") %></p>
                <span class="comment-footer"><span class="cTxt21">
                    <%# Eval("Website") != null ? "<a href='" + Eval("Website") + "'>" + Eval("UserName") + "</a>" : Eval("UserName")%>
                    &raquo;
                    <%# Eval("CreatedDate") %></span> </span>
                <%# Eval("FileUrl") != null ? " &raquo; <a href='" +  ResolveUrl(Eval("FileUrl").ToString()) + "' target='blank' class='file'>" + Eval("FileName") + "</a>" : "" %>
            </ItemTemplate>
        </asp:DataList>
        <table width="100%">
            <tr id="namePanel" runat="server">
                <td align="left">
                    Nome:<br />
                    <asp:TextBox ID="txtName" CssClass="cDat11" runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ControlToValidate="txtName" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <br />
                    E-mail:<br />
                    <asp:TextBox ID="txtMail" CssClass="cDat11 " runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="cErr21" runat="server" ControlToValidate="txtMail" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator CssClass="cErr21" runat="server" ControlToValidate="txtMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="E-mail inválido!"></asp:RegularExpressionValidator>
                    <br />
                    Website:<br />
                    <asp:TextBox ID="txtSite" CssClass="cDat11" runat="server" Width="200px"></asp:TextBox>
                    <br />
                    Comentários:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtDescription" CssClass="cDat11" runat="server" Height="100px"
                        TextMode="MultiLine" Width="99%" onfocus="clearInterval(intervalID);" onblur="Refresh();"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="cErr21" ID="valtxtDescription" runat="server" ControlToValidate="txtDescription"
                        ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;" ValidationGroup="saveComment" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:Label runat="server" ID="lblSelectFile" Text="Selecionar arquivo:"></asp:Label><br />
                    <asp:FileUpload ID="fupComments" runat="server" />
                    <br />
                </td>
            </tr>
            <tr id="buttonsPanel" runat="server">
                <td align="center">
                    <asp:Button runat="server" ID="btnInsert" CssClass="cBtn11" Text="Inserir" OnClick="btnInsert_Click" />
                    <asp:Button runat="server" ID="Button2" CssClass="cBtn11" Text="Cancelar" OnClientClick="top.$('#comments').hide(1000); return false;"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<VFX:BusinessManagerDataSource ID="odsComments" runat="server" TypeName="Vivina.Erp.BusinessRules.Comments.CommentsManager"
    OnSelecting="odsComments_Selecting" SelectMethod="GetComments">
    <SelectParameters>
        <asp:Parameter Name="SubjectId" Type="Int32" />
        <asp:Parameter Name="PageName" Type="String" />
    </SelectParameters>
</VFX:BusinessManagerDataSource>
