using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules.Services;
using InfoControl;
using InfoControl.Web.Security;
using Telerik.Web.UI;

[PermissionRequired("ServiceOrders")]
public partial class InfoControl_Services_ServiceOrders : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucDateTimeInterval.DateInterval = new DateTimeInterval(DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(1));           
            cboPageSize.Items.Add(new ListItem() { Value = Int32.MaxValue.ToString(), Text = "Todos" });

            if (Page.Customization["cboServiceOrderType"] != null)
                cboServiceOrderType.SelectedIndex = Convert.ToInt32(Page.Customization["cboServiceOrderType"]);

            if (Page.Customization["cboStatus"] != null)
                cboStatus.SelectedIndex = Convert.ToInt32(Page.Customization["cboStatus"]);

            if (Page.Customization["cboTechnicalUser"] != null)
                cboTechnicalUser.SelectedValue = Convert.ToString(Page.Customization["cboTechnicalUser"]);

            if (Page.Customization["competency"] != null && cboCompetency.Items.FindByText(Convert.ToString(Page.Customization["competency"])) != null)
                cboCompetency.Text = Convert.ToString(Page.Customization["competency"]);

            txtCustomer.Text = Convert.ToString(Page.Customization["customerName"]);
        }
    }

    protected void odsServiceOrders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
        e.InputParameters["dateTimeInterval"] = ucDateTimeInterval.DateInterval;

        e.InputParameters["customerName"] = txtCustomer.Text;

        if (!String.IsNullOrEmpty(cboServiceOrderType.SelectedValue))
            e.InputParameters["serviceOrderTypeId"] = cboServiceOrderType.SelectedValue;

        if (!String.IsNullOrEmpty(cboStatus.SelectedValue))
            e.InputParameters["serviceOrderStatusId"] = cboStatus.SelectedValue;

        if (!String.IsNullOrEmpty(cboOrganizationLevelTree.SelectedValue))
            e.InputParameters["organizationLevelId"] = cboOrganizationLevelTree.SelectedValue;

        if (!String.IsNullOrEmpty(cboTechnicalUser.SelectedValue))
            e.InputParameters["employeeId"] = cboTechnicalUser.SelectedValue;

        if (!String.IsNullOrEmpty(cboCompetency.Text))
            e.InputParameters["competency"] = cboCompetency.Text;
    }

    protected void odsServiceOrders_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
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

    protected void odsServiceOrders_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdServiceOrders.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        grdServiceOrders.DataBind();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        Page.Customization["cboServiceOrderType"] = cboServiceOrderType.SelectedIndex;
        Page.Customization["cboStatus"] = cboStatus.SelectedIndex;
        Page.Customization["cboTechnicalUser"] = cboTechnicalUser.SelectedValue;
        Page.Customization["competency"] = cboCompetency.Text;
        Page.Customization["customerName"] = txtCustomer.Text;

        grdServiceOrders.DataBind();
    }

    [WebMethod]
    public static bool DeleteServiceOrder(int serviceOrderId)
    {
        bool result = true;
        using (ServicesManager servicesManager = new ServicesManager(null))
        {
            try
            {
                servicesManager.DeleteServiceOrder(servicesManager.GetServiceOrder(serviceOrderId));
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                result = false;
            }
        }
        return result;
    }

    protected void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }

    protected void grdServiceOrders_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.EditFormItem || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            e.Item.Attributes["onclick"] = "location='ServiceOrder.aspx?ServiceOrderId=" + e.Item.DataItem.GetPropertyValue("ServiceOrderId").EncryptToHex() + "';";
            //
            // Cancel a nested event fires
            //
            e.Item.Cells[e.Item.Cells.Count - 1].Attributes.Add("onclick", "event.cancelBubble=true;javascript:if(confirm('O registro será excluido e não poderá mais ser recuperado, deseja realmente efetuar a operação?') == false) return false;");
        }
    }

    protected void grdServiceOrders_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Delete" && e.Item.ItemType != GridItemType.GroupFooter)
        {
            var servicesManager = new ServicesManager(this);
            if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceOrderId"] != null)
            {
                var serviceOrderId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceOrderId"]);
                servicesManager.DeleteServiceOrder(servicesManager.GetServiceOrder(serviceOrderId));
            }
            grdServiceOrders.DataBind();
        }
    }



}
