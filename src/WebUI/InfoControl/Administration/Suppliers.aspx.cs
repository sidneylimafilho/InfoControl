using System;
using System.Collections;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

using InfoControl;
using InfoControl.Web.Security;

[PermissionRequired("Suppliers")]
public partial class Administration_Company_Suppliers : Vivina.Erp.SystemFramework.PageBase
{
    bool isInserting = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });

            txtCNPJ.Text = Convert.ToString(Page.Customization["txtCNPJ"]);
            txtCPF.Text = Convert.ToString(Page.Customization["txtCPF"]);
            txtMail.Text = Convert.ToString(Page.Customization["txtMail"]);
            txtPhone.Text = Convert.ToString(Page.Customization["txtPhone"]);
            cboPageSize.SelectedIndex = Convert.ToInt32(Page.Customization["cboPageSize"]);
            txtSelectedSupplier.Text = Convert.ToString(Page.Customization["SelectedSupplier"]);

        }
    }

    protected void grdSuppliers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            e.Row.Attributes["onclick"] = "location='Supplier.aspx?SupplierId=" + e.Row.DataItem.GetPropertyValue("SupplierId").EncryptToHex() + "';";
        }
    }
    protected void grdSuppliers_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Server.Transfer("Supplier.aspx");
        }
    }


    protected void odsSuppliers_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting Products that are associated with others Tables
        //
        if (e.Exception != null)
        {
            if (e.Exception.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = e.Exception.InnerException as System.Data.SqlClient.SqlException;
                if (err.ErrorCode.Equals(Convert.ToInt32("0x80131904", 16)))
                {
                    ShowError(Resources.Exception.DeletingRegisterWithForeignKey);
                    e.ExceptionHandled = true;
                }
            }
        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        Server.Transfer("Supplier_General.aspx");
    }
    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdSuppliers.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdSuppliers.DataBind();
    }

    [WebMethod]
    public static bool DeleteSupplier(int supplierid, int companyid)
    {
        bool result = true;
        using (SupplierManager supplierManager = new SupplierManager(null))
        {
            try
            {
                supplierManager.Delete(supplierManager.GetSupplier(supplierid, companyid));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grdSuppliers.DataBind();

        Page.Customization["txtCNPJ"] = txtCNPJ.Text;
        Page.Customization["txtCPF"] = txtCPF.Text;
        Page.Customization["txtMail"] = txtMail.Text;
        Page.Customization["txtPhone"] = txtPhone.Text;
        Page.Customization["cboPageSize"] = cboPageSize.SelectedIndex;
        Page.Customization["SelectedSupplier"] = txtSelectedSupplier.Text;
    }

    protected void odsSearchSupplier_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable htSupplier = new Hashtable();
        htSupplier.Add("CompanyId", Company.CompanyId);
        htSupplier.Add("SelectedSupplier", txtSelectedSupplier.Text);
        htSupplier.Add("CPF", Convert.ToString(txtCPF.Text));
        htSupplier.Add("CNPJ", Convert.ToString(txtCNPJ.Text));
        htSupplier.Add("Email", txtMail.Text);
        htSupplier.Add("Phone", txtPhone.Text);
        htSupplier.Add("Ranking", rtnRanking.CurrentRating);

        e.InputParameters["htSupplier"] = htSupplier;
        e.InputParameters["initialLetter"] = ucAlphabeticalPaging.Letter;
    }

    protected void ucAlphabeticalPaging_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdSuppliers.DataBind();
    }
}
