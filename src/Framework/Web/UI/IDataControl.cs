using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace InfoControl.Web.UI
{
    public interface IDataControl
    {
        /// <summary>
        /// Indica qual coluna será ligado a base de dados
        /// </summary>        
        string DataField
        {
            set;
            get;
        }
    }
}
