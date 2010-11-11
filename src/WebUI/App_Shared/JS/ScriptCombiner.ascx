<%@ Control Language="C#" AutoEventWireup="true" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/javascript";

        Server.Execute("jquery.js", Response.Output);
        
        Server.Execute("jquery.cookies.js", Response.Output);
        Server.Execute("jquery.dimensions.js", Response.Output);
        Server.Execute("jquery.template.js", Response.Output);

        Server.Execute("jquery.jGrowl.js", Response.Output);
        Server.Execute("jquery.meioMask.js", Response.Output);
        Server.Execute("jquery.validate.js", Response.Output);
        Server.Execute("jquery.tooltip.js", Response.Output);
        Server.Execute("jquery.serializer.js", Response.Output);
        Server.Execute("jquery.template.js", Response.Output);

        Server.Execute("jquery.UI.core.js", Response.Output);
        Server.Execute("jquery.UI.widget.js", Response.Output);
        Server.Execute("jquery.UI.autocomplete.js", Response.Output);
        Server.Execute("jquery.UI.duallistbox.js", Response.Output);
        Server.Execute("jquery.ui.datepicker.js", Response.Output);

        Server.Execute("smartclient/jquery.smartclient.js", Response.Output);

    }
</script>
