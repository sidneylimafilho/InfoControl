using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;


using InfoControl.Web;

namespace InfoControl.Web.UI.WebControls
{
    public enum RoundedPanelSkin
    {
        Default,
        DefaultDarkGray,
        VS2005,
        XP_Blue,
        XP_Silver
    }

    [PersistChildren(true)]
    [ParseChildren(false)]
    [Designer(typeof(RoundedPanelDesigner))]
    [ToolboxData("<{0}:RoundedPanel runat=server></{0}:RoundedPanel>")]
    [ToolboxBitmap(typeof(Mirror))]
    public class RoundedPanel : WebControl
    {
        #region Properties

        #region Configuration
        [Browsable(true)]
        [Category("Configuration")]
        public bool Draggable
        {
            get { return (bool)(ViewState["Draggable"] ?? false); }
            set { ViewState["Draggable"] = value; }
        }


        [Browsable(true)]
        [Category("Configuration")]
        public string SkinPath
        {
            get
            {
                string path = (string)ViewState["SkinPath"] ?? StringResources.ClientFilesPath + "Scripts/Controls/RoundedPanel/Skins/";

                if (path.StartsWith("~"))
                    path = ResolveUrl(path);

                return path;
            }

            set { ViewState["SkinPath"] = value; }
        }

        [Browsable(true)]
        [Category("Configuration")]
        public RoundedPanelSkin SkinName
        {
            get { return (RoundedPanelSkin)(ViewState["SkinName"] ?? RoundedPanelSkin.Default); }
            set
            {
                if (!value.Equals(ViewState["SkinName"]))
                {
                    ViewState["CornerTopLeftImageUrl"] = null;
                    ViewState["TopImageUrl"] = null;
                    ViewState["CornerTopRightImageUrl"] = null;
                    ViewState["LeftImageUrl"] = null;
                    ViewState["RightImageUrl"] = null;
                    ViewState["CornerBottomLeftImageUrl"] = null;
                    ViewState["BottomImageUrl"] = null;
                    ViewState["CornerBottomRightImageUrl"] = null;
                }

                ViewState["SkinName"] = value;
            }
        }

        #endregion

