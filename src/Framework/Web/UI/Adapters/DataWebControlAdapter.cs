using System;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;
using InfoControl.Properties;

namespace InfoControl.Web.UI.Adapters
{
    public class DataWebControlAdapter : WebControlAdapter
    {
        public new DataPage Page
        {
            get
            {
                //
                // Caso não seja um Page nem começa a carregar o controle
                //
                if ((base.Page as DataPage) == null)
                    throw new Exception(Resources.DataUserControl_BadCaller);

                return base.Page as DataPage;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack && Page.DataMembers.Count > 0)
            {
                string dataMember = Control.Attributes["DataMember"];
                string dataField = Control.Attributes["DataField"];
                object dataObject = null;
                if (!String.IsNullOrEmpty(dataField))
                {
                    if (!String.IsNullOrEmpty(dataMember))
                    {
                        dataObject = Page.DataMembers[dataMember];
                    }
                    else
                    {
                        foreach (object member in Page.DataMembers.Values)
                        {
                            dataObject = member;
                            break;
                        }
                    }

                    object value = dataObject.GetPropertyValue<object>(dataField);

                    if (Control.GetType() == typeof (CheckBox))
                    {
                        ((CheckBox) Control).Checked = value != DBNull.Value;
                        (Control).Enabled = true;
                    }
                    else if (Control.GetType() == typeof (TextBox))
                    {
                        ((TextBox) Control).Text = (string) value;
                    }
                    else if (Control.GetType() == typeof (DropDownList))
                    {
                        ((DropDownList) Control).SelectedValue = (string) value;
                    }
                    else if (Control.GetType() == typeof (ListBox))
                    {
                        ((ListBox) Control).SelectedValue = (string) value;
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}