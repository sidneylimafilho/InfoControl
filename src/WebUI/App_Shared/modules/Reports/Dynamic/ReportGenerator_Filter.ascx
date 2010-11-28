<%@ Control Language="C#" AutoEventWireup="true" Inherits="ReportGenerator_Filter"
    CodeBehind="ReportGenerator_Filter.ascx.cs" %>
<%@ Register Assembly="InfoControl" Namespace="InfoControl.Web.UI.WebControls"
    TagPrefix="VFX" %>
<table cellspacing="0" cellpadding="5" width="100%" border="0">
    <%--<tr>
        <td colspan="3">
            <h2>
                FILTROS</h2>
        </td>
    </tr>--%>
    <tr>
        <td class="cTxt11b" style="padding-right: 10px; width: 1%" valign="middle">
            <h1>
                4</h1>
        </td>
        <td colspan="7" valign="middle">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, FilterReportDescription %>" />
        </td>
    </tr>
</table>
<br />
<br />
<asp:UpdatePanel runat="server">
    <contenttemplate>
<table cellspacing="0" cellpadding="5" border="0" align="center">
    <tr>
        <td colspan="4" valign="top">
           
            &nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, CreateFilterBy %>" />
            <asp:DropDownList ID="cboColumns" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboColumns_SelectedIndexChanged">
            </asp:DropDownList>
            <%-- <div style="width: 220px; overflow-y: auto;">--%>
            <span ID="pnlFilter" runat="server" Width="100%">
            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Resource, Where %>" />:
            <br />
                <asp:DropDownList ID="lstFilters" Height="130px" runat="server" Width="100px" DataTextField="Name" DataValueField="ReportFilterTypeId">
                </asp:DropDownList>
                <asp:TextBox ID="txtValue" runat="server" Columns="20" MaxLength="9999"></asp:TextBox>
                <asp:RegularExpressionValidator ID="validator" runat="server" ControlToValidate="txtValue"
                    ErrorMessage="<%$ Resources: Resource, InvalidNumber %>" ValidationExpression="\d{0,999999999}" Display="Dynamic"
                    ValidationGroup="Adicionar"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtValue" ErrorMessage="<%$ Resources: Resource, DataRequired %>"
                        Display="Dynamic" ValidationGroup="Adicionar"></asp:RequiredFieldValidator>
            </span>
            <span ID="pnlList" runat="server">
            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources: Resource, BeIt %>" />:
            <br />
                <asp:CheckBoxList ID="chkFilterList" runat="server" BorderWidth="0" RepeatDirection="Vertical"
                    RepeatLayout="Flow">
                </asp:CheckBoxList>
                <br />
            </span>
            <%--</div>--%>
            <%--<div align="center">--%>
            
            <%--</div>--%>
            
        </td>
        <td style="padding:10px;" valign="top">
        <asp:ImageButton ID="btnAdicionar" runat="server" OnClick="btnAdicionar_Click" ValidationGroup="Adicionar" ImageUrl="~/App_Themes/GlassCyan/arrow_right.gif" />
        </td>
   
        <td colspan="4" valign="top" style="vertical-align: top;">
            <fieldset>
                <legend><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources: Resource, ListOfSelectedFilters %>" />: </legend>
                <br />
                
                    <asp:GridView ID="grdSelectedFilters" runat="server" AutoGenerateColumns="False"
                        ShowHeader="False" GridLines="None" Width="100%" CssClass="clean" OnRowDeleting="grdSelectedFilters_RowDeleting">
                        <RowStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:ButtonField CommandName="Delete" ImageUrl="~/App_Reports/images/funnel_delete.gif" ButtonType="Image" Visible="true" ></asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                
            </fieldset>
        </td>
    </tr>
</table>
</contenttemplate>
</asp:UpdatePanel>

<script type="text/javascript">

    function SelectColumn(column, name, value) {
        if (column.checked) {
            var hidden;
            hidden = document.getElementById("SelectedColumnName");
            hidden.value = name;
            hidden = document.getElementById("SelectedColumnId");
            hidden.value = value.split("|")[0];
            hidden = document.getElementById("SelectedColumnDataType");
            hidden.value = value.split("|")[1];
        }
        else {
            document.getElementById("SelectedColumnName").value = "";
            document.getElementById("SelectedColumnId").value = "";
            document.getElementById("SelectedColumnDataType").value = "";
            event.returnValue = true;
        }
    }
</script>

