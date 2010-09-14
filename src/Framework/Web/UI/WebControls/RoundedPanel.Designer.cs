using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;


namespace InfoControl.Web.UI.WebControls
{
    class RoundedPanelDesigner : ContainerControlDesigner
    {
        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            this.FrameStyle.BackColor = Color.White;
            this.FrameStyle.BorderColor = Color.LightGray;
            this.FrameStyle.BorderWidth = Unit.Pixel(1);

            RoundedPanel roundPanel = (RoundedPanel)Component;
            return (roundPanel.GetBeginTag() + base.GetDesignTimeHtml(regions) + roundPanel.GetEndTag());
        }

        /// <summary>
        /// Para não apresentar a barra cinza com o ID do objeto
        /// </summary>
        public override string FrameCaption
        {
            get
            {
                return null;
            }
        }
    }
}
