using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Resources;
using System.Reflection;


using InfoControl.Data;
using InfoControl.Web;
using InfoControl.Web.UI;
using InfoControl.Web.UI.WebControls;

namespace InfoControl.Web.UI
{
    public class DataMasterPage : System.Web.UI.MasterPage
    {
        new public DataPage Page
        {
            get
            {
                return (base.Page as DataPage);
            }
            set
            {
                base.Page = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //
            // Caso não seja um Page nem começa a carregar o controle
            //
            if (!DesignMode && Page == null)
                throw new Exception(Properties.Resources.MasterPage_BadCaller);

            base.OnLoad(e);
        }

    }
}
