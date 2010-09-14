using System;
using System.IO;
using System.Text;
using System.Threading;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.BusinessRules
{
    public class DocumentBuilder
    {
        private Budget budget;
        private Company company;
        private Customer customer;
        private Receipt receipt;
        private Supplier supplier;
        private DocumentTemplate template;
        private string templateHtml = "";

        public DocumentBuilder(DocumentTemplate template)
        {
            this.template = template;
        }

        /// <summary>
        /// Contains the Html that will be replaced
        /// </summary>
        public string TemplateHtml
        {
            get
            {
                if (String.IsNullOrEmpty(templateHtml))
                {
                    string appPath = Thread.GetDomain().BaseDirectory +
                                     template.FileUrl.Replace("~", "").Replace("/", "\\");
                    templateHtml = File.ReadAllText(appPath);
                }
                return templateHtml;
            }
        }

        public void Add(Company company)
        {
            this.company = company;
        }

        public void Add(Budget budget)
        {
            this.budget = budget;
        }

        public void Add(Customer customer)
        {
            this.customer = customer;
        }

        public void Add(Receipt receipt)
        {
            this.receipt = receipt;
        }

        public void Add(Supplier supplier)
        {
            this.supplier = supplier;
        }

        /// <summary>
        /// Transforms the document template using attributes of Company, Budget, Customer, Receipt or Supplier
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            var stringBuilder = new StringBuilder(TemplateHtml);

            stringBuilder.Replace("[Logo]",
                                  " <img src='" + company.LegalEntityProfile.Website +
                                  "/site/imagehandler.aspx' border='0' /> ");

            /*
            if (budget.BudgetItems == null)
                budget = GetBudget(budget.BudgetId, budget.CompanyId);

            
            var tempStringBuilder = new StringBuilder();

            stringBuilder.Replace("[NumeroDoOrcamento]", budget.BudgetCode + " <br /> ");
            stringBuilder.Replace("[DataEmissao]", budget.ModifiedDate.ToShortDateString() + " <br /> ");

            //Customer
            if (budget.CustomerId.HasValue || !String.IsNullOrEmpty(budget.CustomerName))
            {
                #region Customer Data

                stringBuilder.Replace("[NomeDoCliente]", String.IsNullOrEmpty(budget.CustomerName)
                                                         ? budget.Customer.Name
                                                         : budget.CustomerName);

                stringBuilder.Replace("[EmailDoClente]", budget.Customer != null
                                                         ? budget.Customer.Email
                                                         : budget.CustomerMail);

                stringBuilder.Replace("[TelefoneDoCliente]", String.IsNullOrEmpty(budget.CustomerPhone)
                                                             ? budget.Customer.Phone
                                                             : budget.CustomerPhone);

                #endregion

                #region Customer Address

                if (budget.Customer != null)
                {

                    stringBuilder.Replace("[EnderecoDoCliente]", budget.Customer.Profile != null
                                                                 ? budget.Customer.Profile.Address.Name
                                                                 : budget.Customer.LegalEntityProfile.Address.Name);

                    stringBuilder.Replace("[Endereco-Complemento]", budget.Customer.Profile != null
                                                                 ? budget.Customer.Profile.AddressComp
                                                                 : budget.Customer.LegalEntityProfile.AddressComp);

                    stringBuilder.Replace("[Endereco-Numero]", budget.Customer.Profile != null
                                                               ? budget.Customer.Profile.AddressNumber
                                                               : budget.Customer.LegalEntityProfile.AddressNumber);

                    stringBuilder.Replace("[Endereco-Cep]", budget.Customer.Profile != null
                                                            ? budget.Customer.Profile.PostalCode
                                                            : budget.Customer.LegalEntityProfile.PostalCode);

                    stringBuilder.Replace("[Endereco-Cidade]", budget.Customer.Profile != null
                                                               ? budget.Customer.Profile.Address.City
                                                               : budget.Customer.LegalEntityProfile.Address.City);

                    stringBuilder.Replace("[Endereco-Estado]", budget.Customer.Profile != null
                                                               ? budget.Customer.Profile.Address.State
                                                               : budget.Customer.LegalEntityProfile.Address.State);

                    stringBuilder.Replace("[Telefone2]", budget.Customer.LegalEntityProfile.Phone2);
                    stringBuilder.Replace("[Telefone3]", budget.Customer.LegalEntityProfile.Phone3);
                }
                else
                {
                    stringBuilder.Replace("[EnderecoDoCliente]", String.Empty);
                    stringBuilder.Replace("[Endereco-Complemento]", String.Empty);
                    stringBuilder.Replace("[Endereco-Numero]", String.Empty);
                    stringBuilder.Replace("[Endereco-Cep]", String.Empty);
                    stringBuilder.Replace("[Endereco-Cidade]", String.Empty);
                    stringBuilder.Replace("[Endereco-Estado]", String.Empty);
                    stringBuilder.Replace("[Telefone2]", String.Empty);
                    stringBuilder.Replace("[Telefone3]", String.Empty);
                }


                #endregion
            }
            //else
            //{
            //    tempStringBuilder.AppendLine("Indefinido" + "<br>");
            //}

            #region BudgetItems

            tempStringBuilder = new StringBuilder();

            //Header
            tempStringBuilder.Append(@"
                <table width='100%'>
                <tr>
                <td>ITEM</td>
                <td>UNID</td>
                <td>QTD</td>
                <td>DESCRI&Ccedil;&Atilde;O</td>
                <td>VLR UNIT</td>
                <td>VLR TOTAL</td>
                </tr>");

            //Body
            Int32 itemCount = 0;
            Decimal totalValue = Decimal.Zero;
            foreach (BudgetItem item in GetBudgetItemByBudget(budget.BudgetId, budget.CompanyId))
            {
                itemCount++;
                tempStringBuilder.Append("<tr>");
                tempStringBuilder.Append("<td>" + itemCount + "</td>");

                string itemName = item.SpecialProductName;
                if (item.Product != null)
                    itemName = item.Product.Name;
                if (item.Service != null)
                    itemName = item.Service.Name;

                tempStringBuilder.Append("<td>" + itemName + "</td>");
                tempStringBuilder.Append("<td>" + item.Quantity + "</td>");
                tempStringBuilder.Append("<td>" + item.ProductDescription + "</td>");
                tempStringBuilder.Append("<td>" + item.UnitPrice.Value.ToString("c") + "</td>");
                tempStringBuilder.Append("<td>" + (item.Quantity * item.UnitPrice.Value).ToString("c") + "</td>");
                tempStringBuilder.Append("</tr>");

                totalValue += (item.Quantity * item.UnitPrice.Value);
            }
            //Footer
            tempStringBuilder.Append("<tr>");
            tempStringBuilder.Append("<td colspan='4'>Total<td>");
            tempStringBuilder.Append("<td>" + totalValue.ToString("c") + "<td>");
            tempStringBuilder.Append("</tr>");
            tempStringBuilder.Append("<tr>");
            tempStringBuilder.Append(@"<td>Contato</td>
                                       <td>Entrega(dias)</td>
                                       <td>garantia</td>
                                       <td>Validade da proposta</td>
                                       <td>Pagamento</td>
                                       <td>Local de Entrega</td>
                                       <td>Observa&ccedil;&atilde;o</td>
                                       <td>Vendedor</td>");
            tempStringBuilder.Append("</tr>");
            tempStringBuilder.Append("<tr>");
            tempStringBuilder.Append("<td>" + budget.ContactName + "</td>");
            tempStringBuilder.Append("<td>" + budget.DeliveryDate + "</td>");
            tempStringBuilder.Append("<td>" + budget.Warranty + "</td>");
            tempStringBuilder.Append("<td>" + budget.ExpirationDate.ToString() + "</td>");
            tempStringBuilder.Append("<td>" + budget.PaymentMethod + "</td>");
            tempStringBuilder.Append("<td>" + budget.DeliveryDescription + "</td>");
            tempStringBuilder.Append("<td>" + budget.Observation + "</td>");
#warning Pode existir cadastros antigos ainda sem vendedor associado
            tempStringBuilder.Append("<td>" + budget.Employee.Profile.Name + "</td>");
            tempStringBuilder.Append("</tr>");
            tempStringBuilder.Append("</table>");

            #endregion

            stringBuilder.Replace("[Items]", tempStringBuilder.ToString());

             * */
            return stringBuilder.ToString();
        }
    }
}