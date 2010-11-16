using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using System.Collections;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

namespace Vivina.Erp.WebUI.InfoControl.Host
{
    public partial class Companies : SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void odsCompanies_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            var htCompany = new Hashtable();
            htCompany.Add("CNPJ", txtCnpj.Text);
            htCompany.Add("Email", txtEmail.Text);
            htCompany.Add("CompanyName", txtCompanyName.Text);

            htCompany.Add("sortExpression", "LastActivityDate");

            htCompany.Add("sortDirection", "desc");

            htCompany.Add("FantasyName", String.Empty);
            htCompany.Add("IE", String.Empty);
            htCompany.Add("StartDate", null);

            htCompany.Add("Phone", String.Empty);
            htCompany.Add("Website", String.Empty);

            e.InputParameters["htCompany"] = htCompany;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdCompanies.DataBind();
        }

        protected void grdCompanies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes["onclick"] = "location='../Administration/Company.aspx?CompanyId=" + grdCompanies.DataKeys[e.Row.RowIndex]["CompanyId"] + "&host=true';";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CompanyManager cManager = new CompanyManager(this);
            char delimiter = new char();
            delimiter = ',';

            //make a list of customers
            String[] lista = Request["chkCompany"].Split(delimiter);

            try
            {
                //delete company
                for (int index = 0; index < lista.Length; index++)
                {
                    cManager.DeleteCompany(Convert.ToInt32(lista[index]));
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert1", "alert('" + ex.Message + "');", true);
                ShowError("Companhia contendo registros relacionados!");
            }

            grdCompanies.DataBind();
        }
    }
}