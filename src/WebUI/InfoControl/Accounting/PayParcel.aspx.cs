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

using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;

public partial class Company_Accounting_PayParcel : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int ParcelId = Convert.ToInt16(Request.QueryString["ParcelId"].ToString());
            ParcelsManager pManager = new ParcelsManager(this);
            Parcel parcel = pManager.GetParcel(ParcelId, Company.CompanyId);
            txtEffectedAmount.Text = parcel.Amount.ToString();
            txtEffectedDate.Text = DateTime.Today.ToShortDateString().Replace("00:00:00", "");
            if (Request.QueryString["Mode"] != null)
            {
                pnlAccount.Visible = true;
                cboAccount.DataBind();

                if (parcel.RecurrentPeriod == 7)
                    RadioButton1.Checked = true;
                else if (parcel.RecurrentPeriod == 15)
                    RadioButton2.Checked = true;
                else if (parcel.RecurrentPeriod == 30)
                    RadioButton3.Checked = true;
                else if (parcel.RecurrentPeriod == 365)
                    RadioButton4.Checked = true;

                if (Convert.ToBoolean(parcel.IsRecurrent))
                {
                    chkRecurrent.Checked = true;
                    lblRecurrent.Visible = chkRecurrent.Checked;
                    RadioButton1.Visible = chkRecurrent.Checked;
                    RadioButton2.Visible = chkRecurrent.Checked;
                    RadioButton3.Visible = chkRecurrent.Checked;
                    RadioButton4.Visible = chkRecurrent.Checked;
                }

            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "top.$.LightBoxObject.close();", true);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        int ParcelId = Convert.ToInt16(Request.QueryString["ParcelId"].ToString());
        ParcelsManager pManager = new ParcelsManager(this);
        Parcel originalParcel = pManager.GetParcel(ParcelId, Company.CompanyId);
        Parcel parcel = pManager.GetParcel(ParcelId, Company.CompanyId);
        parcel.EffectedAmount = Convert.ToDecimal(txtEffectedAmount.Text);
        parcel.EffectedDate = Convert.ToDateTime(txtEffectedDate.Text);

        try
        {
            pManager.Update(originalParcel, parcel);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        if (chkRecurrent.Checked)
        {
            parcel.IsRecurrent = true;
            if (RadioButton1.Checked)
                parcel.RecurrentPeriod = 7;
            else if (RadioButton2.Checked)
                parcel.RecurrentPeriod = 15;
            else if (RadioButton3.Checked)
                parcel.RecurrentPeriod = 30;
            else
                parcel.RecurrentPeriod = 365;

            pManager.Insert(parcel, new FinancierCondition());
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "modal", "top.$.modal.Hide();", true);
    }

    protected void odsAccount_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["CompanyId"] = Company.CompanyId;
    }

    protected void chkRecurrent_CheckedChanged(object sender, EventArgs e)
    {
        lblRecurrent.Visible = chkRecurrent.Checked;
        RadioButton1.Visible = chkRecurrent.Checked;
        RadioButton2.Visible = chkRecurrent.Checked;
        RadioButton3.Visible = chkRecurrent.Checked;
        RadioButton4.Visible = chkRecurrent.Checked;
    }
}
