<%@ Control Language="C#" AutoEventWireup="true" Inherits="Commons_AlphabeticalPaging"
    CodeBehind="/AlphabeticalPaging.ascx" %>
<span class="ui-theme-table" style="border: 0px; width: 100%">
    <table cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td>
                <asp:LinkButton ID="lnkLetterA" runat="server" OnClick="lnkLetter_Click">A</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterB" runat="server" OnClick="lnkLetter_Click">B</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterC" runat="server" OnClick="lnkLetter_Click">C</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterD" runat="server" OnClick="lnkLetter_Click">D</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterE" runat="server" OnClick="lnkLetter_Click">E</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterF" runat="server" OnClick="lnkLetter_Click">F</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterG" runat="server" OnClick="lnkLetter_Click">G</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterH" runat="server" OnClick="lnkLetter_Click">H</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterI" runat="server" OnClick="lnkLetter_Click">I</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterJ" runat="server" OnClick="lnkLetter_Click">J</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterK" runat="server" OnClick="lnkLetter_Click">K</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterL" runat="server" OnClick="lnkLetter_Click">L</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterM" runat="server" OnClick="lnkLetter_Click">M</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterN" runat="server" OnClick="lnkLetter_Click">N</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterO" runat="server" OnClick="lnkLetter_Click">O</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterP" runat="server" OnClick="lnkLetter_Click">P</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterQ" runat="server" OnClick="lnkLetter_Click">Q</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterR" runat="server" OnClick="lnkLetter_Click">R</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterS" runat="server" OnClick="lnkLetter_Click">S</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterT" runat="server" OnClick="lnkLetter_Click">T</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterU" runat="server" OnClick="lnkLetter_Click">U</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterV" runat="server" OnClick="lnkLetter_Click">V</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterX" runat="server" OnClick="lnkLetter_Click">X</asp:LinkButton>
                <asp:LinkButton ID="lnlLetterW" runat="server" OnClick="lnkLetter_Click">W</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterY" runat="server" OnClick="lnkLetter_Click">Y</asp:LinkButton>
                <asp:LinkButton ID="lnkLetterZ" runat="server" OnClick="lnkLetter_Click">Z</asp:LinkButton>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                <asp:LinkButton ID="lnkNumber0" runat="server" OnClick="lnkLetter_Click">[0-9]</asp:LinkButton>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                <asp:LinkButton ID="lnkAllLetter" runat="server" OnClick="lnkLetter_Click">Todos</asp:LinkButton>
            </td>
        </tr>
    </table>
</span>