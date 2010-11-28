<%@ Control Language="C#" Inherits="ReportGenerator_Columns" CodeBehind="ReportGenerator_Columns.ascx.cs" %>
<table cellspacing="10" cellpadding="0" width="100%" border="0">
    <tr>
        <td align="left" valign="middle" style="padding-right: 10px; width: 1%">
            <h1>
                2</h1>
        </td>
        <td align="left" valign="middle">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, SelectColumnsInReportDescription %>" />
        </td>
        <td align="left" valign="middle" style="padding-right: 10px; width: 1%">
            <h1>
                3</h1>
        </td>
        <td align="left" valign="middle">
            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, SelectOrderOfColumnsInReportDescription %>" />
        </td>
    </tr>
    <tr>
        <td valign="middle" style="width: 50%" colspan="2">
            <img src="../images/garbage_empty.gif" border="0" style="cursor: pointer;" onclick="ClearColumns()" />
        </td>
        <td style="text-align: center; width: 50%;" colspan="2">
            <img id="btnSobeTela3" alt="Sobe" src="../../App_Themes/GlassCyan/arrow_up.gif" border="0" name="incluir"
                style="cursor: pointer;" onclick="MoveUpColumn()">&nbsp;&nbsp;&nbsp;
            <img id="btnDesceTela3" alt="Desce" src="../../App_Themes/GlassCyan/arrow_down.gif" border="0" name="incluir"
                style="cursor: pointer;" onclick="MoveDownColumn()"></a>
        </td>
    </tr>
    <tr>
        <td valign="top" align="left" style="border: gray 1px solid;" colspan="2">
            <div class="placeholdercontainer" id="placeHolderDisposition" style="overflow: auto;
                height: 150px; width: 100%;">
                <asp:CheckBoxList ID="chkColumns" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                    BorderWidth="0" DataTextField="Name" DataValueField="ReportColumnsSchemaId" RepeatLayout="Flow">
                </asp:CheckBoxList>
            </div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Selecione pelo menos uma coluna!"
                ControlToValidate="OrderedColumns" Display="None"></asp:RequiredFieldValidator>
        </td>
        <td valign="top" colspan="2" align="center">
            <asp:ListBox ID="OrderedColumns" SelectionMode="Multiple" runat="server" DataTextField="Name"
                DataValueField="ReportColumnsSchemaId" Style="width: 90%; height: 150px"></asp:ListBox>
        </td>
    </tr>
</table>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" />

<script type="text/javascript">
    var columnListName = "<%=chkColumns.ClientID %>";
    var columnPositionName = "<%=OrderedColumns.ClientID %>";
    var listView = document.getElementById(columnPositionName);
    if (listView.options.length > 0)
        listView.options[0].selected = true;

    function ClearColumns(container) {
        var root;
        if (arguments.length == 0) {
            root = document.getElementById(columnListName);
        }
        else {
            root = arguments[0];
        }


        for (var i = 0; i < root.childNodes.length; i++) {

            ClearColumns(root.childNodes[i]);
        }


        if (root.tagName == "INPUT") {
            root.checked = true;
            root.click();
        }
    }

    function MoveUpColumn() {
        var listView = document.getElementById(columnPositionName);
        var selectedOption, priorOption;

        for (var i = 0; i < listView.options.length; i++) {
            if (listView.options[i].selected) {
                if (i > 0) {
                    selectedOption = listView.options[i];
                    priorOption = listView.options[i - 1];
                    listView.options[i] = new Option(priorOption.text, priorOption.value);
                    listView.options[i - 1] = new Option(selectedOption.text, selectedOption.value);
                    listView.options[i - 1].selected = true;
                    break;
                }
            }
        }
        SaveOptionsInHidden();
    }

    function MoveDownColumn() {
        var listView = document.getElementById(columnPositionName);
        var selectedOption, nextOption;

        for (var i = 0; i < listView.options.length; i++) {
            if (listView.options[i].selected) {
                if (i < listView.options.length - 1) {
                    selectedOption = listView.options[i];
                    nextOption = listView.options[i + 1];
                    listView.options[i] = new Option(nextOption.text, nextOption.value);
                    listView.options[i + 1] = new Option(selectedOption.text, selectedOption.value);
                    listView.options[i + 1].selected = true;
                    break;
                }
            }
        }
        SaveOptionsInHidden();
    }

    function SelectColumn(column, name, value) {
        var listView = document.getElementById(columnPositionName);
        var hidden = document.getElementById("OrderedColumnsHidden");

        if (column.checked) {
            var option = new Option(name, value);
            option.selected = true;
            listView.options[listView.options.length] = option;
        }
        else {
            for (var i = 0; i < listView.options.length; i++) {
                if (listView.options[i].text == name)
                    listView.options[i] = null;
            }

            for (var i = 0; i < listView.options.length; i++) {
                listView.options[i].selected = true;
            }
        }

        SaveOptionsInHidden();
    }

    function SaveOptionsInHidden() {
        var listView = document.getElementById(columnPositionName);
        var hidden = document.getElementById("OrderedColumnsHidden");
        hidden.value = "";
        for (var i = 0; i < listView.options.length; i++) {
            hidden.value += listView.options[i].value + ",";
        }
    }

    function LoadSelectedOptions(container) {

        var root;
        var listView = document.getElementById(columnPositionName);

        if (arguments.length == 0) {
            root = document.getElementById(columnListName);
            for (var i = 0; i < listView.options.length; i++) {
                listView.options[i] = null;
            }
        }
        else {
            root = arguments[0];
        }


        for (var i = 0; i < root.childNodes.length; i++) {
            LoadSelectedOptions(root.childNodes[i]);
        }


        if (root.tagName == "INPUT") {
            if (root.checked) {
                root.onclick();
            }
        }
    }
</script>

