using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;


namespace InfoControl.Web.UI.WebControls
{
    
    
    [ToolboxData("<{0}:DataCheckBox runat=server></{0}:DataCheckBox>")]
    [ToolboxBitmap(typeof(DataCheckBox))]
    public class DataCheckBox : CheckBox, IDataControl
    {
        #region IDataControl Members

        [Category("Data")]
        [Localizable(true)]
        [ToolboxItem(true)]
        [Description("Indica qual coluna representará na base de dados")]
        public string DataField
        {
            get
            {
                return ((string)ViewState["DataField"]);
            }
            set
            {
                ViewState["DataField"] = value;
            }
        }

        #endregion
    }
}
