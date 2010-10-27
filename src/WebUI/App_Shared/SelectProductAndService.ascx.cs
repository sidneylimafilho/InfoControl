using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using System.ComponentModel;

namespace InfoControl.Web.UI
{
    public partial class SelectProductAndService : Vivina.Erp.SystemFramework.UserControlBase
    {
        private bool _required = false;
        public String Name
        {
            get
            {
                return txtItem.Text;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            valtxtItem.ValidationGroup = Required ? ValidationGroup : "_NonValidation";
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public string ValidationGroup
        {
            get;
            set;
        }

        [Bindable(true, BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        #region Methods


        /// <summary>
        /// This method clean the TextBox
        /// </summary>
        public void ClearField()
        {
            this.txtItem.Text = String.Empty;
        }

        public Boolean IsProduct
        {
            get { return Product != null; }
        }

        public Boolean IsService
        {
            get { return Service != null; }
        }

        public Service Service { get { return _service ?? (_service = new ServicesManager(this).GetServiceByName(Page.Company.CompanyId, txtItem.Text)); } }

        private Service _service;

        public Product Product { get { return _product ?? (_product = new ProductManager(this).GetProductByCode(CleanProductCode(), Page.Company.CompanyId)); } }

        private Product _product;

        private String CleanProductCode()
        {
            string[] productNameClear = txtItem.Text.Split('|');
            return productNameClear[0].TrimEnd();
        }

        #endregion

    }
}