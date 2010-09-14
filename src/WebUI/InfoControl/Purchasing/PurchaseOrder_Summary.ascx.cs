using System;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


public partial class Company_POS_PurchaseOrder_Summary : Vivina.Erp.SystemFramework.UserControlBase<Company_POS_PurchaseOrder>
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            if (Session["SupplierId"] != null && Convert.ToBoolean(Session["SummaryFilled"]) != true)
            {
                var supplierManager = new SupplierManager(this);
                Supplier supplier = supplierManager.GetSupplier(Convert.ToInt32(Session["SupplierId"]),
                                                                Page.Company.CompanyId);
                Session["SummaryFilled"] = true;
                string name;
                if (supplier.LegalEntityProfile != null)
                {
                    name = supplier.LegalEntityProfile.FantasyName;
                }
                else
                {
                    name = supplier.Profile.Name;
                }
                DescriptionTextBox.Html =
                    @"<p align=center>Sem mais para o momento e no aguardo de um contato, 
                    antecipamos agradecimentos<br><br>Atenciosamente,<br>____________________________________<br>" +
                    Page.Company.LegalEntityProfile.FantasyName +
                    @"<br><br><br><strong>DE ACORDO COM AS CONDIÇÕES DA PROPOSTA, <br><br></strong>" +
                    "Rio de Janeiro,&nbsp;" + DateTime.Now.ToLongDateString() +
                    @"<br><br>____________________________________<br>" + name + "</p>";

            }

        Page.ViewState["DescriptionCompany"] = DescriptionTextBox.Content;

    }
}