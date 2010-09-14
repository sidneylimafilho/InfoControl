using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoControl;
using InfoControl.Data;
using InfoControl;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;




public partial class InfoControl_Notepad : Vivina.Erp.SystemFramework.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtDescription.Focus();
        if (!IsPostBack)
            txtDescription.Content = Convert.ToString(User.Personalization["Notepad"]);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        User.Personalization["Notepad"] = txtDescription.Content;
    }

}
