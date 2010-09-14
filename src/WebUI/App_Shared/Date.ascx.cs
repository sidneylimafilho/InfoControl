using System;
using System.ComponentModel;
using System.Web.UI;


namespace InfoControl.Web.UI
{
    public partial class Date : InfoControl.Web.UI.DataUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cboTime.Visible = ShowTime;

            reqtxtDate.Enabled = Required;

            if (!String.IsNullOrEmpty(ValidationGroup))
                cmptxtDate.ValidationGroup = reqtxtDate.ValidationGroup = ValidationGroup;

            txtDate.Enabled = Enabled;

            //caltxtDate.OnClientDateSelectionChanged = "__doPostBack('" + txtDate.ClientID + "','')";
        }

        #region private properties

        private DateTime? _date;

        #endregion

        #region  publicProperties

        private Boolean _enabled = true;

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public String ValidationGroup { get; set; }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public Boolean Required { get; set; }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public DateTime? DateTime
        {
            get
            {
                DateTime tempDate;
                if (System.DateTime.TryParse(txtDate.Text, out tempDate))
                    _date = tempDate;
                else
                    _date = null;

                if (_date != null && ShowTime)
                {
                    var time = cboTime.SelectedValue.Split(':');
                    _date = new DateTime(_date.Value.Year, _date.Value.Month, _date.Value.Day, Convert.ToInt32(time.GetValue(0)), Convert.ToInt32(time.GetValue(1)), 00);
                }
                return _date;
            }
            set
            {
                OnDateTimeChanging(value);
                _date = value;

                if (_date != null && ShowTime)
                    cboTime.SelectedValue = _date.Value.Hour.ToString("00") + ":" + _date.Value.Minute.ToString("00");

            }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public string Text
        {
            get { return Convert.ToString(DateTime); }
            set
            {
                DateTime tempDate;
                if (System.DateTime.TryParse(value, out tempDate))
                    DateTime = tempDate;
                else
                    DateTime = null;
            }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public Boolean ShowTime { get; set; }

        #endregion

        #region Events

        private void OnDateTimeChanging(DateTime? dateTime)
        {
            txtDate.Text = dateTime != null ? dateTime.Value.ToLocalDateString() : String.Empty;
        }

        #endregion

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            OnSelectedDateTime(sender, new SelectedDateTimeEventArgs
                                       {
                                           DateTime = this.DateTime
                                       });
        }

        private event SelectedDateTime SelectedDateTime;
        protected void OnSelectedDateTime(object sender, SelectedDateTimeEventArgs e)
        {
            if (SelectedDateTime != null)
                SelectedDateTime(sender, e);
        }
    }

    public class SelectedDateTimeEventArgs : EventArgs
    {
        public DateTime? DateTime { get; set; }
    }

    public delegate void SelectedDateTime(object sender, SelectedDateTimeEventArgs e);
}