using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl.Web.UI;

namespace InfoControl.Web.UI
{
    [SupportsEventValidation]
    [ValidationProperty("Text")]
    [ControlValueProperty("Text")]
    public partial class CurrencyField : DataUserControl
    {
        #region private properties

        private Decimal? _currencyValue;
        private Boolean _enabled = true;

        #endregion

        #region  public Propereties

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
        public int MaxLength { get; set; }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public String Mask { get; set; }

        public Decimal? CurrencyValue
        {
            get
            {
                if (!String.IsNullOrEmpty(RemoveMask(txtCurrencyValue.Text)))
                {
                    Decimal _temp = Decimal.Zero;
                    Decimal.TryParse(RemoveMask(txtCurrencyValue.Text), out _temp);

                    _currencyValue = _temp;
                }
                else
                    _currencyValue = null;
                return _currencyValue;
            }
            set
            {
                OnCurrencyValueChanging(value);
                _currencyValue = value;
            }
        }

        public int IntValue
        {
            get { return Convert.ToInt32(CurrencyValue); }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public Int32 Columns { get; set; }

        /// <summary>
        /// This property is used at bound event such as grid
        /// </summary>
        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public String Text
        {
            get { return Convert.ToString(CurrencyValue); }
            set
            {
                Decimal tempValue = Decimal.Zero;
                Decimal.TryParse(value, out tempValue);
                CurrencyValue = tempValue;
            }
        }

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            Configure();

            if (!IsPostBack)
            {
                reqtxtCurrencyValue.ValidationGroup = "None";

                if (!String.IsNullOrEmpty(ValidationGroup))
                    reqtxtCurrencyValue.ValidationGroup = ValidationGroup;


            }
        }

        private void Configure()
        {
            if (String.IsNullOrEmpty(Mask))
                Mask = "decimal";

            txtCurrencyValue.Attributes["mask"] = Mask;
            txtCurrencyValue.Columns = Columns;
            txtCurrencyValue.Width = Unit.Pixel(Mask.Length * 8);
            txtCurrencyValue.Enabled = Enabled;
            txtCurrencyValue.MaxLength = MaxLength;
        }

        private void OnCurrencyValueChanging(Decimal? currencyValue)
        {
            Configure();
            txtCurrencyValue.Text = currencyValue.HasValue
                                        ? currencyValue.Value.ToString()
                                        : String.Empty;
        }

        #endregion

        /// <summary>
        /// Erase the format text that MaskedEdit put
        /// </summary>
        /// <param name="textbox"></param>
        /// <returns></returns>
        public string RemoveMask(string textbox)
        {
            return (textbox ?? "").Replace("(__)", "") // Phone
                .Replace("_/_", "") // DateTime
                .Replace("_", "")
                .Replace(".", "");
        }
    }
}