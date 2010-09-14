using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using InfoControl.Web.UI;
using InfoControl.Web.Security;

namespace InfoControl.Web
{
    public static class WebControlsExtensions
    {
        /// <summary>
        /// Searches the page naming container for a server control with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier for the control to be found.</param>
        /// <param name="control">The owner control will to be found</param>
        /// <returns>The specified control, or null if the specified control does not exist.</returns>
        public static T FindControl<T>(this Control control, string id) where T : Control
        {
            foreach (Control uc in control.Controls)
            {
                if (uc.ID == id)
                    return ((T)uc);

                T candidate = FindControl<T>(uc, id);
                if (candidate != null)
                    return (candidate);
            }
            return null;
        }

        public static void EnsureSecurity(this WebControl control)
        {
            if (control.Page is UI.DataPage)
            {
                String permission = String.Empty;

                if (System.Web.SiteMap.CurrentNode != null)
                    permission = System.Web.SiteMap.CurrentNode[Properties.Resources.PermissionRequiredKey];

                if (String.IsNullOrEmpty(permission))
                {
                    object[] attributes = control.Page.GetType().GetCustomAttributes(typeof(PermissionRequiredAttribute), true);
                    if (attributes != null && attributes.Length > 0)
                    {
                        permission = (attributes[0] as PermissionRequiredAttribute).PermissionName;
                    }
                }

                if (String.IsNullOrEmpty(permission))
                    permission = control.Attributes[Properties.Resources.PermissionRequiredKey];

                if (!String.IsNullOrEmpty(permission))
                {
                    control.Visible = (control.Page as UI.DataPage).User.CanView(permission);
                    if (control.Visible)
                        control.Enabled = (control.Page as UI.DataPage).User.CanChange(permission);
                }
            }
        }

        /// <summary>
        /// Output server control content to html
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static string RenderToHtml(this WebControl control)
        {
            using (HtmlTextWriter writer = new HtmlTextWriter(new StringWriter()))
            {
                control.RenderControl(writer);
                return (writer.InnerWriter as StringWriter).ToString();
            }
        }
    }
}
