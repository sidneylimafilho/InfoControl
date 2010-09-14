using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using InfoControl.Data;

namespace InfoControl.Web.UI
{
    internal class ControlHelper
    {
        ControlCollection _controls;

        public ControlHelper(ControlCollection controls)
        {
            _controls = controls;
        }


        /// <summary>
        /// Limpa todos os controles da tela
        /// </summary>
        internal void ResetControls()
        {
            foreach (Control uc in _controls)
            {
                if (uc.ID != null)
                {
                    if (uc.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)uc).Checked = false;
                        ((CheckBox)uc).Enabled = true;
                    }
                    else if (uc.GetType() == typeof(TextBox))
                    {
                        ((TextBox)uc).Text = String.Empty;
                    }
                    else if (uc.GetType() == typeof(DropDownList))
                    {
                        ((DropDownList)uc).SelectedIndex = -1;
                    }
                    else if (uc.GetType() == typeof(ListBox))
                    {
                        ((ListBox)uc).Items.Clear();
                    }
                }
            }
        }

        

        /// <summary>
        /// Seta o conteudo de cada controle ligado a base de dados
        /// </summary>
        internal void DataBindControls(DataRow dr)
        {
            foreach (Control c in _controls)
            {

                if (c.GetType().IsSubclassOf(typeof(WebControl)))
                {
                    WebControl uc = (c as WebControl);
                    if (uc.Attributes["DataField"] != null)
                    {
                        string dataField = uc.Attributes["DataField"];

                        if (uc.GetType() == typeof(CheckBox))
                        {
                            ((CheckBox)uc).Checked = dr[dataField] != DBNull.Value;
                            ((CheckBox)uc).Enabled = true;
                        }
                        else if (uc.GetType() == typeof(TextBox))
                        {
                            ((TextBox)uc).Text = dr[dataField].ToString();
                        }
                        else if (uc.GetType() == typeof(DropDownList))
                        {
                            ((DropDownList)uc).SelectedValue = dr[dataField].ToString();
                        }
                        else if (uc.GetType() == typeof(ListBox))
                        {
                            ((ListBox)uc).SelectedValue = dr[dataField].ToString();
                        }

                    }
                }
            }
        }

        
    }
}
