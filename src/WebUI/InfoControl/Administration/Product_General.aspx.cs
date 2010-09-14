using System;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

using Exception = Resources.Exception;

namespace Vivina.Erp.WebUI.Administration
{
    public partial class Product_General : Vivina.Erp.SystemFramework.PageBase
    {
        private ProductManager _productManager;
        Product originalProduct;
        Product product;

        protected void Page_Load(object sender, EventArgs e)
        {

            txtManufacturer.Attributes["servicepath"] = ResolveUrl("~/Controller/SearchService/SearchManufacturer");

            //
            // Se for inserção então essa página é mostrada fora do iframe, logo precisa 
            // continuar com o titulo, caso contrário remove.
            //
            if (!String.IsNullOrEmpty(Request["pid"]))
                Title = String.Empty;

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request["pid"]))
                {
                    txtProductCode.Text = txtBarCode.Text = Util.GenerateUniqueID();
                    cboBarCodeType.DataBind();
                    ListItem selectedItem = cboBarCodeType.Items.FindByText("EAN13");

                    if (selectedItem != null)
                        cboBarCodeType.SelectedValue = selectedItem.Value;

                    return;
                }

                Page.ViewState["ProductId"] = Request["pid"].DecryptFromHex();
                ShowProduct();
            }
        }

        private void ShowProduct()
        {
            _productManager = new ProductManager(this);
            Product product = _productManager.GetProduct(Convert.ToInt32(Page.ViewState["ProductId"]));

            if (product == null)
                product = _productManager.GetTempProduct(Convert.ToInt32(Page.ViewState["ProductId"]), Company.CompanyId);

            if (product == null)
                return;

            if (product.IsTemp == true)
                btnApproveProduct.Visible = true;

            txtProduct.Name = product.Name;
            txtProductCode.Text = product.ProductCode;
            txtBarCode.Text = product.BarCode;
            txtIdentificationOrPlaca.Text = product.IdentificationOrPlaca;
            txtPatrimonioOrRenavam.Text = product.PatrimonioOrRenavam;
            txtSerialNumberOrChassi.Text = product.SerialNumberOrChassi;
            DescriptionTextBox.Content = product.Description;
            ucCurrFieldIPI.CurrencyValue = product.IPI;
            ucCurrFieldICMS.CurrencyValue = product.ICMS;
            ucCurrFieldWarrantyDays.CurrencyValue = product.WarrantyDays;
            txtFiscalClass.Text = product.FiscalClass;
            chkAddCustomerEquipment.Checked = Convert.ToBoolean(product.AddCustomerEquipmentInSale);
            chkAllowNegativeStock.Checked = Convert.ToBoolean(product.AllowNegativeStock);
            chkAllowSaleBelowCost.Checked = Convert.ToBoolean(product.AllowSaleBelowCost);
            chkDropCompositeInStock.Checked = Convert.ToBoolean(product.DropCompositeInStock);
            ChkIsActive.Checked = Convert.ToBoolean(product.IsActive);


            #region non required/opcional fields

            /*
             * Add fields here that may not be used or only used in some companies(_companies)
             * Here is made a verification if there's value in the field of the DataBank to not assign a
             * null value to a control and prevent exceptions
             */


            if (cboCategories != null && product.CategoryId.HasValue)
                cboCategories.SelectedValue = product.CategoryId.ToString();

            if (txtManufacturer != null && product.ManufacturerId.HasValue)
                txtManufacturer.Text = product.Manufacturer.Name;

            if (cboBarCodeType != null && product.BarCodeTypeId.HasValue)
                cboBarCodeType.SelectedValue = product.BarCodeTypeId.ToString();

            if (chkRequiresAuthorization != null && product.RequiresAuthorization.HasValue)
                chkRequiresAuthorization.Checked = (bool)product.RequiresAuthorization;

            if (txtUnit != null)
                txtUnit.Text = product.Unit;

            if (txtPackage != null)
                txtPackage.Text = product.Package;

            if (txtKeyWord != null)
                txtKeyWord.Text = product.Keywords;

            if (chkIsCasting != null)
                chkIsCasting.Checked = Convert.ToBoolean(product.IsCasting);

            if (chkIsEmphasizeInHome != null)
                chkIsEmphasizeInHome.Checked = Convert.ToBoolean(product.EmphasizeInHome);

            if (ucCurrFieldWheight != null)
                ucCurrFieldWheight.CurrencyValue = product.Weight;

            #endregion
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _productManager = new ProductManager(this);
            product = new Product();
            originalProduct = new Product();

            if (Page.ViewState["ProductId"] != null)
            {
                originalProduct = _productManager.GetProduct(Convert.ToInt32(Page.ViewState["ProductId"]));
            }
            else if (_productManager.CheckIfProductCodeExists(Company.CompanyId, txtProductCode.Text))
            {
                ShowError(Exception.DuplicateCode);
                return;
            }

            if (originalProduct == null)
                originalProduct = _productManager.GetTempProduct(Convert.ToInt32(Page.ViewState["ProductId"]), Company.CompanyId);

            if (originalProduct != null)
                product.CopyPropertiesFrom(originalProduct);

            product.CompanyId = (Int32)Company.MatrixId;

            if (!String.IsNullOrEmpty(cboCategories.SelectedValue))
                product.CategoryId = Convert.ToInt32(cboCategories.SelectedValue);

            product.Name = txtProduct.Name;
            InsertManufacturer(product);

            product.ProductCode = txtProductCode.Text;
            product.BarCodeTypeId = Convert.ToInt32(cboBarCodeType.SelectedValue);
            product.BarCode = txtBarCode.Text;
            product.IdentificationOrPlaca = txtIdentificationOrPlaca.Text;
            product.PatrimonioOrRenavam = txtPatrimonioOrRenavam.Text;

            product.SerialNumberOrChassi = txtSerialNumberOrChassi.Text;
            product.IPI = ucCurrFieldIPI.CurrencyValue;
            product.ICMS = ucCurrFieldICMS.CurrencyValue;
            product.FiscalClass = txtFiscalClass.Text;
            product.WarrantyDays = ucCurrFieldWarrantyDays.IntValue;

            product.Description = DescriptionTextBox.Content;
            product.IsActive = ChkIsActive.Checked;
            product.DropCompositeInStock = Convert.ToBoolean(chkDropCompositeInStock.Checked);
            product.AddCustomerEquipmentInSale = Convert.ToBoolean(chkAddCustomerEquipment.Checked);
            product.AllowNegativeStock = Convert.ToBoolean(chkAllowNegativeStock.Checked);

            product.AllowSaleBelowCost = Convert.ToBoolean(chkAllowSaleBelowCost.Checked);
            product.ModifiedDate = DateTime.Now;
            product.RequiresAuthorization = chkRequiresAuthorization.Checked;

            #region Optional Fields
            /*
             * Add fields here that may not be used or only used in some companies(_companies)
             * Here is made a verification if there's value in the control to not assign a value
             * to the DataBank from a null control and prevent exceptions
             */

            if (txtUnit != null)
                product.Unit = txtUnit.Text;

            if (txtKeyWord != null)
                product.Keywords = txtKeyWord.Text;

            if (txtPackage != null)
                product.Package = txtPackage.Text;

            if (chkIsEmphasizeInHome != null)
                product.EmphasizeInHome = chkIsEmphasizeInHome.Checked;

            if (chkIsCasting != null)
                product.IsCasting = chkIsCasting.Checked;

            if (ucCurrFieldWheight != null)
                product.Weight = ucCurrFieldWheight.CurrencyValue;



            #endregion

            if (!String.IsNullOrEmpty(Request["IsTemp"]))
                product.IsTemp = true;

            string redirectUrl = "";

            if (originalProduct.ProductId == 0)
            {
                product.CreatedByUser = User.Identity.UserName;
                _productManager.Insert(product);
            }
            else
            {
                product.ModifiedByUser = User.Identity.UserName;
                _productManager.Update(originalProduct, product);
                redirectUrl = "parent.";
            }

            if (((WebControl)sender).ID == "btnSaveAndNew")
                redirectUrl += "location='Product_General.aspx';";
            else
            {
                if (product.IsTemp == true)
                    redirectUrl += "location='../Purchasing/PurchaseRequests.aspx';";
                else
                    redirectUrl += "location='Product.aspx?ProductId=" + product.ProductId.EncryptToHex() + "'";
            }

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "", redirectUrl, true);
        }

        protected void odsCategories_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //
            //Seta os valores necessários para o Manager executar a query de busca
            //
            e.InputParameters["companyId"] = Company.MatrixId;
        }

        private Product InsertManufacturer(Product product)
        {
            //
            //Função que valida o FABRICANTE
            //
            var manufacturerManager = new ManufacturerManager(this);

            if (!String.IsNullOrEmpty(txtManufacturer.Text))
                product.ManufacturerId = manufacturerManager.TryManufacturer(txtManufacturer.Text);
            
            return product;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["pid"]))
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "", "parent.location='Products.aspx';", true);
            else
                Response.Redirect("Products.aspx");
        }
    }
}