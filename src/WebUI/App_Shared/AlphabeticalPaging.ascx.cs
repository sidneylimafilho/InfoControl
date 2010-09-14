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
using System.ComponentModel;
//using Vivina.Erp.BusinessRules;
//using Vivina.Erp.DataClasses;
using Vivina.Erp.SystemFramework;

using InfoControl.Web.UI;


using InfoControl;


public partial class Commons_AlphabeticalPaging : DataUserControl
{
    public String Letter
    {
        get
        {
            return (string)ViewState["_Letter"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkLetter_Click(object sender, EventArgs e)
    {
        if ((sender as LinkButton).Text.ToUpper() == "TODOS")
            ViewState["_Letter"] = String.Empty;
        else
            ViewState["_Letter"] = (sender as LinkButton).Text.ToUpper();

        OnSelectedLetter(this, new SelectedLetterEventArgs() { SelectedLetter = (string)ViewState["_Letter"] });
    }

    protected void OnSelectedLetter(Object sender, SelectedLetterEventArgs e)
    {
        if (SelectedLetter != null)
            SelectedLetter(sender, e);
    }

    public event EventHandler<SelectedLetterEventArgs> SelectedLetter;

   
}

public class SelectedLetterEventArgs : EventArgs
{
    public String SelectedLetter { get; set; }
}