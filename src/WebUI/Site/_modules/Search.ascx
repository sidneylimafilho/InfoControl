<%@ Control Language="C#" AutoEventWireup="true" %>
<table class='search'>
    <tr>
        <td>
            <input type='text' class='campo' id='txtSearch' />
        </td>
        <td>
            &nbsp;
            <input type="button" class="button" onclick="location='<%=ResolveUrl("~/site/products.aspx?txt=") %>' + document.getElementById('txtSearch').value; return false;" />
        </td>
    </tr>
</table>
