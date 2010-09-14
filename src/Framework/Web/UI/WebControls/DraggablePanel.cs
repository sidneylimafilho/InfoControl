using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{

    [ToolboxData("<{0}:DraggablePanel runat=server></{0}:DraggablePanel>")]
    public class DraggablePanel : Panel
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Adiciona os javascript que serão utilizados nessa página
            Page.ClientScript.RegisterClientScriptInclude("_Global", StringResources.ClientScriptFilesPath + "_Global.js");
            Page.ClientScript.RegisterClientScriptInclude("_Config", StringResources.ClientScriptFilesPath + "_Config.js");
            Page.ClientScript.RegisterClientScriptInclude("Window", StringResources.ClientScriptFilesPath + "Window.js");
            Page.ClientScript.RegisterClientScriptInclude("Window.DraggablePanel", StringResources.ClientScriptFilesPath + "Window.DraggablePanel.js");

            Attributes["onmouseup"] = "Draggable_onDrop(this, event);";
            Attributes["onmousedown"] = "Draggable_onDrag(this, event);";

        }

    }
}
