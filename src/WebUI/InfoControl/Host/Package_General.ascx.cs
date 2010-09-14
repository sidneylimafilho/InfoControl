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
using Vivina.Framework.Web.UI;

using Vivina.InfoControl.DataClasses;

public partial class Host_Package_General : Vivina.Framework.Web.UI.DataUserControl
{
    private string FormatDouble(TextBox Double)
    {
        //
        // This method fixes the bug that happens on .NET with 2 diferents cultures
        // The data was inconsistance, 'cause the en-US culture uses the "." (dot) to separate decimal
        // values, when the pt-BR culture uses the "," (coma).
        // The replace for the "_" (underline) character is to fix the mask extender problem. If the focus
        // are in the control that have a mask, that control send a "_" to the database.
        //
        Double.Text = Double.Text.Replace("_", "0");
        Double.Text = Double.Text.Replace(".", "");
        Double.Text = Double.Text.Replace(",", ".");
        return Double.Text;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Page.ViewState["PackageId"] != null)
                frmPackage.ChangeMode(FormViewMode.Edit);

        }
    }
    protected void odsPackage_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["PackageId"] = Convert.ToInt16(Page.ViewState["PackageId"]);
    }
    protected void frmPackage_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert" || e.CommandName == "Update")
        {
            CurrencyField ucCurrFieldPrice = frmPackage.FindControl("ucCurrFieldPrice") as CurrencyField;
            CurrencyField ucCurrFieldValueByHour = frmPackage.FindControl("ucCurrFieldValueByHour") as CurrencyField;
            CurrencyField ucCurrFieldSetupFee = frmPackage.FindControl("ucCurrFieldSetupFee") as CurrencyField;
            CurrencyField ucCurrFieldProductPrice = frmPackage.FindControl("ucCurrFieldProductPrice") as CurrencyField;

            if (!ucCurrFieldPrice.CurrencyValue.HasValue)
                ucCurrFieldPrice.CurrencyValue = 0;

            if (!ucCurrFieldValueByHour.CurrencyValue.HasValue)
                ucCurrFieldValueByHour.CurrencyValue = 0;

            if (!ucCurrFieldSetupFee.CurrencyValue.HasValue)
                ucCurrFieldSetupFee.CurrencyValue = 0;

            if (!ucCurrFieldProductPrice.CurrencyValue.HasValue)
                ucCurrFieldProductPrice.CurrencyValue = 0;

        }
        else if (e.CommandName == "Cancel")
            Response.Redirect("Packages.aspx");
    }
    protected void odsPackage_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Package pak = (Package)e.InputParameters["entity"];

        pak.ModifiedDate = DateTime.Now;
        //pak.ValuePerHour = ucCurrField
    }
    protected void odsPackage_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Package pak = (Package)e.InputParameters["entity"];
        pak.ModifiedDate = DateTime.Now;
        if ((frmPackage.FindControl("chkIsActive") as CheckBox).Checked == true)
        {
            pak.IsActive = true;
        }
        else
        {
            pak.IsActive = false;
        }
    }
    protected void odsPackage_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        //Se houver qualquer problema durante a execução das querys, será relançada uma exceção, forçando assim
        //uma tela amarela de erro na tela, pra qualquer erro, evitando comportamentos aleatórios e 
        //mal compreendidos
        //
        if (e.Exception != null)
        {
            throw e.Exception;
        }
        Context.Items["PackageId"] = e.ReturnValue;
        Context.Items["sucess"] = 1;

        Server.Transfer("Package.aspx");

    }
    protected void odsPackage_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //
        //Se houver qualquer problema durante a execução das querys, será relançada uma exceção, forçando assim
        //uma tela amarela de erro na tela, pra qualquer erro, evitando comportamentos aleatórios e 
        //mal compreendidos
        //
        if (e.Exception != null)
        {
            throw e.Exception;
        }
        Response.Redirect("Packages.aspx");
    }
    protected void frmPackage_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        e.Values["Price"] = Convert.ToDecimal("0" + e.Values["Price"].ToString().Replace("_", ""));
        
    }
}
