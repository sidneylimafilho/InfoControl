using System;
using System.ComponentModel;
using System.Web.UI;
using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;


public partial class Company_GeneralProfile : Vivina.Erp.SystemFramework.UserControlBase
{
    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public Profile ProfileEntity
    {
        get
        {
            if (rdbCPF.Checked)            
                return ucProfile.ProfileEntity;

            
            return null;
        }
        set
        {
            ucProfile.ProfileEntity = value;
            if (value != null)
            {
                //Page.ViewState["ProfileId"] = value.ProfileId;
                rdbCPF.Checked = true;
                rdbCNPJ.Checked = false;
                //
                // Disable the radio buttons
                //
                rdbCPF.Enabled = false;
                rdbCNPJ.Enabled = false;

                ucCompanyProfile.Visible = false;
                ucProfile.Visible = true;
            }
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public LegalEntityProfile CompanyProfileEntity
    {
        get
        {
            if (rdbCPF.Checked)
            {
                return null;
            }
            else
            {
                //if (Page.ViewState["LegalEntityProfileId"] != null)
                //{
                //    ucCompanyProfile.CompanyProfileEntity.LegalEntityProfileId = Convert.ToInt32(Page.ViewState["LegalEntityProfileId"]);
                //}

                return ucCompanyProfile.CompanyProfileEntity;
            }
        }
        set
        {
            ucCompanyProfile.CompanyProfileEntity = value;
            if (value != null)
            {
                //Page.ViewState["LegalEntityProfileId"] = value.LegalEntityProfileId;
                rdbCNPJ.Checked = true;
                rdbCPF.Checked = false;

                //
                // Disable the radio buttons
                //
                rdbCPF.Enabled = false;
                rdbCNPJ.Enabled = false;

                ucCompanyProfile.Visible = true;
                ucProfile.Visible = false;
            }
        }
    }

    [Bindable(true, BindingDirection.TwoWay)]
    [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
    public string ValidationGroup { get; set; }

    protected void rdbCNPJ_CheckedChanged(object sender, EventArgs e)
    {
        ucCompanyProfile.Visible = true;
        ucProfile.Visible = false;
    }

    protected void rdbCPF_CheckedChanged(object sender, EventArgs e)
    {
        ucCompanyProfile.Visible = false;
        ucProfile.Visible = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (rdbCNPJ.Checked)
        {
            ucCompanyProfile.ValidationGroup = ValidationGroup;
            ucCompanyProfile.Visible = true;
        }
        else
        {
            ucProfile.ValidationGroup = ValidationGroup;
            ucProfile.Visible = true;
        }
    }
}