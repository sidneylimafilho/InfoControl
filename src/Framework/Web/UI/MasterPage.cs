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
    public class MasterPage : System.Web.UI.MasterPage
    {
        new public InfoControl.Web.UI.Page Page
        {
            get
            {
                return (base.Page as Page);
            }
            set
            {
                //
                // Caso não seja um Page nem começa a carregar o controle
                //
                if (!value.GetType().IsSubclassOf(typeof(Page)))
                    throw new Exception("O controle MasterPage só pode ser utilizado em uma página herdada de InfoControl.Web.UI.Page");

                base.Page = value;
            }
        }
    }
}
