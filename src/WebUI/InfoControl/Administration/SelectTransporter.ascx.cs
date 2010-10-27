using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Vivina.Erp.SystemFramework;


using Vivina.Erp.DataClasses;
using Vivina.Erp.BusinessRules;
using InfoControl;

public partial class App_Shared_SelectTransporter : Vivina.Erp.SystemFramework.UserControlBase
{
    TransporterManager transporterManager;
    Transporter transporter;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }


    protected void OnSelectingTransporter(object sender, SelectingTransporterEventArgs e)
    {
        if (SelectingTransporter != null)
            SelectingTransporter(sender, e);
    }
    protected void OnSelectedTransporter(object sender, SelectedTransporterEventArgs e)
    {
        if (SelectedTransporter != null)
            SelectedTransporter(sender, e);
    }

    //events
    public event EventHandler<SelectingTransporterEventArgs> SelectingTransporter;
    public event EventHandler<SelectedTransporterEventArgs> SelectedTransporter;

    public void ShowTransporter(Transporter transporter)
    {
        if (transporter != null)
        {
            lblTransporterAddress.Text = transporter.LegalEntityProfile.Address.Name
                + ", " + transporter.LegalEntityProfile.AddressNumber
                + " " + transporter.LegalEntityProfile.AddressComp;


            lblTransporterLocalization.Text = transporter.LegalEntityProfile.Address.City
                      + " - " + transporter.LegalEntityProfile.Address.Neighborhood
                      + ", " + transporter.LegalEntityProfile.Address.StateId;

            lblPostalCode.Text = "CEP: " + transporter.LegalEntityProfile.Address.PostalCode;
            lblTransporterPhone.Text = "Tel: " + transporter.LegalEntityProfile.Phone.Replace("(__)____-____", "");
            lblCNPJ.Text = transporter.LegalEntityProfile.CNPJ;
            lnkTransporterName.Text = transporter.LegalEntityProfile.CompanyName;
            String encripted = transporter.TransporterId.EncryptToHex();
            lnkTransporterName.OnClientClick = "top.tb_show('Cadastro de Transportadoras','Administration/Transporter.aspx?TransporterId=" + encripted + "');return;";
            pnlTransporter.Visible = true;
        }
        txtTransporter.Text = "";
        pnlTransporterSearch.Style.Add(HtmlTextWriterStyle.Display, "none");
        OnSelectedTransporter(this, new SelectedTransporterEventArgs() { Transporter = transporter });
    }
    protected void txtTransporter_TextChanged(object sender, EventArgs e)
    {
        OnSelectingTransporter(this, new SelectingTransporterEventArgs() { TransporterName = txtTransporter.Text });
        ProfileManager profileManager;
        if (txtTransporter.Text.Contains('|'))
        {
            profileManager = new ProfileManager(this);
            transporterManager = new TransporterManager(this);

            string[] identifications = txtTransporter.Text.Split('|');
            string identification = identifications[0].ToString().Trim();

            LegalEntityProfile legalEntityProfile = profileManager.GetLegalEntityProfile(identification);
            if (legalEntityProfile != null)
                transporter = transporterManager.GetTransporterByLegalEntityProfile(Page.Company.CompanyId, legalEntityProfile.LegalEntityProfileId);

            ShowTransporter(transporter);
        }
    }

}

public class SelectingTransporterEventArgs : EventArgs
{
    public string TransporterName { get; set; }
}
public class SelectedTransporterEventArgs : EventArgs
{
    public Transporter Transporter { get; set; }
}