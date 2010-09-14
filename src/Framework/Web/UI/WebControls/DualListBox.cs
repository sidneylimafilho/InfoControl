using System;
using System.Data;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace InfoControl.Web.UI.WebControls
{
    [Description("User can carry items to and remove items from the right list")]
    public class DualListBox : System.Web.UI.WebControls.ListControl
    {
        #region Properties


        [Description("...")]
        public string InsertImageUrl
        {
            get
            {
                return (string)ViewState["InsertImageUrl"] ?? String.Empty;
            }
            set { ViewState["InsertImageUrl"] = value; }
        }

        [Description("...")]
        public string RemoveImageUrl
        {
            get
            {
                return (string)ViewState["RemoveImageUrl"] ?? String.Empty;
            }
            set { ViewState["RemoveImageUrl"] = value; }
        }

        [Description("...")]
        public string InsertAllImageUrl
        {
            get
            {
                return (string)ViewState["InsertAllImageUrl"] ?? String.Empty;
            }
            set { ViewState["InsertAllImageUrl"] = value; }
        }

        [Description("...")]
        public string RemoveAllImageUrl
        {
            get
            {
                return (string)ViewState["RemoveAllImageUrl"] ?? String.Empty;
            }
            set { ViewState["RemoveAllImageUrl"] = value; }
        }

        /// <summary>
        ///  Gets or sets the object from which the data-bound control retrieves its Left ListBox
        ///  of data items.
        /// </summary>
        [Description("...")]
        public object LeftDataSource
        {
            get { return DataSource; }
            set { DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the control from which the data-bound control retrieves
        ///     its Left ListBox of data items.
        /// </summary>
        [Description("...")]
        public string LeftDataSourceID
        {
            get { return DataSourceID; }
            set { DataSourceID = value; }
        }

        [Description("...")]
        public string LeftDataTextField
        {
            get { return DataTextField; }
            set { DataTextField = value; }
        }

        [Description("...")]
        public string LeftDataValueField
        {
            get { return DataValueField; }
            set { DataValueField = value; }
        }

        /// <summary>
        ///  Gets or sets the object from which the data-bound control retrieves its Right ListBox
        ///  of data items.
        /// </summary>
        [Description("...")]
        public object RightDataSource
        {
            get { return ViewState["RightDataSource"]; }
            set { ViewState["RightDataSource"] = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the control from which the data-bound control retrieves
        ///     its Right ListBox of data items.
        /// </summary>
        [Description("...")]
        public string RightDataSourceID
        {
            get { return (string)ViewState["RightDataSourceID"] ?? ""; }
            set { ViewState["RightDataSourceID"] = value; }
        }

        [Description("...")]
        public string RightDataTextField
        {
            get { return (string)ViewState["RightDataTextField"] ?? ""; }
            set { ViewState["RightDataTextField"] = value; }
        }

        [Description("...")]
        public string RightDataValueField
        {
            get { return (string)ViewState["RightDataValueField"] ?? ""; }
            set { ViewState["RightDataValueField"] = value; }
        }



        private ListItemCollection _selectedItems = new ListItemCollection();
        public ListItemCollection SelectedItems
        {
            get
            {
                return this._selectedItems;
            }
        }

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ParseRequest();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter w)
        {

            //TABLE
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
            w.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
            w.AddAttribute("attach", "dualListBox");
            w.RenderBeginTag(HtmlTextWriterTag.Table);

            w.RenderBeginTag(HtmlTextWriterTag.Tr);


            //
            // Left Cell
            //
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, "50%");
            w.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region render first select item
            //attributes
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            w.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            w.AddAttribute(HtmlTextWriterAttribute.Multiple, "true");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + ":leftSelect");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":leftSelect");
            w.RenderBeginTag(HtmlTextWriterTag.Select);
            foreach (ListItem item in Items)
            {
                w.AddAttribute(HtmlTextWriterAttribute.Value, item.Value);
                w.RenderBeginTag(HtmlTextWriterTag.Option);
                w.Write(item.Text);
                w.RenderEndTag();
            }
            w.RenderEndTag();

            #endregion
            w.RenderEndTag();//</td>


            //
            // Center Cell
            //
            w.AddAttribute("width", "1px");
            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region Buttons
            #region InsertAll
            w.AddAttribute(HtmlTextWriterAttribute.Onclick, "DualListBox_FireEvent(this)");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + ":InsertAll");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":InsertAll");
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            if (String.IsNullOrEmpty(InsertAllImageUrl))
            {
                w.AddAttribute(HtmlTextWriterAttribute.Value, ">>");
                w.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                w.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            else
            {
                w.AddAttribute(HtmlTextWriterAttribute.Src, InsertAllImageUrl);
                w.RenderBeginTag(HtmlTextWriterTag.Img);
            }
            w.RenderEndTag();
            w.WriteBreak();
            #endregion
            #region Insert
            w.AddAttribute(HtmlTextWriterAttribute.Onclick, "DualListBox_FireEvent(this)");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + ":Insert");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":Insert");
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            if (String.IsNullOrEmpty(InsertImageUrl))
            {
                w.AddAttribute(HtmlTextWriterAttribute.Value, ">");
                w.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                w.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            else
            {
                w.AddAttribute(HtmlTextWriterAttribute.Src, InsertImageUrl);
                w.RenderBeginTag(HtmlTextWriterTag.Img);
            }
            w.RenderEndTag();
            w.WriteBreak();
            #endregion
            #region Remove
            w.AddAttribute(HtmlTextWriterAttribute.Onclick, "DualListBox_FireEvent(this)");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + ":Remove");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":Remove");
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");

            if (String.IsNullOrEmpty(RemoveImageUrl))
            {
                w.AddAttribute(HtmlTextWriterAttribute.Value, "<");
                w.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                w.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            else
            {
                w.AddAttribute(HtmlTextWriterAttribute.Src, RemoveImageUrl);
                w.RenderBeginTag(HtmlTextWriterTag.Img);
            }

            w.RenderEndTag();
            w.WriteBreak();
            #endregion
            #region RemoveAll
            w.AddAttribute(HtmlTextWriterAttribute.Onclick, "DualListBox_FireEvent(this)");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + ":RemoveAll");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":RemoveAll");
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");

            if (String.IsNullOrEmpty(RemoveAllImageUrl))
            {
                w.AddAttribute(HtmlTextWriterAttribute.Value, "<<");
                w.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                w.RenderBeginTag(HtmlTextWriterTag.Input);
            }
            else
            {
                w.AddAttribute(HtmlTextWriterAttribute.Src, RemoveAllImageUrl);
                w.RenderBeginTag(HtmlTextWriterTag.Img);
            }

            w.RenderEndTag();
            w.WriteBreak();
            #endregion
            #endregion
            w.RenderEndTag();//</td>


            //
            // Right Cell
            //
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, "50%");
            w.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region render second select item
            //attributes
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            w.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            w.AddAttribute(HtmlTextWriterAttribute.Multiple, "true");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + ":rightSelect");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":rightSelect");
            w.RenderBeginTag(HtmlTextWriterTag.Select);
            foreach (ListItem item in _selectedItems)
            {
                w.AddAttribute(HtmlTextWriterAttribute.Value, item.Value);
                w.RenderBeginTag(HtmlTextWriterTag.Option);
                w.Write(item.Text);
                w.RenderEndTag();
            }
            w.RenderEndTag();
            #endregion
            w.RenderEndTag();//</td>



            w.RenderEndTag();//</tr>	
            w.RenderEndTag();//</table>

            WriteHiddenField();
        }

        /// <summary>
        /// Retrieve the selected Items in Hidden field and parse the values
        /// </summary>
        private void ParseRequest()
        {
            string requestItems = Page.Request[this.ClientID + ":hidden"];
            if (!String.IsNullOrEmpty(requestItems))
            {
                _selectedItems.Clear();

                foreach (string requestItem in requestItems.Split(','))
                {
                    if (!String.IsNullOrEmpty(requestItem.Trim()))
                    {
                        string value = requestItem.Split(':')[0].Trim();
                        string text = requestItem.Split(':')[1].Trim();

                        _selectedItems.Add(new ListItem(text, value));
                    }
                }
            }
        }

        private void WriteHiddenField()
        {
            StringBuilder strb = new StringBuilder();
            foreach (ListItem item in _selectedItems)
                strb.Append(item.Value + ":" + item.Text + ",");

            if (strb.Length > 0)
                strb = strb.Remove(strb.Length - 1, 1);

            //
            //hidden field in order to keep selected items..
            //
            Page.ClientScript.RegisterHiddenField(this.ClientID + ":hidden", strb.ToString());
        }

        private void PerformDataBindingLeftListBox(IEnumerable dataSource)
        {
            if (dataSource != null)
            {
                string dataTextField = this.LeftDataTextField;
                string dataValueField = this.LeftDataValueField;
                string dataTextFormatString = this.DataTextFormatString;
                if (!AppendDataBoundItems)
                    Items.Clear();

                foreach (object obj2 in dataSource)
                {
                    ListItem item = new ListItem();
                    if (dataTextField.Length != 0 || dataValueField.Length != 0)
                    {
                        if (dataTextField.Length > 0)
                        {
                            item.Text = DataBinder.GetPropertyValue(obj2, dataTextField, dataTextFormatString);
                        }
                        if (dataValueField.Length > 0)
                        {
                            item.Value = DataBinder.GetPropertyValue(obj2, dataValueField, null);
                        }
                    }
                    else
                    {
                        if (dataTextFormatString.Length != 0)
                        {
                            item.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, dataTextFormatString, new object[] { obj2 });
                        }
                        else
                        {
                            item.Text = obj2.ToString();
                        }
                        item.Value = obj2.ToString();
                    }
                    this.Items.Add(item);
                }
            }

        }

        private void PerformDataBindingRightListBox(IEnumerable dataSource)
        {
            if (dataSource != null)
            {
                string dataTextField = RightDataTextField;
                string dataValueField = RightDataValueField;
                string dataTextFormatString = DataTextFormatString;
                _selectedItems.Clear();

                foreach (object obj2 in dataSource)
                {
                    ListItem item = new ListItem();
                    if (dataTextField.Length != 0 || dataValueField.Length != 0)
                    {
                        if (dataTextField.Length > 0)
                        {
                            item.Text = DataBinder.GetPropertyValue(obj2, dataTextField, dataTextFormatString);
                        }
                        if (dataValueField.Length > 0)
                        {
                            item.Value = DataBinder.GetPropertyValue(obj2, dataValueField, null);
                        }
                    }
                    else
                    {
                        if (dataTextFormatString.Length != 0)
                        {
                            item.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, dataTextFormatString, new object[] { obj2 });
                        }
                        else
                        {
                            item.Text = obj2.ToString();
                        }
                        item.Value = obj2.ToString();
                    }

                    _selectedItems.Add(item);
                }
            }

        }

        public override void DataBind()
        {
            IDataSource dataSource;
            Control control;

            // 
            // Perform left
            //
            if (!String.IsNullOrEmpty(LeftDataSourceID))
            {
                control = this.FindControl(LeftDataSourceID);
                if (control == null)
                {
                    throw new System.Web.HttpException(String.Format(Properties.Resources.DataControl_DataSourceDoesntExist, this.ID, LeftDataSourceID));
                }
                dataSource = control as IDataSource;
                if (dataSource == null)
                {
                    throw new System.Web.HttpException(String.Format(Properties.Resources.DataControl_DataSourceIDMustBeDataControl, this.ID, LeftDataSourceID));
                }

                dataSource.GetView(DataMember).Select(DataSourceSelectArguments.Empty, new DataSourceViewSelectCallback(PerformDataBindingLeftListBox));
            }
            else if (LeftDataSource is IEnumerable)
            {
                PerformDataBindingLeftListBox(LeftDataSource as IEnumerable);
            }






            // 
            // Perform Right
            // 

            if (!String.IsNullOrEmpty(RightDataSourceID))
            {
                control = this.FindControl(RightDataSourceID);
                if (control == null)
                {
                    throw new System.Web.HttpException(String.Format(Properties.Resources.DataControl_DataSourceDoesntExist, this.ID, RightDataSourceID));
                }

                dataSource = control as IDataSource;
                if (dataSource == null)
                {
                    throw new System.Web.HttpException(String.Format(Properties.Resources.DataControl_DataSourceIDMustBeDataControl, this.ID, RightDataSourceID));
                }
                dataSource.GetView(DataMember).Select(DataSourceSelectArguments.Empty, new DataSourceViewSelectCallback(PerformDataBindingRightListBox));
            }
            else if (RightDataSource is IEnumerable)
            {
                PerformDataBindingRightListBox(RightDataSource as IEnumerable);
            }

        }
    }
}