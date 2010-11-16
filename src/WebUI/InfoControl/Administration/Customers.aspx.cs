using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;

[PermissionRequired("Customers")]
public partial class Company_Customers : Vivina.Erp.SystemFramework.PageBase
{

    CustomerManager customerManager;
    ContactManager contactManager;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboPageSize.Items.Add(new ListItem() { Value = Int16.MaxValue.ToString(), Text = "Todos" });

            cboCustomerType.DataBind();


            txtMail.Text = Convert.ToString(Page.Customization["txtMail"]);
            txtEquipment.Text = Convert.ToString(Page.Customization["txtEquipment"]);
            txtPhone.Text = Convert.ToString(Page.Customization["txtPhone"]);
            txtCNPJ.Text = Convert.ToString(Page.Customization["txtCNPJ"]);
            txtCPF.Text = Convert.ToString(Page.Customization["txtCPF"]);
            cboPageSize.SelectedValue = Convert.ToString(Page.Customization["cboPageSize"]);
            txtSelectedCustomer.Text = Convert.ToString(Page.Customization["SelectedCustomer"]);

            if (Page.Customization["CustomerType"] != null && cboCustomerType.Items.FindByValue(Convert.ToString(Page.Customization["CustomerType"])) != null)
                cboCustomerType.SelectedValue = Convert.ToString(Page.Customization["CustomerType"]);

        }


    }

    protected void odsCustomers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.MatrixId.Value;

        var represetantUser = new RepresentantManager(this).GetRepresentantUser(User.Identity.UserId);

        if (represetantUser != null)
            e.InputParameters["representantId"] = represetantUser.RepresentantId;

        e.InputParameters["initialLetter"] = ucAlphabeticalPaging.Letter;
    }

    protected void grdCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //
        //Verify if the row is a data row, to not get header and footer
        //
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
            e.Row.Attributes["onclick"] = "location='Customer.aspx?CustomerId=" + grdCustomers.DataKeys[e.Row.RowIndex]["CustomerId"] + "' ;";
        }
    }

    protected void grdCustomers_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            Response.Redirect("Customer.aspx");
        }
    }

    protected void grdCustomers_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        Context.Items["CustomerId"] = grdCustomers.DataKeys[e.NewSelectedIndex]["CustomerId"].ToString();
        if (grdCustomers.DataKeys[e.NewSelectedIndex]["CPF"] != null)
        {
            if (grdCustomers.DataKeys[e.NewSelectedIndex]["CPF"].ToString().Length >= 2)
            {
                Context.Items["CPF"] = grdCustomers.DataKeys[e.NewSelectedIndex]["CPF"].ToString();
            }
        }
        Server.Transfer("Customer.aspx");
    }

    protected void odsCustomers_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        // This method is to not allow deleting items that are associated with others Tables
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

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdCustomers.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdCustomers.DataBind();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {


        Page.Customization["txtMail"] = txtMail.Text;
        Page.Customization["txtEquipment"] = txtEquipment.Text;
        Page.Customization["txtCNPJ"] = txtCNPJ.Text;
        Page.Customization["txtCPF"] = txtCPF.Text;
        Page.Customization["txtPhone"] = txtPhone.Text;
        Page.Customization["cboPageSize"] = cboPageSize.SelectedValue;
        Page.Customization["SelectedCustomer"] = txtSelectedCustomer.Text;
        Page.Customization["CustomerType"] = cboCustomerType.SelectedValue;
        grdCustomers.DataSourceID = odsSearchCustomer.ID;
        grdCustomers.DataBind();
    }

    protected void odsSearchCustomer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        CustomerManager customerManager = new CustomerManager(this);
        Hashtable htCustomer = new Hashtable();

        htCustomer.Add("CompanyId", Company.MatrixId); 
        htCustomer.Add("SelectedCustomer", txtSelectedCustomer.Text);
        htCustomer.Add("CPF", txtCPF.Text);
        htCustomer.Add("CNPJ", txtCNPJ.Text);
        htCustomer.Add("Email", txtMail.Text);
        htCustomer.Add("Phone", txtPhone.Text);
        htCustomer.Add("EquipmentName", txtEquipment.Text);
        htCustomer.Add("Ranking", rtnRanking.CurrentRating);
        htCustomer.Add("CustomerTypeId", cboCustomerType.SelectedValue);

        var represetantUser = new RepresentantManager(this).GetRepresentantUser(User.Identity.UserId);

        if (represetantUser != null)
            htCustomer.Add("representantId", represetantUser.RepresentantId.ToString());
        else
            htCustomer.Add("representantId", String.Empty);

        e.InputParameters["htCustomer"] = htCustomer;
    }

    protected void ucAlphabeticalPaging_SelectedLetter(object sender, SelectedLetterEventArgs e)
    {
        grdCustomers.DataBind();
    }

    protected void odsCustomerType_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    [WebMethod]
    public static bool DeleteCustomer(int customerid, int companyid)
    {
        bool result = true;
        using (CustomerManager customerManager = new CustomerManager(null))
        {
            try
            {
                customerManager.Delete(customerManager.GetCustomer(customerid, companyid));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    protected void btnImportExcelFile_Click(object sender, EventArgs e)
    {
        if (!fupImportExcelFile.FileName.EndsWith("xls"))
        {
            ShowError("Formato de arquivo incorreto! Importe um arquivo de extensão '.xls' ");
            return;
        }

        var fileName = Path.GetTempFileName() + ".xls";
        fupImportExcelFile.SaveAs(fileName);

        var message = string.Empty;

        new CustomerImporter(this).ImportDataFromExcelFile(Company.CompanyId, User.Identity.UserId, fileName, out message);

        File.Delete(fileName);

        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('" + message + "');", true);

        grdCustomers.DataBind();
    }


}
