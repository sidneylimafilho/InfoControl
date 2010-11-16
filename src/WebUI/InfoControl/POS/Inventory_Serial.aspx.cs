using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoControl;
using InfoControl.Data;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.BusinessRules.Services;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Web.Security;

namespace Vivina.Erp.WebUI.POS
{
    public partial class Inventory_Serial : Vivina.Erp.SystemFramework.PageBase
    {
        private Int32 inventoryId;
        private Int32 depositId;
        private Int32 productId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["ProductId"]))
            {
                productId = Convert.ToInt32(Request["ProductId"]);
                inventoryId = Convert.ToInt32(Request["InventoryId"]);
                depositId = Convert.ToInt32(Request["DepositId"]);
            }
        }

        protected void odsInventorySerial_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["inventoryId"] = inventoryId;
        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            InventoryManager inventoryManager = new InventoryManager(this);
            InventorySerial inventorySerial = new InventorySerial();
            inventorySerial.CompanyId = Company.CompanyId;
            inventorySerial.DepositId = depositId;
            if (ucDtDueDate.DateTime.HasValue)
                inventorySerial.DueDate = ucDtDueDate.DateTime.Value.Date;
            inventorySerial.Lot = txtLot.Text;
            inventorySerial.Serial = txtSerial.Text;
            inventorySerial.InventoryId = inventoryId;
            inventoryManager.InsertInventorySerial(inventorySerial);
            grdInventorySerial.DataBind();
        }
    }
}
