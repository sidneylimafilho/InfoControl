using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Resources;
using System.Reflection;

using InfoControl.Data;

namespace InfoControl.Web.UI
{
    public class UserControl : System.Web.UI.UserControl
    {
        public UserControl()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            OnLoading(e);
            base.OnLoad(e);
            OnLoaded(e);
        }

        #region Properties
        new public InfoControl.Web.UI.Page Page
        {
            get
            {
                //
                // Caso não seja um Page nem começa a carregar o controle
                //
                if (!DesignMode && (base.Page as Page) == null)
                    throw new Exception(Properties.Resources.UserControl_BadCaller);

                return (base.Page as Page);
            }
            set
            {
                base.Page = value;
            }
        }

        #endregion

        #region Methods
        
        #endregion

        #region Events
        public event EventHandler Loading;
        /// <summary>
        /// Occurs before the page System.Web.UI.Control.Load event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoading(EventArgs e)
        {
            if (Loading != null)
                Loading(this, e);
        }

        public event EventHandler Loaded;
        /// <summary>
        /// Occurs after the page System.Web.UI.Control.Load event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoaded(EventArgs e)
        {
            if (Loaded != null)
                Loaded(this, e);
        }

        #endregion
       
    }
}
