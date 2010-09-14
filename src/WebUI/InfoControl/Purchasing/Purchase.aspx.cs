using System;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


namespace Vivina.Erp.WebUI.Purchasing
{
    public partial class CompanyPosPurchase : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var cManager = new CompanyManager(this);
                CompanyConfiguration cSettings = Company.CompanyConfiguration;

                if (cSettings != null)
                {
                    lblHeader.Text = cSettings.ReportHeader;
                    lblFooter.Text = cSettings.ReportFooter;
                }
            }

            var purchaseOrder = new PurchaseOrder();
            var purchaseOrderManager = new PurchaseOrderManager(this);
            var supplierManager = new SupplierManager(this);

            rptProduct.DataSource = Session["_productList"];
            rptProduct.DataBind();

            //
            // Este bloco formata os dados de endereço do cliente                        
            //        

            #region Endereço

            lblLocalDate.Text = "Rio de Janeiro " + DateTime.Now.ToLongDateString();
            if (Session["SupplierId"] != null)
            {
                Supplier supplier = supplierManager.GetSupplier((int) Session["SupplierId"], (int) Company.MatrixId);

                if (supplier != null)
                {
                    if (supplier.LegalEntityProfile != null)
                    {
                        lblSupplierAddress.Text = supplier.LegalEntityProfile.Address.Name
                                                  + " " + supplier.LegalEntityProfile.AddressNumber
                                                  + " " + supplier.LegalEntityProfile.AddressComp;

                        lblSupplierLocalization.Text = supplier.LegalEntityProfile.Address.City
                                                       + " - " + supplier.LegalEntityProfile.Address.Neighborhood
                                                       + ", " + supplier.LegalEntityProfile.Address.StateId;

                        lblPostalCode.Text = "CEP: " + supplier.LegalEntityProfile.Address.PostalCode;

                        lblSupplierPhone.Text = "Tel: " + supplier.LegalEntityProfile.Phone.Replace("(__)____-____", "") ??
                                                supplier.LegalEntityProfile.Phone.Replace("(__)____-____", "");
                        lblCNPJ_CPF.Text = supplier.LegalEntityProfile.CNPJ ?? supplier.LegalEntityProfile.CNPJ;

                        lblSupplierName.Text = supplier.LegalEntityProfile.CompanyName + "/";
                    }
                    else
                    {
                        lblSupplierAddress.Text = supplier.Profile.Address.Name
                                                  + " " + supplier.Profile.AddressNumber
                                                  + " " + supplier.Profile.AddressComp;

                        lblSupplierLocalization.Text = supplier.Profile.Address.City
                                                       + " - " + supplier.Profile.Address.Neighborhood
                                                       + ", " + supplier.Profile.Address.StateId;

                        lblPostalCode.Text = "CEP: " + supplier.Profile.Address.PostalCode;

                        lblSupplierPhone.Text = "Tel: " + supplier.Profile.Phone.Replace("(__)____-____", "") ??
                                                supplier.Profile.Phone.Replace("(__)____-____", "");
                        lblCNPJ_CPF.Text = supplier.Profile.CPF ?? supplier.Profile.CPF;

                        lblSupplierName.Text = supplier.Profile.Name + "/";
                    }

                    #endregion

                    lblFooter.Text =
                        @"<p align=center>Sem mais para o momento e no aguardo de um contato, 
                antecipamos agradecimentos<br><br>Atenciosamente,<br><br>____________________________________
                <br>" +
                        Company.LegalEntityProfile.CompanyName +
                        @"<br><br><br><strong>DE ACORDO COM AS CONDIÇÕES DA PROPOSTA,
                <br><br></strong>" +
                        "Rio de Janeiro,&nbsp;" + DateTime.Now.ToLongDateString() +
                        @"<br><br>____________________________________<br>";
                    if (supplier != null)
                    {
                        if (supplier.Profile != null)
                            lblFooter.Text += supplier.Profile.Name + "</p>";
                    }
                    else
                    {
                        lblFooter.Text += supplier.LegalEntityProfile.CompanyName + "</p>";
                    }
                }
            }
        }

        protected void odsBudgetItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["CompanyId"] = Company.CompanyId;
        }
    }
}