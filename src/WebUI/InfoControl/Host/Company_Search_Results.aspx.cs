using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.BusinessRules;
using System.Reflection;
using InfoControl;

public partial class Company_Search_Results : Vivina.Erp.SystemFramework.PageBase
{

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        CompanyManager cManager = new CompanyManager(this);
        char delimiter = new char();
        delimiter = ',';

        //make a list of customers
        String[] lista = Request["chkCustomer"].Split(delimiter);

        try
        {
            //delete company
            for (int index = 0; index < lista.Length; index++)
            {   //DeleteRelationshipsOfCompany(Convert.ToInt32(lista[index]));
                cManager.DeleteCompany(Convert.ToInt32(lista[index]));
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('" + ex.Message + "');", true);
            ShowError("Compahia contendo registros relacionados!");
        }

        grvCustomer.DataBind();
    }
    /// <summary>
    /// This method delete Relationships of specific Company
    /// </summary>
    /// <param name="companyId"></param>
    public void DeleteRelationshipsOfCompany(Int32 companyId)
    {
        var company = new CompanyManager(this).GetCompany(Convert.ToInt32(companyId));

        foreach (PropertyInfo propertyItem in company.GetType().GetProperties())
        {
            if (propertyItem.Name != "CompanyId")
            {
                company.SetPropertyValue(propertyItem.Name, null);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Company_Search.aspx");
    }
    protected void grvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void grvCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["CompanyId"] = grvCustomer.DataKeys[grvCustomer.SelectedIndex]["CompanyId"].ToString();
        Server.Transfer("../Administration/Company.aspx");
    }
    protected void odsCustomer_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["htCompany"] = (Hashtable)Page.ViewState["htCompany"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Page.ViewState["htCompany"] = Context.Items["htCompany"];
    }

    protected void grvCustomer_Sorting(object sender, GridViewSortEventArgs e)
    {
        var ht = (Hashtable)Page.ViewState["htCompany"];
        ht["sortExpression"] = e.SortExpression;
        ht["sortDirection"] = e.SortDirection;
    }
}
