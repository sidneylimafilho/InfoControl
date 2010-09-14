using System;
using InfoControl;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using System.ComponentModel;
using System.Web.UI;

namespace Vivina.Erp.WebUI.Site
{
    public partial class CheckoutPageBase : SitePageBase
    {


        private Budget budget;
        public Budget Budget
        {
            get
            {
                return budget ??
                       (budget = (Budget)Session["budget"]) ??
                       (Budget)(Session["budget"] = budget = new Budget { CompanyId = Company.CompanyId });
            }

            set
            {
                Session["budget"] = budget = value;
                if (budget != null)
                    budget.CompanyId = Company.CompanyId;
            }
        }

        //public IList<BudgetItem> Basket { get { return Budget.BudgetItems; } }

        public decimal SubTotal
        {
            get { return Convert.ToDecimal(Session["subtotal"]); }
            set { Session["subtotal"] = value; }
        }

        public decimal Total
        {
            get { return Convert.ToDecimal(Session["total"]); }
            set { Session["total"] = value; }
        }

        public decimal Freight
        {
            get { return Convert.ToDecimal(Session["freight"]); }
            set { Session["freight"] = value; }
        }

        public string FreightType
        {
            get { return Convert.ToString(Session["freightType"]); }
            set { Session["freightType"] = value; }
        }

        public decimal Weight
        {
            get { return Convert.ToDecimal(Session["weight"]); }
            set { Session["weight"] = value; }
        }

        public Address DeliveryAddress
        {
            get { return (Address)Session["deliveryAddress"]; }
            set { Session["deliveryAddress"] = value; }
        }
    }
}
