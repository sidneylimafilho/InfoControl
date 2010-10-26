using System;
using System.ComponentModel;
using System.Web.UI;
using InfoControl;


namespace InfoControl.Web.UI
{
    [SupportsEventValidation]
    [ValidationProperty("CustomerId")]
    [ControlValueProperty("CustomerId")]
    public partial class DateTimeIntervalControl : InfoControl.Web.UI.DataUserControl
    {
        #region private properties

        private DateTimeInterval _dateInterval;
        private Boolean _enabled = true;

        #endregion

        #region properties

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
        public DateTime DefaultBeginDate
        {
            set { DateInterval = new DateTimeInterval(value, value.AddDays(7)); }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public Boolean Required { get; set; }

        public DateTimeInterval DateInterval
        {
            get
            {
                DateTime beginDate = DateTime.MinValue.Sql2005MinValue(), endDate = DateTime.MaxValue;
                if (!String.IsNullOrEmpty(txtBeginDate.Text) && txtBeginDate.Text != "__/__/____")
                    beginDate = Convert.ToDateTime(txtBeginDate.Text);

                if (!String.IsNullOrEmpty(txtEndDate.Text) && txtEndDate.Text != "__/__/____")
                    endDate = Convert.ToDateTime(txtEndDate.Text);

                if (beginDate > DateTime.MinValue.Sql2005MinValue() || endDate < DateTime.MaxValue)
                    _dateInterval = new DateTimeInterval(beginDate, endDate);

                return _dateInterval;
            }
            set
            {
                OnDateTimeIntervalChanging(value);
                _dateInterval = value;
            }
        }

        #endregion

        private void OnDateTimeIntervalChanging(DateTimeInterval dateTimeInterval)
        {
            txtBeginDate.Text = String.Empty;
            txtEndDate.Text = String.Empty;
            if (dateTimeInterval != null)
            {
                if (dateTimeInterval.BeginDate > DateTime.MinValue.Sql2005MinValue())
                    txtBeginDate.Text = dateTimeInterval.BeginDate.ToLocalDateString();
                if (dateTimeInterval.EndDate < DateTime.MaxValue.Date)
                    txtEndDate.Text = dateTimeInterval.EndDate.ToLocalDateString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            cmpDates.ValidationGroup =
                cmptxtBeginDate.ValidationGroup =
                cmptxtEndDate.ValidationGroup =
                reqtxtBeginDate.ValidationGroup =
                reqtxtEndDate.ValidationGroup = "_NonValidate";

            if (Required)
                cmpDates.ValidationGroup =
                cmptxtBeginDate.ValidationGroup =
                cmptxtEndDate.ValidationGroup =
                reqtxtBeginDate.ValidationGroup =
                reqtxtEndDate.ValidationGroup = ValidationGroup;


            txtBeginDate.Enabled = txtEndDate.Enabled = Enabled;

        }
    }
}