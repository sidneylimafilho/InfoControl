using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl.Web.Security;

[PermissionRequired("DropPayout")]
public partial class Company_POS_DropPayout : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Deposit == null)
            pnlDeposit.Visible = true;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Focus();
        DropPayout dropPayout = new DropPayout();
        DropPayoutManager dropPayoutManager = new DropPayoutManager(this);

        if (ucCurrFieldAmount.CurrencyValue.HasValue)
            dropPayout.Amount = ucCurrFieldAmount.CurrencyValue.Value;

        if (rbtSangria.Checked)
            dropPayout.Amount = Decimal.Negate(dropPayout.Amount);

        dropPayout.Comment = txtReason.Text;
        dropPayout.CompanyId = Company.CompanyId;

        if (Deposit == null)
            dropPayout.DepositId = Convert.ToInt32(cboDeposit.SelectedValue);
        else
            dropPayout.DepositId = Deposit.DepositId;
                
        dropPayout.ModifiedDate = DateTime.Now;
        dropPayout.UserId = User.Identity.UserId;
        dropPayoutManager.Insert(dropPayout);

        txtReason.Text = String.Empty;
        ucCurrFieldAmount.CurrencyValue = null;
        rbtSangria.Checked = true;
        Server.Transfer("DropPayoutReport.aspx");
    }

    protected void odsDeposits_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["companyId"] = Company.CompanyId;
    }
}
