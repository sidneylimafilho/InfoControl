using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivina.Erp.SystemFramework;
using InfoControl.Web.Services;


public partial class Commons_ToolTip : InfoControl.Web.UI.DataUserControl
{
    private string _indication;
    [Bindable(true)]
    public string Indication
    {
        get { return _indication; }
        set { _indication = value; }
    }
    private string _top;
    [Bindable(true)]
    public string Top
    {
        get { return _top; }
        set { _top = value; }
    }


    private string _left;
    [Bindable(true)]
    public string Left
    {
        get { return _left; }
        set { _left = value; }
    }

    private string _right;
    [Bindable(true)]
    public string Right
    {
        get { return _right; }
        set { _right = value; }
    }

    private string _title;
    [Bindable(true)]
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    private string _message;
    [Bindable(true)]
    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }

    private int _timer = -10000;
    [Bindable(true)]
    public int Timer
    {
        get { return _timer; }
        set { _timer = value; }
    }

    private bool _persistent = false;
    [Bindable(true)]
    public bool Persistent
    {
        get { return _persistent; }
        set { _persistent = value; }
    }

    [Bindable(true)]
    public override bool Visible
    {
        get { return base.Visible; }
        set { base.Visible = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToBoolean(Page.Customization[tip.ClientID]))
        {
            Visible = false;
            return;
        }

        Page.ClientScript.RegisterClientScriptInclude("Commons_ToolTip", ResolveClientUrl("ToolTip.ascx.js"));

        btnPersist.Style["display"] = Persistent ? "none" : "block";

        if (Timer > 0)
        {
            Page.ClientScript.RegisterStartupScript(
                typeof(Commons_ToolTip),
                "Commons_ToolTip_Timer",
                "setTimeout(\"$get('" + btnFechar.ClientID + "').click()\", " + Timer + ");" +
                "var elapsed=" + Timer / 1000 + ";" +
                "var changeLabel = function(){if(elapsed>0){$get('" + lblTimer.ClientID + "').innerText='Esta informação fechará em '+ elapsed +' segundos!';elapsed--; setTimeout(\"changeLabel()\", 1000);}};" +
                "changeLabel();", true);
            lblTimer.Style["display"] = "";
            btnFechar.Style["display"] = "none";
            btnPersist.Style["display"] = "none";
        }
        else
        {
            lblTimer.Style["display"] = "none";
            btnFechar.Style["display"] = "";
        }

        if (!String.IsNullOrEmpty(Top)) tip.Style["top"] = Top;
        if (!String.IsNullOrEmpty(Left)) tip.Style["left"] = Left;
        if (!String.IsNullOrEmpty(Right)) tip.Style["right"] = Right;

        tip.Attributes["class"] = "ToolTip_" + (String.IsNullOrEmpty(_indication) ? "top" : _indication).ToLower();

        //
        // indicate the page that tool tip is load
        //
        tip.Attributes["page"] = Page.GetType().FullName;

        lblTitle.Text = Title;
        lblMessage.Text = Message;

    }
}

public class ToolTipController : DataControllerBase
{
    [JavaScriptSerializer]    
    public void SetToolTipClosed(string page, string toolTipId)
    {
        if (User.Personalization[page] == null)
            User.Personalization[page] = new Hashtable();

        (User.Personalization[page] as Hashtable)[toolTipId] = true;
    }
}