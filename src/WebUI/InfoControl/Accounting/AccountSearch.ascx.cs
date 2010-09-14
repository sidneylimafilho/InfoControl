using System;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules.Accounting;

namespace InfoControl.Web.UI
{
    public partial class AccountSearch : Vivina.Erp.SystemFramework.UserControlBase, IAccountSearch
    {
        public int? AccountPlanId
        {
            get
            {
                if (!String.IsNullOrEmpty(cboAccountPlan.SelectedValue))
                    return Convert.ToInt32(cboAccountPlan.SelectedValue);
                return null;
            }
            set { }
        }
        public int? CostCenterId
        {
            get
            {
                if (!String.IsNullOrEmpty(cboCostCenter.SelectedValue))
                    return Convert.ToInt32(cboCostCenter.SelectedValue);
                return null;
            }
            set { }
        }
        public int? ParcelStatus
        {
            get
            {
                if (!String.IsNullOrEmpty(rdbListStatus.SelectedValue))
                    return Convert.ToInt32(rdbListStatus.SelectedValue);
                return null;
            }
            set { }
        }
        public InfoControl.DateTimeInterval dateTimeInterval
        {
            get
            {
                return ucDateInterval.DateInterval;
            }
            set { }
        }
        public string Name
        {
            get
            {
                if (!String.IsNullOrEmpty(txtName.Text))
                    return txtName.Text;
                return null;
            }
            set { }
        }

        public decimal? ParcelValue
        {
            get
            {
                if (ucCurrFieldParcelValue.CurrencyValue.HasValue)
                    if (ucCurrFieldParcelValue.CurrencyValue.Value > 0)
                        return ucCurrFieldParcelValue.CurrencyValue.Value;

                return null;
            }
            set { }
        }

        public string Identification
        {

            get
            {

                return txtIdentification.Text;
            }
            set { }
        }

        public int? AccountId
        {
            get
            {
                if (!String.IsNullOrEmpty(cboAccount.SelectedValue))
                    return Convert.ToInt32(cboAccount.SelectedValue);
                return null;
            }
            set { }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            //
            //This code load the comboBox of deal the respective page 
            //
            if (Page.GetType().Name == "infocontrol_accounting_bills_aspx")
                odsAccountingPlan.SelectMethod = "GetOutboundPlan";
            else
                odsAccountingPlan.SelectMethod = "GetInboundPlan";

            if (!IsPostBack)
            {
                //
                // Load the last search  
                //

                String rdbStatusValue = ((Int32)Vivina.Erp.DataClasses.ParcelStatus.OPEN).ToString();

                if (Page.Customization["ParcelStatus"] != null && rdbListStatus.Items.FindByValue(Convert.ToString(Page.Customization["ParcelStatus"])) != null)
                    rdbStatusValue = Convert.ToString(Page.Customization["ParcelStatus"]);

                rdbListStatus.Items.FindByValue(rdbStatusValue).Selected = true;

                if (cboCostCenter.ExistItem(Convert.ToString(Page.Customization["CostCenterId"])))
                    cboCostCenter.SelectedValue = Convert.ToString(Page.Customization["CostCenterId"]);

                if (cboAccountPlan.ExistItem(Convert.ToString(Page.Customization["AccountPlanId"])))
                    cboAccountPlan.SelectedValue = Convert.ToString(Page.Customization["AccountPlanId"]);

                ucCurrFieldParcelValue.CurrencyValue = (decimal?)Page.Customization["ParcelValue"];

                if (cboAccount.Items.FindByValue(Convert.ToString(Page.Customization["AccountId"])) != null)
                    cboAccount.SelectedValue = Convert.ToString(Page.Customization["AccountId"]);

                txtIdentification.Text = Convert.ToString(Page.Customization["Identification"]);

                ucDateInterval.DefaultBeginDate = DateTime.Now.Date;

                this.dateTimeInterval = ucDateInterval.DateInterval;
                this.ParcelStatus = Vivina.Erp.DataClasses.ParcelStatus.OPEN;

                if ((InfoControl.DateTimeInterval)Page.Customization["DateTimeInterval"] != null) //dateTimeInterval
                    ucDateInterval.DateInterval = (InfoControl.DateTimeInterval)Page.Customization["DateTimeInterval"];

                if (Page.Customization["Name"] != null)
                    txtName.Text = Convert.ToString(Page.Customization["Name"]);

                btnSearchInvoice_Click(this, e);
            }
        }

        protected void btnSearchInvoice_Click(object sender, EventArgs e)
        {
            //
            // Reseting fields when performing a new search
            //
            //this.CostCenterId = this.AccountPlanId = this.ParcelStatus = null;
            //this.dateTimeInterval = null;
            //this.Name = null;

            OnSelectingSearchAccountParameters(this, new EventArgs());

            //
            // Setting the properties from the controls 
            //
            //if (!String.IsNullOrEmpty(rdbListStatus.SelectedValue))
            //    this.ParcelStatus = Convert.ToInt32(rdbListStatus.SelectedValue);

            //if (cboAccountPlan.SelectedValue != null)
            //    this.AccountPlanId = Convert.ToInt32(cboAccountPlan.SelectedValue);

            //if (cboCostCenter.SelectedValue != null)
            //    this.CostCenterId = Convert.ToInt32(cboCostCenter.SelectedValue);

            //if (!String.IsNullOrEmpty(txtName.Text))
            //    this.Name = txtName.Text;

            //this.dateTimeInterval = ucDateInterval.DateInterval;

            //
            // Save variables from last search for this user
            //
            Page.Customization["ParcelStatus"] = rdbListStatus.SelectedValue;
            Page.Customization["DateTimeInterval"] = ucDateInterval.DateInterval;
            Page.Customization["CostCenterId"] = cboCostCenter.SelectedValue;
            Page.Customization["AccountPlanId"] = cboAccountPlan.SelectedValue;
            Page.Customization["Name"] = txtName.Text;

            Page.Customization["AccountId"] = cboAccount.SelectedValue;
            Page.Customization["Identification"] = txtIdentification.Text;

            if (ucCurrFieldParcelValue.CurrencyValue.HasValue)
                Page.Customization["ParcelValue"] = ucCurrFieldParcelValue.CurrencyValue.Value;


            OnSelectedSearchAccountParameters(this, new EventArgs());
        }

        protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Page.Company.CompanyId;
        }

        protected void OnSelectingSearchAccountParameters(object sender, EventArgs e)
        {
            if (SelectingSearchAccountParameters != null)
                SelectingSearchAccountParameters(sender, e);
        }
        protected void OnSelectedSearchAccountParameters(object sender, EventArgs e)
        {
            if (SelectedSearchAccountParameters != null)
                SelectedSearchAccountParameters(sender, e);
        }
        public event EventHandler SelectingSearchAccountParameters;
        public event EventHandler SelectedSearchAccountParameters;

        protected void odsAccountType_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Page.Company.CompanyId;
        }
    }

    public class SelectingSearchAccountParametersEventArgs : EventArgs
    {

    }
    public class SelectedSearchAccountParametersEventArgs : EventArgs
    {
        public DateTimeInterval DateTimeInterval { get; set; }
        public Int32? ParcelStatus { get; set; }
        public Int32 AccountPlanId { get; set; }
        public Int32 CostCenterId { get; set; }
        public IAccountSearch AccountSearch { get; set; }

    }
}