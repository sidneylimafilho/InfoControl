using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;

namespace InfoControl.Web.UI.WebControls
{   
    [PersistChildren(true)]
    [ParseChildren(false)]
    [ToolboxData("<{0}:TooglePanel runat=server></{0}:TooglePanel>")]
    [Designer(typeof(TooglePanelDesigner))]
    [ToolboxBitmap(typeof(Mirror))]
    public class TooglePanel : System.Web.UI.WebControls.WebControl
    {
        string _t = "";

        [UrlProperty]
        [Browsable(true)]
        [Category("Configurations")]
        [DefaultValue("")]
        [EditorAttribute(typeof(ImageUrlEditor), typeof(UITypeEditor))]        
        public string Url
        {
            set { _t = value; }
            get { return _t; }

        }
    }
}
