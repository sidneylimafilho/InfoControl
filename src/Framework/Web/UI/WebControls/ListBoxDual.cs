/*
<gm:listboxdual id="BizListBoxDual1" runat="server" 
	LeftImageUrl="picture\ToLeft.gif" RigthImageUrl="picture\ToRight.gif"></gm:listboxdual>
*/
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace InfoControl.Web.UI.WebControls
{
    [Description("User can carry items to and remove items from the right list")]
    public class ListBoxDual : System.Web.UI.WebControls.ListControl
    {
        #region Properties

        [Description("Size of html select elements"), DefaultValue(5)]
        public int Size
        {
            get
            {
                object o = ViewState["Value"];
                return (o == null ? 5 : Convert.ToInt32(ViewState["Value"]));
            }
            set { ViewState["Value"] = value; }
        }

        public int Width
        {
            get
            {
                object o = ViewState["Width"];
                return (o == null ? 100 : Convert.ToInt32(ViewState["Width"]));
            }
            set { ViewState["Width"] = value; }
        }

        [Description("...")]
        public string AddImageUrl
        {
            get
            {
                return (string)ViewState["AddImageUrl"] ?? String.Empty;
            }
            set { ViewState["AddImageUrl"] = value; }
        }

        [Description("...")]
        public string DeleteImageUrl
        {
            get
            {
                return (string)ViewState["DeleteImageUrl"] ?? String.Empty;
            }
            set { ViewState["DeleteImageUrl"] = value; }
        }

        [Description("...")]
        public string AddAllImageUrl
        {
            get
            {
                return (string)ViewState["AddAllImageUrl"] ?? String.Empty;
            }
            set { ViewState["AddAllImageUrl"] = value; }
        }

        [Description("...")]
        public string DeleteAllImageUrl
        {
            get
            {
                return (string)ViewState["DeleteAllImageUrl"] ?? String.Empty;
            }
            set { ViewState["DeleteAllImageUrl"] = value; }
        }

        public object DataSource
        {
            get
            {
                return ViewState["DataSource"];
            }
            set
            {
                if ((value == null) || (value is IListSource))	// || (datasource is IEnumerable))
                {
                    ViewState["DataSource"] = value;
                }
                else
                {
                    throw new ArgumentException("DataSource must be DataSet or DataTable");
                }
            }
        }


        public string DataTableName
        {
            get
            {
                return (string)ViewState["DataTableName"] ?? String.Empty;
            }
            set { ViewState["DataTableName"] = value; }
        }


        public string DataTextField
        {
            get
            {
                string str = (string)ViewState["DataTextField"];
                return (str == null ? string.Empty : str);
            }
            set { ViewState["DataTextField"] = value; }
        }


        public string DataValueField
        {
            get
            {
                string str = (string)ViewState["DataValueField"];
                return (str == null ? string.Empty : str);
            }
            set { ViewState["DataValueField"] = value; }
        }

        [Description("This attribute is set true when DataBind() method is called")]
        private bool DataBindingState
        {
            get
            {
                object o = ViewState["DataBindingState"];
                return (o == null ? false : Convert.ToBoolean(o));
            }
            set
            {
                ViewState["DataBindingState"] = value;
            }
        }

        public DataTable SelectedItems
        {
            get
            {
                string keyValueString = Page.Request.Form[this.ClientID + "_hidden"];

                //when page refreshed
                if (keyValueString == null)
                {
                    return null;
                }
                //when page submitted
                else if (keyValueString == string.Empty)
                {
                    ViewState["SelectedItems"] = null;
                    return null;
                }

                //select element has items
                string[] keyValue = keyValueString.Split(new char[] { ':' });
                DataTable dt = new DataTable();
                dt.Columns.Add(DataValueField, typeof(string));
                dt.Columns.Add(DataTextField, typeof(string));

                int pairCount = keyValue.Length / 2;
                for (int i = 0; i < pairCount; i++)
                {
                    dt.Rows.Add(new object[2] { keyValue[i * 2].ToString(), keyValue[i * 2 + 1].ToString() });
                }

                ViewState["SelectedItems"] = dt;
                return dt;
            }
            set
            {
                if ((value == null) || (value is IListSource))	// || (datasource is IEnumerable))
                {
                    if (value.Columns[0].ColumnName != DataValueField || value.Columns[1].ColumnName != DataTextField)
                    {
                        throw new ArgumentException("DataTable ought to have columns with same name as DataValueField and DataTextField");
                    }
                    ViewState["SelectedItems"] = value;
                }
                else
                {
                    throw new ArgumentException("DataSource must be DataSet or DataTable");
                }
            }
        }

        #endregion

        #region DataBind
        public override void DataBind()
        {
            //base.DataBind ();

            //Data source must be specified
            if (DataSource == null)
                throw new System.ArgumentException("Data source must be specified");

            DataBindingState = true;
        }

        #endregion

        #region GetDataSource
        //select datasource from various choices..
        /*
        protected virtual IEnumerable GetDataSource()
        {	
            if (datasource == null){
                return null;
            }
			
            IEnumerable resolvedDataSource = datasource as IEnumerable;
            if (resolvedDataSource != null){
                return resolvedDataSource;
            }
            return null;
        }
        */
        #endregion

        #region OnPreRender
        protected override void OnPreRender(EventArgs e)
        {
            //base.OnPreRender (e);

            #region JS script is prepared
            StringBuilder js = new StringBuilder();
            js.Append("<script type='text/javascript' language='JavaScript1.2'>");
            js.Append(" function DualListBox_ImageButtons(ID){ ");
            js.Append(" var option; ");
            js.Append(" var htmlImage = document.all[ID]; ");
            js.Append(" var elementPrefix = htmlImage.id.substring(0, htmlImage.id.indexOf('_')); ");
            js.Append(" var htmlImagePostfix = htmlImage.id.substring(htmlImage.id.indexOf('_')+1); ");
            js.Append(" var htmlLeftSelect = document.all[ elementPrefix +'_leftSelect']; ");
            js.Append(" var htmlRightSelect  = document.all[ elementPrefix +'_rightSelect']; ");
            js.Append(" var htmlInputHidden = document.all[elementPrefix + '_hidden']; ");

            js.Append(" switch(htmlImagePostfix){ ");
            js.Append(" case '2rightImage': ");
            js.Append(" for(var i=0; i<htmlLeftSelect.length; i++){ ");
            js.Append(" if(htmlLeftSelect.options[i].selected){ ");
            js.Append(" if( !DualListBox_IsItemInLeftSelect(htmlRightSelect, htmlLeftSelect.options[i].value)){ ");
            js.Append(" option = new Option(htmlLeftSelect.options[i].text, htmlLeftSelect.options[i].value); ");
            js.Append(" htmlRightSelect.options.add(option); ");
            js.Append(" DualListBox_FillHtmlInputHidden(htmlRightSelect, htmlInputHidden); }}} break; ");

            js.Append(" case '2leftImage': \n");
            js.Append(" for(var i=(htmlRightSelect.length)-1; i>-1; i--){ ");
            js.Append(" if(htmlRightSelect.options[i].selected){ ");
            js.Append(" htmlRightSelect.options[i] = null; ");
            js.Append(" DualListBox_FillHtmlInputHidden(htmlRightSelect, htmlInputHidden); }} break;}} ");

            js.Append(" function DualListBox_IsItemInLeftSelect(selectElement, value){ ");
            js.Append(" for(var i=0; i< selectElement.length; i++){ ");
            js.Append(" if(selectElement.options[i].value == value) return true;} return false; } ");

            js.Append(" function DualListBox_FillHtmlInputHidden(selectElement, dualListBoxBuffer){ ");
            js.Append(" var buffer = ''; ");
            js.Append(" for(var i=0; i<selectElement.length; i++){ ");
            js.Append(" buffer += selectElement[i].value +':'+ selectElement[i].text +':'; } ");
            js.Append(" buffer = buffer.substring(0, buffer.lastIndexOf(':')); ");
            js.Append("  dualListBoxBuffer.value = buffer; } ");
            //js.Append("  document.title = buffer; } ");
            js.Append(" </script>");
            #endregion

            if (!Page.IsStartupScriptRegistered("DualListBox"))
            {
                Page.RegisterStartupScript("DualListBox", js.ToString());
            }
        }
        #endregion

        protected override void Render(HtmlTextWriter w)
        {
            //base.Render (w);

            //check LeftImageUrl property
            if (DeleteImageUrl.Length == 0)
                throw new System.ArgumentException("LeftImageUrl property of ListBoxDual must be set");

            //check RightImageUrl property
            if (AddImageUrl.Length == 0)
                throw new System.ArgumentException("RightImageUrl property of ListBoxDual must be set");

            //TABLE
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            w.RenderBeginTag(HtmlTextWriterTag.Table);
            FirstRowOfTable(w);
            SecondRowOfTable(w);
            w.RenderEndTag();//</table>

            //hidden field in order to keep selected items..
            Page.RegisterHiddenField(this.ClientID + "_hidden", GetHiddenFieldValue());
        }//<Render>


        #region FirstRowOfTable
        private void FirstRowOfTable(HtmlTextWriter w)
        {
            w.RenderBeginTag(HtmlTextWriterTag.Tr);

            w.AddAttribute(HtmlTextWriterAttribute.Rowspan, "2");
            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region render first select item
            //attributes
            w.AddAttribute("size", Size.ToString());
            w.AddAttribute(HtmlTextWriterAttribute.Multiple, "true");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_leftSelect");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":leftSelect");
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
            w.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Verdana, Arial, Helvetica, sans-serif");
            w.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
            w.AddStyleAttribute(HtmlTextWriterStyle.Color, "#626B78");
            w.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
            //tag
            w.RenderBeginTag(HtmlTextWriterTag.Select);
            #region DataBindingState is true
            if (DataBindingState)
            {
                if (DataSource is DataTable)
                {
                    RenderOptions((DataTable)DataSource, w);
                }
                else
                {
                    DataSet ds = DataSource as DataSet;

                    if (DataTableName != string.Empty)
                        RenderOptions(ds.Tables[DataTableName], w);
                    else
                        RenderOptions(ds.Tables[0], w);
                }
            }
            #endregion
            w.RenderEndTag();
            #endregion
            w.RenderEndTag();//</td>

            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region Image 2 Right
            w.AddAttribute(HtmlTextWriterAttribute.Onclick, "DualListBox_ImageButtons(this.id)");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_2rightImage");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":2rightImage");
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            w.AddAttribute(HtmlTextWriterAttribute.Src, DeleteImageUrl);
            w.RenderBeginTag(HtmlTextWriterTag.Img);
            w.RenderEndTag();
            #endregion
            w.RenderEndTag();//</td>

            w.AddAttribute(HtmlTextWriterAttribute.Rowspan, "2");
            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region render second select item
            //attributes
            w.AddAttribute("size", Size.ToString());
            w.AddAttribute(HtmlTextWriterAttribute.Multiple, "true");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_rightSelect");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":rightSelect");
            w.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
            w.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Verdana, Arial, Helvetica, sans-serif");
            w.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
            w.AddStyleAttribute(HtmlTextWriterStyle.Color, "#626B78");
            w.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
            //tag
            w.RenderBeginTag(HtmlTextWriterTag.Select);
            #region Get ViewState for Right Select
            RenderOptions((DataTable)ViewState["SelectedItems"], w);
            #endregion
            w.RenderEndTag();
            #endregion
            w.RenderEndTag();//</td>

            w.RenderEndTag();//</tr>		
        }

        #endregion

        #region SecondRowOfTable
        private void SecondRowOfTable(HtmlTextWriter w)
        {
            w.RenderBeginTag(HtmlTextWriterTag.Tr);
            w.RenderBeginTag(HtmlTextWriterTag.Td);
            #region Image 2 Left
            w.AddAttribute(HtmlTextWriterAttribute.Onclick, "DualListBox_ImageButtons(this.id)");
            w.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_2leftImage");
            w.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + ":2leftImage");
            w.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            w.AddAttribute(HtmlTextWriterAttribute.Src, AddImageUrl);
            w.RenderBeginTag(HtmlTextWriterTag.Img);
            w.RenderEndTag();
            #endregion
            w.RenderEndTag();//</td>
            w.RenderEndTag();//</tr>
        }
        #endregion

        #region RenderOptions
        private void RenderOptions(DataTable dt, HtmlTextWriter w)
        {
            if (dt != null)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    w.AddAttribute(HtmlTextWriterAttribute.Value, dr[DataValueField].ToString());
                    w.RenderBeginTag(HtmlTextWriterTag.Option);
                    w.Write(dr[DataTextField]);
                    w.RenderEndTag();
                }
            }
        }
        #endregion

        #region GetHiddenFieldValue
        private string GetHiddenFieldValue()
        {
            DataTable dt = (DataTable)ViewState["SelectedItems"];
            if (dt == null)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder strb = new StringBuilder();

                foreach (DataRow dr in dt.Rows)
                {
                    strb.Append(dr[DataValueField].ToString() + ":" + dr[DataTextField].ToString() + ":");
                }//foreach

                return strb.Remove(strb.Length - 1, 1).ToString();
            }//if			
        }
        #endregion

    }//<c.ListBoxDual>
}//<ns.Gmtl.Web.UI>