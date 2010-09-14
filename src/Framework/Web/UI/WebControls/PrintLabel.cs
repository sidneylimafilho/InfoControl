using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;


using InfoControl.Web;

namespace InfoControl.Web.UI.WebControls
{

    [Designer(typeof(PrintLabelsDesigner))]
    [ToolboxData("<{0}:PrintLabel runat=server></{0}:PrintLabel>")]
    [ToolboxBitmap(typeof(Mirror))]
    public class PrintLabel : CompositeDataBoundControl
    {
        #region Inner Classes
        [ToolboxItem(false)]
        public sealed class TemplateOwner : WebControl, INamingContainer, IDataItemContainer
        {
            protected override HtmlTextWriterTag TagKey { get { return HtmlTextWriterTag.Div; } }

            #region IDataItemContainer Members
            private object _dataItem;
            public object DataItem
            {
                get { return _dataItem; }
            }

            private int _dataItemIndex;
            public int DataItemIndex
            {
                get { return _dataItemIndex; }
            }

            public int DisplayIndex
            {
                get { throw new NotImplementedException(); }
            }

            #endregion

            public TemplateOwner(object dataItem, int dataItemIndex)
            {
                _dataItem = dataItem;
                _dataItemIndex = dataItemIndex;
            }
        }

        sealed class DefaultTemplate : ITemplate
        {
            void ITemplate.InstantiateIn(Control owner)
            {
                Label title = new Label();
                LiteralControl linebreak = new LiteralControl("<br/>");
                Label caption = new Label();

                owner.Controls.Add(title);
                owner.Controls.Add(linebreak);
                owner.Controls.Add(caption);

            }


        }
        #endregion

        private ITemplate templateValue;
        private TemplateOwner ownerValue;
        private static DataTable labelFormatData;
        private DataTable LabelFormatData
        {
            get
            {
                if (labelFormatData == null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(new System.IO.StringReader(Properties.Resources.WebControls_PrintLabel_labelFormatData));
                    labelFormatData = ds.Tables[0];
                    labelFormatData.PrimaryKey = new DataColumn[] { labelFormatData.Columns[0] };
                }
                return labelFormatData;
            }
        }



        private string _labelFormat;
        [Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string LabelFormat
        {
            get { return _labelFormat; }
            set { _labelFormat = value.ToUpper(); }
        }

         [Browsable(false)]
        [DefaultValue(typeof(ITemplate), "")]
        [Description("Control template")]
        [TemplateContainer(typeof(TemplateOwner))]
        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public virtual ITemplate ItemTemplate
        {
            get
            {
                return templateValue;
            }
            set
            {
                templateValue = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            DataRow[] rows = LabelFormatData.Select("name='" + _labelFormat + "'");
            if (rows.Length == 0)
                throw new InvalidOperationException("LabelFormat is invalid!");

            DataRow row = rows[0];

            //
            // page setup
            //
            Style.Add("Text-Align", "center");
            Style.Add("padding",
                row["PageTopMargin"] + "mm " +
                row["PageRightMargin"] + "mm " +
                row["PageBottomMargin"] + "mm " +
                row["PageLeftMargin"] + "mm");
            Style.Add("width", row["PageWidth"] + "mm");
            Style.Add("height", row["PageHeight"] + "mm");

            Controls.Clear();
            ITemplate temp = templateValue ?? new DefaultTemplate();
            int count = 0;

            foreach (object o in dataSource)
            {
                ownerValue = new TemplateOwner(o, count);
                ownerValue.ID = "Panel" + count;
                ownerValue.Style.Add("width", row["LabelWidth"] + "mm");
                ownerValue.Style.Add("height", row["LabelHeight"] + "mm");
                ownerValue.Style.Add("float", "left");
                ownerValue.Style.Add("border", "1px solid #CCCCCC");
                ownerValue.Style.Add("margin-right", row["LabelRightMargin"] + "mm");
                ownerValue.Style.Add("margin-bottom", row["LabelBottomMargin"] + "mm");
                temp.InstantiateIn(ownerValue);                
                this.Controls.Add(ownerValue);
                ownerValue.DataBind();
                count++;
            }

            return 0;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.DataBind();
            base.OnPreRender(e);
        }
    }
}