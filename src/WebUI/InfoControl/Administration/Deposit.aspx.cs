using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;
using Vivina.Erp.SystemFramework;
using InfoControl.Web.Security;

using InfoControl;
using InfoControl.Data;
using InfoControl;


[PermissionRequired("Deposits")]
public partial class Company_Deposit : Vivina.Erp.SystemFramework.PageBase
{
    private Deposit original_deposit;
    Deposit deposit;
    DepositManager depositManager;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request["DepositId"]))
            {
                Page.ViewState["DepositId"] = Request["DepositId"];
                ShowDeposit();
            }
        }

    }


    private void ShowDeposit()
    {
        deposit = new DepositManager(this).GetDeposit(Convert.ToInt32(Page.ViewState["DepositId"]));

        txtName.Text = deposit.Name;
        ucCurrFieldFifthWeekGoal.CurrencyValue = deposit.FifthWeekGoal;
        ucCurrFieldFirstWeekGoal.CurrencyValue = deposit.FirstWeekGoal;
        ucCurrFieldForthWeekGoal.CurrencyValue = deposit.ForthWeekGoal;
        ucCurrFieldMonthlyGoal.CurrencyValue = deposit.MonthlyGoal;
        ucCurrFieldSecondWeekGoal.CurrencyValue = deposit.SecondWeekGoal;
        ucCurrFieldThirdWeekGoal.CurrencyValue = deposit.ThirdWeekGoal;
        ucDepositAddress.AddressComp = deposit.AddressComp;
        ucDepositAddress.PostalCode = deposit.PostalCode;
        ucDepositAddress.AddressNumber = deposit.AddressNumber;
        ucDepositAddress.AddressNumber = deposit.AddressNumber;

    }

    private void SaveDeposit()
    {

        depositManager = new DepositManager(this);
        original_deposit = depositManager.GetDeposit(Convert.ToInt32(Page.ViewState["DepositId"]));
        deposit = new Deposit();

        if (original_deposit != null)
            deposit.CopyPropertiesFrom(original_deposit);

        deposit.Name = txtName.Text;
        deposit.CompanyId = Company.CompanyId;
        deposit.PostalCode = ucDepositAddress.PostalCode;
        deposit.AddressComp = ucDepositAddress.AddressComp;
        deposit.AddressNumber = ucDepositAddress.AddressNumber;


        if (ucCurrFieldFirstWeekGoal.CurrencyValue.HasValue)
            deposit.FirstWeekGoal = ucCurrFieldFirstWeekGoal.CurrencyValue.Value;

        if (ucCurrFieldSecondWeekGoal.CurrencyValue.HasValue)
            deposit.SecondWeekGoal = ucCurrFieldSecondWeekGoal.CurrencyValue.Value;

        if (ucCurrFieldThirdWeekGoal.CurrencyValue.HasValue)
            deposit.ThirdWeekGoal = ucCurrFieldThirdWeekGoal.CurrencyValue.Value;

        if (ucCurrFieldForthWeekGoal.CurrencyValue.HasValue)
            deposit.ForthWeekGoal = ucCurrFieldForthWeekGoal.CurrencyValue.Value;

        if (ucCurrFieldFirstWeekGoal.CurrencyValue.HasValue)
            deposit.FifthWeekGoal = ucCurrFieldFifthWeekGoal.CurrencyValue.Value;

        if(ucCurrFieldMonthlyGoal.CurrencyValue.HasValue)
            deposit.MonthlyGoal = ucCurrFieldMonthlyGoal.CurrencyValue.Value;


        if (original_deposit != null)
            depositManager.Update(original_deposit, deposit);
        else
            depositManager.Insert(deposit);

        Response.Redirect("Deposits.aspx");

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveDeposit();

    }


}
