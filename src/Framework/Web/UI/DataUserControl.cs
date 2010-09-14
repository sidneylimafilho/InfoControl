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
    public class DataUserControl : UserControl, IDataAccessor
    {
        public DataUserControl()
        {
        }

        #region Properties
        new public InfoControl.Web.UI.DataPage Page
        {
            get
            {
                //
                // Caso não seja um Page nem começa a carregar o controle
                //
                if (!DesignMode && (base.Page as DataPage) == null)
                    throw new Exception(Properties.Resources.DataUserControl_BadCaller);

                return (base.Page as DataPage);
            }
            set
            {
                base.Page = value;
            }
        }

        #endregion

        #region Methods
        protected override void OnLoad(EventArgs e)
        {
            //
            // Caso não seja um Page nem começa a carregar o controle
            //
            if (!DesignMode && (base.Page as DataPage) == null)
                throw new Exception(Properties.Resources.DataUserControl_BadCaller);

            if (!IsPostBack)
                OnLoadingData(new EventArgs());

            base.OnLoad(e);

            if (!IsPostBack)
                OnLoadedData(new EventArgs());
        }
        
        #endregion

        #region Events

        public event EventHandler LoadingData;
        /// <summary>
        /// Disparado antes de carregar os dados nos controles
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoadingData(EventArgs e)
        {
            if (LoadingData != null)
                LoadingData(this, e);
        }

        public event EventHandler LoadedData;
        /// <summary>
        /// Disparado depois de carregar os dados nos controles
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoadedData(EventArgs e)
        {
            if (LoadedData != null)
                LoadedData(this, e);
        }
        #endregion

        #region IDataAccessor Members
        public DataManager DataManager
        {
            get
            {
                return Page.DataManager;
            }
        }
        #endregion
    }
}
