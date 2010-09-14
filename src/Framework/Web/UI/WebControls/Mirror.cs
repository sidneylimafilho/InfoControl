using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{
    /// <summary>
    /// Duplica um controle já existente na página
    /// </summary>    
    [ToolboxData("<{0}:Mirror runat=server></{0}:Mirror>")]
    [ToolboxBitmap(typeof(Mirror))]
    public class Mirror : WebControl
    {
        private string controlID = null;

        [Browsable(true)]
        [Category("Configuration")]
        public string ControlID
        {
            get { return controlID; }
            set { controlID = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (ControlID == null)
                throw new NullReferenceException("O ID de um controle é necessário para o Mirror replicar!");

            Control c = Parent.FindControl(ControlID);
            
            if (c == null)
                throw new NullReferenceException("O ControlID especificado não foi encontrado!");

            // Call the control's Render function in order to
            // generate the Mirror control's HTML.
            // This, in a nutshell is the mirroring process.
            c.RenderControl(writer);
        }
    }
}



