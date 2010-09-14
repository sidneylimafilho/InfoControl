using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vivina.Erp.DataClasses;
using InfoControl;
using Vivina.Erp.SystemFramework;


public partial class Company_Administration_Customer_Equipment : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            frmEquipment.Visible = false;
            Page.ViewState["CustomerId"] = Request["CustomerId"].DecryptFromHex();
        }
    }

    protected void odsEquipments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["customerId"] = Page.ViewState["CustomerId"];
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        grdEquipments.Visible = false;
    }

    protected void grdEquipments_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PostBackOptions postOptions = new PostBackOptions(grdEquipments, "Select$" + e.Row.RowIndex.ToString());
            string insertScript = Page.ClientScript.GetPostBackEventReference(postOptions);

            for (int a = 0; a < (e.Row.Cells.Count - 2); a++)
                e.Row.Cells[a].Attributes.Add("onclick", insertScript);

            e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble == true;javascript:if(confirm('Deseja realmente apagar a linha solicitada') == false)return false;");
        }
    }

    protected void grdEquipments_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Page.ViewState["customerEquipmentId"] = grdEquipments.DataKeys[grdEquipments.SelectedIndex]["CustomerEquipmentId"].ToString();
        frmEquipment.PageIndex = grdEquipments.SelectedIndex;
        frmEquipment.ChangeMode(FormViewMode.Edit);
        grdEquipments.Visible = false;
        frmEquipment.Visible = true;
    }

    protected void grdEquipments_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression == "Insert")
        {
            frmEquipment.DataBind();
            frmEquipment.PageIndex = -1;
            frmEquipment.ChangeMode(FormViewMode.Insert);

            grdEquipments.Visible = false;
            frmEquipment.Visible = true;
            e.Cancel = true;
        }
    }

    protected void btnAddCustomerEquipment_Click(object sender, EventArgs e)
    {
        grdEquipments.Visible = false;
        frmEquipment.Visible = true;
    }

    protected void odsEquipments_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        CustomerEquipment customerEquipment = (CustomerEquipment)e.InputParameters["entity"];
        customerEquipment.CustomerId = Convert.ToInt32(Page.ViewState["CustomerId"]);
        customerEquipment.CompanyId = Company.CompanyId;
    }

    protected void frmEquipment_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            frmEquipment.Visible = false;
            grdEquipments.Visible = true;
        }

      
    }

    protected void odsEquipments_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        grdEquipments.Visible = true;
        frmEquipment.Visible = false;
    }

    protected void odsEquipments_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        grdEquipments.Visible = true;
        frmEquipment.Visible = false;
    }

    protected void frmEquipment_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.Values["WarrantyBeginDate"].ToString().RemoveMask()))
            e.Values["WarrantyBeginDate"] = Convert.ToDateTime(e.Values["WarrantyBeginDate"].ToString().RemoveMask()).Date;
        else
            e.Values["WarrantyBeginDate"] = null;

        if (!String.IsNullOrEmpty(e.Values["WarrantyEndDate"].ToString().RemoveMask()))
            e.Values["WarrantyEndDate"] = Convert.ToDateTime(e.Values["WarrantyEndDate"].ToString().RemoveMask()).Date;
        else
            e.Values["WarrantyEndDate"] = null;


        if (!String.IsNullOrEmpty(e.Values["FactoringYear"].ToString().RemoveMask()))
            e.Values["FactoringYear"] = Convert.ToInt32(e.Values["FactoringYear"].ToString().RemoveMask());
        else
            e.Values["FactoringYear"] = null;

        if (!String.IsNullOrEmpty(e.Values["ModelYear"].ToString().RemoveMask()))
            e.Values["ModelYear"] = Convert.ToInt32(e.Values["ModelYear"].ToString().RemoveMask());
        else
            e.Values["ModelYear"] = null;

        if (String.IsNullOrEmpty(e.Values["ContractId"].ToString()))
            e.Values["ContractId"] = null;

        if (e.Values["Comments"].ToString().Length > 200)
            e.Values["Comments"] = e.Values["Comments"].ToString().Substring(0, 200);
    }

    protected void frmEquipment_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.NewValues["WarrantyBeginDate"].ToString().RemoveMask()))
            e.NewValues["WarrantyBeginDate"] = Convert.ToDateTime(e.NewValues["WarrantyBeginDate"].ToString().RemoveMask()).Date;
        else
            e.NewValues["WarrantyBeginDate"] = null;

        if (!String.IsNullOrEmpty(e.NewValues["WarrantyEndDate"].ToString().RemoveMask()))
            e.NewValues["WarrantyEndDate"] = Convert.ToDateTime(e.NewValues["WarrantyEndDate"].ToString().RemoveMask()).Date;
        else
            e.NewValues["WarrantyEndDate"] = null;


        if (!String.IsNullOrEmpty(e.NewValues["FactoringYear"].ToString().RemoveMask()))
            e.NewValues["FactoringYear"] = Convert.ToInt32(e.NewValues["FactoringYear"].ToString().RemoveMask());
        else
            e.NewValues["FactoringYear"] = null;

        if (!String.IsNullOrEmpty(e.NewValues["ModelYear"].ToString().RemoveMask()))
            e.NewValues["ModelYear"] = Convert.ToInt32(e.NewValues["ModelYear"].ToString().RemoveMask());
        else
            e.NewValues["ModelYear"] = null;



        if (!String.IsNullOrEmpty(e.NewValues["ContractId"].ToString()))
            e.NewValues["ContractId"] = Convert.ToInt32(e.NewValues["ContractId"].ToString());
        else
            e.NewValues["ContractId"] = null;

        if (e.NewValues["Comments"].ToString().Length > 200)
            e.NewValues["Comments"] = e.NewValues["Comments"].ToString().Substring(0, 200);
    }

    protected void odsContracts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["customerId"] = Page.ViewState["CustomerId"];
    }

    protected void odsServiceOrder_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (Page.ViewState["customerEquipmentId"] != null)
            e.InputParameters["customerEquipmentId"] = Convert.ToInt32(Page.ViewState["customerEquipmentId"]);
    }

    protected void grdServiceOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        Context.Items["ServiceOrderId"] = (frmEquipment.FindControl("grdServiceOrder") as GridView).DataKeys[(frmEquipment.FindControl("grdServiceOrder") as GridView).SelectedIndex]["ServiceOrderId"].ToString();
        Server.Transfer("../Services/ServiceOrder.aspx");
    }

    protected void grdServiceOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PostBackOptions postOptions = new PostBackOptions((frmEquipment.FindControl("grdServiceOrder") as GridView), "Select$" + e.Row.RowIndex.ToString());
            String insertScript = Page.ClientScript.GetPostBackEventReference(postOptions);
            for (int a = 0; a < e.Row.Cells.Count; a++)
                e.Row.Cells[a].Attributes.Add("onclick", insertScript);
        }
    }

    protected void odsEquipments_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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
        grdEquipments.DataBind();
    }
}




