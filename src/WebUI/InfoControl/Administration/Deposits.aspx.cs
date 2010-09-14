using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using System.Web.Services;
using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;


namespace Vivina.Erp.WebUI.Administration
{
    public partial class Deposits : Vivina.Erp.SystemFramework.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)

        }

        protected void odsDeposit_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["companyId"] = Company.CompanyId;
        }

        protected void grdDeposits_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = "location='Deposit.aspx?DepositId=" + Convert.ToString(grdDeposits.DataKeys[e.Row.RowIndex]["DepositId"]).EncryptToHex() + "';";


            }
        }


        [WebMethod]
        public static bool DeleteDeposit(Int32 companyId, Int32 depositId)
        {
            bool result = true;
            using (DepositManager depositManager = new DepositManager(null))
            {
                try
                {
                    depositManager.Delete(depositManager.GetDeposit(depositId));
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    result = false;
                }
            }
            return result;
        }


    }
}
