using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using System.Web.Security;
using InfoControl.Net;

namespace Vivina.Erp.WebUI.Site
{
    public partial class CheckoutBasket : CheckoutPageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["b"]))
                Budget.BudgetId = Convert.ToInt32(Request["b"].DecryptFromHex());



            if (Budget.BudgetId == 0 && !String.IsNullOrEmpty(Request["productId"]))
            {
                bool isNewProduct = true;

                // Aqui é verificado se o item já existe na basket e altera somente a quantidade quando necessário
                // Isto ocorre quando o usuário não acessou redirecionado pelo e-mail com uma proposta cadastrada

                foreach (BudgetItem item in Budget.BudgetItems)
                    if (item.ProductId == Convert.ToInt32(Request["productId"]))
                    {
                        item.Quantity += Convert.ToInt32(Request["quantity"]);
                        isNewProduct = false;
                        break;
                    }

                if (isNewProduct)
                {
                    var budgetItem = new BudgetItem();
                    budgetItem.UnitCost = Convert.ToDecimal(Request["unitCost"]);
                    budgetItem.UnitPrice = Convert.ToDecimal(Request["unitPrice"]);
                    budgetItem.Quantity = Convert.ToInt32(Request["quantity"]);
                    budgetItem.SpecialProductName = Request["name"];
                    budgetItem.ModifiedDate = DateTime.Now;
                    budgetItem.ProductId = Convert.ToInt32(Request["productId"]);


                    //budgetItem.Product.ProductImages.Add(new ProductImage { ImageUrl = Request["ImageUrl"] });
                    Budget.BudgetItems.Add(budgetItem);
                }

            }

            if (!IsPostBack)
            {
                /*
                 * Aqui carrega o Budget
                 */
                if (Budget.BudgetId > 0)
                {
                    var saleManager = new SaleManager(this);
                    Budget = saleManager.GetBudget(Budget.BudgetId, Company.CompanyId);
                }

                BindBasket();
            }
        }

        /// <summary>
        /// verify if exists an image for the item and brings the main image url.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetImageUrl(object item)
        {
            var prod = (new ProductManager(this)).GetProduct((item as BudgetItem).ProductId.Value);

            if (prod.ProductImages.Any())
                return ResolveUrl(prod.ProductImages.First().ImageUrl);

            return null;
        }


        /// <summary>
        /// This method reload the basket-gridview  
        /// </summary>
        private void BindBasket()
        {
            SubTotal = 0;
            itemList.DataSource = Budget.BudgetItems;
            itemList.DataBind();

            if (itemList.Items.Count == 0)
            {
                pnlTotal.Visible = false;
                pnlSubTotal.Visible = false;
            }
        }

        protected void itemList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            int index = ((ListViewDataItem)e.Item).DataItemIndex;
            BudgetItem budgetItem = Budget.BudgetItems[index];
            SubTotal += budgetItem.Quantity * (budgetItem.UnitPrice ?? 0);

            if (budgetItem.Product != null)
            {
                //var prod = new ProductManager(this).GetProduct(budgetItem.ProductId.Value, Company.CompanyId);
                Weight += ((budgetItem.Product.Weight ?? 0.3m) * budgetItem.Quantity);
            }
            else
                Weight += 0.3m * budgetItem.Quantity;


            Total = SubTotal;
        }

        protected void lnkDelete_Command(object sender, ListViewCommandEventArgs e)
        {
            Budget.BudgetItems.RemoveAt(((ListViewDataItem)e.Item).DataItemIndex);
            BindBasket();


            if (!String.IsNullOrEmpty(ucDeliveryAddress.PostalCode))
                GetDeliveryPrices(Weight,
                                  Total,
                                  Company.LegalEntityProfile.Address.PostalCode,
                                  ucDeliveryAddress.PostalCode);
        }

        protected void odsPaymentMethods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void ucDeliveryAddress_Changed(object sender, EventArgs e)
        {
            GetDeliveryPrices(Weight,
                              Total,
                              Company.LegalEntityProfile.Address.PostalCode,
                              ucDeliveryAddress.PostalCode);
        }


        protected void rbtListDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            FreightType = rbtListDelivery.SelectedItem.Text.Trim().Split(':')[0];
            Freight = Convert.ToDecimal(rbtListDelivery.SelectedItem.Value);
            Total = SubTotal + Freight;
        }

        #region Private methods

        protected void LinkCommand_Command(object sender, CommandEventArgs e)
        {
            //
            // Save the address when step out of page
            //
            DeliveryAddress = ucDeliveryAddress;

            String nextUrl = "";
            if (e.CommandName == "Back")
                nextUrl = "~/site/Products.aspx?";

            if (e.CommandName == "Next")
            {
                nextUrl = "~/site/Checkout_Identification.aspx?";

                if (Budget.BudgetId > 0 && Budget.CustomerId.HasValue)
                    nextUrl = "~/site/Checkout_Payment.aspx?customerId=" + Budget.CustomerId.EncryptToHex();

                //                FormsAuthentication.Encrypt(new FormsAuthenticationTicket("Budget", true, Int32.MaxValue));
            }

            Response.Redirect(nextUrl);
        }


        /// <summary>
        /// This method calculates delivery prices via web Service that resquests informations of webSite's Correios company 
        /// </summary>
        /// <param name="weight"> sum weight of products</param>
        /// <param name="price"> sum of prices of products  </param>
        /// <param name="initialPostalCode"> postal code of initial place </param>
        /// <param name="finishPostalCode"> postal code of delivery</param>
        /// <returns></returns>
        private void GetDeliveryPrices(decimal weight, decimal price, String initialPostalCode, String finishPostalCode)
        {
            weight = (weight > 0 ? weight : 1);

            var servicos = new[] { 
                new { id = 41106, name = "Pac" }, 
                new { id = 40010, name = "Sedex" }, 
                new { id = 40215, name = "Sedex10" } 
            };

            var queriesString = "http://ws.correios.com.br/calculador/calcprecoprazo.asmx?" +
                "StrRetorno=xml&nCdServico=" + String.Join(",", servicos.Select(a => a.id.ToString()).ToArray()) +
                "&nVlPeso=" + weight.ToString().Replace(",", ".") +
                "&sCepOrigem=" + initialPostalCode +
                "&sCepDestino=" + finishPostalCode +
                "&nCdFormato=1" +
                "&nVlComprimento=30" +
                "&nVlAltura=30" +
                "&nVlLargura=30";

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(queriesString);

            var nodes = xmlDocument.SelectNodes("//cServico");

            if (nodes != null)
                for (var i = 0; i < nodes.Count; i++)
                {
                    string txt = nodes[i].SelectSingleNode("Valor").InnerText;
                    rbtListDelivery.Items[i].Value = txt;
                    rbtListDelivery.Items[i].Text = servicos[i].name + ":  </td><td class='valor'>" + Convert.ToDecimal(txt).ToString("C");
                    rbtListDelivery.Items[i].Selected = false;
                }
            pnlDeliveryPrices.Visible = true;
        }

        #endregion
    }
}
