using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;

using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.DataClasses;
namespace Vivina.Erp.WebUI.POS
{
    [Serializable()]
    public class Payment
    {
        private Int32 _financierOperationId;
        private String _financierOperationName;
        private DateTime _dueDate;
        private Decimal _amount;
        private Int32 _accountId;

        public Int32 FinancierOperationId { get { return _financierOperationId; } set { _financierOperationId = value; } }
        public String FinancierOperationName { get { return _financierOperationName; } set { _financierOperationName = value; } }
        public DateTime DueDate { get { return _dueDate; } set { _dueDate = value; } }
        public Decimal Amount { get { return _amount; } set { _amount = value; } }
        public Int32 AccountId { get { return _accountId; } set { _accountId = value; } }

    }




    public partial class Sale_Parcels2 : Vivina.Erp.SystemFramework.PageBase
    {
        public List<SaleItem> SaleItemList
        {
            get
            {
                if (Session["SaleItemList"] == null)
                    Session["SaleItemList"] = new List<SaleItem>();

                return (Session["SaleItemList"] as List<SaleItem>);
            }
            set { Session["SaleItemList"] = value; }


        }
        public List<Payment> PaymentList
        {
            get
            {
                if (Session["PaymentList"] == null)
                    Session["PaymentList"] = new List<Payment>();

                return (Session["PaymentList"] as List<Payment>);
            }
            set { Session["PaymentList"] = value; }


        }
        Decimal totalSale = 0;
        Decimal totalParcels = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrdParcels();
                ucDueDate.DateTime = DateTime.Now;
                txtQtdParcels.Text = "1";
                Page.ViewState["Amount"] = Convert.ToDecimal(Session["Total"]);
                Page.ViewState["CustomerId"] =Session["CustomerId"];
                Page.ViewState["BudgetId"] = Session["BudgetId"];
                txtAmount.Text = Page.ViewState["Amount"].ToString();
                btnFinishSale.Attributes["accesskey"] = "F10";
                ttpEmployee.Visible = cboEmployee.Items.Count > 0;

            }
        }

        protected void odsGeneric_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void btnFinishSale_Click(object sender, ImageClickEventArgs e)
        {
            // do not have items in grdParcels, validate the fields of parcel
            if (!PaymentList.Any())
            {
                bool isValid = true;

                ///execute the validators
                reqtxtAmount.Validate();
                reqcboFinancierOperations.Validate();

                ///retrieving result of validators
                isValid &= reqtxtAmount.IsValid;
                isValid &= reqcboFinancierOperations.IsValid;
                if (!isValid)
                    return;
            }

            //save the sale
            SaveSale();

        }

        private void BindGrdParcels()
        {
            grdParcels.DataSource = PaymentList;
            grdParcels.DataBind();
        }

        protected void grdParcels_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //
            //calculate total parcels
            //
            if (e.Row.RowType == DataControlRowType.DataRow)
                totalParcels += Convert.ToDecimal(grdParcels.DataKeys[e.Row.RowIndex]["Amount"]);

            //
            //calculate diff between total sale and total parcels
            //
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                totalSale = Convert.ToDecimal("0" + Page.ViewState["totalSale"]);

                lblTotalSale.Text = "<b>Total da venda: " + totalSale.ToString("C") + "</b>";
                lblTotalParcels.Text = "<b>Total das parcelas: " + totalParcels.ToString("C") + "</b>";
                lblDif.Text = "<b>Diferença: " + (totalParcels - totalSale).ToString("C") + "</b>";
            }
        }

        private void AddParcel()
        {
            Decimal amount = Convert.ToDecimal(txtAmount.Text);
            Decimal parcelValue;
            if (!ucDueDate.DateTime.HasValue)
                ucDueDate.DateTime = DateTime.Now;

            DateTime dueDate = ucDueDate.DateTime.Value;

            int qtdParcels = Convert.ToInt32(FormatDecimal(txtQtdParcels.Text));

            Payment payment;
            for (int i = 1; i <= qtdParcels; i++)
            {
                parcelValue = Math.Round(calculateParcelValue(amount, qtdParcels), 2);
                //esta condicional verifica se a parcela corrente é a primeira, se a condição for satisfeita
                //ele calcula a parcela. fez se necessário esta condicional para o caso do valor da parcela
                // der uma dizima periodica. o codigo calcula todas as parcelas e verifica a diferença entre o valor total
                // e o somatório da parcela, essa diferença é somada ao valor da primeira parcela
                if (i == 1)
                    parcelValue = parcelValue + (amount - (parcelValue * qtdParcels));

                payment = new Payment();
                payment.FinancierOperationId = Convert.ToInt32(cboFinancierOperations.SelectedValue);
                payment.FinancierOperationName = cboFinancierOperations.SelectedItem.Text;
                payment.DueDate = dueDate;
                payment.Amount = parcelValue;
                payment.AccountId = 0;
                PaymentList.Add(payment);

            }
            BindGrdParcels();
        }

        /// <summary>
        /// this method create and return a list of SaleItem 
        /// </summary>
        /// <returns></returns>
        private List<Vivina.Erp.DataClasses.SaleItem> CreateSaleItemList()
        {
            List<Vivina.Erp.DataClasses.SaleItem> list = new List<Vivina.Erp.DataClasses.SaleItem>();


            if (!SaleItemList.Any())
                return new List<Vivina.Erp.DataClasses.SaleItem>();
            Vivina.Erp.DataClasses.SaleItem saleItem;
            foreach (SaleItem sItem in SaleItemList)
            {
                saleItem = new Vivina.Erp.DataClasses.SaleItem();
                saleItem.CompanyId = Company.CompanyId;
                saleItem.ModifiedDate = DateTime.Now;
                saleItem.Quantity = sItem.Quantity;
                saleItem.SerialNumber = sItem.SerialNumber;
                saleItem.UnitCost = sItem.UnitCost;
                saleItem.UnitPrice = sItem.Price;
                //saleItem.UserId = User.Identity.UserId;
                saleItem.ProductId = sItem.ProductId;

                if (!saleItem.ProductId.HasValue && sItem.Name.Contains("&nbsp;"))
                    saleItem.SpecialProductName = sItem.Name.Remove(sItem.Name.IndexOf("&nbsp;"));

                list.Add(saleItem);
            }

            return list;
        }

        /// <summary>
        /// this method create and return a Sale 
        /// </summary>
        /// <returns></returns>
        private Sale CreateSale()
        {
            Sale sale = new Sale();
            sale.CompanyId = Company.CompanyId;
            sale.DepositId = Deposit.DepositId;
            
            sale.SaleDate = DateTime.Now;
            sale.OrderDate = DateTime.Now;
            sale.ShipDate = DateTime.Now;
            if (Page.ViewState["CustomerId"] != null)
                sale.CustomerId = Convert.ToInt32(Page.ViewState["CustomerId"]);
            if (Page.ViewState["BudgetId"] != null)
                sale.BudgetId = Convert.ToInt16(Page.ViewState["BudgetId"]);

            sale.Discount = Convert.ToDecimal(Page.ViewState["discount"]);
            if (!String.IsNullOrEmpty(cboEmployee.SelectedValue))
                sale.VendorId = Convert.ToInt16(cboEmployee.SelectedValue);

            return sale;
        }

        /// <summary>
        /// this method create and return a list of parcel
        /// </summary>
        /// <returns></returns>
        private List<Parcel> CreateParcelList()
        {
            Parcel parcel;
            List<Parcel> parcelList = new List<Parcel>();
            Int32 index = 0;
            foreach (Payment item in PaymentList)
            {
                parcel = new Parcel();
                parcel.FinancierOperationId = item.FinancierOperationId;
                parcel.Amount = item.Amount;
                parcel.Description = (index + 1).ToString() + "/" + PaymentList.Count();
                parcel.DueDate = item.DueDate;
                parcel.EffectedAmount = null;
                parcel.EffectedDate = null;
                parcel.CompanyId = Company.CompanyId;

#warning verificar de onde vem este numero magico
                if (parcel.FinancierOperationId == 1)
                {
                    parcel.EffectedDate = DateTime.Now;
                    parcel.EffectedAmount = parcel.Amount;
                }
                parcelList.Add(parcel);
                index++;
            }

            return parcelList;
        }

        public Sale SaveSale()
        {
            if (!String.IsNullOrEmpty(txtAmount.Text) && !String.IsNullOrEmpty(cboFinancierOperations.SelectedValue) && ucDueDate.DateTime.HasValue)
                AddParcel();


            Sale sale = new SaleManager(this).SaveSale(CreateSale(), CreateSaleItemList(), User.Identity.UserId, CreateParcelList());

            if (chkPrint.Checked)
                HandleLegalTicket();

            //
            // Disposing Session from memory
            //

            Page.ViewState["BudgetId"] = null;
            Page.ViewState["CustomerId"] = null;
            //Session["basket"] = null;
            Page.ViewState["discount"] = null;
            Page.ViewState["FiscalNumber"] = null;
            PaymentList = null;

            if (chkReceipt.Checked)
            {

                Page.ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "CloseModal",
                        "top.$.modal.Hide();" +
                        "top.content.location.href='../Accounting/Receipt.aspx?SaleId=" + sale.SaleId.EncryptToHex() + "';",
                        true);

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "CloseModal",
                        "top.$.modal.Hide();" +
                        "top.content.location.href+='?';",
                        true);
            }
            return sale;
        }

        protected Decimal calculateParcelValue(Decimal value, Int32 qtdParcel)
        {
            return value / qtdParcel;
        }
        private String FormatDecimal(String value)
        {
            return value.Replace('_', ' ').Trim();
        }

        private void HandleLegalTicket()
        {
            //Ecf.Provider.AbrePortaSerial();
            //Ecf.Provider.AbreCupom(
            //    Company.LegalEntityProfile.CompanyName,
            //    Company.AddressComp + " - " + Company.AddressNumber,
            //    "", "", "",
            //    Company.LegalEntityProfile.PostalCode,
            //    Company.LegalEntityProfile.IE,
            //    Company.LegalEntityProfile.CNPJ,
            //    "");

            ////
            //// The Sale Item Data
            ////
            //List<SaleItem> list = new List<SaleItem>();
            //DataTable basket = Session["basket"] as DataTable;
            //foreach (DataRow row in basket.Rows)
            //{
            //    Ecf.Provider.VendeItem(
            //        Convert.ToString(row["ProductCode"]),
            //        Convert.ToString(row["ProductName"]).Split('&')[0],
            //        "FF",
            //        "I",
            //        Convert.ToInt16(row["Quantity"]),
            //        2,
            //        Convert.ToDouble(row["UnitPrice"]),
            //        "",
            //        "");
            //}

            //Ecf.Provider.IniciaFechamentoCupom(false, "", 0);

            ////
            //// Parcelas
            ////

            //for (int i = 0; i < PaymentTable.Rows.Count; i++)
            //{
            //    Ecf.Provider.EfetuaFormaPagamento((string)PaymentTable.Rows[i]["FinancierOperationName"], Convert.ToDouble(PaymentTable.Rows[i]["Amount"]));
            //}

            //Ecf.Provider.TerminaFechamentoCupom("Garantia...");


            //Ecf.Provider.FechaPortaSerial();
        }
        protected void btnAddParcel_Click(object sender, ImageClickEventArgs e)
        {
            AddParcel();
            txtAmount.Text = String.Empty;
            ucDueDate.DateTime = null;
            txtQtdParcels.Text = "1";
            btnFinishSale.Enabled = true;
        }
    }


}


