using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:IFrame runat=server></{0}:IFrame>")]
    [Designer(typeof(ControlDesigner))]
    [ToolboxBitmap(typeof(IFrame))]    
    public class IFrame : WebControl
    {

        public IFrame()
        {
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Src
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        public override Unit Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Table);//               <table>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);//                      <tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);//                          <td>
            writer.Write("Title");//                                                    Title
            writer.RenderEndTag(); //                                               </td>
            writer.RenderEndTag(); //                                           </tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);//                      <tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Td);//                          <td>
            writer.RenderEndTag(); //                                               </td>
            writer.RenderEndTag(); //                                           </tr>
            writer.RenderEndTag(); //                                       </table>

            base.Render(writer);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "");
            base.AddAttributesToRender(writer);
        }

    }

        
}