        #region Appearance
        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string CornerTopLeftImageUrl
        {
            get { return (string)(ViewState["CornerTopLeftImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/01.gif"); }
            set { ViewState["CornerTopLeftImageUrl"] = value; }
        }



        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string TopImageUrl
        {
            get { return (string)(ViewState["TopImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/02.gif"); }
            set { ViewState["TopImageUrl"] = value; }
        }



        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string CornerTopRightImageUrl
        {
            get { return (string)(ViewState["CornerTopRightImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/03.gif"); }
            set { ViewState["CornerTopRightImageUrl"] = value; }
        }



        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string LeftImageUrl
        {
            get { return (string)(ViewState["LeftImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/04.gif"); }
            set { ViewState["LeftImageUrl"] = value; }
        }


        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string RightImageUrl
        {
            get { return (string)(ViewState["RightImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/06.gif"); }
            set { ViewState["RightImageUrl"] = value; }
        }


        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string CornerBottomLeftImageUrl
        {
            get { return (string)(ViewState["CornerBottomLeftImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/07.gif"); }
            set { ViewState["CornerBottomLeftImageUrl"] = value; }
        }

        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string BottomImageUrl
        {
            get { return (string)(ViewState["BottomImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/08.gif"); }
            set { ViewState["BottomImageUrl"] = value; }
        }


        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Appearance")]
        public string CornerBottomRightImageUrl
        {
            get { return (string)(ViewState["CornerBottomRightImageUrl"] ?? SkinPath + Enum.GetName(typeof(RoundedPanelSkin), SkinName) + "/09.gif"); }
            set { ViewState["CornerBottomRightImageUrl"] = value; }
        }



        [Browsable(true)]
        [Category("Appearance")]
        public string Title
        {
            get { return (string)(ViewState["Title"] ?? "Title"); }
            set { ViewState["Title"] = value; }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public string TitleCssClass
        {
            get { return (string)(ViewState["TitleCssClass"] ?? ""); }
            set { ViewState["TitleCssClass"] = value; }
        }

        #endregion

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Draggable)
            {
                // Adiciona os javascript que serão utilizados nessa página
                Page.ClientScript.RegisterClientScriptInclude("_Global", StringResources.ClientScriptFilesPath + "_Global.js");
                Page.ClientScript.RegisterClientScriptInclude("_Config", StringResources.ClientScriptFilesPath + "_Config.js");
                Page.ClientScript.RegisterClientScriptInclude("Window", StringResources.ClientScriptFilesPath + "Window.js");
                Page.ClientScript.RegisterClientScriptInclude("Window.DraggablePanel", StringResources.ClientScriptFilesPath + "Window.DraggablePanel.js");
            }

        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (Visible)
            {
                writer.Write(GetBeginTag());
                base.RenderControl(writer);
                writer.Write(GetEndTag());
            }
        }


        internal string GetBeginTag()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE Draggable_Event WIDTH=" + Width.ToString() + " height=" + Height.ToString() + " BORDER=0 CELLPADDING=0 CELLSPACING=0>");


            sb.Append("  <TR >");
            sb.Append("      <TD valign=top>CornerTopLeftImageUrl</TD>");
            sb.Append("      <TD TopImageUrl valign=top class=" + TitleCssClass + " nowrap>" + Title + "</TD>");
            sb.Append("      <TD valign=top>CornerTopRightImageUrl</TD>");
            sb.Append("  </TR>");


            sb.Append("  <TR>");
            sb.Append("      <TD LeftImageUrl><FONT></FONT></TD>");
            sb.Append("      <TD height=100% valign=top>");

            if (Draggable)
                sb.Replace("Draggable_Event", "onmouseup='Draggable_onDrop(this, event);' onmousedown='Draggable_onDrag(this, event);'");
            else
                sb.Replace("Draggable_Event", "");

            if (String.IsNullOrEmpty(CornerTopLeftImageUrl))
                sb.Replace("CornerTopLeftImageUrl", "");
            else
                sb.Replace("CornerTopLeftImageUrl", "<IMG SRC=\"" + ResolveUrl(CornerTopLeftImageUrl) + "\">");

            if (String.IsNullOrEmpty(CornerTopRightImageUrl))
                sb.Replace("CornerTopRightImageUrl", "");
            else
                sb.Replace("CornerTopRightImageUrl", "<IMG SRC=\"" + ResolveUrl(CornerTopRightImageUrl) + "\">");

            if (String.IsNullOrEmpty(TopImageUrl))
                sb.Replace("TopImageUrl", "");
            else
                sb.Replace("TopImageUrl", "style=\"background:url(" + ResolveUrl(TopImageUrl) + ") repeat-x; \"");

            if (String.IsNullOrEmpty(LeftImageUrl))
                sb.Replace("LeftImageUrl", "");
            else
                sb.Replace("LeftImageUrl", "style=\"background:url(" + ResolveUrl(LeftImageUrl) + ")\"");



            return (sb.ToString());
        }

        internal string GetEndTag()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("      </TD>");
            sb.Append("      <TD RightImageUrl><FONT></FONT></TD>");
            sb.Append("  </TR>");
            sb.Append("  <TR>");
            sb.Append("      <TD>CornerBottomLeftImageUrl</TD>");
            sb.Append("      <TD BottomImageUrl><FONT></FONT></TD>");
            sb.Append("      <TD>CornerBottomRightImageUrl</TD>");
            sb.Append("  </TR>");
            sb.Append("</TABLE>");


            if (String.IsNullOrEmpty(CornerBottomLeftImageUrl))
                sb.Replace("CornerBottomLeftImageUrl", "");
            else
                sb.Replace("CornerBottomLeftImageUrl", "<IMG SRC=\"" + ResolveUrl(CornerBottomLeftImageUrl) + "\">");

            if (String.IsNullOrEmpty(CornerBottomRightImageUrl))
                sb.Replace("CornerBottomRightImageUrl", "");
            else
                sb.Replace("CornerBottomRightImageUrl", "<IMG SRC=\"" + ResolveUrl(CornerBottomRightImageUrl) + "\">");

            if (String.IsNullOrEmpty(RightImageUrl))
                sb.Replace("RightImageUrl", "");
            else
                sb.Replace("RightImageUrl", "style=\"background:url(" + ResolveUrl(RightImageUrl) + ")\"");

            if (String.IsNullOrEmpty(BottomImageUrl))
                sb.Replace("BottomImageUrl", "");
            else
                sb.Replace("BottomImageUrl", "style=\"background:url(" + ResolveUrl(BottomImageUrl) + ")\"");


            return (sb.ToString());
        }
    }
}

