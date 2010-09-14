using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoControl.Web.UI.WebControls
{

    [ToolboxData("<{0}:DataMaskedTextBox runat=server></{0}:DataMaskedTextBox>")]
    [ToolboxBitmap(typeof(DataMaskedTextBox))]
    public class DataMaskedTextBox : MaskedTextBox, IDataControl
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
